using BusinessProcessAPIReq.RequestModels;
using CommonLib;
using FluentValidation;
using static CommonLib.CommonData;

namespace APIServer.Validation
{
    /// <summary>
    /// 2.2	API14 – Sửa lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
    /// </summary>
    public class API14ReplaceInquiryReposValidator : AbstractValidator<API14ReplaceInquiryReposRequest>
    {
        public API14ReplaceInquiryReposValidator()
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
                    return false;
                return true;
            }).WithMessage("'RefExchangeID' is not null").WithName("RefExchangeID").WithErrorCode(ErrorCodeDefine.RefExchangeID_IsValid);

            //
            RuleFor(x => x.QuoteType).Must((s, checkQuoteType) =>
            {
                if (checkQuoteType != QuoteType.LENH_SUA_INQUIRY)
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
            RuleFor(x => x.OrderValue).Must((s, checkOrderValuee) =>
            {
                if (checkOrderValuee <= 0)
                {
                    return false;
                }
                return true;
            }).WithMessage($"'OrderValue' must be bigger than 0").WithName("OrderValue").WithErrorCode(ErrorCodeDefine.OrderValue_IsValid);
            //
            RuleFor(s => s.EffectiveTime).Must((s, checkEfectiveTime) =>
            {
                if (string.IsNullOrWhiteSpace(checkEfectiveTime) || Utils.Validator_Data_Date(checkEfectiveTime) == false || checkEfectiveTime.Length < 8)
                {
                    return false;
                }
                return true;
            }).WithMessage("'EffectiveTime' is incorrect (format is yyyyMMdd and length is 8)").WithName("EffectiveTime").WithErrorCode(ErrorCodeDefine.EffectiveTime_IsValid);
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
            //
            RuleFor(s => s.SettleDate1).Must((s, checkSettleDate1) =>
            {
                if (string.IsNullOrWhiteSpace(checkSettleDate1) || Utils.Validator_Data_Date(checkSettleDate1) == false || checkSettleDate1.Length < 8)
                {
                    return false;
                }
                return true;
            }).
                WithMessage("'SettleDate1' is incorrect (format is yyyyMMdd and length is 8)").WithName("SettleDate1").WithErrorCode(ErrorCodeDefine.SettleDate_IsValid);
            //
            RuleFor(s => s.SettleDate2).Must((s, checkSettleDate2) =>
            {
                if (string.IsNullOrWhiteSpace(checkSettleDate2) || Utils.Validator_Data_Date(checkSettleDate2) == false || checkSettleDate2.Length < 8)
                {
                    return false;
                }
                return true;
            }).
                WithMessage("'SettleDate2' is incorrect (format is yyyyMMdd and length is 8)").WithName("SettleDate2").WithErrorCode(ErrorCodeDefine.SettleDate_IsValid);
            //
            RuleFor(s => s.EndDate).Must((s, checkEndDate) =>
            {
                if (string.IsNullOrWhiteSpace(checkEndDate) || Utils.Validator_Data_Date(checkEndDate) == false || checkEndDate.Length < 8)
                {
                    return false;
                }
                return true;
            }).
                WithMessage("'EndDate' is incorrect (format is yyyyMMdd and length is 8)").WithName("EndDate").WithErrorCode(ErrorCodeDefine.EndDate_IsValid);
            //
            RuleFor(x => x.RepurchaseTerm).Must((s, checkRepurchaseTerm) =>
            {
                if (checkRepurchaseTerm <= 0)
                {
                    return false;
                }
                return true;
            }).WithMessage($"'RepurchaseTerm' must be bigger than 0").WithName("RepurchaseTerm").WithErrorCode(ErrorCodeDefine.RepurchaseTerm_IsValid);
            //
            RuleFor(x => x.RegistID).Must((s, checkRegistID) =>
            {
                if (string.IsNullOrWhiteSpace(checkRegistID))
                {
                    return false;
                }
                return true;
            }).WithMessage($"'RegistID' is not null").WithName("RegistID").WithErrorCode(ErrorCodeDefine.RegistID_IsValid);

            //
            RuleFor(x => x.Symbol).Must((objRequest, checkSymbol) =>
            {
                if (string.IsNullOrWhiteSpace(checkSymbol) && string.IsNullOrWhiteSpace(objRequest.Text))
                {
                    return false;
                }
                return true;
            }).WithMessage($"'Text' is not null").WithName("Text").WithErrorCode(ErrorCodeDefine.Text_IsValid);
        }
    }
}