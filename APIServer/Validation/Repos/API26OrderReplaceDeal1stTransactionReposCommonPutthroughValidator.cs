using BusinessProcessAPIReq.RequestModels;
using CommonLib;
using FluentValidation;
using static CommonLib.CommonData;

namespace APIServer.Validation
{
    /// <summary>
    /// 2.14	API26 – Phản hồi sửa lệnh thỏa thuận Repos đã thực hiện trong ngày (phản hồi sửa GD Repos)
    /// </summary>
    public class API26OrderReplaceDeal1stTransactionReposCommonPutthroughValidator : AbstractValidator<API26OrderReplaceDeal1stTransactionReposCommonPutthroughRequest>
    {
        public API26OrderReplaceDeal1stTransactionReposCommonPutthroughValidator()
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
                if (checkQuoteType != QuoteType_BCGDRepos.LENH_XAC_NHAN_BCGD_REPOS && checkQuoteType != QuoteType_BCGDRepos.LENH_TUCHOI_BCGD_REPOS)
                {
                    return false;
                }
                return true;
            }).WithMessage($"'QuoteType' is incorrect").WithName("QuoteType").WithErrorCode(ErrorCodeDefine.QuoteType_IsValid);

            //
            RuleFor(x => x.OrderType).Must((s, checkOrderType) =>
            {
                if (checkOrderType != ORDER_ORDERTYPE.BCGD_REPOS && checkOrderType != ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO)
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
            RuleFor(s => s.ClientID).Must((s, checkClientID) =>
            {
                if (string.IsNullOrWhiteSpace(checkClientID))
                {
                    return false;
                }
                return true;
            }).WithMessage("'ClientID' is not null").WithName("ClientID").WithErrorCode(ErrorCodeDefine.ClientID_IsValid);
            //
            RuleFor(x => x.MemberCounterFirm).Must((s, checkMemberCounterFirm) =>
            {
                if (string.IsNullOrWhiteSpace(checkMemberCounterFirm))
                {
                    return false;
                }
                return true;
            }).WithMessage($"'MemberCounterFirm' is not null").WithName("MemberCounterFirm").WithErrorCode(ErrorCodeDefine.MemberCounterFirm_IsValid);
            //
            RuleFor(x => x.MemberCounterFirm).Must((objRequest, checkMemberCounterFirm) =>
            {
                if (checkMemberCounterFirm == ConfigData.FirmID && string.IsNullOrWhiteSpace(objRequest.ClientIDCounterFirm))
                {
                    return false;
                }
                return true;
            }).WithMessage($"'ClientIDCounterFirm' is not null").WithName("ClientIDCounterFirm").WithErrorCode(ErrorCodeDefine.ClientIDCounterFirm_IsValid);
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
                if (checkSettlMethod != ORDER_SETTLMETHOD.PAYMENT_LASTDATE)
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
            RuleFor(x => x.RepurchaseRate).Must((s, checkRepurchaseRate) =>
            {
                if (checkRepurchaseRate < 0)
                {
                    return false;
                }
                return true;
            }).WithMessage($"'RepurchaseRate' must not be a negative number").WithName("RepurchaseRate").WithErrorCode(ErrorCodeDefine.RepurchaseRate_IsValid);
            //
            RuleFor(x => x.NoSide).Must((s, checkNoSide) =>
            {
                if (checkNoSide <= 0)
                {
                    return false;
                }
                return true;
            }).WithMessage($"'NoSide' must be bigger than 0").WithName("NoSide").WithErrorCode(ErrorCodeDefine.NoSide_IsValid);
            //
            RuleForEach(x => x.SymbolFirmInfo).SetValidator(new API26SymbolFirmInfoValidator());
        }

        public class API26SymbolFirmInfoValidator : AbstractValidator<APIReposSideList>
        {
            public API26SymbolFirmInfoValidator()
            {
                //
                RuleFor(x => x.NumSide).Must((s, checkNumSide) =>
                {
                    if (checkNumSide <= 0)
                    {
                        return false;
                    }
                    return true;
                }).WithMessage($"'NumSide' must be bigger than 0").WithName("NumSide").WithErrorCode(ErrorCodeDefine.NumSide_IsValid);
                //
                RuleFor(s => s.Symbol).Must((s, checkSymbol) =>
                {
                    if (string.IsNullOrWhiteSpace(checkSymbol))
                    {
                        return false;
                    }
                    return true;
                }).
                    WithMessage("'Symbol' is not null").WithName("Symbol").WithErrorCode(ErrorCodeDefine.Symbol_IsValid);
                //
                RuleFor(x => x.OrderQty).Must((s, checkOrderQty) =>
                {
                    if (checkOrderQty <= 0)
                    {
                        return false;
                    }
                    return true;
                }).WithMessage($"'OrderQty' must be bigger than 0").WithName("OrderQty").WithErrorCode(ErrorCodeDefine.OrderQty_IsValid);
                //
                RuleFor(x => x.MergePrice).Must((s, checkMergePrice) =>
                {
                    if (checkMergePrice <= 0)
                    {
                        return false;
                    }
                    return true;
                }).WithMessage($"'MergePrice' must be bigger than 0").WithName("MergePrice").WithErrorCode(ErrorCodeDefine.MergePrice_IsValid);
                //
                RuleFor(x => x.HedgeRate).Must((s, checkHedgeRate) =>
                {
                    if (checkHedgeRate < 0)
                    {
                        return false;
                    }
                    return true;
                }).WithMessage($"'HedgeRate' must not be a negative number").WithName("HedgeRate").WithErrorCode(ErrorCodeDefine.HedgeRate_IsValid);
            }
        }
    }
}