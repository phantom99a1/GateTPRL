/* 
 * Project: InternalMsg 
 * Summary: Detail Define for QouteCancel message
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 * Mar 28, 2022  	root     Created
 */

using System;

namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageQuoteCancel : FIXMessageBase
    {

        #region fields
        public string QuoteID; //SHL lênh quote muôn cancel
        public int QuoteCancelType;
        public string Symbol; //mã TP
        public string OrdType;              // 40: loai lenh: 1=Market price,2=ato,3=atc

        public string ClOrdID;
        #endregion

        public MessageQuoteCancel()
            : base()
        {
            MsgType = MessageType.QuoteCancel;
        }

    }
}
