using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperSecurityStatus : MessageHelper
    {
        private MessageSecurityStatus c_message;

        public MessageHelperSecurityStatus(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageSecurityStatus)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);
            sb.Append(MessageSecurityStatus.TAG_SecurityStatusReqID.ToString()).Append(Common.DELIMIT).Append(c_message.SecurityStatusReqID).Append(Common.SOH);
            sb.Append(MessageSecurityStatus.TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(c_message.Symbol).Append(Common.SOH);
            sb.Append(MessageSecurityStatus.TAG_SecurityType.ToString()).Append(Common.DELIMIT).Append(c_message.SecurityType).Append(Common.SOH);

            sb.Append(MessageSecurityStatus.TAG_MaturityDate.ToString()).Append(Common.DELIMIT).Append(Utils.Convert.ToShortDate(c_message.MaturityDate)).Append(Common.SOH);
            sb.Append(MessageSecurityStatus.TAG_IssueDate.ToString()).Append(Common.DELIMIT).Append(Utils.Convert.ToShortDate(c_message.IssueDate)).Append(Common.SOH);
            sb.Append(MessageSecurityStatus.TAG_TotalListingQtty.ToString()).Append(Common.DELIMIT).Append(c_message.TotalListingQtty.ToString()).Append(Common.SOH);
            sb.Append(MessageSecurityStatus.TAG_Issuer.ToString()).Append(Common.DELIMIT).Append(c_message.Issuer).Append(Common.SOH);
            //sb.Append(MessageSecurityStatus.TAG_SecurityDesc.ToString()).Append(Common.DELIMIT).Append(c_message.SecurityDesc).Append(Common.SOH);
            sb.Append(MessageSecurityStatus.TAG_LastPx.ToString()).Append(Common.DELIMIT).Append(c_message.LastPx).Append(Common.SOH);
            sb.Append(MessageSecurityStatus.TAG_HighPx.ToString()).Append(Common.DELIMIT).Append(c_message.HighPx).Append(Common.SOH);
            sb.Append(MessageSecurityStatus.TAG_LowPx.ToString()).Append(Common.DELIMIT).Append(c_message.LowPx).Append(Common.SOH);
            sb.Append(MessageSecurityStatus.TAG_HighPxOut.ToString()).Append(Common.DELIMIT).Append(c_message.HighPxOut).Append(Common.SOH);
            sb.Append(MessageSecurityStatus.TAG_LowPxOut.ToString()).Append(Common.DELIMIT).Append(c_message.LowPxOut).Append(Common.SOH);
            sb.Append(MessageSecurityStatus.TAG_HighPxRep.ToString()).Append(Common.DELIMIT).Append(c_message.HighPxRep).Append(Common.SOH);
            sb.Append(MessageSecurityStatus.TAG_LowPxRep.ToString()).Append(Common.DELIMIT).Append(c_message.LowPxRep).Append(Common.SOH);
            sb.Append(MessageSecurityStatus.TAG_Allowed_Trading_Subject.ToString()).Append(Common.DELIMIT).Append(c_message.Allowed_Trading_Subject).Append(Common.SOH);
            sb.Append(MessageSecurityStatus.TAG_Allowed_Trading_Subject_Sell.ToString()).Append(Common.DELIMIT).Append(c_message.Allowed_Trading_Subject_Sell).Append(Common.SOH);
            sb.Append(MessageSecurityStatus.TAG_SecurityTradingStatus.ToString()).Append(Common.DELIMIT).Append(c_message.SecurityTradingStatus).Append(Common.SOH);
            sb.Append(MessageSecurityStatus.TAG_BuyVolume.ToString()).Append(Common.DELIMIT).Append(c_message.BuyVolume).Append(Common.SOH);
            sb.Append(MessageSecurityStatus.TAG_TradingSessionSubID.ToString()).Append(Common.DELIMIT).Append(c_message.TradingSessionSubID).Append(Common.SOH);
            sb.Append(MessageSecurityStatus.TAG_TypeRule.ToString()).Append(Common.DELIMIT).Append(c_message.TypeRule).Append(Common.SOH);
            sb.Append(MessageSecurityStatus.TAG_DateNo.ToString()).Append(Common.DELIMIT).Append(c_message.DateNo).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (MessageSecurityStatus.TAG_SecurityStatusReqID == field.Tag)
                c_message.SecurityStatusReqID = field.Value;
            else if (MessageSecurityStatus.TAG_Symbol == field.Tag)
                c_message.Symbol = field.Value;
            else if (MessageSecurityStatus.TAG_DateNo == field.Tag)
                c_message.DateNo = field.Value;
            else if (MessageSecurityStatus.TAG_SecurityType == field.Tag)
                c_message.SecurityType = field.Value;
            else if (MessageSecurityStatus.TAG_MaturityDate == field.Tag)
            {
                c_message.MaturityDate = Utils.Convert.FromShortDate(field.Value);
            }
            else if (MessageSecurityStatus.TAG_IssueDate == field.Tag)
            {
                c_message.IssueDate = Utils.Convert.FromShortDate(field.Value);
            }
            else if (MessageSecurityStatus.TAG_TotalListingQtty == field.Tag)
            {
                bool _isSuccess = false;
                c_message.TotalListingQtty = Utils.Convert.ParseLong(field.Value, ref _isSuccess);
                if (!_isSuccess) return false;
            }
            else if (MessageSecurityStatus.TAG_Issuer == field.Tag)
            {
                c_message.Issuer = field.Value;
            }
            else if (MessageSecurityStatus.TAG_SecurityDesc == field.Tag)
            {
                c_message.SecurityDesc = field.Value;
            }
            else if (MessageSecurityStatus.TAG_HighPx == field.Tag)
            {
                bool _isSuccess = false;
                c_message.HighPx = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageSecurityStatus.TAG_LowPx == field.Tag)
            {
                bool _isSuccess = false;
                c_message.LowPx = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageSecurityStatus.TAG_HighPxOut == field.Tag)
            {
                bool _isSuccess = false;
                c_message.HighPxOut = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageSecurityStatus.TAG_LowPxOut == field.Tag)
            {
                bool _isSuccess = false;
                c_message.LowPxOut = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageSecurityStatus.TAG_HighPxRep == field.Tag)
            {
                bool _isSuccess = false;
                c_message.HighPxRep = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageSecurityStatus.TAG_LowPxRep == field.Tag)
            {
                bool _isSuccess = false;
                c_message.LowPxRep = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageSecurityStatus.TAG_LastPx == field.Tag)
            {
                bool _isSuccess = false;
                c_message.LastPx = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageSecurityStatus.TAG_SecurityTradingStatus == field.Tag)
            {
                bool _isSuccess = false;
                c_message.SecurityTradingStatus = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageSecurityStatus.TAG_TypeRule == field.Tag)
            {
                bool _isSuccess = false;
                c_message.TypeRule = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageSecurityStatus.TAG_Allowed_Trading_Subject == field.Tag)
            {
                c_message.Allowed_Trading_Subject = field.Value;
            }    
            else if (MessageSecurityStatus.TAG_Allowed_Trading_Subject_Sell == field.Tag)
            {
                c_message.Allowed_Trading_Subject_Sell = field.Value;   
            }    
            else if (MessageSecurityStatus.TAG_BuyVolume == field.Tag)
            {
                bool _isSuccess = false;
                c_message.BuyVolume = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageSecurityStatus.TAG_TradingSessionSubID == field.Tag)
            {
                c_message.TradingSessionSubID = field.Value;
            }
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}