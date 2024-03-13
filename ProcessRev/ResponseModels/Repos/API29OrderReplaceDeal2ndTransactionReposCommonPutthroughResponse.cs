using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 2.17	API29 – Sửa lệnh thỏa thuận Repos mua lại đảo ngược (sửa GD Reverse Repos)
    /// </summary>
    public class API29OrderReplaceDeal2ndTransactionReposCommonPutthroughResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest InData { get; set; }
    }
}