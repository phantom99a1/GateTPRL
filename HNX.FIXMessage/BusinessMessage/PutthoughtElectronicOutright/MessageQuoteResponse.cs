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
    public class MessageQuoteResponse : FIXMessageBase
    {

        #region fields
        public int QuoteRespType; //537 Loại lệnh thỏa thuận điện tử 
        public string QuoteRespID; //644 SHL cua lenh quote muốn sửa
        public string OrdType;              // 40: loai lenh: 1=Market price,2=ato,3=atc
        public string Account; //1 so tk ben đối ứng        public string Account; //1 so tk ben đối ứng
        public string CoAccount; //2

        //public string PartyID; //448 Mã thành viên quote
        //public string OrderID; //37 shl mới
        public string Symbol; //55 mã ck Xác nhận vẫn cần gửi Symbol lên

        public int Side;   //54: 1 = Buy, 2 = Sell --> ngược lại với thông tin lệnh muốn Quote
        public Int64 OrderQty; //38: khoi luong dat lenh

        public Int64 Price; //44: gia yết
        public double Yield; //236 lợi suất 
        public Int64 Price2; //640 giá thực hiện - giá bấn
        public double SettlValue; //6464 giá trị thanh toán
        public string SettDate; // 64 ngày thanh toán. 

        public int SettlMethod;    //Phương thức thành toán: 1 TT ngay, 2 TT trong ngày 3 TT tương lai

        //public string RegistID; //513 danh sach đại diện GD được quote,  = 0 là quote public
        public string ClOrdID;
        public string OrderNo { get; set; } = ""; // Dùng để lưu OrderNo khi push queue save db
        #endregion

        public MessageQuoteResponse()
            : base()
        {
            MsgType = MessageType.QuoteResponse;
        }

    }


}