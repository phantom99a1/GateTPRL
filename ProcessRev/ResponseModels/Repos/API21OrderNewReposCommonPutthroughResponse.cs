using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 2.9	API21 – Đặt lệnh thỏa thuận báo cáo giao dịch Repos
    /// </summary>
    public class API21OrderNewReposCommonPutthroughResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API21OrderNewReposCommonPutthroughRequest InData { get; set; }
    }
}