using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 3.3	API33 – Hủy lệnh giao dịch khớp lệnh
    /// </summary>
    public class API33OrderCancelAutomaticOrderMatchingResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API33OrderCancelAutomaticOrderMatchingRequest InData { get; set; }
    }
}