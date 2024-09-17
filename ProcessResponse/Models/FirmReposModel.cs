namespace BusinessProcessResponse
{
    public class FirmReposModel 
    {
        public string MsgType { get; set; } = "";
        public string OrderNo { get; set; } = "";
        public string RefMsgType { get; set; } = "";
        public string ExchangeID { get; set; } = "";
        public string RefExchangeID { get; set; } = "";
        public string QuoteType { get; set; } = "";
        public string OrdType { get; set; } = "";
        public string OrderStatus { get; set; } = "";
        public string OrderPartyID { get; set; } = "";
        public string InquiryMember { get; set; } = "";
        public string Side { get; set; } = "";
        public string EffectiveTime { get; set; } = "";
        public int RepurchaseTerm { get; set; } = 0;
        public double RepurchaseRate { get; set; } = 0;
        public string SettleDate1 { get; set; } = "";
        public string SettleDate2 { get; set; } = "";
        public string EndDate { get; set; } = "";
        public int SettleMethod { get; set; } = 0;
        public string ClientID { get; set; } = "";
        public string ClientIDCounterFirm { get; set; } = "";
        public string MemberCounterFirm { get; set; } = "";
        public long NoSide { get; set; }
        public List<ReposSideListResponse> SymbolFirmInfo { get; set; }
        public long MatchReportType { get; set; }
        public string RejectReasonCode { get; set; } = "";
        public string RejectReason { get; set; } = "";
        public string Text { get; set; } = "";
        public string SendingTime { get; set; } = "";
        public int RefSeqNum { get; set; } = 0;
    }

    public class ReposSideListResponse
    {
        public long NumSide { get; set; } = 0;
        public string Symbol { get; set; } = "";
        public long OrderQty { get; set; } = 0;
        public long ExecPrice { get; set; } = 0;
        public long MergePrice { get; set; } = 0;
        public double ReposInterest { get; set; } = 0;
        public double HedgeRate { get; set; } = 0;
        public double SettleValue1 { get; set; } = 0;
        public double SettleValue2 { get; set; } = 0;
    }
}