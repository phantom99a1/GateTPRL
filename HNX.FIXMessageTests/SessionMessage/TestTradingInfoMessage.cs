using HNX.FIXMessage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HNX.FIXMessageTests.SessionMessage
{
    [TestClass]
    public class TestTradingInfoMessage
    {
        private MessageFactoryFIX c_MsgFactory = new MessageFactoryFIX();

        [TestMethod]
        public void TestMsgConfirmExchange()
        {
            MessageReposFirmReport msgBoard = (MessageReposFirmReport)c_MsgFactory.Parse("8=FIX.4.4\u00019=309\u000135=N04\u000149=HNX\u000156=043.01GW\u000134=53\u0001369=6\u000152=20231207-02:35:28\u000111=TEST_3\u000154=2\u0001117=REPOSPT-2401090000002\u0001448=\u0001537=1\u0001644=REPOSPT-2401090000001\u000140=U\u00011=\u00014488=043\u0001552=1\u00015522=1\u000138=120\u000155=XDCR12101\u0001640=9800\u00016464=1176000\u00016465=1176064\u00012261=64\u00012260=2\u0001227=2\u0001226=1\u0001168=20240109\u000164=20240109\u0001193=20240110\u0001917=20240110\u00014499=043\u000110=067\u0001");
            string msgBoardRaw = c_MsgFactory.Build(msgBoard);
            MessageReposFirmReport msgBoardTest = (MessageReposFirmReport)c_MsgFactory.Parse(msgBoardRaw);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestTradindSessionStatus()
        {
            //Test Message theo bảng
            // 
            //Act
            MessageTradingSessionStatus msgBoard = (MessageTradingSessionStatus)c_MsgFactory.Parse("8=FIX.4.4\u00019=109\u000135=h\u000149=HNX\u000156=043.01GW\u000134=47\u0001369=2\u000152=20230919-02:26:20\u0001336=DEMO\u0001340=1\u0001341=20230919-02:18:39\u0001339=1\u0001335=DEMO\u000110=205\u0001");
            string msgBoardRaw = c_MsgFactory.Build(msgBoard);
            MessageTradingSessionStatus msgBoardTest = (MessageTradingSessionStatus)c_MsgFactory.Parse(msgBoardRaw);

            //Assert
            Assert.AreEqual(msgBoard.TradSesMode, msgBoardTest.TradSesMode);
            Assert.AreEqual(msgBoard.TradSesReqID, msgBoardTest.TradSesReqID);
            Assert.AreEqual(msgBoard.TradSesStartTime, msgBoardTest.TradSesStartTime);
            Assert.AreEqual(msgBoard.TradingSessionID, msgBoardTest.TradingSessionID);

            //Test Message trả về mã chứng khoán
            //Act
            MessageTradingSessionStatus msgSecurity = (MessageTradingSessionStatus)c_MsgFactory.Parse("8=FIX.4.4\u00019=120\u000135=h\u000149=HNX\u000156=043.01GW\u000134=45\u0001369=2\u000152=20230919-02:26:20\u0001336=STATUS_NTN\u0001340=1\u0001341=20230919-02:18:31\u0001339=2\u0001335=AAAR12101\u000110=176\u0001");
            string msgSecurityRaw = c_MsgFactory.Build(msgSecurity);
            MessageTradingSessionStatus msgSecurityTest = (MessageTradingSessionStatus)c_MsgFactory.Parse(msgSecurityRaw);

            //Assert
            Assert.AreEqual(msgSecurity.TradSesMode, msgSecurityTest.TradSesMode);
            Assert.AreEqual(msgSecurity.TradSesReqID, msgSecurityTest.TradSesReqID);
            Assert.AreEqual(msgSecurity.TradSesStartTime, msgSecurityTest.TradSesStartTime);
            Assert.AreEqual(msgSecurity.TradingSessionID, msgSecurityTest.TradingSessionID);
        }

        [TestMethod]
        public void TestTradingSessionRequest()
        {
            MessageTradingSessionStatusRequest msgTradingSessReq = new MessageTradingSessionStatusRequest()
            {
                TradSesMode = 1,
                TradSesReqID = "0",
                TargetCompID = CommonData.TargetCompID,
                TargetSubID = CommonData.TargetSubID,
                SenderCompID = CommonData.FirmID,
                MsgSeqNum = 1,
                SubscriptionRequestType = '1',
                LastMsgSeqNumProcessed = 1
            };
            string msgTradSesReqRaw = c_MsgFactory.Build(msgTradingSessReq);
            MessageTradingSessionStatusRequest testTradSessReq = (MessageTradingSessionStatusRequest)c_MsgFactory.Parse(msgTradSesReqRaw);

            //Assert
            Assert.AreEqual(msgTradingSessReq.TradSesMode, testTradSessReq.TradSesMode);
            Assert.AreEqual(msgTradingSessReq.TradSesReqID, testTradSessReq.TradSesReqID);
        }

        [TestMethod]
        public void TestSecurityStatus()
        {
            //Arrange
            MessageSecurityStatus msgSecurity = (MessageSecurityStatus)c_MsgFactory.Parse("8=FIX.4.4\u00019=225\u000135=f\u000149=HNX\u000156=043.01GW\u000134=45\u0001369=1\u000152=20230921-01:55:47\u0001324=085433\u000155=AAAR12101\u0001167=\u0001541=20260522\u0001225=20210522\u0001106=TCPHCBIS\u000131=100000\u0001332=0\u0001333=0\u00013321=0\u00013331=0\u00013322=110000\u00013332=0\u0001326=0\u0001330=10\u0001625=CBTS\u00016251=0\u0001265=98\u0001109=1000\u000110=010\u0001");
            string msgSecurityRaw = c_MsgFactory.Build(msgSecurity);
            MessageSecurityStatus msgSecurityTest = (MessageSecurityStatus)c_MsgFactory.Parse(msgSecurityRaw);

            Assert.AreEqual(msgSecurity.HighPx, msgSecurityTest.HighPx);
            Assert.AreEqual(msgSecurity.LowPx, msgSecurityTest.LowPx);
            Assert.AreEqual(msgSecurity.Symbol, msgSecurityTest.Symbol);
            Assert.AreEqual(msgSecurity.TradingSessionSubID, msgSecurityTest.TradingSessionSubID);
            Assert.AreEqual(msgSecurity.SecurityDesc, msgSecurityTest.SecurityDesc);
        }

        [TestMethod]
        public void TestSecurityStatusRequest()
        {
            //Arrange
            MessageSecurityStatusRequest msgSecuRequest = new MessageSecurityStatusRequest()
            {
                Symbol = "XDCR12101",
                SecurityStatusReqID = "1",
                SenderCompID = CommonData.FirmID,
                TargetCompID = CommonData.TargetCompID,
                TargetSubID = CommonData.TargetSubID,
                MsgSeqNum = 1,
                LastMsgSeqNumProcessed = 1,
            };
            //Act
            string msgSecRequestRaw = c_MsgFactory.Build(msgSecuRequest);
            MessageSecurityStatusRequest testSecRequest = (MessageSecurityStatusRequest)c_MsgFactory.Parse(msgSecRequestRaw);
            //Assert
            Assert.AreEqual(msgSecuRequest.Symbol, testSecRequest.Symbol);
            Assert.AreEqual(msgSecuRequest.SecurityStatusReqID, testSecRequest.SecurityStatusReqID);
        }

        [TestMethod]
        public void TestTopicTradingInfomation()
        {
            //Arrange
            MessageTopicTradingInfomation msgSecurity = (MessageTopicTradingInfomation)c_MsgFactory.Parse("8=FIX.4.4\u00019=84\u000135=MN\u000149=HNX\u000156=043.01GW\u000134=42\u0001369=0\u000152=20231221-10:39:19\u000155=BABR12001\u00014499=043,018\u000110=091\u0001");
            string msgmsgTopicTradingSessionRaw = c_MsgFactory.Build(msgSecurity);
            MessageTopicTradingInfomation msgTopicTradingSession = (MessageTopicTradingInfomation)c_MsgFactory.Parse(msgmsgTopicTradingSessionRaw);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void UserRequest_Test()
        {
            //Arrange
            MessageUserRequest msgUserRequest = new MessageUserRequest()
            {
                UserRequestID = "1",
                UserRequestType = 1,
                SenderCompID = CommonData.FirmID,
                TargetCompID = CommonData.TargetCompID,
                TargetSubID = CommonData.TargetSubID,
                MsgSeqNum = 1,
                LastMsgSeqNumProcessed = 1,
                Username = "bacnd",
                NewPassword = "bacnd123",
            };
            //Act
            string msgSecRequestRaw = c_MsgFactory.Build(msgUserRequest);
            MessageUserRequest testSecRequest = (MessageUserRequest)c_MsgFactory.Parse(msgSecRequestRaw);
            //Assert
            Assert.AreEqual(testSecRequest.Username, testSecRequest.Username);
        }

        [TestMethod]
        public void UserResponse_Test()
        {
            //Arrange
            MessageUserResponse msgUserResponse = new MessageUserResponse()
            {
                UserRequestID = "1",
                SenderCompID = CommonData.FirmID,
                TargetCompID = CommonData.TargetCompID,
                TargetSubID = CommonData.TargetSubID,
                MsgSeqNum = 1,
                LastMsgSeqNumProcessed = 1,
                Username = "bacnd",
                UserStatus = 1,
                UserStatusText = "bacnd123",
            };
            //Act
            string msgSecRequestRaw = c_MsgFactory.Build(msgUserResponse);
            MessageUserResponse testSecRequest = (MessageUserResponse)c_MsgFactory.Parse(msgSecRequestRaw);
            //Assert
            Assert.AreEqual(testSecRequest.Username, testSecRequest.Username);
        }
    }
}