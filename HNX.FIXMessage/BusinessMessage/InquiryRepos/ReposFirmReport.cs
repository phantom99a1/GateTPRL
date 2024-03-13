namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageReposFirmReport : FIXMessageBase
    {
        #region fields

        public string ClOrdID; // Tag 11
        public string QuoteID; // Tag 117
        public string RFQReqID; //Tag 644
        public int QuoteType; // Tag 537
        public string OrdType; // Tag 40: loai lenh: 1=Market price,2=ato,3=atc
        public string OrderPartyID; // Tag 4488
        public string InquiryMember;//Tag 4499 Mã thành viên của thăng lệnh Inquiry 
        public int Side;   //Tag 54: 1 = Buy, 2 = Sell
        public string EffectiveTime; // Tag 168
        public int RepurchaseTerm; //Tag 226
        public double RepurchaseRate; // Tag 227 Lai suat repos
        public string SettlDate; // Tag 64
        public string SettlDate2; //Tag 193
        public string EndDate; //Tag 917
        public int SettlMethod;    // Tag 6363
        public string Account;      // Tag 1
        public int NoSide; // Tag 552
        public long MatchReportType; // Tag 5632

        public ReposSideList RepoSideList;

        #endregion fields

        public MessageReposFirmReport()
            : base()
        {
            MsgType = MessageType.ReposFirmReport;
            RepoSideList = new ReposSideList();
        }
    }
}