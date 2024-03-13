/* 
 * Project: FIXMessage
 * Author : Nguyen Nhat Linh – Navisoft.
 * Summary: Provide function to reader each field in fix msg string
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
    class FieldReaderFIX
    {

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        private string c_message; //xau can phan tich
        private int c_messageLen; //do dai cua xau
        private int c_fieldStart; //vi tri bat dau cua 1 truong
        private int c_fieldDelim; //vi tri cua dau phan cach truong (chinh la dau =)
        private int c_fieldEnd; //vi tri cuoi truong (vi tri cua SOH)

        public FieldReaderFIX(string message)
        {
            c_message = message.Trim(Common.SOH) + Common.SOH.ToString ();
            if (c_message != null)
                c_messageLen = c_message.Length;
        }

        public Field GetNextField()
        {
            try
            {
                if (c_fieldStart >= c_messageLen)
                    return null;

                c_fieldDelim = c_message.IndexOf(Common.DELIMIT, c_fieldStart);
                if (c_fieldDelim == -1)
                {
                    log.Error("GetNextField - Failed to find tag delimiter / Message={0} FieldStart={1}", c_message, c_fieldStart);
                    throw new Exception("Failed to find tag delimiter");
                }

                c_fieldEnd = c_message.IndexOf(Common.SOH, c_fieldDelim);
                if (c_fieldEnd == -1)
                {
                    log.Error("GetNextField - Failed to find field delimiter / Message={0} FieldDelim={1}", c_message, c_fieldDelim);
                    throw new Exception("Failed to find field terminator");
                }

                bool _IsSuccess = false;
                int _tag = Utils.Convert.ParseInt(c_message, c_fieldStart, c_fieldDelim - c_fieldStart, ref _IsSuccess );
                if (_IsSuccess == false) return new Field(0, "0");
                string _value = c_message.Substring(c_fieldDelim + 1, c_fieldEnd - c_fieldDelim - 1);

                c_fieldStart = c_fieldEnd + 1;

                return new Field(_tag, _value);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return null;
            }
        }
    }
}
