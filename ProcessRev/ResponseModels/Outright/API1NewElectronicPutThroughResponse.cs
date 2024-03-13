using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    public class API1NewElectronicPutThroughResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public API1NewElectronicPutThroughRequest InData { get; set; }
    }
}