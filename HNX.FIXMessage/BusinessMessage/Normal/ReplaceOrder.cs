namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageReplaceOrder : FIXMessageBase
    {
        #region fields

        public string ClOrdID; // Tag 11
        public string OrigClOrdID; // Tag 41
        public string Account; // Tag 1
        public string Symbol; // Tag 55
        public long OrderQty; // Tag 38
        public long OrgOrderQty; // Tag 2238
        public long Price2; // Tag 640
        //

        #endregion fields

        public MessageReplaceOrder()
            : base()
        {
            MsgType = MessageType.ReplaceOrder;
        }
    }
}