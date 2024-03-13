using BusinessProcessAPIReq.RequestModels;
using CommonLib;
using FluentValidation;

namespace APIServer.Validation
{
    /// <summary>
    /// 3.2	API32 – Sửa lệnh giao dịch khớp lệnh
    /// </summary>
    public class API32OrderReplaceAutomaticOrderMatchingValidator : AbstractValidator<API32OrderReplaceAutomaticOrderMatchingRequest>
    {
        public API32OrderReplaceAutomaticOrderMatchingValidator()
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
            RuleFor(x => x.ClientID).Must((s, checkClientID) =>
            {
                if (string.IsNullOrWhiteSpace(checkClientID))
                {
                    return false;
                }
                return true;
            }).WithMessage($"'ClientID' is not null").WithName("ClientID").WithErrorCode(ErrorCodeDefine.ClientID_IsValid);
            //
            RuleFor(x => x.Symbol).Must((s, checkSymbol) =>
            {
                if (string.IsNullOrWhiteSpace(checkSymbol))
                {
                    return false;
                }
                return true;
            }).WithMessage($"'Symbol' is not null").WithName("Symbol").WithErrorCode(ErrorCodeDefine.Symbol_IsValid);
            //
            RuleFor(x => x.OrderQty).Must((s, checkOrderQtty) =>
            {
                if (checkOrderQtty <= 0)
                { 
                    return false; 
                }
                return true;
            }).WithMessage("'OrderQty' must be bigger than 0").WithName("OrderQty").WithErrorCode(ErrorCodeDefine.OrderQty_IsValid);
            //
            RuleFor(x => x.OrgOrderQty).Must((s, checkOrderQtty) =>
            {
                if (checkOrderQtty <= 0)
                {
                    return false;
                }
                return true;
            }).WithMessage("'OrgOrderQty' must be bigger than 0").WithName("OrgOrderQty").WithErrorCode(ErrorCodeDefine.OrgOrderQty_IsValid);
            //
            RuleFor(x => x.Price).Must((s, checkPrice) =>
            {
                if (checkPrice <= 0)
                {
                    return false;
                }
                return true;
            }).WithMessage("'Price' must be bigger than 0").WithName("Price").WithErrorCode(ErrorCodeDefine.Price_IsValid);
        }
    }
}