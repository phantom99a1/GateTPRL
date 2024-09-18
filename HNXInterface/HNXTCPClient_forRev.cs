using CommonLib;
using HNX.FIXMessage;
using LocalMemory;
using StorageProcess;

namespace HNXInterface
{
	public partial class HNXTCPClient : iHNXClient, IDisposable
    {
        private int LastResendSeq = 0;
        private int LastBeginResendSeq = 0;
        private int LastEndResendSeq = 0;
        private Thread LastThrdResend;
        private ProcessRevHNX c_ProcessRevHNX;

        public void ProcessRevMessage(string p_strMsg, ref string pGetMsgType)
        {
            //xử lý các mesage nhận về theo nguyên tắc
            // Các message Admin sẽ xử lý luôn
            //Các message Nghiệp vụ sẽ forward trả về cho core
            FIXMessageBase fMSgBase = c_MsgFactoryFix.Parse(p_strMsg);
            if (fMSgBase == null)
            {
                Logger.HNXTcpLog.Warn("Receive not FIX Message {0}", p_strMsg);
            }
            else
            {
                pGetMsgType = fMSgBase.GetMsgType;
                //
                if (c_MsgFactoryFix.IsAdminitrativeMessage(fMSgBase))
                {
                    ProcessSessionMsg(fMSgBase);
                }
                else
                {
                    fMSgBase.TimeInit = DateTime.Now.Ticks;
                    ProcessBusinessMessage(fMSgBase);
                }
            }
        }

        public void ProcessSessionMsg(FIXMessageBase fMsgBase)
        {
            if (fMsgBase.MsgSeqNum > GateSeqInfo.LastCliProcessSeq)
            {
                // Gửi Resend request
                Send_ResendRequest(GateSeqInfo.LastCliProcessSeq + 1, fMsgBase.MsgSeqNum);
            }
            else if (fMsgBase.MsgSeqNum < GateSeqInfo.LastCliProcessSeq)
            {
                Logger.HNXTcpLog.Warn("Something went wrong, HNX send sequence {0} < client's LastProcessSequence {1} at Message {2}", fMsgBase.MsgSeqNum, GateSeqInfo.LastCliProcessSeq, fMsgBase.GetMessageRaw);
                return;
            }
            switch (fMsgBase.GetMsgType)
            {
                case MessageType.Logon:
                    ProcessLogon(fMsgBase);
                    break;

                case MessageType.Heartbeat:
                    break;

                case MessageType.Reject:
                    //2024.09.05 add msg reject on memory
                    CommonFunc.FuncAddMessageRejectForITMonitor(fMsgBase);
                    //End
                    MessageReject messageReject = (MessageReject)fMsgBase;
                    ProcessReject(messageReject);
                    // BacND: bổ sung thêm ghi vào DB sau khi gửi sở và save file xong
                    SharedStorageProcess.c_DataStorageProcess.EnqueueData(fMsgBase, Data_SoR.Recei);
                    break;

                case MessageType.ResendRequest:
                    ProcessResendRequest((MessageResendRequest)fMsgBase);
                    break;

                case MessageType.SequenceReset:
                    ProcessSequenceReset(fMsgBase);
                    // BacND: bổ sung thêm ghi vào DB sau khi gửi sở và save file xong
                    SharedStorageProcess.c_DataStorageProcess.EnqueueData(fMsgBase, Data_SoR.Recei);
                    break;

                case MessageType.Logout:
                    ProcessLogout((MessageLogout)fMsgBase);
                    // BacND: bổ sung thêm ghi vào DB sau khi gửi sở và save file xong
                    SharedStorageProcess.c_DataStorageProcess.EnqueueData(fMsgBase, Data_SoR.Recei);
                    break;                
            }
            GateSeqInfo.Set_LastCliProcess(fMsgBase.MsgSeqNum);
            GateSeqInfo.Set_LastSerProcess(fMsgBase.LastMsgSeqNumProcessed);
            GateSeqInfo.Set_SerSeq(fMsgBase.MsgSeqNum);
        }

        public void ProcessBusinessMessage(FIXMessageBase fMsgBase)
        {
            if (fMsgBase.MsgSeqNum > GateSeqInfo.LastCliProcessSeq + 1)
            {
                //Gửi Resend request
                Send_ResendRequest(GateSeqInfo.LastCliProcessSeq + 1, fMsgBase.MsgSeqNum);
            }
            else if (fMsgBase.MsgSeqNum < GateSeqInfo.LastCliProcessSeq + 1)
            {
                if (fMsgBase.PossDupFlag == true)
                {
                    // tức là message gửi lại, xử lý bình thường
                }
                else
                {
                    Logger.HNXTcpLog.Warn("Something went wrong, Sequence exchange send {0} smaller than seqeunce processed {1} and Possdupflag is {2} at Message {3}",
                        fMsgBase.MsgSeqNum, GateSeqInfo.LastCliProcessSeq, fMsgBase.PossDupFlag, fMsgBase.GetMessageRaw);
                    return;
                }
            }
            //Xử lý message ở đây
            GateSeqInfo.Set_LastSerProcess(fMsgBase.LastMsgSeqNumProcessed);
            GateSeqInfo.Set_SerSeq(fMsgBase.MsgSeqNum);
            c_ProcessRevHNX.ProcessHNXMessage(fMsgBase);
            GateSeqInfo.Set_LastCliProcess(fMsgBase.MsgSeqNum);
        }

        public void ProcessLogon(FIXMessageBase p_FIXMessageBase)
        {
            try
            {
                if (__ClientStatus == enumClientStatus.DATA_TRANSFER) return;

                MessageLogon _MessageLogon = (MessageLogon)p_FIXMessageBase;
                int _ServerSuccessSeq = _MessageLogon.LastMsgSeqNumProcessed; //đây là seq mà server đã nhận và xử lý thành công khi bắt tay lại
                ShareMemoryData.c_LoginStatus = _MessageLogon.Text;
                //
                // BacND: bổ sung thêm ghi vào DB sau khi gửi sở và save file xong
                SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_FIXMessageBase, Data_SoR.Recei);
                //
                CommonLib.Logger.log.Info("LOGIN SUCCESS");

                // xử lý đồng bộ seq ở đây
                if (_ServerSuccessSeq > GateSeqInfo.CliSeq)
                {
                    //không thể xảy ra, vì như thế sẽ nhận đc message 4, không thể nhận được login
                    StopCurrentConnected($"Sequence server nhan duoc lon hon Sequence cua Gate, MessageRaw = {p_FIXMessageBase.GetMessageRaw}");
                    return;
                }
                else if (_ServerSuccessSeq == GateSeqInfo.CliSeq)
                {
                    Logger.HNXTcpLog.Info("Gate-HNX|Login Success");
                    IsRequest = false;

                    //Đến đây là hết rule. lúc này mới chấp nhận là DATA_TRANSFER
                    __ClientStatus = enumClientStatus.DATA_TRANSFER;
                }
                else if (_ServerSuccessSeq < GateSeqInfo.CliSeq)
                {
                    Logger.HNXTcpLog.Warn($"GATE - HNX| Sequence Server nhận được nhỏ hơn Sequence Gate gửi đi: SeqGateSend={GateSeqInfo.CliSeq} SeqServerReceived = {_ServerSuccessSeq} MessageRaw = {p_FIXMessageBase.GetMessageRaw}");
                    //Như này chắc chắn sẽ nhận message resend. đợi resend xong mới gửi cho vào DATA_TRANSFER
                }
            }
            catch (Exception ex)
            {
                Logger.HNXTcpLog.Error(ex, "GATE - HNX| Exception: {0} Message {1}", ex.Message, p_FIXMessageBase.GetMessageRaw);
            }
        }

        public void ProcessReject(MessageReject fMsgReject)
        {
            if (__ClientStatus != enumClientStatus.DATA_TRANSFER)
            {
                //Chỉ có thể là do login sai
                Logger.HNXTcpLog.Warn("Nhan message reject {0}", fMsgReject.Text);
                StopCurrentConnected(fMsgReject.Text);
            }
            else
            {
                return;
            }
        }        

        public void ProcessResendRequest(MessageResendRequest fMsgResendRequest)
        {
            __ClientStatus = enumClientStatus.PROCESS_RESEND;
            //fMsgResendRequest.BeginSeqNo > LastBeginResendSeq -> đây là yêu cầu mới
            if (fMsgResendRequest.BeginSeqNo > LastBeginResendSeq)
            {
                Logger.HNXTcpLog.Warn("Nhan Message Resend Request co BeginSeqNo {0} > LastBeginResendSeq {1} -> Yeu cau moi", fMsgResendRequest.BeginSeqNo, LastBeginResendSeq);
                LastBeginResendSeq = fMsgResendRequest.BeginSeqNo;
                LastResendSeq = GateSeqInfo.CliSeq;
                LastEndResendSeq = fMsgResendRequest.EndSeqNo;
                ProcessResend(fMsgResendRequest.BeginSeqNo, GateSeqInfo.CliSeq);
            }
            else if (fMsgResendRequest.BeginSeqNo == LastBeginResendSeq)
            {
                //fMsgResendRequest.EndSeqNo > LastEndResendSeq -> có thể vẫn là yêu cầu cũ do 2 bên đang đuổi nhau
                if (fMsgResendRequest.EndSeqNo > LastEndResendSeq)
                {
                    //Không làm gì. chỉ update lại LastEndResendSeq
                    LastEndResendSeq = fMsgResendRequest.EndSeqNo;
                }
                else if (fMsgResendRequest.EndSeqNo < LastEndResendSeq)
                {
                    //fMsgResendRequest.EndSeqNo < LastEndResendSeq -> toàn bộ message đã bị lost, coi như là yêu cầu mới
                    Logger.HNXTcpLog.Warn("Nhan Message Resend Request co BeginSeqNo {0} < LastBeginResendSeq {1} -> toan bo message resend da bi lost", fMsgResendRequest.BeginSeqNo, LastBeginResendSeq);
                    LastBeginResendSeq = fMsgResendRequest.BeginSeqNo;
                    LastEndResendSeq = GateSeqInfo.CliSeq;
                    ProcessResend(fMsgResendRequest.BeginSeqNo, GateSeqInfo.CliSeq);
                }
            }
        }

        public void ProcessResend(int BeginSeqNo, int EndSeqNo)
        {
            Thread thrdResend = new Thread(() => { ThrdResend(BeginSeqNo, EndSeqNo); });
            thrdResend.IsBackground = true;
            thrdResend.Start();
        }

        public void ThrdResend(int BeginSeqNo, int EndSeqNo)
        {
            if (LastThrdResend != null)
            {
                Thread WaitThrd = LastThrdResend;
                LastThrdResend = Thread.CurrentThread;
                WaitThrd.Join();
            }
            else
            {
                LastThrdResend = Thread.CurrentThread;
            }
            List<FIXMessageBase> ListResend = c_BackupData.GetMsgfromBackup(BeginSeqNo, EndSeqNo);
            for (int i = 0; i < ListResend.Count; i++)
            {
                FIXMessageBase fixMessageBase = ListResend[i];
                bool result = ForResend(c_MsgFactoryFix.Build(fixMessageBase));

                //if (result)
                //{
                //    // BacND: bổ sung thêm ghi vào DB sau khi gửi sở và save file xong
                //    SharedStorageProcess.c_DataStorageProcess.EnqueueData(fixMessageBase, Data_SoR.Recei);
                //}

                Logger.HNXTcpLog.Warn("Resend Message seq {0}", ListResend[i].MsgSeqNum);
                if (ListResend[i].MsgSeqNum > GateSeqInfo.LastSerProcessSeq + ConfigData.SafeWindowSize)
                {
                    SendTestquest();
                    Thread.Sleep(1000);
                }
                Thread.Sleep(1);
            }
            if (LastResendSeq >= GateSeqInfo.CliSeq)
            {
                //Ngủ 1 giây cho hệ thống ổn định. rồi mới mở mode data transfer
                Thread.Sleep(1000);
                __ClientStatus = enumClientStatus.DATA_TRANSFER;
            }
        }

        private bool ForResend(string s)
        {
            //Chỉ là Resend thôi, nên ko có xử lý Sequence gì ở đây
            try
            {
                c_CurrentConnected.WriteString(s);
                ShareMemoryData.c_FileStore.StoreResendMsg("", s, GateSeqInfo.CliSeq, GateSeqInfo.LastCliProcessSeq, GateSeqInfo.SerSeq, GateSeqInfo.LastSerProcessSeq);
                //
                return true;
            }
            catch (Exception ex)
            {
                Logger.HNXTcpLog.Error(ex, "Send Error {0}", s);
                return false;
            }
        }

        public void ProcessSequenceReset(FIXMessageBase p_fixMsgBase)
        {
            MessageSequenceReset SequenceReset = (MessageSequenceReset)p_fixMsgBase;
            string _msg = string.Format("Rev SequenceReset by SeqSend ={0}, SeqSerRev={1}, NewSeqNo = {2}", GateSeqInfo.SerSeq, SequenceReset.LastMsgSeqNumProcessed, SequenceReset.NewSeqNo);
            //DungNT
            c_IsAutoReconnect = false;//Tạm thời khóa, ko cho connect lại
            Logger.log.Error("Disconnect by Receive SequenceReset:" + _msg);
            StopCurrentConnected("Receive Reset Sequence");
            GateSeqInfo.Set_LastSerProcess(SequenceReset.LastMsgSeqNumProcessed);
            GateSeqInfo.Set_CliSeq(SequenceReset.LastMsgSeqNumProcessed);
            GateSeqInfo.Set_SerSeq(SequenceReset.MsgSeqNum);
            GateSeqInfo.Set_MaxSeqProcess(SequenceReset.MsgSeqNum);
            GateSeqInfo.Set_LastCliProcess(SequenceReset.MsgSeqNum);
            c_IsAutoReconnect = true;// xử lý thì mở ra.
        }

        public void ProcessLogout(MessageLogout Logout)
        {
            Logger.log.Info("Recv Logout");
            c_IsAutoReconnect = false;
            __ClientStatus = enumClientStatus.RECEIVE_LOGOUT;
            StopCurrentConnected("Recv Logout");
        }
    }
}