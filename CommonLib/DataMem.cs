﻿using HNX.FIXMessage;

namespace CommonLib
{
    public class DataMem
    {
        /// <summary>
        /// List lưu toàn bộ danh sách các msg reject từ Sở
        /// Mục đích để sử dụng cho phần hiển thị ITMonitor
        /// </summary>
        public static List<MessageReject> lstAllMsgRejectOnMemory = new();

        /// <summary>
        /// List lưu toàn bộ các securities
        /// </summary>
        public static List<MessageSecurityStatus> lstAllSecurities = new();

        /// <summary>
        /// Lấy ra số lượng message Exchange
        /// </summary>
        public static GateTPRLMonitorExchange gateTPRLMonitorExchange = new(); 
    }
}
