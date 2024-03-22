namespace ObjectInfo
{
    public class Msg_Tprl_Request_Info
    {
        public long Id { get; set; }
        public string Sor { get; set; } = string.Empty;
        public int Msgtype { get; set; }
        public string Sendercompid { get; set; } = string.Empty;
        public string Targetcompid { get; set; } = string.Empty;
        public decimal Msgseqnum { get; set; }
        public string Sendingtime { get; set; } = string.Empty;
        public decimal Lastmsgseqnumprocessed { get; set; }
        public decimal Beginseqno { get; set; }
        public decimal Endseqno { get; set; }
        public decimal Testreqid { get; set; }
        public decimal Newseqno { get; set; }
        public string Remark { get; set; } = string.Empty;
        public string Lastchange { get; set; } = string.Empty;
        public string Createtime { get; set; } = string.Empty;
    }
}