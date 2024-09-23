using BusinessProcessResponse;
using CommonLib;
using HNX.FIXMessage;
using LocalMemory;
using System.Net;
using System.Net.Sockets;

namespace HNXInterface
{
    public partial class HNXTCPClient : iHNXClient, IDisposable
    {
        private PeerInfo c_HNXPeerInfo = new PeerInfo();
        private bool c_IsAutoReconnect = true;
        public MyStream c_CurrentConnected;
        private BackupDataControl c_BackupData;
        private bool _HNXTCPClientStarted = false; // biến để xác định chỉ start 1 lần
        public enumClientStatus __ClientStatus;

        #region instanceSingleton

        private SeqInfo GateSeqInfo = new SeqInfo();

        private MessageFactoryFIX c_MsgFactoryFix = new MessageFactoryFIX();
        private long LastTimeRevMsg = DateTime.Now.Ticks;
        private long _lasttimeKeapAlive = DateTime.Now.Ticks;   //
        private int tradingSessionID = 0;
        private bool IsRequest = false;
        private long AverageTimeProcess = 0;
        private long TimesSended = 0;
        private long LastProcessedTime = 0;
        private long MaxOfLast100ProcessTime = 0;
        private long MinOfLast100ProcessTime = DateTime.Now.Ticks;

        public HNXTCPClient()
        {
        }

        public HNXTCPClient(IResponseInterface p_ResponseInterface)
        {
            try
            {
                c_ProcessRevHNX = new ProcessRevHNX(p_ResponseInterface);

                _HNXTCPClientStarted = false;
                c_HNXPeerInfo.IPAddress = IPAddress.Parse(CommonLib.ConfigData.IPServer);
                c_HNXPeerInfo.Port = CommonLib.ConfigData.PortServer;
                c_BackupData = new BackupDataControl(ConfigData.BackupCapacity);//Chỉ recover lại 100 message gần nhất
                CommonLib.Logger.HNXTcpLog.Info("Create SingletonInstance SendMWInterface");
            }
            catch (Exception ex)
            {
                CommonLib.Logger.HNXTcpLog.Error(ex);
            }
        }

        #endregion instanceSingleton

        public void Logout()
        {
            SendLogout();
        }

        public enumClientStatus ClientStatus()
        {
            return __ClientStatus;
        }

        public int Seq()
        {
            return GateSeqInfo.CliSeq;
        }

        public int LastSeqProcess()
        {
            return GateSeqInfo.LastCliProcessSeq;
        }

        public int ChangeSeq(int value)
        {
            GateSeqInfo.Set_CliSeq(value);
            return GateSeqInfo.CliSeq;
        }

        public int ChangeLastSeqProcess(int value)
        {
            GateSeqInfo.Set_LastCliProcess(value);
            return GateSeqInfo.LastCliProcessSeq;
        }

        public int TradingSessionID()
        {
            return tradingSessionID;
        }

       /* public void ChangeSeq(int Seq)
        {
            GateSeqInfo.Set_CliSeq(Seq);
        }*/

        // 01.03.2024 bacnd rem lai k thay dung de giam unit test
        //public void ChangeLastSeqProcess(int LasSeqProcess)
        //{
        //    GateSeqInfo.Set_LastCliProcess(LasSeqProcess);
        //}

        public bool StartHNXTCPClient()
        {
            try
            {
                if (_HNXTCPClientStarted == false)
                {
                    CommonLib.Logger.log.Info("HNXTCPClient Starting...");
                    //Create thread to Connect anh keep connect to HNX
                    Thread _ThreadKeapAlive = new Thread(() => ThreadKeapAlive());
                    _ThreadKeapAlive.Name = "ThreadKeapAlive";
                    _ThreadKeapAlive.IsBackground = true;
                    _ThreadKeapAlive.Start();

                    //Create a thread to read data from the socket
                    c_IsAutoReconnect = true;
                    Thread _ThreadReadSocketData = new Thread(() => ThreadReadSocketData());
                    _ThreadReadSocketData.Name = "ThreadReadSocketData";
                    _ThreadReadSocketData.IsBackground = true;
                    _ThreadReadSocketData.Start();

                    return true;
                }
                else
                {
                    CommonLib.Logger.log.Info("HNXTCPClient Started !!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                CommonLib.Logger.log.Error("StartHNXTCPClient Fail |ex:{0}", ex.ToString());

                return false;
            }
        }

        public void StopCurrentConnected(string p_Reason)
        {
            __ClientStatus = enumClientStatus.DISCONNECT;
            if (c_CurrentConnected != null)
            {
                CommonLib.Logger.HNXTcpLog.Info("StopCurrentConnected  Connectedkey:{0}  on {1}:{2} - Reason: {3}", c_CurrentConnected.streamkey, c_CurrentConnected.LocalIP, c_CurrentConnected.LocalPort, p_Reason);

                c_CurrentConnected.CloseStream();
            }
            else
            {
                CommonLib.Logger.HNXTcpLog.Info("StopCurrentConnected Is null - Reason: {0} ", p_Reason);
            }
        }

        //2024/09/23 thêm hàm xử lý phần reset seq
        public void ResetSequence(int sequence, int lastProcessSequence)
        {
            GateSeqInfo.Set_CliSeq(sequence);
            GateSeqInfo.Set_SerSeq(lastProcessSequence);
            GateSeqInfo.Set_LastCliProcess(sequence);
            GateSeqInfo.Set_LastSerProcess(lastProcessSequence);
        }

        private void ThreadReadSocketData()
        {
            CommonLib.Logger.log.Info($"START ExecuteClientSocketThread ManagedThreadId:{Thread.CurrentThread.ManagedThreadId.ToString()}");
            string _strMsg = null;
            while (true)
            {
                try
                {
                    if (c_CurrentConnected != null && c_CurrentConnected.isAvaiable)
                    {
                        _strMsg = c_CurrentConnected.ReadString();

                        if (_strMsg != null && _strMsg != "off")
                        {
                            LastTimeRevMsg = DateTime.Now.Ticks;
                            string gMsgType = "";
                            ProcessRevMessage(_strMsg, ref gMsgType);
                            ShareMemoryData.c_FileStore.StoreRecvMsg(gMsgType, _strMsg, GateSeqInfo.CliSeq, GateSeqInfo.LastCliProcessSeq, GateSeqInfo.SerSeq, GateSeqInfo.LastSerProcessSeq);
                        }
                        else if (_strMsg == "off")
                        {
                            StopCurrentConnected("Time out");
                        }
                        else
                        {
                            Thread.Sleep(1);
                        }
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    CommonLib.Logger.log.Error(ex, "Read data socket or process message error <Message: {0}, Error: {1}>", _strMsg, ex.Message);
                    Thread.Sleep(1000);
                }
            }
        }

        private void ThreadKeapAlive()
        {
            CommonLib.Logger.log.Info($"START ThreadKeapAlive ManagedThreadId:{Thread.CurrentThread.ManagedThreadId.ToString()}");

            long _KeapAliveInterval = CommonLib.ConfigData.Heartbeat * TimeSpan.TicksPerSecond;
            while (true)
            {
                try
                {
                    if (__ClientStatus == enumClientStatus.DATA_TRANSFER && c_CurrentConnected != null && c_CurrentConnected.isAvaiable)
                    {
                        if (DateTime.Now.Ticks - _lasttimeKeapAlive >= _KeapAliveInterval)
                        {
                            SendTestquest();
                            continue;
                        }

                        //2024.09.17 SangDD add thêm theo yc phi chức năng của MBS
                        //Thêm đoạn  GetStockinfo trong config default = true
                        if (ConfigData.GetStockinfo == true)
                        {

                            if (!IsRequest)
                            {
                                // tức là chưa có gì, gửi request thông tin chứng khoán lên
                                MessageSecurityStatusRequest _msg_e = new MessageSecurityStatusRequest();
                                _msg_e.Symbol = "";
                                _msg_e.SecurityStatusReqID = DateTime.Now.ToString("HHmmss");
                                _msg_e.SenderCompID = CommonLib.ConfigData.TraderID;
                                _msg_e.TargetCompID = Common.HNX_TargetCompID;
                                _msg_e.TargetSubID = Common.HNX_TargetSubID;
                                SendToExchange(_msg_e);

                                MessageTradingSessionStatusRequest _msg_g = new MessageTradingSessionStatusRequest();
                                _msg_g.TradSesReqID = "0"; //lấy tất các bảng, cho nó đơn giản
                                _msg_g.SubscriptionRequestType = '2';
                                _msg_g.TradSesMode = 1;//lấy thông tin phiên theo bảng
                                SendToExchange(_msg_g);
                                IsRequest = true;
                            }
                        }
                        //

                    }
                    else if (__ClientStatus == enumClientStatus.DISCONNECT || c_CurrentConnected == null || !c_CurrentConnected.isAvaiable)
                    {
                        if (c_IsAutoReconnect)
                            TCPConnect();
                    }
                    Thread.Sleep(1000);
                    if (DateTime.Now.Ticks - LastTimeRevMsg > 3 * _KeapAliveInterval)
                    {
                        LastTimeRevMsg = DateTime.Now.Ticks;
                        StopCurrentConnected("KeapAlive detected TIME OUT");
                    }
                }
                catch (Exception ex)
                {
                    CommonLib.Logger.HNXTcpLog.Error(ex.ToString());
                    Thread.Sleep(1000);
                }
            }
        }

        private void TCPConnect()
        {
            try
            {
                CommonLib.Logger.HNXTcpLog.Info($"Start Connect");

                TcpClient _TcpClient = new TcpClient();
                _TcpClient.NoDelay = true;
                _TcpClient.Connect(c_HNXPeerInfo.IPAddress, c_HNXPeerInfo.Port);
                if (_TcpClient.Connected)
                {

                    c_CurrentConnected = new MyStream(_TcpClient, DateTime.Now.Ticks.ToString());

                    CommonLib.Logger.HNXTcpLog.Info("Connected  Connectedkey:{0}  on {1}:{2}", c_CurrentConnected.streamkey, c_CurrentConnected.LocalIP, c_CurrentConnected.LocalPort);

                    __ClientStatus = enumClientStatus.CONNECTED;
                    //Kết nối thành công thì send message Login
                    SendLogin();
                    __ClientStatus = enumClientStatus.HANDSHAKING;
                }
                else
                {
                    //Không kết nối thì giải phóng nó đi.
                    _TcpClient.Dispose();
                }    
            }
            catch (Exception ex)
            {
                //đoạn này còn cần check là quá bao nhiều lần connect fail thì thông báo
                StopCurrentConnected("Connect to Server Fail"); // stop CurrentConnected giả sử nó đã khởi động đc
                CommonLib.Logger.HNXTcpLog.Error("Connect to Server Fail IPServer:{0}, PortServer:{1}| ex:{2}", c_HNXPeerInfo.IPAddress, c_HNXPeerInfo.Port, ex.ToString());
                return;
            }
        }

        public void Dispose()
        {
            ShareMemoryData.c_FileStore.Dispose();
            c_CurrentConnected.Dispose();
        }
    }

    public enum enumClientStatus
    {
        DISCONNECT,                 //bị ngắt kết nối,
        CONNECTED,         //Kết nối thành công,
        HANDSHAKING,          // đang bắt tay
        DATA_TRANSFER,       // đã bắt tay thành công, cho phép gửi dữ liệu
        PROCESS_RESEND,         // Xử lý khi Resend
        RESEND_REQUEST,
        SEND_LOGOUT,
        ACCEPT_LOGOUT,
        RECEIVE_LOGOUT
    }
}