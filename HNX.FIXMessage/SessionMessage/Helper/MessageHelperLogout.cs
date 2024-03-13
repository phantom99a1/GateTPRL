using System.Text;

namespace HNX.FIXMessage
{
    internal class MessageHelperLogout : MessageHelper
    {
        private MessageLogout _message;

        public MessageHelperLogout(FIXMessageBase message)
            : base(message)
        {
            _message = (MessageLogout)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);
            //sb.Append(MessageLogon.TAG_Username).Append(Common.DELIMIT).Append(_message.Username).Append(Common.SOH);
            //sb.Append(MessageLogon.TAG_Password).Append(Common.DELIMIT).Append(_message.Password).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            //if (MessageLogon.TAG_Username == field.Tag)
            //    _message.Username = field.Value;
            //else if (MessageLogon.TAG_Password == field.Tag)
            //    _message.Password = field.Value;
            //else
            if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}
