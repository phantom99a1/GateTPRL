using HNX.FIXMessage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNX.FIXMessageTests.SessionMessage
{
    [TestClass]
    public class TestOrderCrossMessage
    {
        private MessageFactoryFIX c_MsgFactory = new MessageFactoryFIX();

        [TestMethod]
        public void TestNewOrderCross()
        {
            //Arrange
            MessageNewOrderCross msgOrderCross = new MessageNewOrderCross()
            {
                Account = "003C111111",
                CoAccount = "048C111111",
                SettDate = DateTime.Now.ToString("yyyyMMdd"),
                ClOrdID = DateTime.Now.Ticks.ToString(),
                PartyID = "003",
                CoPartyID = "048",
                SenderCompID = CommonData.FirmID,
                TargetCompID = CommonData.TargetCompID,
                TargetSubID = CommonData.TargetSubID,
                Symbol = "XDCR12101",
                CrossType = 1,
                CrossID = "11111",
                ExecType = '1',
                Side = 1,
                OrderQty = 11000,
                SettlMethod = 1,
                Text = "Dat thoa thuan",
                Price2 = 135000,
                OrdType = "4",
                Price = 123000,
            };

            //Act
            string OrderCrossRaw = c_MsgFactory.Build(msgOrderCross);
            FIXMessageBase fMsg = c_MsgFactory.Parse(OrderCrossRaw);
            MessageNewOrderCross testNewOrderCross = (MessageNewOrderCross)fMsg;
            //Assert
            Assert.AreEqual(msgOrderCross.Side, testNewOrderCross.Side);
            Assert.AreEqual(msgOrderCross.Symbol, testNewOrderCross.Symbol);
            Assert.AreEqual(msgOrderCross.PartyID, testNewOrderCross.PartyID);
            Assert.AreEqual(msgOrderCross.CoPartyID, testNewOrderCross.CoPartyID);
        }

        [TestMethod]
        public void TestOrderCrossReplace()
        {
            //Arrange
            CrossOrderCancelReplaceRequest crossOrderReplace = new CrossOrderCancelReplaceRequest()
            {
                Account = "003C123456",
                CoAccount = "48C123456",
                SenderCompID = CommonData.FirmID,
                TargetCompID = CommonData.TargetCompID,
                TargetSubID = CommonData.TargetSubID,
                Side = 1,
                SettDate = DateTime.Now.ToString(),
                SettlMethod = 0,
                OrdType = "1",
                Price2 = 0,
                OrderQty = 0,
                ClOrdID = "123456",
                PartyID = CommonData.FirmID,
                CoPartyID = "048",
                CrossType = 1,
                OrgCrossID = "123256",
                SettlValue = 0,
                Symbol = "XDCR12101",
                OrderID = "HNX123455",
            };

            //Act
            string crossOrderReplaceRaw = c_MsgFactory.Build(crossOrderReplace);
            FIXMessageBase fMsg = c_MsgFactory.Parse(crossOrderReplaceRaw);
            CrossOrderCancelReplaceRequest testOrderCrossReplace = (CrossOrderCancelReplaceRequest)fMsg;
            //Assert
            Assert.AreEqual(crossOrderReplace.Side, testOrderCrossReplace.Side);
            Assert.AreEqual(crossOrderReplace.Symbol, testOrderCrossReplace.Symbol);
            Assert.AreEqual(crossOrderReplace.PartyID, testOrderCrossReplace.PartyID);
            Assert.AreEqual(crossOrderReplace.CoPartyID, testOrderCrossReplace.CoPartyID);
        }

        [TestMethod]
        public void TestCrossOrderCancel()
        {
            CrossOrderCancelRequest msgOrderCrossCancel = new CrossOrderCancelRequest()
            {
                OrderID = "1213141",
                ClOrdID = "123456",
                MsgSeqNum = 0,
                LastMsgSeqNumProcessed = 0,
                OrgCrossID = "12124141",
                CrossType = 1,
                Side = 1,
                Symbol = "XDCR12101",
                TargetCompID = CommonData.TargetCompID,
                TargetSubID = CommonData.TargetSubID,
                SenderCompID = CommonData.FirmID,
                OrdType = "1",                
                ExecType = '4'
            };
            string crossOrderCancelRaw = c_MsgFactory.Build(msgOrderCrossCancel);
            FIXMessageBase fMsg = c_MsgFactory.Parse(crossOrderCancelRaw);
            CrossOrderCancelRequest testOrderCrossCancel = (CrossOrderCancelRequest)fMsg;
            //Assert
            Assert.AreEqual(msgOrderCrossCancel.Side, testOrderCrossCancel.Side);
            Assert.AreEqual(msgOrderCrossCancel.Symbol, testOrderCrossCancel.Symbol);
            Assert.AreEqual(msgOrderCrossCancel.OrdType, testOrderCrossCancel.OrdType);
        }

    }
}
