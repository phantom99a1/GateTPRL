using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 3.2	API32 – Sửa lệnh giao dịch khớp lệnh
    /// </summary>
    public class API32OrderReplaceAutomaticOrderMatchingResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API32OrderReplaceAutomaticOrderMatchingRequest InData { get; set; }
    }
}