using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 2.12	API24 – Hủy lệnh thỏa thuận báo cáo giao dịch Repos chưa thực hiện
    /// </summary>
    public class API24OrderCancelReposCommonPutthroughResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API24OrderCancelReposCommonPutthroughRequest InData { get; set; }
    }
}