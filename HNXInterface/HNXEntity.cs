using HNX.FIXMessage;

namespace HNXInterface
{
    public interface iHNXEntity
    {
        void StartHNXTCPClient();

        bool Send2HNX(FIXMessageBase msg);

        public void Logout();

        public enumClientStatus ClientStatus();

        public int Seq();

        public int LastSeqProcess();

        public bool SendSecurityStatusRequest(string Symbol);
        public bool SendTradingSessionRequest();
        
    }

    public class HNXEntity : iHNXEntity
    {
        public iHNXClient c_HNXTCPClient;

        public HNXEntity()
        {
            c_HNXTCPClient = new HNXTCPClient();
        }

        public bool Send2HNX(FIXMessageBase msg)
        {
            return c_HNXTCPClient.Send2HNX(msg);
        }

        public void StartHNXTCPClient()
        {
            c_HNXTCPClient.StartHNXTCPClient();
        }

        public void Logout()
        {
            if (c_HNXTCPClient.ClientStatus() == enumClientStatus.DATA_TRANSFER)
                c_HNXTCPClient.Logout();
        }

        public enumClientStatus ClientStatus()
        {
            return c_HNXTCPClient.ClientStatus();
        }
        #region Thông tin hiển thị cho monitor
        public int Seq()
        {
            return c_HNXTCPClient.Seq();
        }

        public int LastSeqProcess()
        {
            return c_HNXTCPClient.LastSeqProcess();
        }
       
        #endregion

        #region Các hàm điều khiển bằng monitor

        public bool SendSecurityStatusRequest(string Symbol)
        {
            return c_HNXTCPClient.SendSecurityStatusRequest(Symbol);
        }

        public bool SendTradingSessionRequest()
        {
            return c_HNXTCPClient.SendSessionRequest();
        }
     
        #endregion
    }
}