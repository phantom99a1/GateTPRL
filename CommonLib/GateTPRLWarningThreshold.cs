//class trả về thông tin cho bên core khi gọi vào api lấy thông tin thông báo khi vượt ngưỡng, hệ thống gặp lỗi
namespace CommonLib
{
	public class GateTPRLWarningThreshold
	{
		public int MaxSeqBusinessSend { get; set; } = 0;

		public int SeqBusinessAchieve { get; set; } = 0;

		public double Threshold { get; set; } = 0;

        public string Description { get; set; } = string.Empty;

		public string ProcessingTime { get; set; } = string.Empty;
    }
}
