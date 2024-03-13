using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    // 1.12	API12: API phản hồi hủy lệnh thỏa thuận Outright đã thực
    public class API12ResponseForCancelingCommonPutThroughDealResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API12ResponseForCancelingCommonPutThroughDealRequest InData { get; set; }
    }
}