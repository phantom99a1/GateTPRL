/*
 * Project: FIXMessage
 * Author : Nguyen Nhat Linh – Navisoft.
 * Summary: Detail Define for NewOrder message, lenh BCGD Outright
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 * Jul 10, 2009  	Linh.Nguyen     Created
 */

namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageNewOrderCross : FIXMessageBase
    {
        #region fields

        public string OrdType;              // 40: loai lenh: 1=Market price,2=ato,3=atc

        public string Account; // 1:tai khoan khach hang ben ban
        public string CoAccount; // 2:tai khoan khach hang ben mua

        public DateTime TransactTime;  //60: thoi gian dat lenh
        public string Symbol; //55: ma chung khoan
        public int Side;   //54: 1 = Buy, 2 = Sell
        public Int64 OrderQty; //38: khoi luong dat lenh
        public Int64 Price; //44: gia yết
        public string PartyID; //448: ma thanh vien ban ban
        public string CoPartyID; //449: ma thanh vien ban mua
        public int CrossType; //549: hinh thuc thoa thuan
        public string CrossID; //548 SHL trong truong hop tu

        public string EffectiveTime; //ngày bat dau giao dich
        public double Yield; //236 lợi suất
        public Int64 Price2; //640 giá thực hiện - giá bấn
        public double SettlValue; //6464 giá trị thanh toán
        public string SettDate; // 64 ngày thanh toán.
        public string ClOrdID; //11: so hieu lenh cua cong ty CK

        public int SettlMethod;    //Phương thức thành toán: 1 TT ngay, 2 TT trong ngày 3 TT tương lai

        #endregion fields

        public MessageNewOrderCross()
            : base()
        {
            MsgType = MessageType.NewOrderCross;
        }
    }
}