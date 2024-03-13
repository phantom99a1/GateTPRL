/* 
 * Project: FIXMessage
 * Author : Nguyen Nhat Linh – Navisoft.
 * Summary: Detail Define for TestRequest message
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 * Jul 10, 2009  	Linh.Nguyen     Created
 */

using System;

namespace HNX.FIXMessage
{
    public class MessageTestRequest : FIXMessageBase
    {
        #region define Tag

        internal const int TAG_TestReqID = 112;
        #endregion

        private string _sTestReqID = null;

        public MessageTestRequest()
            : base()
        {
            MsgType = MessageType.TestRequest;
        }

        public string TestReqID
        {
            get { return _sTestReqID; }
            set { _sTestReqID = value; }
        }
    }
}