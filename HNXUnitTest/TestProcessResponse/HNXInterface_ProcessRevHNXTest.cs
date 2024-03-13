using CommonLib;
using HNX.FIXMessage;
using HNXInterface;
using KafkaInterface;
using Moq;
using BusinessProcessResponse;
using LocalMemory;

namespace HNXUnitTest
{
    [TestClass]
    public class HNXInterface_ProcessRevHNXTest
    {
        private MessageFactoryFIX c_FIXMsgFactory = new MessageFactoryFIX();
        private FIXMessageBase fMsgBase = new FIXMessageBase();

        private static bool IsAdd = false;

        [TestInitialize]
        public void InitEnviromentTest()
        {
            InitObject.ReadConfigTest();
            if (!IsAdd)
            {
                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "59960aad-1812-4307-bed6-459ee5cda3e2",
                    SeqNum = 1001,
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ExchangeID = "BABR12001-2312180000002",
                    SeqNum = 1002,
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "",
                    ExchangeID = "XDCR12101-1812230000008",
                    SeqNum = 1003,
                    Side = "B",
                    ClientID = "",
                });
                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "",
                    Symbol = "XDCR12101",
                    ExchangeID = "XDCR12101-2318120000010",
                    SeqNum = 1004,
                    Side = "S",
                    ClientID = "",
                });
            }
            IsAdd = true;
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=197\u000135=AI\u000149=HNX\u000156=003.01GW\u000134=36\u0001369=7\u000152=20230831-08:38:12\u000111=18\u00014488=003\u0001537=1\u00011=003C111111\u000155=XDCR12101\u000154=1\u000140=S\u000138=5\u0001131=\u0001171=XDCR12101-1812230000008\u0001640=1000\u00016464=5000\u00016363=1\u000164=20230823\u0001513=0\u000110=190\u0001")]//Nhận phản hồi AI, 573 =1 khi đặt TTDT
        [DataRow("8=FIX.4.4\u00019=223\u000135=AI\u000149=HNX\u000156=003.01GW\u000134=41\u0001369=11\u000152=20230831-08:52:16\u000111=21\u00014488=003\u0001537=2\u00011=003C111111\u000155=XDCR12101\u000154=1\u000140=S\u000138=9\u0001131=XDCR12101-2308230000011\u0001171=XDCR12101-2308230000012\u0001640=1000\u00016464=9000\u00016363=2\u000164=20230823\u0001513=003\u000110=020\u0001")]//Nhận phản hồi AI, 537=2 khi sửa TTĐT
        [DataRow("8=FIX.4.4\u00019=224\u000135=AI\u000149=HNX\u000156=003.01GW\u000134=43\u0001369=12\u000152=20230831-08:52:23\u000111=102\u00014488=003\u0001537=3\u00011=003C111111\u000155=XDCR12101\u000154=1\u000140=S\u000138=9\u0001131=XDCR12101-2308230000011\u0001171=XDCR12101-2308230000013\u0001640=1000\u00016464=9000\u00016363=2\u000164=20230823\u0001513=003\u000110=072\u0001")]//Nhận phản hồi AI, 537=3 Khi hủy TTĐT
        public void TestProcessRevHNX_MsgAI(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            var mockResponseInterface = new Mock<IResponseInterface>();
            ProcessRevHNX processRevHNX = new ProcessRevHNX(mockResponseInterface.Object);
            //Act
            fMsgBase = c_FIXMsgFactory.Parse(msgRaw);
            processRevHNX.ProcessHNXMessage(fMsgBase);
            //Assert
            mockResponseInterface.Verify(helper => helper.HNXSendQuoteStatusReport(It.IsAny<MessageQuoteSatusReport>()), Times.Once);
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=222\u000135=8\u000149=HNX\u000156=003.01GW\u000134=39\u0001369=9\u000152=20230831-08:51:00\u0001150=3\u000139=2\u000111=23\u000141=XDCR12101-2318120000010\u0001526=XDCR12101-2318120000010\u000137=XDCR12101-2308230000002\u000132=10\u000131=10\u000117=XDCR12101-2308230000002\u000155=XDCR12101\u000154=2\u00016464=100\u000110=158\u0001")] //Khớp
        [DataRow("8=FIX.4.4\u00019=233\u000135=8\u000149=HNX\u000156=018.01GW\u000134=32\u0001369=0\u000152=20230812-02:59:54\u0001150=5\u000139=3\u000111=eac6a915-79db-49a7-8a4b-4fe3c701aae6\u000141=BABR12001-2307180000001\u000137=BABR12001-2312180000002\u00011=018C111111\u000155=BABR12001\u000154=1\u000140=R\u000132=10\u000131=300000\u0001151=0\u00016464=3000000\u000110=185\u0001")]//Sửa
        [DataRow("8=FIX.4.4\u00019=191\u000135=8\u000149=HNX\u000156=018.01GW\u000134=27\u0001369=0\u000152=20230814-03:41:09\u0001150=4\u000139=A\u000111=CANBCGDTH982\u000141=AAAR12101-2307240000001\u000137=AAAR12101-2307240000002\u0001151=5\u00011=018C111111\u000155=AAAR12101\u000154=1\u000140=R\u000144=3000000\u000110=168\u0001")]//Hủy

        public void TestProcessHNX_Send_Msg_8(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            var mockResponseInterface = new Mock<IResponseInterface>();
            ProcessRevHNX processRevHNX = new ProcessRevHNX(mockResponseInterface.Object);
            //Act
            MessageExecutionReport exReport = (MessageExecutionReport)c_FIXMsgFactory.Parse(msgRaw);
            processRevHNX.ProcessHNXMessage(exReport);
            //Assert
            switch (exReport.ExecType)
            {
                case ExecutionReportType.ER_ExecOrder_3:
                    mockResponseInterface.Verify(helper => helper.HNXResponseExcQuote(It.IsAny<MessageER_ExecOrder>()), Times.Once);
                    break;

                case ExecutionReportType.ER_CancelOrder_4:
                    mockResponseInterface.Verify(helper => helper.HNXResponse_EROrderCancel(It.IsAny<MessageER_OrderCancel>()), Times.Once);
                    break;

                case ExecutionReportType.ER_ReplaceOrder_5:
                    mockResponseInterface.Verify(helper => helper.HNXResponse_ExecReplace(It.IsAny<MessageER_OrderReplace>()), Times.Once);
                    break;
            }
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=257\u000135=s\u000149=HNX\u000156=050.02GW\u000134=5\u0001369=1\u000152=20230816-06:51:42\u000111=920055be-84e7-4259-91d4-35a64880d154\u00011=050C111111\u00012=003CX00002\u000155=XDCR12101\u000154=2\u000138=1000\u0001448=050\u0001449=003\u0001548=XDCR12101-2307280000002\u0001549=1\u000140=R\u0001640=1000\u00016464=1000000\u000164=20230728\u0001168=20230728\u00016363=1\u000110=240\u0001")]
        public void TestProcessHNX_Send_Msg_s(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            var mockResponseInterface = new Mock<IResponseInterface>();
            ProcessRevHNX processRevHNX = new ProcessRevHNX(mockResponseInterface.Object);
            //Act
            MessageNewOrderCross exReport = (MessageNewOrderCross)c_FIXMsgFactory.Parse(msgRaw);
            processRevHNX.ProcessHNXMessage(exReport);
            //Assert
            mockResponseInterface.Verify(helper => helper.HNXSendOrderCross(It.IsAny<MessageNewOrderCross>()), Times.Once);
        }

        //HNX gửi từ chối
        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=135\u000135=3\u000149=HNX\u000156=003.01GW\u000134=35\u0001369=6\u000152=20230831-08:37:50\u000158=NG?Y THANH TO?N PH?I L?N H?N NG?Y B?T ??U GIAO D?CH!\u0001372=S\u0001373=-34006\u000145=2\u000110=016\u0001")]
        public void TestHNX_SendReject_Success(string p_message)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            var mockResponseInterface = new Mock<IResponseInterface>();
            ProcessRevHNX processRevHNX = new ProcessRevHNX(mockResponseInterface.Object);
            //Act
            fMsgBase = c_FIXMsgFactory.Parse(p_message);
            processRevHNX.ProcessHNXMessage(fMsgBase);
            //Assert
            mockResponseInterface.Verify(helper => helper.HNXSendReject(It.IsAny<MessageReject>()), Times.Once);
        }

        //HNX gửi từ chối
        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=135\u000135=3\u000149=HNX\u000156=003.01GW\u000134=35\u0001369=6\u000152=20230831-08:37:50\u000158=NG?Y THANH TO?N PH?I L?N H?N NG?Y B?T ??U GIAO D?CH!\u0001372=D\u0001373=-34006\u000145=2\u000110=016\u0001")]
        public void TestHNX_SendReject_35D_Success(string p_message)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            var mockResponseInterface = new Mock<IResponseInterface>();
            ProcessRevHNX processRevHNX = new ProcessRevHNX(mockResponseInterface.Object);
            //Act
            fMsgBase = c_FIXMsgFactory.Parse(p_message);
            processRevHNX.ProcessHNXMessage(fMsgBase);
            //Assert
            mockResponseInterface.Verify(helper => helper.HNXSendReject(It.IsAny<MessageReject>()), Times.Once);
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=133\u000135=u\u000149=018.01GW\u000156=HNX\u000134=2\u0001369=0\u000152=20230814-03:41:08\u000111=CANBCGDTH982\u000137=\u0001551=AAAR12101-2307240000002\u0001549=3\u000155=AAAR12101\u000154=1\u000140=R\u000110=209\u0001")]
        public void TestResponseRecvFromHNX_Msg_u(string msgRaw)
        {
            //Arrange
            var mockResponseInterface = new Mock<IResponseInterface>();
            ProcessRevHNX processRevHNX = new ProcessRevHNX(mockResponseInterface.Object);
            //Act
            CrossOrderCancelRequest orderCrossCancel = (CrossOrderCancelRequest)c_FIXMsgFactory.Parse(msgRaw);
            processRevHNX.ProcessHNXMessage(orderCrossCancel);
            //Assert
            mockResponseInterface.Verify(helper => helper.HNXResponse_CrossOrderCancelRequest(It.IsAny<CrossOrderCancelRequest>()), Times.Once);
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=280\u000135=t\u000149=HNX\u000156=028.01GW\u000134=29\u0001369=0\u000152=20230812-02:58:57\u000111=df1a4e55-3b8e-4582-a75b-1aaa62c21254\u00011=028C111111\u00012=018C111111\u000155=BABR12001\u000154=2\u000138=10\u0001448=028\u0001449=018\u000137=BABR12001-2307180000001\u0001551=BABR12001-2307180000001\u0001549=2\u0001640=300000\u00016464=3000000\u000164=20230713\u0001168=20230713\u00016363=2\u000110=081\u0001")]
        public void TestResponseRecvFromHNX_Msg_t(string msgRaw)
        {
            //Arrange
            var mockResponseInterface = new Mock<IResponseInterface>();
            ProcessRevHNX processRevHNX = new ProcessRevHNX(mockResponseInterface.Object);
            //Act
            CrossOrderCancelReplaceRequest orderCrossReplace = (CrossOrderCancelReplaceRequest)c_FIXMsgFactory.Parse(msgRaw);
            processRevHNX.ProcessHNXMessage(orderCrossReplace);
            //Assert
            mockResponseInterface.Verify(helper => helper.HNXResponse_CrossOrderCancelReplace(It.IsAny<CrossOrderCancelReplaceRequest>()), Times.Once);
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=135\u000135=3\u000149=HNX\u000156=003.01GW\u000134=35\u0001369=6\u000152=20230831-08:37:50\u000158=NG?Y THANH TO?N PH?I L?N H?N NG?Y B?T ??U GIAO D?CH!\u0001372=S\u0001373=-34006\u000145=8\u000110=016\u0001")]
        [DataRow("8=FIX.4.4\u00019=135\u000135=3\u000149=HNX\u000156=003.01GW\u000134=35\u0001369=6\u000152=20230831-08:37:50\u000158=NG?Y THANH TO?N PH?I L?N H?N NG?Y B?T ??U GIAO D?CH!\u0001372=N01\u0001373=-34006\u000145=8\u000110=016\u0001")]
        [DataRow("8=FIX.4.4\u00019=84\u000135=3\u000149=HNX\u000156=043.01GW\u000134=147\u0001369=31\u000152=20231107-07:47:19\u0001372=N03\u0001373=-34039\u000145=8\u000110=027\u0001")]
        [DataRow("8=FIX.4.4\u00019=82\u000135=3\u000149=HNX\u000156=043.01GW\u000134=101\u0001369=13\u000152=20231212-08:26:35\u0001372=G\u0001373=-34000\u000145=13\u000110=146\u0001")]
        public void TestResponseRecvFromHNX_Msg(string p_message)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaHelper = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaHelper.Object);

            FIXMessageBase messageBase = c_FIXMsgFactory.Parse(p_message);

            // Act thông báo nhận từ chối
            _testKafkaInterface.HNXSendReject((MessageReject)messageBase);
            //Assert
            mockKafkaHelper.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

    }
}