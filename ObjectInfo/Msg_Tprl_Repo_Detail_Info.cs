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
        public double Reposinterest { get; set; }
        public double Hedgerate { get; set; }
        public double Settlvalue2 { get; set; }
        public double Settlvalue { get; set; }
        public decimal Price2 { get; set; }
        public string Remark { get; set; } = string.Empty;
        public string Lastchange { get; set; } = string.Empty;
        public string Createtime { get; set; } = string.Empty;
    }
}