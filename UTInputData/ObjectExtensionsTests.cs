using CommonLib;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UTInputData
{

    [TestClass()]
    public class ObjectExtensionsTests
    {

        [TestMethod()]
        public void GetPropertyTest()
        {
            //Arrange 
            ObjectExtensionsClass objectExtensionsClass = new ObjectExtensionsClass();
            objectExtensionsClass.OrderNo = "OrderNo1";
            objectExtensionsClass.ClientID = "ClientID11";

            //Act
            var _return1 = ObjectExtensions.GetProperty(null, "OrderNo");

            var _return2 = (PropertyInfo)ObjectExtensions.GetProperty(objectExtensionsClass, "OrderNo");
            var _return3 = (PropertyInfo)ObjectExtensions.GetProperty(objectExtensionsClass, "ORDERNO", true );
            var _return4 = (PropertyInfo)ObjectExtensions.GetProperty(objectExtensionsClass, "orderno", true);

            //Asset
            Assert.IsNull (_return1);
            Assert.AreEqual ("OrderNo", _return2.Name );
            Assert.IsNull(_return3);
            Assert.IsNull(_return4);

        }

        [TestMethod()]
        public void SetPropertyValueTest()
        {
            //Arrange 
            ObjectExtensionsClass objectExtensionsClass = new ObjectExtensionsClass();
            objectExtensionsClass.OrderNo = "OrderNo1";
            objectExtensionsClass.ClientID = "ClientID11";

            //Act
              ObjectExtensions.SetPropertyValue(objectExtensionsClass, "OrderNo", "OrderNo2");
            var _return =  ObjectExtensions.GetPropertyValue(objectExtensionsClass, "OrderNo");

            //Asset
            Assert.AreEqual("OrderNo2", _return.ToString());

        }


        private  class ObjectExtensionsClass
        {
            public string OrderNo { get; set; } = string.Empty;
            public string ClientID { get; set; } = string.Empty;
        }
    }



  

}
