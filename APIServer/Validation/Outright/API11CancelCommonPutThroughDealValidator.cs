﻿using CommonLib;
using FluentValidation;
using BusinessProcessAPIReq.RequestModels;
using static CommonLib.CommonData;

namespace APIServer.Validation
{
    // 1.11	API11: API hủy lệnh thỏa thuận Outright đã thực hiện
    public class API11CancelCommonPutThroughDealValidator : AbstractValidator<API11CancelCommonPutThroughDealRequest>
    {
        public API11CancelCommonPutThroughDealValidator()
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
                   if (string.IsNullOrWhiteSpace(checkOrderType) || (checkOrderType != ORDER_ORDERTYPE.BCGDO && checkOrderType != ORDER_ORDERTYPE.TTDTO))
                   {
                       return false;
                   }
                   return true;
               })
                .WithMessage($"'OrderType' is incorrect").WithName("OrderType").WithErrorCode(ErrorCodeDefine.OrderType_IsValid);
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
            RuleFor(x => x.CrossType).Must((s, checkCrossType) =>
            {
                if (checkCrossType != ORDER_CrossType.HUY_LENH_THOA_THUAN_DA_THUC_HIEN)
                {
                    return false;
                }
                return true;
            })
           .WithMessage($"'CrossType' is incorrect").WithName("CrossType").WithErrorCode(ErrorCodeDefine.CrossType_IsValid);
        }
    }
}