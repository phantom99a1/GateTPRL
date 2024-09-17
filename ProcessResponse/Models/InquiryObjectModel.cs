namespace BusinessProcessResponse
{
    public class InquiryObjectModel
    {
        public string MsgType { get; set; } = "";
        public string OrderNo { get; set; } = "";
        public string ExchangeID { get; set; } = "";
        public string RefExchangeID { get; set; } = "";
        public string QuoteType { get; set; } = "";
        public string OrdType { get; set; } = "";
        public string OrderStatus { get; set; } = "";
        public string OrderPartyID { get; set; } = "";
        public string Side { get; set; } = "";
        public string EffectiveTime { get; set; } = "";
        public int RepurchaseTerm { get; set; } = 0;
        public int SettleMethod { get; set; } = 0;
        public string RegistID { get; set; } = "";
        public string SettleDate1 { get; set; } = "";
        public string SettleDate2 { get; set; } = "";
        public string EndDate { get; set; } = "";
        public long OrderValue { get; set; } = 0;
        public string Symbol { get; set; } = "";
        public string RejectReasonCode { get; set; } = "";
        public string RejectReason { get; set; } = "";
        public string Text { get; set; } = "";
        public string SendingTime { get; set; } = "";
        public int RefSeqNum { get; set; } = 0;
    }
}