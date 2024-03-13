namespace BusinessProcessAPIReq.RequestModels
{
    /// <summary>
    /// 3.1	API31 – Đặt lệnh giao dịch khớp lệnh
    /// </summary>
    public class API31OrderNewAutomaticOrderMatchingRequest
    {
        public string OrderNo { get; set; } = string.Empty;
        public string ClientID { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
        public string OrderType { get; set; } = string.Empty;
        public long OrderQty { get; set; }
        public long Price { get; set; }
        public long OrderQtyMM2 { get; set; }
        public long PriceMM2 { get; set; }
        public int SpecialType { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}