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
    public class MessageUserRequest : FIXMessageBase
    {

        #region #Tag const

        public const int TAG_UserRequestID = 923; //ID cua yeu cau gui len HNX
        public const int TAG_UserRequestType = 924; //Loai yeu cau
        public const int TAG_Username = 553; //User đăng nhập hệ thống
        public const int TAG_Password = 554; //mat khau
        public const int TAG_NewPassword = 925; //Mat khau moi

        #endregion

        #region fields
        private string _UserRequestID; // 
        private int _UserRequestType; //
        private string _Username;
        private string _Password;
        private string _NewPassword;
        //

        public string UserRequestID
        {
            get { return _UserRequestID; }
            set { _UserRequestID = value; }
        }

        public int UserRequestType
        {
            get { return _UserRequestType; }
            set { _UserRequestType = value; }
        }
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        public string NewPassword
        {
            get { return _NewPassword; }
            set { _NewPassword = value; }
        }
        #endregion

        public MessageUserRequest()
            : base()
        {
            MsgType = MessageType.UserRequest;
        }
    }
}