using HNX.FIXMessage;

namespace CommonLib
{
    public class DataMem
    {
        /// <summary>
        /// List lưu toàn bộ danh sách các msg reject từ Sở
        /// </summary>
        public static List<MessageReject> lstAllMsgRejectOnMemory = new();

        /// <summary>
        /// List lưu toàn bộ các securities
        /// </summary>
        public static List<MessageSecurityStatus> lstAllSecurities = new();
    }
}
