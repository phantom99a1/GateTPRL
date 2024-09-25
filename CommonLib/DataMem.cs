using HNX.FIXMessage;

namespace CommonLib
{
    public class DataMem
    {
        /// <summary>
        /// List lưu toàn bộ danh sách các msg reject từ Sở
        /// Mục đích để sử dụng cho phần hiển thị ITMonitor khối Rejection
        /// </summary>
        public static List<MessageReject> lstAllMsgRejectOnMemory = new();

        /// <summary>
        /// List lưu toàn bộ các securities
        /// Mục đích để sử dụng cho phần hiển thị ITMonitor khối Securities
        /// </summary>
        public static List<MessageSecurityStatus> lstAllSecurities = new();

        /// <summary>
        /// Lấy ra số lượng message Exchange
        ///  Mục đích để sử dụng cho phần hiển thị ITMonitor ở mục  GateTPRLMonitor
        /// </summary>
        public static GateTPRLMonitorExchange gateTPRLMonitorExchange = new();

		/// <summary>
		/// Lưu list thông tin cảnh báo cho đơn vị vận hành khi vượt ngưỡng, hệ thống gặp lỗi
		///  Mục đích để sử dụng cho bên Core gọi vào sẽ nhận được thông tin
		/// </summary>
        public static List<GateTPRLWarningThreshold> lstGateTPRLWarningThreshold = new();
	}
}
