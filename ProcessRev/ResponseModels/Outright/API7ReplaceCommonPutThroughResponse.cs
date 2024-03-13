using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    // 1.7	API7: API sửa lệnh thỏa thuận báo cáo giao dịch Outright chưa thực hiện
    public class API7ReplaceCommonPutThroughResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public API7ReplaceCommonPutThroughRequest InData { get; set; }
    }
}