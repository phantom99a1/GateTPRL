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
    public class MessageReposFirm : FIXMessageBase
    {
        #region fields

        public string ClOrdID ; // Tag 11
        public string RFQReqID ; //Tag 644
        public int QuoteType; // tag 537
        public string OrdType ; // Tag 40
        public int Side;   // Tag 54: 1 = Buy, 2 = Sell
        public string Account ; // Tag 1
        public string EffectiveTime ; // Tag 168
        public int SettlMethod;    // Tag 6363
        public string SettlDate ; // Tag 64 ngày thanh toán.
        public string SettlDate2 ; //Tag 193
        public string EndDate ; //Tag 917
        public int RepurchaseTerm; //Tag 226
        public double RepurchaseRate; //Tag 227
        public long NoSide; // Tag 552
                                            
        public ReposSideList RepoSideList;

        #endregion fields

        public MessageReposFirm()
            : base()
        {
            MsgType = MessageType.ReposFirm;
            RepoSideList = new ReposSideList();
        }
    }

    [Serializable]
    public class ReposSide
    {
        //Các thông tin này lặp lại
        public long NumSide { get; set; } = 0; // Tag 5522
        public string Symbol { get; set; } = ""; //Tag 55
        public long OrderQty { get; set; } = 0; // Tag 38
        public long Price { get; set; } = 0; // Tag 44
        public double HedgeRate { get; set; } = 0; // Tag 2260
        public double SettlValue { get; set; } = 0;        //6464 giá trị thanh toán
        public double SettlValue2 { get; set; } = 0;        //6465 giá trị thanh toán lan 2   
        public double ReposInterest { get; set; } = 0;         //2261  Loai repose
        public long ExecPrice { get; set; } = 0;         // 640

        //Ket thuc thông tin lặp lại
    }

    [Serializable]
    public class ReposSideList
    {
        private System.Collections.ArrayList _al;

        public ReposSide this[int i]
        {
            get
            {
                if (_al != null && i < _al.Count)
                    return (ReposSide)_al[i];
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
                    Add(new ReposSide());
                }
            }
        }

        public void Clear()
        {
            _al = null;
        }

        public void Add(ReposSide item)
        {
            if (_al == null)
                _al = new System.Collections.ArrayList();
            _al.Add(item);
        }
    }
}