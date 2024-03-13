using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperReposBCGDCancel : MessageHelper
    {
        private MessageReposBCGDCancel c_message;

        #region #Tag const

        private const int TAG_ClOrdID = 11;
        private const int TAG_OrgOrderID = 198;
        private const int TAG_QuoteType = 563;
        private const int TAG_OrdType = 40;
        private const int TAG_Side = 54;

        #endregion #Tag const

        public MessageHelperReposBCGDCancel(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageReposBCGDCancel)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);

            sb.Append(TAG_ClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);
            sb.Append(TAG_OrgOrderID.ToString()).Append(Common.DELIMIT).Append(c_message.OrgOrderID).Append(Common.SOH);
            sb.Append(TAG_QuoteType.ToString()).Append(Common.DELIMIT).Append(c_message.QuoteType).Append(Common.SOH);
            sb.Append(TAG_OrdType.ToString()).Append(Common.DELIMIT).Append(c_message.OrdType).Append(Common.SOH);
            sb.Append(TAG_Side.ToString()).Append(Common.DELIMIT).Append(c_message.Side).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (TAG_ClOrdID == field.Tag)
                c_message.ClOrdID = field.Value;
            else if (TAG_OrgOrderID == field.Tag)
                c_message.OrgOrderID = field.Value;
            else if (TAG_QuoteType == field.Tag)
            {
                bool _isSuccess = false;
                c_message.QuoteType = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_OrdType == field.Tag)
            {
                if (field.Value.Length > 0)
                    c_message.OrdType = field.Value;
            }
            else if (TAG_Side == field.Tag)
            {
                bool _isSuccess = false;
                c_message.Side = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}