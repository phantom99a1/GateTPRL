namespace BusinessProcessAPIReq.RequestModels
{
    /// <summary>
    /// 3.2	API32 – Sửa lệnh giao dịch khớp lệnh
    /// </summary>
    public class API32OrderReplaceAutomaticOrderMatchingRequest
    {
        public string OrderNo { get; set; } = string.Empty;
        public string RefExchangeID { get; set; } = string.Empty;
        public string ClientID { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public long OrderQty { get; set; }
        public long OrgOrderQty { get; set; }
        public long Price { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}