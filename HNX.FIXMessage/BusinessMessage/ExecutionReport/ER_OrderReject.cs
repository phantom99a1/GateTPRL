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
    public class MessageER_OrderReject : MessageExecutionReport
    {
        public const int TAG_OrdStatus = 39; //trang thai cua lenh 
        public const int TAG_ClOrdID = 11; //so hieu lenh do ctck gui len
        public const int TAG_OrderID = 37;  //so thu tu message tra ve cua moi lenh 
        //public const int TAG_TransactTime = 60; //thoi gian dat lenh      
        public const int TAG_OrdRejReason = 103; //ma xac dinh ly do tu choi lenh
        public const int TAG_UnderlyingLastQty = 652; //khoi luong bi reject

        #region VuTT them 30/11/2012
        public const int TAG_Side = 54;
        public const int TAG_OrdType = 40;// Loại lệnh
        #endregion
       
        #region Fields

        private string _OrdStatus = "0";
        private string _ClOrdID = "";
        private string _OrderID;
        private DateTime _TransactTime;
        public string _SecondaryClOrdID; //so hieu lenh goc cua lenh doi ung trong lenh khop        
        public int _UnderlyingLastQty; //khoi luong bi reject     
        //
        private int _OrdRejReason; //103: ma xac dinh ly do tu choi lenh

        private string _Side;
        private string _OrdType;

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

        //

        public int OrdRejReason
        {
            get { return _OrdRejReason; }
            set { _OrdRejReason = value; }
        }

        public int UnderlyingLastQty
        {
            get { return _UnderlyingLastQty; }
            set { _UnderlyingLastQty = value; }
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
        #endregion

        public MessageER_OrderReject()
            : base()
        {
            base.MsgType = MessageType.ExecutionReport;
            base.ExecType = ExecutionReportType.ER_Rejected_8;
        }
    }
}