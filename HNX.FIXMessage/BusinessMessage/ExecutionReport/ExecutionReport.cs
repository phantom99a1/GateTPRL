/* 
 * Project: FIXMessage
 * Author : Nguyen Nhat Linh – Navisoft.
 * Summary: Detail Define for NewOrder message
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 * Jul 10, 2009  	Linh.Nguyen     Created
 */

using System;

namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageExecutionReport : FIXMessageBase
    {

        #region fields
        public string Account; // 1:tai khoan khach hang
        public DateTime TransactTime;  //60: thoi gian dat lenh
        public string Symbol; //55: ma chung khoan
        public int Side;   //54: 1 = Buy, 2 = Sell
        public Int64 OrderQty; //38: khoi luong dat lenh 
        public string OrdType; //40: loai lenh: 1=Market price,2=ato,3=atc
        public Int64 Price; //44: gia 
        public string PartyID; //448: ma thanh vien đặt lệnh

        public string OrderID; // 37 SHL 
        public string OrgOrderID; //198 SHL gốc, ý nghĩa với confirm sửa/hủy lệnh
        public string ClOrdID; // 372

        public int RejectReason;// 103 ly do reject
       
       
        

        /// <summary>
        /// Tam để lại để xem xét lệnh MM tách theo ordertype hay theo flag riêng
        /// Loại lệnh đặc biêt MM, ShortSell, Margin, = 1 là yết giá 1 chiều với OrdType = MM
        /// = 2 là yết giá 2 chiều với OrdType = MM, = 3 là 2 chiều thay thế với OrdType = MM.
        /// </summary>
        public int Special_Type;            // 440

      
        public Int64 Price2; //640 giá thực hiện - giá bấn
        public double SettlValue; //6464 giá trị thanh toán
        public DateTime SettDate; // 64 ngày thanh toán.


        #endregion

        public MessageExecutionReport()
            : base()
        {
            MsgType = MessageType.ExecutionReport;
        }

    }


}