namespace ObjectInfo
{
    public class Msg_Tprl_Sesion_Info
    {
        public long Id { get; set; }
        public string Sor { get; set; } = string.Empty;
        public string Msgtype { get; set; } = string.Empty;
        public string Sendercompid { get; set; } = string.Empty;
        public string Targetcompid { get; set; } = string.Empty;
        public decimal Msgseqnum { get; set; }
        public string Possdupflag { get; set; } = string.Empty;
        public string Sendingtime { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public decimal Lastmsgseqnumprocessed { get; set; }
        public string Tradsesreqid { get; set; } = string.Empty;
        public string Tradingsessionid { get; set; } = string.Empty;
        public string Tradsesmode { get; set; } = string.Empty;
        public string Tradsesstatus { get; set; } = string.Empty;
        public string Tradsesstarttime { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public string Lastchange { get; set; } = string.Empty;
        public string Createtime { get; set; } = string.Empty;
    }
}