namespace ObjectInfo
{
    public class Msg_Tprl_Repo_Detail_Info
    {
        public long Id { get; set; }
        public decimal Refrepoid { get; set; }
        public decimal Numside { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public decimal Execqty { get; set; }
        public decimal Execpx { get; set; }
        public decimal Price { get; set; }
        public decimal Reposinterest { get; set; }
        public decimal Hedgerate { get; set; }
        public decimal Settlvalue2 { get; set; }
        public decimal Settlvalue { get; set; }
        public decimal Price2 { get; set; }
        public string Remark { get; set; } = string.Empty;
        public string Lastchange { get; set; } = string.Empty;
        public string Createtime { get; set; } = string.Empty;
    }
}