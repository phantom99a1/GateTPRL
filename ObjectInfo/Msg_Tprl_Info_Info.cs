namespace ObjectInfo
{
    public class Msg_Tprl_Info_Info
    {
        public long Id { get; set; }
        public string Sor { get; set; } = string.Empty;
        public string Msgtype { get; set; } = string.Empty;
        public string Sendercompid { get; set; } = string.Empty;
        public string Targetcompid { get; set; } = string.Empty;
        public long Msgseqnum { get; set; }
        public string Sendingtime { get; set; } = string.Empty;
        public long Lastmsgseqnumprocessed { get; set; }
        public string Encryptmethod { get; set; } = string.Empty;
        public string Heartbtint { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Pwd { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public string Lastchange { get; set; } = string.Empty;
        public string Createtime { get; set; } = string.Empty;
    }
}