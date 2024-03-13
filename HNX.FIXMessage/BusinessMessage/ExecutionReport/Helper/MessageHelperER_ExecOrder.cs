using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperER_ExecOrder : MessageHelperExectutionReport
    {
        private MessageER_ExecOrder c_message;

        public MessageHelperER_ExecOrder(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageER_ExecOrder)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);

            sb.Append(MessageER_ExecOrder.TAG_OrdStatus.ToString()).Append(Common.DELIMIT).Append(c_message.OrdStatus).Append(Common.SOH);
            if (c_message.ClOrdID != "")
                sb.Append(MessageER_ExecOrder.TAG_ClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);
            if (c_message.OrigClOrdID != null && c_message.OrigClOrdID != "")
            {
                sb.Append(MessageER_ExecOrder.TAG_OrigClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.OrigClOrdID).Append(Common.SOH);
            }
            if (c_message.SecondaryClOrdID != null && c_message.SecondaryClOrdID != "")
            {
                sb.Append(MessageER_ExecOrder.TAG_SecondaryClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.SecondaryClOrdID).Append(Common.SOH);
            }
            sb.Append(MessageER_ExecOrder.TAG_OrderID.ToString()).Append(Common.DELIMIT).Append(c_message.OrderID).Append(Common.SOH);
            //sb.Append(MessageER_ExecOrder.TAG_TransactTime.ToString()).Append(Common.DELIMIT).Append(Utils.Convert.ToFIXUTCTimestamp(c_message.TransactTime)).Append(Common.SOH);

            sb.Append(MessageER_ExecOrder.TAG_LastQty.ToString()).Append(Common.DELIMIT).Append(c_message.LastQty).Append(Common.SOH);
            sb.Append(MessageER_ExecOrder.TAG_LastPx.ToString()).Append(Common.DELIMIT).Append(c_message.LastPx).Append(Common.SOH);
            sb.Append(MessageER_ExecOrder.TAG_ExecID.ToString()).Append(Common.DELIMIT).Append(c_message.ExecID).Append(Common.SOH);
            if (c_message.Symbol != null && c_message.Symbol != "")
            {
                sb.Append(MessageER_ExecOrder.TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(c_message.Symbol).Append(Common.SOH);
            }
            if (c_message.Side != 0)
                sb.Append(MessageER_ExecOrder.TAG_Side.ToString()).Append(Common.DELIMIT).Append(c_message.Side).Append(Common.SOH);

            if (c_message.ReciprocalMember != null && c_message.ReciprocalMember != "")
            {
                sb.Append(MessageER_ExecOrder.TAG_ReciprocalMember.ToString()).Append(Common.DELIMIT).Append(c_message.ReciprocalMember).Append(Common.SOH);
            }

            if (c_message.SettlValue > 0)
                sb.Append(MessageER_Order.TAG_SettlValue.ToString()).Append(Common.DELIMIT).Append(c_message.SettlValue.ToString()).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (MessageER_ExecOrder.TAG_OrdStatus == field.Tag)
                c_message.OrdStatus = field.Value;
            else if (MessageER_ExecOrder.TAG_ClOrdID == field.Tag)
                c_message.ClOrdID = field.Value;
            else if (MessageER_ExecOrder.TAG_OrigClOrdID == field.Tag)
                c_message.OrigClOrdID = field.Value;
            else if (MessageER_ExecOrder.TAG_SecondaryClOrdID == field.Tag)
                c_message.SecondaryClOrdID = field.Value;
            else if (MessageER_ExecOrder.TAG_OrderID == field.Tag)
                c_message.OrderID = field.Value;
            //else if (MessageER_ExecOrder.TAG_TransactTime == field.Tag)
            //    c_message.TransactTime = Utils.Convert.FromFIXUTCTimestamp(field.Value);
            else if (MessageER_ExecOrder.TAG_LastQty == field.Tag)
            {
                bool _isSuccess = false;
                c_message.LastQty = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageER_ExecOrder.TAG_LastPx == field.Tag)
            {
                bool _isSuccess = false;
                c_message.LastPx = Utils.Convert.ParseLong(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageER_ExecOrder.TAG_ExecID == field.Tag)
                c_message.ExecID = field.Value;
            //
            else if (MessageER_ExecOrder.TAG_Symbol == field.Tag)
                c_message.Symbol = field.Value;
            else if (MessageER_ExecOrder.TAG_Side == field.Tag)
            {
                bool _isSuccess = false;
                c_message.Side = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageER_ExecOrder.TAG_ReciprocalMember == field.Tag)
                c_message.ReciprocalMember = field.Value;
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