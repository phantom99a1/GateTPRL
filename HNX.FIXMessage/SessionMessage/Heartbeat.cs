/* 
 * Project: FIXMessage
 * Author : Nguyen Nhat Linh – Navisoft.
 * Summary: Detail Define for Heartbeat message
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 * Jul 10, 2009  	Linh.Nguyen     Created
 */
using System;

namespace HNX.FIXMessage
{
    public class MessageHeartbeat : FIXMessageBase
    {
        #region define Tag
        internal const int TAG_TestReqID = 112;
        #endregion
        public MessageHeartbeat()
            : base()
        {
            MsgType = MessageType.Heartbeat;
        }

        #region Member

        private string _sTestReqID = ""; //Sequence cua TestRequestMessage

        public string TestReqID
        {
            get { return _sTestReqID; }
            set { _sTestReqID = value; }
        }

        #endregion
    }
}