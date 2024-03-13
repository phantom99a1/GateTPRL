namespace APIMonitor.Models
{
    public class HNXSystemConnectModel
    {
        public string IP { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Status { get; set; } = string.Empty;
        public string LoginStatus { get; set; } = string.Empty;
        public int Sequence { get; set; }
        public int LastSeqProcess { get; set; }
    }
}