//class trả về thông tin cho bên core khi gọi vào api lấy thông tin thông báo khi vượt ngưỡng, hệ thống gặp lỗi
namespace CommonLib
{
	public class GateTPRLWarningThreshold
	{
		public double MaxSeqBusinessSendDay { get; set; }

		public double MaxSeqBusinessSendSession { get; set; }

		public double SeqBusinessAchieve { get; set; }

		public string ThresholdSession { get; set; } = string.Empty;

		public string ThresholdDay { get; set; } = string.Empty;

        public string DescriptionSession { get; set; } = string.Empty;

        public string DescriptionDay { get; set; } = string.Empty;

		public string ProcessingTime { get; set; } = string.Empty;

		public string Session { get; set; } = string.Empty;
	}
}
