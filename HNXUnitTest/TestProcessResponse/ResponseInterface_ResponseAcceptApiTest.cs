using BusinessProcessResponse;
using CommonLib;
using HNX.FIXMessage;
using KafkaInterface;
using Moq;
using System.Reflection;
using static CommonLib.CommonDataInCore;

namespace HNXUnitTest
{
    [TestClass]
    public sealed class ResponseInterface_ResponseAcceptApiTest
    {
        private MessageFactoryFIX messageFactoryFIX = new MessageFactoryFIX();
        private bool IsAdd = false;

        [TestInitialize]
        public void InitEnviromentTest()
        {
            InitObject.ReadConfigTest();
        }

        #region Gửi Phản hồi khi đặt lệnh API gửi sang kafka

        // Test 35= S
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=166\u000135=S\u000149=003.01GW\u000156=HNX\u000134=45\u0001369=164\u000152=20230825-07:52:02\u000158=DAT TTĐT\u000111=18\u00011=003C111111\u000155=XDCR12101\u000154=1\u000138=5\u000140=S\u0001640=1000\u00016464=0\u000164=20230810\u0001513=0\u00011111=0\u00016363=1\u000110=000")] //Đặt TTĐT 35= S with side = buy
        [DataRow("8=FIX.4.4\u00019=166\u000135=S\u000149=003.01GW\u000156=HNX\u000134=45\u0001369=164\u000152=20230825-07:52:02\u000158=DAT TTĐT\u000111=18\u00011=003C111111\u000155=XDCR12101\u000154=2\u000138=5\u000140=S\u0001640=1000\u00016464=0\u000164=20230810\u0001513=0\u00011111=0\u00016363=1\u000110=000")] //Đặt TTĐT 35= S with side = sell
        [DataRow("8=FIX.4.4\u00019=166\u000135=S\u000149=003.01GW\u000156=HNX\u000134=45\u0001369=164\u000152=20230825-07:52:02\u000158=\u000111=18\u00011=003C111111\u000155=XDCR12101\u000154=2\u000138=5\u000140=S\u0001640=1000\u00016464=0\u000164=20230810\u0001513=0\u00011111=0\u00016363=1\u000110=000")] //Đặt TTĐT 35= S with side = sell and Text = null
        public void ResponseApi2Kafka_35S_MessageQuote_Test(string p_Msg)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act Đặt TTĐT
            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
            _testKafkaInterface.ResponseApi2Kafka(messageBase, 0);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35=s
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=205\u000135=s\u000149=003.01GW\u000156=HNX\u000134=49\u0001369=168\u000152=20230825-07:52:15\u000158=DAT BCGD\u000111=11\u00011=003C111111\u00012=003CX00002\u000155=XDCR12101\u000154=2\u000138=10\u0001448=003\u0001449=003\u0001548=\u0001549=1\u000140=R\u0001640=10\u00016464=0\u000164=20230810\u0001168=20230810\u00016363=1\u000110=000\u0001")]
        [DataRow("8=FIX.4.4\u00019=238\u000135=s\u000149=003.01GW\u000156=HNX\u000134=50\u0001369=169\u000152=20230825-07:52:18\u000158=CHAP NHAN BCGD\u000111=55\u00011=043C111111\u00012=003C111111\u000155=XDCR12101\u000154=1\u000138=1000\u0001448=043\u0001449=003\u0001548=XDCR12101-2308100000019\u0001549=3\u000140=R\u0001640=1000\u00016464=0\u000164=20230810\u0001168=20230810\u00016363=1\u000110=000\u0001")]
        [DataRow("8=FIX.4.4\u00019=238\u000135=s\u000149=003.01GW\u000156=HNX\u000134=50\u0001369=169\u000152=20230825-07:52:18\u000158=\u000111=55\u00011=043C111111\u00012=003C111111\u000155=XDCR12101\u000154=1\u000138=1000\u0001448=043\u0001449=003\u0001548=XDCR12101-2308100000019\u0001549=3\u000140=R\u0001640=1000\u00016464=0\u000164=20230810\u0001168=20230810\u00016363=1\u000110=000\u0001")] // 58=null
        public void ResponseApi2Kafka_35s_MessageNewOrderCross_Test(string p_Msg)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act Đặt TTĐT
            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
            _testKafkaInterface.ResponseApi2Kafka(messageBase, 0);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35=Z
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=124\u000135=Z\u000149=003.01GW\u000156=HNX\u000134=48\u0001369=167\u000152=20230825-07:52:12\u000158=HUY\u000111=23\u0001298=4\u0001171=XDCR12101-2308100000014\u000140=S\u000155=XDCR12101\u000110=000\u0001")] // 58 # null
        [DataRow("8=FIX.4.4\u00019=124\u000135=Z\u000149=003.01GW\u000156=HNX\u000134=48\u0001369=167\u000152=20230825-07:52:12\u000158=\u000111=23\u0001298=4\u0001171=XDCR12101-2308100000014\u000140=S\u000155=XDCR12101\u000110=000\u0001")] // 58 = null
        [DataRow("8=FIX.4.4\u00019=124\u000135=Z\u000149=003.01GW\u000156=HNX\u000134=48\u0001369=167\u000152=20230825-07:52:12\u000158=\u000111=23\u0001298=4\u0001171=XDCR12101-2308100000014\u000140=S\u000155=XDCR12101\u000110=000\u0001")] // 171 # null
        [DataRow("8=FIX.4.4\u00019=124\u000135=Z\u000149=003.01GW\u000156=HNX\u000134=48\u0001369=167\u000152=20230825-07:52:12\u000158=\u000111=23\u0001298=4\u0001171=\u000140=S\u000155=XDCR12101\u000110=000\u0001")] // 171 = null
        public void ResponseApi2Kafka_35Z_MessageQuoteCancel_Test(string p_Msg)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act Đặt TTĐT
            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
            _testKafkaInterface.ResponseApi2Kafka(messageBase, 0);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35=R
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=189\u000135=R\u000149=003.01GW\u000156=HNX\u000134=47\u0001369=166\u000152=20230825-07:52:09\u000158=Replace\u000111=40\u0001644=XDCR12101-2308100000013\u000140=S\u00011=003CX00002\u000155=XDCR12101\u000154=1\u000138=8\u0001640=1000\u00016464=0\u000164=20230810\u0001513=0\u00016363=2\u00011111=0\u000110=000")] // 54=1
        [DataRow("8=FIX.4.4\u00019=189\u000135=R\u000149=003.01GW\u000156=HNX\u000134=47\u0001369=166\u000152=20230825-07:52:09\u000158=Replace\u000111=40\u0001644=XDCR12101-2308100000013\u000140=S\u00011=003CX00002\u000155=XDCR12101\u000154=2\u000138=8\u0001640=1000\u00016464=0\u000164=20230810\u0001513=0\u00016363=2\u00011111=0\u000110=000")] // 54=2
        [DataRow("8=FIX.4.4\u00019=189\u000135=R\u000149=003.01GW\u000156=HNX\u000134=47\u0001369=166\u000152=20230825-07:52:09\u000158=\u000111=40\u0001644=XDCR12101-2308100000013\u000140=S\u00011=003CX00002\u000155=XDCR12101\u000154=2\u000138=8\u0001640=1000\u00016464=0\u000164=20230810\u0001513=0\u00016363=2\u00011111=0\u000110=000")] // 58=null
        public void ResponseApi2Kafka_35R_MessageQuoteRequest_Test(string p_Msg)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act Đặt TTĐT
            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
            _testKafkaInterface.ResponseApi2Kafka(messageBase, 0);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35=AJ
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=198\u000135=AJ\u000149=003.01GW\u000156=HNX\u000134=46\u0001369=165\u000152=20230825-07:52:05\u000158=CHAP NHAN TTĐT\u000111=17\u0001693=XDCR12101-2308100000011\u0001694=1\u00011=003CX00002\u000140=S\u000155=XDCR12101\u000154=1\u000138=1000\u0001640=10000\u00016464=0\u000164=20230810\u00016363=1\u000110=000")]  // 54=1
        [DataRow("8=FIX.4.4\u00019=198\u000135=AJ\u000149=003.01GW\u000156=HNX\u000134=46\u0001369=165\u000152=20230825-07:52:05\u000158=CHAP NHAN TTĐT\u000111=17\u0001693=XDCR12101-2308100000011\u0001694=1\u00011=003CX00002\u000140=S\u000155=XDCR12101\u000154=2\u000138=1000\u0001640=10000\u00016464=0\u000164=20230810\u00016363=1\u000110=000")]  // 54=2
        [DataRow("8=FIX.4.4\u00019=198\u000135=AJ\u000149=003.01GW\u000156=HNX\u000134=46\u0001369=165\u000152=20230825-07:52:05\u000158=\u000111=17\u0001693=XDCR12101-2308100000011\u0001694=1\u00011=003CX00002\u000140=S\u000155=XDCR12101\u000154=2\u000138=1000\u0001640=10000\u00016464=0\u000164=20230810\u00016363=1\u000110=000")]  // 58==null
        public void ResponseApi2Kafka_35AJ_MessageQuoteResponse_Test(string p_Msg)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act Đặt TTĐT
            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
            _testKafkaInterface.ResponseApi2Kafka(messageBase, 0);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35=t
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=259\u000135=t\u000149=018.01GW\u000156=HNX\u000134=6\u0001369=0\u000152=20230811-10:17:42\u000111=4422c692-55fa-420e-aba3-f8bffdad7c79\u00011=018C111111\u00012=018CX00001\u000155=BABR12001\u000154=1\u000138=5\u0001448=018\u0001449=018\u000137=\u0001551=BABR12001-2307170000004\u0001549=2\u000140=R\u0001640=50000\u00016464=4000000\u000164=20230712\u0001168=20230712\u00016363=2\u000110=178\u0001'")] // 54= 1
        [DataRow("8=FIX.4.4\u00019=248\u000135=t\u000149=028.01GW\u000156=HNX\u000134=4\u0001369=0\u000152=20230812-02:58:58\u000111=df1a4e55-3b8e-4582-a75b-1aaa62c21254\u00011=028C111111\u00012=018C111111\u000155=BABR12001\u000154=2\u000138=10\u0001448=028\u0001449=018\u000137=\u0001551=BABR12001-2307180000001\u0001549=2\u000140=R\u0001640=300000\u000164=20230713\u0001168=20230713\u00016363=2\u000110=239\u0001")] // 54=2
        [DataRow("8=FIX.4.4\u00019=205\u000135=t\u000149=018.01GW\u000156=HNX\u000134=5\u0001369=0\u000152=20230812-02:59:43\u000111=MODBCGDTH633\u00011=\u00012=018C111111\u000155=BABR12001\u000154=1\u000138=0\u0001448=\u0001449=018\u000137=\u0001551=BABR12001-2307180000002\u0001549=3\u000140=R\u0001640=0\u000164=00010101\u0001168=00010101\u00016363=2\u000110=201\u0001")]
        [DataRow("8=FIX.4.4\u00019=205\u000135=t\u000149=018.01GW\u000156=HNX\u000134=5\u0001369=0\u000152=20230812-02:59:43\u000111=MODBCGDTH633\u00011=\u00012=018C111111\u000155=BABR12001\u000154=1\u000138=0\u0001448=\u0001449=018\u000137=\u0001551=\u0001549=3\u000140=R\u0001640=0\u000164=00010101\u0001168=00010101\u00016363=2\u000110=201\u0001")]
        [DataRow("8=FIX.4.4\u00019=205\u000135=t\u000149=018.01GW\u000156=HNX\u000134=5\u0001369=0\u000152=20230812-02:59:43\u000111=MODBCGDTH633\u00011=\u00012=018C111111\u000155=BABR12001\u000154=1\u000138=0\u0001448=\u0001449=018\u000137=\u0001551=\u0001549=3\u000140=R\u0001640=0\u000164=00010101\u0001168=00010101\u00016363=2\u000158=\u000110=201\u0001")]
        public void ResponseApi2Kafka_35t_CrossOrderCancelReplaceRequest_Test(string p_Msg)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act Đặt TTĐT
            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
            _testKafkaInterface.ResponseApi2Kafka(messageBase, 0);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35=u
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=158\u000135=u\u000149=018.01GW\u000156=HNX\u000134=12\u0001369=0\u000152=20230812-03:47:25\u000111=06df6388-f9cc-47c6-841e-f4e3dacc6a72\u000137=\u0001551=BABR12001-2307180000006\u0001549=2\u000155=BABR12001\u000154=1\u000140=R\u000110=216\u0001")]//Hủy cùng cty
        [DataRow("8=FIX.4.4\u00019=157\u000135=u\u000149=028.01GW\u000156=HNX\u000134=2\u0001369=0\u000152=20230814-03:40:46\u000111=ba56eb63-2180-4324-939c-4431a0b643db\u000137=\u0001551=AAAR12101-2307240000001\u0001549=2\u000155=AAAR12101\u000154=2\u000140=R\u000110=139\u0001")] //gửi yêu cầu hủy khác công ty -
        [DataRow("8=FIX.4.4\u00019=133\u000135=u\u000149=018.01GW\u000156=HNX\u000134=2\u0001369=0\u000152=20230814-03:41:08\u000111=CANBCGDTH982\u000137=\u0001551=AAAR12101-2307240000002\u0001549=3\u000155=AAAR12101\u000154=1\u000140=R\u000110=209\u0001")] //Phản hồi yêu cầu hủy khác công ty
        public void ResponseApi2Kafka_35u_CrossOrderCancelRequest_Test(string p_Msg)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act Đặt TTĐT
            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
            _testKafkaInterface.ResponseApi2Kafka(messageBase, 0);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35= N01
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=190\u000135=N01\u000149=050.02GW\u000156=HNX\u000134=4\u0001369=50\u000152=20231025-02:54:51\u000158=string\u000111=TEST_2\u000155=XDCR12101\u0001537=1\u000140=R\u000154=2\u000138=0\u0001168=20231108\u00016363=1\u000164=20231108\u0001193=20231109\u0001917=20231109\u0001226=1\u0001513=019\u0001644=\u000110=000\u0001")] // 537= 1
        [DataRow("8=FIX.4.4\u00019=190\u000135=N01\u000149=050.02GW\u000156=HNX\u000134=4\u0001369=50\u000152=20231025-02:54:51\u000158=string\u000111=BACND-TEST\u000155=XDCR12101\u0001537=2\u000140=R\u000154=2\u000138=0\u0001168=20231108\u00016363=1\u000164=20231108\u0001193=20231109\u0001917=20231109\u0001226=1\u0001513=019\u0001644=\u000110=000\u0001")] // 537= 2
        [DataRow("8=FIX.4.4\u00019=190\u000135=N01\u000149=050.02GW\u000156=HNX\u000134=4\u0001369=50\u000152=20231025-02:54:51\u000158=string\u000111=BACND-TEST\u000155=XDCR12101\u0001537=3\u000140=R\u000154=2\u000138=0\u0001168=20231108\u00016363=1\u000164=20231108\u0001193=20231109\u0001917=20231109\u0001226=1\u0001513=019\u0001644=\u000110=000\u0001")] // 537= 3
        [DataRow("8=FIX.4.4\u00019=190\u000135=N01\u000149=050.02GW\u000156=HNX\u000134=4\u0001369=50\u000152=20231025-02:54:51\u000158=string\u000111=BACND-TEST\u000155=XDCR12101\u0001537=4\u000140=R\u000154=2\u000138=0\u0001168=20231108\u00016363=1\u000164=20231108\u0001193=20231109\u0001917=20231109\u0001226=1\u0001513=019\u0001644=123\u000110=000\u0001")] // 537= 4
        [DataRow("8=FIX.4.4\u00019=190\u000135=N01\u000149=050.02GW\u000156=HNX\u000134=4\u0001369=50\u000152=20231025-02:54:51\u000158=string\u000111=BACND-TEST\u000155=XDCR12101\u0001537=4\u000140=R\u000154=3\u000138=0\u0001168=20231108\u00016363=1\u000164=20231108\u0001193=20231109\u0001917=20231109\u0001226=1\u0001513=019\u0001644=123\u000110=000\u0001")] // 54= 3 khong co
        public void ResponseApi2Kafka_35N01_NewInquiryRepos_Test(string p_Msg)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act Đặt TTĐT

            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
            _testKafkaInterface.ResponseApi2Kafka(messageBase, 0);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35= N03
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=278\u000135=N03\u000149=043.01GW\u000156=HNX\u000134=30\u0001369=145\u000152=20231107-07:38:58\u000158=test\u000111=TEST_31\u0001644=REPOSPT-2312060000021\u0001537=1\u000140=U\u000154=1\u00011=043C111111\u0001168=20231206\u00016363=1\u000164=20231206\u0001193=20231207\u0001917=20231207\u0001226=1\u0001227=2.2\u0001552=1\u00015522=1\u000155=XDCR12101\u000138=10\u000144=10000\u00012260=2.3\u00016464=0\u00016465=0\u00012261=0\u0001")]
        [DataRow("8=FIX.4.4\u00019=278\u000135=N03\u000149=043.01GW\u000156=HNX\u000134=30\u0001369=145\u000152=20231107-07:38:58\u000158=test\u000111=TEST_31\u0001644=REPOSPT-2312060000021\u0001537=1\u000140=U\u000154=2\u00011=043C111111\u0001168=20231206\u00016363=1\u000164=20231206\u0001193=20231207\u0001917=20231207\u0001226=1\u0001227=2.2\u0001552=1\u00015522=1\u000155=XDCR12101\u000138=10\u000144=10000\u00012260=2.3\u00016464=0\u00016465=0\u00012261=0\u0001")]
        [DataRow("8=FIX.4.4\u00019=278\u000135=N03\u000149=043.01GW\u000156=HNX\u000134=30\u0001369=145\u000152=20231107-07:38:58\u000158=test\u000111=TEST_31\u0001644=REPOSPT-2312060000021\u0001537=1\u000140=U\u000154=3\u00011=043C111111\u0001168=20231206\u00016363=1\u000164=20231206\u0001193=20231207\u0001917=20231207\u0001226=1\u0001227=2.2\u0001552=1\u00015522=1\u000155=XDCR12101\u000138=10\u000144=10000\u00012260=2.3\u00016464=0\u00016465=0\u00012261=0\u0001")]
        public void ResponseApi2Kafka_35N03_MessageReposFirm_Test(string p_Msg)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act
            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
            //
            MessageReposFirm newFirmRepos = (MessageReposFirm)messageBase;
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
            _testKafkaInterface.ResponseApi2Kafka(messageBase2, 0);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35= N05
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=221\u000135=N05\u000149=043.01GW\u000156=HNX\u000134=8\u0001369=76\u000152=20231226-08:29:50\u000158=accept\u000111=6715603b-96f1-42dc-bb1e-fe3b7ffa8cc0\u0001644=REPOSPT-2402090000006\u0001537=1\u000140=U\u00011=043CX00002\u0001227=2\u0001552=1\u00015522=1\u000155=XDCR12101\u000138=100\u000144=10000\u00012260=2\u00012261=0\u000110=000")]
        public void ResponseApi2Kafka_35N05_MessageReposFirmAccept_Test(string p_Msg)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act
            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
            _testKafkaInterface.ResponseApi2Kafka(messageBase, 0);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35= ME
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=335\u000135=ME\u000149=043.01GW\u000156=HNX\u000134=49\u0001369=167\u000152=20231107-08:35:00\u000158=THM\u000111=0ace0fb1-702d-4f76-9799-a0579c7d6d42\u0001198=XDCR12101-2312060000007\u0001563=1\u000140=T\u000154=2\u00011=043C111111\u00012=015C111111\u0001448=043\u0001449=015\u0001168=20231206\u00016363=1\u000164=20231206\u0001193=20231208\u0001917=20231208\u0001226=3\u0001227=1.4\u0001552=1\u00015522=1\u000155=XDCR12101\u000138=10\u000144=8000\u00012260=3.2\u00012261=0\u00016464=0\u00016465=0\u000110=000\u0001")]
        public void ResponseApi2Kafka_35ME_MessageReposBCGDModify_Test(string p_Msg)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act
            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
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
            _testKafkaInterface.ResponseApi2Kafka(messageBase2, 0);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35= MC
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=150\u000135=MC\u000149=043.01GW\u000156=HNX\u000134=51\u0001369=169\u000152=20231107-08:38:22\u000158=th\u000111=2c4055e1-9a1d-4073-ad70-e043d3a67dae\u0001198=XDCR12101-2312060000007\u0001563=1\u000140=T\u000154=2\u000110=000\u0001")]
        public void ResponseApi2Kafka_35MC_MessageReposBCGDCancel_Test(string p_Msg)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act
            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
            _testKafkaInterface.ResponseApi2Kafka(messageBase, 0);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35= MA
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=282\u000135=MA\u000149=043.01GW\u000156=HNX\u000134=38\u0001369=155\u000152=20231107-08:08:16\u000158=aaa\u000111=TEST_40\u0001198=\u0001563=1\u000140=T\u000154=1\u00011=043CX00002\u00012=043C111111\u0001448=043\u0001449=043\u0001168=20231206\u00016363=1\u000164=20231206\u0001193=20231208\u0001917=20231208\u0001226=2\u0001227=1.3\u0001552=1\u00015522=1\u000155=XDCR12101\u000138=100\u000144=9000\u00012260=1\u00012261=0\u00016464=0\u00016465=0\u0001")]
        [DataRow("8=FIX.4.4\u00019=282\u000135=MA\u000149=043.01GW\u000156=HNX\u000134=39\u0001369=156\u000152=20231107-08:10:00\u000158=NNN\u000111=TEST_41\u0001198=\u0001563=1\u000140=T\u000154=2\u00011=043C111111\u00012=015C111111\u0001448=043\u0001449=015\u0001168=20231206\u00016363=1\u000164=20231206\u0001193=20231208\u0001917=20231208\u0001226=2\u0001227=1.3\u0001552=1\u00015522=1\u000155=XDCR12101\u000138=100\u000144=9000\u00012260=1\u00012261=0\u00016464=0\u00016465=0\u000110=000\u0001")]
        public void ResponseApi2Kafka_35MA_MessageReposBCGD_Test(string p_Msg)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act
            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
            //
            MessageReposBCGD newFirmRepos = (MessageReposBCGD)messageBase;
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
            _testKafkaInterface.ResponseApi2Kafka(messageBase2, 0);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35= D
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=147\u000135=D\u000149=043.01GW\u000156=HNX\u000134=23\u0001369=99\u000152=20231115-03:30:29\u000158=test\u000111=TEST_11\u00011=043C111111\u000155=AAAR12101\u000154=1\u000140=2\u000138=100\u0001192=0\u0001640=10000\u000144=0\u0001440=0\u000110=000\u0001")]
        [DataRow("8=FIX.4.4\u00019=146\u000135=D\u000149=043.01GW\u000156=HNX\u000134=6\u0001369=92\u000152=20231212-08:13:50\u000158=test\u000111=TEST_13\u00011=043CX00002\u000155=AAAR12101\u000154=2\u000140=2\u000138=200\u0001192=0\u0001640=10000\u000144=0\u0001440=0\u000110=000\u0001")]
        public void ResponseApi2Kafka_35D_MessageNewOrder_Test(string p_Msg)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act
            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
            _testKafkaInterface.ResponseApi2Kafka(messageBase, It.IsAny<char>());

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35= G
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=179\u000135=G\u000149=043.01GW\u000156=HNX\u000134=18\u0001369=105\u000152=20231212-08:27:55\u000111=48d5f383-b249-44f9-ba87-9c9e5f6316b7\u000141=AAAR12101-2401150000003\u00011=043CX00002\u000155=AAAR12101\u000138=800\u00012238=1000\u0001640=90000\u000110=000\u0001")]
        [DataRow("8=FIX.4.4\u00019=180\u000135=G\u000149=043.01GW\u000156=HNX\u000134=13\u0001369=100\u000152=20231212-08:25:26\u000111=2838a0db-9f04-402b-ab61-53407e7ddca1\u000141=AAAR12101-2401150000003\u00011=043C111111\u000155=XDCR12101\u000138=2000\u00012238=1000\u0001640=90000\u000110=000\u0001")]
        public void ResponseApi2Kafka_35G_MessageNewOrder_Test(string p_Msg)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act
            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
            _testKafkaInterface.ResponseApi2Kafka(messageBase, 0);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Test 35= F
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=149\u000135=F\u000149=043.01GW\u000156=HNX\u000134=21\u0001369=108\u000152=20231212-08:31:16\u000158=cancel\u000111=c27ff50d-b73d-414a-8325-6cddde0f0106\u000141=AAAR12101-2401150000005\u000155=AAAR12101\u000110=000\u0001")]
        [DataRow("8=FIX.4.4\u00019=149\u000135=F\u000149=043.01GW\u000156=HNX\u000134=20\u0001369=107\u000152=20231212-08:30:56\u000158=cancel\u000111=851c69a6-6bea-4c3c-a2a3-d55a19de1210\u000141=AAAR12101-2401150000005\u000155=XDCR12101\u000110=000\u0001")]
        public void ResponseApi2Kafka_35F_MessageCancelOrder_Test(string p_Msg) 
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            _testKafkaInterface.NumOfMsg();
            mockKafkaClient.Reset();

            // Act
            FIXMessageBase messageBase = messageFactoryFIX.Parse(p_Msg);
            _testKafkaInterface.ResponseApi2Kafka(messageBase, 0);

            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }
        #endregion Gửi Phản hồi khi đặt lệnh API gửi sang kafka

        #region Gate gửi reject về cho kafka do check phiên sai

        [TestMethod()]
        public void TestGateSendReject()
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            // Act Đặt TTĐT
            _testKafkaInterface.ReportGateReject(new FIXMessageBase(), "UnitTest", "-1");
            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=185\u000135=N01\u000149=050.02GW\u000156=HNX\u000134=6\u0001369=52\u000152=20231025-03:11:16\u000111=TEST_4\u000155=XDCR12101\u0001537=1\u000140=U\u000154=2\u000138=100000\u0001168=20231116\u00016363=2\u000164=20231116\u0001193=20231117\u0001917=20231117\u0001226=1\u0001513=019\u0001644=\u000110=000\u0001")]
        public void TestGateSendReject_35N01(string msgRaw)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);

            // Act Đặt TTĐT
            _testKafkaInterface.ReportGateReject(messageBase, "UnitTest", "-1");
            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=147\u000135=D\u000149=043.01GW\u000156=HNX\u000134=23\u0001369=99\u000152=20231115-03:30:29\u000158=test\u000111=TEST_11\u00011=043C111111\u000155=AAAR12101\u000154=1\u000140=2\u000138=100\u0001192=0\u0001640=10000\u000144=0\u0001440=0\u000110=000\u0001")]
        public void TestGateSendReject_35D(string msgRaw)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);

            // Act Đặt TTĐT
            _testKafkaInterface.ReportGateReject(messageBase, "UnitTest", "-1");
            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=278\u000135=N03\u000149=043.01GW\u000156=HNX\u000134=30\u0001369=145\u000152=20231107-07:38:58\u000158=test\u000111=TEST_31\u0001644=REPOSPT-2312060000021\u0001537=1\u000140=U\u000154=2\u00011=043C111111\u0001168=20231206\u00016363=1\u000164=20231206\u0001193=20231207\u0001917=20231207\u0001226=1\u0001227=2.2\u0001552=1\u00015522=1\u000155=XDCR12101\u000138=10\u000144=10000\u00012260=2.3\u00016464=0\u00016465=0\u00012261=0\u000110=000\u0001", "-1")] // -1
        [DataRow("8=FIX.4.4\u00019=278\u000135=N03\u000149=043.01GW\u000156=HNX\u000134=30\u0001369=145\u000152=20231107-07:38:58\u000158=test\u000111=TEST_31\u0001644=REPOSPT-2312060000021\u0001537=1\u000140=U\u000154=2\u00011=043C111111\u0001168=20231206\u00016363=1\u000164=20231206\u0001193=20231207\u0001917=20231207\u0001226=1\u0001227=2.2\u0001552=1\u00015522=1\u000155=XDCR12101\u000138=10\u000144=10000\u00012260=2.3\u00016464=0\u00016465=0\u00012261=0\u000110=000\u0001", "017")] // 017
        [DataRow("8=FIX.4.4\u00019=278\u000135=N03\u000149=043.01GW\u000156=HNX\u000134=30\u0001369=145\u000152=20231107-07:38:58\u000158=test\u000111=TEST_31\u0001644=REPOSPT-2312060000021\u0001537=1\u000140=U\u000154=2\u00011=043C111111\u0001168=20231206\u00016363=1\u000164=20231206\u0001193=20231207\u0001917=20231207\u0001226=1\u0001227=2.2\u0001552=1\u00015522=1\u000155=XDCR12101\u000138=10\u000144=10000\u00012260=2.3\u00016464=0\u00016465=0\u00012261=0\u000110=000\u0001", "020")] // 023
        public void TestGateSendReject_35N03(string msgRaw, string p_Code)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);

            // Act Đặt TTĐT
            _testKafkaInterface.ReportGateReject(messageBase, "UnitTest", p_Code);
            // Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        #endregion Gate gửi reject về cho kafka do check phiên sai

        [TestMethod()]
        public void JsonObject_ResponseMessage_Test()
        {
            ResponseMessageKafka responseMessageKafka = new ResponseMessageKafka();
            TestGetProperty(responseMessageKafka, typeof(ResponseMessageKafka));

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void JsonObject_ResponseOrderFilledToKafka_Test()
        {
            ResponseOrderFilledToKafka responseOrderFilledToKafka = new ResponseOrderFilledToKafka();
            TestGetProperty(responseOrderFilledToKafka, typeof(ResponseOrderFilledToKafka));

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void JsonObject_ResponseTradingSession_Test()
        {
            MessageTradingSessionStatus _MessageTradingSessionStatus = new MessageTradingSessionStatus();
            TestGetProperty(_MessageTradingSessionStatus, typeof(MessageTradingSessionStatus));

            ResponseTradingSession responseTradingSession = new ResponseTradingSession(_MessageTradingSessionStatus);
            TestGetProperty(responseTradingSession, typeof(ResponseTradingSession));

            MessageSecurityStatus MessageSecurityStatus = new MessageSecurityStatus();
            TestGetProperty(MessageSecurityStatus, typeof(MessageSecurityStatus));

            ResponseSecurityStatus MessageTradingSessionStatus = new ResponseSecurityStatus(MessageSecurityStatus);
            TestGetProperty(MessageTradingSessionStatus, typeof(ResponseSecurityStatus));

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void InquiryObjectModel_Test()
        {
            InquiryObjectModel _Response = new InquiryObjectModel();
            _Response.MsgType = CORE_MsgType.MsgIS;
            _Response.OrderNo = "";
            _Response.ExchangeID = "";
            _Response.RefExchangeID = "";
            _Response.QuoteType = "".ToString();
            _Response.OrdType = "";
            _Response.OrderStatus = CORE_OrderStatus.OrderStatus_QE;
            _Response.OrderPartyID = "";
            _Response.Side = "";
            _Response.EffectiveTime = "";
            _Response.RepurchaseTerm = 0;
            _Response.SettleMethod = 0;
            _Response.RegistID = "";
            _Response.SettleDate1 = "";
            _Response.SettleDate2 = "";
            _Response.EndDate = "";
            _Response.OrderValue = 0;
            _Response.Symbol = "";
            _Response.RejectReasonCode = "";
            _Response.RejectReason = "";
            _Response.Text = "";
            _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);
            TestGetProperty(_Response, typeof(InquiryObjectModel));

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void FirmReposModel_Test()
        {
            FirmReposModel _Response = new FirmReposModel();
            _Response.MsgType = CORE_MsgType.MsgIS;
            _Response.RefMsgType = CORE_MsgType.MsgIS;
            _Response.OrderNo = "";
            _Response.ExchangeID = "";
            _Response.RefExchangeID = "";
            _Response.QuoteType = "".ToString();
            _Response.OrdType = "";
            _Response.OrderStatus = CORE_OrderStatus.OrderStatus_QE;
            _Response.OrderPartyID = "";
            _Response.Side = "";
            _Response.EffectiveTime = "";
            _Response.RepurchaseTerm = 0;
            _Response.SettleMethod = 0;
            _Response.SettleDate1 = "";
            _Response.SettleDate2 = "";
            _Response.EndDate = "";
            _Response.MatchReportType = 0;
            _Response.RejectReasonCode = "";
            _Response.RejectReason = "";
            _Response.Text = "";
            _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);

            _Response.InquiryMember = "";
            _Response.ClientID = "";
            _Response.ClientIDCounterFirm = "";
            _Response.MemberCounterFirm = "";
            _Response.MemberCounterFirm = "";
            _Response.NoSide = 1;
            //
            List<ReposSideListResponse> listReposSide = new List<ReposSideListResponse>();
            ReposSideListResponse _ReposSideList = new ReposSideListResponse();
            _ReposSideList.NumSide = 1;
            _ReposSideList.Symbol = "SSI";
            _ReposSideList.OrderQty = 1;
            _ReposSideList.ExecPrice = 1;
            _ReposSideList.MergePrice = 1;
            _ReposSideList.HedgeRate = 1;
            _ReposSideList.SettleValue1 = 1;
            _ReposSideList.SettleValue2 = 1;
            listReposSide.Add(_ReposSideList);
            //
            _Response.SymbolFirmInfo = listReposSide;

            TestGetProperty(_Response, typeof(FirmReposModel));

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void ReposSideListResponse_Test()
        {
            ReposSideListResponse _ReposSideList = new ReposSideListResponse();
            _ReposSideList.NumSide = 1;
            _ReposSideList.Symbol = "SSI";
            _ReposSideList.OrderQty = 1;
            _ReposSideList.ExecPrice = 1;
            _ReposSideList.MergePrice = 1;
            _ReposSideList.ReposInterest = 1;
            _ReposSideList.HedgeRate = 1;
            _ReposSideList.SettleValue1 = 1;
            _ReposSideList.SettleValue2 = 1;

            TestGetProperty(_ReposSideList, typeof(ReposSideListResponse));

            Assert.IsTrue(true);
        }

        //
        [TestMethod()]
        public void ExecOrderReposModel_Test()
        {
            ExecOrderReposModel _Response = new ExecOrderReposModel();
            _Response.MsgType = CORE_MsgType.MsgRE;
            _Response.Text = "";
            _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);
            _Response.OrderNo = "";
            _Response.OrderID = "";
            _Response.SellOrderID = "";
            _Response.BuyOrderID = "";
            _Response.MemberCounterFirm = "";
            _Response.RepurchaseTerm = 0;
            _Response.RepurchaseRate = 0;
            _Response.SettleDate1 = "";
            _Response.SettleDate2 = "";
            _Response.EndDate = "";
            _Response.Side = "";
            _Response.MatchReportType = 0;
            _Response.NoSide = 1;
            //
            List<ReposSideListExecOrderReposResponse> listReposSide = new List<ReposSideListExecOrderReposResponse>();
            ReposSideListExecOrderReposResponse _ReposSideList = new ReposSideListExecOrderReposResponse();
            _ReposSideList.NumSide = 1;
            _ReposSideList.Symbol = "SSI";
            _ReposSideList.ExecQty = 1;
            _ReposSideList.ExecPx = 1;
            _ReposSideList.MergePrice = 1;
            _ReposSideList.ReposInterest = 1;
            _ReposSideList.HedgeRate = 1;
            _ReposSideList.SettleValue1 = 1;
            _ReposSideList.SettleValue2 = 1;
            listReposSide.Add(_ReposSideList);
            //
            _Response.SymbolFirmInfo = listReposSide;

            TestGetProperty(_Response, typeof(ExecOrderReposModel));

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void ReposSideListExecOrderReposResponse_Test()
        {
            ReposSideListExecOrderReposResponse _ReposSideList = new ReposSideListExecOrderReposResponse();
            _ReposSideList.NumSide = 1;
            _ReposSideList.Symbol = "SSI";
            _ReposSideList.ExecQty = 1;
            _ReposSideList.ExecPx = 1;
            _ReposSideList.MergePrice = 1;
            _ReposSideList.ReposInterest = 1;
            _ReposSideList.HedgeRate = 1;
            _ReposSideList.SettleValue1 = 1;
            _ReposSideList.SettleValue2 = 1;

            TestGetProperty(_ReposSideList, typeof(ReposSideListExecOrderReposResponse));

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void NormalObjectModel_Test()
        {
            NormalObjectModel _Response = new NormalObjectModel();
            _Response.MsgType = CORE_MsgType.MsgRE;
            _Response.Text = "";
            _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);
            _Response.OrderNo = "";
            _Response.ExchangeID = "";
            _Response.RefExchangeID = "";
            _Response.OrderType = "";
            _Response.OrderStatus = "";
            _Response.Side = "";
            _Response.Symbol = "";
            _Response.OrderQty = 0;
            _Response.OrgOrderQty = 0;
            _Response.LeavesQty = 0;
            _Response.LastQty = 0;
            _Response.Price = 0;
            _Response.ClientID = "";
            _Response.SettleValue = 0.0;
            _Response.OrderQtyMM2 = 0;
            _Response.PriceMM2 = 0;
            _Response.SpecialType = 0;
            _Response.RejectReasonCode = "";
            _Response.RejectReason = "";

            TestGetProperty(_Response, typeof(NormalObjectModel));

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void NormalOrderExecutionObjectModel_Test()
        {
            NormalOrderExecutionObjectModel _Response = new NormalOrderExecutionObjectModel();
            _Response.MsgType = CORE_MsgType.MsgRE;
            _Response.Text = "";
            _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);
            _Response.OrderNo = "";
            _Response.OrderID = "";
            _Response.BuyOrderID = "";
            _Response.SellOrderID = "";
            _Response.Side = "";
            _Response.Symbol = "";
            _Response.LastQty = 0;
            _Response.LastPx = 0;
            _Response.SettleValue = 0;
            _Response.ExecID = "";
            _Response.MemberCounterFirm = "";

            TestGetProperty(_Response, typeof(NormalOrderExecutionObjectModel));

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void ResponseOrderFilledToKafka_Test()
        {
            ResponseOrderFilledToKafka _Response = new ResponseOrderFilledToKafka();
            _Response.MsgType = CORE_MsgType.MsgRE;
            _Response.Text = "";
            _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);
            _Response.OrderNo = "";
            _Response.OrderID = "";
            _Response.OrigClOrdID = "";
            _Response.SecondaryClOrdID = "";
            _Response.Side = "";
            _Response.Symbol = "";
            _Response.LastQty = 0;
            _Response.LastPx = 0;
            _Response.SettleValue = 0;
            _Response.ExecID = "";
            _Response.MemberCounterFirm = "";

            TestGetProperty(_Response, typeof(ResponseOrderFilledToKafka));

            Assert.IsTrue(true);
        }

		[TestMethod()]
		public void ResponseTopicTradingInfomation_Test()
		{
            MessageTopicTradingInfomation message = new MessageTopicTradingInfomation();
            message.InquiryMember = "003";
            message.Symbol = "SSI";

			ResponseTopicTradingInfomation _Response = new ResponseTopicTradingInfomation(message);
			_Response.MsgType = MessageType.TopicTradingInfomation;
			_Response.Member = "003";
			_Response.Symbol = "SSI";
			_Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);
			TestGetProperty(_Response, typeof(ResponseTopicTradingInfomation));

			Assert.IsTrue(true);
		}

		private void TestGetProperty(object obj, Type p_objType)
        {
            foreach (PropertyInfo objProperty in p_objType.GetProperties())
            {
                var propertyvalye = objProperty.GetValue(obj);
            }
        }
    }
}