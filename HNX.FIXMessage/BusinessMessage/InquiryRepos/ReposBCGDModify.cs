namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageReposBCGDModify : FIXMessageBase
    {
        #region fields

        public string ClOrdID; // Tag 11
        public string OrgOrderID; // Tag 198
        public int QuoteType; // Tag 563
        public string OrdType; // Tag 40
        public int Side;   // Tag 54: 1 = Buy, 2 = Sell
        public string Account; // 1:tai khoan khach hang ben ban
        public string CoAccount; // 2:tai khoan khach hang ben mua
        public string PartyID; //448: ma thanh vien ban ban
        public string CoPartyID; //449: ma thanh vien ban mua
        public string EffectiveTime; // Tag 168
        public int SettlMethod;    // Tag 6363
        public string SettlDate; // Tag 64 ngày thanh toán.
        public string SettlDate2; //Tag 193
        public string EndDate; //Tag 917
        public int RepurchaseTerm; //Tag 226
        public double RepurchaseRate; //Tag 227
        public long NoSide; // Tag 552

        public string OrderNo { get; set; } = ""; // Dùng để lưu OrderNo khi push queue save db
        //
        public ReposSideList RepoSideList;
        #endregion fields

        public MessageReposBCGDModify()
            : base()
        {
            MsgType = MessageType.ReposBCGDModify;
            RepoSideList = new ReposSideList();
        }
    }
}