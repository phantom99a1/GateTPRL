using Microsoft.VisualStudio.TestTools.UnitTesting;
using HNX.FIXMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNX.FIXMessage.Tests
{
    [TestClass()]
    public class MessageFactoryFIXTests
    {
        MessageFactoryFIX c_MessageFactoryFIX = new MessageFactoryFIX();


        [TestMethod()]
         
        public void ParseTest_null( )
        {
            string _msg = "8=FIX.4.4\u00019=259\u000135=8\u000149=HNX\u00010=018.01GW\u000134=44\u0001369=0\u000152=20230811-09:34:22\u0001150=3\u000139=2\u000111=8c34db4d-aa2c-4eb0-b64a-46202199d9f6\u000141=AAAR42202-2307170000002\u0001526=AAAR42202-2307170000002\u000137=AAAR42202-2307170000002\u000132=8\u000131=5000\u000117=AAAR42202-2307170000002\u000155=AAAR42202\u000154=2\u00016464=40000\u000110=018\u0001";
         var _return=   c_MessageFactoryFIX.Parse(_msg);

            Assert.IsTrue (_return ==null);
        }

        [TestMethod()]

        public void ParseTest_Cat()
        {
            var _return = c_MessageFactoryFIX.Parse(null);

            Assert.IsTrue(_return == null);
        }


        [TestMethod()]

        public void ParseTest_NotMsgtype()
        {
            string _msg = "8=FIX.4.4\u00019=\u000135=88";
            var _return = c_MessageFactoryFIX.Parse(_msg);

            Assert.IsTrue(_return == null);
        }


        [TestMethod()]

        public void ParseTest_MsgtypeNull()
        {
            string _msg = "8=FIX.4.4\u00019=\u000135=";
            var _return = c_MessageFactoryFIX.Parse(_msg);

            Assert.IsTrue(_return == null);
        }
    }
}