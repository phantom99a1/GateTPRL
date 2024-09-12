/* 
 * Project: FIXMessage
 * Author : Nguyen Nhat Linh – Navisoft.
 * Summary: Define common component of fix message. Each fix message will inherit from FIXMessageBase
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 * Jul 10, 2009  	Linh.Nguyen     Created
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HNX.FIXMessage
{
    /// <summary>
    /// Summary description for Message.
    /// </summary>
    [Serializable]
    public class FIXMessageBase
    {
        public FIXMessageBase()
        {
            MsgType = "";
            RefMsgType = "";
            MessageRaw = "";
            SenderCompID = "";
            TargetCompID = "";
            TargetSubID = "";
            MsgSeqNum = 0;
            LastMsgSeqNumProcessed = 0;
            Text = "";
        }

        public FIXMessageBase(string msgType)
        {
            MsgType = msgType;
        }

        #region Tags #'s
        //tag for header
        public const int TAG_BeginString = 8;
        public const int TAG_BodyLength = 9;
        public const int TAG_MsgType = 35;
        public const int TAG_SenderCompID = 49;
        public const int TAG_TargetCompID = 56;
        //public const int TAG_TargetSubID = 57;
        public const int TAG_MsgSeqNum = 34;
        public const int TAG_SendingTime = 52;
        public const int TAG_LastMsgSeqNumProcessed = 369;//  
        public const int TAG_PossDupFlag = 43;//   bao day la message gui lai

        public const int TAG_Text = 58;
        public const int TAG_ExecType = 150;//  phan loai message report tra ve
        public const int TAG_RefMsgType = 372;


        //tag for Trailer
        public const int TAG_Signature = 89;
        public const int TAG_CheckSum = 10;

        #endregion

        #region Message Varialbes
        internal string MessageRaw;
        public string GetMessageRaw
        {
            get { return MessageRaw; }
        }

        internal string MessageRawEncrypt;
        public string GetMessageRawEncrypt
        {
            get { return MessageRawEncrypt; }
        }

        /// <summary>
        /// Lấy giá trị tag 35
        /// </summary>
        public string GetMsgType
        {
            get { return MsgType; }
        }

        /// <summary>
        /// Lấy giá trị tag 52
        /// </summary>
        public DateTime GetSendingTime
        {
            get { return SendingTime; }
        }

        /// <summary>
        /// Lấy giá trị tag 49
        /// </summary>
        public string GetSenderCompID
        {
            get { return SenderCompID; }
        }


        /// <summary>
        /// Lấy giá trị tag 56
        /// </summary>
        public string GetTargetCompID
        {
            get { return TargetCompID; }
        }



        public byte[] MessageRawByte;
        //
        public string ApiOrderNo { get; set; }

        public string BeginString = Common.VERSION;
        internal int BodyLength;
        internal string MsgType;
        public string SenderCompID;
        public string TargetCompID;
        public string TargetSubID;

        /// <summary>
        /// Lấy giá trị tag 34
        /// </summary>
        public int MsgSeqNum;

        /// <summary>
        /// Lấy giá trị tag 369
        /// </summary>
        public int LastMsgSeqNumProcessed;

        public Boolean PossDupFlag;//  message gui la public hay private.
        //Neu =Y la message public, khi do MsgSeqNum la sequency cua cac goi tin public
        // con lai thi MsgSeqNum la message cua goi tin private

        //2024.09.09 Duat them get, set cho RefMgType 
        public string RefMsgType { get; set; } = string.Empty;

        internal DateTime SendingTime ;
        public string Signature;
        internal byte CheckSum;
        /// <summary>
        /// 2024.09.09 Duat bo sung them phan get set cho tham so Text
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Tag 150
        /// </summary>
        public char ExecType;
        //
        internal List<Field> Fields = new List<Field>();

        //
        public string APIBussiness { get; set; } = "";
        public int ApiSeqNum { get; set; } = -1;

        /// <summary>
        /// ID xử lý cho đầu nhận Api
        /// </summary>
        public string IDRequest { get; set; } = "";

        public long TimeInit;

        public string TimeRecv { get; set; }
        #endregion
    }
}
