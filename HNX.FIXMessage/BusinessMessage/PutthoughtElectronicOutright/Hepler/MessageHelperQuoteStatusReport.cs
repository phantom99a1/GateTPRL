using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperQuoteStatusReport : MessageHelper
    {
        private MessageQuoteSatusReport c_message;

        #region #Tag const

        private const int TAG_Account = 1; //tai khoan khach hang
        private const int TAG_TransactTime = 60; //thoi gian dat lenh
        private const int TAG_Symbol = 55; //ma chung khoan
        private const int TAG_Side = 54;   //1 = Buy, 2 = Sell
        private const int TAG_OrderQty = 38; //khoi luong dat lenh
        private const int TAG_PartyID = 448; //TV ban quote
        private const int TAG_OrdType = 40;                // loai lenh
        private const int TAG_QuoteReqID = 131;
        private const int TAG_QuoteID = 171; // SHL HNX của lệnh mới sinh ra

        private const int TAG_QuoteType = 537; //Loai quote

        private const int TAG_Yield = 236; //Yiel
        private const int TAG_Price2 = 640; //gia thực hiện giá bẩn
        private const int TAG_Price = 44; //gia yết
        private const int TAG_SettlValue = 6464; //6464 giá trị thanh toán
        private const int TAG_SettDate = 64; // 64 ngày thanh toán.
        private const int TAG_SettMethod = 6363; // 64 ngày thanh toán.

        private const int TAG_RegistID = 513; //513 danh sach đại diện GD được quote,  = 0 là quote public

        private const int TAG_ClOrdID = 11;
        private const int TAG_OrderPartyID = 4488;

        #endregion #Tag const

        public MessageHelperQuoteStatusReport(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageQuoteSatusReport)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);
            sb.Append(TAG_ClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);
            sb.Append(TAG_OrderPartyID.ToString()).Append(Common.DELIMIT).Append(c_message.OrderPartyID).Append(Common.SOH);

            sb.Append(TAG_QuoteType.ToString()).Append(Common.DELIMIT).Append(c_message.QuoteType).Append(Common.SOH);
            sb.Append(TAG_Account.ToString()).Append(Common.DELIMIT).Append(c_message.Account).Append(Common.SOH);
            //sb.Append(TAG_TransactTime.ToString()).Append(Common.DELIMIT).Append(Utils.Convert.ToFIXUTCTimestamp(c_message.TransactTime)).Append(Common.SOH);
            sb.Append(TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(c_message.Symbol).Append(Common.SOH);
            sb.Append(TAG_Side.ToString()).Append(Common.DELIMIT).Append(c_message.Side).Append(Common.SOH);
            sb.Append(TAG_OrdType.ToString()).Append(Common.DELIMIT).Append(c_message.OrdType).Append(Common.SOH);

            if (c_message.OrderQty > 0)
                sb.Append(TAG_OrderQty.ToString()).Append(Common.DELIMIT).Append(c_message.OrderQty).Append(Common.SOH);
            if (c_message.Price > 0)
                sb.Append(TAG_Price.ToString()).Append(Common.DELIMIT).Append(c_message.Price).Append(Common.SOH);
            //sb.Append(TAG_PartyID.ToString()).Append(Common.DELIMIT).Append(c_message.PartyID).Append(Common.SOH);
            sb.Append(TAG_QuoteReqID.ToString()).Append(Common.DELIMIT).Append(c_message.QuoteReqID).Append(Common.SOH);
            sb.Append(TAG_QuoteID.ToString()).Append(Common.DELIMIT).Append(c_message.QuoteID).Append(Common.SOH);            
            if (c_message.Price2 > 0)
                sb.Append(TAG_Price2.ToString()).Append(Common.DELIMIT).Append(c_message.Price2).Append(Common.SOH);
            sb.Append(TAG_SettlValue.ToString()).Append(Common.DELIMIT).Append(c_message.SettlValue).Append(Common.SOH);
            if (c_message.SettlMethod > 0)
                sb.Append(TAG_SettMethod.ToString()).Append(Common.DELIMIT).Append(c_message.SettlMethod).Append(Common.SOH);
            if (c_message.SettDate != string.Empty)
                sb.Append(TAG_SettDate.ToString()).Append(Common.DELIMIT).Append(c_message.SettDate).Append(Common.SOH);
            if (c_message.RegistID != "")
                sb.Append(TAG_RegistID.ToString()).Append(Common.DELIMIT).Append(c_message.RegistID).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (TAG_Account == field.Tag)
                c_message.Account = field.Value;           
            //else if (TAG_PartyID == field.Tag)
            //    c_message.PartyID = field.Value;
            else if (TAG_QuoteReqID == field.Tag)
                c_message.QuoteReqID = field.Value;
            else if (TAG_QuoteID == field.Tag)
                c_message.QuoteID = field.Value;
            else if (TAG_Price2 == field.Tag)
            {
                bool _isSuccess = false;
                c_message.Price2 = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            //else if (TAG_TransactTime == field.Tag)
            //    c_message.TransactTime = Utils.Convert.FromFIXUTCTimestamp(field.Value);
            else if (TAG_Symbol == field.Tag)
                c_message.Symbol = field.Value;
            else if (TAG_SettlValue == field.Tag)
            {
                bool _isSuccess = false;
                c_message.SettlValue = Utils.Convert.ParseDouble(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_SettMethod == field.Tag)
            {
                bool _isSuccess = false;
                c_message.SettlMethod = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
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
            else if (TAG_QuoteType == field.Tag)
            {
                bool _isSuccess = false;
                c_message.QuoteType = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_Price == field.Tag)
            {
                bool _isSuccess = false;
                c_message.Price = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_RegistID == field.Tag)
                c_message.RegistID = field.Value;
            else if (TAG_SettDate == field.Tag)
                c_message.SettDate = field.Value;
            else if (TAG_OrdType == field.Tag)
            {
                if (field.Value.Length > 0)
                    c_message.OrdType = field.Value;
            }
            else if (TAG_ClOrdID == field.Tag)
                c_message.ClOrdID = field.Value;
            else if (TAG_OrderPartyID == field.Tag)
                c_message.OrderPartyID = field.Value;
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}