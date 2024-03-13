using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperCancelOrder : MessageHelper
    {
        private MessageCancelOrder c_message;

        #region #Tag const

        private const int TAG_ClOrdID = 11;
        private const int TAG_OrigClOrdID = 41;
        private const int TAG_Symbol = 55;

        #endregion #Tag const

        public MessageHelperCancelOrder(FIXMessageBase message)
           : base(message)
        {
            c_message = (MessageCancelOrder)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);

            sb.Append(TAG_ClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);
            sb.Append(TAG_OrigClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.OrigClOrdID).Append(Common.SOH);
            sb.Append(TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(c_message.Symbol).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (TAG_ClOrdID == field.Tag)
                c_message.ClOrdID = field.Value;
            if (TAG_OrigClOrdID == field.Tag)
                c_message.OrigClOrdID = field.Value;
            else if (TAG_Symbol == field.Tag)
                c_message.Symbol = field.Value;
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}