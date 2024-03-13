/* 
 * Project: FIXMessage
 * Author : Nguyen Nhat Linh – Navisoft.
 * Summary: Detail Define for Logout message
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 * Jul 10, 2009  	Linh.Nguyen     Created
 */

using System;

namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageER_OrderCancel : MessageExecutionReport
    {
        public const int TAG_OrdStatus = 39; //trang thai cua lenh 
        public const int TAG_ClOrdID = 11; //so hieu lenh do ctck gui len
        public const int TAG_OrderID = 37;  //so thu tu message tra ve cua moi lenh 
        public const int TAG_TransactTime = 60; //thoi gian dat lenh
        public const int TAG_LeavesQty = 151; //khoi luong huy
        public const int TAG_OrigClOrdID = 41; //so hieu lenh goc
        //
        public const int TAG_Symbol = 55; //ma chung khoan
        public const int TAG_Side = 54;   //1 = Buy, 2 = Sell
        public const int TAG_OrdType = 40; //loai lenh: 1=Market price,2=ato,3=atc
        public const int TAG_Price = 44; //gia ban
        public const int TAG_Account = 1; //tai khoan khach hang
     
        #region Fields

        private string _OrdStatus = "0";
        private string _ClOrdID = "";
        private string _OrderID;
        //private DateTime _TransactTime;
        private int _LeavesQty; //khoi luong huy
        private string _OrigClOrdID; // 41: so hieu lenh goc
        //
        private string _Symbol; //55: ma chung khoan
        private string _Side;   //54: 1 = Buy, 2 = Sell
        private string _OrdType; //40: loai lenh: 1=Market price,2=ato,3=atc
        private long  _Price; //44: gia
        private string _Account; // 1:tai khoan khach hang

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

        public int LeavesQty
        {
            get { return _LeavesQty; }
            set { _LeavesQty = value; }
        }

        //
        public string Symbol
        {
            get { return _Symbol; }
            set { _Symbol = value; }
        }

        public string Side
        {
            get { return _Side; }
            set { _Side = value; }
        }

        public string OrdType
        {
            get { return _OrdType; }
            set { _OrdType = value; }
        }

        public long  Price
        {
            get { return _Price; }
            set { _Price = value; }
        }

        public string Account
        {
            get { return _Account; }
            set { _Account = value; }
        }
        #endregion

        public MessageER_OrderCancel()
            : base(  )
        {
            base.MsgType = MessageType.ExecutionReport;
            base.ExecType = ExecutionReportType.ER_CancelOrder_4;
        }


    }
}