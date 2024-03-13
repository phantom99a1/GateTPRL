using System;
using System.Text;

namespace HNX.FIXMessage
{
    internal class MessageHelperSequenceReset : MessageHelper
    {
        private MessageSequenceReset c_message;

        public MessageHelperSequenceReset(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageSequenceReset)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);
            if (c_message.GapFillFlag)
                sb.Append(MessageSequenceReset.TAG_GapFillFlag.ToString()).Append(Common.DELIMIT).Append(Utils.Convert.ToFIXBoolean(c_message.GapFillFlag)).Append(Common.SOH);
            sb.Append(MessageSequenceReset.TAG_NewSeqNo.ToString()).Append(Common.DELIMIT).Append(c_message.NewSeqNo).Append(Common.SOH);
        }

        public override bool ParseField(Field field)
        {
            if (MessageSequenceReset.TAG_GapFillFlag == field.Tag)
                c_message.GapFillFlag =Utils.Convert.FromFIXBoolean(field.Value);
            else if (MessageSequenceReset.TAG_NewSeqNo == field.Tag)
            {
                bool _isSuccess = false;
                c_message.NewSeqNo = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }
    }
}
