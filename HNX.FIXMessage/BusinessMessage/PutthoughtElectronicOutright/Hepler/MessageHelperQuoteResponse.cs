using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperQuoteResponse : MessageHelper
    {
        private MessageQuoteResponse c_message;

        #region #Tag const

        private const int TAG_Account = 1; //tai khoan khach hang
        private const int TAG_CoAccount = 2; 
        private const int TAG_TransactTime = 60; //thoi gian dat lenh
        private const int TAG_Symbol = 55; //ma chung khoan
        private const int TAG_Side = 54;   //1 = Buy, 2 = Sell
        private const int TAG_OrdType = 40;                // loai lenh
        private const int TAG_OrderQty = 38; //khoi luong dat lenh

        //const int TAG_PartyID = 448; //TV ban quote
        private const int TAG_Yield = 236; //Yiel

        private const int TAG_QuoteRespType = 694; //Loai quoteresponse

        private const int TAG_Price2 = 640; //gia thực hiện giá bẩn
        private const int TAG_Price = 44; //gia yết
        private const int TAG_SettlValue = 6464; //6464 giá trị thanh toán
        private const int TAG_SettDate = 64; // 64 ngày thanh toán.

        private const int TAG_SettlMethod = 6363;  //phương thức thanht toán
        private const int TAG_QuoteRespID = 693; //SHL quote muon response
        private const int TAG_ClOrdID = 11;

        #endregion #Tag const

        public MessageHelperQuoteResponse(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageQuoteResponse)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);
            sb.Append(TAG_ClOrdID).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);

            sb.Append(TAG_QuoteRespID.ToString()).Append(Common.DELIMIT).Append(c_message.QuoteRespID).Append(Common.SOH);
            sb.Append(TAG_QuoteRespType.ToString()).Append(Common.DELIMIT).Append(c_message.QuoteRespType).Append(Common.SOH);
            sb.Append(TAG_Account.ToString()).Append(Common.DELIMIT).Append(c_message.Account).Append(Common.SOH);
            sb.Append(TAG_CoAccount.ToString()).Append(Common.DELIMIT).Append(c_message.CoAccount).Append(Common.SOH);
            sb.Append(TAG_OrdType.ToString()).Append(Common.DELIMIT).Append(c_message.OrdType).Append(Common.SOH);
            sb.Append(TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(c_message.Symbol).Append(Common.SOH);
            sb.Append(TAG_Side.ToString()).Append(Common.DELIMIT).Append(c_message.Side).Append(Common.SOH);
            sb.Append(TAG_OrderQty.ToString()).Append(Common.DELIMIT).Append(c_message.OrderQty).Append(Common.SOH);
            if (c_message.Price > 0)
                sb.Append(TAG_Price.ToString()).Append(Common.DELIMIT).Append(c_message.Price).Append(Common.SOH);
            sb.Append(TAG_Price2.ToString()).Append(Common.DELIMIT).Append(c_message.Price2).Append(Common.SOH);
            sb.Append(TAG_SettlValue.ToString()).Append(Common.DELIMIT).Append(c_message.SettlValue).Append(Common.SOH);
            sb.Append(TAG_SettDate.ToString()).Append(Common.DELIMIT).Append(c_message.SettDate).Append(Common.SOH);
            sb.Append(TAG_SettlMethod.ToString()).Append(Common.DELIMIT).Append(c_message.SettlMethod).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (TAG_Account == field.Tag)
                c_message.Account = field.Value;
            else if (TAG_CoAccount == field.Tag)
                c_message.CoAccount = field.Value;
            else if (TAG_QuoteRespID == field.Tag)
                c_message.QuoteRespID = field.Value;
            else if (TAG_ClOrdID == field.Tag)
                c_message.ClOrdID = field.Value;
            /*else if (TAG_Yield == field.Tag)
            {
                bool _isSuccess = false;
                c_message.Yield = Utils.Convert.ParseDouble(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }*/
            //else if (TAG_PartyID == field.Tag)
            //    c_message.PartyID = field.Value;
            else if (TAG_Price2 == field.Tag)
            {
                bool _isSuccess = false;
                c_message.Price2 = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_SettDate == field.Tag)
                c_message.SettDate = field.Value;
            else if (TAG_Symbol == field.Tag)
                c_message.Symbol = field.Value;
            else if (TAG_SettlValue == field.Tag)
            {
                bool _isSuccess = false;
                c_message.SettlValue = Utils.Convert.ParseDouble(field.Value, ref _isSuccess);
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
            else if (TAG_QuoteRespType == field.Tag)
            {
                bool _isSuccess = false;
                c_message.QuoteRespType = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_Price == field.Tag)
            {
                bool _isSuccess = false;
                c_message.Price = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
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
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}