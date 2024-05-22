namespace HNX.FIXMessage
{
    public class MessageQuoteSatusReport : FIXMessageBase
    {
        /// <summary>
        /// Tag 537 Loại lệnh thỏa thuận điện tử
        /// </summary>
        public int QuoteType;

        /// <summary>
        /// Tag 131 SHL của lệnh muốn sửa/hủy với case response cho sửa/hủy
        /// </summary>
        public string QuoteReqID;

        /// <summary>
        /// Tag 40: loai lenh: 1=Market price,2=ato,3=atc
        /// </summary>
        public string OrdType;

        /// <summary>
        /// Tag 171: SHL HNX của lệnh mới sinh ra
        /// </summary>
        public string QuoteID;

        /// <summary>
        /// Tag 1 so tk ben quote
        /// </summary>
        public string Account;

        /// <summary>
        /// Tag 60: thoi gian dat lenh
        /// </summary>
        public string TransactTime;

        /// <summary>
        /// Tag 55: ma chung khoan
        /// </summary>
        public string Symbol;

        /// <summary>
        /// Tag 54: 1 = Buy, 2 = Sell
        /// </summary>
        public int Side;

        /// <summary>
        /// Tag 38: khoi luong dat lenh
        /// </summary>
        public Int64 OrderQty;

        /// <summary>
        /// Tag 44 Gia
        /// </summary>
        public Int64 Price;
       
        /// <summary>
        /// Tag 640 giá thực hiện - giá bấn
        /// </summary>
        public Int64 Price2;

        /// <summary>
        /// Tag 6464 giá trị thanh toán
        /// </summary>
        public double SettlValue;

        /// <summary>
        /// Tag 64 ngày thanh toán.
        /// </summary>
        public string SettDate;

        /// <summary>
        /// Tag 6363 Phương thức thành toán: 1 TT ngay, 2 TT trong ngày 3 TT tương lai
        /// </summary>
        public int SettlMethod;

        //public string PartyID; //448: ma thanhvien Quote

        /// <summary>
        /// Tag 513 danh sach đại diện GD được quote,  = 0 là quote public
        /// </summary>
        public string RegistID;

        /// <summary>
        /// Tag 11
        /// </summary>
        public string ClOrdID;

        /// <summary>
        /// Tag 4488
        /// </summary>
        public string OrderPartyID;

        public string OrderNo { get; set; } = ""; // Dùng để lưu OrderNo khi push queue save db

        public MessageQuoteSatusReport()
           : base()
        {
            Price = 0;
            OrderQty = 0;            
            Price2 = 0;
            SettlValue = 0;
            SettDate = string.Empty;
            RegistID = "";
            MsgType = MessageType.QuoteStatusReport;
            SenderCompID = "HNX.TPDN";
        }
    }
}