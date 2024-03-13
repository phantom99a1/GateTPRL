using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperER_OrderReject : MessageHelper
    {
        private MessageER_OrderReject c_message;

        public MessageHelperER_OrderReject(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageER_OrderReject)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);

            sb.Append(MessageER_OrderReject.TAG_OrdStatus.ToString()).Append(Common.DELIMIT).Append(c_message.OrdStatus).Append(Common.SOH);
            if (c_message.ClOrdID != "")
                sb.Append(MessageER_OrderReject.TAG_ClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);
            sb.Append(MessageER_OrderReject.TAG_OrderID.ToString()).Append(Common.DELIMIT).Append(c_message.OrderID).Append(Common.SOH);
            //sb.Append(MessageER_OrderReject.TAG_TransactTime.ToString()).Append(Common.DELIMIT).Append(Utils.Convert.ToFIXUTCTimestamp(c_message.TransactTime)).Append(Common.SOH);
            //
            sb.Append(MessageER_OrderReject.TAG_OrdRejReason.ToString()).Append(Common.DELIMIT).Append(c_message.OrdRejReason).Append(Common.SOH);
            sb.Append(MessageER_OrderReject.TAG_UnderlyingLastQty.ToString()).Append(Common.DELIMIT).Append(c_message.UnderlyingLastQty).Append(Common.SOH);
            sb.Append(MessageER_OrderReject.TAG_Side.ToString()).Append(Common.DELIMIT).Append(c_message.Side).Append(Common.SOH);
            sb.Append(MessageER_OrderReject.TAG_OrdType.ToString()).Append(Common.DELIMIT).Append(c_message.OrdType).Append(Common.SOH);

        }

        public override bool ParseField(Field field)
        {

            if (MessageER_OrderReject.TAG_OrdStatus == field.Tag)
                c_message.OrdStatus = field.Value;
            else if (MessageER_OrderReject.TAG_ClOrdID == field.Tag)
                c_message.ClOrdID = field.Value;
            else if (MessageER_OrderReject.TAG_OrderID == field.Tag)
                c_message.OrderID = field.Value;
            //else if (MessageER_OrderReject.TAG_TransactTime == field.Tag)
            //    c_message.TransactTime = Utils.Convert.FromFIXUTCTimestamp(field.Value);
            ////
            else if (MessageER_OrderReject.TAG_OrdRejReason == field.Tag)
            {
                bool _isSuccess = false;
                c_message.OrdRejReason = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageER_OrderReject.TAG_UnderlyingLastQty == field.Tag)
            {
                bool _isSuccess = false;
                c_message.UnderlyingLastQty = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageER_OrderReject.TAG_Side == field.Tag)
                c_message.Side = field.Value;
            else if (MessageER_OrderReject.TAG_OrdType == field.Tag)
                c_message.OrdType = field.Value;
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}
