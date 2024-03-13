using HNX.FIXMessage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HNX.FIXMessage.Utils;
using static System.Net.Mime.MediaTypeNames;

namespace HNX.FIXMessageTests.TestFunction
{
    [TestClass]
    public class TestCommonFunc
    {
        MessageFactoryFIX c_MsgFactoryFIX = new MessageFactoryFIX();
        [TestMethod]        
        public void TestParseMsgSeqNum()
        {
            string s = "8=FIX.4.4\u00019=197\u000135=AI\u000149=HNX\u000156=003.01GW\u000134=1\u0001369=1\u000152=20230920-08:21:42\u000111=111\u00014488=003\u0001537=1\u00011=003C111111\u000155=XDCR12101\u000154=1\u000140=S\u000138=5\u0001131=\u0001171=XDCR12101-2308280000002\u0001640=1000\u00016464=5000\u00016363=1\u000164=20230828\u0001513=0\u000110=000\u0001";
            int seq = c_MsgFactoryFIX.ParseMsgSeqNum(s);
            Assert.AreEqual(seq, 1);
           
            
        }

        [TestMethod]
        public void TestParseMsgType()
        {
            string s = "8=FIX.4.4\u00019=197\u000135=AI\u000149=HNX\u000156=003.01GW\u000134=1\u0001369=1\u000152=20230920-08:21:42\u000111=111\u00014488=003\u0001537=1\u00011=003C111111\u000155=XDCR12101\u000154=1\u000140=S\u000138=5\u0001131=\u0001171=XDCR12101-2308280000002\u0001640=1000\u00016464=5000\u00016363=1\u000164=20230828\u0001513=0\u000110=000\u0001";
            string msgtype, execType;
            (msgtype, execType) = c_MsgFactoryFIX.Parse_MsgType_ExecType(s);
            Assert.AreEqual(msgtype, "AI");
        }
        [TestMethod]
        public void TestParseMessageException()
        {
            string s = "8=FIX.4.4\u00019=197\u000135=AI\u000149=HNX\u000156=003.01GW\u000134=1\u0001369=1\u000152=20230920-08:21:42\u000111=111\u00014488=003\u0001537=1\u00011=003C111111\u000155=XDCR12101\u000154=1\u000140=S\u000138=5\u0001131=\u0001171=XDCR12101-2308280000002\u0001640=1000\u00016464=50sad00\u00016363=1\u000164=202308sc28\u0001513=0\u000110=000\u0001";
            FIXMessageBase msgBase = c_MsgFactoryFIX.Parse(s);    
            Assert.AreEqual(msgBase, null);
        }

        [TestMethod]
        public void TestParseException()
        {
            string s = "8=FIX.4.4\u00019=197\u000135=AI\u000149=HNX\u000156=003.01GW\u000134=1\u0001369=1\u000152=20230920-08:21:42\u000111=111\u00014488=003\u0001537=1\u00011=003C111111\u000155=XDCR12101\u000154=1\u000140=S\u000138=5\u0001131=\u0001171=XDCR12101-2308280000002\u0001640=1000\u00016464=50sad00\u00016363=1\u000164=202308sc28\u0001513=0\u000110=000\u0001";
            ParseFieldException e = new ParseFieldException("Wrong Field", new Exception())
            {
                RefSeqNum = 1,
                Text = s
            };

            Assert.AreEqual(e.RefSeqNum, 1);
            
        }
    }
}
