/* 
 * Project: FIXMessage
 * Author : Nguyen Nhat Linh – Navisoft.
 * Summary: Utility function for parse message fix
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 * Jul 10, 2009  	Linh.Nguyen     Created
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HNX.FIXMessage.Utils

{
    public class Convert
    {
        /// <summary>
        /// Parses a int32 from a string.
        /// </summary>
        /// <param name="s">string that gets parsed</param>
        /// <returns>int value of string</returns>
        public static int ParseInt(string s, ref bool IsSuccess)
        {
            try
            {
                IsSuccess = true;
                return int.Parse(s);
            }
            catch
            {
                IsSuccess = false;
                return int.MinValue;
            }
        }

        /// <summary>
        /// Parses a int32 from a string.
        /// </summary>
        public static int ParseInt(string s, int iIndex, int iLen, ref bool IsSuccess)
        {
            try
            {
                string _sub = s.Substring(iIndex, iLen);
                IsSuccess = true;
                return int.Parse(_sub);
            }
            catch
            {
                IsSuccess = false;
                return int.MinValue;
            }

            //int iStop = iIndex + iLen;
            //int iValue = 0;

            //for (int i = iIndex; i < iStop; i++)
            //    iValue = (iValue * 10) + (s[i] - 48);

            //return iValue;
        }

        public static long ParseLong(string s, ref bool IsSuccess)
        {
            try
            {
                IsSuccess = true;
                return long.Parse(s);
            }
            catch
            {
                IsSuccess = false;
                return long.MinValue;
            }
        }

        public static long ParseLong(string s, int iIndex, int iLen, ref bool IsSuccess)
        {
            try
            {
                string _sub = s.Substring(iIndex, iLen);
                IsSuccess = true;
                return long.Parse(_sub);
            }
            catch
            {
                IsSuccess = false;
                return long.MinValue;
            }
        }
        /// <summary>
        /// Parses a int16 from a string.
        /// </summary>
        public static byte ParseByte(string s, int iIndex, int iLen)
        {
            int iStop = iIndex + iLen;
            byte iValue = 0;

            for (int i = iIndex; i < iStop; i++)
                iValue = (byte)((iValue * 10) + (s[i] - 48));

            return iValue;
        }


        /// <summary>
        /// Parses a int16 from a string.
        /// </summary>
        public static double ParseDouble(string s, ref bool IsSuccess)
        {
            try
            {
                IsSuccess = true;
                return double.Parse(s);
            }
            catch
            {
                IsSuccess = false;
                return double.MinValue;
            }
        }
        public static decimal ParseDecimal(string s, ref bool IsSuccess)
        {
            try
            {
                IsSuccess = true;
                return decimal.Parse(s);
            }
            catch
            {
                IsSuccess = false;
                return decimal.MinValue;
            }
        }

        public static DateTime FromShortDate(string s)
        {
            DateTime _result = DateTime.MinValue;
            DateTime.TryParseExact(s, Common.FORMAT_DATE, Common.CULTURE_PROVIDER, System.Globalization.DateTimeStyles.AssumeLocal, out _result);
            return _result;
        }

        public static DateTime FromFIXUTCDate(string s)
        {
            DateTime _result = DateTime.MinValue;

            DateTime.TryParseExact(s, Common.FORMAT_DATE, Common.CULTURE_PROVIDER, System.Globalization.DateTimeStyles.AssumeLocal, out _result);

            return _result;
        }

        /// <summary>
        /// Converts from a DateTime object to the standard FIX UTC Timestamp format.
        /// </summary>
        public static string ToFIXUTCTimestamp(DateTime dt)
        {

            return dt.ToUniversalTime().ToString(Common.FORMAT_DATETIME, Common.CULTURE_PROVIDER);
        }

        /// <summary>
        /// Converts from the standard FIX UTC Timestamp to a DateTime object.
        /// </summary>
        public static DateTime FromFIXUTCTimestamp(string s)
        {
            try
            {
                //  IsSuccess = true;
                return DateTime.ParseExact(s, Common.FORMAT_DATETIME, Common.CULTURE_PROVIDER).ToLocalTime();
                // return DateTime.Parse(s, Common.FORMAT_DATETIME, Common.CULTURE_PROVIDER).ToUniversalTime();
            }
            catch
            {
                // IsSuccess = false;
                return DateTime.MinValue;
            }
        }
        public static string ToShortDate(DateTime dt)
        {
            //return dt.ToString(Common.FORMAT_DATE, Common.CULTURE_PROVIDER);
            return dt.ToString(Common.FORMAT_DATE, Common.CULTURE_PROVIDER);
        }

        /// <summary>
        /// Converts from a Boolean to the standard FIX format.
        /// </summary>
        public static char ToFIXBoolean(bool b)
        {
            if (b)
                return 'Y';
            else
                return 'N';
        }

        /// <summary>
        /// Converts from the standard FIX to a Boolean.
        /// </summary>
        public static bool FromFIXBoolean(char c)
        {
            return 'Y' == c;
        }

        /// <summary>
        /// Converts from the standard FIX to a Boolean.
        /// </summary>
        public static bool FromFIXBoolean(string s)
        {
            if (s == null || s.Length == 0)
                return false;
            else
                return 'Y' == s[0];
        }
        /// <summary>
        /// Author: SangVV
        /// Ham thuc hien chuyen chuoi Msg thanh chuoi binh thuong (Replace SOH thanh ;)
        /// </summary>
        /// <param name="pv_strMsg"> La chuoi gia tri cua FIX Msg</param>

        public static Int64 ParseInt64(string s, ref bool IsSuccess)
        {
            try
            {
                IsSuccess = true;
                Int64 x = System.Convert.ToInt64(s);
                return System.Convert.ToInt64(x);
            }
            catch
            {
                IsSuccess = false;
                return Int64.MinValue;
            }
        }
    }
}
