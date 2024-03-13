using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 2.8	API20 – Xác nhận lệnh điện tử tùy chọn Firm Repos
    /// </summary>
    public class API20OrderConfirmFirmReposResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API20OrderConfirmFirmReposRequest InData { get; set; }
    }
}