namespace BusinessProcessAPIReq.RequestModels
{
    public class API4CancelElectronicPutThroughRequest
    {
        public string OrderNo { get; set; } = string.Empty;
        public string RefExchangeID { get; set; } = string.Empty;
        public string OrderType { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string Text { get; set; }= string.Empty;
    }
}