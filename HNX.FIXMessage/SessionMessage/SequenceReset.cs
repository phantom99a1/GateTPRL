/* 
 * Project: FIXMessage
 * Author : Nguyen Nhat Linh – Navisoft.
 * Summary: Detail Define for SequenceRequest message
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 * Jul 10, 2009  	Linh.Nguyen     Created
 */

using System;

namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageSequenceReset : FIXMessageBase
    {
        #region define Tag

        internal const int TAG_GapFillFlag = 123;
        internal const int TAG_NewSeqNo = 36;
        #endregion

        private bool _bGapFillFlag;
        private int _iNewSeqNo;

        public MessageSequenceReset()
            : base()
        {
            MsgType = MessageType.SequenceReset;
        }

        public bool GapFillFlag
        {
            get { return _bGapFillFlag; }
            set { _bGapFillFlag = value; }
        }

        public int NewSeqNo
        {
            get { return _iNewSeqNo; }
            set { _iNewSeqNo = value; }
        }
    }
}