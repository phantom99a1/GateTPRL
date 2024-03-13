namespace BusinessProcessAPIReq.RequestModels
{
    // 1.12	API12: API phản hồi hủy lệnh thỏa thuận Outright đã thực
    public class API12ResponseForCancelingCommonPutThroughDealRequest
    {
        public string OrderNo { get; set; } = string.Empty;
        public string RefExchangeID { get; set; } = string.Empty;
        public string OrderType { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
        public int CrossType { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}