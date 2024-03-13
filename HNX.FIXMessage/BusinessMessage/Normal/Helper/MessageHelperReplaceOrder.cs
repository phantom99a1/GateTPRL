using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperReplaceOrder : MessageHelper
    {
        private MessageReplaceOrder c_message;

        #region #Tag const

        private const int TAG_ClOrdID = 11;
        private const int TAG_OrigClOrdID = 41;
        private const int TAG_Account = 1;
        private const int TAG_Symbol = 55;
        private const int TAG_OrderQty = 38; //khoi luong dat lenh
        private const int TAG_OrgOrderQty = 2238; //khoi luong dat lenh
        private const int TAG_Price2 = 640; //gia thực hiện giá bẩn

        #endregion #Tag const

        public MessageHelperReplaceOrder(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageReplaceOrder)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);

            sb.Append(TAG_ClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);
            sb.Append(TAG_OrigClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.OrigClOrdID).Append(Common.SOH);
            sb.Append(TAG_Account.ToString()).Append(Common.DELIMIT).Append(c_message.Account).Append(Common.SOH);
            sb.Append(TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(c_message.Symbol).Append(Common.SOH);
            sb.Append(TAG_OrderQty.ToString()).Append(Common.DELIMIT).Append(c_message.OrderQty).Append(Common.SOH);
            sb.Append(TAG_OrgOrderQty.ToString()).Append(Common.DELIMIT).Append(c_message.OrgOrderQty).Append(Common.SOH);
            sb.Append(TAG_Price2.ToString()).Append(Common.DELIMIT).Append(c_message.Price2).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (TAG_ClOrdID == field.Tag)
                c_message.ClOrdID = field.Value;
            else if (TAG_OrigClOrdID == field.Tag)
                c_message.OrigClOrdID = field.Value;
            else if (TAG_Account == field.Tag)
                c_message.Account = field.Value;
            else if (TAG_Symbol == field.Tag)
                c_message.Symbol = field.Value;
            else if (TAG_OrderQty == field.Tag)
            {
                bool _isSuccess = false;
                c_message.OrderQty = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_OrgOrderQty == field.Tag)
            {
                bool _isSuccess = false;
                c_message.OrgOrderQty = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_Price2 == field.Tag)
            {
                bool _isSuccess = false;
                c_message.Price2 = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}