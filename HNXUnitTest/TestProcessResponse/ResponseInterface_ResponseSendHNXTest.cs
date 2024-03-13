using BusinessProcessResponse;
using CommonLib;
using HNX.FIXMessage;
using KafkaInterface;
using LocalMemory;
using Moq;

namespace HNXUnitTest
{
    [TestClass]
    public sealed class ResponseInterface_ResponseSendHNXTest
    {
        private MessageFactoryFIX messageFactoryFIX = new MessageFactoryFIX();
        private static bool IsAdd = false;

        [TestInitialize]
        public void InitEnviromentTest()
        {
            InitObject.ReadConfigTest();
            if (!IsAdd)
            {
                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "BAC_TEST_5",
                    SeqNum = 2000,
                    ClientID = "043C111111",
                }); 
                //
                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "ed57d060-69a0-4946-a6e2-9cdd3338bf81",
                    SeqNum = 2001,
                    ClientID = "",
                });
                //
                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "540a8b48-b4e9-4618-ba58-5385263a0db9",
                    SeqNum = 2002,
                    ClientID = "",
                });
                //
                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "a95bf5cb-1dc7-4eb5-a8c2-e00544fc715a",
                    SeqNum = 2003,
                    ClientID = "",
                });
                //
                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "baeefae3-830b-4d77-ac70-26b2c440d63e",
                    SeqNum = 2004,
                    ClientID = "",
                });
                //
                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "9e4b4f8f-84c8-4b5c-a4c6-570e867fe441",
                    SeqNum = 2005,
                    ClientID = "",
                });
                //
                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "051d67f3-3067-4163-b8be-e2606d948bc4",
                    SeqNum = 2006,
                    ClientID = "",
                });
                //
                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "045d6ffc-1676-4ecd-9de3-1072b752e575",
                    SeqNum = 2007,
                    ClientID = "",
                });  
                //
                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "3855fc73-3ee9-4aa8-8c48-d09a0306f408",
                    SeqNum = 2008,
                    ClientID = "",
                });
                //
                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "f50e7e90-5998-426c-be15-a5a9deadc8ac",
                    SeqNum = 2009,
                    ClientID = "",
                });  //
                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "f848ce91-9ada-4042-9c2a-8bec83b487ff",
                    SeqNum = 2010,
                    ClientID = "",
                });
            }
        }

        #region Thông báo đã gửi lệnh lên sở cho kafka

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=205\u000135=s\u000149=003.01GW\u000156=HNX\u000134=49\u0001369=168\u000152=20230825-07:52:15\u000158=DAT BCGD\u000111=ed57d060-69a0-4946-a6e2-9cdd3338bf81\u00011=003C111111\u00012=003CX00002\u000155=XDCR12101\u000154=2\u000138=10\u0001448=003\u0001449=003\u0001548=\u0001549=1\u000140=R\u0001640=10\u00016464=0\u000164=20230810\u0001168=20230810\u00016363=1\u000110=000\u0001")]//Thông báo gửi đặt BCGD
        [DataRow("8=FIX.4.4\u00019=238\u000135=s\u000149=003.01GW\u000156=HNX\u000134=50\u0001369=169\u000152=20230825-07:52:18\u000158=\u000111=ed57d060-69a0-4946-a6e2-9cdd3338bf81\u00011=043C111111\u00012=003C111111\u000155=XDCR12101\u000154=1\u000138=1000\u0001448=043\u0001449=003\u0001548=XDCR12101-2308100000019\u0001549=3\u000140=R\u0001640=1000\u00016464=0\u000164=20230810\u0001168=20230810\u00016363=1\u000110=000\u0001")]//Thông báo gửi xác nhận BCGD
        public void TestResponseGateSend2HNX(string Message)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            FIXMessageBase messageBase = messageFactoryFIX.Parse(Message);
            // Act
            _testKafkaInterface.ResponseGateSend2HNX(messageBase);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=124\u000135=Z\u000149=003.01GW\u000156=HNX\u000134=48\u0001369=167\u000152=20230825-07:52:12\u000158=HUY\u000111=540a8b48-b4e9-4618-ba58-5385263a0db9\u0001298=4\u0001171=XDCR12101-2308100000014\u000140=S\u000155=XDCR12101\u000110=000\u0001")] //  ClOrdID(11) tôn tại trong memory
        [DataRow("8=FIX.4.4\u00019=124\u000135=Z\u000149=003.01GW\u000156=HNX\u000134=48\u0001369=167\u000152=20230825-07:52:12\u000158=HUY\u000111=540a8b48-b4e9-4618-ba58-5385263a0db9\u0001298=4\u0001171=XDCR12101-2308100000014\u000140=S\u000155=XDCR12101\u000110=000\u0001")] // Thông báo gửi hủy TTĐT
        public void ResponseSend2HNX_35Z_MessageQuoteCancel_TEST(string Message)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            FIXMessageBase messageBase = messageFactoryFIX.Parse(Message);
            // Act
            _testKafkaInterface.ResponseGateSend2HNX(messageBase);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=189\u000135=R\u000149=003.01GW\u000156=HNX\u000134=47\u0001369=166\u000152=20230825-07:52:09\u000158=SỬA\u000111=a95bf5cb-1dc7-4eb5-a8c2-e00544fc715a\u0001644=XDCR12101-2308100000013\u000140=S\u00011=003CX00002\u000155=XDCR12101\u000154=1\u000138=8\u0001640=1000\u00016464=0\u000164=20230810\u0001513=0\u00016363=2\u00011111=0\u000110=000")]
        [DataRow("8=FIX.4.4\u00019=189\u000135=R\u000149=003.01GW\u000156=HNX\u000134=47\u0001369=166\u000152=20230825-07:52:09\u000158=SỬA\u000111=a95bf5cb-1dc7-4eb5-a8c2-e00544fc715a\u0001644=XDCR12101-2308100000013\u000140=S\u00011=003CX00002\u000155=XDCR12101\u000154=2\u000138=8\u0001640=1000\u00016464=0\u000164=20230810\u0001513=0\u00016363=2\u00011111=0\u000110=000")]
        public void ResponseSend2HNX_35R_MessageQuoteRequest_TEST(string Message)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            FIXMessageBase messageBase = messageFactoryFIX.Parse(Message);
            // Act
            _testKafkaInterface.ResponseGateSend2HNX(messageBase);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=198\u000135=AJ\u000149=003.01GW\u000156=HNX\u000134=46\u0001369=165\u000152=20230825-07:52:05\u000158=CHAP NHAN TTĐT\u000111=baeefae3-830b-4d77-ac70-26b2c440d63e\u0001693=XDCR12101-2308100000011\u0001694=1\u00011=003CX00002\u000140=S\u000155=XDCR12101\u000154=1\u000138=1000\u0001640=10000\u00016464=0\u000164=20230810\u00016363=1\u000110=000")]//Thông báo gửi xác nhận TTĐT
        [DataRow("8=FIX.4.4\u00019=198\u000135=AJ\u000149=003.01GW\u000156=HNX\u000134=46\u0001369=165\u000152=20230825-07:52:05\u000158=CHAP NHAN TTĐT\u000111=baeefae3-830b-4d77-ac70-26b2c440d63e\u0001693=XDCR12101-2308100000011\u0001694=1\u00011=003CX00002\u000140=S\u000155=XDCR12101\u000154=2\u000138=1000\u0001640=10000\u00016464=0\u000164=20230810\u00016363=1\u000110=000")]
        public void ResponseSend2HNX_35AJ_MessageQuoteResponse_TEST(string Message)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            FIXMessageBase messageBase = messageFactoryFIX.Parse(Message);
            // Act
            _testKafkaInterface.ResponseGateSend2HNX(messageBase);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=259\u000135=t\u000149=018.01GW\u000156=HNX\u000134=6\u0001369=0\u000152=20230811-10:17:42\u000111=9e4b4f8f-84c8-4b5c-a4c6-570e867fe441\u00011=018C111111\u00012=018CX00001\u000155=BABR12001\u000154=1\u000138=5\u0001448=018\u0001449=018\u000137=\u0001551=BABR12001-2307170000004\u0001549=2\u000140=R\u0001640=50000\u00016464=4000000\u000164=20230712\u0001168=20230712\u00016363=2\u000110=178\u0001'")] //Gửi yêu cầu sửa TTDT đã thực hiện cùng cty
        [DataRow("8=FIX.4.4\u00019=248\u000135=t\u000149=028.01GW\u000156=HNX\u000134=4\u0001369=0\u000152=20230812-02:58:58\u000111=9e4b4f8f-84c8-4b5c-a4c6-570e867fe441\u00011=028C111111\u00012=018C111111\u000155=BABR12001\u000154=2\u000138=10\u0001448=028\u0001449=018\u000137=\u0001551=BABR12001-2307180000001\u0001549=2\u000140=R\u0001640=300000\u000164=20230713\u0001168=20230713\u00016363=2\u000110=239\u0001")] // Gửi lệnh sửa TTDT đã thực hiện khác cty - bên gửi nhận
        [DataRow("8=FIX.4.4\u00019=205\u000135=t\u000149=018.01GW\u000156=HNX\u000134=5\u0001369=0\u000152=20230812-02:59:43\u000111=9e4b4f8f-84c8-4b5c-a4c6-570e867fe441\u00011=\u00012=018C111111\u000155=BABR12001\u000154=1\u000138=0\u0001448=\u0001449=018\u000137=\u0001551=BABR12001-2307180000002\u0001549=3\u000140=R\u0001640=0\u000164=00010101\u0001168=00010101\u00016363=2\u000110=201\u0001")]// Chấp nhận sửa lệnh TTDT - khác cty, bên đối ứng chấp nhận
        public void ResponseSend2HNX_35t_CrossOrderCancelReplaceRequest_TEST(string Message)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            FIXMessageBase messageBase = messageFactoryFIX.Parse(Message);
            // Act
            _testKafkaInterface.ResponseGateSend2HNX(messageBase);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=158\u000135=u\u000149=018.01GW\u000156=HNX\u000134=12\u0001369=0\u000152=20230812-03:47:25\u000111=051d67f3-3067-4163-b8be-e2606d948bc4\u000137=\u0001551=BABR12001-2307180000006\u0001549=2\u000155=BABR12001\u000154=1\u000140=R\u000110=216\u0001")]//Hủy cùng cty
        [DataRow("8=FIX.4.4\u00019=157\u000135=u\u000149=028.01GW\u000156=HNX\u000134=2\u0001369=0\u000152=20230814-03:40:46\u000111=051d67f3-3067-4163-b8be-e2606d948bc4\u000137=\u0001551=AAAR12101-2307240000001\u0001549=2\u000155=AAAR12101\u000154=2\u000140=R\u000110=139\u0001")] //gửi yêu cầu hủy khác công ty -
        [DataRow("8=FIX.4.4\u00019=133\u000135=u\u000149=018.01GW\u000156=HNX\u000134=2\u0001369=0\u000152=20230814-03:41:08\u000111=051d67f3-3067-4163-b8be-e2606d948bc4\u000137=\u0001551=AAAR12101-2307240000002\u0001549=3\u000155=AAAR12101\u000154=1\u000140=R\u000110=209\u0001")] //Phản hồi yêu cầu hủy khác công ty
        [DataRow("8=FIX.4.4\u00019=158\u000135=u\u000149=018.01GW\u000156=HNX\u000134=12\u0001369=0\u000152=20230812-03:47:25\u000111=051d67f3-3067-4163-b8be-e2606d948bc4\u000137=\u0001551=BABR12001-2307180000006\u0001549=2\u000155=BABR12001\u000154=1\u000140=R\u000110=216\u0001")]//Hủy cùng cty
        public void ResponseSend2HNX_35u_CrossOrderCancelRequest_TEST(string Message)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            FIXMessageBase messageBase = messageFactoryFIX.Parse(Message);
            // Act
            _testKafkaInterface.ResponseGateSend2HNX(messageBase);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=166\u000135=S\u000149=003.01GW\u000156=HNX\u000134=45\u0001369=164\u000152=20230825-07:52:02\u000158=DAT TTĐT\u000111=18\u00011=003C111111\u000155=XDCR12101\u000154=1\u000138=5\u000140=S\u0001640=1000\u00016464=0\u000164=20230810\u0001513=0\u00011111=0\u00016363=1\u000110=000")]
        [DataRow("8=FIX.4.4\u00019=166\u000135=S\u000149=003.01GW\u000156=HNX\u000134=45\u0001369=164\u000152=20230825-07:52:02\u000158=DAT TTĐT\u000111=18\u00011=003C111111\u000155=XDCR12101\u000154=2\u000138=5\u000140=S\u0001640=1000\u00016464=0\u000164=20230810\u0001513=0\u00011111=0\u00016363=1\u000110=000")]
        public void ResponseSend2HNX_35S_MessageQuote_TEST(string Message)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            FIXMessageBase messageBase = messageFactoryFIX.Parse(Message);
            // Act
            _testKafkaInterface.ResponseGateSend2HNX(messageBase);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=185\u000135=N01\u000149=050.02GW\u000156=HNX\u000134=6\u0001369=52\u000152=20231025-03:11:16\u000111=045d6ffc-1676-4ecd-9de3-1072b752e575\u000155=XDCR12101\u0001537=1\u000140=U\u000154=1\u000138=100000\u0001168=20231116\u00016363=2\u000164=20231116\u0001193=20231117\u0001917=20231117\u0001226=1\u0001513=019\u0001644=\u000110=000\u0001")] // 537= 1 side 54=1 Buy
        [DataRow("8=FIX.4.4\u00019=185\u000135=N01\u000149=050.02GW\u000156=HNX\u000134=6\u0001369=52\u000152=20231025-03:11:16\u000111=045d6ffc-1676-4ecd-9de3-1072b752e575\u000155=XDCR12101\u0001537=1\u000140=U\u000154=2\u000138=100000\u0001168=20231116\u00016363=2\u000164=20231116\u0001193=20231117\u0001917=20231117\u0001226=1\u0001513=019\u0001644=TEST\u000110=000\u0001")] // 537= 1 side 54=2 sell
        [DataRow("8=FIX.4.4\u00019=185\u000135=N01\u000149=050.02GW\u000156=HNX\u000134=6\u0001369=52\u000152=20231025-03:11:16\u000111=045d6ffc-1676-4ecd-9de3-1072b752e575\u000155=XDCR12101\u0001537=1\u000140=U\u000154=3\u000138=100000\u0001168=20231116\u00016363=2\u000164=20231116\u0001193=20231117\u0001917=20231117\u0001226=1\u0001513=019\u0001644=TEST\u000110=000\u000158=UNIT TEST\u0001")] // 537= 1 side 54=3  side k ton tai
        [DataRow("8=FIX.4.4\u00019=185\u000135=N01\u000149=050.02GW\u000156=HNX\u000134=6\u0001369=52\u000152=20231025-03:11:16\u000111=045d6ffc-1676-4ecd-9de3-1072b752e575\u000155=XDCR12101\u0001537=2\u000140=U\u000154=1\u000138=100000\u0001168=20231116\u00016363=2\u000164=20231116\u0001193=20231117\u0001917=20231117\u0001226=1\u0001513=019\u0001644=\u000110=000\u0001")] // 537= 2
        [DataRow("8=FIX.4.4\u00019=185\u000135=N01\u000149=050.02GW\u000156=HNX\u000134=6\u0001369=52\u000152=20231025-03:11:16\u000111=045d6ffc-1676-4ecd-9de3-1072b752e575\u000155=XDCR12101\u0001537=3\u000140=U\u000154=1\u000138=100000\u0001168=20231116\u00016363=2\u000164=20231116\u0001193=20231117\u0001917=20231117\u0001226=1\u0001513=019\u0001644=\u000110=000\u0001")] // 537= 3
        [DataRow("8=FIX.4.4\u00019=185\u000135=N01\u000149=050.02GW\u000156=HNX\u000134=6\u0001369=52\u000152=20231025-03:11:16\u000111=045d6ffc-1676-4ecd-9de3-1072b752e575\u000155=XDCR12101\u0001537=4\u000140=U\u000154=1\u000138=100000\u0001168=20231116\u00016363=2\u000164=20231116\u0001193=20231117\u0001917=20231117\u0001226=1\u0001513=019\u0001644=\u000110=000\u0001")] // 537= 4
        [DataRow("8=FIX.4.4\u00019=185\u000135=N01\u000149=050.02GW\u000156=HNX\u000134=6\u0001369=52\u000152=20231025-03:11:16\u000111=045d6ffc-1676-4ecd-9de3-1072b752e575\u000155=XDCR12101\u0001537=4\u000140=U\u000154=1\u000138=100000\u0001168=20231116\u00016363=2\u000164=20231116\u0001193=20231117\u0001917=20231117\u0001226=1\u0001513=019\u0001644=\u000110=000\u0001")] // 11=BAC_TEST_5 co trong memory
        public void ResponseSend2HNX_35N01_NewInquiryRepos_TEST(string Message)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            FIXMessageBase messageBase = messageFactoryFIX.Parse(Message);
            // Act
            _testKafkaInterface.ResponseGateSend2HNX(messageBase);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=281\u000135=N03\u000149=043.01GW\u000156=HNX\u000134=34\u0001369=149\u000152=20231107-07:53:18\u000158=cancel\u000111=3855fc73-3ee9-4aa8-8c48-d09a0306f408\u0001644=REPOSPT-2312060000022\u0001537=3\u000140=U\u000154=0\u00011=\u0001168=00010101\u00016363=0\u000164=00010101\u0001193=00010101\u0001917=00010101\u0001226=0\u0001227=0\u0001552=0\u00015522=0\u000155=\u000138=0\u000144=0\u00012260=0\u00016464=0\u00016465=0\u00012261=0")]
        public void ResponseSend2HNX_35N03_MessageReposFirm_TEST(string Message)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            FIXMessageBase messageBase = messageFactoryFIX.Parse(Message);

            // Act
            MessageReposFirm newFirmRepos = (MessageReposFirm)messageBase;
            ReposSide _reposSide = new ReposSide();
            _reposSide.NumSide = 1;
            _reposSide.Symbol = "XDCR12101";
            _reposSide.OrderQty = 100;
            _reposSide.Price = 1000;
            _reposSide.HedgeRate = 1;
            //
            newFirmRepos.RepoSideList.Add(_reposSide);
            //
            FIXMessageBase messageBase2 = newFirmRepos;

            // Act
            _testKafkaInterface.ResponseGateSend2HNX(messageBase2);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35= ME
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=335\u000135=ME\u000149=043.01GW\u000156=HNX\u000134=49\u0001369=167\u000152=20231107-08:35:00\u000158=THM\u000111=0ace0fb1-702d-4f76-9799-a0579c7d6d42\u0001198=XDCR12101-2312060000007\u0001563=1\u000140=T\u000154=2\u00011=043C111111\u00012=015C111111\u0001448=043\u0001449=015\u0001168=20231206\u00016363=1\u000164=20231206\u0001193=20231208\u0001917=20231208\u0001226=3\u0001227=1.4\u0001552=1\u00015522=1\u000155=XDCR12101\u000138=10\u000144=8000\u00012260=3.2\u00012261=0\u00016464=0\u00016465=0\u000110=000\u0001")]
        public void ResponseSend2HNX_35ME_MessageReposBCGDModify_TEST(string Message)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            // Act
            FIXMessageBase messageBase = messageFactoryFIX.Parse(Message);
            //
            MessageReposBCGDModify newFirmRepos = (MessageReposBCGDModify)messageBase;
            ReposSide _reposSide = new ReposSide();
            _reposSide.NumSide = 1;
            _reposSide.Symbol = "SSI";
            _reposSide.OrderQty = 100;
            _reposSide.Price = 1000;
            _reposSide.HedgeRate = 1;
            //
            newFirmRepos.RepoSideList.Add(_reposSide);
            //
            FIXMessageBase messageBase2 = newFirmRepos;
            _testKafkaInterface.ResponseGateSend2HNX(messageBase2);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35= MC
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=150\u000135=MC\u000149=043.01GW\u000156=HNX\u000134=51\u0001369=169\u000152=20231107-08:38:22\u000158=th\u000111=2c4055e1-9a1d-4073-ad70-e043d3a67dae\u0001198=XDCR12101-2312060000007\u0001563=1\u000140=T\u000154=2\u000110=000\u0001")]
        public void ResponseSend2HNX_35MC_MessageReposBCGDCancel_Test(string p_Msg)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act
            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
            _testKafkaInterface.ResponseGateSend2HNX(messageBase);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=282\u000135=MA\u000149=043.01GW\u000156=HNX\u000134=38\u0001369=155\u000152=20231107-08:08:16\u000158=aaa\u000111=TEST_40\u0001198=\u0001563=1\u000140=T\u000154=1\u00011=043CX00002\u00012=043C111111\u0001448=043\u0001449=043\u0001168=20231206\u00016363=1\u000164=20231206\u0001193=20231208\u0001917=20231208\u0001226=2\u0001227=1.3\u0001552=1\u00015522=1\u000155=XDCR12101\u000138=100\u000144=9000\u00012260=1\u00012261=0\u00016464=0\u00016465=0\u0001")]
        [DataRow("8=FIX.4.4\u00019=282\u000135=MA\u000149=043.01GW\u000156=HNX\u000134=39\u0001369=156\u000152=20231107-08:10:00\u000158=NNN\u000111=TEST_41\u0001198=\u0001563=1\u000140=T\u000154=2\u00011=043C111111\u00012=015C111111\u0001448=043\u0001449=015\u0001168=20231206\u00016363=1\u000164=20231206\u0001193=20231208\u0001917=20231208\u0001226=2\u0001227=1.3\u0001552=1\u00015522=1\u000155=XDCR12101\u000138=100\u000144=9000\u00012260=1\u00012261=0\u00016464=0\u00016465=0\u000110=000\u0001")]
        public void ResponseSend2HNX_35MA_MessageReposBCGD_TEST(string Message)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            FIXMessageBase messageBase = messageFactoryFIX.Parse(Message);
            // Act
            _testKafkaInterface.ResponseGateSend2HNX(messageBase);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=221\u000135=N05\u000149=043.01GW\u000156=HNX\u000134=8\u0001369=76\u000152=20231226-08:29:50\u000158=accept\u000111=6715603b-96f1-42dc-bb1e-fe3b7ffa8cc0\u0001644=REPOSPT-2402090000006\u0001537=1\u000140=U\u00011=043CX00002\u0001227=2\u0001552=1\u00015522=1\u000155=XDCR12101\u000138=100\u000144=10000\u00012260=2\u00012261=0\u000110=000")]
        public void ResponseSend2HNX_35N05_MessageReposFirmAccept_TEST(string Message)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            FIXMessageBase messageBase = messageFactoryFIX.Parse(Message);
            // Act
            _testKafkaInterface.ResponseGateSend2HNX(messageBase);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35= D
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=147\u000135=D\u000149=043.01GW\u000156=HNX\u000134=23\u0001369=99\u000152=20231115-03:30:29\u000158=test\u000111=f50e7e90-5998-426c-be15-a5a9deadc8ac\u00011=043C111111\u000155=AAAR12101\u000154=1\u000140=2\u000138=100\u0001192=0\u0001640=10000\u000144=0\u0001440=0\u000110=000\u0001")]
        [DataRow("8=FIX.4.4\u00019=146\u000135=D\u000149=043.01GW\u000156=HNX\u000134=6\u0001369=92\u000152=20231212-08:13:50\u000158=test\u000111=f50e7e90-5998-426c-be15-a5a9deadc8ac\u00011=043CX00002\u000155=AAAR12101\u000154=2\u000140=2\u000138=200\u0001192=0\u0001640=10000\u000144=0\u0001440=0\u000110=000\u0001")]
        public void ResponseSend2HNX_35D_MessageNewOrder_Test(string p_Msg)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act
            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
            _testKafkaInterface.ResponseGateSend2HNX(messageBase);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35= G
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=179\u000135=G\u000149=043.01GW\u000156=HNX\u000134=18\u0001369=105\u000152=20231212-08:27:55\u000111=f848ce91-9ada-4042-9c2a-8bec83b487ff\u000141=AAAR12101-2401150000003\u00011=043CX00002\u000155=AAAR12101\u000138=800\u00012238=1000\u0001640=90000\u000110=000\u0001")]
        [DataRow("8=FIX.4.4\u00019=180\u000135=G\u000149=043.01GW\u000156=HNX\u000134=13\u0001369=100\u000152=20231212-08:25:26\u000111=f848ce91-9ada-4042-9c2a-8bec83b487ff\u000141=AAAR12101-2401150000003\u00011=043C111111\u000155=XDCR12101\u000138=2000\u00012238=1000\u0001640=90000\u000110=000\u0001")]
        public void ResponseSend2HNX_35G_MessageNewOrder_Test(string p_Msg)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act
            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
            _testKafkaInterface.ResponseGateSend2HNX(messageBase);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35= F
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=149\u000135=F\u000149=043.01GW\u000156=HNX\u000134=21\u0001369=108\u000152=20231212-08:31:16\u000158=cancel\u000111=c27ff50d-b73d-414a-8325-6cddde0f0106\u000141=AAAR12101-2401150000005\u000155=AAAR12101\u000110=000\u0001")]
        [DataRow("8=FIX.4.4\u00019=149\u000135=F\u000149=043.01GW\u000156=HNX\u000134=20\u0001369=107\u000152=20231212-08:30:56\u000158=cancel\u000111=851c69a6-6bea-4c3c-a2a3-d55a19de1210\u000141=AAAR12101-2401150000005\u000155=XDCR12101\u000110=000\u0001")]
        public void ResponseSend2HNX_35F_MessageCancelOrder_Test(string p_Msg)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act
            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
            _testKafkaInterface.ResponseGateSend2HNX(messageBase);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        #endregion Thông báo đã gửi lệnh lên sở cho kafka
    }
}