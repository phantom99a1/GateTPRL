using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperER_OrderCancel : MessageHelper
    {
        private MessageER_OrderCancel c_message;

        public MessageHelperER_OrderCancel(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageER_OrderCancel)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);

            sb.Append(MessageER_OrderCancel.TAG_OrdStatus.ToString()).Append(Common.DELIMIT).Append(c_message.OrdStatus).Append(Common.SOH);
            if (c_message.ClOrdID != "")
                sb.Append(MessageER_OrderCancel.TAG_ClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);
            if (c_message.OrigClOrdID != "")
                sb.Append(MessageER_OrderCancel.TAG_OrigClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.OrigClOrdID).Append(Common.SOH);
            sb.Append(MessageER_OrderCancel.TAG_OrderID.ToString()).Append(Common.DELIMIT).Append(c_message.OrderID).Append(Common.SOH);
            //sb.Append(MessageER_OrderCancel.TAG_TransactTime.ToString()).Append(Common.DELIMIT).Append(Utils.Convert.ToFIXUTCTimestamp(c_message.TransactTime)).Append(Common.SOH);

            if (c_message.LeavesQty > 0)
                sb.Append(MessageER_OrderCancel.TAG_LeavesQty.ToString()).Append(Common.DELIMIT).Append(c_message.LeavesQty).Append(Common.SOH);
            //sb.Append(MessageER_OrderCancel.TAG_CumQty.ToString()).Append(Common.DELIMIT).Append(c_message.CumQty).Append(Common.SOH);
            //
            if (c_message.Account != "")
                sb.Append(MessageER_OrderCancel.TAG_Account.ToString()).Append(Common.DELIMIT).Append(c_message.Account).Append(Common.SOH);
            sb.Append(MessageER_OrderCancel.TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(c_message.Symbol).Append(Common.SOH);
            sb.Append(MessageER_OrderCancel.TAG_Side.ToString()).Append(Common.DELIMIT).Append(c_message.Side).Append(Common.SOH);
            //sb.Append(MessageER_ExecOrder.TAG_OrderQty.ToString()).Append(Common.DELIMIT).Append(c_message.OrderQty).Append(Common.SOH);
            sb.Append(MessageER_OrderCancel.TAG_OrdType.ToString()).Append(Common.DELIMIT).Append(c_message.OrdType).Append(Common.SOH);
            sb.Append(MessageER_OrderCancel.TAG_Price.ToString()).Append(Common.DELIMIT).Append(c_message.Price).Append(Common.SOH);

        }

        public override bool ParseField(Field field)
        {

            if (MessageER_OrderCancel.TAG_OrdStatus == field.Tag)
                c_message.OrdStatus = field.Value;
            else if (MessageER_OrderCancel.TAG_ClOrdID == field.Tag)
                c_message.ClOrdID = field.Value;
            else if (MessageER_OrderCancel.TAG_OrigClOrdID == field.Tag)
                c_message.OrigClOrdID = field.Value;
            else if (MessageER_OrderCancel.TAG_OrderID == field.Tag)
                c_message.OrderID = field.Value;
            //else if (MessageER_OrderCancel.TAG_TransactTime == field.Tag)
            //    c_message.TransactTime = Utils.Convert.FromFIXUTCTimestamp(field.Value);
            else if (MessageER_OrderCancel.TAG_LeavesQty == field.Tag)
            {
                bool _isSuccess = false;
                c_message.LeavesQty = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            //
            else if (MessageER_OrderCancel.TAG_Account == field.Tag)
                c_message.Account = field.Value;
            else if (MessageER_ExecOrder.TAG_Symbol == field.Tag)
                c_message.Symbol = field.Value;
            else if (MessageER_ExecOrder.TAG_Side == field.Tag)
                c_message.Side = field.Value;
            else if (MessageER_OrderCancel.TAG_OrdType == field.Tag)
            {
                if (field.Value.Length > 0)
                    c_message.OrdType = field.Value;
            }
            else if (MessageER_OrderCancel.TAG_Price == field.Tag)
            {
                bool _isSuccess = false;
                c_message.Price = Utils.Convert.ParseLong(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}
