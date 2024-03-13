namespace BusinessProcessResponse
{
    public class ExecOrderReposModel
    {
        public string MsgType { get; set; } = "";
        public string SendingTime { get; set; } = "";
        public string Text { get; set; } = "";
        public string OrderNo { get; set; } = "";
        public string OrderID { get; set; } = "";
        public string SellOrderID { get; set; } = "";
        public string BuyOrderID { get; set; } = "";
        public string Side { get; set; } = "";
        public string MemberCounterFirm { get; set; } = "";
        public double RepurchaseRate { get; set; } = 0;
        public int RepurchaseTerm { get; set; } = 0;
        public string SettleDate1 { get; set; } = "";
        public string SettleDate2 { get; set; } = "";
        public string EndDate { get; set; } = "";
        public long MatchReportType { get; set; }
        public long NoSide { get; set; }
        public List<ReposSideListExecOrderReposResponse> SymbolFirmInfo { get; set; }
    }

    public class ReposSideListExecOrderReposResponse
    {
        public long NumSide { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public long ExecQty { get; set; }
        public long ExecPx { get; set; }
        public long MergePrice { get; set; }
        public double ReposInterest { get; set; }
        public double HedgeRate { get; set; }
        public double SettleValue1 { get; set; }
        public double SettleValue2 { get; set; }
    }
}