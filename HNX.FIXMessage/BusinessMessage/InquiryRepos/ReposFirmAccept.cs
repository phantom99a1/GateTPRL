namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageReposFirmAccept : FIXMessageBase
    {
        #region fields
        public string ClOrdID ; // Tag 11
        public string RFQReqID; // Tag 644
        public int QuoteType; // Tag 537 1 nhap firm, 2 sửa firm, 3 hủy firm
        public string OrdType;              // 40: loai lenh: I --> là lệnh inquyri repos
        public string Account;  // Tag 1 Tai khoan ben doi ung
        public string CoAccount;  

        public double RepurchaseRate; // Tag 227
		public long NoSide; // Tag 552
		public ReposSideList RepoSideList;

		#endregion fields

		public MessageReposFirmAccept()
            : base()
        {
            MsgType = MessageType.ReposFirmAccept;
			RepoSideList = new ReposSideList();
		}
	}
}