using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 2.1	API13 – Đặt lệnh điện tử tùy chọn Inquiry Repos
    /// </summary>
    public class API13NewInquiryReposResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API13NewInquiryReposRequest InData { get; set; }
    }
}