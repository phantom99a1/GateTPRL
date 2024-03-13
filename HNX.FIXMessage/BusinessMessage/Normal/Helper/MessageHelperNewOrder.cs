using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperNewOrder : MessageHelper
    {
        private MessageNewOrder c_message;

        #region #Tag const

        private const int TAG_ClOrdID = 11;              // so hieu lenh
        private const int TAG_Account = 1;                 // tai khoan khach hang
        private const int TAG_Symbol = 55;                 // ma chung khoan
        private const int TAG_Side = 54;                   // 1 = Buy, 2 = Sell
        private const int TAG_OrdType = 40;                // loai lenh: 1=Market price,2=ato,3=atc
        private const int TAG_OrderQty = 38;                // khoi luong dat lenh
        private const int TAG_OrderQty2 = 192;                  // KL ứng với vế mua của MM 2 chiều
        private const int TAG_Price = 44;                       // ứng với vế mua của MM 2 chiều
        private const int TAG_Price2 = 640; //gia thực hiện giá bẩn
        private const int TAG_SpecialType = 440;        ////Loại yết giá đặc biệt dùng cho lệnh Marketmaker: = 1 là yết giá 1 chiều, = 2 là 2 chiều, = 3 là 2 chiều thay thế

        #endregion #Tag const

        public MessageHelperNewOrder(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageNewOrder)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);
            sb.Append(TAG_ClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);
            sb.Append(TAG_Account.ToString()).Append(Common.DELIMIT).Append(c_message.Account).Append(Common.SOH);
            sb.Append(TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(c_message.Symbol).Append(Common.SOH);
            sb.Append(TAG_Side.ToString()).Append(Common.DELIMIT).Append(c_message.Side).Append(Common.SOH);
            sb.Append(TAG_OrdType.ToString()).Append(Common.DELIMIT).Append(c_message.OrdType).Append(Common.SOH);
            sb.Append(TAG_OrderQty.ToString()).Append(Common.DELIMIT).Append(c_message.OrderQty).Append(Common.SOH);
            sb.Append(TAG_OrderQty2.ToString()).Append(Common.DELIMIT).Append(c_message.OrderQty2).Append(Common.SOH);
            sb.Append(TAG_Price2.ToString()).Append(Common.DELIMIT).Append(c_message.Price2).Append(Common.SOH);
            sb.Append(TAG_Price.ToString()).Append(Common.DELIMIT).Append(c_message.Price).Append(Common.SOH);
            sb.Append(TAG_SpecialType.ToString()).Append(Common.DELIMIT).Append(c_message.SpecialType).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (TAG_ClOrdID == field.Tag)
                c_message.ClOrdID = field.Value;
            else if (TAG_Account == field.Tag)
                c_message.Account = field.Value;
            else if (TAG_Symbol == field.Tag)
                c_message.Symbol = field.Value;
            else if (TAG_Side == field.Tag)
            {
                bool _isSuccess = false;
                c_message.Side = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_OrdType == field.Tag)
            {
                if (field.Value.Length > 0)
                    c_message.OrdType = field.Value;
            }
            else if (TAG_OrderQty == field.Tag)
            {
                bool _isSuccess = false;
                c_message.OrderQty = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_OrderQty2 == field.Tag)
            {
                bool _isSuccess = false;
                c_message.OrderQty2 = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_Price == field.Tag)
            {
                bool _isSuccess = false;
                c_message.Price = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_Price2 == field.Tag)
            {
                bool _isSuccess = false;
                c_message.Price2 = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_SpecialType == field.Tag)
            {
                bool _isSuccess = false;
                c_message.SpecialType = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}