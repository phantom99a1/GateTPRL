using HNX.FIXMessage;

namespace APIMonitor.Models
{
    public class DataMemModel
    {
        public List<MessageReject> ListAllMsgRejectOnMemory { get; set; } = new();

		public List<MessageReject> ListDisplayMsgRejectOnMemory { get; set; } = new();

        public List<MessageSecurityStatus> ListSearchSecurities { get; set; } = new();

		public List<MessageSecurityStatus> ListDisplaySecurities { get; set; } = new();

		public int? PageIndexRejection { get; set; }

		public int? PageIndexMaxRejection { get; set; }

		public int? PageIndexSecurities { get; set; }

		public int? PageIndexMaxSecurities { get; set; }
	}
}
