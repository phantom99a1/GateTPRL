namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageNewOrder : FIXMessageBase
    {
        #region fields

        public string ClOrdID;  // Tag 11
        public string Account;  // Tag 1
        public string Symbol;   // Tag 55
        public int Side;        // Tag 54
        public string OrdType;  // Tag 40
        public long OrderQty;   // Tag 38
        public long OrderQty2;  // Tag 192
        public long Price;      // Tag 44
        public long Price2;     // Tag 640
        public int SpecialType; // Tag 440       //Loại yết giá đặc biệt dùng cho lệnh Marketmaker: = 1 là yết giá 1 chiều, = 2 là 2 chiều, = 3 là 2 chiều thay thế
        public string OrderNo { get; set; } = ""; // Dùng để lưu OrderNo khi push queue save db

        #endregion fields

        public MessageNewOrder() : base()
        {
            MsgType = MessageType.NewOrder;
            SpecialType = 0;
            OrderQty = 0;
            OrderQty2 = 0;
            Price = 0;
            Price2 = 0;
        }
    }
}