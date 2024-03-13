namespace BusinessProcessResponse
{
    public class NormalObjectModel
    {
        public string MsgType { get; set; } = "";
		public string OrderNo { get; set; } = "";
        public string ExchangeID { get; set; } = "";
        public string RefExchangeID { get; set; } = "";
		public string RefMsgType { get; set; } = "";
		public string OrderType { get; set; } = "";
        public string OrderStatus { get; set; } = "";
        public string Side { get; set; } = "";
        public string Symbol { get; set; } = "";
        public long OrderQty { get; set; } = 0;
        public long OrgOrderQty { get; set; } = 0;
        public long LeavesQty { get; set; } = 0;
        public long LastQty { get; set; } = 0;
        public long Price { get; set; } = 0;
        public string ClientID { get; set; } = "";
        public double SettleValue { get; set; }
        public long OrderQtyMM2 { get; set; }
        public long PriceMM2 { get; set; }
        public int SpecialType { get; set; }
        public string RejectReasonCode { get; set; } = "";
        public string RejectReason { get; set; } = "";
        public string Text { get; set; } = "";
        public string SendingTime { get; set; } = "";
    }
}