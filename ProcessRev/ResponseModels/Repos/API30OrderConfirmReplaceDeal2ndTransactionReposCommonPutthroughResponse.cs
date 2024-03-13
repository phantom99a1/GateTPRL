using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 2.18	API30 – Phản hồi sửa lệnh thỏa thuận Repos mua lại đảo ngược (phản hồi sửa GD Reverse Repos)
    /// </summary>
    public class API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughRequest InData { get; set; }
    }
}