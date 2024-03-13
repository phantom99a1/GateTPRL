namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageReposInquiry : FIXMessageBase
    {
        #region fields

        public string ClOrdID; // Tag 11
        public string Symbol; // Tag 55
        public int QuoteType; // tag 537
        public string OrdType; // Tag 40
        public int Side;   // Tag 54: 1 = Buy, 2 = Sell
        public double OrderQty; // Tag 38
        public string EffectiveTime; // Tag 168
        public int SettlMethod;    // Tag 6363
        public string SettlDate ; // Tag 64 ngày thanh toán.
        public string SettlDate2 ; //Tag 193
        public string EndDate ; //Tag 917
        public int RepurchaseTerm; //Tag 226
        public string RegistID ; //Tag 513
        public string RFQReqID ; //Tag 644

        public bool IsAPI15_16 { get; set; } = false; // dùng để build bỏ tag

        #endregion fields

        public MessageReposInquiry()
            : base()
        {
            MsgType = MessageType.ReposInquiry;
        }
    }
}