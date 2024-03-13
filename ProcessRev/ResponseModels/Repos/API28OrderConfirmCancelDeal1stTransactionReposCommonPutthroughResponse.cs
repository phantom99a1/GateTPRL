using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 2.16	API28 – Phản hồi hủy lệnh thỏa thuận Repos đã thực hiện trong ngày (phản hồi hủy GD Repos)
    /// </summary>
    public class API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughRequest InData { get; set; }
    }
}