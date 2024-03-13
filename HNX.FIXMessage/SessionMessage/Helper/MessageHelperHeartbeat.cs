using System.Text;

namespace HNX.FIXMessage
{
    internal class MessageHelperHeartbeat : MessageHelper
    {
  

        private MessageHeartbeat c_message;

        public MessageHelperHeartbeat(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageHeartbeat)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);
            if (c_message.TestReqID != null)
                sb.Append(MessageHeartbeat.TAG_TestReqID.ToString()).Append(Common.DELIMIT).Append(c_message.TestReqID).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (MessageHeartbeat.TAG_TestReqID == field.Tag)
                c_message.TestReqID = field.Value;
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}
