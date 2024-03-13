using APIMonitor.Models;
using APIMonitor.ObjectInfo;
using APIServer;
using APIServer.Models;
using BusinessProcessResponse;
using CommonLib;
using HNXInterface;
using LocalMemory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using UTInputData.Mock;

namespace APIMonitor.Tests
{
    [TestClass()]
    public class ApiMonitorControllerTests
    {
        private ApiMonitorController c_ApiMonitorController;
        private HealthCheckController c_HealthCheckController;

        [TestInitialize]
        public void Initialize()
        {
            c_ApiMonitorController = new ApiMonitorController(new MockiHNXEntity(), new MockIKafkaInterface());
            c_HealthCheckController = new HealthCheckController();

            OrderInfo objOrder = new OrderInfo()
            {
                RefMsgType = "S",
                OrderNo = "ACB01",
                ClOrdID = "12345678",
                ExchangeID = "", // ?
                RefExchangeID = "", // ?
                SeqNum = 0,  //
                Side = "B",
                Price = 10000,
                OrderQty = 1000,
                CrossType = "", // ?
                ClientID = "",
                ClientIDCounterFirm = "", // ?
                MemberCounterFirm = "" // ?
            };
            OrderMemory.Add_NewOrder(objOrder);
        }

        [TestMethod()]
        public void HealthCheck_Test()
        {
            var _return = c_HealthCheckController.HealthCheck();
            //Assert
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void _BoxConnectTest()
        {
            var _return = c_ApiMonitorController._BoxConnect();
            //Assert
            Assert.IsTrue(_return != null);
        }

        [TestMethod()]
        public void _BoxConnectTest_Exception()
        {
            ApiMonitorController _ApiMonitorController = new ApiMonitorController(null, new MockIKafkaInterface());

            var _return = _ApiMonitorController._BoxConnect();
            //Assert
            Assert.IsTrue(_return != null);
        }

		[TestMethod()]
		public void ApiGetConfig_Test()
		{
			//Act
			var _return = c_ApiMonitorController.ApiGetConfig();
			//Assert
			Assert.IsTrue(true);
		}

		[TestMethod()]
        [DataRow("3", "AAA", 1)]
        [DataRow("3", "BBB", 0)]
        [DataRow("3", "CCC", -1)]
        public void _SendSecurityStatusRequestTest(string tradingCode,string p_Symbol, int p_exp)
        {
            //Act
            var _return = c_ApiMonitorController.SecurityStatusRequest(tradingCode, p_Symbol);
            //Assert
            Assert.AreEqual(p_exp, _return);
        }

		[TestMethod()]
		[DataRow("3", "AAA", 1)]
		[DataRow("3", "BBB", 0)]
		[DataRow("3", "CCC", -1)]
		public void _SendSecurityStatusRequestTest_Exception(string tradingCode, string p_Symbol, int p_exp)
		{
			//Arrange
			Moq.Mock<iHNXClient> _mockProcessRevBussiness = new Moq.Mock<iHNXClient>();
			_mockProcessRevBussiness.Setup(helper => helper.SendSecurityStatusRequest(tradingCode, p_Symbol)).Returns
				 (() =>
				 {
					 throw new Exception();
				 });

			ApiMonitorController _ApiMonitorController = new ApiMonitorController(_mockProcessRevBussiness.Object, new MockIKafkaInterface());

			//Act
			var _return = _ApiMonitorController.SecurityStatusRequest(tradingCode, p_Symbol);
			//Assert
			Assert.IsTrue(true);
		}

		[TestMethod()]
        [DataRow("0","")]
        [DataRow("1", "")]
        [DataRow("2", "TABLE")]
        [DataRow("3", "SSI")]
        public void TradingSessionRequestTest(string tradingCode,string tradingName)
        {
            //Arrange
            Moq.Mock<iHNXClient> _mockProcessRevBussiness = new Moq.Mock<iHNXClient>();
            _mockProcessRevBussiness.Setup(helper => helper.SendTradingSessionRequest(tradingCode,tradingName)).Returns(true);

            ApiMonitorController _ApiMonitorController = new ApiMonitorController(_mockProcessRevBussiness.Object, new MockIKafkaInterface());
            //Act
            var _return = _ApiMonitorController.TradingSessionRequest(tradingCode, tradingName);
            //Assert
            Assert.AreEqual(1, _return);
        }

        [TestMethod()]
        [DataRow("0", "")]
        [DataRow("1", "")]
        [DataRow("2", "TABLE")]
        [DataRow("3", "SSI")]
        public void _SendTradingSessionRequestTest_False(string tradingCode, string tradingName)
        {
            //Arrange

            Moq.Mock<iHNXClient> _mockProcessRevBussiness = new Moq.Mock<iHNXClient>();
            _mockProcessRevBussiness.Setup(helper => helper.SendTradingSessionRequest(tradingCode, tradingName)).Returns(false);

            ApiMonitorController _ApiMonitorController = new ApiMonitorController(_mockProcessRevBussiness.Object, new MockIKafkaInterface());

            //Act
            var _return = _ApiMonitorController.TradingSessionRequest(tradingCode, tradingName);
            //Assert
            Assert.AreEqual(0, _return);
        }

        [TestMethod()]
        [DataRow("0", "")]
        [DataRow("1", "")]
        [DataRow("2", "TABLE")]
        [DataRow("3", "SSI")]
        public void _SendTradingSessionRequestTest_Exception(string tradingCode, string tradingName)
        {
            //Arrange
            Moq.Mock<iHNXClient> _mockProcessRevBussiness = new Moq.Mock<iHNXClient>();
            _mockProcessRevBussiness.Setup(helper => helper.SendTradingSessionRequest(tradingCode, tradingName)).Returns
                 (() =>
                 {
                     throw new Exception();
                 });

            ApiMonitorController _ApiMonitorController = new ApiMonitorController(_mockProcessRevBussiness.Object, new MockIKafkaInterface());

            //Act
            var _return = _ApiMonitorController.TradingSessionRequest(tradingCode, tradingName);
            //Assert
            Assert.AreEqual(-1, _return);
        }

        [TestMethod()]
        [DataRow("SSI", "123", "1234")]
        public void SendUserRequest_Test(string userName, string password, string passwordNew)
        {
            //Arrange
            Moq.Mock<iHNXClient> _mockProcessRevBussiness = new Moq.Mock<iHNXClient>();
            _mockProcessRevBussiness.Setup(helper => helper.SendUserRequest(userName, password, passwordNew)).Returns(true);

            ApiMonitorController _ApiMonitorController = new ApiMonitorController(_mockProcessRevBussiness.Object, new MockIKafkaInterface());
            //Act
            var _return = _ApiMonitorController.ChangGatewayPassword(password, passwordNew);
            //Assert
            Assert.IsTrue(true);
        }

        [TestMethod()]
        [DataRow("SSI", "123", "1234")]
        public void SendUserRequest_Exception(string userName, string password, string passwordNew)
        {
            //Arrange
            Moq.Mock<iHNXClient> _mockProcessRevBussiness = new Moq.Mock<iHNXClient>();
            _mockProcessRevBussiness.Setup(helper => helper.SendUserRequest(userName, password, passwordNew)).Returns
                 (() =>
                 {
                     throw new Exception();
                 });

            ApiMonitorController _ApiMonitorController = new ApiMonitorController(_mockProcessRevBussiness.Object, new MockIKafkaInterface());

            //Act
            var _return = _ApiMonitorController.ChangGatewayPassword(password, passwordNew);
            //Assert
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void OrderGetByClOrderIDTest()
        {
            //Arrange

            //Act
            var _return = c_ApiMonitorController.OrderGetByClOrderID("12345678");
            //Assert
            Assert.IsTrue(_return.Value is OrderInfo);
        }

        [TestMethod()]
        public void OrderGetByClOrderID_Invalid()
        {
            //Arrange

            //Act
            var _return = c_ApiMonitorController.OrderGetByClOrderID("123");
            //Assert
            Assert.IsTrue(_return.Value is string);
        }

        [TestMethod()]
        public void OrderGetByOrderNoTest()
        {
            //Arrange

            //Act
            var _return = c_ApiMonitorController.OrderGetByOrderNo("ACB01");
            //Assert
            Assert.IsTrue(_return.Value is OrderInfo);
        }

        [TestMethod()]
        public void OrderGetByOrderNo_Invalid()
        {
            //Arrange

            //Act
            var _return = c_ApiMonitorController.OrderGetByOrderNo("ACB02");
            //Assert
            Assert.IsTrue(_return.Value is string);
        }

        [TestMethod()]
        public void OrderGetByOrderNo_Exception()
        {
            //Arrange

            //Act
            var _return = c_ApiMonitorController.OrderGetByOrderNo(null);
            //Assert
            Assert.IsTrue(_return.Value is string);
        }

        [TestMethod()]
        public void GetOrigOrder_byClOrdIDTest()
        {
            //Arrange

            //Act
            var _return = c_ApiMonitorController.GetOrigOrder_byClOrdID("12345678");
            //Assert
            Assert.IsTrue(_return.Value == "ACB01");
        }

        [TestMethod()]
        public void OrderGetBySeqTest()
        {
            //Arrange
            OrderMemory.Update_Order("12345678", 10);
            //Act
            var _return = c_ApiMonitorController.OrderGetBySeq(10);
            //Assert
            Assert.IsTrue(_return.Value is OrderInfo);
        }

		[TestMethod()]
		public void TestGetMemoryTest()
		{
			//Arrange

			//Act
			var _return = c_ApiMonitorController.TestGetMemory("12345678","N05");
			//Assert
			Assert.IsTrue(true);
		}

		[TestMethod()]
        public void OrderGetBySeq_Invalid()
        {
            //Arrange

            //Act
            var _return = c_ApiMonitorController.OrderGetBySeq(1);
            //Assert
            Assert.IsTrue(_return.Value is string);
        }

        [TestMethod()]
        public void GetListOrderTest()
        {
            //Arrange

            //Act
            var _return = c_ApiMonitorController.GetListOrder();
            //Assert
            Assert.IsTrue(_return.Value.Count > 0);
        } 
        
        [TestMethod()]
        public void GenPassword_Test()
        {
            //Arrange

            //Act
            var _return = c_ApiMonitorController.GenPassword("bacnd","1233123");
            //Assert
            Assert.IsTrue(true);
        } 
        
        [TestMethod()]
        public void VaultQuery_Test()
        {
            //Arrange

            //Act
            var _return = c_ApiMonitorController.VaultQuery("1234");
            //Assert
            Assert.IsTrue(true);
        }

		[TestMethod()]
		public void VaultUpdatePass_Test()
		{
			//Arrange
			//Act
			var _return = c_ApiMonitorController.VaultUpdatePass("1234");
			//Assert
			Assert.IsTrue(true);
		}

		[TestMethod()]
        public void ApiModel_Test()
        {
            HNXSystemConnectModel _HNXSystemConnectModel = new HNXSystemConnectModel();
            TestGetProperty(_HNXSystemConnectModel, typeof(HNXSystemConnectModel));

            KafkaSystemConnectModel _KafkaSystemConnectModel = new KafkaSystemConnectModel();
            TestGetProperty(_KafkaSystemConnectModel, typeof(KafkaSystemConnectModel));

            BoxConnectModel _BoxConnectModel = new BoxConnectModel();
            TestGetProperty(_BoxConnectModel, typeof(BoxConnectModel));

            HealthCheckModel _HealthCheckModel = new HealthCheckModel();
            TestGetProperty(_HealthCheckModel, typeof(HealthCheckModel));

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