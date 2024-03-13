namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageUserResponse : FIXMessageBase
    {
        #region #Tag const

        public const int TAG_UserRequestID = 923; //ID cua yeu cau gui len HNX
        public const int TAG_Username = 553; //User đăng nhập hệ thống
        public const int TAG_UserStatus = 926; //User đăng nhập hệ thống
        public const int TAG_UserStatusText = 927; //User đăng nhập hệ thống

        #endregion #Tag const

        #region fields

        private string _UserRequestID; //
        private string _Username;
        private int _UserStatus;
        private string _UserStatusText;
        //

        public string UserRequestID
        {
            get { return _UserRequestID; }
            set { _UserRequestID = value; }
        }

        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        public int UserStatus
        {
            get { return _UserStatus; }
            set { _UserStatus = value; }
        }

        public string UserStatusText
        {
            get { return _UserStatusText; }
            set { _UserStatusText = value; }
        }

        #endregion fields

        public MessageUserResponse()
            : base()
        {
            MsgType = MessageType.UserResponse;
        }
    }
}