/* 
 * Project: FIXMessage
 * Author : Nguyen Nhat Linh – Navisoft.
 * Summary: Detail Define for Reject message
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 * Jul 10, 2009  	Linh.Nguyen     Created
 */

using System;

namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageReject : FIXMessageBase
    {
        #region define Tag

        internal const int TAG_RefSeqNum = 45; //So sequence của lệnh bị reject
        internal const int TAG_SessionRejectReason = 373; //ma loi cua ly do reject  
        #endregion 

        public int SessionRejectReason;
        public int RefSeqNum;


        public MessageReject()
            : base()
        {
            MsgType = MessageType.Reject;
        }

    }
}