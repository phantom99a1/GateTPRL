namespace BusinessProcessAPIReq.RequestModels
{
    /// <summary>
    /// 1.1	API1: Api đặt lệnh thỏa thuận điện tử Outright
    /// </summary>
    public class API1NewElectronicPutThroughRequest
    {
        public string OrderNo { get; set; } = string.Empty;
        public string ClientID { get; set; } = string.Empty;
        public string OrderType { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public long Price { get; set; }
        public long OrderQty { get; set; }
        public string SettleDate { get; set; } = string.Empty;
        public int SettleMethod { get; set; } 
        public string RegistID { get; set; } = string.Empty;
        public int IsVisible { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}