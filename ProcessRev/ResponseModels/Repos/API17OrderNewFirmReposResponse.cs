using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 2.5	API17 – Đặt lệnh điện tử tùy chọn Firm Repos
    /// </summary>
    public class API17OrderNewFirmReposResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API17OrderNewFirmReposRequest InData { get; set; }
    }
}