using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 2.11	API23 – Sửa lệnh thỏa thuận báo cáo giao dịch Repos chưa thực hiện
    /// </summary>
    public class API23OrderReplaceReposCommonPutthroughResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API23OrderReplaceReposCommonPutthroughRequest InData { get; set; }
    }
}