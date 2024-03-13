using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperReposInquiry : MessageHelper
    {
        private MessageReposInquiry c_message;

        #region #Tag const

        private const int TAG_ClOrdID = 11;
        private const int TAG_Symbol = 55;
        private const int TAG_QuoteType = 537;
        private const int TAG_OrdType = 40;
        private const int TAG_Side = 54;
        private const int TAG_OrderQty = 38;
        private const int TAG_EffectiveTime = 168;
        private const int TAG_SettlMethod = 6363;
        private const int TAG_SettlDate = 64;
        private const int TAG_SettlDate2 = 193;
        private const int TAG_EndDate = 917;
        private const int TAG_RepurchaseTerm = 226;
        private const int TAG_RegistID = 513;
        private const int TAG_RFQReqID = 644;

        #endregion #Tag const

        public MessageHelperReposInquiry(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageReposInquiry)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);

            sb.Append(TAG_ClOrdID).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);
            sb.Append(TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(c_message.Symbol).Append(Common.SOH);
            sb.Append(TAG_QuoteType.ToString()).Append(Common.DELIMIT).Append(c_message.QuoteType).Append(Common.SOH);
            //
            sb.Append(TAG_OrdType.ToString()).Append(Common.DELIMIT).Append(c_message.OrdType).Append(Common.SOH);
            if (c_message.IsAPI15_16 == false)
                sb.Append(TAG_Side.ToString()).Append(Common.DELIMIT).Append(c_message.Side).Append(Common.SOH);
            if (c_message.IsAPI15_16 == false)
                sb.Append(TAG_OrderQty.ToString()).Append(Common.DELIMIT).Append(c_message.OrderQty).Append(Common.SOH);
            //
            if (c_message.IsAPI15_16 == false)
                sb.Append(TAG_EffectiveTime.ToString()).Append(Common.DELIMIT).Append(c_message.EffectiveTime).Append(Common.SOH);
            if (c_message.IsAPI15_16 == false)
                sb.Append(TAG_SettlMethod.ToString()).Append(Common.DELIMIT).Append(c_message.SettlMethod).Append(Common.SOH);
            if (c_message.IsAPI15_16 == false)
                sb.Append(TAG_SettlDate.ToString()).Append(Common.DELIMIT).Append(c_message.SettlDate).Append(Common.SOH);
            //
            if (c_message.IsAPI15_16 == false)
                sb.Append(TAG_SettlDate2.ToString()).Append(Common.DELIMIT).Append(c_message.SettlDate2).Append(Common.SOH);
            if (c_message.IsAPI15_16 == false)
                sb.Append(TAG_EndDate.ToString()).Append(Common.DELIMIT).Append(c_message.EndDate).Append(Common.SOH);
            if (c_message.IsAPI15_16 == false)
                sb.Append(TAG_RepurchaseTerm.ToString()).Append(Common.DELIMIT).Append(c_message.RepurchaseTerm).Append(Common.SOH);
            if (c_message.IsAPI15_16 == false)
                sb.Append(TAG_RegistID.ToString()).Append(Common.DELIMIT).Append(c_message.RegistID).Append(Common.SOH);

            sb.Append(TAG_RFQReqID.ToString()).Append(Common.DELIMIT).Append(c_message.RFQReqID).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (TAG_ClOrdID == field.Tag)
                c_message.ClOrdID = field.Value;
            else if (TAG_Symbol == field.Tag)
                c_message.Symbol = field.Value;
            else if (TAG_QuoteType == field.Tag)
            {
                bool _isSuccess = false;
                c_message.QuoteType = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_OrdType == field.Tag)
                c_message.OrdType = field.Value;
            else if (TAG_Side == field.Tag)
            {
                bool _isSuccess = false;
                c_message.Side = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_OrderQty == field.Tag)
            {
                bool _isSuccess = false;
                c_message.OrderQty = Utils.Convert.ParseDouble(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_EffectiveTime == field.Tag)
                c_message.EffectiveTime = field.Value;
            else if (TAG_SettlMethod == field.Tag)
            {
                bool _isSuccess = false;
                c_message.SettlMethod = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_SettlDate == field.Tag)
                c_message.SettlDate = field.Value;
            else if (TAG_SettlDate2 == field.Tag)
                c_message.SettlDate2 = field.Value;
            else if (TAG_EndDate == field.Tag)
                c_message.EndDate = field.Value;
            else if (TAG_RepurchaseTerm == field.Tag)
            {
                bool _isSuccess = false;
                c_message.RepurchaseTerm = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_RegistID == field.Tag)
                c_message.RegistID = field.Value;
            else if (TAG_RFQReqID == field.Tag)
                c_message.RFQReqID = field.Value;
            else if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}