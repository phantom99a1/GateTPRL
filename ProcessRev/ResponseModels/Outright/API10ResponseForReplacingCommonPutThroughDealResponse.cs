using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    // 1.10	API10: API phản hồi yêu cầu sửa thỏa thuận Outright đã thực hiện 
    public class API10ResponseForReplacingCommonPutThroughDealResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API10ResponseForReplacingCommonPutThroughDealRequest InData { get; set; }
    }
}