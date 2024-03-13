/* 
 * Project: FIXMessage
 * Author : Nguyen Nhat Linh – Navisoft.
 * Summary: Detail Define for Logon message
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 * Jul 10, 2009  	Linh.Nguyen     Created
 */

using System;

namespace HNX.FIXMessage
{
    public class MessageLogon : FIXMessageBase
    {
        #region define Tag 
        internal const int TAG_EncryptMethod = 98;
        internal const int TAG_HeartBtInt = 108;
        internal const int TAG_Username = 553;
        internal const int TAG_Password = 554;
        #endregion


        #region Member


        public int EncryptMethod = 0;
        public int HeartBtInt = 30;
        public string Username = "";
        public string Password = "";

        #endregion

        public MessageLogon()
            : base()
        {
            MsgType = MessageType.Logon;
        }

    }
}