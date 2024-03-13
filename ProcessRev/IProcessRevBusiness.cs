using BusinessProcessAPIReq.RequestModels;
using BusinessProcessAPIReq.ResponseModels;

namespace BusinessProcessAPIReq
{
    public interface IProcessRevBussiness
    {
        public (long, long) ItemsInQueue();

        public void RecoverData();

        public void StopReceiveApi();

        /// <summary>
        /// 1.1	Api đặt lệnh thỏa thuận điện tử Outright
        /// </summary>
        public Task<API1NewElectronicPutThroughResponse> API1NewElectronicPutThrough_BU(API1NewElectronicPutThroughRequest request, API1NewElectronicPutThroughResponse _response);

        /// <summary>
        /// 1.2	Api chấp nhận lệnh thỏa thuận điện tử Outright
        /// </summary>
        public Task<API2AcceptElectronicPutThroughResponse> API2AcceptElectronicPutThrough_BU(API2AcceptElectronicPutThroughRequest request, API2AcceptElectronicPutThroughResponse _response);

        /// <summary>
        /// 1.3	Api sửa thỏa thuận điện tử Outright chưa thực hiện
        /// </summary>
        public Task<API3ReplaceElectronicPutThroughResponse> API3ReplaceElectronicPutThrough_BU(API3ReplaceElectronicPutThroughRequest request, API3ReplaceElectronicPutThroughResponse _response);

        /// <summary>
        /// 1.4	Api huỷ thỏa thuận điện tử Outright chưa thực hiện
        /// </summary>
        public Task<API4CancelElectronicPutThroughResponse> API4CancelElectronicPutThrough_BU(API4CancelElectronicPutThroughRequest request, API4CancelElectronicPutThroughResponse _response);

        /// <summary>
        /// 1.5	Api đặt lệnh thỏa thuận Báo cáo giao dịch Outright
        /// </summary>
        public Task<API5NewCommonPutThroughResponse> API5NewCommonPutThrough_BU(API5NewCommonPutThroughRequest request, API5NewCommonPutThroughResponse _response);

        /// <summary>
        /// 1.6	Api chấp nhận thỏa thuận Báo cáo giao dịch Outright
        /// </summary>
        public Task<API6AcceptCommonPutThroughResponse> API6AcceptCommonPutThrough_BU(API6AcceptCommonPutThroughRequest request, API6AcceptCommonPutThroughResponse _response);

        /// <summary>
        /// 1.7	API7: API sửa lệnh thỏa thuận báo cáo giao dịch Outright chưa thực hiện
        /// </summary>
        public Task<API7ReplaceCommonPutThroughResponse> API7ReplaceCommonPutThrough_BU(API7ReplaceCommonPutThroughRequest request, API7ReplaceCommonPutThroughResponse _response);

        /// <summary>
        /// 1.8	API8: API hủy lệnh thỏa thuận báo cáo giao dịch Outright chưa thực hiện
        /// </summary>
        public Task<API8CancelCommonPutThroughResponse> API8CancelCommonPutThrough_BU(API8CancelCommonPutThroughRequest request, API8CancelCommonPutThroughResponse _response);

        /// <summary>
        /// 1.9	API9: API sửa lệnh thỏa thuận Outright đã thực hiện
        /// </summary>
        public Task<API9ReplaceCommonPutThroughDealResponse> API9ReplaceCommonPutThroughDeal_BU(API9ReplaceCommonPutThroughDealRequest request, API9ReplaceCommonPutThroughDealResponse _response);

        /// <summary>
        /// 1.10	API10: API phản hồi yêu cầu sửa thỏa thuận Outright đã thực hiện
        /// </summary>
        public Task<API10ResponseForReplacingCommonPutThroughDealResponse> API10ResponseForReplacingCommonPutThroughDeal_BU(API10ResponseForReplacingCommonPutThroughDealRequest request, API10ResponseForReplacingCommonPutThroughDealResponse _response);

        /// <summary>
        /// 1.11	API11: API hủy lệnh thỏa thuận Outright đã thực hiện
        /// </summary>
        public Task<API11CancelCommonPutThroughDealResponse> API11CancelCommonPutThroughDeal_BU(API11CancelCommonPutThroughDealRequest request, API11CancelCommonPutThroughDealResponse _response);

        /// <summary>
        /// 1.12	API12: API phản hồi hủy lệnh thỏa thuận Outright đã thực
        /// </summary>
        public Task<API12ResponseForCancelingCommonPutThroughDealResponse> API12ResponseForCancelingCommonPutThroughDeal_BU(API12ResponseForCancelingCommonPutThroughDealRequest request, API12ResponseForCancelingCommonPutThroughDealResponse _response);

        /// <summary>
        ///  2.1	API13 – Đặt lệnh điện tử tùy chọn Inquiry Repos
        /// </summary>
        public Task<API13NewInquiryReposResponse> API13NewInquiryRepos_BU(API13NewInquiryReposRequest request, API13NewInquiryReposResponse _response);

        /// <summary>
        ///  2.2	API14 – Sửa lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
        /// </summary>
        public Task<API14ReplaceInquiryReposResponse> API14ReplaceInquiryRepos_BU(API14ReplaceInquiryReposRequest request, API14ReplaceInquiryReposResponse _response);

        /// <summary>
        ///  2.3	API15 – Hủy lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
        /// </summary>
        public Task<API15CancelInquiryReposResponse> API15CancelInquiryRepos_BU(API15CancelInquiryReposRequest request, API15CancelInquiryReposResponse _response);


        /// <summary>
        ///  2.4	API16 – Đóng lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
        /// </summary>
        public Task<API16CloseInquiryReposResponse> API16CloseInquiryRepos_BU(API16CloseInquiryReposRequest request, API16CloseInquiryReposResponse _response);


        /// <summary>
        ///  2.5	API17 – Đặt lệnh điện tử tùy chọn Firm Repos
        /// </summary>
        public Task<API17OrderNewFirmReposResponse> API17OrderNewFirmRepos_BU(API17OrderNewFirmReposRequest request, API17OrderNewFirmReposResponse _response);

        /// <summary>
        ///  2.6	API18 – Sửa lệnh điện tử tùy chọn Firm Repos chưa thực hiện
        /// </summary>
        public Task<API18OrderReplaceFirmReposResponse> API18OrderReplaceFirmRepos_BU(API18OrderReplaceFirmReposRequest request, API18OrderReplaceFirmReposResponse _response);

        /// <summary>
        ///  2.7	API19 – Hủy lệnh điện tử tùy chọn Firm Repos chưa thực hiện
        /// </summary>
        public Task<API19OrderCancelFirmReposResponse> API19OrderCancelFirmRepos_BU(API19OrderCancelFirmReposRequest request, API19OrderCancelFirmReposResponse _response);

        /// <summary>
        ///  2.8	API20 – Xác nhận lệnh điện tử tùy chọn Firm Repos
        /// </summary>
        public Task<API20OrderConfirmFirmReposResponse> API20OrderConfirmFirmRepos_BU(API20OrderConfirmFirmReposRequest request, API20OrderConfirmFirmReposResponse _response);

        /// <summary>
        /// 2.9	API21 – Đặt lệnh thỏa thuận báo cáo giao dịch Repos
        /// </summary>
        public Task<API21OrderNewReposCommonPutthroughResponse> API21OrderNewReposCommonPutthough_BU(API21OrderNewReposCommonPutthroughRequest request, API21OrderNewReposCommonPutthroughResponse _response);

        /// <summary>
        /// 2.10	API22 – Xác nhận lệnh thỏa thuận báo cáo giao dịch Repos
        /// </summary>
        public Task<API22OrderConfirmReposCommonPutthroughResponse> API22OrderConfirmReposCommonPutthrough_BU(API22OrderConfirmReposCommonPutthroughRequest request, API22OrderConfirmReposCommonPutthroughResponse _response);

        /// <summary>
        /// 2.11	API23 – Sửa lệnh thỏa thuận báo cáo giao dịch Repos chưa thực hiện
        /// </summary>
        public Task<API23OrderReplaceReposCommonPutthroughResponse> API23OrderReplaceReposCommonPutthrough_BU(API23OrderReplaceReposCommonPutthroughRequest request, API23OrderReplaceReposCommonPutthroughResponse _response);


        /// <summary>
        /// 2.12	API24 – Hủy lệnh thỏa thuận báo cáo giao dịch Repos chưa thực hiện
        /// </summary>
        public Task<API24OrderCancelReposCommonPutthroughResponse> API24OrderCancelReposCommonPutthrough_BU(API24OrderCancelReposCommonPutthroughRequest request, API24OrderCancelReposCommonPutthroughResponse _response);

        /// <summary>
        /// 2.13	API25 – Sửa lệnh thỏa thuận Repos đã thực hiện trong ngày (sửa GD Repos)
        /// </summary>
        public Task<API25OrderReplaceDeal1stTransactionReposCommonPutthroughResponse> API25OrderReplaceDeal1stTransactionReposCommonPutthrough_BU(API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest request, API25OrderReplaceDeal1stTransactionReposCommonPutthroughResponse _response);

        /// <summary>
        /// 2.14	API26 – Phản hồi sửa lệnh thỏa thuận Repos đã thực hiện trong ngày (phản hồi sửa GD Repos)
        /// </summary>
        public Task<API26OrderReplaceDeal1stTransactionReposCommonPutthroughResponse> API26OrderReplaceDeal1stTransactionReposCommonPutthrough_BU(API26OrderReplaceDeal1stTransactionReposCommonPutthroughRequest request, API26OrderReplaceDeal1stTransactionReposCommonPutthroughResponse _response);


        /// <summary>
        /// 2.15	API27 – Hủy lệnh thỏa thuận Repos đã thực hiện trong ngày (hủy GD Repos)
        /// </summary>
        public Task<API27OrderCancelDeal1stTransactionReposCommonPutthroughResponse> API27OrderCancelDeal1stTransactionReposCommonPutthrough_BU(API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest request, API27OrderCancelDeal1stTransactionReposCommonPutthroughResponse _response);


        /// <summary>
        /// 2.16	API28 – Phản hồi hủy lệnh thỏa thuận Repos đã thực hiện trong ngày (phản hồi hủy GD Repos)
        /// </summary>
        public Task<API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughResponse> API28OrderConfirmCancelDeal1stTransactionReposCommonPutthrough_BU(API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughRequest request, API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughResponse _response);

        /// <summary>
        /// 2.17	API29 – Sửa lệnh thỏa thuận Repos mua lại đảo ngược (sửa GD Reverse Repos)
        /// </summary>
        public Task<API29OrderReplaceDeal2ndTransactionReposCommonPutthroughResponse> API29OrderReplaceDeal2ndTransactionReposCommonPutthrough_BU(API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest request, API29OrderReplaceDeal2ndTransactionReposCommonPutthroughResponse _response);

        /// <summary>
        /// 2.18	API30 – Phản hồi sửa lệnh thỏa thuận Repos mua lại đảo ngược (phản hồi sửa GD Reverse Repos)
        /// </summary>
        public Task<API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughResponse> API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthrough_BU(API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughRequest request, API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughResponse _response);

        /// <summary>
        /// 3.1	API31 – Đặt lệnh giao dịch khớp lệnh
        /// </summary>
        public Task<API31OrderNewAutomaticOrderMatchingResponse> API31OrderNewAutomaticOrderMatching_BU(API31OrderNewAutomaticOrderMatchingRequest request, API31OrderNewAutomaticOrderMatchingResponse _response);

        /// <summary>
        /// 3.2	API32 – Sửa lệnh giao dịch khớp lệnh
        /// </summary>
        public Task<API32OrderReplaceAutomaticOrderMatchingResponse> API32OrderReplaceAutomaticOrderMatching_BU(API32OrderReplaceAutomaticOrderMatchingRequest request, API32OrderReplaceAutomaticOrderMatchingResponse _response);

        /// <summary>
        /// 3.3	API33 – Hủy lệnh giao dịch khớp lệnh
        /// </summary>
        public Task<API33OrderCancelAutomaticOrderMatchingResponse> API33OrderCancelAutomaticOrderMatching_BU(API33OrderCancelAutomaticOrderMatchingRequest request, API33OrderCancelAutomaticOrderMatchingResponse _response);
    }
}