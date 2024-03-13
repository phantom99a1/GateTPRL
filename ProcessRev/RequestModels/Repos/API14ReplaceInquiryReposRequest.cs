namespace BusinessProcessAPIReq.RequestModels
{
    /// <summary>
    /// 2.2	API14 – Sửa lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
    /// </summary>
    public class API14ReplaceInquiryReposRequest
    {
        public string OrderNo { get; set; } = string.Empty;
        public string RefExchangeID { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public int QuoteType { get; set; }
        public string OrderType { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
        public long OrderValue { get; set; }
        public string EffectiveTime { get; set; } = string.Empty;
        public int SettleMethod { get; set; }
        public string SettleDate1 { get; set; } = string.Empty;
        public string SettleDate2 { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public int RepurchaseTerm { get; set; }
        public string RegistID { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}