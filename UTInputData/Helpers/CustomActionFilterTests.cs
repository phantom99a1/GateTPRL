using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CommonLib;
using BusinessProcessAPIReq.RequestModels;
using Moq;
using HNXInterface;

namespace APIServer.Helpers.Tests
{
    [TestClass()]
    public class CustomActionFilterTests
    {
        Mock<iHNXClient> mockHNXClient = new Mock<iHNXClient>();
        ActionExecutingContext c_ActionExecutingContext;
        CustomActionFilter c_CustomActionFilter; 
        [TestInitialize]
        public void Initialize()
        {
            c_CustomActionFilter = new CustomActionFilter(mockHNXClient.Object);
            ActionContext _ActionContext = new ActionContext(
           new Microsoft.AspNetCore.Http.DefaultHttpContext(),
           new Microsoft.AspNetCore.Routing.RouteData(),
          new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor(),
          new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()
          );


            Dictionary<string, object?> _actionArguments = new Dictionary<string, object?>();
            _actionArguments.Add("request", new API1NewElectronicPutThroughRequest());

            List<IFilterMetadata> _filters = new List<IFilterMetadata>();
            _filters.Add(c_CustomActionFilter);

            c_ActionExecutingContext = new ActionExecutingContext(_ActionContext, _filters, _actionArguments, null);

            c_ActionExecutingContext.HttpContext.Connection.LocalPort = 1090;
            c_ActionExecutingContext.HttpContext.Request.Headers.Add("service_sec_key", "7ajntYzxGns8TVaodIgl8w==");
        }

        [TestMethod()]
        public void OnActionExecutingTest()
        {
            //Arrange
            ConfigData.TokenSecret = "7ajntYzxGns8TVaodIgl8w==";
            ConfigData.APIBusinessPort = 1090;
            mockHNXClient.Setup(helper => helper.ClientStatus()).Returns(enumClientStatus.DATA_TRANSFER);
            //Act
            c_CustomActionFilter.OnActionExecuting(c_ActionExecutingContext);

            //Assert
            Assert.IsNull(c_ActionExecutingContext.Result);
        }


        [TestMethod()]
        public void OnActionExecutingTest_Fail()
        {
            //Arrange
            ConfigData.TokenSecret = "7ajntYzxGns8TVaodIgl8w==1";
            ConfigData.APIBusinessPort = 1091;

            //Act
            c_CustomActionFilter.OnActionExecuting(c_ActionExecutingContext);

            //Assert
            Assert.IsNotNull(c_ActionExecutingContext.Result);
        }
    }
}