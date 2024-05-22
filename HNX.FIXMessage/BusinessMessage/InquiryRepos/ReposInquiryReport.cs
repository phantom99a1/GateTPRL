namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageReposInquiryReport : FIXMessageBase
    {
        #region fields

        public string ClOrdID; // Tag 11
        public string QuoteID; // Tag 117
        public string RFQReqID; //Tag 644
        public int QuoteType; // tag 537
        public string OrdType; // Tag 40
        public string OrderPartyID; // Tag 4488

        public int Side;   // Tag 54: 1 = Buy, 2 = Sell
        public string EffectiveTime; // Tag 168
        public int RepurchaseTerm; //Tag 226
        public int SettlMethod;    // Tag 6363
        public string RegistID; //Tag 513

        public string SettlDate; // Tag 64
        public string SettlDate2; //Tag 193
        public string EndDate; //Tag 917

        public double OrderQty; //Tag 38
        public string Symbol; //Tag 55
        public string OrderNo { get; set; } = ""; // Dùng để lưu OrderNo khi push queue save db

        #endregion fields

        public MessageReposInquiryReport()
            : base()
        {
            MsgType = MessageType.ReposInquiryReport;
        }
    }
}