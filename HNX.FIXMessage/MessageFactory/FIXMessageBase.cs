﻿/* 
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

        public string GetMsgType
        {
            get { return MsgType; }
        }

        public DateTime GetSendingTime
        {
            get { return SendingTime; }
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
        public int MsgSeqNum;
        public int LastMsgSeqNumProcessed;

        public Boolean PossDupFlag;//  message gui la public hay private.
        //Neu =Y la message public, khi do MsgSeqNum la sequency cua cac goi tin public
        // con lai thi MsgSeqNum la message cua goi tin private
        public string RefMsgType;

        internal DateTime SendingTime;
        public string Signature;
        internal byte CheckSum;
        public string Text = string.Empty;

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
        #endregion
    }
}
