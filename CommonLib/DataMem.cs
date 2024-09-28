//2024.09.10
//Class lưu các  List, Object sử dụng cho phần ITMOnitor
 
using HNX.FIXMessage;
using ObjectInfo;

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
        /// Số msg Bussiness đã gửi lên Sở 
        /// Phục vụ cho ITMonitor giám sát ngưỡng giao dịch.
        /// </summary>
        public static int NumMsgSend { get; set; } = 0;

        /// <summary>
        /// Thời gian gửi msg  gần nhất
        /// </summary>
        public static DateTime lastTimeMsgSend { get; set; } = DateTime.Now;

        public static object lockObj = new object();

        /// <summary>
        /// Lưu thông tin cảnh báo cho đơn vị vận hành khi vượt ngưỡng, hệ thống gặp lỗi
        ///  Mục đích để sử dụng cho bên Core gọi vào sẽ nhận được thông tin
        /// </summary>
        public static GateTPRLWarningThreshold? warningThreshold;
    }
}
