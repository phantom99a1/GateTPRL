using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperCrossOrderCancelRequest : MessageHelper
    {
        private CrossOrderCancelRequest c_message;

        #region #Tag const 
        const int TAG_TransactTime = 60; //thoi gian dat lenh 
        const int TAG_CrossType = 549; //gia ban
        const int TAG_OrgCrossID = 551; //gia ban 
        const int TAG_Symbol = 55; // 55: Mã ck
        const int TAG_Side = 54;
        const int TAG_OrdType = 40;                // loai lenh
        const int TAG_ClOrdID = 11;
        const int TAG_OrderID = 37; //shl cần sửa

        #endregion

        public MessageHelperCrossOrderCancelRequest(FIXMessageBase message)
            : base(message)
        {
            c_message = (CrossOrderCancelRequest)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);
            //sb.Append(TAG_TransactTime.ToString()).Append(Common.DELIMIT).Append(Utils.Convert.ToFIXUTCTimestamp(c_message.TransactTime)).Append(Common.SOH);
            
            sb.Append(TAG_ClOrdID).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);
            sb.Append(TAG_OrderID.ToString()).Append(Common.DELIMIT).Append(c_message.OrderID).Append(Common.SOH);
            sb.Append(TAG_OrgCrossID.ToString()).Append(Common.DELIMIT).Append(c_message.OrgCrossID).Append(Common.SOH);
            sb.Append(TAG_CrossType.ToString()).Append(Common.DELIMIT).Append(c_message.CrossType).Append(Common.SOH);
            if (c_message.Symbol != "")
                sb.Append(TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(c_message.Symbol).Append(Common.SOH);

            sb.Append(TAG_Side.ToString()).Append(Common.DELIMIT).Append(c_message.Side).Append(Common.SOH);
            sb.Append(TAG_OrdType.ToString()).Append(Common.DELIMIT).Append(c_message.OrdType).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (TAG_OrgCrossID == field.Tag)
                c_message.OrgCrossID = field.Value;
            else if (TAG_OrderID == field.Tag)
                c_message.OrderID = field.Value;
            else if (TAG_ClOrdID == field.Tag)
                c_message.ClOrdID = field.Value;
            /*else if (TAG_TransactTime == field.Tag)
                c_message.TransactTime = Utils.Convert.FromFIXUTCTimestamp(field.Value);*/
            else if (TAG_CrossType == field.Tag)
            {
                bool _isSuccess = false;
                c_message.CrossType = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
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
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}
