using Microsoft.VisualStudio.TestTools.UnitTesting;
using HNX.FIXMessage.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNX.FIXMessage.Utils.Tests
{
    [TestClass()]
    public class ConvertTests
    {
        [TestMethod()]
        [DataRow ("1", true)]
        [DataRow ("1s", false )]
        public void ParseIntTest(string strInt, bool exp)
        {
            bool _IsSuccess =false;
            var _return = Convert.ParseInt(strInt, ref _IsSuccess);
            Assert.AreEqual (exp, _IsSuccess);
        }

        [TestMethod()]
        [DataRow("12", true)]
        [DataRow("1s", false)]
        public void ParseIntTest2(string strInt, bool exp)
        {
            bool _IsSuccess = false;
            var _return = Convert.ParseInt(strInt, 0,2,ref _IsSuccess);
            Assert.AreEqual(exp, _IsSuccess);
        }
        [TestMethod()]
        [DataRow("1", true)]
        [DataRow("1s", false)]
        public void ParseInt64Test(string strInt, bool exp)
        {
            bool _IsSuccess = false;
            var _return = Convert.ParseInt64(strInt, ref _IsSuccess);
            Assert.AreEqual(exp, _IsSuccess);
        }

        [TestMethod()]
        [DataRow("1", true)]
        [DataRow("1s", false)]
        public void ParseLongTest(string strInt, bool exp)
        {
            bool _IsSuccess = false;
            var _return = Convert.ParseLong(strInt, ref _IsSuccess);
            Assert.AreEqual(exp, _IsSuccess);
        }

        [TestMethod()]
        [DataRow("12", true)]
        [DataRow("1s", false)]
        public void ParseLongTest2(string strInt, bool exp)
        {
            bool _IsSuccess = false;
            var _return = Convert.ParseLong(strInt, 0, 2, ref _IsSuccess);
            Assert.AreEqual(exp, _IsSuccess);
        }

        [TestMethod()]
        [DataRow("1", true)]
        [DataRow("1s", false)]
        public void ParseDoubleTest(string strInt, bool exp)
        {
            bool _IsSuccess = false;
            var _return = Convert.ParseDouble(strInt, ref _IsSuccess);
            Assert.AreEqual(exp, _IsSuccess);
        }


        [TestMethod()]
        [DataRow("1", true)]
        [DataRow("1s", false)]
        public void ParseDecimalTest(string strInt, bool exp)
        {
            bool _IsSuccess = false;
            var _return = Convert.ParseDecimal(strInt, ref _IsSuccess);
            Assert.AreEqual(exp, _IsSuccess);
        }

        


        [TestMethod()]
        [DataRow("12", 2)]
        [DataRow("1s", 67)]
        public void ParseByteTest2(string strInt, int exp)
        {
            var _return = Convert.ParseByte(strInt, 1, 1 );
            Assert.AreEqual(exp, _return);
        }



        [TestMethod()]
        public void FromFIXUTCDateTest( )
        {
            string _date = "20320505";
            var _return = Convert.FromFIXUTCDate(_date );
            Assert.IsTrue(_return != DateTime.MinValue);
        }

        [TestMethod()]
        public void FromFIXUTCTimestampTest_cat()
        {
            string _date = "20322505";
            var _return = Convert.FromFIXUTCTimestamp(_date);
            Assert.IsTrue(_return == DateTime.MinValue);
        }

        [TestMethod()]
        [DataRow(true, 'Y')]
        [DataRow(false , 'N')]
        public void FromFIXBooleanTest(bool  p_input, char exp)
        {
            var _return = Convert.ToFIXBoolean(p_input);
            Assert.AreEqual(exp, _return);
        }


        [TestMethod()]
        [DataRow( "Y", true)]
        [DataRow( "", false )]
        [DataRow( "N", false )]
        public void FromFIXBooleanTest2(string p_input, bool  exp)
        {
            var _return = Convert.FromFIXBoolean(p_input);
            Assert.AreEqual(exp, _return);
        }
    }
}