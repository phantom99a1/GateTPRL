using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 2.3	API15 – Hủy lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
    /// </summary>
    public class API15CancelInquiryReposResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API15CancelInquiryReposRequest InData { get; set; }
    }
}