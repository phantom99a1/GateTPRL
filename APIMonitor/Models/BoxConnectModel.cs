using APIMonitor.Models;
using CommonLib;

namespace APIMonitor.ObjectInfo
{
    public class BoxConnectModel
    {
        public HNXSystemConnectModel _HNXSystemConnect { get; set; }
        public KafkaSystemConnectModel _KafkaSystemConnect { get; set; }

        public DataMemModel? DataMem { get; set; }

        public ApplicationErrorModel? ApplicationError { get; set; }

        public string Session { get; set; } = string.Empty;
    }
}