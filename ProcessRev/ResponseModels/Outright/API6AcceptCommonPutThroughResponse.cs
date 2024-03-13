using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    public class API6AcceptCommonPutThroughResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public API6AcceptCommonPutThroughRequest InData { get; set; }
    }
}