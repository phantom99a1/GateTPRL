namespace BusinessProcessAPIReq.RequestModels
{
    /// <summary>
    /// 2.4	API16 – Đóng lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
    /// </summary>
    public class API16CloseInquiryReposRequest
    {
        public string OrderNo { get; set; } = string.Empty;
        public string RefExchangeID { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public int QuoteType { get; set; }
        public string OrderType { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}