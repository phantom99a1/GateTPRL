using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperQuoteCancel : MessageHelper
    {
        private MessageQuoteCancel c_message;

        #region #Tag const

        private const int TAG_Account = 1; //tai khoan khach hang
        private const int TAG_QuoteID = 171;
        private const int TAG_QuoteCancelType = 298; // QuoteCancelType
        private const int TAG_OrdType = 40;                // loai lenh
        private const int TAG_Symbol = 55;// shl mới
        private const int TAG_ClOrdID = 11;

        #endregion #Tag const

        public MessageHelperQuoteCancel(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageQuoteCancel)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);
            sb.Append(TAG_ClOrdID).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);

            sb.Append(TAG_QuoteCancelType.ToString()).Append(Common.DELIMIT).Append(c_message.QuoteCancelType).Append(Common.SOH);
            sb.Append(TAG_QuoteID.ToString()).Append(Common.DELIMIT).Append(c_message.QuoteID).Append(Common.SOH);
            sb.Append(TAG_OrdType.ToString()).Append(Common.DELIMIT).Append(c_message.OrdType).Append(Common.SOH);
            sb.Append(TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(c_message.Symbol).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (FIXMessageBase.TAG_MsgType == field.Tag)
                c_message.MsgType = field.Value;
            else if (TAG_ClOrdID == field.Tag)
                c_message.ClOrdID = field.Value;
            else if (TAG_QuoteCancelType == field.Tag)
            {
                bool _isSuccess = false;
                c_message.QuoteCancelType = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_OrdType == field.Tag)
            {
                if (field.Value.Length > 0)
                    c_message.OrdType = field.Value;
            }
            else if (TAG_Symbol == field.Tag)
                c_message.Symbol = field.Value;
            else if (TAG_QuoteID == field.Tag)
                c_message.QuoteID = field.Value;
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}