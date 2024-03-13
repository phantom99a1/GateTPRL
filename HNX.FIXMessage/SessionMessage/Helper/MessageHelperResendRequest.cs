using System.Text;

namespace HNX.FIXMessage
{
    internal class MessageHelperResendRequest : MessageHelper
    {

        private MessageResendRequest c_message;

        public MessageHelperResendRequest(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageResendRequest)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);
            if (c_message.BeginSeqNo != int.MinValue)
                sb.Append(MessageResendRequest.TAG_BeginSeqNo.ToString()).Append(Common.DELIMIT).Append(c_message.BeginSeqNo).Append(Common.SOH);
            if (c_message.EndSeqNo != int.MinValue)
                sb.Append(MessageResendRequest.TAG_EndSeqNo.ToString()).Append(Common.DELIMIT).Append(c_message.EndSeqNo).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (MessageResendRequest.TAG_BeginSeqNo == field.Tag)
            {
                bool _isSuccess = false;
                c_message.BeginSeqNo = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (MessageResendRequest.TAG_EndSeqNo == field.Tag)
            {
                bool _isSuccess = false;
                c_message.EndSeqNo = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }

}
