using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    // 1.11	API11: API hủy lệnh thỏa thuận Outright đã thực hiện
    public class API11CancelCommonPutThroughDealResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API11CancelCommonPutThroughDealRequest InData { get; set; }
    }
}