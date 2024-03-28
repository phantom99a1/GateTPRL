namespace ObjectInfo
{
    public class Msg_Tprl_Hnx_Confirm_Info
    {
        public long Id { get; set; }
        public string Sor { get; set; } = string.Empty;
        public string Msgtype { get; set; } = string.Empty;
        public string Sendercompid { get; set; } = string.Empty;
        public string Targetcompid { get; set; } = string.Empty;
        public long Msgseqnum { get; set; }
        public string Possdupflag { get; set; } = string.Empty;
        public string Sendingtime { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Exectype { get; set; } = string.Empty;
        public string Lastmsgseqnumprocessed { get; set; } = string.Empty;
        public string Ordstatus { get; set; } = string.Empty;
        public string Orderid { get; set; } = string.Empty;
        public string Clordid { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
        public decimal Orderqty { get; set; }
        public string Ordtype { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Account { get; set; } = string.Empty;
        public double Settlvalue { get; set; }
        public decimal Leavesqty { get; set; }
        public string Origclordid { get; set; } = string.Empty;
        public decimal Lastqty { get; set; }
        public decimal Lastpx { get; set; }
        public string Execid { get; set; } = string.Empty;
        public string Reciprocalmember { get; set; } = string.Empty;
        public string Ordrejreason { get; set; } = string.Empty;
        public string Underlyinglastqty { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public string Lastchange { get; set; } = string.Empty;
        public string Createtime { get; set; } = string.Empty;
    }
}