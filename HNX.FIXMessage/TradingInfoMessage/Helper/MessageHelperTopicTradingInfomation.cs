using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperTopicTradingInfomation : MessageHelper
    {
        private MessageTopicTradingInfomation c_message;

        #region #Tag const

        private const int TAG_InquiryMember = 4499;
        private const int TAG_Symbol = 55;

        #endregion #Tag const

        public MessageHelperTopicTradingInfomation(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageTopicTradingInfomation)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);
            sb.Append(TAG_InquiryMember.ToString()).Append(Common.DELIMIT).Append(c_message.InquiryMember).Append(Common.SOH);
            sb.Append(TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(c_message.Symbol).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (TAG_InquiryMember == field.Tag)
                c_message.InquiryMember = field.Value;
            else if (TAG_Symbol == field.Tag)
                c_message.Symbol = field.Value;
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}