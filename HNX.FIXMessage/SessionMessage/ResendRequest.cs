/* 
 * Project: FIXMessage
 * Author : Nguyen Nhat Linh – Navisoft.
 * Summary: Detail Define for ResentRequest message
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 * Jul 10, 2009  	Linh.Nguyen     Created
 */

using System;

namespace HNX.FIXMessage
{
    public class MessageResendRequest : FIXMessageBase
    {
        #region define Tag

        internal const int TAG_BeginSeqNo = 7;
        internal const int TAG_EndSeqNo = 16;
        #endregion

        private int _iBeginSeqNo = int.MinValue;
        private int _iEndSeqNo = int.MinValue;

        public MessageResendRequest()
            : base()
        {
            MsgType = MessageType.ResendRequest;
        }

        public int BeginSeqNo
        {
            get { return _iBeginSeqNo; }
            set { _iBeginSeqNo = value; }
        }

        public int EndSeqNo
        {
            get { return _iEndSeqNo; }
            set { _iEndSeqNo = value; }
        }

    }
}