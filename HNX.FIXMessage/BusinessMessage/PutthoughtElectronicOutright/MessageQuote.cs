using System;
using System.Collections.Generic;
using System.Text;

namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageQuote : FIXMessageBase
    {
        #region fields
        //public int QuoteType; //537 Loại lệnh thỏa thuận điện tử 
        public string OrdType;              // 40: loai lenh: 1=Market price,2=ato,3=atc
        public string Account; //1 so tk ben quote  
        public DateTime TransactTime;  //60: thoi gian dat lenh TransactTime
        public string Symbol; //55: ma chung khoan
        public int Side;   //54: 1 = Buy, 2 = Sell
        public Int64 OrderQty; //38: khoi luong dat lenh

        public Int64 Price; //44: gia yết
        public double Yield; //236 lợi suất 
        public Int64 Price2; //640 giá thực hiện - giá bấn
        public double SettlValue; //6464 giá trị thanh toán
        public string SettDate; // 64 ngày thanh toán.
        //public string PartyID; //448: ma thanh vien Quote -->lay tu trader dat lenh

        public int SettlMethod;    //Phương thức thành toán: 1 TT ngay, 2 TT trong ngày 3 TT tương lai

        public string RegistID; //513 danh sach đại diện GD được quote,  = 0 là quote public

        public int IsVisible;       //Lệnh có ẩn danh hay ko, = 1 là ẩn danh, = 0 là ko ẩn danh
        public string ClOrdID;
        #endregion

        public MessageQuote()
            : base()
        {
            MsgType = MessageType.Quote;
            SettlValue = 0;
            Yield = 0;
            OrdType = "";
        }
    }
}
