using HNX.FIXMessage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static HNX.FIXMessage.MessageExecOrderRepos;
using static HNX.FIXMessage.MessageReposBCGDReport;

namespace HNX.FIXMessageTests.SessionMessage
{
    [TestClass]
    public class InquiryMessageTest
    {
        private MessageFactoryFIX c_MsgFactory = new MessageFactoryFIX();

        [TestMethod]
        public void MessageReposBCGD_Test()
        {
            //Arrange
            MessageReposBCGD message = new MessageReposBCGD()
            {
                SenderCompID = "003",
                TargetCompID = "HNX",
                TargetSubID = "HNX1",
                //
                ClOrdID = "XDR123",
                OrgOrderID = "Test_1",
                QuoteType = 1,
                OrdType = "1",
                Side = 1,
                Account = "1",
                CoAccount = "1",
                PartyID = "",
                CoPartyID = "",
                EffectiveTime = "20231109",
                SettlMethod = 3,
                SettlDate = "20231116",
                SettlDate2 = "20231117",
                EndDate = "20231117",
                RepurchaseTerm = 2,
                NoSide = 1,
            };
            ReposSide _reposSide = new ReposSide();
            _reposSide.NumSide = 1;
            _reposSide.Symbol = "1";
            _reposSide.OrderQty = 100;
            _reposSide.Price = 1000;
            _reposSide.HedgeRate = 1;
            //
            message.RepoSideList.Add(_reposSide);

            //Act
            string s = c_MsgFactory.Build(message);
            MessageReposBCGD message_1 = (MessageReposBCGD)c_MsgFactory.Parse(s);
            //Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void MessageReposBCGDCancel_Test()
        {
            //Arrange
            MessageReposBCGDCancel message = new MessageReposBCGDCancel()
            {
                SenderCompID = "003",
                TargetCompID = "HNX",
                TargetSubID = "HNX1",
                //
                ClOrdID = "XDR123",
                OrgOrderID = "Test_1",
                QuoteType = 1,
                OrdType = "1",
                Side = 1
            };
            //Act
            string s = c_MsgFactory.Build(message);
            MessageReposBCGDCancel message_parse = (MessageReposBCGDCancel)c_MsgFactory.Parse(s);
            //Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void MessageReposBCGDModify_Test()
        {
            //Arrange
            MessageReposBCGDModify message = new MessageReposBCGDModify()
            {
                SenderCompID = "003",
                TargetCompID = "HNX",
                TargetSubID = "HNX1",
                //
                ClOrdID = "XDR123",
                OrgOrderID = "Test_1",
                QuoteType = 1,
                OrdType = "1",
                Side = 1,
                Account = "1",
                CoAccount = "1",
                PartyID = "1",
                CoPartyID = "1",
                EffectiveTime = "1",
                SettlMethod = 1,
                SettlDate = DateTime.Now.ToString("yyyyMMdd"),
                SettlDate2 = DateTime.Now.ToString("yyyyMMdd"),
                EndDate = DateTime.Now.ToString("yyyyMMdd"),
                RepurchaseTerm = 1,
                RepurchaseRate = 1,
                NoSide = 1,
            };
            //
            ReposSide _reposSide = new ReposSide();
            _reposSide.NumSide = 1;
            _reposSide.Symbol = "1";
            _reposSide.OrderQty = 100;
            _reposSide.Price = 1000;
            _reposSide.HedgeRate = 1;
            //
            message.RepoSideList.Add(_reposSide);
            //Act
            string s = c_MsgFactory.Build(message);
            MessageReposBCGDModify message_parse = (MessageReposBCGDModify)c_MsgFactory.Parse(s);
            //Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void MessageReposFirm_Test()
        {
            //Arrange
            MessageReposFirm message = new MessageReposFirm()
            {
                SenderCompID = "003",
                TargetCompID = "HNX",
                TargetSubID = "HNX1",
                //
                ClOrdID = "XDR123",
                RFQReqID = "RFQReqID_",
                QuoteType = 1,
                OrdType = "1",
                Side = 1,
                Account = "1",
                EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
                SettlMethod = 1,
                SettlDate = DateTime.Now.ToString("yyyyMMdd"),
                SettlDate2 = DateTime.Now.ToString("yyyyMMdd"),
                EndDate = DateTime.Now.ToString("yyyyMMdd"),
                RepurchaseTerm = 1,
                RepurchaseRate = 1,
                NoSide = 1,
            };
            //
            ReposSide _reposSide = new ReposSide();
            _reposSide.NumSide = 1;
            _reposSide.Symbol = "1";
            _reposSide.OrderQty = 100;
            _reposSide.Price = 1000;
            _reposSide.HedgeRate = 1;
            //
            message.RepoSideList.Add(_reposSide);
            //Act
            string s = c_MsgFactory.Build(message);
            MessageReposFirm message_parse = (MessageReposFirm)c_MsgFactory.Parse(s);
            //Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void MessageReposFirmAccept_Test()
        {
            //Arrange
            MessageReposFirmAccept message = new MessageReposFirmAccept()
            {
                SenderCompID = "003",
                TargetCompID = "HNX",
                TargetSubID = "HNX1",
                //
                ClOrdID = "XDR123",
                RFQReqID = "RFQReqID_",
                QuoteType = 1,
                OrdType = "1",
                Account = "1"
            };
            //Act
            string s = c_MsgFactory.Build(message);
            MessageReposFirmAccept message_parse = (MessageReposFirmAccept)c_MsgFactory.Parse(s);
            //Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void MessageReposInquiry_Test()
        {
            //Arrange
            MessageReposInquiry message = new MessageReposInquiry()
            {
                SenderCompID = "003",
                TargetCompID = "HNX",
                TargetSubID = "HNX1",
                //
                Symbol = "XDR123",
                ClOrdID = "Test_1",
                QuoteType = 1,
                OrdType = "1",
                RFQReqID = "12345678",
                Side = 1,
                OrderQty = 200,
                EffectiveTime = "20231115",
                SettlMethod = 3,
                SettlDate = "20231116",
                SettlDate2 = "20231117",
                EndDate = "20231117",
                RepurchaseTerm = 2,
                RegistID = "003",
            };
            //Act
            string s = c_MsgFactory.Build(message);
            MessageReposInquiry message_1 = (MessageReposInquiry)c_MsgFactory.Parse(s);
            //Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void MessageReposInquiryReport_Test()
        {
            //Arrange
            MessageReposInquiryReport message = new MessageReposInquiryReport()
            {
                SenderCompID = "003",
                TargetCompID = "HNX",
                TargetSubID = "HNX1",
                Symbol = "XDR123",
                ClOrdID = "Test_1",
                QuoteType = 1,
                OrdType = "1",
                RFQReqID = "12345678",
                Side = 1,
                OrderQty = 200,
                EffectiveTime = "20231115",
                SettlMethod = 3,
                SettlDate = "20231116",
                SettlDate2 = "20231117",
                EndDate = "20231117",
                RepurchaseTerm = 2,
                RegistID = "003",
                OrderPartyID = "003",
                QuoteID = "13246413",
            };
            //Act
            string s = c_MsgFactory.Build(message);
            MessageReposInquiryReport message_1 = (MessageReposInquiryReport)c_MsgFactory.Parse(s);
            //Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void MessageExecOrderRepos_Test()
        {
            //Arrange
            MessageExecOrderRepos message = new MessageExecOrderRepos()
            {
                SenderCompID = "003",
                TargetCompID = "HNX",
                TargetSubID = "HNX1",
                //
                PartyID = "PartyID_EE",
                CoPartyID = "CoPartyID_EE",
                OrderID = "OrderID_EE",
                BuyOrderID = "BuyOrderID_EE",
                SellOrderID = "SellOrderID_EE",
                RepurchaseTerm = 1,
                RepurchaseRate = 1,
                SettDate = DateTime.Now.ToString("yyyyMMdd"),
                SettlDate2 = DateTime.Now.ToString("yyyyMMdd"),
                EndDate = DateTime.Now.ToString("yyyyMMdd"),
                MatchReportType = 1,
                NoSide = 1
            };
            //
            ReposSideExecOrder _reposSide = new ReposSideExecOrder();
            _reposSide.NumSide = 1;
            _reposSide.Symbol = "1";
            _reposSide.ExecQty = 100;
            _reposSide.ExecPx = 100;
            _reposSide.Price = 1000;
            _reposSide.HedgeRate = 1;
            _reposSide.ReposInterest = 1;
            _reposSide.HedgeRate = 1;
            _reposSide.SettlValue = 1;
            _reposSide.SettlValue2 = 1;
            //
            message.ReposSideList.Add(_reposSide);
            //Act
            string s = c_MsgFactory.Build(message);
            MessageExecOrderRepos message_parse = (MessageExecOrderRepos)c_MsgFactory.Parse(s);
            //Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void MessageReposBCGDReport_Test()
        {
            //Arrange
            MessageReposBCGDReport message = new MessageReposBCGDReport()
            {
                SenderCompID = "003",
                TargetCompID = "HNX",
                TargetSubID = "HNX1",
                //
                ClOrdID = "ClOrdID_MR",
                OrgOrderID = "OrgOrderID_MR",
                OrderID = "OrderID_MR",
                QuoteType = 1,
                OrdType = "T",
                OrderPartyID = "OrderPartyID",
                InquiryMember = "InquiryMember",
                EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
                RepurchaseTerm = 1,
                RepurchaseRate = 1,
                SettlDate = "SettlDate",
                SettlDate2 = "SettlDate2",
                EndDate = "EndDate",
                SettlMethod=1,
                Account = "Account",
                CoAccount = "CoAccount",
                PartyID = "PartyID",
                CoPartyID = "CoPartyID",
                MatchReportType = 1,
                NoSide = 1
            };
            //
            ReposSideReposBCGDReport _reposSide = new ReposSideReposBCGDReport();
            _reposSide.NumSide = 1;
            _reposSide.Symbol = "1";
            _reposSide.OrderQty = 100;
            _reposSide.Price = 1000;
            _reposSide.ExecPrice = 1000;
            _reposSide.HedgeRate = 1;
            _reposSide.ReposInterest = 1;
            _reposSide.HedgeRate = 1;
            _reposSide.SettlValue = 1;
            _reposSide.SettlValue2 = 1;
            //
            message.RepoBCGDSideList.Add(_reposSide);
            //Act
            string s = c_MsgFactory.Build(message);
            MessageReposBCGDReport message_parse = (MessageReposBCGDReport)c_MsgFactory.Parse(s);
            //Assert
            Assert.IsTrue(true);
        }
    }
}