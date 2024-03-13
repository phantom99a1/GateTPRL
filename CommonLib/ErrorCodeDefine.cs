namespace CommonLib
{
    public class ErrorCodeDefine
    {
        public const string Success = "000";
        public const string Error_Application = "-999";
        public const string PortMonitor_Isvalid = "404";
        public const string Error_UnAuthorized = "401";
        public const string OrderNo_IsValid = "001";
        public const string OrderType_IsValid = "002";
        public const string Side_IsValid = "003";
        public const string Price_IsValid = "004";
        public const string OrderQty_IsValid = "005";
        public const string SettleDate_IsValid = "006";
        public const string SettleMethod_IsValid = "007";
        public const string IsVisible_IsValid = "008";
        public const string CrossType_IsValid = "009";
        public const string EffectiveTime_IsValid = "010";
        public const string RegistID_IsValid = "011";
        public const string ClientID_IsValid = "012";
        public const string Symbol_IsValid = "013";
        public const string RefExchangeID_IsValid = "014";
        public const string ClientIDCounterFirm_IsValid = "015";
        public const string MemberCounterFirm_IsValid = "016";
        public const string Market_Close = "17";
        public const string Gateway_had_no_Session = "18";
        public const string OrderNo_Duplicated = "019";

        public const string OrderNoAPI_IsValid = "023";
        public const string QuoteType_IsValid = "024";
        public const string OrderValue_IsValid = "025";
        public const string EndDate_IsValid = "026";
        public const string RepurchaseTerm_IsValid = "027";
        public const string Text_IsValid = "028";

        public const string RepurchaseRate_IsValid = "030";
        public const string HedgeRate_IsValid = "031";
        public const string NoSide_IsValid = "032";
        public const string NumSide_IsValid = "033";
        public const string MergePrice_IsValid = "034";
        public const string OrderQtyMM2_IsValid = "035";
        public const string PriceQtyMM2_IsValid = "036";
        public const string OrgOrderQty_IsValid = "037";
        public const string SpecialType_IsValid = "038";
        public const string PriceType_IsValid = "040";

	}

    public class ErrorTextDefine
    {
        public const string Success = "Thanh cong";
        public const string Error_Application = "ERROR_APPLICATION";
        public const string Error_UnAuthorized = "ERROR_UNAUTHORIZED";
        public const string OrderNo_IsValid = "'OrderNo' is not null";
        public const string OrderType_IsValid = "'OrderType' is incorrect";
        public const string Side_IsValid = "'Side' must be 'B' or 'S'";
        public const string Price_IsValid = "'Price' must be bigger than 0";
        public const string OrderQty_IsValid = "'OrderQty' must be bigger than 0";
        public const string SettleDate_IsValid = "'SettleDate' is incorrect (format is yyyyMMdd  and length is 8";
        public const string SettleMethod_IsValid = "'SettleMethod' is incorrect";
        public const string IsVisible_IsValid = "'IsVisible' is incorrect";
        public const string CrossType_IsValid = "'CrossType' is incorrect";
        public const string EffectiveTime_IsValid = "'EffectiveTime' is incorrect (format is yyyyMMdd  and length is 8)";
        public const string RegistID_IsValid = "'RegistID' is not null";
        public const string ClientID_IsValid = "'ClientID' is not null";
        public const string Symbol_IsValid = "'Symbol' is not null";
        public const string RefExchangeID_IsValid = "'RefExchangeID' is not null";
        public const string ClientIDCounterFirm_IsValid = "'ClientIDCounterFirm' is not null";
        public const string MemberCounterFirm_IsValid = "'MemberCounterFirm' is not null";
        public const string Market_Close = "Market close";
        public const string Gateway_had_no_Session = "Bond DMA Gateway has no session";
        public const string OrderNo_Duplicated = "'OrderNo' had been duplicated";
    }
}