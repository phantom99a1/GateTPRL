namespace ObjectInfo
{
    public class Msg_Tprl_Reject_Info
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
        public decimal Refseqnum { get; set; }
        public string Lastmsgseqnumprocessed { get; set; } = string.Empty;
        public long Sessionrejectreason { get; set; }
        public string Refmsgtype { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public string Lastchange { get; set; } = string.Empty;
        public string Createtime { get; set; } = string.Empty;
    }
}