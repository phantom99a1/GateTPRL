namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageExecOrderRepos : FIXMessageBase
    {
        #region MyRegion

        public string ClOrdID;  // Tag 11
        public string PartyID = "";// Tag 448 Mã thành viên bán
        public string CoPartyID = "";// Tag 449 Mã thành viên mua
        public string OrderID = "";// Tag 37 Số hiệu lệnh khớp.
        public string BuyOrderID = "";// Tag 41 Số hiệu lệnh gốc bên mua
        public string SellOrderID = "";// Tag 526 Số hiệu lệnh gốc bên bán
        public int RepurchaseTerm; //Tag 226 Ky hạn repose
        public double RepurchaseRate; // Tag 227 Lai suat repos
        public string SettDate = ""; // Tag 64 ngày thanh toán.
        public string SettlDate2 = "";     //Tag 193 Ngay thanh toan lan 2
        public string EndDate = "";        //Tag 917 ngay ket thuc giao dich
        public int MatchReportType;     //Tag 5632 thông báo là khớp repos trong ngày hay thông tin repos leg2, = 1 là Thông tin lệnh khớp Repos trong ngày = 2 là thông báo thông tin Repos leg2
        public int NoSide; //Tag 552 So mã chứng khoán trong lệnh khớp

        public ReposSideListExecOrder ReposSideList;

        #endregion MyRegion

        public MessageExecOrderRepos()
            : base()
        {
            MsgType = MessageType.ExecOrderRepos;
            ReposSideList = new ReposSideListExecOrder();
            MatchReportType = 1;        //khởi tạo luôn = 1 là Thông tin lệnh khớp Repos trong ngày
        }


        [Serializable]
        public class ReposSideExecOrder
        {
            //Các thông tin này lặp lại
            public long NumSide { get; set; } = 0; // Tag 5522
            public string Symbol { get; set; } = ""; //Tag 55
            public long ExecQty { get; set; } = 0; // Tag 32
            public long ExecPx { get; set; } = 0; // Tag 31
            public long Price { get; set; } = 0; // Tag 44
            public double ReposInterest { get; set; } = 0;         //2261  
            public double HedgeRate { get; set; } = 0; // Tag 2260
            public double SettlValue { get; set; } = 0;        //6464 giá trị thanh toán
            public double SettlValue2 { get; set; } = 0;        //6465 giá trị thanh toán lan 2   
            //Ket thuc thông tin lặp lại
        }

        [Serializable]
        public class ReposSideListExecOrder
        {
            private System.Collections.ArrayList _al;

            public ReposSideExecOrder this[int i]
            {
                get
                {
                    if (_al != null && i < _al.Count)
                        return (ReposSideExecOrder)_al[i];
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
                        Add(new ReposSideExecOrder());
                    }
                }
            }

            public void Clear()
            {
                _al = null;
            }

            public void Add(ReposSideExecOrder item)
            {
                if (_al == null)
                    _al = new System.Collections.ArrayList();
                _al.Add(item);
            }
        }
    }
}