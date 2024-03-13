using BusinessProcessAPIReq.RequestModels;
using CommonLib;
using FluentValidation;
using static CommonLib.CommonData;

namespace APIServer.Validation
{
    /// <summary>
    /// 2.15	API27 – Hủy lệnh thỏa thuận Repos đã thực hiện trong ngày (hủy GD Repos)
    /// </summary>
    public class API27OrderCancelDeal1stTransactionReposCommonPutthroughValidator : AbstractValidator<API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest>
    {
        public API27OrderCancelDeal1stTransactionReposCommonPutthroughValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

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
                if (checkQuoteType != QuoteTypeRepos.LENH_SUA)
                {
                    return false;
                }
                return true;
            }).WithMessage($"'QuoteType' is incorrect").WithName("QuoteType").WithErrorCode(ErrorCodeDefine.QuoteType_IsValid);

            //
            RuleFor(x => x.OrderType).Must((s, checkOrderType) =>
            {
                if (checkOrderType != ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO && checkOrderType != ORDER_ORDERTYPE.BCGD_REPOS)
                {
                    return false;
                }
                return true;
            }).WithMessage($"'OrderType' is incorrect").WithName("OrderType").WithErrorCode(ErrorCodeDefine.OrderType_IsValid);
            //
            RuleFor(x => x.Side).Must((s, checkSide) =>
            {
                if (string.IsNullOrWhiteSpace(checkSide) || (checkSide != ORDER_SIDE.SIDE_BUY && checkSide != ORDER_SIDE.SIDE_SELL))
                {
                    return false;
                }
                return true;
            }).WithMessage($"'Side' must be '{ORDER_SIDE.SIDE_BUY}' or '{ORDER_SIDE.SIDE_SELL}'").WithName("Side").WithErrorCode(ErrorCodeDefine.Side_IsValid);
        }
    }
}