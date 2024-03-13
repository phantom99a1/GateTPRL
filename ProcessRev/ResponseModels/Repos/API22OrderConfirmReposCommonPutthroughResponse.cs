using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 2.10	API22 – Xác nhận lệnh thỏa thuận báo cáo giao dịch Repos
    /// </summary>
    public class API22OrderConfirmReposCommonPutthroughResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API22OrderConfirmReposCommonPutthroughRequest InData { get; set; }
    }
}