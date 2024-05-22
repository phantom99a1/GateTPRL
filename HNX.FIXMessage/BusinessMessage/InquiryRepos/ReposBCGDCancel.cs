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
    public class MessageReposBCGDCancel : FIXMessageBase
    {
        #region fields
        public string ClOrdID; // Tag 11
        public string OrgOrderID; // Tag 198
        public int QuoteType; // Tag 563
        public string OrdType; // Tag 40
        public int Side;   // Tag 54: 1 = Buy, 2 = Sell

        public string OrderNo { get; set; } = ""; // Dùng để lưu OrderNo khi push queue save db

        #endregion fields

        public MessageReposBCGDCancel()
            : base()
        {
            MsgType = MessageType.ReposBCGDCancel;
        }
    }
}
