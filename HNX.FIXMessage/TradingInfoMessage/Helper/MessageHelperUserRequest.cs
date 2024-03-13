using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNX.FIXMessage
{
    public class MessageHelperUserRequest : MessageHelper
    {
        private MessageUserRequest c_message;

        public MessageHelperUserRequest(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageUserRequest)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);

            if (c_message.Username != null && c_message.Username.Length > 0)
                sb.Append(MessageUserRequest.TAG_Username).Append(Common.DELIMIT).Append(c_message.Username).Append(Common.SOH);
            if (c_message.Password != null && c_message.Password.Length > 0)
                sb.Append(MessageUserRequest.TAG_Password).Append(Common.DELIMIT).Append(c_message.Password).Append(Common.SOH);
            sb.Append(MessageUserRequest.TAG_UserRequestID).Append(Common.DELIMIT).Append(c_message.UserRequestID).Append(Common.SOH);
            sb.Append(MessageUserRequest.TAG_UserRequestType).Append(Common.DELIMIT).Append(c_message.UserRequestType).Append(Common.SOH);
            sb.Append(MessageUserRequest.TAG_NewPassword).Append(Common.DELIMIT).Append(c_message.NewPassword).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (MessageUserRequest.TAG_Username == field.Tag)
                c_message.Username = field.Value;
            else if (MessageUserRequest.TAG_Password == field.Tag)
                c_message.Password = field.Value;
            else if (MessageUserRequest.TAG_UserRequestID == field.Tag)
                c_message.UserRequestID = field.Value;
            else if (MessageUserRequest.TAG_UserRequestType == field.Tag)
            {
                bool _isSuccess = false;
                c_message.UserRequestType = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageUserRequest.TAG_NewPassword == field.Tag)
                c_message.NewPassword = field.Value;
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}
