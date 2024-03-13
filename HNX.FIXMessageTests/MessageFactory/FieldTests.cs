using Microsoft.VisualStudio.TestTools.UnitTesting;
using HNX.FIXMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNX.FIXMessage.Tests
{
    [TestClass()]
    public class FieldTests
    {
        [TestMethod()]
        public void ToStringTest()
        {
            Field field = new Field();
            field.Tag = 1;
            field.Value = "value";
            string _fvalue = field.ToString();

            Assert.IsTrue(_fvalue == "1=value");
        }


        [TestMethod()]
        public void ToStringTest2()
        {
            Field field = new Field(1, "value");
            
            string _fvalue = field.ToString();

            Assert.IsTrue(_fvalue == "1=value");
        }
    }
}