using BusinessProcessResponse;
using CommonLib;
using HNX.FIXMessage;
using KafkaInterface;
using LocalMemory;
using Moq;
using static CommonLib.CommonData;
using static CommonLib.CommonDataInCore;

namespace HNXUnitTest
{
    [TestClass]
    public sealed class ResponseInterface_RevHNXTest
    {
        private MessageFactoryFIX messageFactoryFIX = new MessageFactoryFIX();
        private static bool IsAdd = false;

        [TestInitialize]
        public void InitEnviromentTest()
        {
            InitObject.ReadConfigTest();

            if (!IsAdd)
            {
                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "7830b4e9-3403-4fcf-a4d3-a84714d89307",
                    SeqNum = 1,
                    ClientID = "",
                });
                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "59960aad-9076-4307-bed6-459ee5cda3e2",
                    SeqNum = 2,
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "",
                    ExchangeID = "AAAR12101-2309140000001",
                    SeqNum = 3,
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "",
                    SeqNum = 4,
                    ExchangeID = "AAAR12101-2309140000002",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "",
                    ExchangeID = "AAAR12101-2308180000007",
                    SeqNum = 5,
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ExchangeID = "BABR12001-2307180000002",
                    SeqNum = 6,
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "",
                    ExchangeID = "BABR12001-2307180000004",
                    SeqNum = 7,
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "",
                    ExchangeID = "XDCR12101-2308230000008",
                    SeqNum = 8,
                    Side = "B",
                    ClientID = "",
                });
                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "",
                    Symbol = "XDCR12101",
                    ExchangeID = "XDCR12101-2308230000010",
                    SeqNum = 9,
                    Side = "S",
                    ClientID = "",
                });
                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "",
                    ExchangeID = "AAAR12101-2308100000003",
                    SeqNum = 10,
                    Side = "S",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    ClOrdID = "BACND-TEST",
                    SeqNum = 11,
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.NewOrderCross,
                    Price = 1000,
                    OrderQty = 1000000,
                    ClOrdID = "BACND",
                    ExchangeID = "XDCR12101-2312060000006",
                    Symbol = "SSI",
                    SeqNum = 12,
                    Side = "B",
                    ClientID = "",
                });
                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.NewOrderCross,
                    Price = 1000,
                    OrderQty = 1000000,
                    ClOrdID = "BACND",
                    ExchangeID = "XDCR12101-2312060000006",
                    Symbol = "SSI",
                    SeqNum = 13,
                    Side = "S",
                    ClientID = "",
                });
                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.NewOrderCross,
                    Price = 1000,
                    OrderQty = 1000000,
                    ClOrdID = "BACND",
                    ExchangeID = "XDCR12101-2312060000006",
                    Symbol = "SSI",
                    SeqNum = 14,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.CancelOrder,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND1",
                    ClOrdID = "c27ff50d-b73d-414a-8325-6cddde0f0106",
                    ExchangeID = "XDCR12101-2312060000006",
                    Symbol = "SSI",
                    SeqNum = 15,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.NewOrderCross,
                    Price = 1000,
                    OrderQty = 1000,
                    ClOrdID = "BACND",
                    ExchangeID = "",
                    Symbol = "SSI",
                    SeqNum = 16,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReplaceOrder,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND2",
                    ClOrdID = "48d5f383-b249-44f9-ba87-9c9e5f6316b7",
                    ExchangeID = "XDCR12101-2312060000006",
                    Symbol = "SSI",
                    SeqNum = 17,
                    Side = "S",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReplaceOrder,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND1",
                    ClOrdID = "48d5f383-b249-44f9-ba87-9c9e5f6316b7",
                    ExchangeID = "XDCR12101-2312060000007",
                    Symbol = "SSI",
                    SeqNum = 18,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDModify,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND199",
                    ClOrdID = "48d5f383-b249-44f9-ba87-9c9e5f6316b7",
                    ExchangeID = "",
                    RefExchangeID = "XDCR12101-2312060000007",
                    Symbol = "SSI",
                    SeqNum = 19,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDModify,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND200",
                    ClOrdID = "48d5f383-b249-44f9-ba87-9c9e5f6316b7",
                    ExchangeID = "XDCR12101-2312060000009",
                    RefExchangeID = "",
                    Symbol = "SSI",
                    SeqNum = 20,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDModify,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND201",
                    ClOrdID = "48d5f383-b249-44f9-ba87-9c9e5f6316b7",
                    ExchangeID = "",
                    RefExchangeID = "XDCR12101-2312060000010",
                    Symbol = "SSI",
                    SeqNum = 21,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDModify,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND202",
                    ClOrdID = "48d5f383-b249-44f9-ba87-9c9e5f6316b7",
                    ExchangeID = "XDCR12101-2312060000011",
                    RefExchangeID = "",
                    Symbol = "SSI",
                    SeqNum = 22,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDModify,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND203",
                    ClOrdID = "48d5f383-b249-44f9-ba87-9c9e5f6316b7",
                    ExchangeID = "",
                    RefExchangeID = "XDCR12101-2312060000011",
                    Symbol = "SSI",
                    SeqNum = 23,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDModify,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND204",
                    ClOrdID = "48d5f383-b249-44f9-ba87-9c9e5f6316b7",
                    ExchangeID = "XDCR12101-2312060000012",
                    RefExchangeID = "",
                    Symbol = "SSI",
                    SeqNum = 24,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDModify,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND205",
                    ClOrdID = "48d5f383-b249-44f9-ba87-9c9e5f6316b7",
                    ExchangeID = "",
                    RefExchangeID = "XDCR12101-2312060000012",
                    Symbol = "SSI",
                    SeqNum = 25,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDCancel,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND206",
                    ClOrdID = "48d5f383-b249-44f9-ba87-9c9e5f6316b7",
                    ExchangeID = "XDCR12101-2312060000013",
                    RefExchangeID = "",
                    Symbol = "SSI",
                    SeqNum = 26,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDCancel,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND207",
                    ClOrdID = "48d5f383-b249-44f9-ba87-9c9e5f6316b7",
                    ExchangeID = "",
                    RefExchangeID = "XDCR12101-2312060000013",
                    Symbol = "SSI",
                    SeqNum = 27,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDCancel,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND208",
                    ClOrdID = "48d5f383-b249-44f9-ba87-9c9e5f6316b7",
                    ExchangeID = "XDCR12101-2312060000014",
                    RefExchangeID = "",
                    Symbol = "SSI",
                    SeqNum = 28,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDCancel,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND209",
                    ClOrdID = "48d5f383-b249-44f9-ba87-9c9e5f6316b7",
                    ExchangeID = "",
                    RefExchangeID = "XDCR12101-2312060000014",
                    Symbol = "SSI",
                    SeqNum = 29,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDCancel,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND230",
                    ClOrdID = "48d5f383-b249-44f9-ba87-9c9e5f6316b7",
                    ExchangeID = "XDCR12101-2312060000015",
                    RefExchangeID = "",
                    Symbol = "SSI",
                    SeqNum = 30,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDCancel,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND231",
                    ClOrdID = "48d5f383-b249-44f9-ba87-9c9e5f6316b7",
                    ExchangeID = "",
                    RefExchangeID = "XDCR12101-2312060000015",
                    Symbol = "SSI",
                    SeqNum = 31,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDCancel,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND232",
                    ClOrdID = "48d5f383-b249-44f9-ba87-9c9e5f6316b7",
                    ExchangeID = "XDCR12101-2312060000016",
                    RefExchangeID = "",
                    Symbol = "SSI",
                    SeqNum = 32,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDCancel,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND233",
                    ClOrdID = "48d5f383-b249-44f9-ba87-9c9e5f6316b7",
                    ExchangeID = "",
                    RefExchangeID = "XDCR12101-2312060000016",
                    Symbol = "SSI",
                    SeqNum = 33,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDModify,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND234",
                    ClOrdID = "48d5f383-b249-44f9-ba87-9c9e5f6316b7",
                    ExchangeID = "XDCR12101-2312060000017",
                    RefExchangeID = "",
                    Symbol = "SSI",
                    SeqNum = 34,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDModify,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND235",
                    ClOrdID = "48d5f383-b249-44f9-ba87-9c9e5f6316b7",
                    ExchangeID = "",
                    RefExchangeID = "XDCR12101-2312060000017",
                    Symbol = "SSI",
                    SeqNum = 35,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDReport,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND236",
                    ClOrdID = "48d5f383-b249-8888-ba87-9c9e5f6316b8",
                    ExchangeID = "",
                    RefExchangeID = "XDCR12101-2312060000017",
                    Symbol = "SSI",
                    SeqNum = 36,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDReport,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND237",
                    ClOrdID = "48d5f383-b249-8889-ba87-9c9e5f6316b8",
                    ExchangeID = "",
                    RefExchangeID = "XDCR12101-2312060000018",
                    Symbol = "SSI",
                    SeqNum = 37,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDModify,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND238",
                    ClOrdID = "48d5f383-b249-8886-ba87-9c9e5f6316b8",
                    ExchangeID = "XDCR12101-2312060000019",
                    RefExchangeID = "",
                    Symbol = "SSI",
                    SeqNum = 38,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDModify,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND239",
                    ClOrdID = "48d5f383-b249-8886-ba87-9c9e5f6316b8",
                    ExchangeID = "",
                    RefExchangeID = "XDCR12101-2312060000019",
                    Symbol = "SSI",
                    SeqNum = 39,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDModify,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND238",
                    ClOrdID = "48d5f383-b249-8886-ba87-9c9e5f6316b8",
                    ExchangeID = "XDCR12101-2312060000020",
                    RefExchangeID = "",
                    Symbol = "SSI",
                    SeqNum = 40,
                    Side = "B",
                    ClientID = "",
                });

                OrderMemory.Add_NewOrder(new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDModify,
                    Price = 1000,
                    OrderQty = 1000000,
                    OrderNo = "BACND-ND239",
                    ClOrdID = "48d5f383-b249-8886-ba87-9c9e5f6316b8",
                    ExchangeID = "",
                    RefExchangeID = "XDCR12101-2312060000020",
                    Symbol = "SSI",
                    SeqNum = 41,
                    Side = "B",
                    ClientID = "",
                });

				OrderMemory.Add_NewOrder(new OrderInfo()
				{
					RefMsgType = MessageType.ReposBCGDModify,
					Price = 1000,
					OrderQty = 1000000,
					OrderNo = "",
					ClOrdID = "48d5f383-b249-8886-9999-9c9e5f6316b8",
					ExchangeID = "",
					RefExchangeID = "XDCR12101-2312060000020",
					Symbol = "SSI",
					SeqNum = 42,
					Side = "S",
					ClientID = "",
				});

				OrderMemory.Add_NewOrder(new OrderInfo()
				{
					RefMsgType = MessageType.ReposBCGDModify,
					Price = 1000,
					OrderQty = 1000000,
					OrderNo = "",
					ClOrdID = "48d5f383-b249-8886-9999-9c9e5f6316b8",
					ExchangeID = "",
					RefExchangeID = "XDCR12101-2312060000020",
					Symbol = "SSI",
					SeqNum = 43,
					Side = "B",
					ClientID = "",
				});

				// 35=N04
				List<SymbolFirmObject> listSymbolFirmMem = new List<SymbolFirmObject>();
                SymbolFirmObject _ReposSideListResponse = new SymbolFirmObject();
                _ReposSideListResponse.NumSide = 1;
                _ReposSideListResponse.Symbol = "XDCR12101";
                _ReposSideListResponse.OrderQty = 100;
                _ReposSideListResponse.HedgeRate = 1;
                _ReposSideListResponse.MergePrice = 1;
                //
                listSymbolFirmMem.Add(_ReposSideListResponse);
                //
                OrderInfo objOrder1 = new OrderInfo()
                {
                    RefMsgType = MessageType.ReposFirm,
                    OrderNo = "",
                    ClOrdID = "b0dee40c-dc8c-4db2-ac6c-cea83d2af56c",
                    ExchangeID = "",
                    RefExchangeID = "",
                    SeqNum = 0,  // khi nào sở về mới update
                    Symbol = "",
                    Side = ORDER_SIDE.SIDE_BUY,
                    Price = 0,
                    OrderQty = 0,
                    QuoteType = CORE_QuoteTypeRepos.QuoteType_1,
                    CrossType = "",
                    ClientID = "",
                    ClientIDCounterFirm = "",
                    MemberCounterFirm = "",
                    SettlMethod = 0,
                    SettleDate1 = "",
                    SettleDate2 = "",
                    EndDate = "",
                    RepurchaseTerm = 1,
                    RepurchaseRate = 1,
                    OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
                    NoSide = 1,
                    //
                    SymbolFirmInfo = listSymbolFirmMem
                };
                OrderMemory.Add_NewOrder(objOrder1);
                // N04
                OrderInfo objOrder2 = new OrderInfo()
                {
                    RefMsgType = MessageType.ReposFirm,
                    OrderNo = "",
                    ClOrdID = "",
                    ExchangeID = "REPOSPT-2312060000022",
                    RefExchangeID = "",
                    SeqNum = 0,  // khi nào sở về mới update
                    Symbol = "",
                    Side = ORDER_SIDE.SIDE_SELL,
                    Price = 0,
                    OrderQty = 0,
                    QuoteType = CORE_QuoteTypeRepos.QuoteType_2,
                    CrossType = "",
                    ClientID = "",
                    ClientIDCounterFirm = "",
                    MemberCounterFirm = "",
                    SettlMethod = 0,
                    SettleDate1 = "",
                    SettleDate2 = "",
                    EndDate = "",
                    RepurchaseTerm = 1,
                    RepurchaseRate = 1,
                    OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
                    NoSide = 1,
                    //
                    SymbolFirmInfo = listSymbolFirmMem
                };
                OrderMemory.Add_NewOrder(objOrder2);
            }
            IsAdd = true;
        }

        #region Thông báo nhận lệnh từ sở, gửi về cho kafka

        //Phản hồi nhận message AI
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=c813bae5-c87e-40d9-99c6-608c9dc78081\u00014488=018\u0001537=1\u00011=018C111111\u000155=BABR12001\u000154=1\u000140=S\u000138=10\u0001131=\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]//537 =1 Đặt khác cty
        [DataRow("8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=050.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=c813bae5-c87e-40d9-99c6-608c9dc78082\u00014488=050\u0001537=1\u00011=018C111111\u000155=BABR12001\u000154=1\u000140=S\u000138=10\u0001131=\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]//537 =1 Đặt khác cty
        [DataRow("8=FIX.4.4\u00019=257\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=27\u0001369=0\u000152=20230917-15:06:11\u000111=59960aad-9076-4307-bed6-459ee5cda3e2\u00014488=043\u0001537=2\u00011=048C111111\u000155=AAAR12101\u000154=1\u000140=S\u000138=5\u0001131=AAAR12101-2309140000001\u0001171=AAAR12101-2309140000002\u0001640=3000\u00016464=15000\u00016363=1\u000164=20230911\u0001513=48\u000110=242\u0001")] // Sửa 537=2 - bên đặt nhận
        [DataRow("8=FIX.4.4\u00019=257\u000135=AI\u000149=HNX\u000156=018.01GW\u000134=27\u0001369=0\u000152=20230917-15:06:11\u000111=59960aad-9076-4307-bed6-459ee5cda3e2\u00014488=018\u0001537=2\u00011=018C111111\u000155=AAAR12101\u000154=1\u000140=S\u000138=5\u0001131=AAAR12101-2309140000001\u0001171=AAAR12101-2309140000002\u0001640=3000\u00016464=15000\u00016363=1\u000164=20230911\u0001513=018\u000110=242\u0001")]//Sửa 537=2 - bên phản hồi nhận
        [DataRow("8=FIX.4.4\u00019=259\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=37\u0001369=0\u000152=20230812-03:23:53\u000111=46ffe3ea-8b35-4722-83f2-5f8e09673fee\u00014488=043\u0001537=3\u00011=43C111111\u000155=BABR12001\u000154=1\u000140=S\u000138=5\u0001131=BABR12001-2307180000004\u0001171=BABR12001-2307180000005\u0001640=300000\u00016464=1500000\u00016363=1\u000164=20230713\u0001513=0\u000110=100\u0001")]//Hủy 537=3
        [DataRow("8=FIX.4.4\u00019=256\u000135=AI\u000149=HNX\u000156=028.01GW\u000134=36\u0001369=0\u000152=20230829-16:12:08\u000111=8c578f2e-601d-4941-b5d1-72a990ee70c6\u00014488=028\u0001537=3\u00011=028C111111\u000155=AAAR12101\u000154=1\u000140=S\u000138=10\u0001131=AAAR12101-2308180000007\u0001171=AAAR12101-2308180000008\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230815\u0001513=0\u000110=112\u0001")] //537 = 3 Hủy lệnh chưa thực hiện - bên hủy nhận
        [DataRow("8=FIX.4.4\u00019=256\u000135=AI\u000149=HNX\u000156=43.01GW\u000134=36\u0001369=0\u000152=20230829-16:12:08\u000111=8c578f2e-601d-4941-b5d1-72a990ee70c6\u00014488=028\u0001537=3\u00011=028C111111\u000155=AAAR12101\u000154=1\u000140=S\u000138=10\u0001131=AAAR12101-2308180000007\u0001171=AAAR12101-2308180000008\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230815\u0001513=0\u000110=111\u0001")] //537 = 3 Hủy lệnh chưa thực hiện - bên đối ứng nhận
        [DataRow("8=FIX.4.4\u00019=234\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=47\u0001369=22\u000152=20230922-08:58:43\u000111=a9ba0706-2f51-4cd4-ad48-152e05c223bc\u00014488=019\u0001537=4\u00011=019C111111\u000155=XDCR12101\u000154=1\u000140=S\u000138=100\u0001131=\u0001171=XDCR12101-2309220000015\u0001640=100\u00016464=10000\u00016363=1\u000164=20230922\u0001513=0\u000110=075\u0001")] //537=4 Đặt lệnh  public và được thành viên khác chấp thuận
        [DataRow("8=FIX.4.4\u00019=259\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=27\u0001369=0\u000152=20230917-15:06:11\u000111=59960aad-9076-4307-bed6-459ee5cda3e2\u00014488=018\u0001537=5\u00011=018C111111\u000155=AAAR12101\u000154=1\u000140=S\u000138=5\u0001131=AAAR12101-2309140000001\u0001171=AAAR12101-2309140000002\u0001640=3000\u00016464=15000.0\u00016363=1\u000164=20230911\u0001513=018\u000110=086\u0001")]// 537 = 5
        public void TestResponseRecvFromHNX_MsgAI(string MessageRaw)
        {
            //Setup
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            // Arrange thông báo nhận phản hồi lệnh đặt
            MessageQuoteSatusReport messageQuote = (MessageQuoteSatusReport)messageFactoryFIX.Parse(MessageRaw);

            //Act
            _testKafkaInterface.HNXSendQuoteStatusReport((MessageQuoteSatusReport)messageQuote);

            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // 35=AI with p_Message.OrderPartyID == ConfigData.FirmID && p_Message.QuoteType == CORE_QuoteType.Bao_Co_Dien_Tu_Moi
        [TestMethod()]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=59960aad-9076-4307-bed6-459ee5cda3e2\u00014488=050\u0001537=1\u00011=018C111111\u000155=BABR12001\u000154=1\u000140=S\u000138=10\u0001131=\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=59960aad-9076-4307-bed6-459ee5cda3e2\u00014488=050\u0001537=1\u00011=018C111111\u000155=BABR12001\u000154=2\u000140=S\u000138=10\u0001131=\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=BACND\u00014488=050\u0001537=1\u00011=018C111111\u000155=BABR12001\u000154=1\u000140=S\u000138=10\u0001131=\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=BACND\u00014488=050\u0001537=1\u00011=018C111111\u000155=BABR12001\u000154=2\u000140=S\u000138=10\u0001131=\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        public void TestResponseRecvFromHNX_MsgAI_1_Test(string MessageRaw)
        {
            //Setup
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            // Arrange thông báo nhận phản hồi lệnh đặt
            MessageQuoteSatusReport messageQuote = (MessageQuoteSatusReport)messageFactoryFIX.Parse(MessageRaw);

            //Act
            _testKafkaInterface.HNXSendQuoteStatusReport((MessageQuoteSatusReport)messageQuote);

            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // 35=AI with p_Message.OrderPartyID(4488) == ConfigData.FirmID && p_Message.QuoteType(537) == CORE_QuoteType.Bao_Sua_Dien_Tu 2 /Có trong mem
        [TestMethod()]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=59960aad-9076-4307-bed6-459ee5cda3e2\u00014488=050\u0001537=2\u00011=018C111111\u000155=BABR12001\u000154=1\u000140=S\u000138=10\u0001131=AAAR12101-2309140000001\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=59960aad-9076-4307-bed6-459ee5cda3e2\u00014488=050\u0001537=2\u00011=018C111111\u000155=BABR12001\u000154=2\u000140=S\u000138=10\u0001131=AAAR12101-2309140000001\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=BACND\u00014488=050\u0001537=2\u00011=018C111111\u000155=BABR12001\u000154=1\u000140=S\u000138=10\u0001131=AAAR12101-2309140000001\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=BACND\u00014488=050\u0001537=2\u00011=018C111111\u000155=BABR12001\u000154=2\u000140=S\u000138=10\u0001131=AAAR12101-2309140000001\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        public void TestResponseRecvFromHNX_MsgAI_2_Test(string MessageRaw)
        {
            //Setup
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            // Arrange thông báo nhận phản hồi lệnh đặt
            MessageQuoteSatusReport messageQuote = (MessageQuoteSatusReport)messageFactoryFIX.Parse(MessageRaw);

            //Act
            _testKafkaInterface.HNXSendQuoteStatusReport((MessageQuoteSatusReport)messageQuote);

            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // 35=AI with p_Message.OrderPartyID(4488) == ConfigData.FirmID && p_Message.QuoteType(537) == CORE_QuoteType.Bao_Sua_Dien_Tu 2 / không Có trong mem
        [TestMethod()]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=59960aad-9076-4307-bed6-459ee5cda3e2\u00014488=050\u0001537=2\u00011=018C111111\u000155=BABR12001\u000154=1\u000140=S\u000138=10\u0001131=BACND-2309140000001\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=59960aad-9076-4307-bed6-459ee5cda3e2\u00014488=050\u0001537=2\u00011=018C111111\u000155=BABR12001\u000154=2\u000140=S\u000138=10\u0001131=BACND-2309140000001\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=BACND\u00014488=050\u0001537=2\u00011=018C111111\u000155=BABR12001\u000154=1\u000140=S\u000138=10\u0001131=BACND-2309140000001\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=BACND\u00014488=050\u0001537=2\u00011=018C111111\u000155=BABR12001\u000154=2\u000140=S\u000138=10\u0001131=BACND-2309140000001\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        public void TestResponseRecvFromHNX_MsgAI_2_2_Test(string MessageRaw)
        {
            //Setup
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            // Arrange thông báo nhận phản hồi lệnh đặt
            MessageQuoteSatusReport messageQuote = (MessageQuoteSatusReport)messageFactoryFIX.Parse(MessageRaw);

            //Act
            _testKafkaInterface.HNXSendQuoteStatusReport((MessageQuoteSatusReport)messageQuote);

            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // 35=AI with p_Message.OrderPartyID(4488) == ConfigData.FirmID && p_Message.QuoteType(537) == 3 / có trong memory
        [TestMethod()]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=59960aad-9076-4307-bed6-459ee5cda3e2\u00014488=050\u0001537=3\u00011=018C111111\u000155=BABR12001\u000154=1\u000140=S\u000138=10\u0001131=AAAR12101-2309140000001\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=59960aad-9076-4307-bed6-459ee5cda3e2\u00014488=050\u0001537=3\u00011=018C111111\u000155=BABR12001\u000154=2\u000140=S\u000138=10\u0001131=AAAR12101-2309140000001\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=BACND\u00014488=050\u0001537=3\u00011=018C111111\u000155=BABR12001\u000154=1\u000140=S\u000138=10\u0001131=AAAR12101-2309140000001\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=BACND\u00014488=050\u0001537=3\u00011=018C111111\u000155=BABR12001\u000154=2\u000140=S\u000138=10\u0001131=AAAR12101-2309140000001\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        public void TestResponseRecvFromHNX_MsgAI_3_Test(string MessageRaw)
        {
            //Setup
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            // Arrange thông báo nhận phản hồi lệnh đặt
            MessageQuoteSatusReport messageQuote = (MessageQuoteSatusReport)messageFactoryFIX.Parse(MessageRaw);

            //Act
            _testKafkaInterface.HNXSendQuoteStatusReport((MessageQuoteSatusReport)messageQuote);

            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // 35=AI with p_Message.OrderPartyID(4488) == ConfigData.FirmID && p_Message.QuoteType(537) == 3 / Không có trong memory
        [TestMethod()]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=59960aad-9076-4307-bed6-459ee5cda3e2\u00014488=050\u0001537=3\u00011=018C111111\u000155=BABR12001\u000154=1\u000140=S\u000138=10\u0001131=BACND-2309140000001\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=59960aad-9076-4307-bed6-459ee5cda3e2\u00014488=050\u0001537=3\u00011=018C111111\u000155=BABR12001\u000154=2\u000140=S\u000138=10\u0001131=BACND-2309140000001\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=BACND\u00014488=050\u0001537=3\u00011=018C111111\u000155=BABR12001\u000154=1\u000140=S\u000138=10\u0001131=BACND-2309140000001\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        [DataRow($"8=FIX.4.4\u00019=232\u000135=AI\u000149=HNX\u000156=043.01GW\u000134=1\u0001369=0\u000152=20230811-09:47:50\u000111=BACND\u00014488=050\u0001537=3\u00011=018C111111\u000155=BABR12001\u000154=2\u000140=S\u000138=10\u0001131=BACND-2309140000001\u0001171=BABR12001-2307170000001\u0001640=3000\u00016464=30000\u00016363=1\u000164=20230712\u0001513=0\u000110=154\u0001")]
        public void TestResponseRecvFromHNX_MsgAI_4_Test(string MessageRaw)
        {
            //Setup
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            // Arrange thông báo nhận phản hồi lệnh đặt
            MessageQuoteSatusReport messageQuote = (MessageQuoteSatusReport)messageFactoryFIX.Parse(MessageRaw);

            //Act
            _testKafkaInterface.HNXSendQuoteStatusReport((MessageQuoteSatusReport)messageQuote);

            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=110\u000135=h\u000149=HNX\u000156=003.01GW\u000134=23\u0001369=2\u000152=20230908-02:02:42\u0001336=NONE\u0001340=90\u0001341=20230830-01:00:00\u0001339=1\u0001335=DEMO\u000110=220\u0001")]//Chưa vào phiên
        [DataRow("8=FIX.4.4\u00019=109\u000135=h\u000149=HNX\u000156=003.01GW\u000134=46\u0001369=3\u000152=20230908-02:15:14\u0001336=DEMO\u0001340=1\u0001341=20230908-02:15:14\u0001339=1\u0001335=DEMO\u000110=188\u0001")]//bình thường
        [DataRow("8=FIX.4.4\u00019=109\u000135=h\u000149=HNX\u000156=003.01GW\u000134=49\u0001369=3\u000152=20230908-02:15:44\u0001336=DEMO\u0001340=2\u0001341=20230908-02:15:44\u0001339=1\u0001335=DEMO\u000110=198\u0001")]//tạm dừng
        [DataRow("8=FIX.4.4\u00019=110\u000135=h\u000149=HNX\u000156=003.01GW\u000134=55\u0001369=3\u000152=20230908-02:15:57\u0001336=DEMO\u0001340=13\u0001341=20230908-02:15:57\u0001339=1\u0001335=DEMO\u000110=245\u0001")]//kết thúc nhận lệnh
        [DataRow("8=FIX.4.4\u00019=111\u000135=h\u000149=HNX\u000156=003.01GW\u000134=118\u0001369=3\u000152=20230908-02:17:05\u0001336=NONE\u0001340=97\u0001341=20230908-02:17:05\u0001339=1\u0001335=DEMO\u000110=051\u0001")]//đóng cửa thị trường
        public void TestResponseHNXSendTradingSession(string msgRaw)
        {
            //Setup
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            // Arrange thông báo nhận phản hồi lệnh đặt
            MessageTradingSessionStatus messageTradingSession = (MessageTradingSessionStatus)messageFactoryFIX.Parse(msgRaw);

            //Act
            _testKafkaInterface.ResponseHNXSendTradingSessionStatus(messageTradingSession);

            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_TradingInfo, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=268\u000135=f\u000149=HNX\u000156=043.01GW\u000134=3\u0001369=0\u000152=20230925-01:56:04\u000143=Y\u0001324=\u000155=VPBR11901\u0001167=\u0001541=20261219\u0001225=20191219\u0001106=TCPHCBIS\u000131=1000000000\u0001332=0\u0001333=0\u00013321=0\u00013331=0\u00013322=1100000000\u00013332=0\u0001326=0\u0001330=0\u0001625=CBTS\u00016251=0\u0001265=103\u0001109=700\u00019735=\u00019736=\u00019735=1,2,3,4,5,6\u00019736=1,2\u000110=028\u0001")]//Mã ck thuộc bảng CBTS
        [DataRow("8=FIX.4.4\u00019=247\u000135=f\u000149=HNX\u000156=043.01GW\u000134=24\u0001369=1\u000152=20230925-01:56:05\u0001324=085443\u000155=XDCR12102\u0001167=\u0001541=20250409\u0001225=20210409\u0001106=TCPHCBIS\u000131=100000\u0001332=0\u0001333=0\u00013321=0\u00013331=0\u00013322=110000\u00013332=0\u0001326=0\u0001330=0\u0001625=DEMO\u00016251=0\u0001265=103\u0001109=11000000\u00019735=1,2\u00019736=1,2\u000110=038\u0001")]//Mã ck thuộc bảng DEMO
        public void TestResponseHNXSendSecurity(string msgRaw)
        {
            //Setup
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            // Arrange thông báo nhận phản hồi lệnh đặt
            MessageSecurityStatus messageSecurityStatus = (MessageSecurityStatus)messageFactoryFIX.Parse(msgRaw);

            //Act
            _testKafkaInterface.ResponseHNXSendSecurityStatus(messageSecurityStatus);

            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_TradingInfo, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=84\u000135=MN\u000149=HNX\u000156=043.01GW\u000134=42\u0001369=0\u000152=20231221-10:39:19\u000155=BABR12001\u00014499=043,018\u000110=091\u0001")]
        public void TestTopicTradingInfomation(string msgRaw)
        {
            //Setup
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            // Arrange
            MessageTopicTradingInfomation messageTopicTradingInfomation = (MessageTopicTradingInfomation)messageFactoryFIX.Parse(msgRaw);

            //Act
            _testKafkaInterface.ResponseHNXTopicTradingInfomation(messageTopicTradingInfomation);

            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_TradingInfo, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }



        //Nhận message 8 phản hồi lệnh sửa
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=209\u000135=8\u000149=HNX\u000156=018.01GW\u000134=31\u0001369=0\u000152=20230812-02:59:41\u0001150=5\u000139=A\u000111=MODBCGDTH633\u000141=BABR12001-2307180000001\u000137=BABR12001-2307180000002\u00011=018C111111\u000155=BABR12001\u000154=1\u000140=R\u000132=10\u000131=300000\u0001151=0\u00016464=3000000\u000110=230\u0001")]//Nhận phản hồi chờ kiểm soát - bên đối tác nhận
        [DataRow("8=FIX.4.4\u00019=209\u000135=8\u000149=HNX\u000156=028.01GW\u000134=30\u0001369=0\u000152=20230812-02:59:41\u0001150=5\u000139=A\u000111=MODBCGDTH633\u000141=BABR12001-2307180000001\u000137=BABR12001-2307180000002\u00011=028C111111\u000155=BABR12001\u000154=2\u000140=R\u000132=10\u000131=300000\u0001151=0\u00016464=3000000\u000110=232\u0001")] //Nhận phản hồi chờ kiếm soát - bên đặt lệnh nhận
        [DataRow("8=FIX.4.4\u00019=233\u000135=8\u000149=HNX\u000156=018.01GW\u000134=32\u0001369=0\u000152=20230812-02:59:54\u0001150=5\u000139=3\u000111=eac6a915-79db-49a7-8a4b-4fe3c701aae6\u000141=BABR12001-2307180000001\u000137=BABR12001-2307180000002\u00011=018C111111\u000155=BABR12001\u000154=1\u000140=R\u000132=10\u000131=300000\u0001151=0\u00016464=3000000\u000110=185\u0001")] //Nhận phản hồi kiểm soát - bên đối tác nhận
        [DataRow("8=FIX.4.4\u00019=233\u000135=8\u000149=HNX\u000156=028.01GW\u000134=31\u0001369=0\u000152=20230812-02:59:54\u0001150=5\u000139=4\u000111=eac6a915-79db-49a7-8a4b-4fe3c701aae6\u000141=BABR12001-2307180000001\u000137=BABR12001-2307180000002\u00011=028C111111\u000155=BABR12001\u000154=2\u000140=R\u000132=10\u000131=300000\u0001151=0\u00016464=3000000\u000110=187\u0001")] // 39= 4
        [DataRow("8=FIX.4.4\u00019=233\u000135=8\u000149=HNX\u000156=028.01GW\u000134=31\u0001369=0\u000152=20230812-02:59:54\u0001150=5\u000139=9\u000111=eac6a915-79db-49a7-8a4b-4fe3c701aae6\u000141=BABR12001-2307180000001\u000137=BABR12001-2307180000002\u00011=028C111111\u000155=BABR12001\u000154=2\u000140=R\u000132=10\u000131=300000\u0001151=0\u00016464=3000000\u000110=187\u0001")] // 39= 9
        public void TestProcessRecvFromHNX_ER_Replace(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            //Act
            _testKafkaInterface.HNXResponse_ExecReplace((MessageER_OrderReplace)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=225\u000135=8\u000149=HNX\u000156=043.01GW\u000134=106\u0001369=18\u000152=20231212-08:29:04\u0001150=5\u000139=3\u000111=48d5f383-b249-44f9-ba87-9c9e5f6316b7\u000141=AAAR12101-2401150000003\u000137=AAAR12101-2401150000004\u00011=043CX00002\u000155=AAAR12101\u000154=1\u000140=2\u000132=800\u000131=90000\u0001151=-200\u000110=116\u0001")]
        public void TestProcessRecvFromHNX_ER_ReplaceNormal(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            //Act
            _testKafkaInterface.HNXResponse_ExecReplace((MessageER_OrderReplace)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // 35=8 | 150=0
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=173\u000135=8\u000149=HNX\u000156=043.01GW\u000134=100\u0001369=23\u000152=20231115-03:30:46\u0001150=0\u000139=A\u000111=TEST_11\u000137=AAAR12101-2312210000006\u00011=043C111111\u000155=AAAR12101\u000154=1\u000138=100\u000140=LO\u000144=10000\u00016464=1000000\u000110=136\u0001")]
        [DataRow("8=FIX.4.4\u00019=173\u000135=8\u000149=HNX\u000156=043.01GW\u000134=100\u0001369=23\u000152=20231115-03:30:46\u0001150=0\u000139=A\u000111=TEST_11\u000137=AAAR12101-2312210000006\u00011=043C111111\u000155=AAAR12101\u000154=2\u000138=100\u000140=LO\u000144=10000\u00016464=1000000\u000110=136\u0001")]
        [DataRow("8=FIX.4.4\u00019=173\u000135=8\u000149=HNX\u000156=043.01GW\u000134=100\u0001369=23\u000152=20231115-03:30:46\u0001150=0\u000139=M\u000111=TEST_11\u000137=AAAR12101-2312210000006\u00011=043C111111\u000155=AAAR12101\u000154=1\u000138=100\u000140=LO\u000144=10000\u00016464=1000000\u000110=136\u0001")]
        [DataRow("8=FIX.4.4\u00019=173\u000135=8\u000149=HNX\u000156=043.01GW\u000134=100\u0001369=23\u000152=20231115-03:30:46\u0001150=0\u000139=M\u000111=TEST_11\u000137=AAAR12101-2312210000006\u00011=043C111111\u000155=AAAR12101\u000154=2\u000138=100\u000140=LO\u000144=10000\u00016464=1000000\u000110=136\u0001")]
        public void TestProcessRecvFromHNX_ExecOrder(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            //Act
            _testKafkaInterface.HNXResponse_ExecOrder((MessageER_Order)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // 35=8 | 150=8
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=127\u000135=8\u000149=HNX\u000156=043.01GW\u000134=128\u0001369=22\u000152=20231214-06:58:53\u0001150=8\u000139=8\u000111=TEST_20\u000137=AAAR12101-2401170000001\u0001103=1\u0001652=500\u000154=1\u000110=228\u0001")]
        [DataRow("8=FIX.4.4\u00019=127\u000135=8\u000149=HNX\u000156=043.01GW\u000134=128\u0001369=22\u000152=20231214-06:58:53\u0001150=8\u000139=8\u000111=TEST_20\u000137=AAAR12101-2401170000001\u0001103=2\u0001652=500\u000154=1\u000110=228\u0001")]
        [DataRow("8=FIX.4.4\u00019=127\u000135=8\u000149=HNX\u000156=043.01GW\u000134=128\u0001369=22\u000152=20231214-06:58:53\u0001150=8\u000139=8\u000111=TEST_20\u000137=AAAR12101-2401170000001\u0001103=3\u0001652=500\u000154=1\u000110=228\u0001")]
        [DataRow("8=FIX.4.4\u00019=127\u000135=8\u000149=HNX\u000156=043.01GW\u000134=128\u0001369=22\u000152=20231214-06:58:53\u0001150=8\u000139=8\u000111=TEST_20\u000137=AAAR12101-2401170000001\u0001103=4\u0001652=500\u000154=1\u000110=228\u0001")]
        [DataRow("8=FIX.4.4\u00019=127\u000135=8\u000149=HNX\u000156=043.01GW\u000134=128\u0001369=22\u000152=20231214-06:58:53\u0001150=8\u000139=8\u000111=TEST_20\u000137=AAAR12101-2401170000001\u0001103=5\u0001652=500\u000154=1\u000110=228\u0001")]
        [DataRow("8=FIX.4.4\u00019=127\u000135=8\u000149=HNX\u000156=043.01GW\u000134=128\u0001369=22\u000152=20231214-06:58:53\u0001150=8\u000139=8\u000111=TEST_20\u000137=AAAR12101-2401170000001\u0001103=9\u0001652=500\u000154=1\u000110=228\u0001")]
        public void TestProcessRecvFromHNX_ExecOrderReject(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);

            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            //Act
            _testKafkaInterface.HNXResponse_ExecOrderReject((MessageER_OrderReject)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        //Nhận message 8 phản hòi lệnh hủy
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=214\u000135=8\u000149=HNX\u000156=018.01GW\u000134=40\u0001369=0\u000152=20230812-03:47:24\u0001150=4\u000139=A\u000111=06df6388-f9cc-47c6-841e-f4e3dacc6a72\u000141=BABR12001-2307180000006\u000137=BABR12001-2307180000007\u0001151=8\u00011=018CX00001\u000155=BABR12001\u000154=1\u000140=R\u000144=300000\u000110=109\u0001")] // 39 = A
        [DataRow("8=FIX.4.4\u00019=208\u000135=8\u000149=HNX\u000156=018.01GW\u000134=41\u0001369=0\u000152=20230812-03:47:42\u0001150=4\u000139=3\u000111=464f9eaf-014a-4df4-b22e-4884aeefa019\u000141=BABR12001-2307180000006\u000137=BABR12001-2307180000007\u00011=018CX00001\u000155=BABR12001\u000154=1\u000140=R\u000144=300000\u000110=069\u0001")] // 39 = 3
        [DataRow("8=FIX.4.4\u00019=209\u000135=8\u000149=HNX\u000156=028.01GW\u000134=28\u0001369=0\u000152=20230814-03:41:25\u0001150=4\u000139=3\u000111=19815701-1f78-48da-8397-324604dfe75f\u000141=AAAR12101-2307240000001\u000137=AAAR12101-2307240000002\u00011=028C111111\u000155=AAAR12101\u000154=2\u000140=R\u000144=3000000\u000110=011\u0001")] // 39 = 3
        [DataRow("8=FIX.4.4\u00019=209\u000135=8\u000149=HNX\u000156=018.01GW\u000134=28\u0001369=0\u000152=20230814-03:41:25\u0001150=4\u000139=3\u000111=19815701-1f78-48da-8397-324604dfe75f\u000141=AAAR12101-2307240000001\u000137=AAAR12101-2307240000002\u00011=018C111111\u000155=AAAR12101\u000154=1\u000140=R\u000144=3000000\u000110=008\u0001")] // 39 = 3
        [DataRow("8=FIX.4.4\u00019=197\u000135=8\u000149=HNX\u000156=043.01GW\u000134=124\u0001369=9\u000152=20230920-03:30:12\u0001150=4\u000139=10\u000111=638308025384594355\u000141=XDCR12101-2309200000003\u000137=XDCR12101-2309200000005\u0001151=100\u00011=043C111111\u000155=XDCR12101\u000154=2\u000140=R\u000144=100\u000110=119\u0001")] // 39 = 10
        [DataRow("8=FIX.4.4\u00019=196\u000135=8\u000149=HNX\u000156=050.02GW\u000134=95\u0001369=3\u000152=20230920-03:30:12\u0001150=4\u000139=5\u000111=638308025384594355\u000141=XDCR12101-2309200000003\u000137=XDCR12101-2309200000005\u0001151=100\u00011=050C111111\u000155=XDCR12101\u000154=1\u000140=R\u000144=100\u000110=067\u0001")] // 39 = 5
        [DataRow("8=FIX.4.4\u00019=196\u000135=8\u000149=HNX\u000156=050.02GW\u000134=95\u0001369=3\u000152=20230920-03:30:12\u0001150=4\u000139=9\u000111=638308025384594355\u000141=XDCR12101-2309200000003\u000137=XDCR12101-2309200000005\u0001151=100\u00011=050C111111\u000155=XDCR12101\u000154=1\u000140=R\u000144=100\u000110=067\u0001")] // 39 = 9
        [DataRow("8=FIX.4.4\u00019=196\u000135=8\u000149=HNX\u000156=050.02GW\u000134=95\u0001369=3\u000152=20230920-03:30:12\u0001150=4\u000139=11\u000111=638308025384594355\u000141=XDCR12101-2309200000003\u000137=XDCR12101-2309200000005\u0001151=100\u00011=050C111111\u000155=XDCR12101\u000154=1\u000140=R\u000144=100\u000110=067\u0001")] // 39 = 11
        public void TestResponseRecvFromHNX_ER_OrderCancel(string Message)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(Message);
            _testKafkaInterface.HNXResponse_EROrderCancel((MessageER_OrderCancel)messageBase);
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // Xu ly cho lenh thong thuong
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=216\u000135=8\u000149=HNX\u000156=043.01GW\u000134=109\u0001369=21\u000152=20231212-08:32:25\u0001150=4\u000139=3\u000111=c27ff50d-b73d-414a-8325-6cddde0f0106\u000141=AAAR12101-2401150000005\u000137=AAAR12101-2401150000006\u0001151=500\u00011=043C111111\u000155=AAAR12101\u000154=1\u000140=2\u000144=9000\u000110=031\u0001")]
        [DataRow("8=FIX.4.4\u00019=216\u000135=8\u000149=HNX\u000156=043.01GW\u000134=109\u0001369=21\u000152=20231212-08:32:25\u0001150=4\u000139=3\u000111=c27ff50d-b73d-414a-8325-6cddde0f0106\u000141=AAAR12101-2401150000005\u000137=AAAR12101-2401150000006\u0001151=500\u00011=043C111111\u000155=AAAR12101\u000154=2\u000140=2\u000144=9000\u000110=031\u0001")]
        public void TestResponseRecvFromHNX_ER_OrderCancel_Normal(string Message)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(Message);
            _testKafkaInterface.HNXResponse_EROrderCancel((MessageER_OrderCancel)messageBase);
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=258\u000135=s\u000149=HNX\u000156=003.01GW\u000134=38\u0001369=8\u000152=20230831-08:45:24\u000111=59960aad-9076-4307-bed6-459ee5cda3e2\u00011=003C111111\u00012=043CX00002\u000155=XDCR12101\u000154=1\u000138=1000\u0001448=003\u0001449=043\u0001548=XDCR12101-2308230000009\u0001549=1\u000140=R\u0001640=1000\u00016464=1000000\u000164=20230823\u0001168=20230823\u00016363=1\u000110=032\u0001")]
        [DataRow("8=FIX.4.4\u00019=258\u000135=s\u000149=HNX\u000156=003.01GW\u000134=38\u0001369=8\u000152=20230831-08:45:24\u000111=59960aad-9076-4307-bed6-459ee5cda3e2\u00011=003C111111\u00012=043CX00002\u000155=XDCR12101\u000154=2\u000138=1000\u0001448=003\u0001449=043\u0001548=XDCR12101-2308230000009\u0001549=1\u000140=R\u0001640=1000\u00016464=1000000\u000164=20230823\u0001168=20230823\u00016363=1\u000110=032\u0001")]
        [DataRow("8=FIX.4.4\u00019=258\u000135=s\u000149=HNX\u000156=003.01GW\u000134=38\u0001369=8\u000152=20230831-08:45:24\u000111=59960aad-9076-4307-bed6-459ee5cda3e2\u00011=003C111111\u00012=043CX00002\u000155=XDCR12101\u000154=2\u000138=1000\u0001448=003\u0001449=003\u0001548=XDCR12101-2308230000009\u0001549=1\u000140=R\u0001640=1000\u00016464=1000000\u000164=20230823\u0001168=20230823\u00016363=1\u000110=032\u0001")]
        [DataRow("8=FIX.4.4\u00019=258\u000135=s\u000149=HNX\u000156=003.01GW\u000134=38\u0001369=8\u000152=20230831-08:45:24\u000111=NO-CLORDERID-TEST\u00011=003C111111\u00012=043CX00002\u000155=XDCR12101\u000154=1\u000138=1000\u0001448=003\u0001449=043\u0001548=XDCR12101-2308230000009\u0001549=1\u000140=R\u0001640=1000\u00016464=1000000\u000164=20230823\u0001168=20230823\u00016363=1\u000110=032\u0001")]
        [DataRow("8=FIX.4.4\u00019=258\u000135=s\u000149=HNX\u000156=003.01GW\u000134=38\u0001369=8\u000152=20230831-08:45:24\u000111=NO-CLORDERID-TEST\u00011=003C111111\u00012=043CX00002\u000155=XDCR12101\u000154=2\u000138=1000\u0001448=003\u0001449=043\u0001548=XDCR12101-2308230000009\u0001549=1\u000140=R\u0001640=1000\u00016464=1000000\u000164=20230823\u0001168=20230823\u00016363=1\u000110=032\u0001")]
        public void TestResponseRecvFromHNX_Msg_s(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXSendOrderCross((MessageNewOrderCross)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        //Nhận yêu cầu sửa TTDT OutRight
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=280\u000135=t\u000149=HNX\u000156=018.01GW\u000134=30\u0001369=0\u000152=20230812-02:58:56\u000111=df1a4e55-3b8e-4582-a75b-1aaa62c21254\u00011=028C111111\u00012=018C111111\u000155=BABR12001\u000154=1\u000138=10\u0001448=028\u0001449=018\u000137=BABR12001-2307180000002\u0001551=BABR12001-2307180000001\u0001549=2\u0001640=300000\u00016464=3000000\u000164=20230713\u0001168=20230713\u00016363=2\u000110=070\u0001")]//Nhận thông tin sửa lệnh - bên đối tác nhận
        [DataRow("8=FIX.4.4\u00019=280\u000135=t\u000149=HNX\u000156=028.01GW\u000134=29\u0001369=0\u000152=20230812-02:58:57\u000111=df1a4e55-3b8e-4582-a75b-1aaa62c21254\u00011=028C111111\u00012=018C111111\u000155=BABR12001\u000154=2\u000138=10\u0001448=028\u0001449=018\u000137=BABR12001-2307180000002\u0001551=BABR12001-2307180000001\u0001549=2\u0001640=300000\u00016464=3000000\u000164=20230713\u0001168=20230713\u00016363=2\u000110=081\u0001")] //Nhận thông tin sửa lệnh - bên đặt lệnh nhận
        [DataRow("8=FIX.4.4\u00019=279\u000135=t\u000149=HNX\u000156=018.01GW\u000134=100\u0001369=0\u000152=20230825-07:44:53\u000111=f23a28b7-edc0-4230-9954-daff66c4d64b\u00011=028C111111\u00012=018C111111\u000155=AAAR12101\u000154=1\u000138=20\u0001448=028\u0001449=018\u000137=AAAR12101-2308100000004\u0001551=AAAR12101-2308100000003\u0001549=1\u0001640=46000\u00016464=920000\u000164=20230807\u0001168=20230807\u00016363=1\u000110=105\u0001")]// Nhận sửa đã thực hiện khác cty, bên đặt nhận
        [DataRow("8=FIX.4.4\u00019=279\u000135=t\u000149=HNX\u000156=028.01GW\u000134=100\u0001369=0\u000152=20230825-07:44:53\u000111=f23a28b7-edc0-4230-9954-daff66c4d64b\u00011=028C111111\u00012=018C111111\u000155=AAAR12101\u000154=2\u000138=20\u0001448=028\u0001449=018\u000137=AAAR12101-2308100000004\u0001551=AAAR12101-2308100000004\u0001549=1\u0001640=46000\u00016464=920000\u000164=20230807\u0001168=20230807\u00016363=1\u000110=107\u0001")]//Nhận thông tin sửa đã thực hiện - khác cty - bên đối ứng nhận
        public void TestResponseRecvFromHNX_Msg_t(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_CrossOrderCancelReplace((CrossOrderCancelReplaceRequest)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        //Nhận yêu cầu hủy TTDT OutRight
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=181\u000135=u\u000149=HNX\u000156=018.01GW\u000134=26\u0001369=0\u000152=20230814-03:40:47\u000111=ba56eb63-2180-4324-939c-4431a0b643db\u000137=AAAR12101-2307240000002\u0001551=AAAR12101-2307240000001\u0001549=2\u000155=AAAR12101\u000154=1\u000140=R\u000110=120\u0001")] // Nhận yêu cầu lệnh hủy - bên đối tác nhận
        [DataRow("8=FIX.4.4\u00019=181\u000135=u\u000149=HNX\u000156=028.01GW\u000134=26\u0001369=0\u000152=20230814-03:40:47\u000111=ba56eb63-2180-4324-939c-4431a0b643db\u000137=AAAR12101-2307240000002\u0001551=AAAR12101-2307240000001\u0001549=2\u000155=AAAR12101\u000154=2\u000140=R\u000110=122\u0001")] // Nhận yêu cầu lệnh hủy - bên đặt lệnh nhận
        public void TestResponseRecvFromHNX_Msg_u(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_CrossOrderCancelRequest((CrossOrderCancelRequest)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=TEST_8\u000155=XDCR12101\u000154=1\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=5\u0001644=123\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=003\u000110=156\u0001")]
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=TEST_8\u000155=XDCR12101\u000154=1\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=6\u0001644=123\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=003\u000110=156\u0001")]
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=TEST_8\u000155=XDCR12101\u000154=1\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=6\u0001644=123\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=050\u000110=156\u0001")]
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=TEST_8\u000155=XDCR12101\u000154=1\u000140=U\u0001117=BABR12001-2307180000004\u000138=100000\u0001448=\u0001537=5\u0001644=123\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=003\u000110=156\u0001")]
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=TEST_8\u000155=XDCR12101\u000154=1\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=6\u0001644=123\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=003\u000110=156\u0001")]
        public void TestResponseRecvFromHNX_Msg35_N02(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_InquiryReposReponse((MessageReposInquiryReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod] // 537 = 1 & 4488 == ConfigData.FirmID
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=59960aad-9076-4307-bed6-459ee5cda3e2\u000155=XDCR12101\u000154=1\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=1\u0001644=\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=050\u000110=156\u0001")]
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=59960aad-9076-4307-bed6-459ee5cda3e2\u000155=XDCR12101\u000154=2\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=1\u0001644=123\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=050\u000110=156\u0001")]
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=BACND\u000155=XDCR12101\u000154=2\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=1\u0001644=123\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=050\u000110=156\u0001")]
        public void TestResponseRecvFromHNX_Msg35_N02_1_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_InquiryReposReponse((MessageReposInquiryReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod] // 537 = 1 & 4488 != ConfigData.FirmID
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=TEST_8\u000155=XDCR12101\u000154=1\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=1\u0001644=123\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=003\u000110=156\u0001")]
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=TEST_8\u000155=XDCR12101\u000154=2\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=1\u0001644=123\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=003\u000110=156\u0001")]
        public void TestResponseRecvFromHNX_Msg35_N02_1_1_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_InquiryReposReponse((MessageReposInquiryReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod] // 537 = 2 & 4488 == ConfigData.FirmID
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=TEST_8\u000155=XDCR12101\u000154=1\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=2\u0001644=XDCR12101-2308230000008\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=050\u000110=156\u0001")]
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=TEST_8\u000155=XDCR12101\u000154=2\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=2\u0001644=XDCR12101-2308230000008\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=050\u000110=156\u0001")]
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=TEST_8\u000155=XDCR12101\u000154=1\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=2\u0001644=BACND-2308230000008\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=050\u000110=156\u0001")]
        public void TestResponseRecvFromHNX_Msg35_N02_2_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_InquiryReposReponse((MessageReposInquiryReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod] // 537 = 2 & 4488 != ConfigData.FirmID
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=TEST_8\u000155=XDCR12101\u000154=1\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=2\u0001644=XDCR12101-2308230000008\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=003\u000110=156\u0001")]
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=TEST_8\u000155=XDCR12101\u000154=2\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=2\u0001644=XDCR12101-2308230000008\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=003\u000110=156\u0001")]
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=TEST_8\u000155=XDCR12101\u000154=1\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=2\u0001644=BACND-2308230000008\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=003\u000110=156\u0001")]
        public void TestResponseRecvFromHNX_Msg35_N02_3_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_InquiryReposReponse((MessageReposInquiryReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod] // 537 = 3 & 4488 == ConfigData.FirmID
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=TEST_8\u000155=XDCR12101\u000154=1\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=3\u0001644=XDCR12101-2308230000008\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=050\u000110=156\u0001")]
        public void TestResponseRecvFromHNX_Msg35_N02_4_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_InquiryReposReponse((MessageReposInquiryReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod] // 537 = 3 & 4488 != ConfigData.FirmID
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=TEST_8\u000155=XDCR12101\u000154=1\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=3\u0001644=XDCR12101-2308230000008\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=003\u000110=156\u0001")]
        public void TestResponseRecvFromHNX_Msg35_N02_5_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_InquiryReposReponse((MessageReposInquiryReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=TEST_8\u000155=XDCR12101\u000154=1\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=4\u0001644=123\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=003\u000110=156\u0001")]
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=TEST_8\u000155=XDCR12101\u000154=1\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=4\u0001644=123\u0001226=1\u0001513=019\u000158=\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=050\u000110=156\u0001")]
        [DataRow("8=FIX.4.4\u00019=239\u000135=N02\u000149=HNX\u000156=050.02GW\u000134=57\u0001369=10\u000152=20231025-03:24:48\u000111=TEST_8\u000155=XDCR12101\u000154=1\u000140=U\u0001117=REPOSPT-2311160000004\u000138=100000\u0001448=\u0001537=4\u0001644=123\u0001226=1\u0001513=019\u000158=AA\u0001168=20231116\u000164=20231116\u0001193=20231117\u0001917=20231117\u00016363=2\u00016364=0\u00014488=003\u000110=156\u0001")]
        public void TestResponseRecvFromHNX_Msg35_N02_6_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_InquiryReposReponse((MessageReposInquiryReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=224\u000135=8\u000149=HNX\u000156=043.01GW\u000134=37\u0001369=8\u000152=20230831-08:42:49\u0001150=3\u000139=2\u000111=17\u000141=XDCR12101-2308230000008\u000140=U\u0001526=AAAR12101-2309140000001\u000137=XDCR12101-2308230000001\u000132=5\u000131=1000\u000117=XDCR12101-2308230000001\u000155=XDCR12101\u000154=1\u0001448=TEST\u00016464=5000\u000110=033\u0001")]//HNX gửi khớp cùng cty F
        [DataRow("8=FIX.4.4\u00019=222\u000135=8\u000149=HNX\u000156=043.01GW\u000134=39\u0001369=9\u000152=20230831-08:51:00\u0001150=3\u000139=2\u000111=23\u000141=XDCR12101-2308230000010\u000140=U\u0001526=XDCR12101-2308230000010\u000137=XDCR12101-2308230000002\u000132=10\u000131=10\u000117=XDCR12101-2308230000002\u000155=XDCR12101\u000154=2\u00016464=100\u000110=158\u0001")]//HNX gửi khớp khac
        [DataRow("8=FIX.4.4\u00019=236\u000135=8\u000149=HNX\u000140=U\u000156=043.01GW\u000134=126\u0001369=56\u000152=20231013-06:28:57\u0001150=3\u000139=2\u000111=ThamNTH_02\u000141=XDCR12101-2311020000022\u0001526=XDCR12101-2311020000022\u000137=XDCR12101-2311020000007\u000132=100\u000131=100\u000117=XDCR12101-2311020000007\u000155=XDCR12101\u000154=1\u00016464=10000\u000110=137\u0001")] // F
        [DataRow("8=FIX.4.4\u00019=224\u000135=8\u000149=HNX\u000140=U\u000156=043.01GW\u000134=37\u0001369=8\u000152=20230831-08:42:49\u0001150=3\u000139=2\u000111=17\u000141=XDCR12101-2308230000008\u0001526=XDCR12101-2308230000008\u000137=XDCR12101-2308230000001\u000132=5\u000131=1000\u000117=XDCR12101-2308230000001\u000155=XDCR12101\u000154=2\u0001448=TEST\u00016464=5000\u000110=033\u0001")]
        //[DataRow("8=FIX.4.4\u00019=224\u000135=8\u000149=HNX\u000140=U\u000156=043.01GW\u000134=37\u0001369=8\u000152=20230831-08:42:49\u0001150=3\u000139=2\u000111=BACND\u000141=BACND-2308230000008\u0001526=BACND-2308230000008\u000137=XDCR12101-2308230000001\u000132=1000000\u000131=1000\u000117=XDCR12101-2308230000001\u000155=SSI\u000154=1\u0001448=TEST\u00016464=5000\u000110=033\u0001")] //   if (objOrder_Tag526 == null && objOrder_Tag41 == null)
        //[DataRow("8=FIX.4.4\u00019=224\u000135=8\u000149=HNX\u000140=U\u000156=043.01GW\u000134=37\u0001369=8\u000152=20230831-08:42:49\u0001150=3\u000139=2\u000111=BACND\u000141=BACND-2308230000008\u0001526=BACND-2308230000008\u000137=XDCR12101-2308230000001\u000132=1000\u000131=1000\u000117=XDCR12101-2308230000001\u000155=SSI\u000154=2\u0001448=\u00016464=5000\u000110=033\u0001")] //   if (objOrder_Tag526 == null && objOrder_Tag41 == null)
        public void TestResponseRecvFromHNX_ExecutionStatus(string Message)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(Message);
            _testKafkaInterface.HNXResponseExcQuote((MessageER_ExecOrder)messageBase);
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()));
        }

        // Lệnh khớp thông thường
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=243\u000135=8\u000149=HNX\u000156=043.01GW\u000134=95\u0001369=7\u000152=20231212-08:15:21\u0001150=3\u000139=2\u000111=TEST_13\u000141=XDCR12101-2312060000006\u0001526=XDCR12101-2312060000006\u000137=AAAR12101-2401150000001\u000132=200\u000131=10000\u000117=AAAR12101-2401150000001\u000155=AAAR12101\u000154=1\u00016464=2000000\u0001448=043\u000110=150\u0001")]
        [DataRow("8=FIX.4.4\u00019=243\u000135=8\u000149=HNX\u000156=043.01GW\u000134=95\u0001369=7\u000152=20231212-08:15:21\u0001150=3\u000139=2\u000111=TEST_13\u000141=XDCR12101-2312060000006\u0001526=XDCR12101-2312060000006\u000137=AAAR12101-2401150000001\u000132=200\u000131=10000\u000117=AAAR12101-2401150000001\u000155=AAAR12101\u000154=2\u00016464=2000000\u0001448=043\u000110=150\u0001")]
        //[DataRow("8=FIX.4.4\u00019=264\u000135=8\u000149=HNX\u000156=043.01GW\u000134=71\u0001369=4\u000152=20231228-07:29:16\u0001150=3\u000139=2\u000111=2777e45c-7e5b-4e7c-b521-b43a1df8dfac\u000141=XDCR12101-2402120000001\u0001526=XDCR12101-2402120000001\u000137=XDCR12101-2402120000001\u000132=10\u000131=10\u000117=XDCR12101-2402120000001\u000155=XDCR12101\u000154=2\u00016464=100\u0001448=043\u000110=164\u0001")]
        public void TestResponseRecvFromHNX_ExecutionStatus_2_Test(string Message)
        {
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            //
            FIXMessageBase messageBase = messageFactoryFIX.Parse(Message);
            _testKafkaInterface.HNXResponseExcQuote((MessageER_ExecOrder)messageBase);
            //
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()));
        }

        // MSG 35=N04

        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=339\u000135=N04\u000149=HNX\u000156=043.01GW\u000134=150\u0001369=34\u000152=20231107-07:56:03\u000111=b0dee40c-dc8c-4db2-ac6c-cea83d2af56c\u000154=1\u0001117=REPOSPT-2312060000024\u0001448=\u0001537=1\u0001644=REPOSPT-2312060000022\u000140=U\u00011=\u00014488=050\u0001552=1\u00015522=1\u000138=10\u000155=XDCR12101\u0001640=8793\u00016464=87930\u00016465=87936\u00012261=6\u00012260=2.3\u0001227=2.3\u0001226=1\u0001168=20231206\u000164=20231206\u0001193=20231207\u0001917=20231207\u00014499=015\u000110=128\u0001")]
        [DataRow("8=FIX.4.4\u00019=339\u000135=N04\u000149=HNX\u000156=043.01GW\u000134=148\u0001369=32\u000152=20231107-07:51:35\u000111=9b8634c6-b2ed-4679-87f6-b0e8cc11b834\u000154=2\u0001117=REPOSPT-2312060000023\u0001448=\u0001537=2\u0001644=REPOSPT-2312060000022\u000140=U\u00011=\u00014488=050\u0001552=1\u00015522=1\u000138=10\u000155=XDCR12101\u0001640=8793\u00016464=87930\u00016465=87936\u00012261=6\u00012260=2.3\u0001227=2.3\u0001226=1\u0001168=20231206\u000164=20231206\u0001193=20231207\u0001917=20231207\u00014499=01\u0001")]
        [DataRow("8=FIX.4.4\u00019=339\u000135=N04\u000149=HNX\u000156=043.01GW\u000134=150\u0001369=34\u000152=20231107-07:56:03\u000111=b0dee40c-dc8c-4db2-ac6c-cea83d2af56c\u000154=2\u0001117=REPOSPT-2312060000024\u0001448=\u0001537=3\u0001644=REPOSPT-2312060000022\u000140=U\u00011=\u00014488=043\u0001552=1\u00015522=1\u000138=10\u000155=XDCR12101\u0001640=8793\u00016464=87930\u00016465=87936\u00012261=6\u00012260=2.3\u0001227=2.3\u0001226=1\u0001168=20231206\u000164=20231206\u0001193=20231207\u0001917=20231207\u00014499=0\u0001")]
        [DataRow("8=FIX.4.4\u00019=345\u000135=N04\u000149=HNX\u000156=043.01GW\u000134=152\u0001369=35\u000152=20231107-08:02:23\u000111=045d3118-8ce5-422e-80c9-98636fc7a01e\u000154=3\u0001117=REPOSPT-2312060000026\u0001448=\u0001537=1\u0001644=REPOSPT-2312060000025\u000140=U\u00011=\u00014488=015\u0001552=1\u00015522=1\u000138=120\u000155=XDCR12101\u0001640=9600\u00016464=1152000\u00016465=1152073\u00012261=73\u00012260=4.0\u0001227=2.3\u0001226=1\u0001168=20231206\u000164=20231206\u0001193=20231207\u0001917=20231207\u00014499=0\u0001")]
        [DataRow("8=FIX.4.4\u00019=339\u000135=N04\u000149=HNX\u000156=043.01GW\u000134=150\u0001369=34\u000152=20231107-07:56:03\u000111=b0dee40c-dc8c-4db2-ac6c-cea83d2af56c\u000154=1\u0001117=REPOSPT-2312060000024\u0001448=\u0001537=4\u0001644=REPOSPT-2312060000022\u000140=U\u00011=\u00014488=050\u0001552=1\u00015522=1\u000138=10\u000155=XDCR12101\u0001640=8793\u00016464=87930\u00016465=87936\u00012261=6\u00012260=2.3\u0001227=2.3\u0001226=1\u0001168=20231206\u000164=20231206\u0001193=20231207\u0001917=20231207\u00014499=015\u000110=128\u0001")]
        public void TestResponseRecvFromHNX_Msg35_N04_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            MessageReposFirmReport newFirmRepos = (MessageReposFirmReport)messageBase;
            ReposSide _reposSide = new ReposSide();
            _reposSide.NumSide = 1;
            _reposSide.Symbol = "XDCR12101";
            _reposSide.OrderQty = 100;
            _reposSide.Price = 1000;
            _reposSide.HedgeRate = 1;
            //
            newFirmRepos.RepoSideList.Add(_reposSide);
            //
            FIXMessageBase messageBase2 = newFirmRepos;

            _testKafkaInterface.HNXResponse_ReposFirmReport((MessageReposFirmReport)messageBase2);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // MSG 35=N04 case trong memory
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=339\u000135=N04\u000149=HNX\u000156=043.01GW\u000134=150\u0001369=34\u000152=20231107-07:56:03\u000111=b0dee40c-dc8c-4db2-ac6c-cea83d2af56c\u000154=1\u0001117=REPOSPT-2312060000024\u0001448=\u0001537=3\u0001644=\u000140=U\u00011=\u00014488=050\u0001552=1\u00015522=1\u000138=10\u000155=XDCR12101\u0001640=8793\u00016464=87930\u00016465=87936\u00012261=6\u00012260=2.3\u0001227=2.3\u0001226=1\u0001168=20231206\u000164=20231206\u0001193=20231207\u0001917=20231207\u00014499=015\u000110=128\u0001")]
        [DataRow("8=FIX.4.4\u00019=339\u000135=N04\u000149=HNX\u000156=043.01GW\u000134=148\u0001369=32\u000152=20231107-07:51:35\u000111=9b8634c6-b2ed-4679-87f6-b0e8cc11b834\u000154=2\u0001117=REPOSPT-2312060000023\u0001448=\u0001537=2\u0001644=REPOSPT-2312060000022\u000140=U\u00011=\u00014488=050\u0001552=1\u00015522=1\u000138=10\u000155=XDCR12101\u0001640=8793\u00016464=87930\u00016465=87936\u00012261=6\u00012260=2.3\u0001227=2.3\u0001226=1\u0001168=20231206\u000164=20231206\u0001193=20231207\u0001917=20231207\u00014499=01\u0001")]
        public void TestResponseRecvFromHNX_Msg35_N04_2_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act

            _testKafkaInterface.HNXResponse_ReposFirmReport((MessageReposFirmReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // MSG 35=N04
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=309\u000135=N04\u000149=HNX\u000156=043.01GW\u000134=53\u0001369=6\u000152=20231207-02:35:28\u000111=TEST_3\u000154=2\u0001117=REPOSPT-2401090000002\u0001448=\u0001537=1\u0001644=REPOSPT-2401090000001\u000140=U\u00011=\u00014488=043\u0001552=1\u00015522=1\u000138=120\u000155=XDCR12101\u0001640=9800\u00016464=1176000\u00016465=1176064\u00012261=64\u00012260=2\u0001227=2\u0001226=1\u0001168=20240109\u000164=20240109\u0001193=20240110\u0001917=20240110\u00014499=043\u000110=067\u0001")]
        public void TestResponseRecvFromHNX_Msg35_N04_4_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act

            _testKafkaInterface.HNXResponse_ReposFirmReport((MessageReposFirmReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // MSG 35=EE
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=312\u000135=EE\u000149=HNX\u000156=043.01GW\u000134=156\u0001369=38\u000152=20231107-08:11:01\u0001448=050\u0001449=050\u00015632=1\u000137=XDCR12101-2312060000002\u000141=XDCR12101-2312060000006\u0001526=XDCR12101-2312060000006\u00016363=1\u0001226=2\u0001227=1.3\u0001552=1\u00015522=1\u000132=100\u000131=8910\u000144=9000\u000155=XDCR12101\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u000164=20231206\u0001193=20231208\u0001917=20231208\u000110=248\u0001")] // 448 = 449 = ConfigData.FirmID
        public void TestResponseRecvFromHNX_Msg35_EE_1_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_ExecOrderRepos((MessageExecOrderRepos)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()));
        }

        // MSG 35=EE
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=312\u000135=EE\u000149=HNX\u000156=043.01GW\u000134=156\u0001369=38\u000152=20231107-08:11:01\u0001448=050\u0001449=043\u00015632=1\u000137=XDCR12101-2312060000002\u000141=XDCR12101-2312060000006\u0001526=XDCR12101-2312060000006\u00016363=1\u0001226=2\u0001227=1.3\u0001552=1\u00015522=1\u000132=100\u000131=8910\u000144=9000\u000155=XDCR12101\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u000164=20231206\u0001193=20231208\u0001917=20231208\u000110=248\u0001")] // 448 != 449 |  448 = ConfigData.FirmID
        public void TestResponseRecvFromHNX_Msg35_EE_2_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_ExecOrderRepos((MessageExecOrderRepos)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()));
        }

        // MSG 35=EE
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=312\u000135=EE\u000149=HNX\u000156=043.01GW\u000134=156\u0001369=38\u000152=20231107-08:11:01\u0001448=043\u0001449=050\u00015632=1\u000137=XDCR12101-2312060000002\u000141=XDCR12101-2312060000006\u0001526=XDCR12101-2312060000006\u00016363=1\u0001226=2\u0001227=1.3\u0001552=1\u00015522=1\u000132=100\u000131=8910\u000144=9000\u000155=XDCR12101\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u000164=20231206\u0001193=20231208\u0001917=20231208\u000110=248\u0001")] // 448 != 449 |  449 = ConfigData.FirmID
        public void TestResponseRecvFromHNX_Msg35_EE_3_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_ExecOrderRepos((MessageExecOrderRepos)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()));
        }

        // MSG 35=EE
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=312\u000135=EE\u000149=HNX\u000156=043.01GW\u000134=156\u0001369=38\u000152=20231107-08:11:01\u0001448=050\u0001449=044\u00015632=1\u000137=XDCR12101-2312060000002\u000141=XDCR12101-2312060000006\u0001526=XDCR12101-2312060000006\u00016363=1\u0001226=2\u0001227=1.3\u0001552=1\u00015522=1\u000132=100\u000131=8910\u000144=9000\u000155=XDCR12101\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u000164=20231206\u0001193=20231208\u0001917=20231208\u000110=248\u0001")] // 448 != 449 &  != ConfigData.FirmID
        public void TestResponseRecvFromHNX_Msg35_EE_4_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_ExecOrderRepos((MessageExecOrderRepos)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()));
        }

		// MSG 35=EE
		[TestMethod()]
		[DataRow("8=FIX.4.4\u00019=312\u000135=EE\u000149=HNX\u000156=043.01GW\u000134=156\u0001369=38\u000152=20231107-08:11:01\u000111=48d5f383-b249-8886-9999-9c9e5f6316b8\u0001448=050\u0001449=050\u00015632=1\u000137=XDCR12101-2312060000002\u000141=XDCR12101-2312060000006-8\u0001526=XDCR12101-2312060000006-8\u00016363=1\u0001226=2\u0001227=1.3\u0001552=1\u00015522=1\u000132=100\u000131=8910\u000144=9000\u000155=XDCR12101\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u000164=20231206\u0001193=20231208\u0001917=20231208\u000110=248\u0001")] // 448 = 449 = ConfigData.FirmID
		[DataRow("8=FIX.4.4\u00019=312\u000135=EE\u000149=HNX\u000156=043.01GW\u000134=156\u0001369=38\u000152=20231107-08:11:01\u000111=48d5f383-b249-8886-9999-9c9e5f6316b8\u0001448=050\u0001449=050\u00015632=1\u000137=XDCR12101-2312060000002\u000141=XDCR12101-2312060000006-8\u0001526=XDCR12101-2312060000006-8\u00016363=1\u0001226=2\u0001227=1.3\u0001552=1\u00015522=1\u000132=100\u000131=8910\u000144=9000\u000155=XDCR12101\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u000164=20231206\u0001193=20231208\u0001917=20231208\u000110=248\u0001")] // 448 = 449 = ConfigData.FirmID
		public void TestResponseRecvFromHNX_Msg35_EE_5_Test(string msgRaw)
		{
			//Arrange
			Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
			ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
			FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
			// Act
			_testKafkaInterface.HNXResponse_ExecOrderRepos((MessageExecOrderRepos)messageBase);
			//Assert
			mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()));
		}

		// MSG 35=MR
		[TestMethod()]
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000007\u0001198=\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=1\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // 4488= TV Gate & 563=1 & 54= 1
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000007\u0001198=\u000154=2\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=1\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // 4488= TV Gate & 563=1 & 54= 2
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000007\u0001198=\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=1\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=043\u000110=141\u0001")] // 4488 != TV Gate & 563=1  & 54=1
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000007\u0001198=\u000154=2\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=1\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=043\u000110=141\u0001")] // 4488 != TV Gate & 563=1  & 54=2
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000007\u0001198=\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=2\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // 4488 == TV Gate & 563=2  & 54=1
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000007\u0001198=\u000154=2\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=2\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // 4488 == TV Gate & 563=2  & 54=2
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000007\u0001198=\u000154=2\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=2\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=043\u000110=141\u0001")] // 4488 != TV Gate & 563=2  & 54=2
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000007\u0001198=\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=3\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // 4488 == TV Gate & 563=3  & 54=1
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000007\u0001198=\u000154=2\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=3\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=043\u000110=141\u0001")] // 4488 != TV Gate & 563=3  & 54=2
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000007\u0001198=\u000154=2\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=3\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u0001537=2\u00014488=043\u000110=141\u0001")] // 4488 != TV Gate & 563=3  & 54=2 537=2
        public void TestResponseRecvFromHNX_Msg35_MR_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_ReposBCGDReport((MessageReposBCGDReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        // MSG 35=MR  // Cac case lien quan toi QuoteType
        [TestMethod()]
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000007\u0001198=\u000154=1\u000140=T\u00015632=2\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=1\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_1
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000007\u0001198=\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=2\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_2
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000007\u0001198=\u000154=1\u000140=T\u00015632=2\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=3\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_3
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000007\u0001198=\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=4\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_4
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=BACND\u000137=XDCR12101-2312060000007\u0001198=\u000154=2\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=4\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=047\u000110=141\u0001")] // QuoteType_4 & 4488 != TV Gate
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000007\u0001198=\u000154=2\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=4\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=047\u000110=141\u0001")] // QuoteType_4 & 4488 != TV Gate ClOrdID khong co trong memory
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=BACND\u000137=XDCR12101-2312060000007\u0001198=\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=5\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_5
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=BACND\u000137=XDCR12101-2312060000007\u0001198=\u000154=2\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=5\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_5
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000007\u0001198=\u000154=2\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=5\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=047\u00014499=007\u000110=141\u0001")] // QuoteType_5  & 4488 != TV Gate
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000007\u0001198=\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=5\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=047\u00014499=007\u000110=141\u0001")] // QuoteType_5  & 4488 != TV Gate
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000007\u0001198=\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=6\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_6
        public void TestResponseRecvFromHNX_Msg35_MR_2_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_ReposBCGDReport((MessageReposBCGDReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000007\u0001198=XDCR12101-2312060000007\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=7\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_7 khong co trong mem
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-23120600000072\u0001198=XDCR12101-2312060000007\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=7\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_7 check trong mem
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-23120600000072\u0001198=XDCR12101-23120600000072\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=7\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_7 check trong mem
        public void TestResponseRecvFromHNX_Msg35_MR_MatchReportType_1_QuoteType_7_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_ReposBCGDReport((MessageReposBCGDReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000010\u0001198=XDCR12101-2312060000010\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=8\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_8
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-23120600000072\u0001198=XDCR12101-2312060000010\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=8\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_8 check trong mem
        public void TestResponseRecvFromHNX_Msg35_MR_MatchReportType_1_QuoteType_8_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_ReposBCGDReport((MessageReposBCGDReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000011\u0001198=XDCR12101-2312060000011\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=9\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_9
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-23120600000112\u0001198=XDCR12101-2312060000011\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=9\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_9 check trong mem
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_432\u000137=XDCR12101-23120600000123\u0001198=XDCR12101-2312060000123\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=9\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_9 check trong mem
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_432\u000137=XDCR12101-23120600000124\u0001198=XDCR12101-2312060000124\u000154=1\u000140=T\u00015632=2\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=9\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_9 check trong mem
        public void TestResponseRecvFromHNX_Msg35_MR_MatchReportType_1_QuoteType_9_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_ReposBCGDReport((MessageReposBCGDReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000012\u0001198=XDCR12101-2312060000012\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=10\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_10
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-23120600000122\u0001198=XDCR12101-2312060000012\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=10\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_10 check trong mem
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_432\u000137=XDCR12101-23120600000125\u0001198=XDCR12101-2312060000125\u000154=1\u000140=T\u00015632=2\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=10\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_9 check trong mem
        public void TestResponseRecvFromHNX_Msg35_MR_MatchReportType_1_QuoteType_10_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_ReposBCGDReport((MessageReposBCGDReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000013\u0001198=XDCR12101-2312060000013\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=11\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_11
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-23120600000132\u0001198=XDCR12101-2312060000013\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=11\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_11 check trong mem
        public void TestResponseRecvFromHNX_Msg35_MR_MatchReportType_1_QuoteType_11_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_ReposBCGDReport((MessageReposBCGDReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000014\u0001198=XDCR12101-2312060000014\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=12\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_12
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-23120600000142\u0001198=XDCR12101-2312060000014\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=12\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_12 check trong mem
        public void TestResponseRecvFromHNX_Msg35_MR_MatchReportType_1_QuoteType_12_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_ReposBCGDReport((MessageReposBCGDReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000015\u0001198=XDCR12101-2312060000015\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=13\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_13
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-23120600000152\u0001198=XDCR12101-2312060000015\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=13\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_13 check trong mem
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-23120600000153\u0001198=XDCR12101-2412060000015\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=13\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_13 check k co trong mem
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-23120600000154\u0001198=XDCR12101-2412060000015\u000154=2\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=13\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_13 check k co trong mem
        public void TestResponseRecvFromHNX_Msg35_MR_MatchReportType_1_QuoteType_13_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_ReposBCGDReport((MessageReposBCGDReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000016\u0001198=XDCR12101-2312060000016\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=14\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_14
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-23120600000162\u0001198=XDCR12101-2312060000016\u000154=1\u000140=T\u00015632=1\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=14\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_14 check trong mem
        public void TestResponseRecvFromHNX_Msg35_MR_MatchReportType_1_QuoteType_14_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_ReposBCGDReport((MessageReposBCGDReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=48d5f383-b249-8888-ba87-9c9e5f6316b8\u000137=XDCR12101-2312060000017\u0001198=XDCR12101-2312060000017\u000154=1\u000140=T\u00015632=2\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=4\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_4 khong co trong mem
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_46\u000137=XDCR12101-2312060000018\u0001198=XDCR12101-2312060000018\u000154=1\u000140=T\u00015632=2\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=4\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_4 check trong mem
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_46\u000137=XDCR12101-23120600000182\u0001198=XDCR12101-2312060000018\u000154=2\u000140=T\u00015632=2\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=4\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_4 check trong mem
        public void TestResponseRecvFromHNX_Msg35_MR_MatchReportType_2_QuoteType_4_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_ReposBCGDReport((MessageReposBCGDReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000017\u0001198=XDCR12101-2312060000017\u000154=1\u000140=T\u00015632=2\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=7\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_7 khong co trong mem
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-23120600000172\u0001198=XDCR12101-2312060000017\u000154=1\u000140=T\u00015632=2\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=7\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_7 check trong mem
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-23120600000172\u0001198=XDCR12101-2312060000017\u000154=1\u000140=T\u00015632=2\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=7\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_7 check trong mem
        public void TestResponseRecvFromHNX_Msg35_MR_MatchReportType_2_QuoteType_7_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_ReposBCGDReport((MessageReposBCGDReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000019\u0001198=XDCR12101-2312060000019\u000154=1\u000140=T\u00015632=2\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=8\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_8 khong co trong mem
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-23120600000192\u0001198=XDCR12101-2312060000019\u000154=1\u000140=T\u00015632=2\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=8\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_8 check trong mem
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-23120600000192\u0001198=XDCR12101-2312060000019\u000154=1\u000140=T\u00015632=2\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=8\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_8 check trong mem
        public void TestResponseRecvFromHNX_Msg35_MR_MatchReportType_2_QuoteType_8_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_ReposBCGDReport((MessageReposBCGDReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-2312060000020\u0001198=XDCR12101-2312060000020\u000154=1\u000140=T\u00015632=2\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=9\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_9
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-23120600000202\u0001198=XDCR12101-2312060000020\u000154=1\u000140=T\u00015632=2\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=9\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_9
        [DataRow("8=FIX.4.4\u00019=338\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=157\u0001369=39\u000152=20231107-08:12:46\u000111=TEST_41\u000137=XDCR12101-23120600000202\u0001198=XDCR12101-2312060000020\u000154=1\u000140=T\u00015632=2\u0001448=043\u0001449=015\u00011=043C111111\u00012=015C111111\u0001563=9\u0001552=1\u00015522=1\u000138=100\u000144=9000\u000155=XDCR12101\u0001640=8910\u00016464=891000\u00016465=891063\u00012261=63\u00012260=1\u0001227=1.3\u0001226=2\u0001168=20231206\u000164=20231206\u0001193=20231208\u0001917=20231208\u00016363=1\u00014488=050\u000110=141\u0001")] // QuoteType_9
        public void TestResponseRecvFromHNX_Msg35_MR_MatchReportType_2_QuoteType_9_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_ReposBCGDReport((MessageReposBCGDReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        //
        [TestMethod]
        [DataRow("8=FIX.4.4\u00019=297\u000135=MR\u000149=HNX\u000156=043.01GW\u000134=96\u0001369=14\u000152=20231214-04:17:05\u000111=85449377-1187-4153-80a0-ab77a8bbcb3d\u000137=XDCR12101-2401170000009\u0001198=XDCR12101-2401170000008\u000154=1\u000140=T\u00015632=1\u0001448=003\u0001449=015\u00011=\u00012=015C111111\u0001563=18\u0001552=1\u0001227=1.2\u0001226=1\u0001168=20240117\u000164=20240117\u0001193=20240118\u0001917=20240118\u00016363=2\u00014488=015\u000110=172\u0001")]
        public void TestResponseRecvFromHNX_Msg35_MR_3_Test(string msgRaw)
        {
            //Arrange
            Mock<IKafkaClient> mockKafkaClient = new Mock<IKafkaClient>();
            ResponseInterface _testKafkaInterface = new ResponseInterface(mockKafkaClient.Object);
            FIXMessageBase messageBase = messageFactoryFIX.Parse(msgRaw);
            // Act
            _testKafkaInterface.HNXResponse_ReposBCGDReport((MessageReposBCGDReport)messageBase);
            //Assert
            mockKafkaClient.Verify(helper => helper.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, It.IsAny<Object>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        }

        #endregion Thông báo nhận lệnh từ sở, gửi về cho kafka
    }
}