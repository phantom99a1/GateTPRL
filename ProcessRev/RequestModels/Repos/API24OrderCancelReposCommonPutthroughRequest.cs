namespace BusinessProcessAPIReq.RequestModels
{
    /// <summary>
    /// 2.12	API24 – Hủy lệnh thỏa thuận báo cáo giao dịch Repos chưa thực hiện
    /// </summary>
    public class API24OrderCancelReposCommonPutthroughRequest
    {
        public string OrderNo { get; set; } = string.Empty;
        public string RefExchangeID { get; set; } = string.Empty;
        public int QuoteType { get; set; }
        public string OrderType { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}