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
    [Serializable]
    public class MessageSecurityStatusRequest  : FIXMessageBase
    {
        public const int TAG_SecurityStatusReqID = 324;
        public const int TAG_SubscriptionRequestType = 263;
        public const int TAG_Symbol = 55;

        public MessageSecurityStatusRequest()
            : base()
        {
            MsgType = MessageType.SecurityStatusRequest;
        }

        #region Member

        private string _sSecurityStatusReqID = ""; //ID request
        private char _cSubscriptionRequestType = '0'; // 
        private string _sSymbol = "" ; // 

        public string SecurityStatusReqID
        {
            get { return _sSecurityStatusReqID; }
            set { _sSecurityStatusReqID = value; }
        }

        public char  SubscriptionRequestType
        {
            get { return _cSubscriptionRequestType; }
            set { _cSubscriptionRequestType = value; }
        }

        public string  Symbol
        {
            get { return _sSymbol; }
            set { _sSymbol = value; }
        }

        #endregion
    }
}