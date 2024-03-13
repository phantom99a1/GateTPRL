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
    public class MessageTradingSessionStatus : FIXMessageBase
    {
        public const int TAG_TradingSessionID = 336;
        public const int TAG_TradSesStatus = 340;
        public const int TAG_TradSesStartTime = 341;
        public const int TAG_TradSesReqID = 335;
        public const int TAG_TradSesMode = 339;

        public MessageTradingSessionStatus()
            : base()
        {
            MsgType = MessageType.TradingSessionStatus;
        }


        #region Member

        private string _sTradingSessionID = ""; //ID cua request can tra loi
        private string _iTradSesStatus; // 
        private DateTime _TradSesStartTime; // 
        private string _TradSesReqID;//335
        private int _TradSesMode;

        public string TradSesReqID
        {
            get { return _TradSesReqID; }
            set { _TradSesReqID = value; }
        }

        public string TradingSessionID
        {
            get { return _sTradingSessionID; }
            set { _sTradingSessionID = value; }
        }

        public string TradSesStatus
        {
            get { return _iTradSesStatus; }
            set { _iTradSesStatus = value; }
        }

        public DateTime TradSesStartTime
        {
            get { return _TradSesStartTime; }
            set { _TradSesStartTime = value; }
        }

        public int TradSesMode
        {
            get { return _TradSesMode; }
            set { _TradSesMode = value; }
        }
        #endregion
    }
}