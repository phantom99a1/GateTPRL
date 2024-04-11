using CommonLib;
using Disruptor;
using Disruptor.Dsl;
using HNX.FIXMessage;
using ManagedLayer;
using ObjectInfo;
using static HNX.FIXMessage.MessageExecOrderRepos;
using static HNX.FIXMessage.MessageReposBCGDReport;

namespace StorageProcess
{
    public class DataStorageProcess : IEventHandler<ValueSaveEntry>
    {
        // Size of the ring buffer, must be power of 2.
        // private const int bufferSize = 32768;

        // Create the disruptor
        private Disruptor<ValueSaveEntry> _disruptor;

        private RingBuffer<ValueSaveEntry> _ringBuffer;

        internal DataStorageProcess()
        {
            //để _disruptor là  ProducerType.Multi vì chỉ có 1 tiến trình từ  gọi vào để save dữ liệu BusySpinWaitStrategy YieldingWaitStrategy
            _disruptor = new Disruptor<ValueSaveEntry>(() => new ValueSaveEntry(), ConfigData.DefaultBufferSize, TaskScheduler.Default, ProducerType.Multi, new BlockingWaitStrategy());

            _disruptor.HandleEventsWith(this);

            // Start the disruptor (start the consumer threads)
            _disruptor.Start();

            _ringBuffer = _disruptor.RingBuffer;
        }

        public void EnqueueData(FIXMessageBase fMsgBase, string p_Data_SoR)
        {
            long sequence = _ringBuffer.Next();
            Logger.log.Info($"DataStorageProcess -> EnqueueData |  sequence = {sequence}, fMsgBase_MsgType={fMsgBase.GetMsgType}, TypeIsSendOrRecei={p_Data_SoR}, ringBufferRemain={_ringBuffer.GetRemainingCapacity()}");
            // (1) Claim the next sequence
            try
            {
                // (2) Get and configure the event for the sequence
                _ringBuffer[sequence].fixMessageBaseData = fMsgBase;
                _ringBuffer[sequence].typeMsgIsSendOrRecei = p_Data_SoR;
            }
            finally
            {
                // (3) Publish the event
                _ringBuffer.Publish(sequence);
            }
        }

        public void OnEvent(ValueSaveEntry p_ValueSaveEntry, long sequence, bool endOfBatch)
        {
            try
            {
                FIXMessageBase fixMessageBase = p_ValueSaveEntry.fixMessageBaseData;
                //
                Logger.log.Info($"DataStorageProcess -> OnEvent |  sequence = {sequence}, fMsgBase_MsgType={fixMessageBase.GetMsgType}");

                switch (fixMessageBase.GetMsgType)
                {
                    // 1. Bảng MSG_TPRL_INFO (Msg có tag 35 = A và 35 = 5)
                    case MessageType.Logon:
                    case MessageType.Logout:
                        ProcessSaveMsgType_35_A_35_S(sequence, fixMessageBase, p_ValueSaveEntry.typeMsgIsSendOrRecei);
                        break;

                    // 2.	Bảng MSG_TPRL_REQUEST (Msg có tag 35 = 2, 35 = 4)
                    case MessageType.ResendRequest:
                    case MessageType.SequenceReset:
                        ProcessSaveMsgType_35_2_35_4(sequence, fixMessageBase, p_ValueSaveEntry.typeMsgIsSendOrRecei);
                        break;

                    // 3. Bảng MSG_TPRL_REJECT (Msg có tag 35 = 3)
                    case MessageType.Reject:
                        ProcessSaveMsgType_35_3(sequence, fixMessageBase, p_ValueSaveEntry.typeMsgIsSendOrRecei);
                        break;

                    // 4. Bảng MSG_TPRL_SESION (Msg có tag 35 = g và 35 = h)
                    case MessageType.TradingSessionStatusRequest:
                    case MessageType.TradingSessionStatus:
                        ProcessSaveMsgType_35_h_35_g(sequence, fixMessageBase, p_ValueSaveEntry.typeMsgIsSendOrRecei);
                        break;

                    // 5.	Bảng msg_TPRL_SECURITIES (Msg có tag 35 = f và 35 = e)
                    case MessageType.SecurityStatus:
                    case MessageType.SecurityStatusRequest:
                        ProcessSaveMsgType_35_f_35_e(sequence, fixMessageBase, p_ValueSaveEntry.typeMsgIsSendOrRecei);
                        break;

                    // 6.	Bảng MSG_TPRL_ORDER (Msg có tag 35 = D, 35 = G, 35=F)
                    case MessageType.NewOrder:
                    case MessageType.ReplaceOrder:
                    case MessageType.CancelOrder:
                        ProcessSaveMsgType_35_D_35_G_35_F(sequence, fixMessageBase, p_ValueSaveEntry.typeMsgIsSendOrRecei);
                        break;

                    // 7.	Bảng MSG_TPRL_OUTRIGHT (Msg có tag 35 = AI, AJ, Z, R, S, s, t, u)
                    case MessageType.QuoteStatusReport: // 35=AI
                    case MessageType.QuoteResponse: // 35=AJ
                    case MessageType.QuoteCancel: // 35=Z
                    case MessageType.QuoteRequest:// 35=R
                    case MessageType.Quote:// 35=S
                    case MessageType.NewOrderCross:// 35=s
                    case MessageType.CrossOrderCancelReplaceRequest:// 35=t
                    case MessageType.CrossOrderCancelRequest:// 35=u
                        ProcessSaveMsgType_35_AI_AJ_Z_R_S_s_t_u(sequence, fixMessageBase, p_ValueSaveEntry.typeMsgIsSendOrRecei);
                        break;

                    // 8. Bảng MSG_TPRL_REPO (Msg có tag 35 = EE, N01, N02, N03, N04, N05, MA, ME, MC, MR)
                    case MessageType.ExecOrderRepos: // EE
                    case MessageType.ReposInquiry: // N01
                    case MessageType.ReposInquiryReport: // N02
                    case MessageType.ReposFirm: // N03
                    case MessageType.ReposFirmReport: // N04
                    case MessageType.ReposFirmAccept: // N05
                    case MessageType.ReposBCGD: // MA
                    case MessageType.ReposBCGDModify: // ME
                    case MessageType.ReposBCGDCancel: // MC
                    case MessageType.ReposBCGDReport: // MR
                        ProcessSaveMsgType_35_EE_N01_N02_N03_N04_N05_MA_ME_MC_MR(sequence, fixMessageBase, p_ValueSaveEntry.typeMsgIsSendOrRecei);
                        break;

                    // 10. Bảng msg_TPRL_HNX_CONFIRM (Msg có tag 35 = 8)
                    case MessageType.ExecutionReport: // MR
                        ProcessSaveMsgType_35_8(sequence, fixMessageBase, p_ValueSaveEntry.typeMsgIsSendOrRecei);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error process OnEvent in DataStorageProcess with sequence= {sequence}, Exception: {ex?.ToString()}");
                Thread.Sleep(1000); //tránh nhẩy vào catch thì nó sẽ chạy liên tục
            }
        }

        #region Xử lý: lưu vào DB

        private void ProcessSaveMsgType_35_A_35_S(long sequence, FIXMessageBase fixMessageBase, string p_SendOrRecei)
        {
            try
            {
                Logger.log.Info($"Start process ProcessSaveMsgType_35_A_35_S with sequence= {sequence}, MsgType(35)={fixMessageBase.GetMsgType}");

                int _return = 0;
                //while (true)
                //{
                Msg_Tprl_Info_Info objSaveData = new Msg_Tprl_Info_Info();

                if (fixMessageBase.GetMsgType == MessageType.Logon) // 35= A
                {
                    MessageLogon msgData = (MessageLogon)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed; // 369
                    objSaveData.Encryptmethod = msgData.EncryptMethod.ToString(); // 98
                    objSaveData.Heartbtint = msgData.HeartBtInt.ToString(); // 108
                    objSaveData.Username = msgData.Username;
                    objSaveData.Pwd = msgData.Password;
                    objSaveData.Text = msgData.Text;
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                }
                else if (fixMessageBase.GetMsgType == MessageType.Logout)// 35= 5
                {
                    MessageLogout msgData = (MessageLogout)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed; // 369
                    objSaveData.Encryptmethod = ""; // 98
                    objSaveData.Heartbtint = ""; // 108
                    objSaveData.Username = ""; // logout khong co
                    objSaveData.Pwd = ""; // logout khong co
                    objSaveData.Text = msgData.Text;
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                }

                _return = Msg_tprl_infoBL.Insert(objSaveData);
                if (_return > 0)
                {
                    Logger.log.Info($"ProcessSaveMsgType | MsgSeqNum: {objSaveData.Msgseqnum}, MsgType: {objSaveData.Msgtype},  -->>> Save to DB sucess");
                    //break;
                }
                else
                {
                    Logger.log.Error($"Error process ProcessSaveMsgType with MsgSeqNum: {objSaveData.Msgseqnum}, MsgType: {objSaveData.Msgtype}, Error [{_return}] can't save to DB; Object={System.Text.Json.JsonSerializer.Serialize(objSaveData)} ");
                    //Thread.Sleep(ConfigData.TimeDelaySaveDB);
                }
                //}
                Logger.log.Info($"End process ProcessSaveMsgType_35_A_35_S with sequence= {sequence}, MsgType(35)={fixMessageBase.GetMsgType}");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error process ProcessSaveMsgType_35_A_35_S with sequence= {sequence}, Exception: {ex?.ToString()}");
            }
        }

        private void ProcessSaveMsgType_35_2_35_4(long sequence, FIXMessageBase fixMessageBase, string p_SendOrRecei)
        {
            try
            {
                Logger.log.Info($"Start process ProcessSaveMsgType_35_2_35_4 with sequence= {sequence}, MsgType(35)={fixMessageBase.GetMsgType}");

                int _return = 0;
                //while (true)
                //{
                Msg_Tprl_Request_Info objSaveData = new Msg_Tprl_Request_Info();
                if (fixMessageBase.GetMsgType == MessageType.ResendRequest) // 35=2 Gửi lên sở
                {
                    MessageResendRequest msgData = (MessageResendRequest)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = Utils.ParseInt(fixMessageBase.GetMsgType); // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed; // 369
                    objSaveData.Beginseqno = msgData.BeginSeqNo;
                    objSaveData.Endseqno = msgData.EndSeqNo;
                    objSaveData.Testreqid = 0;
                    objSaveData.Newseqno = 0;
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                }
                else if (fixMessageBase.GetMsgType == MessageType.SequenceReset) // 35=4 Gửi lên sở
                {
                    MessageSequenceReset msgData = (MessageSequenceReset)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = Utils.ParseInt(fixMessageBase.GetMsgType); // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed; // 369
                    objSaveData.Beginseqno = 0;
                    objSaveData.Endseqno = 0;
                    objSaveData.Testreqid = 0;
                    objSaveData.Newseqno = msgData.NewSeqNo;
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                }

                _return = Msg_tprl_requestBL.Insert(objSaveData);
                if (_return > 0)
                {
                    Logger.log.Info($"ProcessSaveMsgType | MsgSeqNum: {objSaveData.Msgseqnum}, MsgType: {objSaveData.Msgtype},  -->>> Save to DB sucess");
                    //break;
                }
                else
                {
                    Logger.log.Error($"Error process ProcessSaveMsgType with MsgSeqNum: {objSaveData.Msgseqnum}, MsgType: {objSaveData.Msgtype}, Error [{_return}] can't save to DB; Object={System.Text.Json.JsonSerializer.Serialize(objSaveData)} ");
                    //Thread.Sleep(ConfigData.TimeDelaySaveDB);
                }
                //}
                Logger.log.Info($"End process ProcessSaveMsgType_35_2_35_4 with sequence= {sequence}, MsgType(35)={fixMessageBase.GetMsgType}");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error process ProcessSaveMsgType_35_2_35_4 with sequence= {sequence}, Exception: {ex?.ToString()}");
            }
        }

        private void ProcessSaveMsgType_35_3(long sequence, FIXMessageBase fixMessageBase, string p_SendOrRecei)
        {
            try
            {
                Logger.log.Info($"Start process ProcessSaveMsgType_35_3 with sequence= {sequence}, MsgType(35)={fixMessageBase.GetMsgType}");
                int _return = 0;
                //while (true)
                //{
                Msg_Tprl_Reject_Info objSaveData = new Msg_Tprl_Reject_Info();
                MessageReject msgData = (MessageReject)fixMessageBase;

                objSaveData.Sor = p_SendOrRecei;
                objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed.ToString(); // 369

                objSaveData.Possdupflag = msgData.PossDupFlag == true ? "Y" : "N";
                objSaveData.Text = msgData.Text;
                objSaveData.Refseqnum = msgData.RefSeqNum;
                objSaveData.Sessionrejectreason = msgData.SessionRejectReason;
                objSaveData.Refmsgtype = msgData.RefMsgType;
                objSaveData.Remark = "";
                //
                objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);

                _return = Msg_tprl_rejectBL.Insert(objSaveData);
                if (_return > 0)
                {
                    Logger.log.Info($"ProcessSaveMsgType | MsgSeqNum: {objSaveData.Msgseqnum}, MsgType: {objSaveData.Msgtype},  -->>> Save to DB sucess");
                    //break;
                }
                else
                {
                    Logger.log.Error($"Error process ProcessSaveMsgType with MsgSeqNum: {objSaveData.Msgseqnum}, MsgType: {objSaveData.Msgtype}, Error [{_return}] can't save to DB; Object={System.Text.Json.JsonSerializer.Serialize(objSaveData)} ");
                    //Thread.Sleep(ConfigData.TimeDelaySaveDB);
                }
                //}
                Logger.log.Info($"End process ProcessSaveMsgType_35_3 with sequence= {sequence}, MsgType(35)={fixMessageBase.GetMsgType}");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error process ProcessSaveMsgType_35_3 with sequence= {sequence}, Exception: {ex?.ToString()}");
            }
        }

        private void ProcessSaveMsgType_35_h_35_g(long sequence, FIXMessageBase fixMessageBase, string p_SendOrRecei)
        {
            try
            {
                Logger.log.Info($"Start process ProcessSaveMsgType_35_h_35_g with sequence= {sequence}, MsgType(35)={fixMessageBase.GetMsgType}");
                int _return = 0;
                //while (true)
                //{
                Msg_Tprl_Sesion_Info objSaveData = new Msg_Tprl_Sesion_Info();
                if (fixMessageBase.GetMsgType == MessageType.TradingSessionStatusRequest) // 35=g Gửi lên sở
                {
                    MessageTradingSessionStatusRequest msgData = (MessageTradingSessionStatusRequest)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Possdupflag = msgData.PossDupFlag == true ? "Y" : "N";
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Text = fixMessageBase.Text;
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed; // 369
                    //
                    objSaveData.Tradsesreqid = msgData.TradSesReqID;
                    objSaveData.Tradingsessionid = "";
                    objSaveData.Tradsesmode = msgData.TradSesMode.ToString();
                    objSaveData.Tradsesstatus = "";
                    objSaveData.Tradsesstarttime = "";
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                }
                else if (fixMessageBase.GetMsgType == MessageType.TradingSessionStatus) // 35=h Nhận từ sở
                {
                    MessageTradingSessionStatus msgData = (MessageTradingSessionStatus)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Possdupflag = msgData.PossDupFlag == true ? "Y" : "N";
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Text = fixMessageBase.Text;
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed; // 369
                    //
                    objSaveData.Tradsesreqid = msgData.TradSesReqID;
                    objSaveData.Tradingsessionid = msgData.TradingSessionID;
                    objSaveData.Tradsesmode = msgData.TradSesMode.ToString();
                    objSaveData.Tradsesstatus = msgData.TradSesStatus;
                    objSaveData.Tradsesstarttime = msgData.TradSesStartTime.ToString(ConfigData.formatDateTime);
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                }

                _return = Msg_tprl_sesionBL.Insert(objSaveData);
                if (_return > 0)
                {
                    Logger.log.Info($"ProcessSaveMsgType | MsgSeqNum: {objSaveData.Msgseqnum}, MsgType: {objSaveData.Msgtype},  -->>> Save to DB sucess");
                    //break;
                }
                else
                {
                    Logger.log.Error($"Error process ProcessSaveMsgType with MsgSeqNum: {objSaveData.Msgseqnum}, MsgType: {objSaveData.Msgtype}, Error [{_return}] can't save to DB; Object={System.Text.Json.JsonSerializer.Serialize(objSaveData)} ");
                    //Thread.Sleep(ConfigData.TimeDelaySaveDB);
                }
                //}
                Logger.log.Info($"End process ProcessSaveMsgType_35_h_35_g with sequence= {sequence}, MsgType(35)={fixMessageBase.GetMsgType}");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error process ProcessSaveMsgType_35_h_35_g with sequence= {sequence}, Exception: {ex?.ToString()}");
            }
        }

        private void ProcessSaveMsgType_35_f_35_e(long sequence, FIXMessageBase fixMessageBase, string p_SendOrRecei)
        {
            try
            {
                Logger.log.Info($"Start process ProcessSaveMsgType_35_f_35_e with sequence= {sequence}, MsgType(35)={fixMessageBase.GetMsgType}");
                int _return = 0;
                //while (true)
                //{
                Msg_Tprl_Securities_Info objSaveData = new Msg_Tprl_Securities_Info();
                if (fixMessageBase.GetMsgType == MessageType.SecurityStatus) // 35=f
                {
                    MessageSecurityStatus msgData = (MessageSecurityStatus)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Possdupflag = msgData.PossDupFlag == true ? "Y" : "N";
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Text = fixMessageBase.Text;
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed; // 369
                    //
                    objSaveData.Tradingsessionsubid = msgData.TradingSessionSubID;
                    objSaveData.Securitystatusreqid = msgData.SecurityStatusReqID;
                    objSaveData.Symbol = msgData.Symbol;
                    objSaveData.Securitytype = msgData.SecurityType;
                    objSaveData.Maturitydate = msgData.MaturityDate.ToString("yyyyMMdd");
                    objSaveData.Issuedate = msgData.IssueDate.ToString("yyyyMMdd");
                    objSaveData.Issuer = msgData.Issuer;
                    objSaveData.Highpx = msgData.HighPx;
                    objSaveData.Lowpx = msgData.LowPx;
                    objSaveData.Highpxout = msgData.HighPxOut;
                    objSaveData.Lowpxout = msgData.LowPxOut;
                    objSaveData.Highpxrep = msgData.HighPxRep;
                    objSaveData.Lowpxrep = msgData.LowPxRep;
                    objSaveData.Lastpx = msgData.LastPx;
                    objSaveData.Securitytradingstatus = msgData.SecurityTradingStatus;
                    objSaveData.Buyvolume = msgData.BuyVolume;
                    objSaveData.Dateno = msgData.DateNo;
                    objSaveData.Totallistingqtty = msgData.TotalListingQtty;
                    objSaveData.Typerule = msgData.TypeRule;
                    objSaveData.Allowed_Trading_Subject = msgData.Allowed_Trading_Subject;
                    objSaveData.Allowed_Trading_Subject_Sell = msgData.Allowed_Trading_Subject_Sell;
                    objSaveData.Subscriptionrequesttype = "";

                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                }
                else if (fixMessageBase.GetMsgType == MessageType.SecurityStatusRequest) // 35=e
                {
                    MessageSecurityStatusRequest msgData = (MessageSecurityStatusRequest)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Possdupflag = msgData.PossDupFlag == true ? "Y" : "N";
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Text = fixMessageBase.Text;
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed; // 369
                    //
                    objSaveData.Tradingsessionsubid = "";
                    objSaveData.Securitystatusreqid = msgData.SecurityStatusReqID;
                    objSaveData.Symbol = msgData.Symbol;
                    objSaveData.Securitytype = "";
                    objSaveData.Maturitydate = "";
                    objSaveData.Issuedate = "";
                    objSaveData.Issuer = "";
                    objSaveData.Highpx = 0;
                    objSaveData.Lowpx = 0;
                    objSaveData.Highpxout = 0;
                    objSaveData.Lowpxout = 0;
                    objSaveData.Highpxrep = 0;
                    objSaveData.Lowpxrep = 0;
                    objSaveData.Lastpx = 0;
                    objSaveData.Securitytradingstatus = 0;
                    objSaveData.Buyvolume = 0;
                    objSaveData.Dateno = "";
                    objSaveData.Totallistingqtty = 0;
                    objSaveData.Typerule = 0;
                    objSaveData.Allowed_Trading_Subject = "";
                    objSaveData.Allowed_Trading_Subject_Sell = "";
                    objSaveData.Subscriptionrequesttype = "";

                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                }

                _return = Msg_tprl_securitiesBL.Insert(objSaveData);
                if (_return > 0)
                {
                    Logger.log.Info($"ProcessSaveMsgType | MsgSeqNum: {objSaveData.Msgseqnum}, MsgType: {objSaveData.Msgtype},  -->>> Save to DB sucess");
                    //break;
                }
                else
                {
                    Logger.log.Error($"Error process ProcessSaveMsgType with MsgSeqNum: {objSaveData.Msgseqnum}, MsgType: {objSaveData.Msgtype}, Error [{_return}] can't save to DB; Object={System.Text.Json.JsonSerializer.Serialize(objSaveData)} ");
                    //Thread.Sleep(ConfigData.TimeDelaySaveDB);
                }
                //}
                Logger.log.Info($"End process ProcessSaveMsgType_35_h_35_g with sequence= {sequence}, MsgType(35)={fixMessageBase.GetMsgType}");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error process ProcessSaveMsgType_35_h_35_g with sequence= {sequence}, Exception: {ex?.ToString()}");
            }
        }

        private void ProcessSaveMsgType_35_D_35_G_35_F(long sequence, FIXMessageBase fixMessageBase, string p_SendOrRecei)
        {
            try
            {
                Logger.log.Info($"Start process ProcessSaveMsgType_35_D_35_G_35_F with sequence= {sequence}, MsgType(35)={fixMessageBase.GetMsgType}");
                int _return = 0;
                //while (true)
                //{
                Msg_Tprl_Order_Info objSaveData = new Msg_Tprl_Order_Info();
                if (fixMessageBase.GetMsgType == MessageType.NewOrder) // 35=D
                {
                    MessageNewOrder msgData = (MessageNewOrder)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = Utils.ParseLongSpan(fixMessageBase.GetTargetCompID); // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum.ToString(); // 34
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Text = fixMessageBase.Text;
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed.ToString(); // 369
                    //
                    objSaveData.Text = msgData.Text;
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Account = msgData.Account;
                    objSaveData.Symbol = msgData.Symbol;
                    objSaveData.Side = msgData.Side.ToString();
                    objSaveData.Orderqty = msgData.OrderQty.ToString();
                    objSaveData.Ordtype = msgData.OrdType;
                    objSaveData.Price2 = msgData.Price2;
                    objSaveData.Price = msgData.Price;
                    objSaveData.Orderqty2 = msgData.OrderQty2;
                    objSaveData.Origclordid = "";
                    objSaveData.Orgorderqty = "";
                    objSaveData.Special_Type = msgData.SpecialType;
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                }
                else if (fixMessageBase.GetMsgType == MessageType.ReplaceOrder) // // 35=G
                {
                    MessageReplaceOrder msgData = (MessageReplaceOrder)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = Utils.ParseLongSpan(fixMessageBase.GetTargetCompID); // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum.ToString(); // 34
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Text = fixMessageBase.Text;
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed.ToString(); // 369
                    //
                    objSaveData.Text = msgData.Text;
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Account = msgData.Account;
                    objSaveData.Symbol = msgData.Symbol;
                    objSaveData.Side = "";
                    objSaveData.Orderqty = msgData.OrderQty.ToString();
                    objSaveData.Ordtype = "";

                    objSaveData.Price2 = msgData.Price2;
                    objSaveData.Price = 0;
                    objSaveData.Orderqty2 = 0;
                    objSaveData.Origclordid = "";
                    objSaveData.Orgorderqty = "";
                    objSaveData.Special_Type = 0;
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                }
                else if (fixMessageBase.GetMsgType == MessageType.CancelOrder) // // 35=F
                {
                    MessageCancelOrder msgData = (MessageCancelOrder)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = Utils.ParseLongSpan(fixMessageBase.GetTargetCompID); // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum.ToString(); // 34
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Text = fixMessageBase.Text;
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed.ToString(); // 369
                    //
                    objSaveData.Text = msgData.Text;
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Account = "";
                    objSaveData.Symbol = msgData.Symbol;
                    objSaveData.Side = "";
                    objSaveData.Orderqty = "";
                    objSaveData.Ordtype = "";

                    objSaveData.Price2 = 0;
                    objSaveData.Price = 0;
                    objSaveData.Orderqty2 = 0;
                    objSaveData.Origclordid = "";
                    objSaveData.Orgorderqty = "";
                    objSaveData.Special_Type = 0;
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                }

                _return = Msg_tprl_orderBL.Insert(objSaveData);
                if (_return > 0)
                {
                    Logger.log.Info($"ProcessSaveMsgType | MsgSeqNum: {objSaveData.Msgseqnum}, MsgType: {objSaveData.Msgtype},  -->>> Save to DB sucess");
                    //break;
                }
                else
                {
                    Logger.log.Error($"Error process ProcessSaveMsgType with MsgSeqNum: {objSaveData.Msgseqnum}, MsgType: {objSaveData.Msgtype}, Error [{_return}] can't save to DB; Object={System.Text.Json.JsonSerializer.Serialize(objSaveData)} ");
                    //Thread.Sleep(ConfigData.TimeDelaySaveDB);
                }
                //}
                Logger.log.Info($"End process ProcessSaveMsgType_35_D_35_G_35_F with sequence= {sequence}, MsgType(35)={fixMessageBase.GetMsgType}");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error process ProcessSaveMsgType_35_D_35_G_35_F with sequence= {sequence}, Exception: {ex?.ToString()}");
            }
        }

        private void ProcessSaveMsgType_35_AI_AJ_Z_R_S_s_t_u(long sequence, FIXMessageBase fixMessageBase, string p_SendOrRecei)
        {
            try
            {
                Logger.log.Info($"Start process ProcessSaveMsgType_35_AI_AJ_Z_R_S_s_t_u with sequence= {sequence}, MsgType(35)={fixMessageBase.GetMsgType}");
                int _return = 0;
                //while (true)
                //{
                Msg_Tprl_Outright_Info objSaveData = new Msg_Tprl_Outright_Info();
                if (fixMessageBase.GetMsgType == MessageType.QuoteStatusReport) // 35=AI
                {
                    MessageQuoteSatusReport msgData = (MessageQuoteSatusReport)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed; // 369

                    objSaveData.Text = msgData.Text;
                    objSaveData.Ordtype = msgData.OrdType;
                    objSaveData.Crosstype = 0;
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Crossid = "";
                    objSaveData.Account = msgData.Account;
                    objSaveData.Coaccount = "";
                    objSaveData.Partyid = "";
                    objSaveData.Copartyid = "";
                    objSaveData.Orderqty = msgData.OrderQty;
                    objSaveData.Effectivetime = "";
                    objSaveData.Side = msgData.Side;
                    objSaveData.Symbol = msgData.Symbol;
                    objSaveData.Price2 = msgData.Price2.ToString();
                    objSaveData.Settlvalue = msgData.SettlValue;
                    objSaveData.Settldate = msgData.SettDate;
                    objSaveData.Settlmethod = msgData.SettlMethod;
                    objSaveData.Orderid = "";
                    objSaveData.Origcrossid = "";
                    objSaveData.Registid = msgData.RegistID;
                    objSaveData.Rfqreqid = "";
                    objSaveData.Quoterespid = "";
                    objSaveData.Quoteresptype = "";
                    objSaveData.Quoteid = msgData.QuoteID;
                    objSaveData.Quotecanceltype = "";
                    objSaveData.Orderpartyid = msgData.OrderPartyID;
                    objSaveData.Quotereqid = "";
                    objSaveData.Quotetype = msgData.QuoteType.ToString();
                    //
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                }
                else if (fixMessageBase.GetMsgType == MessageType.QuoteResponse) // 35=AJ
                {
                    MessageQuoteResponse msgData = (MessageQuoteResponse)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed; // 369

                    objSaveData.Text = msgData.Text;
                    objSaveData.Ordtype = msgData.OrdType;
                    objSaveData.Crosstype = 0;
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Crossid = "";
                    objSaveData.Account = msgData.Account;
                    objSaveData.Coaccount = msgData.CoAccount;
                    objSaveData.Partyid = "";
                    objSaveData.Copartyid = "";
                    objSaveData.Orderqty = msgData.OrderQty;
                    objSaveData.Effectivetime = "";
                    objSaveData.Side = msgData.Side;
                    objSaveData.Symbol = msgData.Symbol;
                    objSaveData.Price2 = msgData.Price2.ToString();
                    //objSaveData.Price = msgData.Price.ToString();  // khong thay map de luu
                    objSaveData.Settlvalue = msgData.SettlValue;
                    objSaveData.Settldate = msgData.SettDate;
                    objSaveData.Settlmethod = msgData.SettlMethod;
                    objSaveData.Orderid = "";
                    objSaveData.Origcrossid = "";
                    objSaveData.Registid = "";
                    objSaveData.Rfqreqid ="";
                    objSaveData.Quoterespid = msgData.QuoteRespID;
                    objSaveData.Quoteresptype = msgData.QuoteRespType.ToString();
                    objSaveData.Quoteid = "";
                    objSaveData.Quotecanceltype = "";
                    objSaveData.Orderpartyid = "";
                    objSaveData.Quotereqid = "";
                    objSaveData.Quotetype = "";
                    objSaveData.Quotetype = "";
                    //
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                }
                else if (fixMessageBase.GetMsgType == MessageType.QuoteCancel) // 35=Z
                {
                    MessageQuoteCancel msgData = (MessageQuoteCancel)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed; // 369

                    objSaveData.Text = msgData.Text;
                    objSaveData.Ordtype = msgData.OrdType;
                    objSaveData.Crosstype = 0;
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Crossid = "";
                    objSaveData.Account = "";
                    objSaveData.Coaccount = "";
                    objSaveData.Partyid = "";
                    objSaveData.Copartyid = "";
                    objSaveData.Orderqty = 0;
                    objSaveData.Effectivetime = "";
                    objSaveData.Side = 0;
                    objSaveData.Symbol = msgData.Symbol;
                    objSaveData.Price2 = "";
                    objSaveData.Settlvalue = 0;
                    objSaveData.Settldate = "";
                    objSaveData.Settlmethod = 0;
                    objSaveData.Orderid = "";
                    objSaveData.Origcrossid = "";
                    objSaveData.Registid = "";
                    objSaveData.Rfqreqid = "";
                    objSaveData.Quoterespid = "";
                    objSaveData.Quoteresptype = "";
                    objSaveData.Quoteid = "";
                    objSaveData.Quotecanceltype = "";
                    objSaveData.Orderpartyid = "";
                    objSaveData.Quotereqid = "";
                    objSaveData.Quotetype = "";
                    objSaveData.Quotetype = "";
                    //
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                }
                else if (fixMessageBase.GetMsgType == MessageType.QuoteRequest) // 35=R
                {
                    MessageQuoteRequest msgData = (MessageQuoteRequest)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed; // 369

                    objSaveData.Text = msgData.Text;
                    objSaveData.Ordtype = msgData.OrdType;
                    objSaveData.Crosstype = 0;
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Crossid = "";
                    objSaveData.Account = msgData.Account;
                    objSaveData.Coaccount = "";
                    objSaveData.Partyid = "";
                    objSaveData.Copartyid = "";
                    objSaveData.Orderqty = msgData.OrderQty;
                    objSaveData.Effectivetime = "";
                    objSaveData.Side = msgData.Side;
                    objSaveData.Symbol = msgData.Symbol;
                    objSaveData.Price2 = msgData.Price2.ToString();
                    objSaveData.Settlvalue = msgData.SettlValue;
                    objSaveData.Settldate = msgData.SettDate;
                    objSaveData.Settlmethod = msgData.SettlMethod;
                    objSaveData.Orderid = msgData.OrderID;
                    objSaveData.Origcrossid = "";
                    objSaveData.Registid = msgData.RegistID;
                    objSaveData.Rfqreqid = msgData.RFQReqID;
                    objSaveData.Quoterespid = "";
                    objSaveData.Quoteresptype = "";
                    objSaveData.Quoteid = "";
                    objSaveData.Quotecanceltype = "";
                    objSaveData.Orderpartyid = "";
                    objSaveData.Quotereqid = "";
                    objSaveData.Quotetype = "";
                    objSaveData.Quotetype = "";
                    //
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                }
                else if (fixMessageBase.GetMsgType == MessageType.Quote) // 35=S
                {
                    MessageQuote msgData = (MessageQuote)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed; // 369

                    objSaveData.Text = msgData.Text;
                    objSaveData.Ordtype = msgData.OrdType;
                    objSaveData.Crosstype = 0;
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Crossid = "";
                    objSaveData.Account = msgData.Account;
                    objSaveData.Coaccount = "";
                    objSaveData.Partyid = "";
                    objSaveData.Copartyid = "";
                    objSaveData.Orderqty = msgData.OrderQty;
                    objSaveData.Effectivetime = "";
                    objSaveData.Side = msgData.Side;
                    objSaveData.Symbol = msgData.Symbol;
                    objSaveData.Price2 = msgData.Price2.ToString();
                    objSaveData.Settlvalue = msgData.SettlValue;
                    objSaveData.Settldate = msgData.SettDate;
                    objSaveData.Settlmethod = msgData.SettlMethod;
                    objSaveData.Orderid = "";
                    objSaveData.Origcrossid = "";
                    objSaveData.Registid = msgData.RegistID;
                    objSaveData.Rfqreqid = "";
                    objSaveData.Quoterespid = "";
                    objSaveData.Quoteresptype = "";
                    objSaveData.Quoteid = "";
                    objSaveData.Quotecanceltype = "";
                    objSaveData.Orderpartyid = "";
                    objSaveData.Quotereqid = "";
                    objSaveData.Quotetype = "";
                    objSaveData.Quotetype = "";
                    //
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                }
                else if (fixMessageBase.GetMsgType == MessageType.NewOrderCross) // 35=s
                {
                    MessageNewOrderCross msgData = (MessageNewOrderCross)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed; // 369

                    objSaveData.Text = msgData.Text;
                    objSaveData.Ordtype = msgData.OrdType;
                    objSaveData.Crosstype = msgData.CrossType;
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Crossid = msgData.CrossID;
                    objSaveData.Account = msgData.Account;
                    objSaveData.Coaccount = msgData.CoAccount;
                    objSaveData.Partyid = msgData.PartyID;
                    objSaveData.Copartyid = msgData.CoPartyID;
                    objSaveData.Orderqty = msgData.OrderQty;
                    objSaveData.Effectivetime = msgData.EffectiveTime;
                    objSaveData.Side = msgData.Side;
                    objSaveData.Symbol = msgData.Symbol;
                    objSaveData.Price2 = msgData.Price2.ToString();
                    objSaveData.Settlvalue = msgData.SettlValue;
                    objSaveData.Settldate = msgData.SettDate;
                    objSaveData.Settlmethod = msgData.SettlMethod;
                    objSaveData.Orderid = "";
                    objSaveData.Origcrossid = "";
                    objSaveData.Registid = "";
                    objSaveData.Rfqreqid = "";
                    objSaveData.Quoterespid = "";
                    objSaveData.Quoteresptype = "";
                    objSaveData.Quoteid = "";
                    objSaveData.Quotecanceltype = "";
                    objSaveData.Orderpartyid = "";
                    objSaveData.Quotereqid = "";
                    objSaveData.Quotetype = "";
                    objSaveData.Quotetype = "";
                    //
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                }
                else if (fixMessageBase.GetMsgType == MessageType.CrossOrderCancelReplaceRequest) // 35=t
                {
                    CrossOrderCancelReplaceRequest msgData = (CrossOrderCancelReplaceRequest)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed; // 369

                    objSaveData.Text = msgData.Text;
                    objSaveData.Ordtype = msgData.OrdType;
                    objSaveData.Crosstype = msgData.CrossType;
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Crossid = "";
                    objSaveData.Account = msgData.Account;
                    objSaveData.Coaccount = msgData.CoAccount;
                    objSaveData.Partyid = msgData.PartyID;
                    objSaveData.Copartyid = msgData.CoPartyID;
                    objSaveData.Orderqty = msgData.OrderQty;
                    objSaveData.Effectivetime = msgData.EffectiveTime;
                    objSaveData.Side = msgData.Side;
                    objSaveData.Symbol = msgData.Symbol;
                    objSaveData.Price2 = msgData.Price2.ToString();
                    objSaveData.Settlvalue = msgData.SettlValue;
                    objSaveData.Settldate = msgData.SettDate;
                    objSaveData.Settlmethod = msgData.SettlMethod;
                    objSaveData.Orderid = msgData.OrderID;
                    objSaveData.Origcrossid = msgData.OrgCrossID;
                    objSaveData.Registid = "";
                    objSaveData.Rfqreqid = "";
                    objSaveData.Quoterespid = "";
                    objSaveData.Quoteresptype = "";
                    objSaveData.Quoteid = "";
                    objSaveData.Quotecanceltype = "";
                    objSaveData.Orderpartyid = "";
                    objSaveData.Quotereqid = "";
                    objSaveData.Quotetype = "";
                    objSaveData.Quotetype = "";
                    //
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                }
                else if (fixMessageBase.GetMsgType == MessageType.CrossOrderCancelRequest) // 35=u
                {
                    CrossOrderCancelRequest msgData = (CrossOrderCancelRequest)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed; // 369

                    objSaveData.Text = msgData.Text;
                    objSaveData.Ordtype = msgData.OrdType;
                    objSaveData.Crosstype = msgData.CrossType;
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Crossid = "";
                    objSaveData.Account = "";
                    objSaveData.Coaccount = "";
                    objSaveData.Partyid = "";
                    objSaveData.Copartyid = "";
                    objSaveData.Orderqty = 0;
                    objSaveData.Effectivetime = "";
                    objSaveData.Side = msgData.Side;
                    objSaveData.Symbol = msgData.Symbol;
                    objSaveData.Price2 = "";
                    objSaveData.Settlvalue = 0;
                    objSaveData.Settldate = "";
                    objSaveData.Settlmethod = 0;
                    objSaveData.Orderid = msgData.OrderID;
                    objSaveData.Origcrossid = msgData.OrgCrossID;
                    objSaveData.Registid = "";
                    objSaveData.Rfqreqid = "";
                    objSaveData.Quoterespid = "";
                    objSaveData.Quoteresptype = "";
                    objSaveData.Quoteid = "";
                    objSaveData.Quotecanceltype = "";
                    objSaveData.Orderpartyid = "";
                    objSaveData.Quotereqid = "";
                    objSaveData.Quotetype = "";
                    objSaveData.Quotetype = "";
                    //
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                }

                _return = Msg_tprl_outrightBL.Insert(objSaveData);
                if (_return > 0)
                {
                    Logger.log.Info($"ProcessSaveMsgType | MsgSeqNum: {objSaveData.Msgseqnum}, MsgType: {objSaveData.Msgtype},  -->>> Save to DB sucess");
                    //break;
                }
                else
                {
                    Logger.log.Error($"Error process ProcessSaveMsgType with MsgSeqNum: {objSaveData.Msgseqnum}, MsgType: {objSaveData.Msgtype}, Error [{_return}] can't save to DB; Object={System.Text.Json.JsonSerializer.Serialize(objSaveData)} ");
                    //Thread.Sleep(ConfigData.TimeDelaySaveDB);
                }
                //}
                Logger.log.Info($"End process ProcessSaveMsgType_35_AI_AJ_Z_R_S_s_t_u with sequence= {sequence}, MsgType(35)={fixMessageBase.GetMsgType}");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error process ProcessSaveMsgType_35_AI_AJ_Z_R_S_s_t_u with sequence= {sequence}, Exception: {ex?.ToString()}");
            }
        }

        private void ProcessSaveMsgType_35_EE_N01_N02_N03_N04_N05_MA_ME_MC_MR(long sequence, FIXMessageBase fixMessageBase, string p_SendOrRecei)
        {
            try
            {
                Logger.log.Info($"Start process ProcessSaveMsgType with sequence= {sequence}, MsgType(35)={fixMessageBase.GetMsgType}");
                long _return = 0;
                //while (true)
                //{
                Msg_Tprl_Repo_Info objSaveData = new Msg_Tprl_Repo_Info();
                bool repoDetailExist = false;
                List<ReposSideExecOrder> listReposSideExecOrder_EE_N01 = null;
                List<ReposSide> listReposSide_N03_N04_N05_MA_ME = null;
                List<ReposSideReposBCGDReport> listReposSideReposBCGDReportList_MR = null;
                string _symbol = string.Empty;
                if (fixMessageBase.GetMsgType == MessageType.ExecOrderRepos) // 35=EE
                {
                    MessageExecOrderRepos msgData = (MessageExecOrderRepos)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Possdupflag = msgData.PossDupFlag == true ? "Y" : "N";
                    objSaveData.Text = msgData.Text;
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed.ToString(); // 369
                    //
                    objSaveData.Partyid = msgData.PartyID;
                    objSaveData.Copartyid = msgData.CoPartyID;
                    objSaveData.Matchreporttype = msgData.MatchReportType;
                    objSaveData.Orderid = msgData.OrderID;
                    objSaveData.Buyorderid = msgData.BuyOrderID;
                    objSaveData.Sellorderid = msgData.SellOrderID;
                    objSaveData.Repurchaserate = msgData.RepurchaseRate.ToString();
                    objSaveData.Repurchaseterm = msgData.RepurchaseTerm;
                    objSaveData.Noside = msgData.NoSide;
                    objSaveData.Quotetype = 0;
                    objSaveData.Multilegrpttypereq = 0;
                    objSaveData.Ordtype = "";
                    objSaveData.Rfqreqid = "";
                    objSaveData.Orgorderid = "";
                    objSaveData.Quoteid = "";
                    objSaveData.Side = "";
                    objSaveData.Orderqty = 0;
                    objSaveData.Effectivetime = "";
                    objSaveData.Coaccount = "";
                    objSaveData.Registid = "";
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Settldate = msgData.SettDate;
                    objSaveData.Settldate2 = msgData.SettlDate2;
                    objSaveData.Enddate = msgData.EndDate;
                    objSaveData.Settlmethod = msgData.SettlMethod.ToString();
                    objSaveData.Orderpartyid = "";
                    objSaveData.Inquirymember = "";
                    //
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Account = "";
                    //
                    repoDetailExist = true;
                    int _countReposSideList = 0;
                    if (msgData.ReposSideList != null && msgData.ReposSideList.Count > 0)
                    {
                        _countReposSideList = msgData.ReposSideList.Count;
                        //
                        listReposSideExecOrder_EE_N01 = new List<ReposSideExecOrder>();
                        ReposSideExecOrder itemSite;
                        for (int i = 0; i < msgData.ReposSideList.Count; i++)
                        {
                            itemSite = msgData.ReposSideList[i];
                            listReposSideExecOrder_EE_N01.Add(itemSite);
                        }
                    }
                    else
                    {
                        Logger.log.Warn($"ProcessSaveMsgType with MsgSeqNum(34)={msgData.MsgSeqNum}, MsgType(35)={fixMessageBase.GetMsgType}, msgData.ReposSideList is null or  msgData.ReposSideList.Count = 0");
                    }

                    Logger.log.Info($"ProcessSaveMsgType with MsgSeqNum(34)={msgData.MsgSeqNum}, MsgType(35)={fixMessageBase.GetMsgType}, repoDetailExist={repoDetailExist}, total ReposSideList={_countReposSideList}");
                }
                else if (fixMessageBase.GetMsgType == MessageType.ReposInquiry) // 35=N01
                {
                    MessageReposInquiry msgData = (MessageReposInquiry)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Possdupflag = msgData.PossDupFlag == true ? "Y" : "N";
                    objSaveData.Text = msgData.Text;
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed.ToString(); // 369

                    objSaveData.Partyid = "";
                    objSaveData.Copartyid = "";
                    objSaveData.Matchreporttype = 0;
                    objSaveData.Orderid = "";
                    objSaveData.Buyorderid = "";
                    objSaveData.Sellorderid = "";
                    objSaveData.Repurchaserate = "";
                    objSaveData.Repurchaseterm = msgData.RepurchaseTerm;
                    objSaveData.Noside = 0;
                    objSaveData.Quotetype = msgData.QuoteType;
                    objSaveData.Multilegrpttypereq = 0;
                    objSaveData.Ordtype = msgData.OrdType;
                    objSaveData.Rfqreqid = msgData.RFQReqID;
                    objSaveData.Orgorderid = "";
                    objSaveData.Quoteid = "";
                    objSaveData.Side = msgData.Side.ToString();
                    objSaveData.Orderqty = msgData.OrderQty;
                    objSaveData.Effectivetime = msgData.EffectiveTime;
                    objSaveData.Coaccount = "";
                    objSaveData.Registid = msgData.RegistID;
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Settldate = msgData.SettlDate;
                    objSaveData.Settldate2 = msgData.SettlDate2;
                    objSaveData.Enddate = msgData.EndDate;
                    objSaveData.Settlmethod = msgData.SettlMethod.ToString();
                    objSaveData.Orderpartyid = "";
                    objSaveData.Inquirymember = "";
                    //
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Account = "";
                    //
                    _symbol = msgData.Symbol;
                }
                else if (fixMessageBase.GetMsgType == MessageType.ReposInquiryReport) // 35=N02
                {
                    MessageReposInquiryReport msgData = (MessageReposInquiryReport)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Possdupflag = msgData.PossDupFlag == true ? "Y" : "N";
                    objSaveData.Text = msgData.Text;
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed.ToString(); // 369

                    objSaveData.Partyid = "";
                    objSaveData.Copartyid = "";
                    objSaveData.Matchreporttype = 0;
                    objSaveData.Orderid = "";
                    objSaveData.Buyorderid = "";
                    objSaveData.Sellorderid = "";
                    objSaveData.Repurchaserate = "";
                    objSaveData.Repurchaseterm = msgData.RepurchaseTerm;
                    objSaveData.Noside = 0;
                    objSaveData.Quotetype = msgData.QuoteType;
                    objSaveData.Multilegrpttypereq = 0;
                    objSaveData.Ordtype = msgData.OrdType;
                    objSaveData.Rfqreqid = msgData.RFQReqID;
                    objSaveData.Orgorderid = "";
                    objSaveData.Quoteid = msgData.QuoteID;
                    objSaveData.Side = msgData.Side.ToString();
                    objSaveData.Orderqty = msgData.OrderQty;
                    objSaveData.Effectivetime = msgData.EffectiveTime;
                    objSaveData.Coaccount = "";
                    objSaveData.Registid = msgData.RegistID;
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Settldate = msgData.SettlDate;
                    objSaveData.Settldate2 = msgData.SettlDate2;
                    objSaveData.Enddate = msgData.EndDate;
                    objSaveData.Settlmethod = msgData.SettlMethod.ToString();
                    objSaveData.Orderpartyid = msgData.OrderPartyID;
                    objSaveData.Inquirymember = "";
                    //
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Account = "";
                    //
                    _symbol = msgData.Symbol;
                }
                else if (fixMessageBase.GetMsgType == MessageType.ReposFirm) // 35=N03
                {
                    MessageReposFirm msgData = (MessageReposFirm)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Possdupflag = msgData.PossDupFlag == true ? "Y" : "N";
                    objSaveData.Text = msgData.Text;
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed.ToString(); // 369
                    //
                    objSaveData.Partyid = "";
                    objSaveData.Copartyid = "";
                    objSaveData.Matchreporttype = 0;
                    objSaveData.Orderid = "";
                    objSaveData.Buyorderid = "";
                    objSaveData.Sellorderid = "";
                    objSaveData.Repurchaserate = msgData.RepurchaseRate.ToString();
                    objSaveData.Repurchaseterm = msgData.RepurchaseTerm;
                    objSaveData.Noside = msgData.NoSide;
                    objSaveData.Quotetype = msgData.QuoteType;
                    objSaveData.Multilegrpttypereq = 0;
                    objSaveData.Ordtype = msgData.OrdType;
                    objSaveData.Rfqreqid = msgData.RFQReqID;
                    objSaveData.Orgorderid = "";
                    objSaveData.Quoteid = "";
                    objSaveData.Side = msgData.Side.ToString();
                    objSaveData.Orderqty = 0;
                    objSaveData.Effectivetime = msgData.EffectiveTime;
                    objSaveData.Coaccount = "";
                    objSaveData.Registid = "";
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Settldate = msgData.SettlDate;
                    objSaveData.Settldate2 = msgData.SettlDate2;
                    objSaveData.Enddate = msgData.EndDate;
                    objSaveData.Settlmethod = msgData.SettlMethod.ToString();
                    objSaveData.Orderpartyid = "";
                    objSaveData.Inquirymember = "";
                    //
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Account = msgData.Account;

                    //
                    repoDetailExist = true;
                    int _countReposSideList = 0;
                    if (msgData.RepoSideList != null && msgData.RepoSideList.Count > 0)
                    {
                        _countReposSideList = msgData.RepoSideList.Count;
                        //
                        listReposSide_N03_N04_N05_MA_ME = new List<ReposSide>();
                        ReposSide itemSite;
                        for (int i = 0; i < msgData.RepoSideList.Count; i++)
                        {
                            itemSite = msgData.RepoSideList[i];
                            listReposSide_N03_N04_N05_MA_ME.Add(itemSite);
                        }
                    }
                    else
                    {
                        Logger.log.Warn($"ProcessSaveMsgType with MsgSeqNum(34)={msgData.MsgSeqNum}, MsgType(35)={fixMessageBase.GetMsgType}, msgData.RepoSideList is null or  msgData.RepoSideList.Count = 0");
                    }
                    Logger.log.Info($"ProcessSaveMsgType with MsgSeqNum(34)={msgData.MsgSeqNum}, MsgType(35)={fixMessageBase.GetMsgType}, repoDetailExist={repoDetailExist}, total RepoSideList={_countReposSideList}");
                }
                else if (fixMessageBase.GetMsgType == MessageType.ReposFirmReport) // 35=N04
                {
                    MessageReposFirmReport msgData = (MessageReposFirmReport)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Possdupflag = msgData.PossDupFlag == true ? "Y" : "N";
                    objSaveData.Text = msgData.Text;
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed.ToString(); // 369

                    objSaveData.Partyid = "";
                    objSaveData.Copartyid = "";
                    objSaveData.Matchreporttype = msgData.MatchReportType;
                    objSaveData.Orderid = "";
                    objSaveData.Buyorderid = "";
                    objSaveData.Sellorderid = "";
                    objSaveData.Repurchaserate = msgData.RepurchaseRate.ToString();
                    objSaveData.Repurchaseterm = msgData.RepurchaseTerm;
                    objSaveData.Noside = msgData.NoSide;
                    objSaveData.Quotetype = msgData.QuoteType;
                    objSaveData.Multilegrpttypereq = 0;
                    objSaveData.Ordtype = msgData.OrdType;
                    objSaveData.Rfqreqid = msgData.RFQReqID;
                    objSaveData.Orgorderid = "";
                    objSaveData.Quoteid = msgData.QuoteID;
                    objSaveData.Side = msgData.Side.ToString();
                    objSaveData.Orderqty = 0;
                    objSaveData.Effectivetime = msgData.EffectiveTime;
                    objSaveData.Coaccount = "";
                    objSaveData.Registid = "";
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Settldate = msgData.SettlDate;
                    objSaveData.Settldate2 = msgData.SettlDate2;
                    objSaveData.Enddate = msgData.EndDate;
                    objSaveData.Settlmethod = msgData.SettlMethod.ToString();
                    objSaveData.Orderpartyid = msgData.OrderPartyID;
                    objSaveData.Inquirymember = msgData.InquiryMember;
                    //
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Account = msgData.Account;
                    // co list
                    repoDetailExist = true;
                    int _countReposSideList = 0;

                    if (msgData.RepoSideList != null && msgData.RepoSideList.Count > 0)
                    {
                        _countReposSideList = msgData.RepoSideList.Count;
                        //
                        listReposSide_N03_N04_N05_MA_ME = new List<ReposSide>();
                        ReposSide itemSite;
                        for (int i = 0; i < msgData.RepoSideList.Count; i++)
                        {
                            itemSite = msgData.RepoSideList[i];
                            listReposSide_N03_N04_N05_MA_ME.Add(itemSite);
                        }
                    }
                    else
                    {
                        Logger.log.Warn($"ProcessSaveMsgType with MsgSeqNum(34)={msgData.MsgSeqNum}, MsgType(35)={fixMessageBase.GetMsgType}, msgData.RepoSideList is null or  msgData.RepoSideList.Count = 0");
                    }
                    Logger.log.Info($"ProcessSaveMsgType with MsgSeqNum(34)={msgData.MsgSeqNum}, MsgType(35)={fixMessageBase.GetMsgType}, repoDetailExist={repoDetailExist}, total RepoSideList={_countReposSideList}");
                }
                else if (fixMessageBase.GetMsgType == MessageType.ReposFirmAccept) // 35=N05
                {
                    MessageReposFirmAccept msgData = (MessageReposFirmAccept)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Possdupflag = msgData.PossDupFlag == true ? "Y" : "N";
                    objSaveData.Text = msgData.Text;
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed.ToString(); // 369

                    objSaveData.Partyid = "";
                    objSaveData.Copartyid = "";
                    objSaveData.Matchreporttype = 0;
                    objSaveData.Orderid = "";
                    objSaveData.Buyorderid = "";
                    objSaveData.Sellorderid = "";
                    objSaveData.Repurchaserate = msgData.RepurchaseRate.ToString();
                    objSaveData.Repurchaseterm = 0;
                    objSaveData.Noside = msgData.NoSide;
                    objSaveData.Quotetype = msgData.QuoteType;
                    objSaveData.Multilegrpttypereq = 0;
                    objSaveData.Ordtype = msgData.OrdType;
                    objSaveData.Rfqreqid = msgData.RFQReqID;
                    objSaveData.Orgorderid = "";
                    objSaveData.Quoteid = "";
                    objSaveData.Side = "";
                    objSaveData.Orderqty = 0;
                    objSaveData.Effectivetime = "";
                    objSaveData.Coaccount = msgData.CoAccount;
                    objSaveData.Registid = "";
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Settldate = "";
                    objSaveData.Settldate2 = "";
                    objSaveData.Enddate = "";
                    objSaveData.Settlmethod = "";
                    objSaveData.Orderpartyid = "";
                    objSaveData.Inquirymember = "";
                    //
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Account = msgData.Account;

                    // co list
                    repoDetailExist = true;
                    int _countReposSideList = 0;

                    if (msgData.RepoSideList != null && msgData.RepoSideList.Count > 0)
                    {
                        _countReposSideList = msgData.RepoSideList.Count;
                        //
                        listReposSide_N03_N04_N05_MA_ME = new List<ReposSide>();
                        ReposSide itemSite;
                        for (int i = 0; i < msgData.RepoSideList.Count; i++)
                        {
                            itemSite = msgData.RepoSideList[i];
                            listReposSide_N03_N04_N05_MA_ME.Add(itemSite);
                        }
                    }
                    else
                    {
                        Logger.log.Warn($"ProcessSaveMsgType with MsgSeqNum(34)={msgData.MsgSeqNum}, MsgType(35)={fixMessageBase.GetMsgType}, msgData.RepoSideList is null or  msgData.RepoSideList.Count = 0");
                    }
                    Logger.log.Info($"ProcessSaveMsgType with MsgSeqNum(34)={msgData.MsgSeqNum}, MsgType(35)={fixMessageBase.GetMsgType}, repoDetailExist={repoDetailExist}, total RepoSideList={_countReposSideList}");
                }
                else if (fixMessageBase.GetMsgType == MessageType.ReposBCGD) // 35=MA
                {
                    MessageReposBCGD msgData = (MessageReposBCGD)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Possdupflag = msgData.PossDupFlag == true ? "Y" : "N";
                    objSaveData.Text = msgData.Text;
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed.ToString(); // 369

                    objSaveData.Partyid = msgData.PartyID;
                    objSaveData.Copartyid = msgData.CoPartyID;
                    objSaveData.Matchreporttype = 0;
                    objSaveData.Orderid = "";
                    objSaveData.Buyorderid = "";
                    objSaveData.Sellorderid = "";
                    objSaveData.Repurchaserate = msgData.RepurchaseRate.ToString();
                    objSaveData.Repurchaseterm = msgData.RepurchaseTerm;
                    objSaveData.Noside = msgData.NoSide;
                    objSaveData.Quotetype = msgData.QuoteType;
                    objSaveData.Multilegrpttypereq = 0;
                    objSaveData.Ordtype = msgData.OrdType;
                    objSaveData.Rfqreqid = "";
                    objSaveData.Orgorderid = msgData.OrgOrderID;
                    objSaveData.Quoteid = "";
                    objSaveData.Side = msgData.Side.ToString();
                    objSaveData.Orderqty = 0;
                    objSaveData.Effectivetime = msgData.EffectiveTime;
                    objSaveData.Coaccount = msgData.CoAccount;
                    objSaveData.Registid = "";
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Settldate = msgData.SettlDate;
                    objSaveData.Settldate2 = msgData.SettlDate2;
                    objSaveData.Enddate = msgData.EndDate;
                    objSaveData.Settlmethod = msgData.SettlMethod.ToString();
                    objSaveData.Orderpartyid = "";
                    objSaveData.Inquirymember = "";
                    //
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Account = msgData.Account;

                    // co list
                    repoDetailExist = true;
                    int _countReposSideList = 0;

                    if (msgData.RepoSideList != null && msgData.RepoSideList.Count > 0)
                    {
                        _countReposSideList = msgData.RepoSideList.Count;
                        //
                        listReposSide_N03_N04_N05_MA_ME = new List<ReposSide>();
                        ReposSide itemSite;
                        for (int i = 0; i < msgData.RepoSideList.Count; i++)
                        {
                            itemSite = msgData.RepoSideList[i];
                            listReposSide_N03_N04_N05_MA_ME.Add(itemSite);
                        }
                    }
                    else
                    {
                        Logger.log.Warn($"ProcessSaveMsgType with MsgSeqNum(34)={msgData.MsgSeqNum}, MsgType(35)={fixMessageBase.GetMsgType}, msgData.RepoSideList is null or  msgData.RepoSideList.Count = 0");
                    }
                    Logger.log.Info($"ProcessSaveMsgType with MsgSeqNum(34)={msgData.MsgSeqNum}, MsgType(35)={fixMessageBase.GetMsgType}, repoDetailExist={repoDetailExist}, total RepoSideList={_countReposSideList}");
                }
                else if (fixMessageBase.GetMsgType == MessageType.ReposBCGDModify) // 35=ME
                {
                    MessageReposBCGDModify msgData = (MessageReposBCGDModify)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Possdupflag = msgData.PossDupFlag == true ? "Y" : "N";
                    objSaveData.Text = msgData.Text;
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed.ToString(); // 369

                    objSaveData.Partyid = msgData.PartyID;
                    objSaveData.Copartyid = msgData.CoPartyID;
                    objSaveData.Matchreporttype = 0;
                    objSaveData.Orderid = "";
                    objSaveData.Buyorderid = "";
                    objSaveData.Sellorderid = "";
                    objSaveData.Repurchaserate = msgData.RepurchaseRate.ToString();
                    objSaveData.Repurchaseterm = msgData.RepurchaseTerm;
                    objSaveData.Noside = msgData.NoSide;
                    objSaveData.Quotetype = msgData.QuoteType;
                    objSaveData.Multilegrpttypereq = 0;
                    objSaveData.Ordtype = msgData.OrdType;
                    objSaveData.Rfqreqid = "";
                    objSaveData.Orgorderid = msgData.OrgOrderID;
                    objSaveData.Quoteid = "";
                    objSaveData.Side = msgData.Side.ToString();
                    objSaveData.Orderqty = 0;
                    objSaveData.Effectivetime = msgData.EffectiveTime;
                    objSaveData.Coaccount = msgData.CoAccount;
                    objSaveData.Registid = "";
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Settldate = msgData.SettlDate;
                    objSaveData.Settldate2 = msgData.SettlDate2;
                    objSaveData.Enddate = msgData.EndDate;
                    objSaveData.Settlmethod = msgData.SettlMethod.ToString();
                    objSaveData.Orderpartyid = "";
                    objSaveData.Inquirymember = "";
                    //
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Account = msgData.Account;

                    // co list
                    repoDetailExist = true;
                    int _countReposSideList = 0;

                    if (msgData.RepoSideList != null && msgData.RepoSideList.Count > 0)
                    {
                        _countReposSideList = msgData.RepoSideList.Count;
                        //
                        listReposSide_N03_N04_N05_MA_ME = new List<ReposSide>();
                        ReposSide itemSite;
                        for (int i = 0; i < msgData.RepoSideList.Count; i++)
                        {
                            itemSite = msgData.RepoSideList[i];
                            listReposSide_N03_N04_N05_MA_ME.Add(itemSite);
                        }
                    }
                    else
                    {
                        Logger.log.Warn($"ProcessSaveMsgType with MsgSeqNum(34)={msgData.MsgSeqNum}, MsgType(35)={fixMessageBase.GetMsgType}, msgData.RepoSideList is null or  msgData.RepoSideList.Count = 0");
                    }
                    Logger.log.Info($"ProcessSaveMsgType with MsgSeqNum(34)={msgData.MsgSeqNum}, MsgType(35)={fixMessageBase.GetMsgType}, repoDetailExist={repoDetailExist}, total RepoSideList={_countReposSideList}");
                }
                else if (fixMessageBase.GetMsgType == MessageType.ReposBCGDCancel) // 35=MC
                {
                    MessageReposBCGDCancel msgData = (MessageReposBCGDCancel)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Possdupflag = msgData.PossDupFlag == true ? "Y" : "N";
                    objSaveData.Text = msgData.Text;
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed.ToString(); // 369

                    objSaveData.Partyid = "";
                    objSaveData.Copartyid = "";
                    objSaveData.Matchreporttype = 0;
                    objSaveData.Orderid = "";
                    objSaveData.Buyorderid = "";
                    objSaveData.Sellorderid = "";
                    objSaveData.Repurchaserate = "";
                    objSaveData.Repurchaseterm = 0;
                    objSaveData.Noside = 0;
                    objSaveData.Quotetype = msgData.QuoteType;
                    objSaveData.Multilegrpttypereq = 0;
                    objSaveData.Ordtype = msgData.OrdType;
                    objSaveData.Rfqreqid = "";
                    objSaveData.Orgorderid = msgData.OrgOrderID;
                    objSaveData.Quoteid = "";
                    objSaveData.Side = msgData.Side.ToString();
                    objSaveData.Orderqty = 0;
                    objSaveData.Effectivetime = "";
                    objSaveData.Coaccount = "";
                    objSaveData.Registid = "";
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Settldate = "";
                    objSaveData.Settldate2 = "";
                    objSaveData.Enddate = "";
                    objSaveData.Settlmethod = "";
                    objSaveData.Orderpartyid = "";
                    objSaveData.Inquirymember = "";
                    //
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Account = "";

                    //
                    _symbol = "";
                }
                else if (fixMessageBase.GetMsgType == MessageType.ReposBCGDReport) // 35=MR
                {
                    MessageReposBCGDReport msgData = (MessageReposBCGDReport)fixMessageBase;

                    objSaveData.Sor = p_SendOrRecei;
                    objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                    objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                    objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                    objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                    objSaveData.Possdupflag = msgData.PossDupFlag == true ? "Y" : "N";
                    objSaveData.Text = msgData.Text;
                    objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                    objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed.ToString(); // 369

                    objSaveData.Partyid = msgData.PartyID;
                    objSaveData.Copartyid = msgData.CoPartyID;
                    objSaveData.Matchreporttype = 0;
                    objSaveData.Orderid = msgData.OrderID;
                    objSaveData.Buyorderid = "";
                    objSaveData.Sellorderid = "";
                    objSaveData.Repurchaserate = msgData.RepurchaseRate.ToString();
                    objSaveData.Repurchaseterm = msgData.RepurchaseTerm;
                    objSaveData.Noside = msgData.NoSide;
                    objSaveData.Quotetype = msgData.QuoteType;
                    objSaveData.Multilegrpttypereq = 0;
                    objSaveData.Ordtype = msgData.OrdType;
                    objSaveData.Rfqreqid = "";
                    objSaveData.Orgorderid = msgData.OrgOrderID;
                    objSaveData.Quoteid = "";
                    objSaveData.Side = msgData.Side.ToString();
                    objSaveData.Orderqty = 0;
                    objSaveData.Effectivetime = msgData.EffectiveTime;
                    objSaveData.Coaccount = msgData.CoAccount;
                    objSaveData.Registid = "";
                    objSaveData.Matchreporttype = 0;
                    objSaveData.Clordid = msgData.ClOrdID;
                    objSaveData.Settldate = msgData.SettlDate;
                    objSaveData.Settldate2 = msgData.SettlDate2;
                    objSaveData.Enddate = msgData.EndDate;
                    objSaveData.Settlmethod = msgData.SettlMethod.ToString();
                    objSaveData.Orderpartyid = msgData.OrderPartyID;
                    objSaveData.Inquirymember = "";
                    //
                    objSaveData.Remark = "";
                    objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                    objSaveData.Account = msgData.Account;
                    // co list
                    repoDetailExist = true;
                    int _countReposSideList = 0;

                    //
                    if (msgData.RepoBCGDSideList != null && msgData.RepoBCGDSideList.Count > 0)
                    {
                        _countReposSideList = msgData.RepoBCGDSideList.Count;
                        //
                        listReposSideReposBCGDReportList_MR = new List<ReposSideReposBCGDReport>();
                        //
                        ReposSideReposBCGDReport itemSite;
                        for (int i = 0; i < msgData.RepoBCGDSideList.Count; i++)
                        {
                            itemSite = msgData.RepoBCGDSideList[i];
                            listReposSideReposBCGDReportList_MR.Add(itemSite);
                        }
                    }
                    else
                    {
                        Logger.log.Warn($"ProcessSaveMsgType with MsgSeqNum(34)={msgData.MsgSeqNum}, MsgType(35)={fixMessageBase.GetMsgType}, msgData.RepoSideList is null or  msgData.RepoSideList.Count = 0");
                    }
                    Logger.log.Info($"ProcessSaveMsgType with MsgSeqNum(34)={msgData.MsgSeqNum}, MsgType(35)={fixMessageBase.GetMsgType}, repoDetailExist={repoDetailExist}, total RepoSideList={_countReposSideList}");
                }

                // isRepoDetail=false -> ignore Repo Detail
                // isRepoDetail=true -> insert Repo Detail
                _return = Msg_tprl_repoBL.ProcessInsertRepos(objSaveData, repoDetailExist, _symbol, listReposSideExecOrder_EE_N01, listReposSide_N03_N04_N05_MA_ME, listReposSideReposBCGDReportList_MR);
                if (_return > 0)
                {
                    Logger.log.Info($"ProcessSaveMsgType | MsgSeqNum(34): {objSaveData.Msgseqnum}, MsgType: {objSaveData.Msgtype},  -->>> Save to DB sucess");
                    //break;
                }
                else
                {
                    Logger.log.Error($"Error process ProcessSaveMsgType with MsgSeqNum(34): {objSaveData.Msgseqnum}, MsgType: {objSaveData.Msgtype}, Error [{_return}] can't save to DB; Object={System.Text.Json.JsonSerializer.Serialize(objSaveData)} ");
                    //Thread.Sleep(ConfigData.TimeDelaySaveDB);
                }
                //}
                Logger.log.Info($"End process ProcessSaveMsgType with sequence = {sequence}, MsgType(35) ={fixMessageBase.GetMsgType}");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error process ProcessSaveMsgType with sequence = {sequence}, MsgType(35) ={fixMessageBase.GetMsgType}, Exception: {ex?.ToString()}");
            }
        }

        private void ProcessSaveMsgType_35_8(long sequence, FIXMessageBase fixMessageBase, string p_SendOrRecei)
        {
            try
            {
                Logger.log.Info($"Start process ProcessSaveMsgType_35_8 with sequence= {sequence}, MsgType(35)={fixMessageBase.GetMsgType}");
                int _return = 0;
                //while (true)
                //{
                Msg_Tprl_Hnx_Confirm_Info objSaveData = new Msg_Tprl_Hnx_Confirm_Info();
                MessageExecutionReport msgData = (MessageExecutionReport)fixMessageBase;

                objSaveData.Sor = p_SendOrRecei;
                objSaveData.Msgtype = fixMessageBase.GetMsgType; // 35
                objSaveData.Sendercompid = fixMessageBase.GetSenderCompID; // 49
                objSaveData.Targetcompid = fixMessageBase.GetTargetCompID; // 56
                objSaveData.Msgseqnum = msgData.MsgSeqNum; // 34
                objSaveData.Possdupflag = msgData.PossDupFlag == true ? "Y" : "N";
                objSaveData.Sendingtime = fixMessageBase.GetSendingTime.ToString(); // 52
                objSaveData.Text = msgData.Text;

                objSaveData.Lastmsgseqnumprocessed = msgData.LastMsgSeqNumProcessed.ToString(); // 369

                if (msgData.ExecType == ExecutionReportType.ER_ExecOrder_3) // 150 = 3
                {
                    MessageER_ExecOrder msgDataExcecType = (MessageER_ExecOrder)fixMessageBase;
                    //
                    objSaveData.Ordstatus = msgDataExcecType.OrdStatus;
                    objSaveData.Orderid = msgDataExcecType.OrderID;
                    objSaveData.Clordid = msgDataExcecType.ClOrdID;
                    objSaveData.Symbol = msgDataExcecType.Symbol;
                    objSaveData.Side = msgDataExcecType.Side.ToString();
                    objSaveData.Orderqty = msgDataExcecType.OrderQty;
                    objSaveData.Ordtype = msgDataExcecType.OrdType;
                    objSaveData.Price = msgDataExcecType.Price;
                    objSaveData.Account = msgDataExcecType.Account;
                    objSaveData.Settlvalue = msgDataExcecType.SettlValue;
                    objSaveData.Leavesqty = 0;
                    objSaveData.Origclordid = msgDataExcecType.OrigClOrdID;
                    objSaveData.Lastqty = msgDataExcecType.LastQty;
                    objSaveData.Lastpx = msgDataExcecType.LastPx;
                    objSaveData.Execid = msgDataExcecType.ExecID;
                    objSaveData.Reciprocalmember = msgDataExcecType.ReciprocalMember;
                    //
                    objSaveData.Underlyinglastqty = "";
                    //
                    objSaveData.Exectype = ExecutionReportType.ER_ExecOrder_3.ToString();
                }
                else if (msgData.ExecType == ExecutionReportType.ER_CancelOrder_4) // 150 = 4
                {
                    MessageER_OrderCancel msgDataExcecType = (MessageER_OrderCancel)fixMessageBase;
                    //
                    objSaveData.Ordstatus = msgDataExcecType.OrdStatus;
                    objSaveData.Orderid = msgDataExcecType.OrderID;
                    objSaveData.Clordid = msgDataExcecType.ClOrdID;
                    objSaveData.Symbol = msgDataExcecType.Symbol;
                    objSaveData.Side = msgDataExcecType.Side;
                    objSaveData.Orderqty = msgDataExcecType.OrderQty;
                    objSaveData.Ordtype = msgDataExcecType.OrdType;
                    objSaveData.Price = msgDataExcecType.Price;
                    objSaveData.Account = msgDataExcecType.Account;
                    objSaveData.Settlvalue = msgDataExcecType.SettlValue;
                    objSaveData.Leavesqty = msgDataExcecType.LeavesQty;
                    objSaveData.Origclordid = msgDataExcecType.OrigClOrdID;
                    objSaveData.Lastqty = 0;
                    objSaveData.Lastpx = 0;
                    objSaveData.Execid = "";
                    objSaveData.Reciprocalmember = "";
                    //
                    objSaveData.Underlyinglastqty = "";
                    //
                    objSaveData.Exectype = ExecutionReportType.ER_CancelOrder_4.ToString();
                }
                else if (msgData.ExecType == ExecutionReportType.ER_ReplaceOrder_5) // 150 = 5
                {
                    MessageER_OrderReplace msgDataExcecType = (MessageER_OrderReplace)fixMessageBase;
                    //
                    objSaveData.Ordstatus = msgDataExcecType.OrdStatus;
                    objSaveData.Orderid = msgDataExcecType.OrderID;
                    objSaveData.Clordid = msgDataExcecType.ClOrdID;
                    objSaveData.Symbol = msgDataExcecType.Symbol;
                    objSaveData.Side = msgDataExcecType.Side;
                    objSaveData.Orderqty = msgDataExcecType.OrderQty;
                    objSaveData.Ordtype = msgDataExcecType.OrdType;
                    objSaveData.Price = msgDataExcecType.Price;
                    objSaveData.Account = msgDataExcecType.Account;
                    objSaveData.Settlvalue = msgDataExcecType.SettlValue;
                    objSaveData.Leavesqty = msgDataExcecType.LeavesQty;
                    objSaveData.Origclordid = msgDataExcecType.OrigClOrdID;
                    objSaveData.Lastqty = msgDataExcecType.LastQty;
                    objSaveData.Lastpx = msgDataExcecType.LastPx;
                    objSaveData.Execid = "";
                    objSaveData.Reciprocalmember = "";
                    //
                    objSaveData.Underlyinglastqty = "";
                    //
                    objSaveData.Exectype = ExecutionReportType.ER_ReplaceOrder_5.ToString();
                }
                else if (msgData.ExecType == ExecutionReportType.ER_Order_0) // 150 = 0
                {
                    MessageER_Order msgDataExcecType = (MessageER_Order)fixMessageBase;
                    //
                    objSaveData.Ordstatus = msgDataExcecType.OrdStatus;
                    objSaveData.Orderid = msgDataExcecType.OrderID;
                    objSaveData.Clordid = msgDataExcecType.ClOrdID;
                    objSaveData.Symbol = msgDataExcecType.Symbol;
                    objSaveData.Side = msgDataExcecType.Side;
                    objSaveData.Orderqty = msgDataExcecType.OrderQty;
                    objSaveData.Ordtype = msgDataExcecType.OrdType;
                    objSaveData.Price = msgDataExcecType.Price;
                    objSaveData.Account = msgDataExcecType.Account;
                    objSaveData.Settlvalue = msgDataExcecType.SettlValue;
                    objSaveData.Leavesqty = 0;
                    objSaveData.Origclordid = "";
                    objSaveData.Lastqty = 0;
                    objSaveData.Lastpx = 0;
                    objSaveData.Execid = "";
                    objSaveData.Reciprocalmember = "";
                    //
                    objSaveData.Underlyinglastqty = "";
                    //
                    objSaveData.Exectype = ExecutionReportType.ER_Order_0.ToString();
                }
                else if (msgData.ExecType == ExecutionReportType.ER_Rejected_8) // 150 = 8
                {
                    MessageER_OrderReject msgDataExcecType = (MessageER_OrderReject)fixMessageBase;
                    //
                    objSaveData.Ordstatus = msgDataExcecType.OrdStatus;
                    objSaveData.Orderid = msgDataExcecType.OrderID;
                    objSaveData.Clordid = msgDataExcecType.ClOrdID;
                    objSaveData.Symbol = msgDataExcecType.Symbol;
                    objSaveData.Side = msgDataExcecType.Side;
                    objSaveData.Orderqty = msgDataExcecType.OrderQty;
                    objSaveData.Ordtype = msgDataExcecType.OrdType;
                    objSaveData.Price = msgDataExcecType.Price;
                    objSaveData.Account = msgDataExcecType.Account;
                    objSaveData.Settlvalue = msgDataExcecType.SettlValue;
                    objSaveData.Leavesqty = 0;
                    objSaveData.Origclordid = "";
                    objSaveData.Lastqty = 0;
                    objSaveData.Lastpx = 0;
                    objSaveData.Execid = "";
                    objSaveData.Reciprocalmember = "";
                    objSaveData.Underlyinglastqty = msgDataExcecType.UnderlyingLastQty.ToString();

                    //
                    objSaveData.Exectype = ExecutionReportType.ER_Rejected_8.ToString();
                    //
                }
                //
                objSaveData.Ordrejreason = msgData.RejectReason.ToString();
                //
                objSaveData.Remark = "";
                objSaveData.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                objSaveData.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);

                _return = Msg_tprl_hnx_confirmBL.Insert(objSaveData);
                if (_return > 0)
                {
                    Logger.log.Info($"ProcessSaveMsgType | MsgSeqNum: {objSaveData.Msgseqnum}, MsgType: {objSaveData.Msgtype}, ExecType={objSaveData.Exectype},  -->>> Save to DB sucess");
                    //break;
                }
                else
                {
                    Logger.log.Error($"Error process ProcessSaveMsgType with MsgSeqNum: {objSaveData.Msgseqnum}, MsgType: {objSaveData.Msgtype}, ExecType={objSaveData.Exectype}, Error [{_return}] can't save to DB; Object={System.Text.Json.JsonSerializer.Serialize(objSaveData)} ");
                    //Thread.Sleep(ConfigData.TimeDelaySaveDB);
                }
                //}
                Logger.log.Info($"End process ProcessSaveMsgType_35_8 with sequence= {sequence}, MsgType(35)={fixMessageBase.GetMsgType}");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error process ProcessSaveMsgType_35_8 with sequence= {sequence}, Exception: {ex?.ToString()}");
            }
        }

        #endregion Xử lý: lưu vào DB
    }

    public class ValueSaveEntry
    {
        public FIXMessageBase fixMessageBaseData;
        public string typeMsgIsSendOrRecei;
    }
}