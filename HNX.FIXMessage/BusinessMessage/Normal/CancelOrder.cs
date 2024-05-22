namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageCancelOrder : FIXMessageBase
    {
        #region fields

        public string ClOrdID; // Tag 11
        public string OrigClOrdID; // Tag 41
        public string Symbol; // Tag 55
        public string OrderNo { get; set; } = ""; // Dùng để lưu OrderNo khi push queue save db

        #endregion fields

        public MessageCancelOrder()
            : base()
        {
            MsgType = MessageType.CancelOrder;
        }
    }
}