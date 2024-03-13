namespace BusinessProcessAPIReq.RequestModels
{
    /// <summary>
    /// 2.16	API28 – Phản hồi hủy lệnh thỏa thuận Repos đã thực hiện trong ngày (phản hồi hủy GD Repos)
    /// </summary>
    public class API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughRequest
    {
        public string OrderNo { get; set; } = string.Empty;
        public string RefExchangeID { get; set; } = string.Empty;
        public int QuoteType { get; set; }
        public string OrderType { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}