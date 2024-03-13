using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 2.6	API18 – Sửa lệnh điện tử tùy chọn Firm Repos chưa thực hiện
    /// </summary>
    public class API18OrderReplaceFirmReposResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API18OrderReplaceFirmReposRequest InData { get; set; }
    }
}