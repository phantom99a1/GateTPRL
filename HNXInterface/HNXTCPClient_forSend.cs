using CommonLib;
using HNX.FIXMessage;
using LocalMemory;
using StorageProcess;
using static CommonLib.CommonDataInCore;

namespace HNXInterface
{
    public partial class HNXTCPClient : iHNXClient, IDisposable
    {
        private object objLockMsgSend = new object();
        private int LastSeqMap = 0;

        #region Các hàm nghiệp vụ

        private void SendLogin()
        {
            //hàm để send Login
            MessageLogon msgLogon = new MessageLogon();
            msgLogon.Username = ConfigData.Username;
            msgLogon.Password = ConfigData.Password;
            msgLogon.HeartBtInt = ConfigData.Heartbeat;
            SendToExchange(msgLogon);
        }

        private void SendTestquest()
        {
            //hàm để send Testquest
            MessageTestRequest msgTestRequest = new MessageTestRequest()
            {
                TestReqID = DateTime.Now.Ticks.ToString(),
            };
            SendToExchange(msgTestRequest);
        }

        private void Send_ResendRequest(int fromSeq, int toSeq)
        {
            MessageResendRequest resendRequest = new MessageResendRequest();
            resendRequest.BeginSeqNo = fromSeq;
            resendRequest.EndSeqNo = toSeq;
            __ClientStatus = enumClientStatus.RESEND_REQUEST;
            SendToExchange(resendRequest);
        }

        private void SendLogout()
        {
            MessageLogout msgLogout = new MessageLogout();
            SendToExchange(msgLogout);
            Thread.Sleep(1000);
            GateSeqInfo.CLose();
        }

        #endregion Các hàm nghiệp vụ

        #region Các hàm chức năng

        //Hàm này chỉ được gửi đi khi đang ở trạng thái Data Transfer
        private bool SendToExchange(FIXMessageBase Message)
        {
            try
            {
                Message.SenderCompID = ConfigData.TraderID;
                Message.TargetCompID = Common.HNX_TargetCompID;
                Message.TargetSubID = Common.HNX_TargetSubID;
                LastSeqMap = (Message.MsgSeqNum > LastSeqMap) ? Message.MsgSeqNum : LastSeqMap;
                lock (objLockMsgSend)
                {
                    Message.LastMsgSeqNumProcessed = GateSeqInfo.LastCliProcessSeq;
                    if (c_MsgFactoryFix.IsAdminitrativeMessage(Message))
                    {
                        Message.MsgSeqNum = GateSeqInfo.CliSeq;
                    }
                    else
                    {
                        if (GateSeqInfo.LastSerProcessSeq + ConfigData.SafeWindowSize < GateSeqInfo.CliSeq && !c_MsgFactoryFix.IsAdminitrativeMessage(Message))
                        {
                            Logger.HNXTcpLog.Warn("HNX Last Process Seq + {0} < Gate Seq, out of Safe windowsize", ConfigData.SafeWindowSize);
                            return false; // return luôn. cho retry ở lần kế tiếp
                        }
                        Message.MsgSeqNum = GateSeqInfo.CliSeq + 1;
                        c_BackupData.AddtoBackup(Message.MsgSeqNum, Message);
                    }

                    c_MsgFactoryFix.Build(Message);
                    c_CurrentConnected.WriteByte(Message.MessageRawByte);
                    //

                    //
                    ShareMemoryData.c_FileStore.StoreSendMsg(Message.GetMsgType, Message.GetMessageRaw, Message.MsgSeqNum, Message.LastMsgSeqNumProcessed, GateSeqInfo.SerSeq, GateSeqInfo.LastSerProcessSeq, LastSeqMap);
                    //

                    // BacND: bổ sung thêm ghi vào DB sau khi gửi sở và save file xong
                    SharedStorageProcess.c_DataStorageProcess.EnqueueData(Message, Data_SoR.Send);

                    //
                    GateSeqInfo.Set_CliSeq(Message.MsgSeqNum);
                    _lasttimeKeapAlive = DateTime.Now.Ticks;
                    if (Message.TimeInit != 0)
                    {
                        LastProcessedTime = _lasttimeKeapAlive - Message.TimeInit;
                        Logger.HNXTcpLog.Info("Time to process message {0} from Api {1} in {2} ns", Message.GetMsgType, Message.APIBussiness, LastProcessedTime);
                        AverageTimeProcess = (AverageTimeProcess * TimesSended++ + LastProcessedTime) / (TimesSended);
                        if (MinOfLast100ProcessTime > LastProcessedTime) MinOfLast100ProcessTime = LastProcessedTime;
                        if (MaxOfLast100ProcessTime < LastProcessedTime) MaxOfLast100ProcessTime = LastProcessedTime;
                        if (TimesSended % 100 == 0)
                        {
                            MinOfLast100ProcessTime = _lasttimeKeapAlive;
                            MaxOfLast100ProcessTime = 0;
                            Logger.HNXTcpLog.Info("Average time to Process Last 100 message: {0} us", LastProcessedTime / 10);
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.HNXTcpLog.Error(ex);
                return false;
            }
        }

        public virtual bool Send2HNX(FIXMessageBase fMsgBase)
        {
            if (__ClientStatus == enumClientStatus.DATA_TRANSFER)
                return SendToExchange(fMsgBase);
            else
                return false;
        }

        #endregion Các hàm chức năng

        #region Các chức năng hàm cho Monitor

        //Gửi Request phiên
        public bool SendTradingSessionRequest(string tradingCode, string tradingName)
        {
            MessageTradingSessionStatusRequest sessionStatusRequest = new MessageTradingSessionStatusRequest();
            if (tradingCode == TradingCode.AllSymbol)
            {
                sessionStatusRequest.TradSesMode = 2;
                sessionStatusRequest.TradSesReqID = "0";
            }
            else if (tradingCode == TradingCode.AllTable)
            {
                sessionStatusRequest.TradSesMode = 1;
                sessionStatusRequest.TradSesReqID = "0";
            }
            else if (tradingCode == TradingCode.WithTable)
            {
                sessionStatusRequest.TradSesMode = 1;
                sessionStatusRequest.TradSesReqID = tradingName;
            }
            else if (tradingCode == TradingCode.WithSymbol)
            {
                sessionStatusRequest.TradSesMode = 2;
                sessionStatusRequest.TradSesReqID = tradingName;
            }
            return Send2HNX(sessionStatusRequest);
        }

        //Gửi Request Mã chứng khoán
        public bool SendSecurityStatusRequest(string tradingCode, string Symbol)
        {
            MessageSecurityStatusRequest messageSecurityStatus = new MessageSecurityStatusRequest();
            if (tradingCode == TradingCode.AllSymbol)
            {
                messageSecurityStatus.Symbol = "";
                messageSecurityStatus.SubscriptionRequestType = '0';
                messageSecurityStatus.SecurityStatusReqID = DateTime.Now.ToString("hhmmss");
            }
            else if (tradingCode == TradingCode.WithSymbol)
            {
                messageSecurityStatus.Symbol = Symbol;
                messageSecurityStatus.SubscriptionRequestType = '0';
                messageSecurityStatus.SecurityStatusReqID = DateTime.Now.ToString("hhmmss");
            }

            return Send2HNX(messageSecurityStatus);
        }

        public bool SendUserRequest(string userName,string oldPass, string newPass)
        {
            MessageUserRequest messageUserRequest = new MessageUserRequest();

            messageUserRequest.UserRequestID = DateTime.Now.ToString("hhmmss");
            messageUserRequest.UserRequestType = 3;
            messageUserRequest.Username = userName;
            messageUserRequest.Password = oldPass;
            messageUserRequest.NewPassword = newPass;

            return Send2HNX(messageUserRequest);
        }

        #endregion Các chức năng hàm cho Monitor
    }
}