using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperSecurityStatusRequest : MessageHelper
    {
        private MessageSecurityStatusRequest c_message;

        public MessageHelperSecurityStatusRequest(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageSecurityStatusRequest)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);
            sb.Append(MessageSecurityStatusRequest.TAG_SecurityStatusReqID.ToString()).Append(Common.DELIMIT).Append(c_message.SecurityStatusReqID).Append(Common.SOH);
            if (c_message.SubscriptionRequestType != '\0')
                sb.Append(MessageSecurityStatusRequest.TAG_SubscriptionRequestType.ToString()).Append(Common.DELIMIT).Append(c_message.SubscriptionRequestType).Append(Common.SOH);
            else
                sb.Append(MessageSecurityStatusRequest.TAG_SubscriptionRequestType.ToString()).Append(Common.DELIMIT).Append("2").Append(Common.SOH);
            if (c_message.Symbol != null && c_message.Symbol.Length > 0)
                sb.Append(MessageSecurityStatusRequest.TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(c_message.Symbol).Append(Common.SOH);

        }

        public override bool ParseField(Field field)
        {
            if (MessageSecurityStatusRequest.TAG_SecurityStatusReqID == field.Tag)
                c_message.SecurityStatusReqID = field.Value;
            if (MessageSecurityStatusRequest.TAG_SubscriptionRequestType == field.Tag)
            {
                if (field.Value.Length > 0)
                    c_message.SubscriptionRequestType = field.Value[0];
            }
            if (MessageSecurityStatusRequest.TAG_Symbol == field.Tag)
                c_message.Symbol = field.Value;
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}
