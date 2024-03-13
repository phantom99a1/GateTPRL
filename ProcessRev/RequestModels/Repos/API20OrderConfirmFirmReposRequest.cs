namespace BusinessProcessAPIReq.RequestModels
{
    /// <summary>
    /// 2.8	API20 – Xác nhận lệnh điện tử tùy chọn Firm Repos
    /// </summary>
    public class API20OrderConfirmFirmReposRequest
    {
        public string OrderNo { get; set; } = string.Empty;
        public string RefExchangeID { get; set; } = string.Empty;
        public int QuoteType { get; set; }
        public string OrderType { get; set; } = string.Empty;
        public string ClientID { get; set; } = string.Empty;
        public string ClientIDCounterFirm { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public double RepurchaseRate { get; set; }
		public long NoSide { get; set; }
		public List<APIReposSideList> SymbolFirmInfo { get; set; }
	}
}