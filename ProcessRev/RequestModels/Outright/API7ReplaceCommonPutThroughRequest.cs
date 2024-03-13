namespace BusinessProcessAPIReq.RequestModels
{
    // 1.7	API7: API sửa lệnh thỏa thuận báo cáo giao dịch Outright chưa thực hiện
    public class API7ReplaceCommonPutThroughRequest
    {
        public string OrderNo { get; set; } = string.Empty;
        public string RefExchangeID { get; set; } = string.Empty;
        public string ClientID { get; set; } = string.Empty;
        public string ClientIDCounterFirm { get; set; } = string.Empty;
        public string MemberCounterFirm { get; set; } = string.Empty;
        public string OrderType { get; set; } = string.Empty;
        public int CrossType { get; set; }
        public string Side { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public long Price { get; set; }
        public long OrderQty { get; set; }
        public string SettleDate { get; set; } = string.Empty;
        public int SettleMethod { get; set; }
        public string EffectiveTime { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}