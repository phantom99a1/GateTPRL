using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperExectutionReport : MessageHelper
    {
        private MessageExecutionReport c_message;

        #region #Tag const
        public const int TAG_ClOrdID = 11; //so hieu lenh
        public const int TAG_Account = 1; //tai khoan khach hang
        public const int TAG_TransactTime = 60; //thoi gian dat lenh
        public const int TAG_Symbol = 55; //ma chung khoan
        public const int TAG_Side = 54;   //1 = Buy, 2 = Sell
        public const int TAG_OrderQty = 38; //khoi luong dat lenh 
        public const int TAG_OrdType = 40; //loai lenh: 1=Market price,2=ato,3=atc
        public const int TAG_Price = 44; //gia ban 
        //
        public const int TAG_OrderID = 37;
        public const int TAG_OrgOrderID = 198;
        public const int TAG_RefMsgType = 372;
        public const int TAG_PartyID = 448; //TV đặt lệnh
        public const int TAG_RejectReason = 103; // ly do reject
        public const int TAG_ReplaceQtty = 3338;//Khối lượng sửa thực tế với lệnh sửa
        public const int TAG_LotType = 1111; //Lô giao dịch

        public const int TAG_Special_Type = 440;           //Tam để lại có thể dùng cho lệnh Market Maker
                                                           //

        public const int TAG_Yield = 236; //Yiel
        public const int TAG_Price2 = 640; //gia thực hiện giá bẩn 
        public const int TAG_SettlValue = 6464; //6464 giá trị thanh toán
        public const int TAG_SettDate = 64; // 64 ngày thanh toán.

        #endregion

        public MessageHelperExectutionReport(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageExecutionReport)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);

            sb.Append(TAG_ClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);
            sb.Append(TAG_Account.ToString()).Append(Common.DELIMIT).Append(c_message.Account).Append(Common.SOH);
            //sb.Append(TAG_TransactTime.ToString()).Append(Common.DELIMIT).Append(Utils.Convert.ToFIXUTCTimestamp(c_message.TransactTime)).Append(Common.SOH);
            sb.Append(TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(c_message.Symbol).Append(Common.SOH);
            sb.Append(TAG_Side.ToString()).Append(Common.DELIMIT).Append(c_message.Side).Append(Common.SOH);
            sb.Append(TAG_OrderQty.ToString()).Append(Common.DELIMIT).Append(c_message.OrderQty).Append(Common.SOH);
            sb.Append(TAG_OrdType.ToString()).Append(Common.DELIMIT).Append(c_message.OrdType).Append(Common.SOH);
            if (c_message.OrderID != null && c_message.OrderID != "")
                sb.Append(TAG_OrderID.ToString()).Append(Common.DELIMIT).Append(c_message.OrderID).Append(Common.SOH);
            if (c_message.OrgOrderID != null && c_message.OrgOrderID != "")
                sb.Append(TAG_OrgOrderID.ToString()).Append(Common.DELIMIT).Append(c_message.OrgOrderID).Append(Common.SOH);
            

            sb.Append(TAG_RejectReason.ToString()).Append(Common.DELIMIT).Append(c_message.RejectReason).Append(Common.SOH);            
            sb.Append(TAG_PartyID.ToString()).Append(Common.DELIMIT).Append(c_message.PartyID).Append(Common.SOH);
            
            sb.Append(TAG_Special_Type.ToString()).Append(Common.DELIMIT).Append(c_message.Special_Type).Append(Common.SOH);
            if (c_message.Price > 0)
                sb.Append(TAG_Price.ToString()).Append(Common.DELIMIT).Append(c_message.Price).Append(Common.SOH);
            //sb.Append(TAG_Yield.ToString()).Append(Common.DELIMIT).Append(c_message.Yield).Append(Common.SOH);
            sb.Append(TAG_Price2.ToString()).Append(Common.DELIMIT).Append(c_message.Price2).Append(Common.SOH);
            if (c_message.SettlValue > 0)
                sb.Append(TAG_SettlValue.ToString()).Append(Common.DELIMIT).Append(c_message.SettlValue).Append(Common.SOH);
            sb.Append(TAG_SettDate.ToString()).Append(Common.DELIMIT).Append(Utils.Convert.ToShortDate(c_message.SettDate)).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (TAG_ClOrdID == field.Tag)
                c_message.ClOrdID = field.Value;
            else if (TAG_Account == field.Tag)
                c_message.Account = field.Value;
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
            else if (TAG_OrdType == field.Tag)
            {
                c_message.OrdType = field.Value;
            }
            else if (TAG_Price == field.Tag)
            {
                bool _isSuccess = false;
                c_message.Price = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            //
            if (TAG_OrderID == field.Tag)
                c_message.OrderID = field.Value;
            if (TAG_OrgOrderID == field.Tag)
                c_message.OrgOrderID = field.Value;
            if (TAG_PartyID == field.Tag)
                c_message.PartyID = field.Value;
            else if (TAG_RejectReason == field.Tag)
            {
                bool _isSuccess = false;
                c_message.RejectReason = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }          
            else if (TAG_Special_Type == field.Tag)
            {
                bool _isSuccess = false;
                c_message.Special_Type = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
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
                c_message.SettDate = Utils.Convert.FromShortDate(field.Value);
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}
