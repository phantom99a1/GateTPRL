using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperER_Order : MessageHelper
    {
        private MessageER_Order c_message;

        public MessageHelperER_Order(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageER_Order)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            try
            {
                base.BuildBody(sb);

                sb.Append(MessageER_Order.TAG_OrdStatus.ToString()).Append(Common.DELIMIT).Append(c_message.OrdStatus).Append(Common.SOH);
                if (c_message.ClOrdID != "")
                    sb.Append(MessageER_Order.TAG_ClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);
                sb.Append(MessageER_Order.TAG_OrderID.ToString()).Append(Common.DELIMIT).Append(c_message.OrderID).Append(Common.SOH);
                //sb.Append(MessageER_Order.TAG_TransactTime.ToString()).Append(Common.DELIMIT).Append(Utils.Convert.ToFIXUTCTimestamp(c_message.TransactTime)).Append(Common.SOH);
                //
                sb.Append(MessageER_Order.TAG_Account.ToString()).Append(Common.DELIMIT).Append(c_message.Account).Append(Common.SOH);
                sb.Append(MessageER_Order.TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(c_message.Symbol).Append(Common.SOH);
                sb.Append(MessageER_Order.TAG_Side.ToString()).Append(Common.DELIMIT).Append(c_message.Side).Append(Common.SOH);
                sb.Append(MessageER_Order.TAG_OrderQty.ToString()).Append(Common.DELIMIT).Append(c_message.OrderQty.ToString()).Append(Common.SOH);
                if (c_message.OrdType != null)
                    sb.Append(MessageER_Order.TAG_OrdType.ToString()).Append(Common.DELIMIT).Append(c_message.OrdType.ToString()).Append(Common.SOH);
                sb.Append(MessageER_Order.TAG_Price.ToString()).Append(Common.DELIMIT).Append(c_message.Price.ToString()).Append(Common.SOH);

                if (c_message.SettlValue > 0)
                    sb.Append(MessageER_Order.TAG_SettlValue.ToString()).Append(Common.DELIMIT).Append(c_message.SettlValue.ToString()).Append(Common.SOH);

                //if (c_message.OrderQty2 != null && c_message.OrderQty2 != 0)
                //    sb.Append(MessageER_Order.TAG_OrderQty2.ToString()).Append(Common.DELIMIT).Append(c_message.OrderQty2.ToString()).Append(Common.SOH);
                //if (c_message.StopPx != null && c_message.StopPx != 0)
                //    sb.Append(MessageER_Order.TAG_StopPx.ToString()).Append(Common.DELIMIT).Append(c_message.StopPx.ToString()).Append(Common.SOH);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public override bool ParseField(Field field)
        {
            if (MessageER_Order.TAG_OrdStatus == field.Tag)
                c_message.OrdStatus = field.Value;
            else if (MessageER_Order.TAG_ClOrdID == field.Tag)
                c_message.ClOrdID = field.Value;
            else if (MessageER_Order.TAG_OrderID == field.Tag)
                c_message.OrderID = field.Value;
            //else if (MessageER_Order.TAG_TransactTime == field.Tag)
            //    c_message.TransactTime = Utils.Convert.FromFIXUTCTimestamp(field.Value);
            //
            else if (MessageER_Order.TAG_Account == field.Tag)
                c_message.Account = field.Value;
            else if (MessageER_ExecOrder.TAG_Symbol == field.Tag)
                c_message.Symbol = field.Value;
            else if (MessageER_ExecOrder.TAG_Side == field.Tag)
                c_message.Side = field.Value;
            else if (MessageER_Order.TAG_OrderQty == field.Tag)
            {
                bool _isSuccess = false;
                c_message.OrderQty = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;

            }
            else if (MessageER_Order.TAG_OrdType == field.Tag)
            {
                c_message.OrdType = field.Value;
            }
            else if (MessageER_Order.TAG_Price == field.Tag)
            {
                bool _isSuccess = false;
                c_message.Price = Utils.Convert.ParseLong(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageER_Order.TAG_SettlValue == field.Tag)
            {
                bool _isSuccess = false;
                c_message.SettlValue = Utils.Convert.ParseDouble(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            //else if (MessageER_Order.TAG_OrderQty2 == field.Tag)
            //{
            //    bool _isSuccess = false;
            //    c_message.OrderQty2 = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
            //    if (_isSuccess == false) return false;
            //}
            //else if (MessageER_Order.TAG_StopPx == field.Tag)
            //{
            //    bool _isSuccess = false;
            //    c_message.StopPx = Utils.Convert.ParseDouble(field.Value, ref _isSuccess);
            //    if (_isSuccess == false) return false;
            //}
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}
