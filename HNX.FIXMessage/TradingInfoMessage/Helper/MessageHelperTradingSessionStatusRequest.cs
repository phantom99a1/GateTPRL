using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperTradingSessionStatusRequest : MessageHelper
    {
        private MessageTradingSessionStatusRequest c_message;

        public MessageHelperTradingSessionStatusRequest(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageTradingSessionStatusRequest)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);
            sb.Append(MessageTradingSessionStatusRequest.TAG_TradSesReqID.ToString()).Append(Common.DELIMIT).Append(c_message.TradSesReqID).Append(Common.SOH);
            if (c_message.SubscriptionRequestType != '\0')
                sb.Append(MessageTradingSessionStatusRequest.TAG_SubscriptionRequestType.ToString()).Append(Common.DELIMIT).Append(c_message.SubscriptionRequestType).Append(Common.SOH);
            else
                sb.Append(MessageTradingSessionStatusRequest.TAG_SubscriptionRequestType.ToString()).Append(Common.DELIMIT).Append("2").Append(Common.SOH);
            sb.Append(MessageTradingSessionStatusRequest.TAG_TradSesMode.ToString()).Append(Common.DELIMIT).Append(c_message.TradSesMode).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (MessageTradingSessionStatusRequest.TAG_TradSesReqID == field.Tag)
                c_message.TradSesReqID = field.Value;
            if (MessageTradingSessionStatusRequest.TAG_SubscriptionRequestType == field.Tag)
            {
                if (field.Value.Length > 0)
                    c_message.SubscriptionRequestType = field.Value[0];
            }
            if (MessageTradingSessionStatusRequest.TAG_TradSesMode == field.Tag)
            {
                bool _isSuccess = false;
                c_message.TradSesMode = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}
