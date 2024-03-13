using Microsoft.VisualStudio.TestTools.UnitTesting;
using LocalMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
using HNX.FIXMessage;
using Microsoft.Extensions.Configuration;
using static CommonLib.CommonData;
using static CommonLib.CommonData.TradingSessionStatus;

namespace HNXUnitTest.LocalMemory
{
    [TestClass()]
    public class TradingRuleDataTests
    {

        [TestInitialize]
        public void InitEnviroment()
        {
            //1 Add TradingSession theo bảng
            MessageTradingSessionStatus _SessionStatus = new MessageTradingSessionStatus()
            {
                TradSesReqID = "CBTS",
                TradSesMode = TradingSessionMode.byTable,
                TradSesStatus = TradingSessionStatus.InTradingSession
            };
            MessageTradingSessionStatus _SessionStatus_Test1 = new MessageTradingSessionStatus()
            {
                TradSesReqID = ConfigData.MainBoard,
                TradSesMode = TradingSessionMode.byTable,
                TradSesStatus = TradingSessionStatus.InTradingSession
            };
            TradingRuleData.ProcessTradingSession(_SessionStatus);
            TradingRuleData.ProcessTradingSession(_SessionStatus_Test1);



            //2 Add SecurityStatus
            MessageSecurityStatus _SecurityStatus1 = new MessageSecurityStatus() {
                TradingSessionSubID = "CBTS",
                Symbol = "AAAR12101"

            };

            MessageSecurityStatus _SecurityStatus2 = new MessageSecurityStatus()
            {
                TradingSessionSubID = "CBTS",
                Symbol = "AAAR12102"

            };

            MessageSecurityStatus _SecurityStatus3 = new MessageSecurityStatus()
            {
                TradingSessionSubID = "bangtest2",
                Symbol = "BBBR12101"

            };

            TradingRuleData.ProcessSecurityStatus(_SecurityStatus1);
            TradingRuleData.ProcessSecurityStatus(_SecurityStatus2);
            TradingRuleData.ProcessSecurityStatus(_SecurityStatus3);

            //3 Add TradingSession theo chứng khoán
            MessageTradingSessionStatus _SessionStatus_bySymbol = new MessageTradingSessionStatus()
            {
                TradSesReqID = "AAAR12102",
                TradSesMode = TradingSessionMode.bySymbol ,
                TradSesStatus = TradingSessionStatus.InTradingSession
            };
            MessageTradingSessionStatus _SessionStatus__bySymbolTest1 = new MessageTradingSessionStatus()
            {
                TradSesReqID = "BBBR12102",
                TradSesMode = TradingSessionMode.bySymbol ,
                TradSesStatus = TradingSessionStatus.InTradingSession,
                TradingSessionID = "bangtest2"
            };
            TradingRuleData.ProcessTradingSession(_SessionStatus_bySymbol);
            TradingRuleData.ProcessTradingSession(_SessionStatus__bySymbolTest1);


        }



        [TestMethod()]
        [DataRow("AAAR12101",  true)]
        [DataRow("BIDLH23310101",  false )]
        [DataRow("",  false )]
        public void CheckTradingRule_InputTest(string p_Symbol, bool p_Exp)
        {
            //Arrange 
            string _Text = "";
            string _Code = "";
            //Act
            var _return = TradingRuleData.CheckTradingRule_Input(p_Symbol, out _Text, out _Code );

            //Asset
            Assert.AreEqual(p_Exp, _return);
        }


        [TestMethod()]
        [DataRow("AAAR12101", true)]
        [DataRow("BIDLH23310101", false)]
        [DataRow("", true)]
        public void CheckTradingRule_OutputTest(string p_Symbol, bool p_Exp)
        {
            //Arrange 
            string _Text = "";
            string _Code = "";
            //Act
            var _return = TradingRuleData.CheckTradingRule_Output(p_Symbol, out _Text, out _Code);

            //Asset
            Assert.AreEqual(p_Exp, _return);
        }


        [TestMethod()]
        [DataRow("CBTS", TradingSessionStatus.InTradingSession)]
        [DataRow("CBTS1", TradingSessionStatus.BeforeTradingSession)]
        public void GetTradeSesStatusofMainBoardTest(string p_Board, string p_Exp)
        {
            //Arrange 
           
            //Act
            ConfigData.MainBoard = p_Board;
            var _return = TradingRuleData.GetTradeSesStatusofMainBoard( );

            //Asset
            Assert.AreEqual(p_Exp, _return);
        }
        [TestMethod]
        public void TestStopBoard()
        {
            MessageFactoryFIX c_MsgFactory  = new MessageFactoryFIX();
            MessageSecurityStatus SecurityStatus = (MessageSecurityStatus)c_MsgFactory.Parse("8=FIX.4.4\u00019=251\u000135=f\u000149=HNX\u000156=043.01GW\u000134=9\u0001369=0\u000152=20230928-08:43:15\u0001324=\u000155=VPBR11901\u0001167=\u0001541=20261219\u0001225=20191219\u0001106=TCPHCBIS\u000131=1000000000\u0001332=0\u0001333=0\u00013321=0\u00013331=0\u00013322=1100000000\u00013332=0\u0001326=0\u0001330=0\u0001625=CBTS\u00016251=0\u0001265=108\u0001109=700\u00019735=1,2,3,4,5,6\u00019736=1,2\u000110=252\u0001");
            TradingRuleData.ProcessSecurityStatus(SecurityStatus);
            SecurityStatus = (MessageSecurityStatus)c_MsgFactory.Parse("8=FIX.4.4\u00019=242\u000135=f\u000149=HNX\u000156=043.01GW\u000134=10\u0001369=0\u000152=20230928-08:43:15\u0001324=\u000155=XDCR12102\u0001167=\u0001541=20250409\u0001225=20210409\u0001106=TCPHCBIS\u000131=100000\u0001332=0\u0001333=0\u00013321=0\u00013331=0\u00013322=110000\u00013332=0\u0001326=0\u0001330=0\u0001625=DEMO\u00016251=0\u0001265=108\u0001109=11000000\u00019735=1,2\u00019736=1,2\u000110=032\u0001");
            TradingRuleData.ProcessSecurityStatus(SecurityStatus);

            //Setup chuyển phiên 2 bảng
            TradingRuleData.ProcessTradingSession((MessageTradingSessionStatus)c_MsgFactory.Parse("8=FIX.4.4\u00019=110\u000135=h\u000149=HNX\u000156=043.01GW\u000134=110\u0001369=2\u000152=20230928-08:45:28\u0001336=DEMO\u0001340=1\u0001341=20230928-08:45:27\u0001339=1\u0001335=DEMO\u000110=254\u0001"));
            TradingRuleData.ProcessTradingSession((MessageTradingSessionStatus)c_MsgFactory.Parse("8=FIX.4.4\u00019=115\u000135=h\u000149=HNX\u000156=043.01GW\u000134=90\u0001369=2\u000152=20230928-08:45:17\u0001336=STATUS_NTN\u0001340=1\u0001341=20230928-08:45:16\u0001339=1\u0001335=CBTS\u000110=235\u0001"));
            
            
            //Setup tạm dừng 2 bảng
            TradingRuleData.ProcessTradingSession((MessageTradingSessionStatus)c_MsgFactory.Parse("8=FIX.4.4\u00019=110\u000135=h\u000149=HNX\u000156=043.01GW\u000134=110\u0001369=2\u000152=20230928-08:45:28\u0001336=DEMO\u0001340=2\u0001341=20230928-08:45:27\u0001339=1\u0001335=DEMO\u000110=254\u0001"));
            TradingRuleData.ProcessTradingSession((MessageTradingSessionStatus)c_MsgFactory.Parse("8=FIX.4.4\u00019=115\u000135=h\u000149=HNX\u000156=043.01GW\u000134=90\u0001369=2\u000152=20230928-08:45:17\u0001336=STATUS_NTN\u0001340=2\u0001341=20230928-08:45:16\u0001339=1\u0001335=CBTS\u000110=235\u0001"));
            //Assert check phiên mã ck
            string Text = "";
            string Code = "";
            TradingRuleData.CheckTradingRule_Input("VPBR11901", out Text, out Code);
            Assert.AreEqual(Text, CommonData.ORDER_RETURNMESSAGE.MARKET_IN_BREAK_TIME);
            Assert.AreEqual(Code, CommonData.ORDER_RETURNCODE.MARKET_IN_BREAK_TIME);

            TradingRuleData.CheckTradingRule_Input("XDCR12102", out Text, out Code);
            Assert.AreEqual(Text, CommonData.ORDER_RETURNMESSAGE.MARKET_IN_BREAK_TIME);
            Assert.AreEqual(Code, CommonData.ORDER_RETURNCODE.MARKET_IN_BREAK_TIME);

            //Setup mở lại bảng demo
            TradingRuleData.ProcessTradingSession((MessageTradingSessionStatus)c_MsgFactory.Parse("8=FIX.4.4\u00019=110\u000135=h\u000149=HNX\u000156=043.01GW\u000134=110\u0001369=2\u000152=20230928-08:45:28\u0001336=DEMO\u0001340=1\u0001341=20230928-08:45:27\u0001339=1\u0001335=DEMO\u000110=254\u0001"));
            TradingRuleData.ProcessTradingSession((MessageTradingSessionStatus)c_MsgFactory.Parse("8=FIX.4.4\u00019=115\u000135=h\u000149=HNX\u000156=043.01GW\u000134=90\u0001369=2\u000152=20230928-08:45:17\u0001336=STATUS_NTN\u0001340=2\u0001341=20230928-08:45:16\u0001339=1\u0001335=CBTS\u000110=235\u0001"));


            //Assert check lại mã phiên ck
            TradingRuleData.CheckTradingRule_Input("VPBR11901", out Text, out Code);
            Assert.AreEqual(Text, CommonData.ORDER_RETURNMESSAGE.MARKET_IN_BREAK_TIME);
            Assert.AreEqual(Code, CommonData.ORDER_RETURNCODE.MARKET_IN_BREAK_TIME);

            TradingRuleData.CheckTradingRule_Input("XDCR12102", out Text, out Code);
            Assert.AreEqual(Text, CommonData.ORDER_RETURNMESSAGE.SUCCESS);
            Assert.AreEqual(Code, CommonData.ORDER_RETURNCODE.SUCCESS);
        }
    }
   
}