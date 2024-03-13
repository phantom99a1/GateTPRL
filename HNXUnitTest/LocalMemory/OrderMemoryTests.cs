using LocalMemory;
using Microsoft.VisualBasic;

namespace HNXUnitTest.LocalMemory
{
    [TestClass()]
    public class OrderMemoryTests
    {
        [TestInitialize]
        public void Initc_ListOrder()
        {
            if (OrderMemory.GetOrder_bySeqNum(99) == null)
            {
                OrderInfo _OrderInfo = UltilTestClass<OrderInfo>.CreateObjectFromType();
                _OrderInfo.ClOrdID = "ClOrdID1";
                _OrderInfo.OrderNo = "OrderNo1";
                _OrderInfo.SeqNum = 99;
                _OrderInfo.ExchangeID = "ExchangeID1";
                _OrderInfo.RefExchangeID = "RefExchangeID1";
                _OrderInfo.Symbol = "ACB";
                _OrderInfo.Price = 10000;
                _OrderInfo.OrderQty = 100;

                OrderMemory.Add_NewOrder(_OrderInfo);
                OrderMemory.Update_Order("ClOrdID1", 99);
            }

            if (OrderMemory.GetOrder_bySeqNum(100) == null)
            {
                OrderInfo _OrderInfo = UltilTestClass<OrderInfo>.CreateObjectFromType();
                _OrderInfo.ClOrdID = "ClOrdID3";
                _OrderInfo.OrderNo = "OrderNo3";
                _OrderInfo.SeqNum = 100;
                _OrderInfo.Symbol = "ACB";
                _OrderInfo.Price = 10000;
                _OrderInfo.OrderQty = 100;
                _OrderInfo.RefMsgType = "s";
                OrderMemory.Add_NewOrder(_OrderInfo);
                OrderMemory.Update_Order("ClOrdID2", 100);
            }
        }

        [TestMethod]
        public void InitObjectOrderInfoTest()
        {
            OrderInfo _OrderInfo = UltilTestClass<OrderInfo>.CreateObjectFromType();
            UltilTestClass<OrderInfo>.TestGetProperty(_OrderInfo);

            Assert.IsTrue(true);
        }

        [TestMethod]
        [DataRow("ClOrdID1", true)]
        [DataRow("ClOrdID2", false)]
        public void Update_OrderTest(string p_ClodrID, bool p_Epx)
        {
            //Arrange
            //Initc_ListOrder();

            //Act
            bool _return = OrderMemory.Update_Order(p_ClodrID, 1900);
            Assert.AreEqual(p_Epx, _return);
        }

        [TestMethod]
        [DataRow("ClOrdID1", true)]
        [DataRow("ClOrdID2", false)]
        public void CheckClOdrIDExistTest(string p_ClodrID, bool p_Epx)
        {
            //Arrange
            //Initc_ListOrder();

            //Act
            bool _return = OrderMemory.CheckCldOrIDExist(p_ClodrID); 
            Assert.AreEqual(p_Epx, _return);
        }

        [TestMethod]
        [DataRow("OrderNo1", true)]
        [DataRow("OrderNo2", false)]
        public void GetOrder_ByOrderNoTest(string p_OrderNo, bool p_Epx)
        {
            //Arrange
            //Initc_ListOrder();

            //Act
            var _return = OrderMemory.GetOrder_ByOrderNo(p_OrderNo);
            Assert.AreEqual(p_Epx, _return != null);
        }

        [TestMethod]
        [DataRow("ClOrdID1", true)]
        [DataRow("ClOrdID2", false)]
        public void GetOrder_byClOrdIDTest(string p_ClodrID, bool p_Epx)
        {
            //Arrange
            //Initc_ListOrder();

            //Act
            var _return = OrderMemory.GetOrder_byClOrdID(p_ClodrID);
            Assert.AreEqual(p_Epx, _return != null);
        }

        [TestMethod]
        [DataRow("ClOrdID1", "OrderNo1")]
        [DataRow("ClOrdID2", "")]
        public void GetOrigOrder_byClOrdIDTest(string p_ClodrID, string p_Epx)
        {
            //Arrange
            //Initc_ListOrder();

            //Act
            var _return = OrderMemory.GetOrigOrder_byClOrdID(p_ClodrID);
            Assert.AreEqual(p_Epx, _return);
        }

        [TestMethod]
        [DataRow("OrderNo1", true)]
        [DataRow("OrderNo2", false)]
        public void IsExist_OrderNoTest(string p_OrderNo1, bool p_Epx)
        {
            //Arrange
            //Initc_ListOrder();

            //Act
            var _return = OrderMemory.IsExist_OrderNo(p_OrderNo1);
            Assert.AreEqual(p_Epx, _return);
        }

        [TestMethod]
        [DataRow(99, true)]
        [DataRow(53, false)]
        public void GetOrder_bySeqNumTest(int p_SeqNum, bool p_Epx)
        {
            //Arrange
            //Initc_ListOrder();

            //Act
            var _return = OrderMemory.GetOrder_bySeqNum(p_SeqNum);
            Assert.AreEqual(p_Epx, _return != null);
        }

        [TestMethod]
        [DataRow("ExchangeID1", true)]
        [DataRow("ExchangeID12", false)]
        public void GetOrder_byExchangeIDTest(string p_ExchangeID, bool p_Epx)
        {
            //Arrange
            //Initc_ListOrder();

            //Act
            var _return = OrderMemory.GetOrder_byExchangeID(p_ExchangeID);
            Assert.AreEqual(p_Epx, _return != null);
        }

        //[TestMethod()]
        //[DataRow("RefExchangeID1", true)]
        //[DataRow("RefExchangeID2", false)]
        //public void GetOrder_ByRefExchangeIDTest(string p_RefExchangeID, bool p_Epx)
        //{
        //    //Arrange
        //    //Initc_ListOrder();

        //    //Act
        //    var _return = OrderMemory.GetOrder_ByRefExchangeID(p_RefExchangeID);
        //    Assert.AreEqual(p_Epx, _return != null);
        //}

        //[TestMethod]
        //[DataRow("ACB", 10000, 100, "OrderNo3", true)]
        //[DataRow("ACB1", 1000, 200, "101", false)]
        //public void GetOrder_By_Symbol_Price_Volume_RefMsgType_OrderNo_Test(string p_Symbol, long p_Price, int p_Volume, string p_OrderNo, bool p_Epx)
        //{
        //    //Arrange
        //    //Initc_ListOrder();

        //    //Act
        //    var _return = OrderMemory.GetOrder_By_Symbol_Price_Volume_RefMsgType_OrderNo(p_Symbol, p_Price, p_Volume, "s", p_OrderNo);
        //    Assert.AreEqual(p_Epx, _return != null);
        //}
    }
}