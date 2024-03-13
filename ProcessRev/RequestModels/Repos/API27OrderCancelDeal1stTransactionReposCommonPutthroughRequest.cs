namespace BusinessProcessAPIReq.RequestModels
{
    /// <summary>
    /// 2.15	API27 – Hủy lệnh thỏa thuận Repos đã thực hiện trong ngày (hủy GD Repos)
    /// </summary>
    public class API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest
    {
        public string OrderNo { get; set; } = string.Empty;
        public string RefExchangeID { get; set; } = string.Empty;
        public int QuoteType { get; set; }
        public string OrderType { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}