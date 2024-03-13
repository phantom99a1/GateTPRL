using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 3.1	API31 – Đặt lệnh giao dịch khớp lệnh
    /// </summary>
    public class API31OrderNewAutomaticOrderMatchingResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API31OrderNewAutomaticOrderMatchingRequest InData { get; set; }
    }
}