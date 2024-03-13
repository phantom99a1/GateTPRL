namespace BusinessProcessAPIReq.RequestModels
{
    /// <summary>
    /// 2.7	API19 – Hủy lệnh điện tử tùy chọn Firm Repos chưa thực hiện
    /// </summary>
    public class API19OrderCancelFirmReposRequest
    {
        public string OrderNo { get; set; } = string.Empty;
        public string RefExchangeID { get; set; } = string.Empty;
        public int QuoteType { get; set; }
        public string OrderType { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}