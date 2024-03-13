using BusinessProcessAPIReq.RequestModels;
using CommonLib;
using FluentValidation;
using static CommonLib.CommonData;

namespace APIServer.Validation
{
    /// <summary>
    /// 3.1	API31 – Đặt lệnh giao dịch khớp lệnh
    /// </summary>
    public class API31OrderNewAutomaticOrderMatchingValidator : AbstractValidator<API31OrderNewAutomaticOrderMatchingRequest>
    {
        public API31OrderNewAutomaticOrderMatchingValidator()
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
            RuleFor(x => x.Side).Must((s, checkSide) =>
            {
                if (string.IsNullOrWhiteSpace(checkSide) || (checkSide != ORDER_SIDE.SIDE_BUY && checkSide != ORDER_SIDE.SIDE_SELL))
                {
                    return false;
                }
                return true;
            }).WithMessage($"'Side' must be '{ORDER_SIDE.SIDE_BUY}' or '{ORDER_SIDE.SIDE_SELL}'").WithName("Side").WithErrorCode(ErrorCodeDefine.Side_IsValid);

            //
            RuleFor(x => x.OrderType).Must((s, checkOrderType) =>
            {
                if (string.IsNullOrWhiteSpace(checkOrderType) || (checkOrderType != NORMAL_ORDERTYPE.LO && checkOrderType != NORMAL_ORDERTYPE.MTL && checkOrderType != NORMAL_ORDERTYPE.MAS && checkOrderType != NORMAL_ORDERTYPE.ATC && checkOrderType != NORMAL_ORDERTYPE.ATO && checkOrderType != NORMAL_ORDERTYPE.MAK && checkOrderType != NORMAL_ORDERTYPE.MOK && checkOrderType != NORMAL_ORDERTYPE.MM))
                {
                    return false;
                }
                return true;
            }).WithMessage($"'OrderType' is incorrect").WithName("OrderType").WithErrorCode(ErrorCodeDefine.OrderType_IsValid);
           

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
            RuleFor(x => x.Price).Must((s, checkPrice) =>
            {
                if (checkPrice < 0)
                {
                    return false;
                }
                return true;
            }).WithMessage("'Price' must be bigger than 0").WithName("Price").WithErrorCode(ErrorCodeDefine.Price_IsValid);
            //
            RuleFor(x => x.OrderType).Must((objRequest, checkOrderType) =>
            {
                if (checkOrderType == NORMAL_ORDERTYPE.MM && objRequest.SpecialType != ORDER_SPECIALTYPE.LENH_MM_YET_GIA_1_CHIEU && objRequest.SpecialType != ORDER_SPECIALTYPE.LENH_MM_YET_GIA_2_CHIEU && objRequest.SpecialType != ORDER_SPECIALTYPE.LENH_MM_2_CHIEU_THAY_THE)
                {
                    return false;
                }
                return true;
            }).WithMessage($"'SpecialType' is incorrect").WithName("SpecialType").WithErrorCode(ErrorCodeDefine.SpecialType_IsValid);
            //
            RuleFor(x => x.OrderType).Must((objRequest, checkOrderType) =>
            {
                if (checkOrderType == NORMAL_ORDERTYPE.MM && (objRequest.SpecialType == ORDER_SPECIALTYPE.LENH_MM_YET_GIA_2_CHIEU || objRequest.SpecialType == ORDER_SPECIALTYPE.LENH_MM_2_CHIEU_THAY_THE))
                {
                    if (objRequest.OrderQtyMM2 <= 0)
                    {
                        return false;
                    }
                }
                return true;
            }).WithMessage($"'OrderQtyMM2' must be bigger than 0").WithName("OrderQtyMM2").WithErrorCode(ErrorCodeDefine.OrderQtyMM2_IsValid);
            //
            RuleFor(x => x.OrderType).Must((objRequest, checkOrderType) =>
            {
                if (checkOrderType == NORMAL_ORDERTYPE.MM && (objRequest.SpecialType == ORDER_SPECIALTYPE.LENH_MM_YET_GIA_2_CHIEU || objRequest.SpecialType == ORDER_SPECIALTYPE.LENH_MM_2_CHIEU_THAY_THE))
                {
                    if (objRequest.PriceMM2 <= 0)
                    {
                        return false;
                    }
                }
                return true;
            }).WithMessage($"'PriceMM2' must be bigger than 0").WithName("PriceMM2").WithErrorCode(ErrorCodeDefine.PriceQtyMM2_IsValid);
            //
            RuleFor(x => x.OrderType).Must((objRequest, checkOrderType) =>
            {
                if (checkOrderType == NORMAL_ORDERTYPE.LO || checkOrderType == NORMAL_ORDERTYPE.MM)
                {
                    if (objRequest.Price <= 0)
                    {
                        return false;
                    }
                }
                return true;
            }).WithMessage($"'Price' must be bigger than 0").WithName("Price").WithErrorCode(ErrorCodeDefine.Price_IsValid);
            //
            RuleFor(x => x.OrderType).Must((objRequest, checkOrderType) =>
            {
                if (checkOrderType == NORMAL_ORDERTYPE.MTL || checkOrderType == NORMAL_ORDERTYPE.MAS || checkOrderType == NORMAL_ORDERTYPE.ATC || checkOrderType == NORMAL_ORDERTYPE.ATO || checkOrderType == NORMAL_ORDERTYPE.MAK || checkOrderType == NORMAL_ORDERTYPE.MOK)
                {
                    if (objRequest.Price != 0)
                    {
                        return false;
                    }
                }
                return true;
            }).WithMessage($"'Price' in market order must be 0").WithName("Price").WithErrorCode(ErrorCodeDefine.PriceType_IsValid);
        }
    }
}