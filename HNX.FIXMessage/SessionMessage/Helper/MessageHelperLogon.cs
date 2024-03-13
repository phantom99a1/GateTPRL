using System.Text;

namespace HNX.FIXMessage
{
    internal class MessageHelperLogon : MessageHelper
    {


        private MessageLogon c_message;

        public MessageHelperLogon(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageLogon)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);
            if (c_message.EncryptMethod >= 0)
                sb.Append(MessageLogon.TAG_EncryptMethod).Append(Common.DELIMIT).Append(c_message.EncryptMethod).Append(Common.SOH);
            if (c_message.HeartBtInt > 0)
                sb.Append(MessageLogon.TAG_HeartBtInt).Append(Common.DELIMIT).Append(c_message.HeartBtInt).Append(Common.SOH);
            if (c_message.Username != null && c_message.Username.Length > 0)
                sb.Append(MessageLogon.TAG_Username).Append(Common.DELIMIT).Append(c_message.Username).Append(Common.SOH);
            if (c_message.Password != null && c_message.Password.Length > 0)
                sb.Append(MessageLogon.TAG_Password).Append(Common.DELIMIT).Append(c_message.Password).Append(Common.SOH);

        }

        public override bool ParseField(Field field)
        {
            if (MessageLogon.TAG_EncryptMethod == field.Tag)
            {
                bool _isSuccess = false;
                c_message.EncryptMethod = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageLogon.TAG_HeartBtInt == field.Tag)
            {
                bool _isSuccess = false;
                c_message.HeartBtInt = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageLogon.TAG_Username == field.Tag)
                c_message.Username = field.Value;
            else if (MessageLogon.TAG_Password == field.Tag)
                c_message.Password = field.Value;
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }

}
