namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageER_ExecOrder : MessageExecutionReport
    {
        public const int TAG_OrdStatus = 39; //trang thai cua lenh
        public const int TAG_ClOrdID = 11; //so hieu lenh của công ty ck gửi lên. Chỉ có giá trị dối với lệnh khớp của thỏa thuận cùng bên
        public const int TAG_OrigClOrdID = 41; //so hieu lenh goc cua 1 lenh trong cap lenh khop
        public const int TAG_SecondaryClOrdID = 526; //so hieu lenh goc cua lenh doi ung trong lenh khop
        public const int TAG_OrderID = 37;  //so thu tu message tra ve cua moi lenh
        public const int TAG_TransactTime = 60; //thoi gian dat lenh
        public const int TAG_LastQty = 32; //khoi luong khop
        public const int TAG_LastPx = 31; // gia khop
        public const int TAG_ExecID = 17; //so xac nhan khop
        public const int TAG_Symbol = 55; //ma chung khoan
        public const int TAG_Side = 54;   //1 = Buy, 2 = Sell
        public const int TAG_SettlValue = 6464; //
        public const int TAG_ReciprocalMember = 448; //
        #region Fields
        private string _OrdStatus = "0";
        private string _ClOrdID = "";
        private string _OrderID;
        //private DateTime _TransactTime;
        private int _LastQty; //khoi luong khop
        private long _LastPx; // gia khop
        private string _ExecID; //so xac nhan khop
        private string _OrigClOrdID; //so hieu lenh goc
        public string _SecondaryClOrdID; //so hieu lenh goc cua lenh doi ung trong lenh khop
        //
        private string _Symbol; //55: ma chung khoan
        private int _Side;   //54: 1 = Buy, 2 = Sell
        private string _ReciprocalMember;   //448
        public string OrderNo { get; set; } = ""; // Dùng để lưu OrderNo khi push queue save db


        public string OrdStatus
        {
            get { return _OrdStatus; }
            set { _OrdStatus = value; }
        }

        public string ClOrdID
        {
            get { return _ClOrdID; }
            set { _ClOrdID = value; }
        }

        public string OrigClOrdID
        {
            get { return _OrigClOrdID; }
            set { _OrigClOrdID = value; }
        }

        public string SecondaryClOrdID
        {
            get { return _SecondaryClOrdID; }
            set { _SecondaryClOrdID = value; }
        }

        public string OrderID
        {
            get { return _OrderID; }
            set { _OrderID = value; }
        }

        /*public DateTime TransactTime
        {
            get { return _TransactTime; }
            set { _TransactTime = value; }
        }*/

        public int LastQty
        {
            get { return _LastQty; }
            set { _LastQty = value; }
        }

        public long LastPx
        {
            get { return _LastPx; }
            set { _LastPx = value; }
        }

        public string ExecID
        {
            get { return _ExecID; }
            set { _ExecID = value; }
        }

        /// <summary>
        public string Symbol
        {
            get { return _Symbol; }
            set { _Symbol = value; }
        }

        public int Side
        {
            get { return _Side; }
            set { _Side = value; }
        }

        public string ReciprocalMember
        {
            get { return _ReciprocalMember; }
            set { _ReciprocalMember = value; }
        }

        private double _SettlValue;

        public double SettlValue
        {
            get { return _SettlValue; }
            set { _SettlValue = value; }
        }

        #endregion Fields

        public MessageER_ExecOrder()
            : base()
        {
            base.MsgType = MessageType.ExecutionReport;
            base.ExecType = ExecutionReportType.ER_ExecOrder_3;
        }
    }
}