using HNX.FIXMessage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HNX.FIXMessageTests.BusinessMessage
{
    [TestClass]
    public class NormalMessageTest
    {
        private MessageFactoryFIX c_MsgFactory = new MessageFactoryFIX();

        [TestMethod]
        public void MessageCancelOrder_Test()
        {
            //Arrange
            MessageCancelOrder message = new MessageCancelOrder()
            {
                SenderCompID = "003",
                TargetCompID = "HNX",
                TargetSubID = "HNX1",
                //
                ClOrdID = "XDR123",
                OrigClOrdID = "Test_1",
                Symbol = "1"
            };
            string s = c_MsgFactory.Build(message);
            MessageCancelOrder message_1 = (MessageCancelOrder)c_MsgFactory.Parse(s);
            //Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void MessageNewOrder_Test()
        {
            //Arrange
            MessageNewOrder message = new MessageNewOrder()
            {
                SenderCompID = "003",
                TargetCompID = "HNX",
                TargetSubID = "HNX1",
                //
                ClOrdID = "XDR123",
                Account = "1",
                Symbol = "1",
                Side = 1,
                OrdType = "1",
                OrderQty = 1,
                OrderQty2 = 1,
                Price = 1,
                Price2 = 1,
                SpecialType = 1
            };
            string s = c_MsgFactory.Build(message);
            MessageNewOrder message_1 = (MessageNewOrder)c_MsgFactory.Parse(s);
            //Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void MessageReplaceOrder_Test()
        {
            //Arrange
            MessageReplaceOrder message = new MessageReplaceOrder()
            {
                SenderCompID = "003",
                TargetCompID = "HNX",
                TargetSubID = "HNX1",
                //
                ClOrdID = "XDR123",
                OrigClOrdID = "XDR123",
                Account = "1",
                Symbol = "1",
                OrderQty = 1,
                OrgOrderQty = 1,
                Price2 = 1
            };
            string s = c_MsgFactory.Build(message);
            MessageReplaceOrder message_1 = (MessageReplaceOrder)c_MsgFactory.Parse(s);
            //Assert
            Assert.IsTrue(true);
        }
    }
}