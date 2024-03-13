using HNX.FIXMessage;
using HNXInterface;

namespace UTInputData.Mock
{
    public class MockiHNXEntity : iHNXClient
    {
        public bool StartHNXTCPClient()
        { return true; }

        public void StopCurrentConnected(string p_Reason)
        {
        }

        public bool Send2HNX(FIXMessageBase msg)
        { return true; }

        public enumClientStatus ClientStatus()
        { return enumClientStatus.DATA_TRANSFER; }

        public int Seq()
        { return 1; }

        public int LastSeqProcess()
        { return 0; }

        public void Logout()
        { }

        public int LastMapSeqProcess
        { get { return 0; } }

        public void Recovery()
        { }

        public bool SendTradingSessionRequest(string tradingCode, string tradingName)
        {
            return true;
        }

        public bool SendSecurityStatusRequest(string tradingCode, string Symbol)
        {
            if (Symbol == "BBB")
            { return false; }
            else if (Symbol == "CCC")
            {
                throw new Exception();
            }
            return true;
        }

        public bool SendUserRequest(string userName, string oldPass, string newPass)
        {
            return true;
        }

    }
}