using BusinessProcessResponse;
using CommonLib;
using HNX.FIXMessage;
using HNXInterface;
using LocalMemory;
using Moq;

namespace HNXUnitTest
{
    [TestClass]
    public class TestHNXInterface
    {
        public MessageFactoryFIX c_MsgFactorryFIX = new MessageFactoryFIX();

        [TestInitialize]
        public void Initialize()
        {
            InitObject.ReadConfigTest();
        }

        [TestMethod]
        public void TestStopConnection()
        {
            var mockResponseInterface = new Mock<IResponseInterface>();
            HNXTCPClient HNXClient = new HNXTCPClient(mockResponseInterface.Object);
            Mock<MyStream> mockMyStream = new Mock<MyStream>();
            HNXClient.c_CurrentConnected = mockMyStream.Object;
            //Act
            HNXClient.StopCurrentConnected("Stop for test");
            //Assert
            mockMyStream.Verify(mystream => mystream.CloseStream(), Times.Once);
            Assert.AreEqual(HNXClient.ClientStatus(), enumClientStatus.DISCONNECT);
            //Act
            HNXClient.c_CurrentConnected = null;
            HNXClient.StopCurrentConnected("Stop for test");
            Assert.AreEqual(HNXClient.ClientStatus(), enumClientStatus.DISCONNECT);
        }



        #region Test xử lý nhận Admin message

        //Xử lý nhận logon
        [TestMethod]
        public void TestRecvLogonHNX()
        {
            var mockResponseInterface = new Mock<IResponseInterface>();
            HNXTCPClient HNXClient = new HNXTCPClient(mockResponseInterface.Object);

            Mock<MyStream> mockMyStream = new Mock<MyStream>();
            //Act - Giả lập kết nối đầu ngày, coi như đã kết nối được và send login
            HNXClient.c_CurrentConnected = mockMyStream.Object;
            HNXClient.__ClientStatus = enumClientStatus.HANDSHAKING;
            HNXClient.StartHNXTCPClient();
            FIXMessageBase fMsgBase = c_MsgFactorryFIX.Parse("8=FIX.4.4\u00019=85\u000135=A\u000149=HNX\u000156=003.01GW\u000134=0\u0001369=0\u000152=20230908-02:55:58\u000158=Accept login\u000198=0\u0001108=30\u000110=117\u0001");
            fMsgBase.MsgSeqNum = HNXClient.LastSeqProcess();
            fMsgBase.LastMsgSeqNumProcessed = HNXClient.Seq();
            string s = c_MsgFactorryFIX.Build(fMsgBase);
            //Act

            mockMyStream.Setup(helper => helper.ReadString()).Returns(() =>
            {
                if (HNXClient.__ClientStatus != enumClientStatus.DATA_TRANSFER)
                    return s;
                else
                    return "";
            });
            //Đoạn này phải đợi cho nó xử lý, tạm thời cho 2s để xử lý
            int times = 0;
            while (HNXClient.ClientStatus() != enumClientStatus.DATA_TRANSFER)
            {
                if (times < 2)
                {
                    times++;
                    Thread.Sleep(1000);
                }
                else break;
            }
            Assert.AreEqual(HNXClient.ClientStatus(), enumClientStatus.DATA_TRANSFER);
        }

        //Xử lý nhận ResendRequest
        [TestMethod]
        public void TestRecvResendRequest()
        {
            var mockResponseInterface = new Mock<IResponseInterface>();
            HNXTCPClient HNXClient = new HNXTCPClient(mockResponseInterface.Object);
            HNXClient.ChangeSeq(100);
            Mock<MyStream> mockMyStream = new Mock<MyStream>();
            //Act - Giả lập kết nối đầu ngày, coi như đã kết nối được và send login
            HNXClient.c_CurrentConnected = mockMyStream.Object;
            HNXClient.__ClientStatus = enumClientStatus.HANDSHAKING;
            HNXClient.StartHNXTCPClient();
            MessageResendRequest fMsgResendRequest = new MessageResendRequest()
            {
                BeginSeqNo = 90,
                EndSeqNo = 91,
                MsgSeqNum = HNXClient.LastSeqProcess(),
            };

            string s = c_MsgFactorryFIX.Build(fMsgResendRequest);
            //Act
            int count = 0;
            mockMyStream.Setup(helper => helper.ReadString()).Returns(() =>
            {
                if (fMsgResendRequest.EndSeqNo < HNXClient.Seq())
                {
                    fMsgResendRequest.EndSeqNo++;
                    return c_MsgFactorryFIX.Build(fMsgResendRequest);
                }
                else
                    return "";
            });
            //Đoạn này phải đợi cho nó xử lý, tạm thời cho 2s để xử lý
            int times = 0;
            Thread.Sleep(1000);
            while (HNXClient.ClientStatus() != enumClientStatus.DATA_TRANSFER)
            {
                if (times < 10)
                {
                    times++;
                    Thread.Sleep(1000);
                }
                else break;
            }
            mockMyStream.Verify(helper => helper.WriteString(It.IsAny<string>()));
        }

        //Test Recv SequenceReset
        [TestMethod]
        public void TestRecvSeqReset()
        {
            var mockResponseInterface = new Mock<IResponseInterface>();
            HNXTCPClient HNXClient = new HNXTCPClient(mockResponseInterface.Object);

            Mock<MyStream> mockMyStream = new Mock<MyStream>();
            //Act - Giả lập kết nối đầu ngày, coi như đã kết nối được và send login
            HNXClient.c_CurrentConnected = mockMyStream.Object;
            HNXClient.__ClientStatus = enumClientStatus.HANDSHAKING;
            HNXClient.StartHNXTCPClient();
            MessageSequenceReset fMsgResendRequest = new MessageSequenceReset()
            {
                NewSeqNo = HNXClient.Seq() + 5,
                MsgSeqNum = HNXClient.LastSeqProcess(),
                LastMsgSeqNumProcessed = HNXClient.Seq() + 4
            };

            string s = c_MsgFactorryFIX.Build(fMsgResendRequest);
            //Act

            mockMyStream.Setup(helper => helper.ReadString()).Returns(() =>
            {
                if (HNXClient.__ClientStatus != enumClientStatus.DATA_TRANSFER)
                    return s;
                else
                    return "";
            });
            //Đoạn này phải đợi cho nó xử lý, tạm thời cho 2s để xử lý
            int times = 0;
            while (HNXClient.__ClientStatus == enumClientStatus.HANDSHAKING)
            {
                if (times < 2)
                {
                    times++;
                    Thread.Sleep(1000);
                }
                else break;
            }
            Assert.AreEqual(HNXClient.Seq(), fMsgResendRequest.LastMsgSeqNumProcessed);
        }

        #endregion Test xử lý nhận Admin message

        #region Xử lý nhận Application Message

        //Xử lý nhận Trading Session
        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=110\u000135=h\u000149=HNX\u000156=003.01GW\u000134=23\u0001369=2\u000152=20230908-02:02:42\u0001336=NONE\u0001340=90\u0001341=20230830-01:00:00\u0001339=1\u0001335=DEMO\u000110=220\u0001", "Cho nhan lenh")]
        [DataRow("8=FIX.4.4\u00019=109\u000135=h\u000149=HNX\u000156=003.01GW\u000134=46\u0001369=3\u000152=20230908-02:15:14\u0001336=DEMO\u0001340=1\u0001341=20230908-02:15:14\u0001339=1\u0001335=DEMO\u000110=188\u0001", "Dang giao dich")]
        [DataRow("8=FIX.4.4\u00019=109\u000135=h\u000149=HNX\u000156=003.01GW\u000134=49\u0001369=3\u000152=20230908-02:15:44\u0001336=DEMO\u0001340=2\u0001341=20230908-02:15:44\u0001339=1\u0001335=DEMO\u000110=198\u0001", "Tam dung giao dich")]
        [DataRow("8=FIX.4.4\u00019=110\u000135=h\u000149=HNX\u000156=003.01GW\u000134=55\u0001369=3\u000152=20230908-02:15:57\u0001336=DEMO\u0001340=13\u0001341=20230908-02:15:57\u0001339=1\u0001335=DEMO\u000110=245\u0001", "Ket thuc nhan lenh")]
        [DataRow("8=FIX.4.4\u00019=111\u000135=h\u000149=HNX\u000156=003.01GW\u000134=118\u0001369=3\u000152=20230908-02:17:05\u0001336=NONE\u0001340=97\u0001341=20230908-02:17:05\u0001339=1\u0001335=DEMO\u000110=051\u0001", "Dong cua thi truong")]
        public void TestRecvTradingSession(string p_MessageRaw, string p_TradingSession)
        {
            //Arrange
            var mockResponseInterface = new Mock<IResponseInterface>();
            HNXTCPClient HNXClient = new HNXTCPClient(mockResponseInterface.Object);
            Mock<MyStream> mockMyStream = new Mock<MyStream>();
            HNXClient.c_CurrentConnected = mockMyStream.Object;
            HNXClient.__ClientStatus = enumClientStatus.DATA_TRANSFER;
            HNXClient.StartHNXTCPClient();
            Mock<IResponseInterface> mockIKAfkaInterface = new Mock<IResponseInterface>();
            //Setup
            FIXMessageBase fMsgBase = c_MsgFactorryFIX.Parse(p_MessageRaw);
            fMsgBase.MsgSeqNum = HNXClient.LastSeqProcess() + 1;
            fMsgBase.LastMsgSeqNumProcessed = HNXClient.Seq();
            string s = c_MsgFactorryFIX.Build(fMsgBase);
            int i = 0;
            mockMyStream.Setup(helper => helper.ReadString()).Returns(() =>
            {
                if (i == 0)
                {
                    i++;
                    return s;
                }
                else return "";
            });
            Thread.Sleep(1000);
            //Assert
            mockResponseInterface.Verify(helper => helper.ResponseHNXSendTradingSessionStatus(It.IsAny<MessageTradingSessionStatus>()), Times.Once);
            Thread.Sleep(100);
            Assert.AreEqual(TradingRuleData.GetTradingSessionNameofMainBoard(), p_TradingSession);
        }

        //Xử lý nhận TradingSession cho Symbol
        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=115\u000135=h\u000149=HNX\u000156=043.01GW\u000134=111\u0001369=2\u000152=20230926-01:54:01\u0001336=DEMO\u0001340=1\u0001341=20230926-01:54:00\u0001339=2\u0001335=XDCR12101\u000110=226\u0001", "XDCR12101", CommonData.ORDER_RETURNMESSAGE.SUCCESS, CommonData.ORDER_RETURNCODE.SUCCESS)]
        [DataRow("8=FIX.4.4\u00019=115\u000135=h\u000149=HNX\u000156=043.01GW\u000134=111\u0001369=2\u000152=20230926-01:54:01\u0001336=DEMO\u0001340=1\u0001341=20230926-01:54:00\u0001339=2\u0001335=XDCR12101\u000110=226\u0001", "DUNGTEST123", CommonData.ORDER_RETURNMESSAGE.SYMBOL_IS_NOT_FOUND, CommonData.ORDER_RETURNCODE.SYMBOL_IS_NOT_FOUND)]
        [DataRow("8=FIX.4.4\u00019=117\u000135=h\u000149=HNX\u000156=043.01GW\u000134=225\u0001369=80\u000152=20230926-10:58:15\u0001336=DEMO\u0001340=13\u0001341=20230926-10:58:15\u0001339=2\u0001335=XDCR12101\u000110=102\u0001", "XDCR12101", CommonData.ORDER_RETURNMESSAGE.MARKET_CLOSE, CommonData.ORDER_RETURNCODE.MARKET_CLOSE)]
        public void TestRecvTradingSession_forSymbol(string p_MessageRaw, string p_Symbol, string p_Text, string p_Code)
        {
            //Arrange
            var mockResponseInterface = new Mock<IResponseInterface>();
            HNXTCPClient HNXClient = new HNXTCPClient(mockResponseInterface.Object);
            Mock<MyStream> mockMyStream = new Mock<MyStream>();
            HNXClient.c_CurrentConnected = mockMyStream.Object;
            HNXClient.__ClientStatus = enumClientStatus.DATA_TRANSFER;
            HNXClient.StartHNXTCPClient();

            //Setup
            MessageSecurityStatus msgSecurityStatus = (MessageSecurityStatus)c_MsgFactorryFIX.Parse("8=FIX.4.4\u00019=240\u000135=f\u000149=HNX\u000156=043.01GW\u000134=48\u0001369=2\u000152=20230926-01:52:44\u0001324=\u000155=XDCR12101\u0001167=\u0001541=20240326\u0001225=20210326\u0001106=TCPHCBIS\u000131=100000\u0001332=0\u0001333=0\u00013321=0\u00013331=0\u00013322=110000\u00013332=0\u0001326=0\u0001330=0\u0001625=DEMO\u00016251=0\u0001265=104\u0001109=8000000\u00019735=1,2\u00019736=1,2\u000110=191\u0001");
            TradingRuleData.ProcessSecurityStatus(msgSecurityStatus);
            MessageTradingSessionStatus msgTradingSession = (MessageTradingSessionStatus)c_MsgFactorryFIX.Parse("8=FIX.4.4\u00019=110\u000135=h\u000149=HNX\u000156=043.01GW\u000134=113\u0001369=3\u000152=20230926-02:02:25\u0001336=DEMO\u0001340=1\u0001341=20230926-01:54:00\u0001339=1\u0001335=DEMO\u000110=222\u0001");
            TradingRuleData.ProcessTradingSession(msgTradingSession);
            FIXMessageBase fMsgBase = c_MsgFactorryFIX.Parse(p_MessageRaw);
            fMsgBase.MsgSeqNum = HNXClient.LastSeqProcess() + 1;
            fMsgBase.LastMsgSeqNumProcessed = HNXClient.Seq();
            string s = c_MsgFactorryFIX.Build(fMsgBase);
            int i = 0;
            mockMyStream.Setup(helper => helper.ReadString()).Returns(() =>
            {
                if (i == 0)
                {
                    i++;
                    return s;
                }
                else return "";
            });
            Thread.Sleep(100);
            //Act
            string text = "";
            string code = "";
            //Assert
            TradingRuleData.CheckTradingRule_Input(p_Symbol, out text, out code);
            Assert.AreEqual(text, p_Text);
            Assert.AreEqual(code, p_Code);
            mockResponseInterface.Verify(helper => helper.ResponseHNXSendTradingSessionStatus(It.IsAny<MessageTradingSessionStatus>()), Times.Once());
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=243\u000135=f\u000149=HNX\u000156=043.01GW\u000134=113\u0001369=2\u000152=20230927-01:56:27\u0001324=\u000155=XDCR12102\u0001167=\u0001541=20250409\u0001225=20210409\u0001106=TCPHCBIS\u000131=100000\u0001332=0\u0001333=0\u00013321=0\u00013331=0\u00013322=110000\u00013332=0\u0001326=10\u0001330=0\u0001625=DEMO\u00016251=0\u0001265=105\u0001109=11000000\u00019735=1,2\u00019736=1,2\u000110=083\u0001", "XDCR12102")]
        public void TestRecv_SecurityStatus_InTradingSession(string p_MessageRaw, string p_Symbol)
        {
            //Arrange
            var mockResponseInterface = new Mock<IResponseInterface>();
            HNXTCPClient HNXClient = new HNXTCPClient(mockResponseInterface.Object);
            Mock<MyStream> mockMyStream = new Mock<MyStream>();
            HNXClient.c_CurrentConnected = mockMyStream.Object;
            HNXClient.__ClientStatus = enumClientStatus.DATA_TRANSFER;
            HNXClient.StartHNXTCPClient();
            //Setup
            MessageSecurityStatus msgSecurityStatus = (MessageSecurityStatus)c_MsgFactorryFIX.Parse("8=FIX.4.4\u00019=240\u000135=f\u000149=HNX\u000156=043.01GW\u000134=88\u0001369=2\u000152=20230927-01:55:14\u0001324=\u000155=XDCR12101\u0001167=\u0001541=20240326\u0001225=20210326\u0001106=TCPHCBIS\u000131=100000\u0001332=0\u0001333=0\u00013321=0\u00013331=0\u00013322=110000\u00013332=0\u0001326=0\u0001330=0\u0001625=DEMO\u00016251=0\u0001265=105\u0001109=8000000\u00019735=1,2\u00019736=1,2\u000110=197\u0001");
            TradingRuleData.ProcessSecurityStatus(msgSecurityStatus);
            MessageTradingSessionStatus msgTradingSession = (MessageTradingSessionStatus)c_MsgFactorryFIX.Parse("8=FIX.4.4\u00019=110\u000135=h\u000149=HNX\u000156=043.01GW\u000134=113\u0001369=3\u000152=20230926-02:02:25\u0001336=DEMO\u0001340=1\u0001341=20230926-01:54:00\u0001339=1\u0001335=DEMO\u000110=222\u0001");
            TradingRuleData.ProcessTradingSession(msgTradingSession);
            FIXMessageBase fMsgBase = c_MsgFactorryFIX.Parse(p_MessageRaw);
            fMsgBase.MsgSeqNum = HNXClient.LastSeqProcess() + 1;
            fMsgBase.LastMsgSeqNumProcessed = HNXClient.Seq();
            string s = c_MsgFactorryFIX.Build(fMsgBase);
            int i = 0;
            mockMyStream.Setup(helper => helper.ReadString()).Returns(() =>
            {
                if (i == 0)
                {
                    i++;
                    return s;
                }
                else return "";
            });
            Thread.Sleep(100);
            //Act
            string text = "";
            string code = "";
            //Assert
            mockResponseInterface.Verify(helper => helper.ResponseHNXSendSecurityStatus(It.IsAny<MessageSecurityStatus>()), Times.Once);
            TradingRuleData.CheckTradingRule_Input(p_Symbol, out text, out code);
            Assert.AreEqual(text, CommonData.ORDER_RETURNMESSAGE.SYMBOL_IS_SUSPENDED);
            Assert.AreEqual(code, CommonData.ORDER_RETURNCODE.SYMBOL_IS_SUSPENDED);
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=240\u000135=f\u000149=HNX\u000156=043.01GW\u000134=27\u0001369=2\u000152=20230926-01:51:14\u0001324=\u000155=XDCR12101\u0001167=\u0001541=20240326\u0001225=20210326\u0001106=TCPHCBIS\u000131=100000\u0001332=0\u0001333=0\u00013321=0\u00013331=0\u00013322=110000\u00013332=0\u0001326=0\u0001330=0\u0001625=DEMO\u00016251=0\u0001265=104\u0001109=8000000\u00019735=1,2\u00019736=1,2\u000110=184\u0001", "XDCR12101")]
        [DataRow("8=FIX.4.4\u00019=248\u000135=f\u000149=HNX\u000156=043.01GW\u000134=28\u0001369=2\u000152=20230926-01:51:14\u0001324=\u000155=VPLR12003\u0001167=\u0001541=20230828\u0001225=20200828\u0001106=TCPHCBIS\u000131=100000\u0001332=0\u0001333=0\u00013321=0\u00013331=0\u00013322=110000\u00013332=0\u0001326=10\u0001330=0\u0001625=CBTS\u00016251=0\u0001265=104\u0001109=500000\u00019735=1,2,3,4,5,6\u00019736=1,2\u000110=104\u0001", "VPLR12003")]
        [DataRow("8=FIX.4.4\u00019=241\u000135=f\u000149=HNX\u000156=043.01GW\u000134=88\u0001369=2\u000152=20230926-01:53:09\u0001324=\u000155=XDCR12102\u0001167=\u0001541=20250409\u0001225=20210409\u0001106=TCPHCBIS\u000131=100000\u0001332=0\u0001333=0\u00013321=0\u00013331=0\u00013322=110000\u00013332=0\u0001326=0\u0001330=0\u0001625=DEMO\u00016251=0\u0001265=104\u0001109=11000000\u00019735=1,2\u00019736=1,2\u000110=246\u0001", "XDCR12102")]
        [DataRow("8=FIX.4.4\u00019=249\u000135=f\u000149=HNX\u000156=043.01GW\u000134=85\u0001369=2\u000152=20230926-01:53:09\u0001324=\u000155=BABR12003\u0001167=\u0001541=20230811\u0001225=20200811\u0001106=TCPHCBIS\u000131=0\u0001332=0\u0001333=0\u00013321=0\u00013331=0\u00013322=1100000000\u00013332=0\u0001326=10\u0001330=0\u0001625=CBTS\u00016251=0\u0001265=102\u0001109=29740650\u00019735=1,2,3,4,5,6\u00019736=1,2\u000110=126\u0001", "BABR12003")]
        public void TestRecv_SecurityStatus_BeforeMarketOpen(string p_MessageRaw, string p_Symbol)
        {
            //Arrange
            var mockResponseInterface = new Mock<IResponseInterface>();
            HNXTCPClient HNXClient = new HNXTCPClient(mockResponseInterface.Object);
            Mock<MyStream> mockMyStream = new Mock<MyStream>();
            HNXClient.c_CurrentConnected = mockMyStream.Object;
            HNXClient.__ClientStatus = enumClientStatus.DATA_TRANSFER;
            HNXClient.StartHNXTCPClient();
            //Setup
            FIXMessageBase fMsgBase = c_MsgFactorryFIX.Parse(p_MessageRaw);
            fMsgBase.MsgSeqNum = HNXClient.LastSeqProcess() + 1;
            fMsgBase.LastMsgSeqNumProcessed = HNXClient.Seq();
            string s = c_MsgFactorryFIX.Build(fMsgBase);
            int i = 0;
            mockMyStream.Setup(helper => helper.ReadString()).Returns(() =>
            {
                if (i == 0)
                {
                    i++;
                    return s;
                }
                else return "";
            });
            Thread.Sleep(1000);
            //Assert
            mockResponseInterface.Verify(helper => helper.ResponseHNXSendSecurityStatus(It.IsAny<MessageSecurityStatus>()), Times.Once);
            string text = "";
            string code = "";
            TradingRuleData.CheckTradingRule_Input(p_Symbol, out text, out code);
            Assert.AreNotEqual(text, CommonData.ORDER_RETURNMESSAGE.SYMBOL_IS_NOT_FOUND);
            Assert.AreNotEqual(text, CommonData.ORDER_RETURNCODE.SYMBOL_IS_NOT_FOUND);
        }

        //Xử lý nhận Quote Status
        [TestMethod]
        public void TestRecvQuoteStatus()
        {
            //Arrange
            FIXMessageBase fMsgBase = c_MsgFactorryFIX.Parse("8=FIX.4.4\u00019=198\u000135=AI\u000149=HNX\u000156=003.01GW\u000134=57\u0001369=8\u000152=20230907-03:23:06\u000111=111\u00014488=003\u0001537=1\u00011=003C111111\u000155=XDCR12101\u000154=1\u000140=S\u000138=5\u0001131=\u0001171=XDCR12101-2308280000002\u0001640=1000\u00016464=5000\u00016363=1\u000164=20230828\u0001513=0\u000110=237\r\n");
            var mockResponseInterface = new Mock<IResponseInterface>();
            HNXTCPClient HNXClient = new HNXTCPClient(mockResponseInterface.Object);
            Mock<MyStream> mockMyStream = new Mock<MyStream>();
            HNXClient.c_CurrentConnected = mockMyStream.Object;
            HNXClient.__ClientStatus = enumClientStatus.DATA_TRANSFER;
            //Setup
            HNXClient.StartHNXTCPClient();

            fMsgBase.MsgSeqNum = HNXClient.LastSeqProcess() + 1;
            fMsgBase.LastMsgSeqNumProcessed = HNXClient.Seq() + 1;
            string s = c_MsgFactorryFIX.Build(fMsgBase);
            int i = 0;
            mockMyStream.Setup(helper => helper.ReadString()).Returns(() =>
            {
                if (i == 0)
                {
                    i++;
                    return s;
                }
                else return "";
            });
            int times = 0;
            while (HNXClient.LastSeqProcess() != fMsgBase.MsgSeqNum)
            {
                if (times < 2)
                {
                    times++;
                    Thread.Sleep(1000);
                }
                else break;
            }
            mockResponseInterface.Verify(helper => helper.HNXSendQuoteStatusReport(It.IsAny<MessageQuoteSatusReport>()), Times.Once);
        }

        //Xử lý nhận Reject
        [TestMethod]
        public void TestRecvApplicationReject()
        {
            //Arrange
            FIXMessageBase fMsgBase = c_MsgFactorryFIX.Parse("8=FIX.4.4\u00019=135\u000135=3\u000149=HNX\u000156=003.01GW\u000134=35\u0001369=6\u000152=20230831-08:37:50\u000158=NG?Y THANH TO?N PH?I L?N H?N NG?Y B?T ??U GIAO D?CH!\u0001372=S\u0001373=-34006\u000145=6\u000110=016\u0001");
            var mockResponseInterface = new Mock<IResponseInterface>();
            HNXTCPClient HNXClient = new HNXTCPClient(mockResponseInterface.Object);
            Mock<MyStream> mockMyStream = new Mock<MyStream>();
            HNXClient.c_CurrentConnected = mockMyStream.Object;
            HNXClient.__ClientStatus = enumClientStatus.DATA_TRANSFER;
            //Setup
            HNXClient.StartHNXTCPClient();

            fMsgBase.MsgSeqNum = HNXClient.LastSeqProcess() + 1;
            fMsgBase.LastMsgSeqNumProcessed = HNXClient.Seq() + 1;
            string s = c_MsgFactorryFIX.Build(fMsgBase);
            int i = 0;
            mockMyStream.Setup(helper => helper.ReadString()).Returns(() =>
            {
                if (i == 0)
                {
                    i++;
                    return s;
                }
                else return "";
            });
            int times = 0;
            while (HNXClient.LastSeqProcess() != fMsgBase.MsgSeqNum)
            {
                if (times < 2)
                {
                    times++;
                    Thread.Sleep(1000);
                }
                else break;
            }
            mockResponseInterface.Verify(helper => helper.HNXSendReject(It.IsAny<MessageReject>()), Times.Once);
        }

        #endregion Xử lý nhận Application Message

        #region Test xử lý gửi

        [TestMethod]
        public void TestSend2HNX()
        {
            var mockResponseInterface = new Mock<IResponseInterface>();
            HNXTCPClient HNXClient = new HNXTCPClient(mockResponseInterface.Object);
            Mock<MyStream> mockMyStream = new Mock<MyStream>();
            HNXClient.c_CurrentConnected = mockMyStream.Object;
            HNXClient.__ClientStatus = enumClientStatus.DATA_TRANSFER;
            mockMyStream.Reset();
            //Act

            HNXClient.Send2HNX(new MessageTradingSessionStatusRequest()
            {
                TradSesMode = 1
            });
            mockMyStream.Verify(helper => helper.WriteByte(It.IsAny<byte[]>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void TestSendLogout()
        {
            var mockResponseInterface = new Mock<IResponseInterface>();
            HNXTCPClient HNXClient = new HNXTCPClient(mockResponseInterface.Object);
            Mock<MyStream> mockMyStream = new Mock<MyStream>();
            HNXClient.c_CurrentConnected = mockMyStream.Object;
            HNXClient.__ClientStatus = enumClientStatus.DATA_TRANSFER;
            mockMyStream.Reset();
            //Act

            HNXClient.Logout();

            mockMyStream.Verify(helper => helper.WriteByte(It.IsAny<byte[]>()), Times.AtLeastOnce);
        }

        [TestMethod]
        [DataRow("0", "")]
        [DataRow("1", "")]
        [DataRow("2", "TABLE")]
        [DataRow("3", "XDCR12101")]
        public void TestSendTradingSessionRequest(string tradingCode, string tradingName)
        {
            var mockResponseInterface = new Mock<IResponseInterface>();
            HNXTCPClient HNXClient = new HNXTCPClient(mockResponseInterface.Object);
            Mock<MyStream> mockMyStream = new Mock<MyStream>();
            HNXClient.c_CurrentConnected = mockMyStream.Object;
            HNXClient.__ClientStatus = enumClientStatus.DATA_TRANSFER;
            mockMyStream.Reset();
            //Act

            HNXClient.SendTradingSessionRequest(tradingCode, tradingName);

            mockMyStream.Verify(helper => helper.WriteByte(It.IsAny<byte[]>()), Times.AtLeastOnce);
        }

        [TestMethod]
        [DataRow("0", "")]
        [DataRow("3", "XDCR12101")]
        public void TestSendSecurityStatusRequest(string tradingCode, string symbol)
        {
            var mockResponseInterface = new Mock<IResponseInterface>();
            HNXTCPClient HNXClient = new HNXTCPClient(mockResponseInterface.Object);
            Mock<MyStream> mockMyStream = new Mock<MyStream>();
            HNXClient.c_CurrentConnected = mockMyStream.Object;
            HNXClient.__ClientStatus = enumClientStatus.DATA_TRANSFER;
            mockMyStream.Reset();
            //Act

            HNXClient.SendSecurityStatusRequest(tradingCode, symbol);

            mockMyStream.Verify(helper => helper.WriteByte(It.IsAny<byte[]>()), Times.AtLeastOnce);
        }

        [TestMethod]
        [DataRow("bacnd", "1234", "1234")]
        public void TestSendUserRequest(string userName, string oldPass, string newPass)
        {
            var mockResponseInterface = new Mock<IResponseInterface>();
            HNXTCPClient HNXClient = new HNXTCPClient(mockResponseInterface.Object);
            Mock<MyStream> mockMyStream = new Mock<MyStream>();
            HNXClient.c_CurrentConnected = mockMyStream.Object;
            HNXClient.__ClientStatus = enumClientStatus.DATA_TRANSFER;
            mockMyStream.Reset();
            //Act

            HNXClient.SendUserRequest(userName, oldPass, newPass);

            mockMyStream.Verify(helper => helper.WriteByte(It.IsAny<byte[]>()), Times.AtLeastOnce);
        }

        #endregion Test xử lý gửi


        [TestMethod]
        public void TestGetInfo()
        {
            var mockResponseInterface = new Mock<IResponseInterface>();
            HNXTCPClient HNXClient = new HNXTCPClient(mockResponseInterface.Object);

            int Seq = HNXClient.Seq();
            int LastProcessSeq = HNXClient.LastSeqProcess();
            Assert.IsTrue(true);
        }

		[TestMethod]
		public void HNXRecover_Test()
		{
			var mockResponseInterface = new Mock<IResponseInterface>();
			HNXTCPClient hnxTCPClient = new HNXTCPClient(mockResponseInterface.Object);

            var gLastMapSeqProcess = hnxTCPClient.LastMapSeqProcess;
            //
            hnxTCPClient.Recovery();
            hnxTCPClient.Dispose();


			Assert.IsTrue(true);
		}
	}
}