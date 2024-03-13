using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    public class API2AcceptElectronicPutThroughResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public API2AcceptElectronicPutThroughRequest InData { get; set; }
    }
}