namespace BusinessProcessAPIReq.RequestModels
{
    /// <summary>
    /// 3.3	API33 – Hủy lệnh giao dịch khớp lệnh
    /// </summary>
    public class API33OrderCancelAutomaticOrderMatchingRequest
    {
        public string OrderNo { get; set; } = string.Empty;
        public string RefExchangeID { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}