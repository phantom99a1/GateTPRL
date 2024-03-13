using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperUserResponse : MessageHelper
    {
        private MessageUserResponse c_message;

        public MessageHelperUserResponse(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageUserResponse)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);

            if (c_message.Username != null && c_message.Username.Length > 0)
                sb.Append(MessageUserResponse.TAG_Username).Append(Common.DELIMIT).Append(c_message.Username).Append(Common.SOH);
            if (c_message.UserRequestID != null && c_message.UserRequestID.Length > 0)
                sb.Append(MessageUserResponse.TAG_UserRequestID).Append(Common.DELIMIT).Append(c_message.UserRequestID).Append(Common.SOH);
            sb.Append(MessageUserResponse.TAG_UserStatus).Append(Common.DELIMIT).Append(c_message.UserStatus).Append(Common.SOH);
            sb.Append(MessageUserResponse.TAG_UserStatusText).Append(Common.DELIMIT).Append(c_message.UserStatusText).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (MessageUserResponse.TAG_Username == field.Tag)
                c_message.Username = field.Value;
            else if (MessageUserResponse.TAG_UserRequestID == field.Tag)
                c_message.UserRequestID = field.Value;
            else if (MessageUserResponse.TAG_UserStatus == field.Tag)
            {
                bool _isSuccess = false;
                c_message.UserStatus = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageUserResponse.TAG_UserStatusText == field.Tag)
                c_message.UserStatusText = field.Value;
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}