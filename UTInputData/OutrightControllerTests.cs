using APIServer;
using APIServer.Validation;
using BusinessProcessAPIReq.RequestModels;
using BusinessProcessAPIReq.ResponseModels;
using CommonLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UTInputData.Mock;

namespace UTInputData.Tests
{
    [TestClass()]
    public class OutrightControllerTests
    {
        //
        private API1NewElectronicPutThroughController c_API1NewElectronicPutThroughController;

        private API1NewElectronicPutThroughController c_API1NewElectronicPutThroughController_E;

        //
        private API2AcceptElectronicPutThroughController c_API2AcceptElectronicPutThroughController;

        private API2AcceptElectronicPutThroughController c_API2AcceptElectronicPutThroughController_E;

        //
        private API3ReplaceElectronicPutThroughController c_API3ReplaceElectronicPutThroughController;

        private API3ReplaceElectronicPutThroughController c_API3ReplaceElectronicPutThroughController_E;

        //
        private API4CancelElectronicPutThroughController c_API4CancelElectronicPutThroughController;

        private API4CancelElectronicPutThroughController c_API4CancelElectronicPutThroughController_E;

        //
        private API5NewCommonPutThroughController c_API5NewCommonPutThroughController;

        private API5NewCommonPutThroughController c_API5NewCommonPutThroughController_E;

        //
        private API6AcceptCommonPutThroughController c_API6AcceptCommonPutThroughController;

        private API6AcceptCommonPutThroughController c_API6AcceptCommonPutThroughController_E;

        //
        private API7ReplaceCommonPutThroughController c_API7ReplaceCommonPutThroughController;

        private API7ReplaceCommonPutThroughController c_API7ReplaceCommonPutThroughController_E;

        //
        private API8CancelCommonPutThroughController c_API8CancelCommonPutThroughController;

        private API8CancelCommonPutThroughController c_API8CancelCommonPutThroughController_E;

        //
        private API9ReplaceCommonPutThroughDealController c_API9ReplaceCommonPutThroughDealController;

        private API9ReplaceCommonPutThroughDealController c_API9ReplaceCommonPutThroughDealController_E;

        //
        private API10ResponseForReplacingCommonPutThroughDealController c_API10ResponseForReplacingCommonPutThroughDealController;

        private API10ResponseForReplacingCommonPutThroughDealController c_API10ResponseForReplacingCommonPutThroughDealController_E;

        //
        private API11CancelCommonPutThroughDealController c_API11CancelCommonPutThroughDealController;

        private API11CancelCommonPutThroughDealController c_API11CancelCommonPutThroughDealController_E;

        //
        private API12ResponseForCancelingCommonPutThroughDealController c_API12ResponseForCancelingCommonPutThroughDealController;

        private API12ResponseForCancelingCommonPutThroughDealController c_API12ResponseForCancelingCommonPutThroughDealController_E;

        [TestInitialize]
        public void Initialize()
        {
            try
            {
                MockIProcessRevBussiness _MockIProcessRevBussiness = new MockIProcessRevBussiness();
                //Moq.Mock<IProcessRevBussiness> _mockProcessRevBussiness = new Moq.Mock<IProcessRevBussiness>();
                //_mockProcessRevBussiness.Setup(helper => helper.API1NewElectronicPutThrough_BU(It.IsAny<API1NewElectronicPutThroughRequest>(), It.IsAny<API1NewElectronicPutThroughResponse>())).ReturnsAsync
                //(() =>
                //{
                //    API1NewElectronicPutThroughResponse _return = new API1NewElectronicPutThroughResponse();
                //    _return.ReturnCode = "000";
                //    return new API1NewElectronicPutThroughResponse() { ReturnCode = "000" };
                //});

                API1NewElectronicPutThroughValidator _API1NewElectronicPutThroughValidator = new API1NewElectronicPutThroughValidator();
                API2AcceptElectronicPutThroughValidator _api2AcceptElectronicPutThrough_Validator = new API2AcceptElectronicPutThroughValidator();
                API3ReplaceElectronicPutThroughValidator _api3ReplaceElectronicPutThrough_Validator = new API3ReplaceElectronicPutThroughValidator();
                API4CancelElectronicPutThroughValidator _api4CancelElectronicPutThrough_Validator = new API4CancelElectronicPutThroughValidator();
                API5NewCommonPutThroughValidator _api5NewCommonPutThrough_Validator = new API5NewCommonPutThroughValidator();

                API6AcceptCommonPutThroughValidator _api6AcceptCommonPutThrough_Validator = new API6AcceptCommonPutThroughValidator();
                API7ReplaceCommonPutThroughValidator _api7ReplaceCommonPutThrough_Validator = new API7ReplaceCommonPutThroughValidator();
                API8CancelCommonPutThroughValidator _api8CancelCommonPutThrough_Validator = new API8CancelCommonPutThroughValidator();
                API9ReplaceCommonPutThroughDealValidator _api9ReplaceCommonPutThroughDeal_Validator = new API9ReplaceCommonPutThroughDealValidator();
                API10ResponseForReplacingCommonPutThroughDealValidator _api10ResponseForReplacingCommonPutThroughDeal_Validator = new API10ResponseForReplacingCommonPutThroughDealValidator();
                API11CancelCommonPutThroughDealValidator _api11CancelCommonPutThroughDeal_Validator = new API11CancelCommonPutThroughDealValidator();
                API12ResponseForCancelingCommonPutThroughDealValidator _api12ResponseForCancelingCommonPutThroughDeal_Validator = new API12ResponseForCancelingCommonPutThroughDealValidator();

                //
                c_API1NewElectronicPutThroughController = new API1NewElectronicPutThroughController(_MockIProcessRevBussiness, _API1NewElectronicPutThroughValidator);
                c_API1NewElectronicPutThroughController_E = new API1NewElectronicPutThroughController(null,
                   _API1NewElectronicPutThroughValidator);
                //

                c_API2AcceptElectronicPutThroughController = new API2AcceptElectronicPutThroughController(_MockIProcessRevBussiness, _api2AcceptElectronicPutThrough_Validator);
                c_API2AcceptElectronicPutThroughController_E = new API2AcceptElectronicPutThroughController(null, _api2AcceptElectronicPutThrough_Validator);
                //

                c_API3ReplaceElectronicPutThroughController = new API3ReplaceElectronicPutThroughController(_MockIProcessRevBussiness, _api3ReplaceElectronicPutThrough_Validator);
                c_API3ReplaceElectronicPutThroughController_E = new API3ReplaceElectronicPutThroughController(null, _api3ReplaceElectronicPutThrough_Validator);
                //

                c_API4CancelElectronicPutThroughController = new API4CancelElectronicPutThroughController(_MockIProcessRevBussiness, _api4CancelElectronicPutThrough_Validator);
                c_API4CancelElectronicPutThroughController_E = new API4CancelElectronicPutThroughController(null, _api4CancelElectronicPutThrough_Validator);
                //

                c_API5NewCommonPutThroughController = new API5NewCommonPutThroughController(_MockIProcessRevBussiness, _api5NewCommonPutThrough_Validator);
                c_API5NewCommonPutThroughController_E = new API5NewCommonPutThroughController(null, _api5NewCommonPutThrough_Validator);

                //
                c_API6AcceptCommonPutThroughController = new API6AcceptCommonPutThroughController(_MockIProcessRevBussiness, _api6AcceptCommonPutThrough_Validator);
                c_API6AcceptCommonPutThroughController_E = new API6AcceptCommonPutThroughController(null, _api6AcceptCommonPutThrough_Validator);
                //
                c_API7ReplaceCommonPutThroughController = new API7ReplaceCommonPutThroughController(_MockIProcessRevBussiness, _api7ReplaceCommonPutThrough_Validator);
                c_API7ReplaceCommonPutThroughController_E = new API7ReplaceCommonPutThroughController(null, _api7ReplaceCommonPutThrough_Validator);
                //
                c_API8CancelCommonPutThroughController = new API8CancelCommonPutThroughController(_MockIProcessRevBussiness, _api8CancelCommonPutThrough_Validator);
                c_API8CancelCommonPutThroughController_E = new API8CancelCommonPutThroughController(null, _api8CancelCommonPutThrough_Validator);
                //

                c_API9ReplaceCommonPutThroughDealController = new API9ReplaceCommonPutThroughDealController(_MockIProcessRevBussiness, _api9ReplaceCommonPutThroughDeal_Validator);
                c_API9ReplaceCommonPutThroughDealController_E = new API9ReplaceCommonPutThroughDealController(null, _api9ReplaceCommonPutThroughDeal_Validator);
                //

                c_API10ResponseForReplacingCommonPutThroughDealController = new API10ResponseForReplacingCommonPutThroughDealController(_MockIProcessRevBussiness, _api10ResponseForReplacingCommonPutThroughDeal_Validator);
                c_API10ResponseForReplacingCommonPutThroughDealController_E = new API10ResponseForReplacingCommonPutThroughDealController(null, _api10ResponseForReplacingCommonPutThroughDeal_Validator);
                //

                c_API10ResponseForReplacingCommonPutThroughDealController = new API10ResponseForReplacingCommonPutThroughDealController(_MockIProcessRevBussiness, _api10ResponseForReplacingCommonPutThroughDeal_Validator);
                c_API10ResponseForReplacingCommonPutThroughDealController_E = new API10ResponseForReplacingCommonPutThroughDealController(null, _api10ResponseForReplacingCommonPutThroughDeal_Validator);
                //

                c_API11CancelCommonPutThroughDealController = new API11CancelCommonPutThroughDealController(_MockIProcessRevBussiness, _api11CancelCommonPutThroughDeal_Validator);
                c_API11CancelCommonPutThroughDealController_E = new API11CancelCommonPutThroughDealController(null, _api11CancelCommonPutThroughDeal_Validator);

                //
                c_API12ResponseForCancelingCommonPutThroughDealController = new API12ResponseForCancelingCommonPutThroughDealController(_MockIProcessRevBussiness, _api12ResponseForCancelingCommonPutThroughDeal_Validator);
                c_API12ResponseForCancelingCommonPutThroughDealController_E = new API12ResponseForCancelingCommonPutThroughDealController(null, _api12ResponseForCancelingCommonPutThroughDeal_Validator);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        [TestMethod()]
        public void KhoitaoRequestTest()
        {
            API1NewElectronicPutThroughRequest _API1NewElectronicPutThroughRequest = CBOTool<API1NewElectronicPutThroughRequest>.CreateObjectFromType();
            CBOTool<API1NewElectronicPutThroughRequest>.TestGetProperty(_API1NewElectronicPutThroughRequest);
            API1NewElectronicPutThroughResponse _API1NewElectronicPutThroughResponse = CBOTool<API1NewElectronicPutThroughResponse>.CreateObjectFromType();
            CBOTool<API1NewElectronicPutThroughResponse>.TestGetProperty(_API1NewElectronicPutThroughResponse);
            //
            API2AcceptElectronicPutThroughRequest _API2AcceptElectronicPutThroughRequest = CBOTool<API2AcceptElectronicPutThroughRequest>.CreateObjectFromType();
            CBOTool<API2AcceptElectronicPutThroughRequest>.TestGetProperty(_API2AcceptElectronicPutThroughRequest);
            API2AcceptElectronicPutThroughResponse _API2AcceptElectronicPutThroughResponse = CBOTool<API2AcceptElectronicPutThroughResponse>.CreateObjectFromType();
            CBOTool<API2AcceptElectronicPutThroughResponse>.TestGetProperty(_API2AcceptElectronicPutThroughResponse);
            //
            API3ReplaceElectronicPutThroughRequest _API3ReplaceElectronicPutThroughRequest = CBOTool<API3ReplaceElectronicPutThroughRequest>.CreateObjectFromType();
            CBOTool<API3ReplaceElectronicPutThroughRequest>.TestGetProperty(_API3ReplaceElectronicPutThroughRequest);
            API3ReplaceElectronicPutThroughResponse _API3ReplaceElectronicPutThroughResponse = CBOTool<API3ReplaceElectronicPutThroughResponse>.CreateObjectFromType();
            CBOTool<API3ReplaceElectronicPutThroughResponse>.TestGetProperty(_API3ReplaceElectronicPutThroughResponse);
            //
            API4CancelElectronicPutThroughRequest _API4CancelElectronicPutThroughRequest = CBOTool<API4CancelElectronicPutThroughRequest>.CreateObjectFromType();
            CBOTool<API4CancelElectronicPutThroughRequest>.TestGetProperty(_API4CancelElectronicPutThroughRequest);
            API4CancelElectronicPutThroughResponse _API4CancelElectronicPutThroughResponse = CBOTool<API4CancelElectronicPutThroughResponse>.CreateObjectFromType();
            CBOTool<API4CancelElectronicPutThroughResponse>.TestGetProperty(_API4CancelElectronicPutThroughResponse);
            //
            API5NewCommonPutThroughRequest _API5NewCommonPutThroughRequest = CBOTool<API5NewCommonPutThroughRequest>.CreateObjectFromType();
            CBOTool<API5NewCommonPutThroughRequest>.TestGetProperty(_API5NewCommonPutThroughRequest);
            API5NewCommonPutThroughResponse _API5NewCommonPutThroughResponse = CBOTool<API5NewCommonPutThroughResponse>.CreateObjectFromType();
            CBOTool<API5NewCommonPutThroughResponse>.TestGetProperty(_API5NewCommonPutThroughResponse);
            //
            API6AcceptCommonPutThroughRequest _API6AcceptCommonPutThroughRequest = CBOTool<API6AcceptCommonPutThroughRequest>.CreateObjectFromType();
            CBOTool<API6AcceptCommonPutThroughRequest>.TestGetProperty(_API6AcceptCommonPutThroughRequest);
            API6AcceptCommonPutThroughResponse _API6AcceptCommonPutThroughResponse = CBOTool<API6AcceptCommonPutThroughResponse>.CreateObjectFromType();
            CBOTool<API6AcceptCommonPutThroughResponse>.TestGetProperty(_API6AcceptCommonPutThroughResponse);
            //
            API7ReplaceCommonPutThroughRequest _API7ReplaceCommonPutThroughRequest = CBOTool<API7ReplaceCommonPutThroughRequest>.CreateObjectFromType();
            CBOTool<API7ReplaceCommonPutThroughRequest>.TestGetProperty(_API7ReplaceCommonPutThroughRequest);
            API7ReplaceCommonPutThroughResponse _API7ReplaceCommonPutThroughResponse = CBOTool<API7ReplaceCommonPutThroughResponse>.CreateObjectFromType();
            CBOTool<API7ReplaceCommonPutThroughResponse>.TestGetProperty(_API7ReplaceCommonPutThroughResponse);
            //
            API8CancelCommonPutThroughRequest _API8CancelCommonPutThroughRequest = CBOTool<API8CancelCommonPutThroughRequest>.CreateObjectFromType();
            CBOTool<API8CancelCommonPutThroughRequest>.TestGetProperty(_API8CancelCommonPutThroughRequest);
            API8CancelCommonPutThroughResponse _API8CancelCommonPutThroughResponse = CBOTool<API8CancelCommonPutThroughResponse>.CreateObjectFromType();
            CBOTool<API8CancelCommonPutThroughResponse>.TestGetProperty(_API8CancelCommonPutThroughResponse);
            //
            API9ReplaceCommonPutThroughDealRequest _API9ReplaceCommonPutThroughDealRequest = CBOTool<API9ReplaceCommonPutThroughDealRequest>.CreateObjectFromType();
            CBOTool<API9ReplaceCommonPutThroughDealRequest>.TestGetProperty(_API9ReplaceCommonPutThroughDealRequest);
            API9ReplaceCommonPutThroughDealResponse _API9ReplaceCommonPutThroughDealResponse = CBOTool<API9ReplaceCommonPutThroughDealResponse>.CreateObjectFromType();
            CBOTool<API9ReplaceCommonPutThroughDealResponse>.TestGetProperty(_API9ReplaceCommonPutThroughDealResponse);
            //
            API10ResponseForReplacingCommonPutThroughDealRequest _API10ResponseForReplacingCommonPutThroughDealRequest = CBOTool<API10ResponseForReplacingCommonPutThroughDealRequest>.CreateObjectFromType();
            CBOTool<API10ResponseForReplacingCommonPutThroughDealRequest>.TestGetProperty(_API10ResponseForReplacingCommonPutThroughDealRequest);
            API10ResponseForReplacingCommonPutThroughDealResponse _API10ResponseForReplacingCommonPutThroughDealResponse = CBOTool<API10ResponseForReplacingCommonPutThroughDealResponse>.CreateObjectFromType();
            CBOTool<API10ResponseForReplacingCommonPutThroughDealResponse>.TestGetProperty(_API10ResponseForReplacingCommonPutThroughDealResponse);
            //
            API11CancelCommonPutThroughDealRequest _API11CancelCommonPutThroughDealRequest = CBOTool<API11CancelCommonPutThroughDealRequest>.CreateObjectFromType();
            CBOTool<API11CancelCommonPutThroughDealRequest>.TestGetProperty(_API11CancelCommonPutThroughDealRequest);
            API11CancelCommonPutThroughDealResponse _API11CancelCommonPutThroughDealResponse = CBOTool<API11CancelCommonPutThroughDealResponse>.CreateObjectFromType();
            CBOTool<API11CancelCommonPutThroughDealResponse>.TestGetProperty(_API11CancelCommonPutThroughDealResponse);
            //
            API12ResponseForCancelingCommonPutThroughDealRequest _API12ResponseForCancelingCommonPutThroughDealRequest = CBOTool<API12ResponseForCancelingCommonPutThroughDealRequest>.CreateObjectFromType();
            CBOTool<API12ResponseForCancelingCommonPutThroughDealRequest>.TestGetProperty(_API12ResponseForCancelingCommonPutThroughDealRequest);
            API12ResponseForCancelingCommonPutThroughDealResponse _API12ResponseForCancelingCommonPutThroughDealResponse = CBOTool<API12ResponseForCancelingCommonPutThroughDealResponse>.CreateObjectFromType();
            CBOTool<API12ResponseForCancelingCommonPutThroughDealResponse>.TestGetProperty(_API12ResponseForCancelingCommonPutThroughDealResponse);
            //
            Assert.IsTrue(true);
        }

        #region API1NewElectronicPutThrough

        [TestMethod()]
        public void API1NewElectronicPutThroughTest()
        {
            //Arrange
            var request = new API1NewElectronicPutThroughRequest()
            {
                OrderNo = "18",
                ClientID = "003C000001",
                OrderType = "S",
                Side = "B",
                Symbol = "XDCR12101",
                Price = 1000,
                OrderQty = 5,
                SettleDate = "20230810",
                SettleMethod = 1,
                RegistID = "0",
                IsVisible = 0,
                Text = "DAT TTĐT"
            };

            //Act
            var _return = c_API1NewElectronicPutThroughController.API1NewElectronicPutThrough(request).Result;

            //Assert
            Assert.AreEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API1NewElectronicPutThroughTest_Invalid()
        {
            //Arrange
            var request = new API1NewElectronicPutThroughRequest()
            {
                OrderNo = "",
                ClientID = "",
                OrderType = "",
                Side = "",
                Symbol = "",
                Price = 0,
                OrderQty = 0,
                SettleDate = "20231810",
                SettleMethod = 3,
                RegistID = "0",
                IsVisible = 3,
                Text = "DAT TTĐT"
            };

            //Act
            var _return = c_API1NewElectronicPutThroughController.API1NewElectronicPutThrough(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API1NewElectronicPutThroughTest_Invalid2()
        {
            //Arrange
            var request = new API1NewElectronicPutThroughRequest()
            {
                OrderNo = "18",
                ClientID = "003C000001",
                OrderType = "S",
                Side = "B",
                Symbol = "XDCR12101",
                Price = 1000,
                OrderQty = 5,
                SettleDate = "20230810",
                SettleMethod = 1,
                RegistID = "0",
                IsVisible = 0,
                Text = "DAT TTĐT"
            };

            //Act
            var _return = c_API1NewElectronicPutThroughController_E.API1NewElectronicPutThrough(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API1NewElectronicPutThroughTest_Exception()
        {
            //Arrange
            var request = new API1NewElectronicPutThroughRequest()
            {
                OrderNo = "18",
                ClientID = null
            };

            //Act
            var _return = c_API1NewElectronicPutThroughController_E.API1NewElectronicPutThrough(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        #endregion API1NewElectronicPutThrough

        #region API2AcceptElectronicPutThrough

        [TestMethod()]
        public void API2AcceptElectronicPutThroughTest()
        {
            //Arrange
            var request = new API2AcceptElectronicPutThroughRequest()
            {
                OrderNo = "DUNG_TEST_API2",
                RefExchangeID = "XDCR12101-2308100000011",
                ClientID = ConfigData.FirmID + "CX00002",
                ClientIDCounterFirm = "ClientIDCounterFirm_CX00002",
                OrderType = "S",
                Side = "S",
                Symbol = "XDCR12101",
                Price = 10000,
                OrderQty = 1000,
                SettleDate = "20230810",
                SettleMethod = 1,
                Text = "CHAP NHAN TTĐT"
            };

            //Act
            var _return = c_API2AcceptElectronicPutThroughController.API2AcceptElectronicPutThrough(request).Result;

            //Assert
            Assert.AreEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API2AcceptElectronicPutThroughTest_Invalid()
        {
            //Arrange
            var request = new API2AcceptElectronicPutThroughRequest()
            {
            };

            //Act
            var _return = c_API2AcceptElectronicPutThroughController.API2AcceptElectronicPutThrough(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API2AcceptElectronicPutThroughTest_Exception()
        {
            //Arrange
            var request = new API2AcceptElectronicPutThroughRequest()
            {
                OrderNo = "DUNG_TEST_API2",
                RefExchangeID = "XDCR12101-2308100000011",
                ClientID = ConfigData.FirmID + "CX00002",
                OrderType = "S",
                Side = "S",
                Symbol = "XDCR12101",
                Price = 10000,
                OrderQty = 1000,
                SettleDate = "20230810",
                SettleMethod = 1,
                Text = "CHAP NHAN TTĐT"
            };

            //Act
            var _return = c_API2AcceptElectronicPutThroughController_E.API2AcceptElectronicPutThrough(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        #endregion API2AcceptElectronicPutThrough

        #region API3ReplaceElectronicPutThrough

        [TestMethod()]
        public void API3ReplaceElectronicPutThroughTest()
        {
            //Arrange
            var request = new API3ReplaceElectronicPutThroughRequest()
            {
                OrderNo = "DUNG_TEST_API3_",
                RefExchangeID = "XDCR12101-2308100000013",
                ClientID = ConfigData.FirmID + "CX00002",
                OrderType = "S",
                Side = "B",
                Symbol = "XDCR12101",
                Price = 1000,
                OrderQty = 8,
                SettleDate = "20230810",
                SettleMethod = 2,
                RegistID = "0",
                Text = "SỬA"
            };

            //Act
            var _return = c_API3ReplaceElectronicPutThroughController.API3ReplaceElectronicPutThrough(request).Result;

            //Assert
            Assert.AreEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API3ReplaceElectronicPutThroughTest_Valid()
        {
            //Arrange
            var request = new API3ReplaceElectronicPutThroughRequest()
            {
            };

            //Act
            var _return = c_API3ReplaceElectronicPutThroughController.API3ReplaceElectronicPutThrough(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API3ReplaceElectronicPutThroughTest_Exception()
        {
            //Arrange
            var request = new API3ReplaceElectronicPutThroughRequest()
            {
                OrderNo = "DUNG_TEST_API3_",
                RefExchangeID = "XDCR12101-2308100000013",
                ClientID = ConfigData.FirmID + "CX00002",
                OrderType = "S",
                Side = "B",
                Symbol = "XDCR12101",
                Price = 1000,
                OrderQty = 8,
                SettleDate = "20230810",
                SettleMethod = 2,
                RegistID = "0",
                Text = "SỬA"
            };

            //Act
            var _return = c_API3ReplaceElectronicPutThroughController_E.API3ReplaceElectronicPutThrough(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        #endregion API3ReplaceElectronicPutThrough

        #region API4CancelElectronicPutThrough

        [TestMethod()]
        public void API4CancelElectronicPutThroughTest()
        {
            //Arrange
            var request = new API4CancelElectronicPutThroughRequest()
            {
                OrderNo = "DUNGTEST_API4",
                RefExchangeID = "XDCR12101-2309210000009",
                OrderType = "S",
                Symbol = "XDCR12101",
                Text = "HUY"
            };

            //Act
            var _return = c_API4CancelElectronicPutThroughController.API4CancelElectronicPutThrough(request).Result;

            //Assert
            Assert.AreEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API4CancelElectronicPutThroughTest_Ịnvalid()
        {
            //Arrange
            var request = new API4CancelElectronicPutThroughRequest()
            {
            };

            //Act
            var _return = c_API4CancelElectronicPutThroughController.API4CancelElectronicPutThrough(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API4CancelElectronicPutThroughTest_Exception()
        {
            //Arrange
            var request = new API4CancelElectronicPutThroughRequest()
            {
                OrderNo = "DUNGTEST_API4",
                RefExchangeID = "XDCR12101-2309210000009",
                OrderType = "S",
                Symbol = "XDCR12101",
                Text = "HUY"
            };

            //Act
            var _return = c_API4CancelElectronicPutThroughController_E.API4CancelElectronicPutThrough(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        #endregion API4CancelElectronicPutThrough

        #region API5NewCommonPutThrough

        [TestMethod()]
        public void API5NewCommonPutThroughTest()
        {
            //Arrange
            var request = new API5NewCommonPutThroughRequest()
            {
                OrderNo = "DUNG_TEST_API4_",
                ClientID = ConfigData.FirmID + "C111111",
                ClientIDCounterFirm = "003CX00002",
                MemberCounterFirm = "003",
                OrderType = "R",
                Side = "S",
                Symbol = "XDCR12101",
                Price = 10,
                OrderQty = 10,
                SettleDate = "20230810",
                SettleMethod = 1,
                CrossType = 1,
                EffectiveTime = "20230810",
                Text = "DAT BCGD"
            };

            //Act
            var _return = c_API5NewCommonPutThroughController.API5NewCommonPutThrough(request).Result;

            //Assert
            Assert.AreEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API5NewCommonPutThroughTest_Invalid()
        {
            //Arrange
            var request = new API5NewCommonPutThroughRequest()
            {
            };

            //Act
            var _return = c_API5NewCommonPutThroughController.API5NewCommonPutThrough(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API5NewCommonPutThroughTest_Invalid2()
        {
            //Arrange
            var request = new API5NewCommonPutThroughRequest()
            {
                OrderNo = "DUNG_TEST_API4_",
                ClientID = ConfigData.FirmID + "C111111",
                ClientIDCounterFirm = "",
                MemberCounterFirm = "003",
                OrderType = "R",
                Side = "S",
                Symbol = "XDCR12101",
                Price = 10,
                OrderQty = 10,
                SettleDate = "20230810",
                SettleMethod = 1,
                CrossType = 1,
                EffectiveTime = "20230810",
                Text = "DAT BCGD"
            };

            //Act
            var _return = c_API5NewCommonPutThroughController.API5NewCommonPutThrough(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API5NewCommonPutThroughTest_Exception()
        {
            //Arrange
            var request = new API5NewCommonPutThroughRequest()
            {
                OrderNo = "DUNG_TEST_API4_",
                ClientID = ConfigData.FirmID + "C111111",
                ClientIDCounterFirm = "003CX00002",
                MemberCounterFirm = "003",
                OrderType = "R",
                Side = "S",
                Symbol = "XDCR12101",
                Price = 10,
                OrderQty = 10,
                SettleDate = "20230810",
                SettleMethod = 1,
                CrossType = 1,
                EffectiveTime = "20230810",
                Text = "DAT BCGD"
            };

            //Act
            var _return = c_API5NewCommonPutThroughController_E.API5NewCommonPutThrough(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        #endregion API5NewCommonPutThrough

        #region API6AcceptCommonPutThrough

        [TestMethod()]
        public void API6AcceptCommonPutThroughTest()
        {
            //Arrange
            var request = new API6AcceptCommonPutThroughRequest()
            {
                OrderNo = "55",
                RefExchangeID = "XDCR12101-2308100000019",
                ClientID = "003C111111",
                ClientIDCounterFirm = "043C111111",
                MemberCounterFirm = "043",
                OrderType = "R",
                Side = "B",
                Symbol = "XDCR12101",
                Price = 1000,
                OrderQty = 1000,
                SettleDate = "20230810",
                SettleMethod = 1,
                CrossType = 3,
                EffectiveTime = "20230810",
                Text = "CHAP NHAN BCGD"
            };

            //Act
            var _return = c_API6AcceptCommonPutThroughController.API6AcceptCommonPutThrough(request).Result;

            //Assert
            Assert.AreEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API6AcceptCommonPutThroughTest_Invalid()
        {
            //Arrange
            var request = new API6AcceptCommonPutThroughRequest()
            {
            };

            //Act
            var _return = c_API6AcceptCommonPutThroughController.API6AcceptCommonPutThrough(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API6AcceptCommonPutThroughTest_Exception()
        {
            //Arrange
            var request = new API6AcceptCommonPutThroughRequest()
            {
                OrderNo = "55",
                RefExchangeID = "XDCR12101-2308100000019",
                ClientID = "003C111111",
                ClientIDCounterFirm = "043C111111",
                MemberCounterFirm = "043",
                OrderType = "R",
                Side = "B",
                Symbol = "XDCR12101",
                Price = 1000,
                OrderQty = 1000,
                SettleDate = "20230810",
                SettleMethod = 1,
                CrossType = 3,
                EffectiveTime = "20230810",
                Text = "CHAP NHAN BCGD"
            };

            //Act
            var _return = c_API6AcceptCommonPutThroughController_E.API6AcceptCommonPutThrough(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        #endregion API6AcceptCommonPutThrough

        #region API7ReplaceCommonPutThrough

        [TestMethod()]
        public void API7ReplaceCommonPutThroughTest()
        {
            //Arrange
            var request = new API7ReplaceCommonPutThroughRequest()
            {
                OrderNo = "DUNG_TEST_API7",
                RefExchangeID = "XDCR12101-2309040000007",
                ClientID = "003C111111",
                ClientIDCounterFirm = "043C111111",
                MemberCounterFirm = "043",
                OrderType = "R",
                CrossType = 1,
                Side = "B",
                Symbol = "XDCR12101",
                Price = 12,
                OrderQty = 12,
                SettleDate = "20230915",
                SettleMethod = 2,
                EffectiveTime = "20230915",
                Text = "replace common"
            };

            //Act
            var _return = c_API7ReplaceCommonPutThroughController.API7ReplaceCommonPutThrough(request).Result;

            //Assert
            Assert.AreEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API7ReplaceCommonPutThroughTest_Invalid()
        {
            //Arrange
            var request = new API7ReplaceCommonPutThroughRequest()
            {
            };

            //Act
            var _return = c_API7ReplaceCommonPutThroughController.API7ReplaceCommonPutThrough(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API7ReplaceCommonPutThroughTest_Invalid2()
        {
            //Arrange
            var request = new API7ReplaceCommonPutThroughRequest()
            {
                OrderNo = "DUNG_TEST_API7",
                RefExchangeID = "XDCR12101-2309040000007",
                ClientID = "003C111111",
                ClientIDCounterFirm = "",
                MemberCounterFirm = "003",
                OrderType = "R",
                CrossType = 1,
                Side = "B",
                Symbol = "XDCR12101",
                Price = 12,
                OrderQty = 12,
                SettleDate = "20230915",
                SettleMethod = 2,
                EffectiveTime = "20230915",
                Text = "replace common"
            };

            //Act
            var _return = c_API7ReplaceCommonPutThroughController.API7ReplaceCommonPutThrough(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API7ReplaceCommonPutThroughTest_Exception()
        {
            //Arrange
            var request = new API7ReplaceCommonPutThroughRequest()
            {
                OrderNo = "DUNG_TEST_API7",
                RefExchangeID = "XDCR12101-2309040000007",
                ClientID = "003C111111",
                ClientIDCounterFirm = "043C111111",
                MemberCounterFirm = "043",
                OrderType = "R",
                CrossType = 1,
                Side = "B",
                Symbol = "XDCR12101",
                Price = 12,
                OrderQty = 12,
                SettleDate = "20230915",
                SettleMethod = 2,
                EffectiveTime = "20230915",
                Text = "replace common"
            };

            //Act
            var _return = c_API7ReplaceCommonPutThroughController_E.API7ReplaceCommonPutThrough(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        #endregion API7ReplaceCommonPutThrough

        #region API8CancelCommonPutThrough

        [TestMethod()]
        public void API8CancelCommonPutThroughTest()
        {
            //Arrange
            var request = new API8CancelCommonPutThroughRequest()
            {
                OrderNo = "DUNG_TEST_API8_",
                RefExchangeID = "XDCR12101-2308310000008",
                OrderType = "R",
                Symbol = "XDCR12101",
                Side = "S",
                CrossType = 1,
                Text = "cancel common"
            };

            //Act
            var _return = c_API8CancelCommonPutThroughController.API8CancelCommonPutThrough(request).Result;

            //Assert
            Assert.AreEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API8CancelCommonPutThroughTest_Invalid()
        {
            //Arrange
            var request = new API8CancelCommonPutThroughRequest()
            {
            };

            //Act
            var _return = c_API8CancelCommonPutThroughController.API8CancelCommonPutThrough(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API8CancelCommonPutThroughTest_Exception()
        {
            //Arrange
            var request = new API8CancelCommonPutThroughRequest()
            {
                OrderNo = "DUNG_TEST_API8_",
                RefExchangeID = "XDCR12101-2308310000008",
                OrderType = "R",
                Symbol = "XDCR12101",
                Side = "S",
                CrossType = 1,
                Text = "cancel common"
            };

            //Act
            var _return = c_API8CancelCommonPutThroughController_E.API8CancelCommonPutThrough(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        #endregion API8CancelCommonPutThrough

        #region API9ReplaceCommonPutThroughDeal

        [TestMethod()]
        public void API9ReplaceCommonPutThroughDealTest()
        {
            //Arrange
            var request = new API9ReplaceCommonPutThroughDealRequest()
            {
                OrderNo = "DUNG_TEST_API_9_",
                RefExchangeID = "XDCR12101-2309150000004",
                ClientID = "043C111111",
                ClientIDCounterFirm = "043CX00002",
                MemberCounterFirm = "043",
                OrderType = "S",
                CrossType = 2,
                Side = "B",
                Symbol = "XDCR12101",
                Price = 1000,
                OrderQty = 5,
                SettleDate = "20230915",
                SettleMethod = 2,
                EffectiveTime = "20230915",
                Text = "replace deal"
            };

            //Act
            var _return = c_API9ReplaceCommonPutThroughDealController.API9ReplaceCommonPutThroughDeal(request).Result;

            //Assert
            Assert.AreEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API9ReplaceCommonPutThroughDealTest_Invalid()
        {
            //Arrange
            var request = new API9ReplaceCommonPutThroughDealRequest()
            {
            };

            //Act
            var _return = c_API9ReplaceCommonPutThroughDealController.API9ReplaceCommonPutThroughDeal(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API9ReplaceCommonPutThroughDealTest_Exception()
        {
            //Arrange
            var request = new API9ReplaceCommonPutThroughDealRequest()
            {
                OrderNo = "DUNG_TEST_API_9_",
                RefExchangeID = "XDCR12101-2309150000004",
                ClientID = "043C111111",
                ClientIDCounterFirm = "043CX00002",
                MemberCounterFirm = "043",
                OrderType = "S",
                CrossType = 2,
                Side = "B",
                Symbol = "XDCR12101",
                Price = 1000,
                OrderQty = 5,
                SettleDate = "20230915",
                SettleMethod = 2,
                EffectiveTime = "20230915",
                Text = "replace deal"
            };

            //Act
            var _return = c_API9ReplaceCommonPutThroughDealController_E.API9ReplaceCommonPutThroughDeal(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        #endregion API9ReplaceCommonPutThroughDeal

        #region API10ResponseForReplacingCommonPutThroughDeal

        [TestMethod()]
        public void API10ResponseForReplacingCommonPutThroughDealTest()
        {
            //Arrange
            var request = new API10ResponseForReplacingCommonPutThroughDealRequest()
            {
                OrderNo = "test 10",
                RefExchangeID = "XDCR12101-2309200000004",
                ClientID = "043C111111",
                ClientIDCounterFirm = "050CX00002",
                MemberCounterFirm = "050",
                OrderType = "S",
                CrossType = 4,
                Side = "S",
                Symbol = "XDCR12101",
                Price = 10,
                OrderQty = 10,
                SettleDate = "20230920",
                SettleMethod = 2,
                EffectiveTime = "20230920",
                Text = "confirm replace deal"
            };

            //Act
            var _return = c_API10ResponseForReplacingCommonPutThroughDealController.API10ResponseForReplacingCommonPutThroughDeal(request).Result;

            //Assert
            Assert.AreEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API10ResponseForReplacingCommonPutThroughDealTest_Invalid()
        {
            //Arrange
            var request = new API10ResponseForReplacingCommonPutThroughDealRequest()
            {
            };

            //Act
            var _return = c_API10ResponseForReplacingCommonPutThroughDealController.API10ResponseForReplacingCommonPutThroughDeal(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API10ResponseForReplacingCommonPutThroughDealTest_Exception()
        {
            //Arrange
            var request = new API10ResponseForReplacingCommonPutThroughDealRequest()
            {
                OrderNo = "test 10",
                RefExchangeID = "XDCR12101-2309200000004",
                ClientID = "043C111111",
                ClientIDCounterFirm = "050CX00002",
                MemberCounterFirm = "050",
                OrderType = "S",
                CrossType = 4,
                Side = "S",
                Symbol = "XDCR12101",
                Price = 10,
                OrderQty = 10,
                SettleDate = "20230920",
                SettleMethod = 2,
                EffectiveTime = "20230920",
                Text = "confirm replace deal"
            };

            //Act
            var _return = c_API10ResponseForReplacingCommonPutThroughDealController_E.API10ResponseForReplacingCommonPutThroughDeal(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        #endregion API10ResponseForReplacingCommonPutThroughDeal

        #region API11CancelCommonPutThroughDeal

        [TestMethod()]
        public void API11CancelCommonPutThroughDealTest()
        {
            //Arrange
            var request = new API11CancelCommonPutThroughDealRequest()
            {
                OrderNo = "OrderNo",
                RefExchangeID = "RefExchangeID",
                OrderType = "R",
                Symbol = "Symbol",
                Side = "B",
                CrossType = 2,
                Text = ""
            };

            //Act
            var _return = c_API11CancelCommonPutThroughDealController.API11CancelCommonPutThroughDeal(request).Result;

            //Assert
            Assert.AreEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API11CancelCommonPutThroughDealTest_Invalid()
        {
            //Arrange
            var request = new API11CancelCommonPutThroughDealRequest()
            {
            };

            //Act
            var _return = c_API11CancelCommonPutThroughDealController.API11CancelCommonPutThroughDeal(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API11CancelCommonPutThroughDealTest_Exception()
        {
            //Arrange
            var request = new API11CancelCommonPutThroughDealRequest()
            {
                OrderNo = "OrderNo",
                RefExchangeID = "RefExchangeID",
                OrderType = "R",
                Symbol = "Symbol",
                Side = "B",
                CrossType = 2,
                Text = ""
            };

            //Act
            var _return = c_API11CancelCommonPutThroughDealController_E.API11CancelCommonPutThroughDeal(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        #endregion API11CancelCommonPutThroughDeal

        #region API12ResponseForCancelingCommonPutThroughDeal

        [TestMethod()]
        public void API12ResponseForCancelingCommonPutThroughDealTest()
        {
            //Arrange
            var request = new API12ResponseForCancelingCommonPutThroughDealRequest()
            {
                OrderNo = "OrderNo",
                RefExchangeID = "RefExchangeID",
                OrderType = "R",
                Symbol = "Symbol",
                Side = "B",
                CrossType = 3,
                Text = ""
            };

            //Act
            var _return = c_API12ResponseForCancelingCommonPutThroughDealController.API12ResponseForCancelingCommonPutThroughDeal(request).Result;

            //Assert
            Assert.AreEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API12ResponseForCancelingCommonPutThroughDealTest_Invalid()
        {
            //Arrange
            var request = new API12ResponseForCancelingCommonPutThroughDealRequest()
            {
            };

            //Act
            var _return = c_API12ResponseForCancelingCommonPutThroughDealController.API12ResponseForCancelingCommonPutThroughDeal(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API12ResponseForCancelingCommonPutThroughDealTest_Exception()
        {
            //Arrange
            var request = new API12ResponseForCancelingCommonPutThroughDealRequest()
            {
                OrderNo = "OrderNo",
                RefExchangeID = "RefExchangeID",
                OrderType = "R",
                Symbol = "Symbol",
                Side = "B",
                CrossType = 3,
                Text = ""
            };

            //Act
            var _return = c_API12ResponseForCancelingCommonPutThroughDealController_E.API12ResponseForCancelingCommonPutThroughDeal(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        #endregion API12ResponseForCancelingCommonPutThroughDeal
    }
}