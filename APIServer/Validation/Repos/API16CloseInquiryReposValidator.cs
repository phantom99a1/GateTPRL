using CommonLib;
using FluentValidation;
using BusinessProcessAPIReq.RequestModels;
using static CommonLib.CommonData;

namespace APIServer.Validation
{
    /// <summary>
    /// 2.4	API16 – Đóng lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
    /// </summary>
    public class API16CloseInquiryReposValidator : AbstractValidator<API16CloseInquiryReposRequest>
    {
        public API16CloseInquiryReposValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            //
            RuleFor(x => x.OrderNo).Must((s, checkOrderNo) =>
            {
                if (string.IsNullOrWhiteSpace(checkOrderNo))
                {
                    return false;
                }
                return true;
            }).WithMessage("'OrderNo' is not null").WithName("OrderNo").WithErrorCode(ErrorCodeDefine.OrderNo_IsValid);
            //
            RuleFor(x => x.RefExchangeID).Must((s, checkRefExchangeID) =>
            {
                if (string.IsNullOrWhiteSpace(checkRefExchangeID))
                {
                    return false;
                }
                return true;
            }).WithMessage("'RefExchangeID' is not null").WithName("RefExchangeID").WithErrorCode(ErrorCodeDefine.RefExchangeID_IsValid);

            //
            RuleFor(x => x.QuoteType).Must((s, checkQuoteType) =>
            {
                if (checkQuoteType != QuoteType.LENH_DONG_INQUIRY)
                {
                    return false;
                }
                return true;
            }).WithMessage($"'QuoteType' is incorrect").WithName("QuoteType").WithErrorCode(ErrorCodeDefine.QuoteType_IsValid);

            //
            RuleFor(x => x.OrderType).Must((s, checkOrderType) =>
            {
                if (checkOrderType != ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO)
                {
                    return false;
                }
                return true;
            }).WithMessage($"'OrderType' is incorrect").WithName("OrderType").WithErrorCode(ErrorCodeDefine.OrderType_IsValid);
        }
    }
}