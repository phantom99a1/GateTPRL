/* 
 * Project: FIXMessage
 * Author : Nguyen Nhat Linh – Navisoft.
 * Summary: Detail Define for Heartbeat message
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 * Jul 10, 2009  	Linh.Nguyen     Created
 */
using System;
using System.Reflection;

namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageSecurityStatus : FIXMessageBase
    {
      public  const int TAG_SecurityStatusReqID = 324;
        public const int TAG_Symbol = 55;
        public const int TAG_SecurityType = 167;
        public const int TAG_MaturityDate = 541;
        public const int TAG_IssueDate = 225;

        public const int TAG_Issuer = 106;
        public const int TAG_SecurityDesc = 107;

        public const int TAG_HighPx = 332;
        public const int TAG_LowPx = 333;
        public const int TAG_HighPxOut = 3321;
        public const int TAG_LowPxOut = 3331;
        public const int TAG_HighPxRep = 3322;
        public const int TAG_LowPxRep = 3332;

        public const int TAG_TotalListingQtty = 109;
        public const int TAG_LastPx = 31;
        public const int TAG_SecurityTradingStatus = 326;
        public const int TAG_BuyVolume = 330;
        public const int TAG_TypeRule = 6251;
        public const int TAG_TradingSessionSubID = 625;
        public const int TAG_DateNo = 265;
        public const int TAG_Allowed_Trading_Subject = 9735;
        public const int TAG_Allowed_Trading_Subject_Sell = 9736;
        public MessageSecurityStatus()
            : base()
        {
            MsgType = MessageType.SecurityStatus;
        }

        #region Member

        public string SecurityStatusReqID { get; set; } //324
        public string Symbol { get; set; } //324
        public string DateNo { get; set; } //324

        public string SecurityType { get; set; }             //167
        public DateTime MaturityDate { get; set; }           //541
        public DateTime IssueDate { get; set; }              //225
        public string Issuer { get; set; }                    //106
        public string SecurityDesc { get; set; }              //107
        public Int64 HighPx { get; set; }                   // 332	Giá trần normal
        public Int64 LowPx { get; set; }                    //333  Giá Sàn normal

        public Int64 HighPxOut { get; set; }                   // 3321	Giá trần Outright
        public Int64 LowPxOut { get; set; }                    //3331  Giá Sàn Outright

        public Int64 HighPxRep { get; set; }                   // 3322	Giá trần Repos
        public Int64 LowPxRep { get; set; }                    //3332  Giá Sàn Repos


        public Int64 LastPx { get; set; }                   //31    Giá tham chiếu
        public int SecurityTradingStatus { get; set; }   //326 trang thai gd của CK --> là trường status trong bảng cb_symbol
        public Int64 BuyVolume { get; set; }            //330 Kl room
        public Int64 TotalListingQtty { get; set; }

        public string TradingSessionSubID { get; set; }          //625  //Mã bảng TP thuộc

        public int TypeRule { get; set; }            //CK ăn theo phiên chung hay riêng = 1 là phiên chung, = 2 là phiên riêng

        public string Allowed_Trading_Subject { get; set; }
        public string Allowed_Trading_Subject_Sell { get; set; }

        #endregion

    }
}