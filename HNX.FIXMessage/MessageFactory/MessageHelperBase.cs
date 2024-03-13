using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelper
    {
        private FIXMessageBase c_message;

        /// <summary>
        /// Initialize instance of MessageHelper.
        /// </summary>
        public MessageHelper(FIXMessageBase message)
        {
            c_message = message;
        }

        public virtual void BuildHeader(StringBuilder sb)
        {
            sb.Append(FIXMessageBase.TAG_MsgType).Append(Common.DELIMIT).Append(c_message.MsgType).Append(Common.SOH);
            sb.Append(FIXMessageBase.TAG_SenderCompID).Append(Common.DELIMIT).Append(c_message.SenderCompID).Append(Common.SOH);
            sb.Append(FIXMessageBase.TAG_TargetCompID).Append(Common.DELIMIT).Append(c_message.TargetCompID).Append(Common.SOH);
            //if (c_message.TargetSubID != null && c_message.TargetSubID.Length > 0)
            //    sb.Append(FIXMessageBase.TAG_TargetSubID).Append(Common.DELIMIT).Append(c_message.TargetSubID).Append(Common.SOH);
            sb.Append(FIXMessageBase.TAG_MsgSeqNum).Append(Common.DELIMIT).Append(c_message.MsgSeqNum).Append(Common.SOH);
            //if (c_message.LastMsgSeqNumProcessed > 0)
            sb.Append(FIXMessageBase.TAG_LastMsgSeqNumProcessed).Append(Common.DELIMIT).Append(c_message.LastMsgSeqNumProcessed).Append(Common.SOH);
            c_message.SendingTime = DateTime.Now;
            sb.Append(FIXMessageBase.TAG_SendingTime).Append(Common.DELIMIT).Append(Utils.Convert.ToFIXUTCTimestamp(c_message.SendingTime)).Append(Common.SOH);
            if (c_message.Text != null && c_message.Text.Length > 0)
                sb.Append(FIXMessageBase.TAG_Text).Append(Common.DELIMIT).Append(c_message.Text).Append(Common.SOH);
            if (c_message.ExecType != '\0')
                sb.Append(FIXMessageBase.TAG_ExecType).Append(Common.DELIMIT).Append(c_message.ExecType).Append(Common.SOH);
            if (c_message.RefMsgType != null && c_message.RefMsgType != "")
                sb.Append(FIXMessageBase.TAG_RefMsgType).Append(Common.DELIMIT).Append(c_message.RefMsgType).Append(Common.SOH);
            if (c_message.PossDupFlag == true)
                sb.Append(FIXMessageBase.TAG_PossDupFlag).Append(Common.DELIMIT).Append(Utils.Convert.ToFIXBoolean(c_message.PossDupFlag).ToString()).Append(Common.SOH);

            //chua co heartbeart
        }

        /// <summary>
        /// Build the standard body tags and values into a StringBuilder.
        /// </summary>
        public virtual void BuildBody(StringBuilder sb)
        {
        }

        /// <summary>
        /// Inserts standard header tags and values into a message.
        /// The BeginString and BodyLen are only 2 that need to be inserted.
        /// </summary>
        public virtual void InsertHeader(StringBuilder sb)
        {
            //This information must be INSERTED in the begining of the message.  Since we are inserting it must be done in REVERSE order.
            sb.Insert(0, Common.SOH).Insert(0, c_message.BodyLength).Insert(0, Common.DELIMIT).Insert(0, FIXMessageBase.TAG_BodyLength);
            sb.Insert(0, Common.SOH).Insert(0, Common.VERSION).Insert(0, Common.DELIMIT).Insert(0, FIXMessageBase.TAG_BeginString);
        }

        /// <summary>
        /// Appends the standard trailer tags and values into a message.
        /// Only the CheckSum is appended.
        /// </summary>
        public virtual void AppendTrailer(StringBuilder sb)
        {
            sb.Append(FIXMessageBase.TAG_CheckSum).Append(Common.DELIMIT).Append("000"/*c_message.CheckSum.ToString("000")*/).Append(Common.SOH);
        }

        /// <summary>
        /// Parse a Field object into a FIX message object.
        /// The Field object contains a FIX string that must be converted to the proper data type.
        /// </summary>
        public virtual bool ParseField(Field field)
        {
            if (FIXMessageBase.TAG_BeginString == field.Tag)
                c_message.BeginString = field.Value;
            else if (FIXMessageBase.TAG_BodyLength == field.Tag)
            {
                bool _isSuccess = false;
                c_message.BodyLength = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (FIXMessageBase.TAG_MsgType == field.Tag)
                c_message.MsgType = field.Value;
            else if (FIXMessageBase.TAG_SenderCompID == field.Tag)
                c_message.SenderCompID = field.Value;
            else if (FIXMessageBase.TAG_TargetCompID == field.Tag)
                c_message.TargetCompID = field.Value;
            //else if (FIXMessageBase.TAG_TargetSubID == field.Tag)
            //    c_message.TargetSubID = field.Value;
            else if (FIXMessageBase.TAG_MsgSeqNum == field.Tag)
            {
                bool _isSuccess = false;
                c_message.MsgSeqNum = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            //else if (FIXMessageBase.TAG_NextExpectedMsgSeqNum == field.Tag)
            //{
            //    bool _isSuccess = false;
            //    c_message.NextExpectedMsgSeqNum = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
            //    if (_isSuccess == false) return false;
            //}
            else if (FIXMessageBase.TAG_SendingTime == field.Tag)
                c_message.SendingTime = Utils.Convert.FromFIXUTCTimestamp(field.Value);
            else if (FIXMessageBase.TAG_Text == field.Tag)
                c_message.Text = field.Value;
            else if (FIXMessageBase.TAG_LastMsgSeqNumProcessed == field.Tag)
            {
                bool _isSuccess = false;
                c_message.LastMsgSeqNumProcessed = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (FIXMessageBase.TAG_PossDupFlag == field.Tag)
            {
                if (field.Value.Length > 0)
                    c_message.PossDupFlag = Utils.Convert.FromFIXBoolean(field.Value[0]);
            }
            else if (FIXMessageBase.TAG_ExecType == field.Tag)
            {
                if (field.Value.Length > 0)
                    c_message.ExecType = field.Value[0];
            }
            else if (FIXMessageBase.TAG_Signature == field.Tag)
            {
                c_message.Signature = field.Value;
            }
            else if (FIXMessageBase.TAG_CheckSum == field.Tag)
                c_message.CheckSum = byte.Parse(field.Value);
            else if (FIXMessageBase.TAG_RefMsgType == field.Tag)
                c_message.RefMsgType = field.Value;
            else
                c_message.Fields.Add(field);

            return true;
        }

        public virtual void AddReposSide()
        {
            //Do Nothing
        }
    }
}