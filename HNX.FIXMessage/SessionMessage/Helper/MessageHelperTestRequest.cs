using System.Text;

namespace HNX.FIXMessage
{
    internal class MessageHelperTestRequest : MessageHelper
    {
        private MessageTestRequest _message;

        public MessageHelperTestRequest(FIXMessageBase message)
            : base(message)
        {
            _message = (MessageTestRequest)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);
            if (_message.TestReqID != null)
                sb.Append(MessageTestRequest.TAG_TestReqID.ToString()).Append( Common.DELIMIT).Append(_message.TestReqID).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (MessageTestRequest.TAG_TestReqID == field.Tag)
                _message.TestReqID = field.Value;
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }

}
