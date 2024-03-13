using LocalMemory;
using static CommonLib.CommonData;

namespace HNXUnitTest.LocalMemory
{
    [TestClass()]
    public class BoardTests
    {
        [TestMethod()]
        [DataRow("ACB1", "1", true)]
        [DataRow("ACB2", "1", false)]
        [DataRow("ACB3", "1", false)]
        [DataRow("ACB4", "1", false)]
        [DataRow("ACB5", "1", false)]
        [DataRow("ACB6", "1", false)]
        [DataRow("ACB1", "2", false)]
        public void CheckAcceptOrderTest(string Symbol, string TradeSesStatus, bool exp)
        {
            Board _Board = new Board();
            _Board.BoardCode = "";
            _Board.TradeSesStatus = TradeSesStatus;
            _Board.ListBOND.Add("ACB1", new BOND() { Name = "ACB1", TradingSession = "", TradeSesStatus = "1" });
            _Board.ListBOND.Add("ACB2", new BOND() { Name = "ACB2", TradingSession = "", TradeSesStatus = "90" });
            _Board.ListBOND.Add("ACB3", new BOND() { Name = "ACB3", TradingSession = "", TradeSesStatus = "13" });
            _Board.ListBOND.Add("ACB4", new BOND() { Name = "ACB3", TradingSession = "", TradeSesStatus = "97" });
            _Board.ListBOND.Add("ACB5", new BOND() { Name = "ACB3", TradingSession = "", TradeSesStatus = "2" });

            string _outText = "";
            string _outCode = "";
            var _return = _Board.CheckAcceptOrder(Symbol, out _outText, out _outCode);

            Assert.AreEqual(exp, _return);
        }

        [TestMethod()]
        public void InitObjectBONDTest()
        {
            BOND _BOND = UltilTestClass<BOND>.CreateObjectFromType();
            UltilTestClass<BOND>.TestGetProperty(_BOND);

            Assert.IsTrue(true);
        }

        [TestMethod()]
        [DataRow("1", ORDER_RETURNCODE.SUCCESS)]
        [DataRow("0", ORDER_RETURNCODE.MARKET_IN_BREAK_TIME)]
        [DataRow("2", ORDER_RETURNCODE.MARKET_IN_BREAK_TIME)]
        [DataRow("13", ORDER_RETURNCODE.MARKET_CLOSE)]
        [DataRow("90", ORDER_RETURNCODE.BOND_GATEWAY_HAS_NO_SESSION)]
        [DataRow("97", ORDER_RETURNCODE.MARKET_CLOSE)]
        public void CheckTradingSessionTest(string p_TradeSesStatus, string p_exp)
        {
            //Arrange
            Board _Board = new Board();
            _Board.BoardCode = "bangtest";
            _Board.TradeSesStatus = p_TradeSesStatus;
            string _outText = "";
            string _outCode = "";

            //Act
            var _return = _Board.CheckTradingSession(out _outText, out _outCode);

            //Assert
            Assert.AreEqual(p_exp, _outCode);
            Assert.AreEqual("bangtest", _Board.BoardCode);
        }
    }
}