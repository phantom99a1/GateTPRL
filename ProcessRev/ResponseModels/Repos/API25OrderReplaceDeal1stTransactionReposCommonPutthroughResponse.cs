using BusinessProcessAPIReq.RequestModels;

namespace BusinessProcessAPIReq.ResponseModels
{
    /// <summary>
    /// 2.13	API25 – Sửa lệnh thỏa thuận Repos đã thực hiện trong ngày (sửa GD Repos)
    /// </summary>
    public class API25OrderReplaceDeal1stTransactionReposCommonPutthroughResponse
    {
        public string ReturnCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest InData { get; set; }
    }
}