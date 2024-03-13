using HNX.FIXMessage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HNX.FIXMessageTests.SessionMessage
{
    [TestClass]
    public class TestExecutionReport
    {
        public MessageFactoryFIX c_MsgFactoryFIX = new MessageFactoryFIX();

        //Lệnh khớp
        [TestMethod]
        public void TestER_ExecOrder()
        {
            //Khớp cùng công ty khi đặt lệnh TTDT outright

            //Arrange
            MessageER_ExecOrder orgER_Exec = (MessageER_ExecOrder)c_MsgFactoryFIX.Parse("8=FIX.4.4\u00019=236\u000135=8\u000149=HNX\u000156=043.01GW\u000134=80\u0001369=27\u000152=20231018-04:16:41\u0001150=3\u000139=2\u000111=TEST_20\u000141=XDCR12101-2311070000012\u0001526=XDCR12101-2311070000012\u000137=XDCR12101-2311070000008\u000132=10\u000131=10\u000117=XDCR12101-2311070000008\u000155=XDCR12101\u000154=2\u00016464=100\u0001448=043\u000110=228\u0001");
            //Act
            string msgER_ExecRaw = c_MsgFactoryFIX.Build(orgER_Exec);
            MessageER_ExecOrder testER_Exec = (MessageER_ExecOrder)c_MsgFactoryFIX.Parse(msgER_ExecRaw);
            //Assert
            Assert.AreEqual(orgER_Exec.Price2, testER_Exec.Price2);
            Assert.AreEqual(orgER_Exec.OrderQty, testER_Exec.OrderQty);
            Assert.AreEqual(orgER_Exec.OrderID, testER_Exec.OrderID);
            Assert.AreEqual(orgER_Exec.Symbol, testER_Exec.Symbol);
            Assert.AreEqual(orgER_Exec.Side, testER_Exec.Side);

            //Khớp khác công ty khi đặt TTDT outright
            //Arrange
            orgER_Exec = (MessageER_ExecOrder)c_MsgFactoryFIX.Parse("8=FIX.4.4\u00019=219\u000135=8\u000149=HNX\u000156=018.01GW\u000134=2\u0001369=0\u000152=20230811-09:48:14\u0001150=3\u000139=2\u000141=BABR12001-2307170000001\u0001526=BABR12001-2307170000001\u000137=BABR12001-2307170000001\u000132=10\u000131=3000\u000117=BABR12001-2307170000001\u000155=BABR12001\u000154=1\u00016464=30000\u000110=174\u0001");
            //Act
            msgER_ExecRaw = c_MsgFactoryFIX.Build(orgER_Exec);
            testER_Exec = (MessageER_ExecOrder)c_MsgFactoryFIX.Parse(msgER_ExecRaw);
            //Assert
            Assert.AreEqual(orgER_Exec.Price2, testER_Exec.Price2);
            Assert.AreEqual(orgER_Exec.OrderQty, testER_Exec.OrderQty);
            Assert.AreEqual(orgER_Exec.OrderID, testER_Exec.OrderID);
            Assert.AreEqual(orgER_Exec.Symbol, testER_Exec.Symbol);
            Assert.AreEqual(orgER_Exec.Side, testER_Exec.Side);

            //Khớp cùng công ty khi đặt BCGD Outright
            //Arrange
            orgER_Exec = (MessageER_ExecOrder)c_MsgFactoryFIX.Parse("8=FIX.4.4\u00019=263\u000135=8\u000149=HNX\u000156=050.02GW\u000134=7\u0001369=3\u000152=20230816-07:10:49\u0001150=3\u000139=2\u000111=e85accad-51f1-4a39-ad10-36be3aa06db1\u000141=XDCR12101-2307280000003\u0001526=XDCR12101-2307280000003\u000137=XDCR12101-2307280000003\u000132=1000\u000131=1000\u000117=XDCR12101-2307280000003\u000155=XDCR12101\u000154=2\u00016464=1000000\u000110=190\u0001");
            //Act
            msgER_ExecRaw = c_MsgFactoryFIX.Build(orgER_Exec);
            testER_Exec = (MessageER_ExecOrder)c_MsgFactoryFIX.Parse(msgER_ExecRaw);
            //Assert
            Assert.AreEqual(orgER_Exec.Price2, testER_Exec.Price2);
            Assert.AreEqual(orgER_Exec.OrderQty, testER_Exec.OrderQty);
            Assert.AreEqual(orgER_Exec.OrderID, testER_Exec.OrderID);
            Assert.AreEqual(orgER_Exec.SettlValue, testER_Exec.SettlValue);
            Assert.AreEqual(orgER_Exec.SettDate, testER_Exec.SettDate);
            Assert.AreEqual(orgER_Exec.Symbol, testER_Exec.Symbol);
            Assert.AreEqual(orgER_Exec.Side, testER_Exec.Side);

            //Khớp khác công ty khi đặt BCGD Outright
            //Arrange
            orgER_Exec = (MessageER_ExecOrder)c_MsgFactoryFIX.Parse("8=FIX.4.4\u00019=223\u000135=8\u000149=HNX\u000156=050.02GW\u000134=6\u0001369=2\u000152=20230816-07:01:49\u0001150=3\u000139=2\u000141=XDCR12101-2307280000002\u0001526=XDCR12101-2307280000002\u000137=XDCR12101-2307280000002\u000132=1000\u000131=1000\u000117=XDCR12101-2307280000002\u000155=XDCR12101\u000154=2\u00016464=1000000\u000110=252\u0001");
            //Act
            msgER_ExecRaw = c_MsgFactoryFIX.Build(orgER_Exec);
            testER_Exec = (MessageER_ExecOrder)c_MsgFactoryFIX.Parse(msgER_ExecRaw);
            //Assert
            Assert.AreEqual(orgER_Exec.Price2, testER_Exec.Price2);
            Assert.AreEqual(orgER_Exec.OrderQty, testER_Exec.OrderQty);
            Assert.AreEqual(orgER_Exec.OrderID, testER_Exec.OrderID);
            Assert.AreEqual(orgER_Exec.Symbol, testER_Exec.Symbol);
            Assert.AreEqual(orgER_Exec.SettlValue, testER_Exec.SettlValue);
            Assert.AreEqual(orgER_Exec.SettDate, testER_Exec.SettDate);
            Assert.AreEqual(orgER_Exec.Side, testER_Exec.Side);
        }

        //Phản hồi lệnh sửa BCGD
        [TestMethod]
        public void TestER_Replace()
        {
            MessageER_OrderReplace originER_Replace;
            string msg_ER_Raw;
            MessageER_OrderReplace testER_Replace;

            //Xác nhận BCGD 2 Firm
            originER_Replace = (MessageER_OrderReplace)c_MsgFactoryFIX.Parse("8=FIX.4.4\u00019=229\u000135=8\u000149=HNX\u000156=050.02GW\u000134=39\u0001369=6\u000152=20230906-07:24:01\u0001150=5\u000139=3\u000111=99ef1acd-3c23-4ef4-93d9-2b1c8c04c7bb\u000141=XDCR12101-2308250000012\u000137=XDCR12101-2308250000013\u00011=050C111111\u000155=XDCR12101\u000154=2\u000140=R\u000132=100\u000131=100\u0001151=0\u00016464=10000\u000110=076\u0001");
            //Act
            msg_ER_Raw = c_MsgFactoryFIX.Build(originER_Replace);
            testER_Replace = (MessageER_OrderReplace)c_MsgFactoryFIX.Parse(msg_ER_Raw);
            //Assert
            Assert.AreEqual(originER_Replace.OrigClOrdID, testER_Replace.OrigClOrdID);
            Assert.AreEqual(originER_Replace.Symbol, testER_Replace.Symbol);
            Assert.AreEqual(originER_Replace.Side, testER_Replace.Side);
            Assert.AreEqual(originER_Replace.OrderQty, testER_Replace.OrderQty);

            //Sửa TTDT cùng cong ty
            originER_Replace = (MessageER_OrderReplace)c_MsgFactoryFIX.Parse("8=FIX.4.4\u00019=229\u000135=8\u000149=HNX\u000156=018.01GW\u000134=7\u0001369=0\u000152=20230811-10:17:39\u0001150=5\u000139=A\u000111=4422c692-55fa-420e-aba3-f8bffdad7c79\u000141=BABR12001-2307170000004\u000137=BABR12001-2307170000005\u00011=018C111111\u000155=BABR12001\u000154=2\u000140=R\u000132=5\u000131=50000\u0001151=0\u00016464=250000\u000110=023\u0001");
            //Act
            msg_ER_Raw = c_MsgFactoryFIX.Build(originER_Replace);
            testER_Replace = (MessageER_OrderReplace)c_MsgFactoryFIX.Parse(msg_ER_Raw);
            //Assert
            Assert.AreEqual(originER_Replace.OrigClOrdID, testER_Replace.OrigClOrdID);
            Assert.AreEqual(originER_Replace.Symbol, testER_Replace.Symbol);
            Assert.AreEqual(originER_Replace.Side, testER_Replace.Side);
            Assert.AreEqual(originER_Replace.OrderQty, testER_Replace.OrderQty);

            //Sửa thỏa thuận đã thực hiện khác công ty
            originER_Replace = (MessageER_OrderReplace)c_MsgFactoryFIX.Parse("8=FIX.4.4\u00019=233\u000135=8\u000149=HNX\u000156=018.01GW\u000134=32\u0001369=0\u000152=20230812-02:59:54\u0001150=5\u000139=3\u000111=eac6a915-79db-49a7-8a4b-4fe3c701aae6\u000141=BABR12001-2307180000001\u000137=BABR12001-2307180000002\u00011=018C111111\u000155=BABR12001\u000154=1\u000140=R\u000132=10\u000131=300000\u0001151=0\u00016464=3000000\u000110=185\u0001");
            //Act
            msg_ER_Raw = c_MsgFactoryFIX.Build(originER_Replace);
            testER_Replace = (MessageER_OrderReplace)c_MsgFactoryFIX.Parse(msg_ER_Raw);
            //Assert
            Assert.AreEqual(originER_Replace.OrigClOrdID, testER_Replace.OrigClOrdID);
            Assert.AreEqual(originER_Replace.Symbol, testER_Replace.Symbol);
            Assert.AreEqual(originER_Replace.Side, testER_Replace.Side);
            Assert.AreEqual(originER_Replace.OrderQty, testER_Replace.OrderQty);

        }

        //Xác nhận lệnh hủy
        [TestMethod]
        public void TestER_Cancel()
        {
            MessageER_OrderCancel originER_Replace;
            string msg_ER_Raw;
            MessageER_OrderCancel testER_Replace;

            //Hủy lệnh thỏa thuận đã khớp cùng cty 
            originER_Replace = (MessageER_OrderCancel)c_MsgFactoryFIX.Parse("8=FIX.4.4\u00019=214\u000135=8\u000149=HNX\u000156=018.01GW\u000134=40\u0001369=0\u000152=20230812-03:47:24\u0001150=4\u000139=A\u000111=06df6388-f9cc-47c6-841e-f4e3dacc6a72\u000141=BABR12001-2307180000006\u000137=BABR12001-2307180000007\u0001151=8\u00011=018CX00001\u000155=BABR12001\u000154=1\u000140=R\u000144=300000\u000110=109\u0001");
            //Act
            msg_ER_Raw = c_MsgFactoryFIX.Build(originER_Replace);
            testER_Replace = (MessageER_OrderCancel)c_MsgFactoryFIX.Parse(msg_ER_Raw);
            //Assert
            Assert.AreEqual(originER_Replace.OrigClOrdID, testER_Replace.OrigClOrdID);
            Assert.AreEqual(originER_Replace.Symbol, testER_Replace.Symbol);
            Assert.AreEqual(originER_Replace.Side, testER_Replace.Side);
            Assert.AreEqual(originER_Replace.OrderQty, testER_Replace.OrderQty);

            //Hủy TTDT khác conong ty dã khớp
            originER_Replace = (MessageER_OrderCancel)c_MsgFactoryFIX.Parse("8=FIX.4.4\u00019=209\u000135=8\u000149=HNX\u000156=018.01GW\u000134=28\u0001369=0\u000152=20230814-03:41:25\u0001150=4\u000139=3\u000111=19815701-1f78-48da-8397-324604dfe75f\u000141=AAAR12101-2307240000001\u000137=AAAR12101-2307240000002\u00011=018C111111\u000155=AAAR12101\u000154=1\u000140=R\u000144=3000000\u000110=008\u0001");
            //Act
            msg_ER_Raw = c_MsgFactoryFIX.Build(originER_Replace);
            testER_Replace = (MessageER_OrderCancel)c_MsgFactoryFIX.Parse(msg_ER_Raw);
            //Assert
            Assert.AreEqual(originER_Replace.OrigClOrdID, testER_Replace.OrigClOrdID);
            Assert.AreEqual(originER_Replace.Symbol, testER_Replace.Symbol);
            Assert.AreEqual(originER_Replace.Side, testER_Replace.Side);
            Assert.AreEqual(originER_Replace.OrderQty, testER_Replace.OrderQty);

            //Hủy BCGD đã thực hiện
            originER_Replace = (MessageER_OrderCancel)c_MsgFactoryFIX.Parse("8=FIX.4.4\u00019=207\u000135=8\u000149=HNX\u000156=018.01GW\u000134=98\u0001369=0\u000152=20230825-07:25:56\u0001150=4\u000139=5\u000111=25061d2b-0a17-4dd2-a51a-18c7053aeb58\u000141=AAAR12101-2308100000001\u000137=AAAR12101-2308100000002\u00011=018C111111\u000155=AAAR12101\u000154=2\u000140=R\u000144=46000\u000110=074\u0001");
            //Act
            msg_ER_Raw = c_MsgFactoryFIX.Build(originER_Replace);
            testER_Replace = (MessageER_OrderCancel)c_MsgFactoryFIX.Parse(msg_ER_Raw);
            //Assert
            Assert.AreEqual(originER_Replace.OrigClOrdID, testER_Replace.OrigClOrdID);
            Assert.AreEqual(originER_Replace.Symbol, testER_Replace.Symbol);
            Assert.AreEqual(originER_Replace.Side, testER_Replace.Side);
            Assert.AreEqual(originER_Replace.OrderQty, testER_Replace.OrderQty);
        }
        //Thông báo xác nhận lệnh
        [TestMethod]
        public void TestER_Order()
        {
            //Arrange
            MessageER_Order originER_Order = new MessageER_Order()
            {
                Price = 0,
                Price2 = 10000,
                Account = "003C313561",
                OrdType = "1",
                ClOrdID = "123456",
                OrderQty = 10000,
                SenderCompID = "HNX",
                TargetCompID = "003",
                TargetSubID = "003",
                OrderID = "1234678",
                OrdStatus = "1",
                SettDate = DateTime.Now,
                SettlValue = 316503661,
                Side = "1",
                Symbol = "XDCR12101"
            };
            //Act
            string msg_ER_Raw = c_MsgFactoryFIX.Build(originER_Order);
            MessageER_Order testER_Order = (MessageER_Order)c_MsgFactoryFIX.Parse(msg_ER_Raw);
            //Assert
            Assert.AreEqual(testER_Order.Side, originER_Order.Side);
            Assert.AreEqual(testER_Order.SettlValue, originER_Order.SettlValue);
            Assert.AreEqual(testER_Order.ClOrdID, originER_Order.ClOrdID);

        }

        [TestMethod]
        public void TestMsgExecutionReport()
        {
            //Arrange
            MessageExecutionReport origExReport = new MessageExecutionReport()
            {
                Account = "003C123126",
                ClOrdID = "1231412",
                ExecType = '9',
                SenderCompID = CommonData.FirmID,
                TargetCompID = CommonData.TargetCompID,
                TargetSubID = CommonData.TargetSubID,
                OrderID = "13210855",
                PartyID = "003",
                OrderQty = 9600,
                Price = 10000,
                Price2 = 10000,
                OrdType = "1",
                OrgOrderID = "13567133",
                SettDate = DateTime.Now,
                SettlValue = 10000,
                MsgSeqNum = 0,
                LastMsgSeqNumProcessed = 0,
                Symbol = "XDCR12101",
                Side = 1,
                Special_Type = 2,
                TransactTime = DateTime.Now,
                
            };
            //Act
            string msg_ER_Raw = c_MsgFactoryFIX.Build(origExReport);
            MessageExecutionReport testER = (MessageExecutionReport)c_MsgFactoryFIX.Parse(msg_ER_Raw);
            //Assert
            Assert.AreEqual(testER.Side, origExReport.Side);
            Assert.AreEqual(testER.SettlValue, origExReport.SettlValue);
            Assert.AreEqual(testER.ClOrdID, origExReport.ClOrdID);
            Assert.AreEqual(testER.PartyID, origExReport.PartyID);

        }

        [TestMethod]
        public void TestER_Reject()
        {
            MessageER_OrderReject origER_Reject = new MessageER_OrderReject()
            {
                Symbol = "XDCR12101",
                Special_Type = 1,
                TransactTime = DateTime.Now,
                ClOrdID = "6464100",
                OrderID = "135467",
                Account = "003C163546",
                OrderQty = 0,
                OrdType = "5",
                OrdRejReason = -0909,
                Price = 10000,
                Price2 = 10000,
                PartyID = "003C123164",
                SettDate = DateTime.Now,
                SettlValue = 0,
                OrgOrderID = "35464321",
                RejectReason = -1231414,
                OrdStatus = "4",
                Side = "1",
                _SecondaryClOrdID = "189041",
                UnderlyingLastQty = 123140

            };
            //Act
            string msg_ER_Raw = c_MsgFactoryFIX.Build(origER_Reject);
            MessageER_OrderReject testER = (MessageER_OrderReject)c_MsgFactoryFIX.Parse(msg_ER_Raw);
            //Assert
            Assert.AreEqual(testER.Side, origER_Reject.Side);
            Assert.AreEqual(testER.ClOrdID, origER_Reject.ClOrdID);
        }
    }
}
