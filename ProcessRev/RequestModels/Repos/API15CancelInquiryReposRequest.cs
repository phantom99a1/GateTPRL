namespace BusinessProcessAPIReq.RequestModels
{
    /// <summary>
    /// 2.3	API15 – Hủy lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
    /// </summary>
    public class API15CancelInquiryReposRequest
    {
        public string OrderNo { get; set; } = string.Empty;
        public string RefExchangeID { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public int QuoteType { get; set; }
        public string OrderType { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}