namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageReposBCGDReport : FIXMessageBase
    {
        #region fields

        public string ClOrdID; // Tag 11
        public string OrgOrderID; // Tag 198
        public string OrderID; // 37 SHL
        public int QuoteType; // Tag 563
        public string OrdType;// Tag 40: loai lenh: 1=Market price,2=ato,3=atc
        public string OrderPartyID; // Tag 4488
        public string InquiryMember;//Tag 4499 Mã thành viên của thăng lệnh Inquiry
        public int Side;   // Tag 54: 1 = Buy, 2 = Sell
        public string EffectiveTime; // Tag 168
        public int RepurchaseTerm; //Tag 226
        public double RepurchaseRate; // Tag 227 Lai suat repos
        public string SettlDate; // Tag 64
        public string SettlDate2; //Tag 193
        public string EndDate; //Tag 917
        public int SettlMethod;    //TAG 6363 phwuong thuc hanh toán, mặc định = 1
        public string Account; // Tag 1
        public string CoAccount; // Tag 2
        public string PartyID; //Tag 448: ma thanh vien ban dat
        public string CoPartyID; //Tag 449: ma thanh vien ben doi ung neu co
        public int NoSide; // Tag 552 So mã chứng khoán trong lệnh khớp
        public int MatchReportType; // Tag 5632 Dùng để gửi ra báo là của lệnh leg2 hay ko default = 1

        public ReposSideReposBCGDReportList RepoBCGDSideList;
        public string OrderNo { get; set; } = ""; // Dùng để lưu OrderNo khi push queue save db
        #endregion fields

        public MessageReposBCGDReport()
            : base()
        {
            MsgType = MessageType.ReposBCGDReport;
            MatchReportType = 1;
            RepoBCGDSideList = new ReposSideReposBCGDReportList();
        }

        [Serializable]
        public class ReposSideReposBCGDReport
        {
            //Các thông tin này lặp lại
            public long NumSide { get; set; } = 0; // Tag 5522
            public string Symbol { get; set; } = ""; //Tag 55
            public long OrderQty { get; set; } = 0; // Tag 38
            public long Price { get; set; } = 0; // Tag 44
            public long ExecPrice { get; set; } = 0; // Tag 640
            public double ReposInterest { get; set; } = 0;         //2261
            public double HedgeRate { get; set; } = 0; // Tag 2260
            public double SettlValue { get; set; } = 0;        //6464 giá trị thanh toán
            public double SettlValue2 { get; set; } = 0;        //6465 giá trị thanh toán lan 2
            //Ket thuc thông tin lặp lại
        }

        [Serializable]
        public class ReposSideReposBCGDReportList
        {
            private System.Collections.ArrayList _al;

            public ReposSideReposBCGDReport this[int i]
            {
                get
                {
                    if (_al != null && i < _al.Count)
                        return (ReposSideReposBCGDReport)_al[i];
                    else
                        return null;
                }
            }

            public int Count
            {
                get
                {
                    if (_al != null)
                        return _al.Count;
                    else
                        return 0;
                }
            }

            public int SetLength
            {
                set
                {
                    int _length = value;
                    _al = null;
                    for (int i = 0; i < _length; i++)
                    {
                        Add(new ReposSideReposBCGDReport());
                    }
                }
            }

            public void Clear()
            {
                _al = null;
            }

            public void Add(ReposSideReposBCGDReport item)
            {
                if (_al == null)
                    _al = new System.Collections.ArrayList();
                _al.Add(item);
            }
        }
    }
}