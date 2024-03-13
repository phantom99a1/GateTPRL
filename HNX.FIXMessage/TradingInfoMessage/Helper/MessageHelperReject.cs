using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HNX.FIXMessage 
{
    internal class MessageHelperReject : MessageHelper
    {
        
        private MessageReject c_message;

        public MessageHelperReject(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageReject)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);
            sb.Append(MessageReject.TAG_SessionRejectReason).Append(Common.DELIMIT).Append(c_message.SessionRejectReason).Append(Common.SOH);
            sb.Append(MessageReject.TAG_RefSeqNum).Append(Common.DELIMIT).Append(c_message.RefSeqNum).Append(Common.SOH);
        }

       public override bool ParseField(Field field)
        {
            if (MessageReject.TAG_SessionRejectReason == field.Tag)
            {
                bool _isSuccess = false;
                c_message.SessionRejectReason = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageReject.TAG_RefSeqNum == field.Tag)
            {
                bool _isSuccess = false;
                c_message.RefSeqNum = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}
