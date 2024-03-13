using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 2.15	API27 – Hủy lệnh thỏa thuận Repos đã thực hiện trong ngày (hủy GD Repos)
    /// </summary>
    public class API27OrderCancelDeal1stTransactionReposCommonPutthroughResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest InData { get; set; }
    }
}