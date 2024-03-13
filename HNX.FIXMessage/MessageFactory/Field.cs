/* 
 * Project: FIXMessage
 * Author : Nguyen Nhat Linh – Navisoft.
 * Summary: Define field of fix message
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
    [Serializable]
    public class Field
    {
        public int Tag;
        public string Value;

        public Field() { }

        public Field(int tag, string value)
        {
            Tag = tag;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}{1}{2}", Tag, Common.DELIMIT, Value);
        }

    }
}
