using Microsoft.VisualStudio.TestTools.UnitTesting;
using HNX.FIXMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNX.FIXMessageTests
{
    [TestClass()]
    public class MessageAdministratorTests
    {
        private MessageFactoryFIX c_MsgFactory;

        [TestInitialize]
        public void Initialize()
        {
            c_MsgFactory = new MessageFactoryFIX();
        }

        [TestMethod]
        public void TestMessageLogon()
        {
            //Arrange
            MessageLogon origLogon = new MessageLogon()
            {
                HeartBtInt = 30,
                Username = CommonData.Username,
                Password = CommonData.Password,
                SenderCompID = CommonData.FirmID,
                TargetCompID = CommonData.TargetCompID,
                TargetSubID = CommonData.TargetCompID,
                LastMsgSeqNumProcessed = 1,
                MsgSeqNum = 1,
            };


            //Action
            string origLogonMsgRaw = c_MsgFactory.Build(origLogon);
            MessageLogon obj_Parser = (MessageLogon)c_MsgFactory.Parse(origLogonMsgRaw);

            //Assert
            Assert.AreEqual(origLogon.Username, obj_Parser.Username);
            Assert.AreEqual(origLogon.Password, obj_Parser.Password);
            Assert.AreEqual(origLogon.HeartBtInt, obj_Parser.HeartBtInt);
        }

        [TestMethod]
        public void TestMsgLogout()
        {
            MessageLogout originLogout = new MessageLogout()
            {
                SenderCompID = CommonData.FirmID,
                TargetCompID = CommonData.TargetCompID,
                TargetSubID = CommonData.TargetSubID,
                Text = "Send Logout",
                MsgSeqNum = 1,
                LastMsgSeqNumProcessed = 1,
            };

            string orgLogoutRaw = c_MsgFactory.Build(originLogout);
            FIXMessageBase testFIXBase = c_MsgFactory.Parse(orgLogoutRaw);
            MessageLogout testLogout = (MessageLogout)testFIXBase;
            //Assert
            Assert.AreEqual(testLogout.Text, originLogout.Text);

        }

        [TestMethod]
        public void TestMessageHearbeat()
        {
            MessageHeartbeat originHeartBeat = new MessageHeartbeat()
            {
                TargetCompID = CommonData.TargetCompID,
                TargetSubID = CommonData.TargetSubID,
                LastMsgSeqNumProcessed = 1,
                MsgSeqNum = 1,
                TestReqID = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now)
            };

            //Act
            string msgRaw = c_MsgFactory.Build(originHeartBeat);
            FIXMessageBase testFIX = c_MsgFactory.Parse(msgRaw);
            MessageHeartbeat testHeartBeat = (MessageHeartbeat)testFIX;
            //Assert
            Assert.AreEqual(testHeartBeat.TestReqID, originHeartBeat.TestReqID);
        }

        [TestMethod]
        public void TestMsgTestRequest()
        {
            //Arrange
            MessageTestRequest originTestRequest = new MessageTestRequest()
            {
                TargetCompID = CommonData.TargetCompID,
                TargetSubID = CommonData.TargetSubID,
                LastMsgSeqNumProcessed = 1,
                MsgSeqNum = 1,
                TestReqID = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now),
            };
            //Act
            string msgRaw = c_MsgFactory.Build(originTestRequest);
            MessageTestRequest testHeartBeat = (MessageTestRequest)(c_MsgFactory.Parse(msgRaw));
            //Assert
            Assert.AreEqual(testHeartBeat.TestReqID, testHeartBeat.TestReqID);
        }

        [TestMethod]
        public void TestResendRequest()
        {
            //Arrange
            MessageResendRequest originResendRequest = new MessageResendRequest()
            {

                TargetCompID = CommonData.TargetCompID,
                TargetSubID = CommonData.TargetSubID,
                SenderCompID = CommonData.FirmID,
                BeginSeqNo = 3,
                EndSeqNo = 9,
                MsgSeqNum = 5,
                LastMsgSeqNumProcessed = 3,
                PossDupFlag = true,
            };

            //Act
            string ResendRequestRaw = c_MsgFactory.Build(originResendRequest);
            MessageResendRequest testResendReq = (MessageResendRequest)c_MsgFactory.Parse(ResendRequestRaw);

            //Assert
            Assert.AreEqual(originResendRequest.BeginSeqNo, testResendReq.BeginSeqNo);
            Assert.AreEqual(originResendRequest.EndSeqNo, testResendReq.EndSeqNo);
            Assert.AreEqual(originResendRequest.PossDupFlag, testResendReq.PossDupFlag);
        }

        [TestMethod]
        public void TestSequenceReset()
        {
            //Arange
            MessageSequenceReset originSeqReset = new MessageSequenceReset()
            {
                NewSeqNo = 35,
                MsgSeqNum = 15,
                LastMsgSeqNumProcessed = 35,
                TargetCompID = CommonData.TargetCompID,
                TargetSubID = CommonData.TargetSubID,
                SenderCompID = CommonData.FirmID,
                GapFillFlag = true
            };

            //Act
            string SeqResetRaw = c_MsgFactory.Build(originSeqReset);
            MessageSequenceReset testSeqReset = (MessageSequenceReset)c_MsgFactory.Parse(SeqResetRaw);

            //Assert
            Assert.AreEqual(testSeqReset.NewSeqNo, testSeqReset.NewSeqNo);
        }

        [TestMethod]
        public void TestIsAdministrator()
        {
            //Check Administrator
            //Act
            MessageLogon msgLogon = new MessageLogon();
            MessageLogout msgLogout = new MessageLogout();
            MessageHeartbeat msgHeartbeat = new MessageHeartbeat();
            MessageTestRequest msgTestRequest = new MessageTestRequest();
            MessageResendRequest msgResendRequest = new MessageResendRequest();
            MessageSequenceReset msgSequenceReset = new MessageSequenceReset();
            //Assert
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgLogon), true);
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgLogout), true);
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgHeartbeat), true);
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgTestRequest), true);
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgResendRequest), true);
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgSequenceReset), true);

        }
        [TestMethod]
        public void TestMsgReject()
        {
            //Arrange
            MessageReject msgReject = new MessageReject()
            {
                TargetCompID = CommonData.TargetCompID,
                TargetSubID = CommonData.TargetSubID,
                Text = "Not Login",
                RefMsgType = MessageType.Logon,
                SessionRejectReason = -10000,
                MsgSeqNum = 1,
                LastMsgSeqNumProcessed = 1,
                SenderCompID = CommonData.FirmID,

            };
            //Act
            string msgRejectRaw = c_MsgFactory.Build(msgReject);
            MessageReject testReject = (MessageReject)c_MsgFactory.Parse(msgRejectRaw);
            //Assert
            Assert.AreEqual(testReject.SessionRejectReason, msgReject.SessionRejectReason);
        }

        [TestMethod]
        public void TestIsAdministratorForReject()
        {
            //Check Administrator
            //Arrange
            MessageReject msgReject = new MessageReject();

            //Act
            msgReject.RefMsgType = MessageType.Logon;
            //Assert
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgReject), true);

            //Act
            msgReject.RefMsgType = MessageType.Logon;
            //Assert
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgReject), true);

            //Act
            msgReject.RefMsgType = MessageType.Logout;
            //Assert
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgReject), true);

            //Act
            msgReject.RefMsgType = MessageType.Heartbeat;
            //Assert
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgReject), true);

            //Act
            msgReject.RefMsgType = MessageType.TestRequest;
            //Assert
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgReject), true);

            //Act
            msgReject.RefMsgType = MessageType.ResendRequest;
            //Assert
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgReject), true);

            //Act
            msgReject.RefMsgType = MessageType.NewOrderCross;
            //Assert
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgReject), false);

            //Act
            msgReject.RefMsgType = MessageType.CrossOrderCancelReplaceRequest;
            //Assert
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgReject), false);
            //Act
            msgReject.RefMsgType = MessageType.CrossOrderCancelRequest;
            //Assert
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgReject), false);
            //Act
            msgReject.RefMsgType = MessageType.ReportOrderCross;
            //Assert
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgReject), false);
            //Act
            msgReject.RefMsgType = MessageType.Quote;
            //Assert
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgReject), false);

            //Act
            msgReject.RefMsgType = MessageType.QuoteCancel;
            //Assert
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgReject), false);

            //Act
            msgReject.RefMsgType = MessageType.QuoteRequest;
            //Assert
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgReject), false);

            //Act
            msgReject.RefMsgType = MessageType.QuoteResponse;
            //Assert
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgReject), false);

            //Act
            msgReject.RefMsgType = MessageType.QuoteStatusReport;
            //Assert
            Assert.AreEqual(c_MsgFactory.IsAdminitrativeMessage(msgReject), false);

        }

    }
}