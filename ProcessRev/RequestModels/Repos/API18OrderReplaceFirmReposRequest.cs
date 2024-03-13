namespace BusinessProcessAPIReq.RequestModels
{
    /// <summary>
    /// 2.6	API18 – Sửa lệnh điện tử tùy chọn Firm Repos chưa thực hiện
    /// </summary>
    public class API18OrderReplaceFirmReposRequest
    {
        public string OrderNo { get; set; } = string.Empty;
        public string RefExchangeID { get; set; } = string.Empty;
        public int QuoteType { get; set; }
        public string OrderType { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
        public string ClientID { get; set; } = string.Empty;
        public string EffectiveTime { get; set; } = string.Empty;
        public int SettleMethod { get; set; }
        public string SettleDate1 { get; set; } = string.Empty;
        public string SettleDate2 { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public int RepurchaseTerm { get; set; }
        public double RepurchaseRate { get; set; }
        public string Text { get; set; } = string.Empty;
        public long NoSide { get; set; }
        public List<APIReposSideList> SymbolFirmInfo { get; set; } 
    }
}