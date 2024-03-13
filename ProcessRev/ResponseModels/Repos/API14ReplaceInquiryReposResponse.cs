using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 2.2	API14 – Sửa lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
    /// </summary>
    public class API14ReplaceInquiryReposResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API14ReplaceInquiryReposRequest InData { get; set; }
    }
}