namespace BusinessProcessAPIReq.RequestModels
{
    public class APIReposSideList
    {
        public long NumSide { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public long OrderQty { get; set; }
        public long MergePrice { get; set; }
        public double HedgeRate { get; set; }
	}
}