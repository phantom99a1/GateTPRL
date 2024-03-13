using APIServer;
using APIServer.Validation;
using BusinessProcessAPIReq.RequestModels;
using BusinessProcessAPIReq.ResponseModels;
using CommonLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UTInputData.Mock;
using static CommonLib.CommonData;

namespace UTInputData
{
    [TestClass()]
    public class NormalControllerTests
    {
        //
        private API31OrderNewAutomaticOrderMatchingController c_API31OrderNewAutomaticOrderMatchingController;

        private API31OrderNewAutomaticOrderMatchingController c_API31OrderNewAutomaticOrderMatchingController_E;

        //
        private API32OrderReplaceAutomaticOrderMatchingController c_API32OrderReplaceAutomaticOrderMatchingController;

        private API32OrderReplaceAutomaticOrderMatchingController c_API32OrderReplaceAutomaticOrderMatchingController_E;

        //
        private API33OrderCancelAutomaticOrderMatchingController c_API33OrderCancelAutomaticOrderMatchingController;

        private API33OrderCancelAutomaticOrderMatchingController c_API33OrderCancelAutomaticOrderMatchingController_E;

        [TestInitialize]
        public void Initialize()
        {
            MockIProcessRevBussiness _MockIProcessRevBussiness = new MockIProcessRevBussiness();
            //
            API31OrderNewAutomaticOrderMatchingValidator _API31OrderNewAutomaticOrderMatchingValidator_Validator = new API31OrderNewAutomaticOrderMatchingValidator();
            c_API31OrderNewAutomaticOrderMatchingController = new API31OrderNewAutomaticOrderMatchingController(_MockIProcessRevBussiness, _API31OrderNewAutomaticOrderMatchingValidator_Validator);
            c_API31OrderNewAutomaticOrderMatchingController_E = new API31OrderNewAutomaticOrderMatchingController(null, _API31OrderNewAutomaticOrderMatchingValidator_Validator);

            //
            API32OrderReplaceAutomaticOrderMatchingValidator _API32OrderReplaceAutomaticOrderMatchingValidator_Validator = new API32OrderReplaceAutomaticOrderMatchingValidator();
            c_API32OrderReplaceAutomaticOrderMatchingController = new API32OrderReplaceAutomaticOrderMatchingController(_MockIProcessRevBussiness, _API32OrderReplaceAutomaticOrderMatchingValidator_Validator);
            c_API32OrderReplaceAutomaticOrderMatchingController_E = new API32OrderReplaceAutomaticOrderMatchingController(null, _API32OrderReplaceAutomaticOrderMatchingValidator_Validator);

            //
            API33OrderCancelAutomaticOrderMatchingValidator _API33OrderCancelAutomaticOrderMatchingValidator_Validator = new API33OrderCancelAutomaticOrderMatchingValidator();
            c_API33OrderCancelAutomaticOrderMatchingController = new API33OrderCancelAutomaticOrderMatchingController(_MockIProcessRevBussiness, _API33OrderCancelAutomaticOrderMatchingValidator_Validator);
            c_API33OrderCancelAutomaticOrderMatchingController_E = new API33OrderCancelAutomaticOrderMatchingController(null, _API33OrderCancelAutomaticOrderMatchingValidator_Validator);
        }

        [TestMethod()]
        public void KhoitaoRequestTest()
        {
            //
            API31OrderNewAutomaticOrderMatchingRequest _API31OrderNewAutomaticOrderMatchingRequest = CBOTool<API31OrderNewAutomaticOrderMatchingRequest>.CreateObjectFromType();
            CBOTool<API31OrderNewAutomaticOrderMatchingRequest>.TestGetProperty(_API31OrderNewAutomaticOrderMatchingRequest);

            API31OrderNewAutomaticOrderMatchingResponse _API31OrderNewAutomaticOrderMatchingResponse = CBOTool<API31OrderNewAutomaticOrderMatchingResponse>.CreateObjectFromType();
            CBOTool<API31OrderNewAutomaticOrderMatchingResponse>.TestGetProperty(_API31OrderNewAutomaticOrderMatchingResponse);

            //
            API32OrderReplaceAutomaticOrderMatchingRequest _API32OrderReplaceAutomaticOrderMatchingRequest = CBOTool<API32OrderReplaceAutomaticOrderMatchingRequest>.CreateObjectFromType();
            CBOTool<API32OrderReplaceAutomaticOrderMatchingRequest>.TestGetProperty(_API32OrderReplaceAutomaticOrderMatchingRequest);

            API32OrderReplaceAutomaticOrderMatchingResponse _API32OrderReplaceAutomaticOrderMatchingResponse = CBOTool<API32OrderReplaceAutomaticOrderMatchingResponse>.CreateObjectFromType();
            CBOTool<API32OrderReplaceAutomaticOrderMatchingResponse>.TestGetProperty(_API32OrderReplaceAutomaticOrderMatchingResponse);

            //
            API33OrderCancelAutomaticOrderMatchingRequest _API33OrderCancelAutomaticOrderMatchingRequest = CBOTool<API33OrderCancelAutomaticOrderMatchingRequest>.CreateObjectFromType();
            CBOTool<API33OrderCancelAutomaticOrderMatchingRequest>.TestGetProperty(_API33OrderCancelAutomaticOrderMatchingRequest);

            API33OrderCancelAutomaticOrderMatchingResponse _API33OrderCancelAutomaticOrderMatchingResponse = CBOTool<API33OrderCancelAutomaticOrderMatchingResponse>.CreateObjectFromType();
            CBOTool<API33OrderCancelAutomaticOrderMatchingResponse>.TestGetProperty(_API33OrderCancelAutomaticOrderMatchingResponse);

            Assert.IsTrue(true);
        }

        #region API31OrderNewAutomaticOrderMatching

        [TestMethod()]
        public void API31OrderNewAutomaticOrderMatching_Test()
        {
            //Arrange
            var request = new API31OrderNewAutomaticOrderMatchingRequest()
            {
                OrderNo = "NO_31",
                ClientID = "ClientID_31",
                Symbol = "SYMBOL_31",
                Side = ORDER_SIDE.SIDE_BUY,
                OrderType = NORMAL_ORDERTYPE.LO,
                OrderQty = 1000,
                Price = 100000,
                SpecialType = 1,
                Text = "TEST API 31"
            };

            //Act
            var _return = c_API31OrderNewAutomaticOrderMatchingController.API31OrderNewAutomaticOrderMatching(request).Result;

            //Assert
            Assert.AreEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API31OrderNewAutomaticOrderMatching_Invalid()
        {
            //Arrange
            var request = new API31OrderNewAutomaticOrderMatchingRequest()
            {
                OrderNo = "",
                Symbol = "",
                OrderType = "S",
                Side = "B",
                Text = "DAT LENH INQUIRY REPOS"
            };

            //Act
            var _return = c_API31OrderNewAutomaticOrderMatchingController.API31OrderNewAutomaticOrderMatching(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API31OrderNewAutomaticOrderMatching_Invalid2()
        {
            //Arrange
            var request = new API31OrderNewAutomaticOrderMatchingRequest()
            {
                OrderNo = "",
                Symbol = "XDCR12101",
                Text = "DAT LENH INQUIRY REPOS"
            };

            //Act
            var _return = c_API31OrderNewAutomaticOrderMatchingController.API31OrderNewAutomaticOrderMatching(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API31OrderNewAutomaticOrderMatching_Invalid_Code35_Test()
        {
            //Arrange
            var request = new API31OrderNewAutomaticOrderMatchingRequest()
            {
                OrderNo = "ORDERNO_API31",
                ClientID = "CLIENTID_API31",
                Symbol = "XDCR12101",
                Side = ORDER_SIDE.SIDE_BUY,
                OrderQty = 1000,
                Price = 1000000,
                OrderType = NORMAL_ORDERTYPE.MM,
                SpecialType = -99,
                Text = "DAT LENH INQUIRY REPOS"
            };

            //Act
            var _return = c_API31OrderNewAutomaticOrderMatchingController.API31OrderNewAutomaticOrderMatching(request).Result;

            //Assert
            Assert.AreEqual(ErrorCodeDefine.SpecialType_IsValid, _return.ReturnCode);
        }

        [TestMethod()]
        public void API31OrderNewAutomaticOrderMatching_Invalid_Code35_2_Test()
        {
            //Arrange
            var request = new API31OrderNewAutomaticOrderMatchingRequest()
            {
                OrderNo = "ORDERNO_API31",
                ClientID = "CLIENTID_API31",
                Side = ORDER_SIDE.SIDE_BUY,
                OrderQty = 1000,
                Price = 1000000,
                Symbol = "XDCR12101",
                OrderType = NORMAL_ORDERTYPE.MM,
                SpecialType = ORDER_SPECIALTYPE.LENH_MM_YET_GIA_2_CHIEU,
                OrderQtyMM2 = -1,
                Text = "DAT LENH INQUIRY REPOS"
            };

            //Act
            var _return = c_API31OrderNewAutomaticOrderMatchingController.API31OrderNewAutomaticOrderMatching(request).Result;

            //Assert
            Assert.AreEqual(ErrorCodeDefine.OrderQtyMM2_IsValid, _return.ReturnCode);
        }

        [TestMethod()]
        public void API31OrderNewAutomaticOrderMatching_Invalid_Code36_Test()
        {
            //Arrange
            var request = new API31OrderNewAutomaticOrderMatchingRequest()
            {
                OrderNo = "ORDERNO_API31",
                ClientID = "CLIENTID_API31",
                Side = ORDER_SIDE.SIDE_BUY,
                OrderQty = 1000,
                Price = 1000000,
                Symbol = "XDCR12101",
                OrderType = NORMAL_ORDERTYPE.MM,
                SpecialType = ORDER_SPECIALTYPE.LENH_MM_YET_GIA_2_CHIEU,
                OrderQtyMM2 = 1000,
                PriceMM2 = -1,
                Text = "DAT LENH INQUIRY REPOS"
            };

            //Act
            var _return = c_API31OrderNewAutomaticOrderMatchingController.API31OrderNewAutomaticOrderMatching(request).Result;

            //Assert
            Assert.AreEqual(ErrorCodeDefine.PriceQtyMM2_IsValid, _return.ReturnCode);
        }

        [TestMethod()]
        public void API31OrderNewAutomaticOrderMatching_Exception()
        {
            //Arrange
            var request = new API31OrderNewAutomaticOrderMatchingRequest()
            {
                OrderNo = "16",
                Symbol = null
            };

            //Act
            var _return = c_API31OrderNewAutomaticOrderMatchingController_E.API31OrderNewAutomaticOrderMatching(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        #endregion API31OrderNewAutomaticOrderMatching

        #region API32OrderReplaceAutomaticOrderMatching

        [TestMethod()]
        public void API32OrderReplaceAutomaticOrderMatching_Test()
        {
            //Arrange
            var request = new API32OrderReplaceAutomaticOrderMatchingRequest()
            {
                OrderNo = "NO_32",
                RefExchangeID = "RefExchangeID_32",
                ClientID = "ClientID_32",
                Symbol = "SYMBOL_32",
                OrderQty = 2000,
                OrgOrderQty = 1000,
                Price = 100000,
                Text = "TEST API 32"
            };

            //Act
            var _return = c_API32OrderReplaceAutomaticOrderMatchingController.API32OrderReplaceAutomaticOrderMatching(request).Result;

            //Assert
            Assert.AreEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API32OrderReplaceAutomaticOrderMatching_Invalid()
        {
            //Arrange
            var request = new API32OrderReplaceAutomaticOrderMatchingRequest()
            {
                OrderNo = "",
                Symbol = "",
                Text = "DAT LENH INQUIRY REPOS"
            };

            //Act
            var _return = c_API32OrderReplaceAutomaticOrderMatchingController.API32OrderReplaceAutomaticOrderMatching(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API32OrderReplaceAutomaticOrderMatching_Invalid2()
        {
            //Arrange
            var request = new API32OrderReplaceAutomaticOrderMatchingRequest()
            {
                OrderNo = "",
                Symbol = "XDCR12101",
                Text = "DAT LENH INQUIRY REPOS"
            };

            //Act
            var _return = c_API32OrderReplaceAutomaticOrderMatchingController.API32OrderReplaceAutomaticOrderMatching(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API32OrderReplaceAutomaticOrderMatching_Exception()
        {
            //Arrange
            var request = new API32OrderReplaceAutomaticOrderMatchingRequest()
            {
                OrderNo = "16",
                Symbol = null
            };

            //Act
            var _return = c_API32OrderReplaceAutomaticOrderMatchingController_E.API32OrderReplaceAutomaticOrderMatching(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        #endregion API32OrderReplaceAutomaticOrderMatching

        #region API33OrderCancelAutomaticOrderMatching

        [TestMethod()]
        public void API33OrderCancelAutomaticOrderMatching_Test()
        {
            //Arrange
            var request = new API33OrderCancelAutomaticOrderMatchingRequest()
            {
                OrderNo = "NO_33",
                RefExchangeID = "RefExchangeID_33",
                Symbol = "SYMBOL_32"
            };

            //Act
            var _return = c_API33OrderCancelAutomaticOrderMatchingController.API33OrderCancelAutomaticOrderMatching(request).Result;

            //Assert
            Assert.AreEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API33OrderCancelAutomaticOrderMatching_Invalid()
        {
            //Arrange
            var request = new API33OrderCancelAutomaticOrderMatchingRequest()
            {
                OrderNo = "",
                Symbol = ""
            };

            //Act
            var _return = c_API33OrderCancelAutomaticOrderMatchingController.API33OrderCancelAutomaticOrderMatching(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API33OrderCancelAutomaticOrderMatching_Invalid2()
        {
            //Arrange
            var request = new API33OrderCancelAutomaticOrderMatchingRequest()
            {
                OrderNo = "",
                Symbol = "XDCR12101"
            };

            //Act
            var _return = c_API33OrderCancelAutomaticOrderMatchingController.API33OrderCancelAutomaticOrderMatching(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        [TestMethod()]
        public void API33OrderCancelAutomaticOrderMatching_Exception()
        {
            //Arrange
            var request = new API33OrderCancelAutomaticOrderMatchingRequest()
            {
                OrderNo = "16",
                Symbol = null
            };

            //Act
            var _return = c_API33OrderCancelAutomaticOrderMatchingController_E.API33OrderCancelAutomaticOrderMatching(request).Result;

            //Assert
            Assert.AreNotEqual("000", _return.ReturnCode);
        }

        #endregion API33OrderCancelAutomaticOrderMatching
    }
}