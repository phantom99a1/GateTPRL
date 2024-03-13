using BusinessProcessAPIReq.RequestModels;
using CommonLib;
using FluentValidation;

namespace APIServer.Validation
{
    public class API33OrderCancelAutomaticOrderMatchingValidator : AbstractValidator<API33OrderCancelAutomaticOrderMatchingRequest>
    {
        public API33OrderCancelAutomaticOrderMatchingValidator()
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
            }).WithMessage($"'RefExchangeID' is not null").WithName("RefExchangeID").WithErrorCode(ErrorCodeDefine.RefExchangeID_IsValid);
            //
            RuleFor(x => x.Symbol).Must((s, checkSymbol) =>
            {
                if (string.IsNullOrWhiteSpace(checkSymbol))
                {
                    return false;
                }
                return true;
            }).WithMessage($"'Symbol' is not null").WithName("Symbol").WithErrorCode(ErrorCodeDefine.Symbol_IsValid);
        }
    }
}