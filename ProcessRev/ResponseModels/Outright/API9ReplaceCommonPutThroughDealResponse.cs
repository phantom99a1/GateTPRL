using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    // 1.9	API9: API sửa lệnh thỏa thuận Outright đã thực hiện
    public class API9ReplaceCommonPutThroughDealResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public API9ReplaceCommonPutThroughDealRequest InData { get; set; }
    }
}