using BusinessProcessAPIReq.RequestModels;
using CommonLib;
using FluentValidation;
using static CommonLib.CommonData;

namespace APIServer.Validation
{
	/// <summary>
	/// 2.8	API20 – Xác nhận lệnh điện tử tùy chọn Firm Repos
	/// </summary>
	public class API20OrderConfirmFirmReposValidator : AbstractValidator<API20OrderConfirmFirmReposRequest>
	{
		public API20OrderConfirmFirmReposValidator()
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
				if (checkQuoteType != QuoteTypeRepos.LENH_DAT)
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
			RuleFor(s => s.ClientID).Must((s, checkClientID) =>
			{
				if (string.IsNullOrWhiteSpace(checkClientID))
				{
					return false;
				}
				return true;
			}).WithMessage("'ClientID' is not null").WithName("ClientID").WithErrorCode(ErrorCodeDefine.ClientID_IsValid);
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
			RuleForEach(x => x.SymbolFirmInfo).SetValidator(new API20SymbolFirmInfoValidator());
		}
	}

	public class API20SymbolFirmInfoValidator : AbstractValidator<APIReposSideList>
	{
		public API20SymbolFirmInfoValidator()
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