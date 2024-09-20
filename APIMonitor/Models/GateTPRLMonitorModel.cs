namespace APIMonitor.Models
{
	public class GateTPRLMonitorModel
	{        
        public int ExchangeSendMessageNum { get; set; } = 0;

        public int ExchangeRevMessageNum { get; set; } = 0;

        public int ExchangeQueueMessageNum { get; set; } = 0;

        public string TradingStatus { get; set; } = string.Empty;

        public string TradingSession { get; set; } = string.Empty;
    }
}
