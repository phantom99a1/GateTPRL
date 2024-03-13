using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 2.14	API26 – Phản hồi sửa lệnh thỏa thuận Repos đã thực hiện trong ngày (phản hồi sửa GD Repos)
    /// </summary>
    public class API26OrderReplaceDeal1stTransactionReposCommonPutthroughResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API26OrderReplaceDeal1stTransactionReposCommonPutthroughRequest InData { get; set; }
    }
}