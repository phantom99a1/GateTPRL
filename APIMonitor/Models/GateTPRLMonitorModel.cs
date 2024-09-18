namespace APIMonitor.Models
{
	public class GateTPRLMonitorModel
	{
        public int CoreSendMessageNum { get; set; } = 0;

        public int CoreRevMessageNum { get; set; } = 0;

        public int CoreQueueMessageNum { get; set; } = 0;

        public int ExchangeSendMessageNum { get; set; } = 0;

        public int ExchangeRevMessageNum { get; set; } = 0;

        public int ExchangeQueueMessageNum { get; set; } = 0;
    }
}
