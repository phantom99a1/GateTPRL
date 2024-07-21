namespace APIMonitor.Models
{
    public class KafkaSystemConnectModel
    {
        public string IP { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string TopicName_OrderStatus { get; set; } = string.Empty;
        public string TopicName_OrderExecution { get; set; } = string.Empty;
        public string TopicName_TradingInfomation { get; set; } = string.Empty;
        public int NumberMessageSend { get; set; }
        public string EnableKafka { get; set; } = string.Empty;
    }
}