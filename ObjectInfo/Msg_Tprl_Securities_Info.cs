namespace ObjectInfo
{
    public class Msg_Tprl_Securities_Info
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
        public string Tradingsessionsubid { get; set; } = string.Empty;
        public string Securitystatusreqid { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string Securitytype { get; set; } = string.Empty;
        public string Maturitydate { get; set; } = string.Empty;
        public string Issuedate { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public decimal Highpx { get; set; }
        public decimal Lowpx { get; set; }
        public decimal Highpxout { get; set; }
        public decimal Lowpxout { get; set; }
        public decimal Highpxrep { get; set; }
        public decimal Lowpxrep { get; set; }
        public decimal Lastpx { get; set; }
        public decimal Securitytradingstatus { get; set; }
        public decimal Buyvolume { get; set; }
        public string Dateno { get; set; } = string.Empty;
        public decimal Totallistingqtty { get; set; }
        public decimal Typerule { get; set; }
        public string Allowed_Trading_Subject { get; set; } = string.Empty;
        public string Allowed_Trading_Subject_Sell { get; set; } = string.Empty;
        public string Subscriptionrequesttype { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public string Lastchange { get; set; } = string.Empty;
        public string Createtime { get; set; } = string.Empty;
    }
}