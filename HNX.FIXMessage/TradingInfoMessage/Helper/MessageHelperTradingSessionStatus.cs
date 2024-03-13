using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperTradingSessionStatus : MessageHelper
    {
        private MessageTradingSessionStatus c_message;

        public MessageHelperTradingSessionStatus(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageTradingSessionStatus)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);
            sb.Append(MessageTradingSessionStatus.TAG_TradingSessionID.ToString()).Append(Common.DELIMIT).Append(c_message.TradingSessionID).Append(Common.SOH);
            sb.Append(MessageTradingSessionStatus.TAG_TradSesStatus.ToString()).Append(Common.DELIMIT).Append(c_message.TradSesStatus.ToString()).Append(Common.SOH);
            sb.Append(MessageTradingSessionStatus.TAG_TradSesStartTime.ToString()).Append(Common.DELIMIT).Append(Utils.Convert.ToFIXUTCTimestamp(c_message.TradSesStartTime)).Append(Common.SOH);
            sb.Append(MessageTradingSessionStatusRequest.TAG_TradSesMode.ToString()).Append(Common.DELIMIT).Append(c_message.TradSesMode).Append(Common.SOH);
            sb.Append(MessageTradingSessionStatusRequest.TAG_TradSesReqID.ToString()).Append(Common.DELIMIT).Append(c_message.TradSesReqID).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (MessageTradingSessionStatus.TAG_TradingSessionID == field.Tag)
                c_message.TradingSessionID = field.Value;
            else if (MessageTradingSessionStatus.TAG_TradSesStatus == field.Tag)
            {
                c_message.TradSesStatus = field.Value;
            }
            else if (MessageTradingSessionStatus.TAG_TradSesReqID == field.Tag)
            {
                c_message.TradSesReqID = field.Value;
            }
            else if (MessageTradingSessionStatus.TAG_TradSesStartTime == field.Tag)
            {
                c_message.TradSesStartTime = Utils.Convert.FromFIXUTCTimestamp(field.Value);
            }
            else if (MessageTradingSessionStatusRequest.TAG_TradSesMode == field.Tag)
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
