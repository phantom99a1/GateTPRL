namespace BusinessProcessAPIReq.RequestModels
{
    // 1.8	API8: API hủy lệnh thỏa thuận báo cáo giao dịch Outright chưa thực hiện
    public class API8CancelCommonPutThroughRequest
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