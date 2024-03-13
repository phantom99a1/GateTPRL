using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 2.7	API19 – Hủy lệnh điện tử tùy chọn Firm Repos chưa thực hiện
    /// </summary>
    public class API19OrderCancelFirmReposResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API19OrderCancelFirmReposRequest InData { get; set; }
    }
}