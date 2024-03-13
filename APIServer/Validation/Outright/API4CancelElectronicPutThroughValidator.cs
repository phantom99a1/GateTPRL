using CommonLib;
using FluentValidation;
using BusinessProcessAPIReq.RequestModels;
using static CommonLib.CommonData;

namespace APIServer.Validation
{
    public class API4CancelElectronicPutThroughValidator : AbstractValidator<API4CancelElectronicPutThroughRequest>
    {
        public API4CancelElectronicPutThroughValidator()
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
            RuleFor(x => x.OrderType)
                .Must((s, checkOrderType) =>
                {
                    if (string.IsNullOrWhiteSpace(checkOrderType) || checkOrderType != ORDER_ORDERTYPE.TTDTO)
                    {
                            return false;
                    }
                    return true;
                })
                .WithMessage($"'OrderType' is incorrect").WithName("OrderType").WithErrorCode(ErrorCodeDefine.OrderType_IsValid);
            //
            RuleFor(x => x.Symbol).NotNull().NotEmpty().WithMessage($"'Symbol' is not null").WithName("Symbol").WithErrorCode(ErrorCodeDefine.Symbol_IsValid);
        }
    }
}