namespace BusinessProcessAPIReq.RequestModels
{
    // 1.11	API11: API hủy lệnh thỏa thuận Outright đã thực hiện
    public class API11CancelCommonPutThroughDealRequest
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