/* 
 * Project: FIXMessage
 * Author : Nguyen Nhat Linh – Navisoft.
 * Summary: Define exception for parse fields of fix message
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
   public  class ParseFieldException  : Exception
    {
        protected int _iRefSeqNum;
        protected string _sText;

        public ParseFieldException(string sMessage, Exception exInner)
            : base(sMessage, exInner)
        {
        }

        public int RefSeqNum
        {
            get { return _iRefSeqNum; }
            set { _iRefSeqNum = value; }
        }

        public string Text
        {
            get { return _sText; }
            set { _sText = value; }
        }
    }
}
