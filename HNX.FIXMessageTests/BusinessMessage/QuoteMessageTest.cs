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
    public class QuoteMessageTest
    {
        private MessageFactoryFIX c_MsgFactory = new MessageFactoryFIX();

        [TestMethod]
        public void TestQuoteMessage()
        {
            //Arrage
            MessageQuote msgQuote = new MessageQuote()
            {
                Account = "003C111111",
                ClOrdID = "123456",
                OrderQty = 1000,
                OrdType = "1",
                Price2 = 1351000,
                SenderCompID = CommonData.FirmID,
                TargetCompID = CommonData.TargetCompID,
                TargetSubID = CommonData.TargetCompID,
                ExecType = '1',
                RegistID = "1",
                SettlValue = 10000.0,
                SettDate = DateTime.Now.ToString("yyyyMMdd"),
                SettlMethod = 2,
                Side = 1,
                Price = 10000,
                Text = "Dat TTDT",
                Symbol = "XDCR12101",
            };
            //Act
            string msgQuoteRaw = c_MsgFactory.Build(msgQuote);

            FIXMessageBase fMsgBase = c_MsgFactory.Parse(msgQuoteRaw);

            //Assert
            MessageQuote testQuote = (MessageQuote)fMsgBase;
            Assert.AreEqual(msgQuote.Symbol, testQuote.Symbol);
            Assert.AreEqual(msgQuote.Price2, testQuote.Price2);
            Assert.AreEqual(msgQuote.SettDate, testQuote.SettDate);
        }


        [TestMethod]
        public void TestMsgQuoteReport()
        {
            //Arrange
            MessageQuoteSatusReport msgQuoteReport = new MessageQuoteSatusReport()
            {
                Account = "003C123456",
                ClOrdID = "12345678",
                OrderPartyID = "048",
                SenderCompID = CommonData.FirmID,
                TargetCompID = CommonData.TargetCompID,
                TargetSubID = CommonData.TargetSubID,
                ExecType = '1',
                OrderQty = 1000,
                OrdType = "2",
                QuoteID = "quote12345",
                Price2 = 123000,
                Price = 123000,
                QuoteType = 2,
                RegistID = "12341",
                SettDate = DateTime.Now.ToString("yyyyMMdd"),
                SettlMethod = 3,
                Side = 2,
                Symbol = "XDCR12101",
                Text = "Chap nhan TTDT"
            };
            //Act 
            string strQuoteStt = c_MsgFactory.Build(msgQuoteReport);
            FIXMessageBase fMsg = c_MsgFactory.Parse(strQuoteStt);
            MessageQuoteSatusReport testQuoteSttReport = (MessageQuoteSatusReport)fMsg;
            //Assert
            Assert.AreEqual(msgQuoteReport.Side, testQuoteSttReport.Side);
            Assert.AreEqual(msgQuoteReport.Symbol, testQuoteSttReport.Symbol);
            Assert.AreEqual(msgQuoteReport.Price2, testQuoteSttReport.Price2);
        }
        [TestMethod]
        public void TestMsgQuoteResponse()
        {
            MessageQuoteResponse msgQuoteResponse = new MessageQuoteResponse()
            {
                Account = "003C111111",
                ClOrdID = "123456",
                ExecType = '1',
                OrderQty = 1000,
                OrdType = "1",
                Price2 = 100000,
                QuoteRespID = "1",
                SettDate = DateTime.Now.ToString(),
                SettlMethod = 1,
                Side = 2,
                SettlValue = 0,
                Price = 100000,
                Symbol = "XDCR12101",
                QuoteRespType = 1,
                SenderCompID = CommonData.FirmID,
                TargetCompID = CommonData.TargetCompID,
                TargetSubID = CommonData.TargetSubID,
            };
            string strQuoteResponse = c_MsgFactory.Build(msgQuoteResponse);
            FIXMessageBase fMsg = c_MsgFactory.Parse(strQuoteResponse);
            MessageQuoteResponse testQuoteResponse = (MessageQuoteResponse)fMsg;
            //Assert
            Assert.AreEqual(msgQuoteResponse.Side, testQuoteResponse.Side);
            Assert.AreEqual(msgQuoteResponse.Symbol, testQuoteResponse.Symbol);
            Assert.AreEqual(msgQuoteResponse.Price2, testQuoteResponse.Price2);

        }

        [TestMethod]
        public void TestQuoteCancel()
        {
            MessageQuoteCancel msgQuoteCancel = new MessageQuoteCancel()
            {
                ClOrdID = "123456",
                ExecType = '4',
                MsgSeqNum = 1,
                LastMsgSeqNumProcessed = 1,
                OrdType = "4",
                QuoteID = "12124212",
                Symbol = "XDCR12101",
                QuoteCancelType = 1,
                TargetCompID = CommonData.TargetSubID,
                TargetSubID = CommonData.TargetSubID,
                SenderCompID = CommonData.FirmID,
            };
            string strQuoteCancel = c_MsgFactory.Build(msgQuoteCancel);
            FIXMessageBase fMsg = c_MsgFactory.Parse(strQuoteCancel);
            MessageQuoteCancel testQuoteCancel = (MessageQuoteCancel)fMsg;
            //Assert
            Assert.AreEqual(msgQuoteCancel.OrdType, testQuoteCancel.OrdType);
            Assert.AreEqual(msgQuoteCancel.Symbol, testQuoteCancel.Symbol);
            Assert.AreEqual(msgQuoteCancel.QuoteID, testQuoteCancel.QuoteID);


        }

        [TestMethod]
        public void TestQuoteRequest()
        {
            //Arrange
            MessageQuoteRequest msgQuoteRequest = new MessageQuoteRequest()
            {
                OrderID = "123125414",
                OrdType = "1",
                Side = 1,
                Symbol = "XDCR12101",
                SettlValue = 0,
                SettDate = DateTime.Now.ToString(),
                Account = "003C132156",
                ClOrdID = "135463311",
                ExecType = '5',
                OrderQty = 10000,
                Price = 1200000,
                Price2 = 120000,
                RFQReqID = "3165461",
                RegistID = "003C1321645",
                SettlMethod = 1,
                SenderCompID = CommonData.FirmID,
                TargetCompID = "HNX",
                TargetSubID = CommonData.TargetSubID,
                MsgSeqNum = 0,
                TransactTime = DateTime.Now,
                
            };
            //Act
            string strQuoteCancel = c_MsgFactory.Build(msgQuoteRequest);
            FIXMessageBase fMsg = c_MsgFactory.Parse(strQuoteCancel);
            MessageQuoteRequest testQuoteRequest = (MessageQuoteRequest)fMsg;

            //Assert
            Assert.AreEqual(msgQuoteRequest.OrdType, testQuoteRequest.OrdType);
            Assert.AreEqual(msgQuoteRequest.Symbol, testQuoteRequest.Symbol);

        }

       
    }
}
