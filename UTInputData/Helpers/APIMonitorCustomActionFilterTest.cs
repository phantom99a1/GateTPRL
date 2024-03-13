using Microsoft.VisualStudio.TestTools.UnitTesting;
using APIServer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CommonLib;
using BusinessProcessAPIReq.RequestModels;
using APIServer.Validation;
using BusinessProcessAPIReq;
using UTInputData.Mock;
using NLog.Filters;

namespace APIMonitor.Helpers.Tests
{
    [TestClass()]
    public class APIMonitorCustomActionFilterTest
    {
        ActionExecutingContext c_ActionExecutingContext;
        CustomActionFilter c_CustomActionFilter = new CustomActionFilter();
        [TestInitialize]
        public void Initialize()
        {
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

            c_ActionExecutingContext.HttpContext.Connection.LocalPort = 1091;
        }

        [TestMethod()]
        public void OnActionExecutingTest()
        {
            //Arrange
            ConfigData.APIMonitorPort = 1091;

            //Act
            c_CustomActionFilter.OnActionExecuting(c_ActionExecutingContext);

            //Assert
            Assert.IsNull(c_ActionExecutingContext.Result);
        }


        [TestMethod()]
        public void OnActionExecutingTest_Fail()
        {
            //Arrange
            ConfigData.APIMonitorPort = 1090;

            //Act
            c_CustomActionFilter.OnActionExecuting(c_ActionExecutingContext);

            //Assert
            Assert.IsNotNull(c_ActionExecutingContext.Result);
        }
    }
}