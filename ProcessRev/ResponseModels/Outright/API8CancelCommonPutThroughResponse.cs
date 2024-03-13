using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    // 1.8	API8: API hủy lệnh thỏa thuận báo cáo giao dịch Outright chưa thực hiện
    public class API8CancelCommonPutThroughResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API8CancelCommonPutThroughRequest InData { get; set; }
    }
}