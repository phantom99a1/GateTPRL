using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    public class API3ReplaceElectronicPutThroughResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public API3ReplaceElectronicPutThroughRequest InData { get; set; }
    }
}