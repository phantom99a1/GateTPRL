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
    public class MessageTradingSessionStatusRequest  : FIXMessageBase
    {
        public const int TAG_TradSesReqID = 335;
        public const int TAG_SubscriptionRequestType = 263;
        public const int TAG_TradSesMode = 339;

        public MessageTradingSessionStatusRequest()
            : base()
        {
            MsgType = MessageType.TradingSessionStatusRequest;
        }

        #region Member

        private string _sTradSesReqID = ""; //ID request
        private char _cSubscriptionRequestType ='2'; // 
        private int _TradSesMode;

        /// <summary>
        /// Tag 339
        /// </summary>
        public int TradSesMode
        {
            get { return _TradSesMode; }
            set { _TradSesMode = value; }
        }

        /// <summary>
        /// Tag 335
        /// </summary>
        public string TradSesReqID
        {
            get { return _sTradSesReqID; }
            set { _sTradSesReqID = value; }
        }

        /// <summary>
        /// Tag 263
        /// </summary>
        public char  SubscriptionRequestType
        {
            get { return _cSubscriptionRequestType; }
            set { _cSubscriptionRequestType = value; }
        }

        #endregion

    }
}