using APIMonitor.Models;

namespace APIMonitor.ObjectInfo
{
    public class BoxConnectModel
    {
        public HNXSystemConnectModel _HNXSystemConnect { get; set; }
        public KafkaSystemConnectModel _KafkaSystemConnect { get; set; }

        public string Session { get; set; } = string.Empty;
    }
}