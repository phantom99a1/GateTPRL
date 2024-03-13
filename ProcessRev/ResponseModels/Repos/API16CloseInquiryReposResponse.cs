using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 2.4	API16 – Đóng lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
    /// </summary>
    public class API16CloseInquiryReposResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API16CloseInquiryReposRequest InData { get; set; }
    }
}