using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperER_OrderReplace : MessageHelper
    {
        private MessageER_OrderReplace c_message;
        public MessageHelperER_OrderReplace(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageER_OrderReplace)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);

            sb.Append(MessageER_OrderReplace.TAG_OrdStatus.ToString()).Append(Common.DELIMIT).Append(c_message.OrdStatus).Append(Common.SOH);
            if (c_message.ClOrdID != "")
                sb.Append(MessageER_OrderReplace.TAG_ClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);
            if (c_message.OrigClOrdID != "")
                sb.Append(MessageER_OrderReplace.TAG_OrigClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.OrigClOrdID).Append(Common.SOH);
            sb.Append(MessageER_OrderReplace.TAG_OrderID.ToString()).Append(Common.DELIMIT).Append(c_message.OrderID).Append(Common.SOH);
            //sb.Append(MessageER_OrderReplace.TAG_TransactTime.ToString()).Append(Common.DELIMIT).Append(Utils.Convert.ToFIXUTCTimestamp(c_message.TransactTime)).Append(Common.SOH);
            //
            sb.Append(MessageER_OrderReplace.TAG_Account.ToString()).Append(Common.DELIMIT).Append(c_message.Account).Append(Common.SOH);
            sb.Append(MessageER_OrderReplace.TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(c_message.Symbol).Append(Common.SOH);
            sb.Append(MessageER_OrderReplace.TAG_Side.ToString()).Append(Common.DELIMIT).Append(c_message.Side).Append(Common.SOH);
            sb.Append(MessageER_OrderReplace.TAG_OrdType.ToString()).Append(Common.DELIMIT).Append(c_message.OrdType).Append(Common.SOH);
            //
            sb.Append(MessageER_OrderReplace.TAG_LastQty.ToString()).Append(Common.DELIMIT).Append(c_message.LastQty).Append(Common.SOH);
            sb.Append(MessageER_OrderReplace.TAG_LastPx.ToString()).Append(Common.DELIMIT).Append(c_message.LastPx).Append(Common.SOH);            
            ////vutt sửa ngày 27/12/2012 nếu 2 cái này = 0 hoặc = null thì ko buil vào msg
            //if (c_message.StopPx != null && c_message.StopPx != 0)
            //    sb.Append(MessageER_OrderReplace.TAG_StopPx.ToString()).Append(Common.DELIMIT).Append(c_message.StopPx).Append(Common.SOH);
            //if (c_message.OrderQty2 != null && c_message.OrderQty2 != 0.0)
            //    sb.Append(MessageER_OrderReplace.TAG_OrderQty2.ToString()).Append(Common.DELIMIT).Append(c_message.OrderQty2).Append(Common.SOH);
            if (c_message.Side != "8") //nếu không phải lệnh thỏa thuận thì mới buil
                sb.Append(MessageER_OrderReplace.TAG_LeavesQty.ToString()).Append(Common.DELIMIT).Append(c_message.LeavesQty).Append(Common.SOH);

            if (c_message.SettlValue > 0)
                sb.Append(MessageER_Order.TAG_SettlValue.ToString()).Append(Common.DELIMIT).Append(c_message.SettlValue.ToString()).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (MessageER_OrderReplace.TAG_OrdStatus == field.Tag)
                c_message.OrdStatus = field.Value;
            else if (MessageER_OrderReplace.TAG_ClOrdID == field.Tag)
                c_message.ClOrdID = field.Value;
            else if (MessageER_OrderReplace.TAG_OrigClOrdID == field.Tag)
                c_message.OrigClOrdID = field.Value;
            else if (MessageER_OrderReplace.TAG_OrderID == field.Tag)
                c_message.OrderID = field.Value;
            //else if (MessageER_OrderReplace.TAG_TransactTime == field.Tag)
            //    c_message.TransactTime = Utils.Convert.FromFIXUTCTimestamp(field.Value);
            ////
            else if (MessageER_OrderReplace.TAG_Account == field.Tag)
                c_message.Account = field.Value;
            else if (MessageER_OrderReplace.TAG_Symbol == field.Tag)
                c_message.Symbol = field.Value;
            else if (MessageER_OrderReplace.TAG_Side == field.Tag)
                c_message.Side = field.Value;
            else if (MessageER_OrderReplace.TAG_OrdType == field.Tag)
            {
                c_message.OrdType = field.Value;
            }
            //
            else if (MessageER_OrderReplace.TAG_LastQty == field.Tag)
            {
                bool _isSuccess = false;
                c_message.LastQty = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageER_OrderReplace.TAG_LastPx == field.Tag)
            {
                bool _isSuccess = false;
                c_message.LastPx = Utils.Convert.ParseLong(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
           
            //else if (MessageER_OrderReplace.TAG_OrderQty2 == field.Tag)
            //{
            //    bool _isSuccess = false;
            //    c_message.OrderQty2 = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
            //    if (_isSuccess == false) return false;
            //}
            //else if (MessageER_OrderReplace.TAG_StopPx == field.Tag)
            //{
            //    bool _isSuccess = false;
            //    c_message.StopPx = Utils.Convert.ParseDouble(field.Value, ref _isSuccess);
            //    if (_isSuccess == false) return false;
            //}
            else if (MessageER_OrderReplace.TAG_LeavesQty == field.Tag)
            {
                bool _isSuccess = false;
                c_message.LeavesQty = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }


            else if (MessageER_Order.TAG_SettlValue == field.Tag)
            {
                bool _isSuccess = false;
                c_message.SettlValue = Utils.Convert.ParseDouble(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}
