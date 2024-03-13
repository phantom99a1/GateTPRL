using CommonLib;
using FluentValidation;
using BusinessProcessAPIReq.RequestModels;
using static CommonLib.CommonData;

namespace APIServer.Validation
{
    public partial class API2AcceptElectronicPutThroughValidator : AbstractValidator<API2AcceptElectronicPutThroughRequest>
    {
        public API2AcceptElectronicPutThroughValidator()
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
            RuleFor(x => x.ClientIDCounterFirm).Must((s, checkClientIDCounterFirm) =>
            {
                if (string.IsNullOrWhiteSpace(checkClientIDCounterFirm))
                {
                    return false;
                }
                return true;
            }).WithMessage($"'ClientIDCounterFirm' is not null").WithName("ClientIDCounterFirm").WithErrorCode(ErrorCodeDefine.ClientIDCounterFirm_IsValid);
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
            RuleFor(x => x.Side)
                 .Must((s, checkSide) =>
                 {
                     if (string.IsNullOrWhiteSpace(checkSide) || (checkSide != ORDER_SIDE.SIDE_BUY && checkSide != ORDER_SIDE.SIDE_SELL))
                     {
                         return false;
                     }
                     return true;
                 })
                 .WithMessage($"'Side' must be '{ORDER_SIDE.SIDE_BUY}' or '{ORDER_SIDE.SIDE_SELL}'").WithName("Side").WithErrorCode(ErrorCodeDefine.Side_IsValid);
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
            RuleFor(x => x.Price).Must((s, checkPrice) =>
            {
                if (checkPrice <= 0)
                {
                    return false;
                }
                return true;
            })
                .WithMessage("'Price' must be bigger than 0").WithName("Price").WithErrorCode(ErrorCodeDefine.Price_IsValid);
            //
            RuleFor(x => x.OrderQty).Must((s, checkOrderQtty) =>
            {
                if (checkOrderQtty <= 0)
                {
                    return false;
                }
                return true;
            })
                .WithMessage("'OrderQty' must be bigger than 0").WithName("OrderQty").WithErrorCode(ErrorCodeDefine.OrderQty_IsValid);
            //
            RuleFor(s => s.SettleDate).Must((s, checkSettleDate) =>
            {
                if (string.IsNullOrWhiteSpace(checkSettleDate) || Utils.Validator_Data_Date(checkSettleDate) == false || checkSettleDate.Length < 8)
                {
                    return false;
                }
                return true;
            }).
                WithMessage("'SettleDate' is incorrect (format is yyyyMMdd and length is 8)").WithName("SettleDate").WithErrorCode(ErrorCodeDefine.SettleDate_IsValid);
            //
            RuleFor(x => x.SettleMethod).Must((s, checkSettlMethod) =>
            {
                if (checkSettlMethod != ORDER_SETTLMETHOD.PAYMENT_NOW && checkSettlMethod != ORDER_SETTLMETHOD.PAYMENT_LASTDATE)
                {
                    return false;
                }
                return true;
            })
               .WithMessage($"'SettleMethod' is incorrect").WithName("SettlMethod").WithErrorCode(ErrorCodeDefine.SettleMethod_IsValid);
        }
    }
}