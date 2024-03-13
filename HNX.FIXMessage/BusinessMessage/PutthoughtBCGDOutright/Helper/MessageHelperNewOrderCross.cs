using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperNewOrderCross : MessageHelper
    {
        private MessageNewOrderCross c_message;

        #region #Tag const

        private const int TAG_Account = 1; //tai khoan khach hang
        private const int TAG_CoAccount = 2; //tai khoan khach hang
        //private const int TAG_TransactTime = 60; //thoi gian dat lenh
        private const int TAG_Symbol = 55; //ma chung khoan
        private const int TAG_Side = 54;   //1 = Buy, 2 = Sell
        private const int TAG_OrderQty = 38; //khoi luong dat lenh
        private const int TAG_PartyID = 448; //TV ban
        private const int TAG_CoPartyID = 449; //TV mua
        private const int TAG_CrossType = 549; //gia ban
        private const int TAG_CrossID = 548; //gia ban
        private const int TAG_Price = 44; //gia ban

        private const int TAG_OrdType = 40;                // loai lenh
        private const int TAG_ClOrdID = 11;

        private const int TAG_Price2 = 640; //gia thực hiện giá bẩn
        private const int TAG_SettlValue = 6464; //6464 giá trị thanh toán
        private const int TAG_SettDate = 64; // 64 ngày thanh toán.
        private const int TAG_EffectiveTime = 168; //ngay bat dau giao dich
        private const int TAG_ReConfirm = 1689; //xác nhận lại GD đối với lệnh tương lai
        private const int TAG_SettlMethod = 6363;  //phương thức thanht toán

        #endregion #Tag const

        public MessageHelperNewOrderCross(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageNewOrderCross)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);

            sb.Append(TAG_ClOrdID).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);
            sb.Append(TAG_Account.ToString()).Append(Common.DELIMIT).Append(c_message.Account).Append(Common.SOH);
            sb.Append(TAG_CoAccount.ToString()).Append(Common.DELIMIT).Append(c_message.CoAccount).Append(Common.SOH);
            //sb.Append(TAG_TransactTime.ToString()).Append(Common.DELIMIT).Append(Utils.Convert.ToFIXUTCTimestamp(c_message.TransactTime)).Append(Common.SOH);
            sb.Append(TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(c_message.Symbol).Append(Common.SOH);
            sb.Append(TAG_Side.ToString()).Append(Common.DELIMIT).Append(c_message.Side).Append(Common.SOH);
            sb.Append(TAG_OrderQty.ToString()).Append(Common.DELIMIT).Append(c_message.OrderQty).Append(Common.SOH);
            sb.Append(TAG_PartyID.ToString()).Append(Common.DELIMIT).Append(c_message.PartyID).Append(Common.SOH);
            sb.Append(TAG_CoPartyID.ToString()).Append(Common.DELIMIT).Append(c_message.CoPartyID).Append(Common.SOH);
            sb.Append(TAG_CrossID.ToString()).Append(Common.DELIMIT).Append(c_message.CrossID).Append(Common.SOH);
            sb.Append(TAG_CrossType.ToString()).Append(Common.DELIMIT).Append(c_message.CrossType).Append(Common.SOH);
            if (c_message.Price >0)
                sb.Append(TAG_Price.ToString()).Append(Common.DELIMIT).Append(c_message.Price).Append(Common.SOH);
            sb.Append(TAG_OrdType.ToString()).Append(Common.DELIMIT).Append(c_message.OrdType).Append(Common.SOH);
            sb.Append(TAG_Price2.ToString()).Append(Common.DELIMIT).Append(c_message.Price2).Append(Common.SOH);
            sb.Append(TAG_SettlValue.ToString()).Append(Common.DELIMIT).Append(c_message.SettlValue).Append(Common.SOH);
            sb.Append(TAG_SettDate.ToString()).Append(Common.DELIMIT).Append(c_message.SettDate).Append(Common.SOH);
            sb.Append(TAG_EffectiveTime.ToString()).Append(Common.DELIMIT).Append(c_message.EffectiveTime).Append(Common.SOH);
            sb.Append(TAG_SettlMethod.ToString()).Append(Common.DELIMIT).Append(c_message.SettlMethod).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (TAG_CrossID == field.Tag)
                c_message.CrossID = field.Value;
            else if (TAG_ClOrdID == field.Tag)
                c_message.ClOrdID = field.Value;
            else if (TAG_Account == field.Tag)
                c_message.Account = field.Value;
            else if (TAG_CoAccount == field.Tag)
                c_message.CoAccount = field.Value;
            else if (TAG_PartyID == field.Tag)
                c_message.PartyID = field.Value;
            else if (TAG_CoPartyID == field.Tag)
                c_message.CoPartyID = field.Value;
            /*else if (TAG_TransactTime == field.Tag)
                c_message.TransactTime = Utils.Convert.FromFIXUTCTimestamp(field.Value);*/
            else if (TAG_Symbol == field.Tag)
                c_message.Symbol = field.Value;
            else if (TAG_Side == field.Tag)
            {
                bool _isSuccess = false;
                c_message.Side = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_OrderQty == field.Tag)
            {
                bool _isSuccess = false;
                c_message.OrderQty = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_CrossType == field.Tag)
            {
                bool _isSuccess = false;
                c_message.CrossType = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
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
            else if (TAG_SettlValue == field.Tag)
            {
                bool _isSuccess = false;
                c_message.SettlValue = Utils.Convert.ParseDouble(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_SettDate == field.Tag)
                c_message.SettDate = field.Value;
            //EffectiveTime
            else if (TAG_EffectiveTime == field.Tag)
                c_message.EffectiveTime = field.Value;
            else if (TAG_SettlMethod == field.Tag)
            {
                bool _isSuccess = false;
                c_message.SettlMethod = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_OrdType == field.Tag)
            {
                if (field.Value.Length > 0)
                    c_message.OrdType = field.Value;
            }
            else if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}