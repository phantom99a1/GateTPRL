using BusinessProcessAPIReq;
using BusinessProcessAPIReq.RequestModels;
using BusinessProcessAPIReq.ResponseModels;

namespace UTInputData.Mock
{
    public class MockIProcessRevBussiness : IProcessRevBussiness
    {
        /// <summary>
        /// 1.1	Api đặt lệnh thỏa thuận điện tử Outright
        /// </summary>
        public async Task<API1NewElectronicPutThroughResponse> API1NewElectronicPutThrough_BU(API1NewElectronicPutThroughRequest request, API1NewElectronicPutThroughResponse _response)
        {
            return new API1NewElectronicPutThroughResponse() { ReturnCode = "000" };
        }

        /// <summary>
        /// 1.2	Api chấp nhận lệnh thỏa thuận điện tử Outright
        /// </summary>
        public async Task<API2AcceptElectronicPutThroughResponse> API2AcceptElectronicPutThrough_BU(API2AcceptElectronicPutThroughRequest request, API2AcceptElectronicPutThroughResponse _response)
        {
            return new API2AcceptElectronicPutThroughResponse() { ReturnCode = "000" };
        }

        /// <summary>
        /// 1.3	Api sửa thỏa thuận điện tử Outright chưa thực hiện
        /// </summary>
        public async Task<API3ReplaceElectronicPutThroughResponse> API3ReplaceElectronicPutThrough_BU(API3ReplaceElectronicPutThroughRequest request, API3ReplaceElectronicPutThroughResponse _response)
        {
            return new API3ReplaceElectronicPutThroughResponse() { ReturnCode = "000" };
        }

        /// <summary>
        /// 1.4	Api huỷ thỏa thuận điện tử Outright chưa thực hiện
        /// </summary>
        public async Task<API4CancelElectronicPutThroughResponse> API4CancelElectronicPutThrough_BU(API4CancelElectronicPutThroughRequest request, API4CancelElectronicPutThroughResponse _response)
        {
            return new API4CancelElectronicPutThroughResponse() { ReturnCode = "000" };
        }

        /// <summary>
        /// 1.5	Api đặt lệnh thỏa thuận Báo cáo giao dịch Outright
        /// </summary>
        public async Task<API5NewCommonPutThroughResponse> API5NewCommonPutThrough_BU(API5NewCommonPutThroughRequest request, API5NewCommonPutThroughResponse _response)
        {
            return new API5NewCommonPutThroughResponse() { ReturnCode = "000" };
        }

        /// <summary>
        /// 1.6	Api chấp nhận thỏa thuận Báo cáo giao dịch Outright
        /// </summary>
        public async Task<API6AcceptCommonPutThroughResponse> API6AcceptCommonPutThrough_BU(API6AcceptCommonPutThroughRequest request, API6AcceptCommonPutThroughResponse _response)
        {
            return new API6AcceptCommonPutThroughResponse() { ReturnCode = "000" };
        }

        /// <summary>
        /// 1.7	API7: API sửa lệnh thỏa thuận báo cáo giao dịch Outright chưa thực hiện
        /// </summary>
        public async Task<API7ReplaceCommonPutThroughResponse> API7ReplaceCommonPutThrough_BU(API7ReplaceCommonPutThroughRequest request, API7ReplaceCommonPutThroughResponse _response)
        {
            return new API7ReplaceCommonPutThroughResponse() { ReturnCode = "000" };
        }

        /// <summary>
        /// 1.8	API8: API hủy lệnh thỏa thuận báo cáo giao dịch Outright chưa thực hiện
        /// </summary>
        public async Task<API8CancelCommonPutThroughResponse> API8CancelCommonPutThrough_BU(API8CancelCommonPutThroughRequest request, API8CancelCommonPutThroughResponse _response)
        {
            return new API8CancelCommonPutThroughResponse() { ReturnCode = "000" };
        }

        /// <summary>
        /// 1.9	API9: API sửa lệnh thỏa thuận Outright đã thực hiện
        /// </summary>
        public async Task<API9ReplaceCommonPutThroughDealResponse> API9ReplaceCommonPutThroughDeal_BU(API9ReplaceCommonPutThroughDealRequest request, API9ReplaceCommonPutThroughDealResponse _response)
        {
            return new API9ReplaceCommonPutThroughDealResponse() { ReturnCode = "000" };
        }

        /// <summary>
        /// 1.10	API10: API phản hồi yêu cầu sửa thỏa thuận Outright đã thực hiện
        /// </summary>
        public async Task<API10ResponseForReplacingCommonPutThroughDealResponse> API10ResponseForReplacingCommonPutThroughDeal_BU(API10ResponseForReplacingCommonPutThroughDealRequest request, API10ResponseForReplacingCommonPutThroughDealResponse _response)
        {
            return new API10ResponseForReplacingCommonPutThroughDealResponse() { ReturnCode = "000" };
        }

        /// <summary>
        /// 1.11	API11: API hủy lệnh thỏa thuận Outright đã thực hiện
        /// </summary>
        public async Task<API11CancelCommonPutThroughDealResponse> API11CancelCommonPutThroughDeal_BU(API11CancelCommonPutThroughDealRequest request, API11CancelCommonPutThroughDealResponse _response)
        {
            return new API11CancelCommonPutThroughDealResponse() { ReturnCode = "000" };
        }

        /// <summary>
        /// 1.12	API12: API phản hồi hủy lệnh thỏa thuận Outright đã thực
        /// </summary>
        public async Task<API12ResponseForCancelingCommonPutThroughDealResponse> API12ResponseForCancelingCommonPutThroughDeal_BU(API12ResponseForCancelingCommonPutThroughDealRequest request, API12ResponseForCancelingCommonPutThroughDealResponse _response)
        {
            return new API12ResponseForCancelingCommonPutThroughDealResponse() { ReturnCode = "000" };
        }

        /// <summary>
        /// 2.1	API13 – Đặt lệnh điện tử tùy chọn Inquiry Repos
        /// </summary>
        public async Task<API13NewInquiryReposResponse> API13NewInquiryRepos_BU(API13NewInquiryReposRequest request, API13NewInquiryReposResponse _response)
        {
            return new API13NewInquiryReposResponse() { ReturnCode = "000" };
        }

        /// <summary>
        /// 2.2	API14 – Sửa lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
        /// </summary>
        public async Task<API14ReplaceInquiryReposResponse> API14ReplaceInquiryRepos_BU(API14ReplaceInquiryReposRequest request, API14ReplaceInquiryReposResponse _response)
        {
            return new API14ReplaceInquiryReposResponse() { ReturnCode = "000" };
        }

        /// <summary>
        ///  2.3	API15 – Hủy lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
        /// </summary>
        public async Task<API15CancelInquiryReposResponse> API15CancelInquiryRepos_BU(API15CancelInquiryReposRequest request, API15CancelInquiryReposResponse _response)
        {
            return new API15CancelInquiryReposResponse() { ReturnCode = "000" };
        }

        /// <summary>
        ///  2.4	API16 – Đóng lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
        /// </summary>
        public async Task<API16CloseInquiryReposResponse> API16CloseInquiryRepos_BU(API16CloseInquiryReposRequest request, API16CloseInquiryReposResponse _response)
        {
            return new API16CloseInquiryReposResponse() { ReturnCode = "000" };
        }

        public (long, long) ItemsInQueue()
        {
            return new(0, 0);
        }

        /// <summary>
        /// 2.5	API17 – Đặt lệnh điện tử tùy chọn Firm Repos
        /// </summary>
        public async Task<API17OrderNewFirmReposResponse> API17OrderNewFirmRepos_BU(API17OrderNewFirmReposRequest request, API17OrderNewFirmReposResponse _response)
        {
            return new API17OrderNewFirmReposResponse() { ReturnCode = "000" };
        }

        /// <summary>
        /// 2.6	API18 – Sửa lệnh điện tử tùy chọn Firm Repos chưa thực hiện
        /// </summary>
        public async Task<API18OrderReplaceFirmReposResponse> API18OrderReplaceFirmRepos_BU(API18OrderReplaceFirmReposRequest request, API18OrderReplaceFirmReposResponse _response)
        {
            return new API18OrderReplaceFirmReposResponse() { ReturnCode = "000" };
        }

        /// <summary>
        ///  2.7	API19 – Hủy lệnh điện tử tùy chọn Firm Repos chưa thực hiện
        /// </summary>
        public async Task<API19OrderCancelFirmReposResponse> API19OrderCancelFirmRepos_BU(API19OrderCancelFirmReposRequest request, API19OrderCancelFirmReposResponse _response)
        {
            return new API19OrderCancelFirmReposResponse() { ReturnCode = "000" };
        }

        /// <summary>
        ///  2.8	API20 – Xác nhận lệnh điện tử tùy chọn Firm Repos
        /// </summary>
        public async Task<API20OrderConfirmFirmReposResponse> API20OrderConfirmFirmRepos_BU(API20OrderConfirmFirmReposRequest request, API20OrderConfirmFirmReposResponse _response)
        {
            return new API20OrderConfirmFirmReposResponse() { ReturnCode = "000" };
        }

        /// <summary>
        /// 2.9	API21 – Đặt lệnh thỏa thuận báo cáo giao dịch Repos
        /// </summary>
        public async Task<API21OrderNewReposCommonPutthroughResponse> API21OrderNewReposCommonPutthough_BU(API21OrderNewReposCommonPutthroughRequest request, API21OrderNewReposCommonPutthroughResponse _response)
        {
            return new API21OrderNewReposCommonPutthroughResponse() { ReturnCode = "000" };
        }

        /// <summary>
        /// 2.10	API22 – Xác nhận lệnh thỏa thuận báo cáo giao dịch Repos
        /// </summary>
        public async Task<API22OrderConfirmReposCommonPutthroughResponse> API22OrderConfirmReposCommonPutthrough_BU(API22OrderConfirmReposCommonPutthroughRequest request, API22OrderConfirmReposCommonPutthroughResponse _response)
        {
            return new API22OrderConfirmReposCommonPutthroughResponse() { ReturnCode = "000" };
        }

        /// <summary>
        /// 2.11	API23 – Sửa lệnh thỏa thuận báo cáo giao dịch Repos chưa thực hiện
        /// </summary>
        public async Task<API23OrderReplaceReposCommonPutthroughResponse> API23OrderReplaceReposCommonPutthrough_BU(API23OrderReplaceReposCommonPutthroughRequest request, API23OrderReplaceReposCommonPutthroughResponse _response)
        {
            return new API23OrderReplaceReposCommonPutthroughResponse() { ReturnCode = "000" };
        }


        /// <summary>
        /// 2.12	API24 – Hủy lệnh thỏa thuận báo cáo giao dịch Repos chưa thực hiện
        /// </summary>
        public async Task<API24OrderCancelReposCommonPutthroughResponse> API24OrderCancelReposCommonPutthrough_BU(API24OrderCancelReposCommonPutthroughRequest request, API24OrderCancelReposCommonPutthroughResponse _response)
        {
            return new API24OrderCancelReposCommonPutthroughResponse() { ReturnCode = "000" };
        }

        /// <summary>
        /// 2.13	API25 – Sửa lệnh thỏa thuận Repos đã thực hiện trong ngày (sửa GD Repos)
        /// </summary>
        public async Task<API25OrderReplaceDeal1stTransactionReposCommonPutthroughResponse> API25OrderReplaceDeal1stTransactionReposCommonPutthrough_BU(API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest request, API25OrderReplaceDeal1stTransactionReposCommonPutthroughResponse _response)
        {
            return new API25OrderReplaceDeal1stTransactionReposCommonPutthroughResponse() { ReturnCode = "000" };
        }

        /// <summary>
        /// 2.14	API26 – Phản hồi sửa lệnh thỏa thuận Repos đã thực hiện trong ngày (phản hồi sửa GD Repos)
        /// </summary>
        public async Task<API26OrderReplaceDeal1stTransactionReposCommonPutthroughResponse> API26OrderReplaceDeal1stTransactionReposCommonPutthrough_BU(API26OrderReplaceDeal1stTransactionReposCommonPutthroughRequest request, API26OrderReplaceDeal1stTransactionReposCommonPutthroughResponse _response)
        {
            return new API26OrderReplaceDeal1stTransactionReposCommonPutthroughResponse() { ReturnCode = "000" };
        }

        /// <summary>
        ///  2.15	API27 – Hủy lệnh thỏa thuận Repos đã thực hiện trong ngày (hủy GD Repos)
        /// </summary>
        public async Task<API27OrderCancelDeal1stTransactionReposCommonPutthroughResponse> API27OrderCancelDeal1stTransactionReposCommonPutthrough_BU(API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest request, API27OrderCancelDeal1stTransactionReposCommonPutthroughResponse _response)
        {
            return new API27OrderCancelDeal1stTransactionReposCommonPutthroughResponse() { ReturnCode = "000" };
        }

        /// <summary>
        ///  2.16	API28 – Phản hồi hủy lệnh thỏa thuận Repos đã thực hiện trong ngày (phản hồi hủy GD Repos)
        /// </summary>
        public async Task<API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughResponse> API28OrderConfirmCancelDeal1stTransactionReposCommonPutthrough_BU(API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughRequest request, API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughResponse _response)
        {
            return new API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughResponse() { ReturnCode = "000" };
        }

        /// <summary>
        ///  2.17	API29 – Sửa lệnh thỏa thuận Repos mua lại đảo ngược (sửa GD Reverse Repos)
        /// </summary>
        public async Task<API29OrderReplaceDeal2ndTransactionReposCommonPutthroughResponse> API29OrderReplaceDeal2ndTransactionReposCommonPutthrough_BU(API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest request, API29OrderReplaceDeal2ndTransactionReposCommonPutthroughResponse _response)
        {
            return new API29OrderReplaceDeal2ndTransactionReposCommonPutthroughResponse() { ReturnCode = "000" };
        }

        /// <summary>
        ///  2.18	API30 – Phản hồi sửa lệnh thỏa thuận Repos mua lại đảo ngược (phản hồi sửa GD Reverse Repos)
        /// </summary>
        public async Task<API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughResponse> API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthrough_BU(API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughRequest request, API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughResponse _response)
        {
            return new API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughResponse() { ReturnCode = "000" };
        }


        /// <summary>
        ///  3.1	API31 – Đặt lệnh giao dịch khớp lệnh
        /// </summary>
        public async Task<API31OrderNewAutomaticOrderMatchingResponse> API31OrderNewAutomaticOrderMatching_BU(API31OrderNewAutomaticOrderMatchingRequest request, API31OrderNewAutomaticOrderMatchingResponse _response)
        {
            return new API31OrderNewAutomaticOrderMatchingResponse() { ReturnCode = "000" };
        }

        /// <summary>
        ///  3.2	API32 – Sửa lệnh giao dịch khớp lệnh
        /// </summary>
        public async Task<API32OrderReplaceAutomaticOrderMatchingResponse> API32OrderReplaceAutomaticOrderMatching_BU(API32OrderReplaceAutomaticOrderMatchingRequest request, API32OrderReplaceAutomaticOrderMatchingResponse _response)
        {
            return new API32OrderReplaceAutomaticOrderMatchingResponse() { ReturnCode = "000" };
        }

        /// <summary>
        ///  3.3	API33 – Hủy lệnh giao dịch khớp lệnh
        /// </summary>
        public async Task<API33OrderCancelAutomaticOrderMatchingResponse> API33OrderCancelAutomaticOrderMatching_BU(API33OrderCancelAutomaticOrderMatchingRequest request, API33OrderCancelAutomaticOrderMatchingResponse _response)
        {
            return new API33OrderCancelAutomaticOrderMatchingResponse() { ReturnCode = "000" };
        }

        public void StopReceiveApi()
        {
            
        }

        public void RecoverData()
        {

        }
    }
}