/* 
 * Project: FIXMessage
 * Author : Nguyen Nhat Linh – Navisoft.
 * Summary: Detail Define for NewOrder message
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 * Jul 10, 2009  	Linh.Nguyen     Created
 */

using System;

namespace HNX.FIXMessage
{
    [Serializable]
    public class CrossOrderCancelRequest  : FIXMessageBase
    {

        #region fields 
        //public DateTime TransactTime;  //60: thoi gian dat lenh
        public int CrossType; //549: hinh thuc thoa thuan
        public string OrdType;              // 40: loai lenh: 1=Market price,2=ato,3=atc
        public string OrgCrossID; //551 SHL hủy 
        public string Symbol; // 55: Mã ck
        public int Side;    // Tag Xem bên nào là bên chủ động Hủy lệnh
        public string ClOrdID; // Tag 11
        public string OrderID; // Tag 37 SHL sửa HNX sinh ra
        public string OrderNo { get; set; } = ""; // Dùng để lưu OrderNo khi push queue save db

        #endregion

        public CrossOrderCancelRequest ()
            : base()
        {
            MsgType = MessageType.CrossOrderCancelRequest;

        }

    }


}