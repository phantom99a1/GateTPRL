namespace BusinessProcessResponse
{
    public class NormalOrderExecutionObjectModel
    {
        public string MsgType { get; set; } = "";
        public string Text { get; set; } = "";
        public string OrderNo { get; set; } = "";
        public string OrderID { get; set; } = "";
        public string BuyOrderID { get; set; } = "";
        public string SellOrderID { get; set; } = "";
        public string Symbol { get; set; } = "";
        public string Side { get; set; } = "";
        public long LastQty { get; set; } = 0;
        public long LastPx { get; set; } = 0;
        public double SettleValue { get; set; }
        public string ExecID { get; set; } = "";
        public string MemberCounterFirm { get; set; } = "";
        public string SendingTime { get; set; } = "";
    }
}