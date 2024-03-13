using System.Net;

namespace HNXInterface
{
    public class PeerInfo
    {
        private IPAddress _IPAddress;
        private string _PeerName;
        private int _Port;

        public IPAddress IPAddress
        {
            get
            {
                return _IPAddress;
            }
            set
            {
                _IPAddress = value;
            }
        }

        public int Port
        {
            get
            {
                return _Port;
            }
            set
            {
                _Port = value;
            }
        }
    }
}
