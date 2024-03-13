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
    public class MessageER_Order : MessageExecutionReport 
	{
        public const int TAG_OrdStatus = 39; //trang thai cua lenh 
        public const int TAG_ClOrdID = 11; //so hieu lenh do ctck gui len
        public const int TAG_OrderID = 37;  //so thu tu message tra ve cua moi lenh 
        public const int TAG_TransactTime = 60; //thoi gian dat lenh
        //
        public const int TAG_Symbol = 55; //ma chung khoan
        public const int TAG_Side = 54;   //1 = Buy, 2 = Sell
        public const int TAG_OrderQty = 38; //khoi luong dat lenh
        //public const int TAG_OrderQty2 = 192;// khoi luong 
        public const int TAG_OrdType = 40; //loai lenh: 1=Market price,2=ato,3=atc
        public const int TAG_Price = 44; //gia ban
        public const int TAG_Account = 1; //tai khoan khach hang
        public const int TAG_SettlValue = 6464; // giá trị thanh toán

        //public const int TAG_StopPx = 99; //gia dung

        #region Fields

        private string _OrdStatus = "0";
        private string _ClOrdID = "";
        private string _OrderID;
        //private DateTime  _TransactTime  ;
        //
        private string _Symbol; //55: ma chung khoan
        private string _Side;   //54: 1 = Buy, 2 = Sell
        private int _OrderQty; //38: khoi luong dat lenh
        //private int _OrderQty2;
        private string _OrdType; //40: loai lenh: 1=Market price,2=ato,3=atc
        private long   _Price; //44: gia
        private string _Account; // 1:tai khoan khach hang


        //private double _StopPx;

        //public double StopPx
        //{
        //    get { return _StopPx; }
        //    set { _StopPx = value; }
        //} 
        public string  OrdStatus
        {
            get { return _OrdStatus; }
            set { _OrdStatus = value; }
        }

        public string ClOrdID
        {
            get { return _ClOrdID; }
            set { _ClOrdID = value; }
        }


        public string  OrderID
        {
            get { return _OrderID; }
            set { _OrderID = value; }
        }

       /* public DateTime   TransactTime
        {
            get { return _TransactTime; }
            set { _TransactTime = value; }
        }
*/
        //public int OrderQty2
        //{
        //    get { return _OrderQty2; }
        //    set { _OrderQty2 = value; }
        //}
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

        public int OrderQty
        {
            get { return _OrderQty; }
            set { _OrderQty = value; }
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

        private double _SettlValue;
        public double SettlValue
        {
            get { return _SettlValue; }
            set { _SettlValue = value; }
        }


        #endregion

        public MessageER_Order()
            : base()
        {
            base.MsgType = MessageType.ExecutionReport;
            base.ExecType = ExecutionReportType.ER_Order_0;
        }

	}
}