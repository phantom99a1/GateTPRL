namespace ObjectInfo
{
    public class Msg_Tprl_Order_Info
    {
        public long Id { get; set; }
        public string Sor { get; set; } = string.Empty;
        public string Msgtype { get; set; } = string.Empty;
        public string Sendercompid { get; set; } = string.Empty;
        public long Targetcompid { get; set; }
        public string Msgseqnum { get; set; } = string.Empty;
        public string Sendingtime { get; set; } = string.Empty;
        public string Lastmsgseqnumprocessed { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Clordid { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
        public string Orderqty { get; set; } = string.Empty;
        public string Ordtype { get; set; } = string.Empty;
        public decimal Price2 { get; set; }
        public decimal Price { get; set; } 
        public decimal Orderqty2 { get; set; }
        public string Origclordid { get; set; } = string.Empty;
        public string Orgorderqty { get; set; } = string.Empty;
        public decimal Special_Type { get; set; }
        public string Remark { get; set; } = string.Empty;
        public string Lastchange { get; set; } = string.Empty;
        public string Createtime { get; set; } = string.Empty;
        public string OrderNo { get; set; } = string.Empty;
    }
}