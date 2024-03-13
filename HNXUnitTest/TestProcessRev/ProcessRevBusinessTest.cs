/*
 * Project: Unit Test
 * Author : Nguyen Tuan Dung – Navisoft.
 * Summary: Unit test for all Api
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 * Sep 10, 2023  	 Created
 */

using BusinessProcessAPIReq;
using BusinessProcessAPIReq.RequestModels;
using BusinessProcessAPIReq.ResponseModels;
using BusinessProcessResponse;
using Castle.Core.Logging;
using CommonLib;
using HNX.FIXMessage;
using HNXInterface;
using LocalMemory;
using Microsoft.Extensions.Configuration;
using Moq;
using static CommonLib.CommonData;

namespace HNXUnitTest
{
	[TestClass]
	public class ProcessRevBusinessTest
	{
		private MessageFactoryFIX c_MsgFactory = new MessageFactoryFIX();

		[TestInitialize]
		public void InitEnviroment()
		{
			var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
			ConfigData.InitConfig(config);
			MessageSecurityStatus SecurityStatus = (MessageSecurityStatus)c_MsgFactory.Parse("8=FIX.4.4\u00019=246\u000135=f\u000149=HNX\u000156=043.01GW\u000134=12\u0001369=1\u000152=20230922-02:52:22\u0001324=095105\u000155=XDCR12101\u0001167=\u0001541=20240326\u0001225=20210326\u0001106=TCPHCBIS\u000131=100000\u0001332=0\u0001333=0\u00013321=0\u00013331=0\u00013322=110000\u00013332=0\u0001326=0\u0001330=0\u0001625=DEMO\u00016251=1\u0001265=102\u0001109=8000000\u00019735=1,2\u00019736=1,2\u000110=231\u0001");
			TradingRuleData.ProcessSecurityStatus(SecurityStatus);
			MessageTradingSessionStatus SecurityTradingSession = (MessageTradingSessionStatus)c_MsgFactory.Parse("8=FIX.4.4\u00019=115\u000135=h\u000149=HNX\u000156=043.01GW\u000134=112\u0001369=2\u000152=20230927-01:55:56\u0001336=DEMO\u0001340=1\u0001341=20230927-01:55:56\u0001339=2\u0001335=XDCR12101\u000110=252\u0001");
			TradingRuleData.ProcessTradingSession(SecurityTradingSession);
			MessageTradingSessionStatus TradingSessionStatus = (MessageTradingSessionStatus)c_MsgFactory.Parse("8=FIX.4.4\u00019=110\u000135=h\u000149=HNX\u000156=050.02GW\u000134=110\u0001369=2\u000152=20230922-10:55:57\u0001336=DEMO\u0001340=1\u0001341=20230922-10:33:11\u0001339=1\u0001335=DEMO\u000110=220\u0001");
			TradingRuleData.ProcessTradingSession(TradingSessionStatus);
		}

		[TestMethod]
		[DataRow(1, "B", 0, "0", "8=FIX.4.4\u00019=180\u000135=S\u000149=043.01GW\u000156=HNX\u000134=8\u0001369=29\u000152=20230922-03:51:22\u000158=order_electronic\u000111=DUNG_TEST_1\u00011=043C111111\u000155=XDCR12101\u000154=1\u000138=10\u000140=S\u0001640=10\u00016464=0\u000164=20230922\u0001513=0\u00011111=0\u00016363=1\u000110=000\u0001")]
		[DataRow(2, "S", 0, "0", "8=FIX.4.4\u00019=180\u000135=S\u000149=043.01GW\u000156=HNX\u000134=9\u0001369=30\u000152=20230922-03:52:34\u000158=order_electronic\u000111=DUNG_TEST_2\u00011=043C111111\u000155=XDCR12101\u000154=2\u000138=10\u000140=S\u0001640=10\u00016464=0\u000164=20230922\u0001513=0\u00011111=0\u00016363=1\u000110=000\u0001")]
		[DataRow(3, "B", 1, "0", "8=FIX.4.4\u00019=181\u000135=S\u000149=043.01GW\u000156=HNX\u000134=10\u0001369=31\u000152=20230922-03:53:17\u000158=order_electronic\u000111=DUNG_TEST_3\u00011=043C111111\u000155=XDCR12101\u000154=1\u000138=10\u000140=S\u0001640=10\u00016464=0\u000164=20230922\u0001513=0\u00011111=1\u00016363=1\u000110=000\u0001")]
		[DataRow(4, "S", 1, "0", "8=FIX.4.4\u00019=181\u000135=S\u000149=043.01GW\u000156=HNX\u000134=11\u0001369=32\u000152=20230922-03:53:28\u000158=order_electronic\u000111=DUNG_TEST_4\u00011=043C111111\u000155=XDCR12101\u000154=2\u000138=10\u000140=S\u0001640=10\u00016464=0\u000164=20230922\u0001513=0\u00011111=1\u00016363=1\u000110=000\u0001")]
		[DataRow(5, "S", 1, "021,043,003", "8=FIX.4.4\u00019=191\u000135=S\u000149=043.01GW\u000156=HNX\u000134=12\u0001369=33\u000152=20230922-03:57:00\u000158=order_electronic\u000111=DUNG_TEST_5\u00011=043C111111\u000155=XDCR12101\u000154=2\u000138=10\u000140=S\u0001640=10\u00016464=0\u000164=20230922\u0001513=021,043,003\u00011111=1\u00016363=1\u000110=000\u0001")]
		public async Task TestAPI1_SUCCESS(int orderNo, string side, int isVisile, string registID, string expectMsgRaw)
		{
			//Case 1
			//Arrange

			var request1 = new API1NewElectronicPutThroughRequest()
			{
				OrderNo = "DUNG_TEST_API1_" + orderNo.ToString(),
				ClientID = ConfigData.FirmID + "C111111",
				OrderType = "S",
				Side = side,
				Symbol = "XDCR12101",
				Price = 10,
				OrderQty = 10,
				SettleDate = "20230922",
				SettleMethod = 1,
				RegistID = registID,
				IsVisible = isVisile,
				Text = "order_electronic"
			};

			var response = new API1NewElectronicPutThroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			MessageQuote ExpectQuote = (MessageQuote)c_MsgFactory.Parse(expectMsgRaw);
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);

			//Act
			var result = await _ProcessRevBussiness.API1NewElectronicPutThrough_BU(request1, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.Is<MessageQuote>(Result =>
			//Check xem đúng tag chưa ở đây
			Result.Side == ExpectQuote.Side &&
			Result.Symbol == ExpectQuote.Symbol &&
			Result.Price == ExpectQuote.Price &&
			Result.IsVisible == ExpectQuote.IsVisible &&
			Result.RegistID == ExpectQuote.RegistID
			), It.IsAny<int>()),
			Times.Once);
		}

		[TestMethod]
		public async Task TestApi1_DupplicateOrderNo()
		{
			//Case 1
			//Act
			var request1 = new API1NewElectronicPutThroughRequest()
			{
				OrderNo = "DUNG_TEST_API1_DUPPLICATE",
				ClientID = ConfigData.FirmID + "C111111",
				OrderType = "S",
				Side = "B",
				Symbol = "XDCR12101",
				Price = 10,
				OrderQty = 10,
				SettleDate = "20230922",
				SettleMethod = 1,
				RegistID = "0",
				IsVisible = 0,
				Text = "order_electronic"
			};
			var response = new API1NewElectronicPutThroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API1NewElectronicPutThrough_BU(request1, response);
			result = await _ProcessRevBussiness.API1NewElectronicPutThrough_BU(request1, response);
			Assert.AreEqual(result.Message, CommonData.ORDER_RETURNMESSAGE.OrderNo_Duplicated);
			Assert.AreEqual(result.ReturnCode, CommonData.ORDER_RETURNCODE.OrderNo_Duplicated);
		}

		[TestMethod]
		public async Task TestApi1_CheckSessionFail()
		{
			//Case 1
			//Act
			var request1 = new API1NewElectronicPutThroughRequest()
			{
				OrderNo = "DUNG_TEST_API1_NÓYMBOL",
				ClientID = ConfigData.FirmID + "C111111",
				OrderType = "S",
				Side = "B",
				Symbol = "NONAMSECURITY",
				Price = 10,
				OrderQty = 10,
				SettleDate = "20230922",
				SettleMethod = 1,
				RegistID = "0",
				IsVisible = 0,
				Text = "order_electronic"
			};
			var response = new API1NewElectronicPutThroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API1NewElectronicPutThrough_BU(request1, response);
			Assert.AreEqual(response.Message, ORDER_RETURNMESSAGE.SYMBOL_IS_NOT_FOUND);
			Assert.AreEqual(response.ReturnCode, ORDER_RETURNCODE.SYMBOL_IS_NOT_FOUND);
		}

		[TestMethod]
		[DataRow("OrderNo_API2_01", "OrderNo1", "ExchangeIDApi2_1", true, 1)]//Do ctck mình gửi
		[DataRow("OrderNo_API2_02", "", "ExchangeIDApi2_2", true, 1)]//Do ctck khác gưi
		[DataRow("OrderNo_API2_03", "", "ExchangeIDApi2_3", false, 1)]//chưa có trong mem
		[DataRow("OrderNo_API2_04", "OrderNo4", "ExchangeIDApi2_4", true, 2)]//Do ctck mình gửi
		public async Task TestApi2_SUCCESS(string OrderNoApi, string OrderNo, string RefExchangeID, bool IsInMem, int pCase)
		{
			string pSide = ORDER_SIDE.SIDE_BUY;
			//
			if (pCase == 1)
			{
				if (IsInMem)
				{
					OrderMemory.Add_NewOrder(new OrderInfo()
					{
						RefMsgType = MessageType.Quote,
						ExchangeID = RefExchangeID,
						OrderNo = OrderNo
					});
				}
			}
			if (pCase == 2)
			{
				pSide = ORDER_SIDE.SIDE_SELL;
				OrderMemory.Add_NewOrder(new OrderInfo()
				{
					RefMsgType = MessageType.Quote,
					RefExchangeID = RefExchangeID,
					ExchangeID = RefExchangeID,
					OrderNo = OrderNo
				});

				OrderMemory.Add_NewOrder(new OrderInfo()
				{
					RefMsgType = MessageType.QuoteResponse,
					ExchangeID = RefExchangeID,
					OrderNo = OrderNo
				});
			}

			//Arrange
			var request = new API2AcceptElectronicPutThroughRequest()
			{
				OrderNo = OrderNoApi,
				RefExchangeID = RefExchangeID,
				ClientID = ConfigData.FirmID + "CX00002",
				OrderType = "S",
				Side = pSide,
				Symbol = "XDCR12101",
				Price = 10000,
				OrderQty = 1000,
				SettleDate = "20230810",
				SettleMethod = 1,
				Text = "CHAP NHAN TTĐT"
			};

			var response = new API2AcceptElectronicPutThroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API2AcceptElectronicPutThrough_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
		}

		[TestMethod]
		public async Task TestApi2_CheckSessionFail()
		{
			//Arrange
			var request = new API2AcceptElectronicPutThroughRequest()
			{
				OrderNo = "DUNG_TEST_API2_NO_SYMBOL",
				RefExchangeID = "XDCR12101-2308100000011",
				ClientID = ConfigData.FirmID + "CX00002",
				OrderType = "S",
				Side = "S",
				Symbol = "NONAMESCURITY",
				Price = 10000,
				OrderQty = 1000,
				SettleDate = "20230810",
				SettleMethod = 1,
				Text = "CHAP NHAN TTĐT"
			};
			var response = new API2AcceptElectronicPutThroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API2AcceptElectronicPutThrough_BU(request, response);
			Assert.AreNotEqual(response.Message, ORDER_RETURNMESSAGE.SUCCESS);
			Assert.AreNotEqual(response.Message, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow(1, "S", "0", "8=FIX.4.4\u00019=217\u000135=R\u000149=043.01GW\u000156=HNX\u000134=18\u0001369=40\u000152=20230922-07:18:17\u000158=replace electronic\u000111=638309890976168484\u0001644=XDCR12101-2309210000009\u000140=S\u00011=043C111111\u000155=XDCR12101\u000154=2\u000138=9\u0001640=12\u00016464=0\u000164=20230922\u0001513=0\u00016363=2\u00011111=0\u000110=000\u0001")]
		[DataRow(1, "B", "0", "8=FIX.4.4\u00019=217\u000135=R\u000149=043.01GW\u000156=HNX\u000134=19\u0001369=41\u000152=20230922-07:18:59\u000158=replace electronic\u000111=638309891393388612\u0001644=XDCR12101-2309210000009\u000140=S\u00011=043C111111\u000155=XDCR12101\u000154=1\u000138=9\u0001640=12\u00016464=0\u000164=20230922\u0001513=0\u00016363=2\u00011111=0\u000110=000\u0001")]
		public async Task TestAPI3_SUCCESS(int orderNo, string side, string registID, string expectedMsgRaw)
		{
			var request = new API3ReplaceElectronicPutThroughRequest()
			{
				OrderNo = "DUNG_TEST_API3_" + orderNo.ToString(),
				RefExchangeID = "XDCR12101-2308100000013",
				ClientID = ConfigData.FirmID + "CX00002",
				OrderType = "S",
				Side = side,
				Symbol = "XDCR12101",
				Price = 1000,
				OrderQty = 8,
				SettleDate = "20230810",
				SettleMethod = 2,
				RegistID = registID,
				Text = "SỬA"
			};

			FIXMessageBase SampleMsgBase = c_MsgFactory.Parse(expectedMsgRaw);
			MessageQuoteRequest ExpectedQuoteRequest = (MessageQuoteRequest)SampleMsgBase;
			var response = new API3ReplaceElectronicPutThroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API3ReplaceElectronicPutThrough_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.Is<MessageQuoteRequest>(Result =>
			//Check Map Message ở đây
			Result.Price2 != 0
			), It.IsAny<int>()), Times.Once);
		}

		[TestMethod]
		[DataRow("8=FIX.4.4\u00019=154\u000135=Z\u000149=043.01GW\u000156=HNX\u000134=24\u0001369=138\u000152=20230925-03:25:13\u000158=cancel_electronic\u000111=638312343137230582\u0001298=4\u0001171=XDCR12101-2309210000014\u000140=S\u000155=XDCR12101\u000110=000\u0001")]
		public async Task TestApi4_SUCCESS(string expectMsgRaw)
		{
			//Arrange
			var request = new API4CancelElectronicPutThroughRequest()
			{
				OrderNo = "DUNGTEST_API4_1",
				RefExchangeID = "XDCR12101-2309210000014",
				OrderType = "S",
				Symbol = "XDCR12101",
				Text = "cancel_electronic"
			};

			var response = new API4CancelElectronicPutThroughResponse();
			var mockLogger = new Mock<ILogger>();
			MessageQuoteCancel SampleMsg = (MessageQuoteCancel)c_MsgFactory.Parse(expectMsgRaw);
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API4CancelElectronicPutThrough_BU(request, response);

			//Verify
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.Is<MessageQuoteCancel>(fixmsg =>

			fixmsg.Symbol == SampleMsg.Symbol &&
			fixmsg.OrdType == SampleMsg.OrdType
			), It.IsAny<int>()), Times.Once);
		}

		[TestMethod]
		[DataRow("DUNG_TEST_API5_1", "B")]
		[DataRow("DUNG_TEST_API5_2", "S")]
		public async Task TestAPI5_Success(string p_OrderNo, string p_Side)
		{
			var request = new API5NewCommonPutThroughRequest()
			{
				OrderNo = p_OrderNo,
				ClientID = ConfigData.FirmID + "C111111",
				ClientIDCounterFirm = "050CX00002",
				MemberCounterFirm = "050",
				OrderType = "R",
				Side = p_Side,
				Symbol = "XDCR12101",
				Price = 100,
				OrderQty = 100,
				SettleDate = "20230921",
				SettleMethod = 2,
				CrossType = 1,
				EffectiveTime = "20230921",
				Text = "order common"
			};

			var response = new API5NewCommonPutThroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);

			//Act
			var result = await _ProcessRevBussiness.API5NewCommonPutThrough_BU(request, response);

			//Assert
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.Is<MessageNewOrderCross>(result =>
			(result.Symbol == request.Symbol) &&
			(result.Text == request.Text) &&
			(result.Price2 == request.Price)
			), It.IsAny<int>()), Times.Once);
		}

		[TestMethod]
		[DataRow("DUNG_TEST_API_6_1", "XDCR12101-2309200000003", "S", true)]
		[DataRow("DUNG_TEST_API_6_2", "XDCR12101-2309200000004", "B", true)]
		[DataRow("DUNG_TEST_API_6_3", "XDCR12101-2309200000005", "B", false)]
		public async Task TestAPI6_SUCCESS(string p_OrderNo, string p_RefExchangeID, string p_Side, bool IsInMem)
		{
			//Setup
			var request = new API6AcceptCommonPutThroughRequest()
			{
				OrderNo = p_OrderNo,
				RefExchangeID = p_RefExchangeID,
				ClientID = "043C111111",
				ClientIDCounterFirm = "050C111111",
				MemberCounterFirm = "050",
				OrderType = "R",
				Side = p_Side,
				Symbol = "XDCR12101",
				Price = 100,
				OrderQty = 100,
				SettleDate = "20230920",
				SettleMethod = 2,
				CrossType = 3,
				EffectiveTime = "20230920",
				Text = "confirm common"
			};
			if (IsInMem)
			{
				OrderMemory.Add_NewOrder(new OrderInfo()
				{
					RefMsgType = MessageType.NewOrderCross,
					ExchangeID = p_RefExchangeID,
					Side = p_Side
				});
			}

			var response = new API6AcceptCommonPutThroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object); ;
			//Act
			var result = await _ProcessRevBussiness.API6AcceptCommonPutThrough_BU(request, response);

			//Assert
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.Is<MessageNewOrderCross>(fixmsg =>
			fixmsg.OrderQty == request.OrderQty &&
			fixmsg.Symbol == request.Symbol &&
			fixmsg.Text == request.Text

			), It.IsAny<int>()), Times.Once);
		}

		[TestMethod]
		[DataRow("DUNG_TEST_API7_1", "S")]
		[DataRow("DUNG_TEST_API7_2", "B")]
		public async Task TestAPI7_SUCCESS(string p_OrderNo, string p_Side)
		{
			var request = new API7ReplaceCommonPutThroughRequest()
			{
				OrderNo = p_OrderNo,
				RefExchangeID = "XDCR12101-2309040000007",
				ClientID = "003C111111",
				ClientIDCounterFirm = "043C111111",
				MemberCounterFirm = "043",
				OrderType = "R",
				CrossType = 1,
				Side = p_Side,
				Symbol = "XDCR12101",
				Price = 12,
				OrderQty = 12,
				SettleDate = "20230915",
				SettleMethod = 2,
				EffectiveTime = "20230915",
				Text = "replace common"
			};

			var response = new API7ReplaceCommonPutThroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API7ReplaceCommonPutThrough_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.AtLeastOnce);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow(ORDER_SIDE.SIDE_BUY)]
		[DataRow(ORDER_SIDE.SIDE_SELL)]
		public async Task TestAPI8_SUCCESS(string p_Side)
		{
			var request = new API8CancelCommonPutThroughRequest()
			{
				OrderNo = "DUNG_TEST_API8_",
				RefExchangeID = "XDCR12101-2308310000008",
				OrderType = "R",
				Symbol = "XDCR12101",
				Side = p_Side,
				CrossType = 1,
				Text = "cancel common"
			};

			var response = new API8CancelCommonPutThroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API8CancelCommonPutThrough_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.Is<CrossOrderCancelRequest>(fixMsg =>
			fixMsg.Symbol == request.Symbol &&
			fixMsg.CrossType == request.CrossType &&
			fixMsg.Text == request.Text
			), It.IsAny<int>()));
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow("DUNG_TEST_API_9_1", "B", "XDCR12101-2309150000004", "EXCHANGEID-2309150000004")]
		[DataRow("DUNG_TEST_API_9_2", "S", "XDCR12101-2309150000005", "EXCHANGEID-2309150000005")]
		[DataRow("DUNG_TEST_API_9_2", "S", "XDCR12101-2309150000006", "EXCHANGEID-2309150000006")]
		[DataRow("DUNG_TEST_API_9_2", "B", "", "")]
		public async Task TestAPI9_SUCCESS(string p_OrderNo, string p_Side, string p_refExchangeID, string p_ExchangeID)
		{
			// Bỏ qua để tìm trong memory không thấy
			if (p_refExchangeID != "XDCR12101-2309150000006")
			{
				OrderMemory.Add_NewOrder(new OrderInfo()
				{
					RefMsgType = MessageType.CrossOrderCancelReplaceRequest,
					RefExchangeID = p_refExchangeID,
					ExchangeID = p_ExchangeID,
					OrderNo = p_OrderNo,
					Side = p_Side
				});
			}
			var request = new API9ReplaceCommonPutThroughDealRequest()
			{
				OrderNo = p_OrderNo,
				RefExchangeID = p_refExchangeID,
				ClientID = "043C111111",
				ClientIDCounterFirm = "043CX00002",
				MemberCounterFirm = "043",
				OrderType = "S",
				CrossType = 2,
				Side = p_Side,
				Symbol = "XDCR12101",
				Price = 1000,
				OrderQty = 5,
				SettleDate = "20230915",
				SettleMethod = 2,
				EffectiveTime = "20230915",
				Text = "replace deal"
			};

			var response = new API9ReplaceCommonPutThroughDealResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API9ReplaceCommonPutThroughDeal_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.Is<CrossOrderCancelReplaceRequest>(fixMsg =>
			(fixMsg.Price2 == request.Price) &&
			(fixMsg.OrderQty == request.OrderQty)
			), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow("DUNG_TEST_API10_1", "XDCR12101-2309200000006", "B")]
		[DataRow("DUNG_TEST_API10_2", "XDCR12101-2309200000007", "S")]
		public async Task TestAPI10_SUCCESS(string p_OrderNo, string p_RefExchangeID, string p_Side)
		{
			var request = new API10ResponseForReplacingCommonPutThroughDealRequest()
			{
				OrderNo = p_OrderNo,
				RefExchangeID = p_RefExchangeID,
				ClientID = "043C111111",
				ClientIDCounterFirm = "050CX00002",
				MemberCounterFirm = "050",
				OrderType = "S",
				CrossType = 4,
				Side = p_Side,
				Symbol = "XDCR12101",
				Price = 10,
				OrderQty = 10,
				SettleDate = "20230920",
				SettleMethod = 2,
				EffectiveTime = "20230920",
				Text = "confirm replace deal"
			};

			var response = new API10ResponseForReplacingCommonPutThroughDealResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API10ResponseForReplacingCommonPutThroughDeal_BU(request, response);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
		}

		[TestMethod]
		[DataRow("OrderNo_1", ORDER_SIDE.SIDE_BUY)]
		[DataRow("OrderNo_2", ORDER_SIDE.SIDE_SELL)]
		public async Task TestAPI11_SUCCESS(string p_OrderNo, string p_Side)
		{
			var request = new API11CancelCommonPutThroughDealRequest()
			{
				OrderNo = p_OrderNo,
				RefExchangeID = "XDCR12101-2309190000007",
				OrderType = "S",
				Symbol = "XDCR12101",
				Side = p_Side,
				CrossType = 2,
				Text = "cancel deal"
			};

			var response = new API11CancelCommonPutThroughDealResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API11CancelCommonPutThroughDeal_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow("DUNG_TEST_API12_1", "B")]
		[DataRow("DUNG_TEST_API12_1", "S")]
		public async Task TestAPI12_SUCCESS(string p_OrderNo, string p_Side)
		{
			var request = new API12ResponseForCancelingCommonPutThroughDealRequest()
			{
				OrderNo = "",
				RefExchangeID = "XDCR12101-2309200000005",
				OrderType = "S",
				Symbol = "XDCR12101",
				Side = "S",
				CrossType = 4,
				Text = "confirm cancel deal"
			};

			var response = new API12ResponseForCancelingCommonPutThroughDealResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API12ResponseForCancelingCommonPutThroughDeal_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow("BACND_TEST_API13_1", "", "B")]
		[DataRow("BACND_TEST_API13_2", "", "S")]
		[DataRow("BACND_TEST_API13_3", "XDCR12101", "B")]
		[DataRow("BACND_TEST_API13_4", "XDCR12101", "S")]
		public async Task TestAPI13(string p_OrderNo, string p_Symbol, string p_Side)
		{
			var request = new API13NewInquiryReposRequest()
			{
				OrderNo = p_OrderNo,
				Symbol = p_Symbol,
				QuoteType = QuoteType.LENH_DAT_INQUIRY,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Side = p_Side,
				OrderValue = 5,
				EffectiveTime = "20231024",
				SettleMethod = 1,
				SettleDate1 = "20231024",
				SettleDate2 = "20231024",
				EndDate = "20231024",
				RepurchaseTerm = 1,
				RegistID = "0",
				Text = "DAT LENH INQUIRY REPOS"
			};

			var response = new API13NewInquiryReposResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API13NewInquiryRepos_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow("BACND_TEST_API14_1", "B", "RefExchangeID_14_1")]
		[DataRow("BACND_TEST_API14_2", "S", "RefExchangeID_14_2")]
		public async Task TestAPI14_SUCCESS(string p_OrderNo, string p_Side, string RefExchangeID)
		{
			var request = new API14ReplaceInquiryReposRequest()
			{
				OrderNo = p_OrderNo,
				Symbol = "XDCR12101",
				QuoteType = QuoteType.LENH_SUA_INQUIRY,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Side = p_Side,
				OrderValue = 5,
				RefExchangeID = RefExchangeID,
				EffectiveTime = "20231024",
				SettleMethod = 1,
				SettleDate1 = "20231024",
				SettleDate2 = "20231024",
				EndDate = "20231024",
				RepurchaseTerm = 1,
				RegistID = "0",
				Text = "DAT LENH INQUIRY REPOS"
			};

			var response = new API14ReplaceInquiryReposResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API14ReplaceInquiryRepos_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow("BACND_TEST_API15_1")]
		[DataRow("BACND_TEST_API15_2")]
		public async Task TestAPI15_SUCCESS(string p_OrderNo)
		{
			var request = new API15CancelInquiryReposRequest()
			{
				OrderNo = p_OrderNo,
				RefExchangeID = "XDCR12101",
				Symbol = "XDCR12101",
				QuoteType = QuoteType.LENH_HUY_INQUIRY,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Text = "HUY LENH INQUIRY REPOS"
			};

			var response = new API15CancelInquiryReposResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API15CancelInquiryRepos_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow("BACND_TEST_API16_1")]
		[DataRow("BACND_TEST_API16_2")]
		public async Task TestAPI16_SUCCESS(string p_OrderNo)
		{
			var request = new API16CloseInquiryReposRequest()
			{
				OrderNo = p_OrderNo,
				RefExchangeID = "XDCR12101",
				Symbol = "XDCR12101",
				QuoteType = QuoteType.LENH_DONG_INQUIRY,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Text = "HUY DONG INQUIRY REPOS"
			};

			var response = new API16CloseInquiryReposResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API16CloseInquiryRepos_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow("NO_17_1", ORDER_SIDE.SIDE_BUY)]
		[DataRow("NO_17_2", ORDER_SIDE.SIDE_SELL)]
		public async Task TestAPI17_SUCCESS(string pOrderNo, string pSide)
		{
			API17OrderNewFirmReposRequest request = new API17OrderNewFirmReposRequest()
			{
				OrderNo = pOrderNo,
				RefExchangeID = "RefExchangeID_XDCR12101",
				QuoteType = QuoteTypeRepos.LENH_DAT,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Side = pSide,
				ClientID = "ClientID_API17",
				EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
				SettleMethod = ORDER_SETTLMETHOD.PAYMENT_NOW,
				SettleDate1 = DateTime.Now.ToString("yyyyMMdd"),
				SettleDate2 = DateTime.Now.ToString("yyyyMMdd"),
				EndDate = DateTime.Now.ToString("yyyyMMdd"),
				RepurchaseTerm = 1,
				RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST NO 17"
			};
			//
			APIReposSideList _APIReposSideList = new APIReposSideList();
			_APIReposSideList.NumSide = 1;
			_APIReposSideList.Symbol = "XDCR12101";
			_APIReposSideList.OrderQty = 1000;
			_APIReposSideList.MergePrice = 1;
			_APIReposSideList.HedgeRate = 1;
			var _SymbolFirmInfo = new List<APIReposSideList>();
			_SymbolFirmInfo.Add(_APIReposSideList);
			request.SymbolFirmInfo = _SymbolFirmInfo;

			var response = new API17OrderNewFirmReposResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API17OrderNewFirmRepos_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow(ORDER_SIDE.SIDE_BUY)]
		[DataRow(ORDER_SIDE.SIDE_SELL)]
		public async Task TestAPI18_SUCCESS(string pSide)
		{
			var request = new API18OrderReplaceFirmReposRequest()
			{
				OrderNo = "NO_17_1",
				RefExchangeID = "XDCR12101",
				QuoteType = QuoteTypeRepos.LENH_SUA,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Side = pSide,
				ClientID = "ClientID_API18",
				EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
				SettleMethod = ORDER_SETTLMETHOD.PAYMENT_NOW,
				SettleDate1 = DateTime.Now.ToString("yyyyMMdd"),
				SettleDate2 = DateTime.Now.ToString("yyyyMMdd"),
				EndDate = DateTime.Now.ToString("yyyyMMdd"),
				RepurchaseTerm = 1,
				RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST API 18"
			};
			//
			APIReposSideList _APIReposSideList = new APIReposSideList();
			_APIReposSideList.NumSide = 1;
			_APIReposSideList.Symbol = "XDCR12101";
			_APIReposSideList.OrderQty = 1000;
			_APIReposSideList.MergePrice = 1;
			_APIReposSideList.HedgeRate = 1;
			var _SymbolFirmInfo = new List<APIReposSideList>();
			_SymbolFirmInfo.Add(_APIReposSideList);
			request.SymbolFirmInfo = _SymbolFirmInfo;
			//
			var response = new API18OrderReplaceFirmReposResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API18OrderReplaceFirmRepos_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		public async Task TestAPI19_SUCCESS()
		{
			var request = new API19OrderCancelFirmReposRequest()
			{
				OrderNo = "NO_17_1",
				RefExchangeID = "XDCR12101",
				QuoteType = QuoteTypeRepos.LENH_HUY,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Text = "TEST API 19"
			};

			var response = new API19OrderCancelFirmReposResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API19OrderCancelFirmRepos_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow("N03", "ODERNO_20_1", "ExchangeID_20_1", "RefExchangeID_20_1", "S", "ClientID_20_1", true,false)] // RefExchangeID gom side S
		[DataRow("N05", "ODERNO_20_2", "ExchangeID_20_2", "RefExchangeID_20_1", "B", "ClientID_20_2", true, false)] // RefExchangeID gom side B

		[DataRow("N03","ODERNO_20_3", "ExchangeID_20_3", "ExchangeID_20_3", "S", "ClientID_20_2", true, false)] // RefExchangeID =ExchangeID  = count =1= N03
		[DataRow("N03", "", "ExchangeID_20_4", "ExchangeID_20_4", "S", "ClientID_20_2", true, false)] // RefExchangeID =ExchangeID  = count =1 = OrderNO =1
		[DataRow("N05", "ODERNO_20_5", "ExchangeID_20_5", "ExchangeID_20_5", "S", "ClientID_20_2", true, false)] // RefExchangeID =ExchangeID  = count =1 = N05

		[DataRow("", "ODERNO_20_6", "ExchangeID_20_6", "ExchangeID_20_6", "S", "ClientID_20_2", false,true)] // RefExchangeID =ExchangeID  = count =2= N03 =N05


		//[DataRow("N00","ODERNO_20_3", "ExchangeID_20_4", "ExchangeID_20_5", "S", "ClientID_20_2", true)] //RefExchangeID != ExchangeID  = khong tim thay
		public async Task TestAPI20_SUCCESS(string p_MsgType, string p_OrderNo, string p_ExchangeID, string p_RefExchangeID, string p_Side, string p_ClientID, bool p_IsAddMem,bool p_IsAdminDuplicate)
		{
			// Init Mem
			if (p_IsAddMem)
			{
				OrderMemory.Add_NewOrder(new OrderInfo()
				{
					RefMsgType = p_MsgType,
					OrderNo = p_OrderNo,
					ClOrdID = "",
					ExchangeID = p_ExchangeID,
					RefExchangeID = p_RefExchangeID,
					SeqNum = 0,  // khi nào sở về mới update
					Symbol = "",
					Side = p_Side,
					Price = 0,
					OrderQty = 0,
					QuoteType = QuoteTypeRepos.LENH_DAT,
					CrossType = "", // ?
					ClientID = p_ClientID,
					ClientIDCounterFirm = "", // ?
					MemberCounterFirm = "", // ?
					OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
					RepurchaseRate = 1,
					NoSide = 1
				});
			}
			//
			if (p_IsAdminDuplicate)
			{
				OrderMemory.Add_NewOrder(new OrderInfo()
				{
					RefMsgType = "N03",
					OrderNo = p_OrderNo,
					ClOrdID = "",
					ExchangeID = p_ExchangeID,
					RefExchangeID = p_RefExchangeID,
					SeqNum = 0,  // khi nào sở về mới update
					Symbol = "",
					Side = p_Side,
					Price = 0,
					OrderQty = 0,
					QuoteType = QuoteTypeRepos.LENH_DAT,
					CrossType = "", // ?
					ClientID = p_ClientID,
					ClientIDCounterFirm = "", // ?
					MemberCounterFirm = "", // ?
					OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
					RepurchaseRate = 1,
					NoSide = 1
				});
				//
				OrderMemory.Add_NewOrder(new OrderInfo()
				{
					RefMsgType = "N05",
					OrderNo = p_OrderNo,
					ClOrdID = "",
					ExchangeID = p_ExchangeID,
					RefExchangeID = p_RefExchangeID,
					SeqNum = 0,  // khi nào sở về mới update
					Symbol = "",
					Side = p_Side,
					Price = 0,
					OrderQty = 0,
					QuoteType = QuoteTypeRepos.LENH_DAT,
					CrossType = "", // ?
					ClientID = p_ClientID,
					ClientIDCounterFirm = "", // ?
					MemberCounterFirm = "", // ?
					OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
					RepurchaseRate = 1,
					NoSide = 1
				});
			}
			var request = new API20OrderConfirmFirmReposRequest()
			{
				OrderNo = p_OrderNo,
				RefExchangeID = p_RefExchangeID,
				QuoteType = QuoteTypeRepos.LENH_DAT,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				ClientID = p_ClientID,
				RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST API 20"
			};
			//
			APIReposSideList _APIReposSideList = new APIReposSideList();
			_APIReposSideList.NumSide = 1;
			_APIReposSideList.Symbol = "XDCR12101";
			_APIReposSideList.OrderQty = 1000;
			_APIReposSideList.MergePrice = 1;
			_APIReposSideList.HedgeRate = 1;
			var _SymbolFirmInfo = new List<APIReposSideList>();
			_SymbolFirmInfo.Add(_APIReposSideList);
			request.SymbolFirmInfo = _SymbolFirmInfo;

			var response = new API20OrderConfirmFirmReposResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API20OrderConfirmFirmRepos_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow("NO_21_1", ORDER_SIDE.SIDE_BUY, "003")] // request.MemberCounterFirm == ConfigData.FirmID
		[DataRow("NO_21_2", ORDER_SIDE.SIDE_SELL, "003")] // request.MemberCounterFirm == ConfigData.FirmID
		[DataRow("NO_21_3", ORDER_SIDE.SIDE_BUY, "004")] // request.MemberCounterFirm != ConfigData.FirmID
		public async Task TestAPI21_SUCCESS(string pOrderNo, string p_Side, string p_FirmID)
		{
			var request = new API21OrderNewReposCommonPutthroughRequest()
			{
				OrderNo = pOrderNo,
				QuoteType = QuoteTypeRepos.LENH_DAT,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Side = p_Side,
				ClientID = "ClientID_API21",
				MemberCounterFirm = p_FirmID,
				EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
				SettleMethod = ORDER_SETTLMETHOD.PAYMENT_NOW,
				SettleDate1 = DateTime.Now.ToString("yyyyMMdd"),
				SettleDate2 = DateTime.Now.ToString("yyyyMMdd"),
				EndDate = DateTime.Now.ToString("yyyyMMdd"),
				RepurchaseTerm = 1,
				RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST API 21"
			};
			//
			APIReposSideList _APIReposSideList = new APIReposSideList();
			_APIReposSideList.NumSide = 1;
			_APIReposSideList.Symbol = "XDCR12101";
			_APIReposSideList.OrderQty = 1000;
			_APIReposSideList.MergePrice = 1;
			_APIReposSideList.HedgeRate = 1;
			var _SymbolFirmInfo = new List<APIReposSideList>();
			_SymbolFirmInfo.Add(_APIReposSideList);
			request.SymbolFirmInfo = _SymbolFirmInfo;
			//
			var response = new API21OrderNewReposCommonPutthroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API21OrderNewReposCommonPutthough_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow(ORDER_SIDE.SIDE_BUY, "003")] // request.MemberCounterFirm == ConfigData.FirmID
		[DataRow(ORDER_SIDE.SIDE_SELL, "003")] // request.MemberCounterFirm == ConfigData.FirmID
		[DataRow(ORDER_SIDE.SIDE_BUY, "004")] // request.MemberCounterFirm != ConfigData.FirmID
		public async Task TestAPI22_SUCCESS(string p_Side, string p_FirmID)
		{
			var request = new API22OrderConfirmReposCommonPutthroughRequest()
			{
				OrderNo = "NO_20",
				RefExchangeID = "XDCR12101",
				QuoteType = QuoteType_BCGDRepos.LENH_XAC_NHAN_BCGD_REPOS,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Side = p_Side,
				ClientID = "ClientID_API22",
				MemberCounterFirm = p_FirmID,
				EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
				SettleMethod = ORDER_SETTLMETHOD.PAYMENT_NOW,
				SettleDate1 = DateTime.Now.ToString("yyyyMMdd"),
				SettleDate2 = DateTime.Now.ToString("yyyyMMdd"),
				EndDate = DateTime.Now.ToString("yyyyMMdd"),
				RepurchaseTerm = 1,
				RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST API 22"
			};

			//
			APIReposSideList _APIReposSideList = new APIReposSideList();
			_APIReposSideList.NumSide = 1;
			_APIReposSideList.Symbol = "XDCR12101";
			_APIReposSideList.OrderQty = 1000;
			_APIReposSideList.MergePrice = 1;
			_APIReposSideList.HedgeRate = 1;
			var _SymbolFirmInfo = new List<APIReposSideList>();
			_SymbolFirmInfo.Add(_APIReposSideList);
			request.SymbolFirmInfo = _SymbolFirmInfo;
			//
			var response = new API22OrderConfirmReposCommonPutthroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API22OrderConfirmReposCommonPutthrough_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow(ORDER_SIDE.SIDE_BUY)]
		[DataRow(ORDER_SIDE.SIDE_SELL)]
		public async Task TestAPI23_SUCCESS(string p_Side)
		{
			var request = new API23OrderReplaceReposCommonPutthroughRequest()
			{
				OrderNo = "NO_21_1",
				RefExchangeID = "RefExchangeID_23",
				QuoteType = QuoteType_BCGDRepos.LENH_SUA_BCGD_REPOS,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Side = p_Side,
				ClientID = "ClientID_API23",
				MemberCounterFirm = "MemberCounterFirm_API23",
				EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
				SettleMethod = ORDER_SETTLMETHOD.PAYMENT_NOW,
				SettleDate1 = DateTime.Now.ToString("yyyyMMdd"),
				SettleDate2 = DateTime.Now.ToString("yyyyMMdd"),
				EndDate = DateTime.Now.ToString("yyyyMMdd"),
				RepurchaseTerm = 1,
				RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST API 23"
			};

			//
			APIReposSideList _APIReposSideList = new APIReposSideList();
			_APIReposSideList.NumSide = 1;
			_APIReposSideList.Symbol = "XDCR12101";
			_APIReposSideList.OrderQty = 1000;
			_APIReposSideList.MergePrice = 1;
			_APIReposSideList.HedgeRate = 1;
			//
			var _SymbolFirmInfo = new List<APIReposSideList>();
			_SymbolFirmInfo.Add(_APIReposSideList);
			request.SymbolFirmInfo = _SymbolFirmInfo;

			var response = new API23OrderReplaceReposCommonPutthroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API23OrderReplaceReposCommonPutthrough_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow(ORDER_SIDE.SIDE_BUY)]
		[DataRow(ORDER_SIDE.SIDE_SELL)]
		public async Task TestAPI24_SUCCESS(string p_Side)
		{
			var request = new API24OrderCancelReposCommonPutthroughRequest()
			{
				OrderNo = "NO_24",
				RefExchangeID = "RefExchangeID_24",
				QuoteType = QuoteTypeRepos.LENH_DAT,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Side = p_Side
			};

			var response = new API24OrderCancelReposCommonPutthroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API24OrderCancelReposCommonPutthrough_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow(ORDER_SIDE.SIDE_BUY, "003")] // request.MemberCounterFirm == ConfigData.FirmID
		[DataRow(ORDER_SIDE.SIDE_SELL, "003")] // request.MemberCounterFirm == ConfigData.FirmID
		[DataRow(ORDER_SIDE.SIDE_BUY, "004")] // request.MemberCounterFirm != ConfigData.FirmID
		public async Task TestAPI25_SUCCESS(string p_Side, string p_FirmID)
		{
			//Arrange
			var request = new API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "NO_25",
				RefExchangeID = "RefExchangeID_25",
				QuoteType = QuoteType_BCGDRepos.LENH_SUA_THOATHUAN_DA_THUC_HIEN_REPOS_TRONG_NGAY,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Side = p_Side,
				ClientID = "ClientID_API25",
				MemberCounterFirm = p_FirmID,
				EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
				SettleMethod = ORDER_SETTLMETHOD.PAYMENT_LASTDATE,
				SettleDate1 = DateTime.Now.ToString("yyyyMMdd"),
				SettleDate2 = DateTime.Now.ToString("yyyyMMdd"),
				EndDate = DateTime.Now.ToString("yyyyMMdd"),
				RepurchaseTerm = 1,
				RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST API 25"
			};

			//
			APIReposSideList _APIReposSideList = new APIReposSideList();
			_APIReposSideList.NumSide = 1;
			_APIReposSideList.Symbol = "XDCR12101";
			_APIReposSideList.OrderQty = 1000;
			_APIReposSideList.MergePrice = 1;
			_APIReposSideList.HedgeRate = 1;
			//
			var _SymbolFirmInfo = new List<APIReposSideList>();
			_SymbolFirmInfo.Add(_APIReposSideList);
			request.SymbolFirmInfo = _SymbolFirmInfo;

			var response = new API25OrderReplaceDeal1stTransactionReposCommonPutthroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API25OrderReplaceDeal1stTransactionReposCommonPutthrough_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow(ORDER_SIDE.SIDE_BUY, "003")] // request.MemberCounterFirm == ConfigData.FirmID
		[DataRow(ORDER_SIDE.SIDE_SELL, "003")] // request.MemberCounterFirm == ConfigData.FirmID
		[DataRow(ORDER_SIDE.SIDE_BUY, "004")] // request.MemberCounterFirm != ConfigData.FirmID
		public async Task TestAPI26_SUCCESS(string p_Side, string p_FirmID)
		{
			//Arrange
			var request = new API26OrderReplaceDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "NO_26",
				RefExchangeID = "RefExchangeID_26",
				QuoteType = QuoteType_BCGDRepos.LENH_XAC_NHAN_BCGD_REPOS,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Side = p_Side,
				ClientID = "ClientID_API26",
				MemberCounterFirm = p_FirmID,
				EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
				SettleMethod = ORDER_SETTLMETHOD.PAYMENT_LASTDATE,
				SettleDate1 = DateTime.Now.ToString("yyyyMMdd"),
				SettleDate2 = DateTime.Now.ToString("yyyyMMdd"),
				EndDate = DateTime.Now.ToString("yyyyMMdd"),
				RepurchaseTerm = 1,
				RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST API 26"
			};
			//
			APIReposSideList _APIReposSideList = new APIReposSideList();
			_APIReposSideList.NumSide = 1;
			_APIReposSideList.Symbol = "XDCR12101";
			_APIReposSideList.OrderQty = 1000;
			_APIReposSideList.MergePrice = 1;
			_APIReposSideList.HedgeRate = 1;
			//
			var _SymbolFirmInfo = new List<APIReposSideList>();
			_SymbolFirmInfo.Add(_APIReposSideList);
			request.SymbolFirmInfo = _SymbolFirmInfo;
			//
			var response = new API26OrderReplaceDeal1stTransactionReposCommonPutthroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API26OrderReplaceDeal1stTransactionReposCommonPutthrough_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow(ORDER_SIDE.SIDE_BUY)]
		[DataRow(ORDER_SIDE.SIDE_SELL)]
		public async Task TestAPI27_SUCCESS(string p_Side)
		{
			var request = new API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "N0_27",
                RefExchangeID = "RefExchangeID_27",
				QuoteType = QuoteTypeRepos.LENH_SUA,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Side = p_Side
			};
			//

			var response = new API27OrderCancelDeal1stTransactionReposCommonPutthroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API27OrderCancelDeal1stTransactionReposCommonPutthrough_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow(ORDER_SIDE.SIDE_BUY)]
		[DataRow(ORDER_SIDE.SIDE_SELL)]
		public async Task TestAPI28_SUCCESS(string p_Side)
		{
			var request = new API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "NO_28",
				RefExchangeID = "XDCR12101",
				QuoteType = QuoteTypeRepos.LENH_HUY,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Side = p_Side
			};

			var response = new API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API28OrderConfirmCancelDeal1stTransactionReposCommonPutthrough_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow(ORDER_SIDE.SIDE_BUY)]
		[DataRow(ORDER_SIDE.SIDE_SELL)]
		public async Task TestAPI29_SUCCESS(string p_Side)
		{
			var request = new API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "NO_29",
				RefExchangeID = "XDCR12101",
				QuoteType = QuoteType_BCGDRepos.LENH_SUA_THOATHUAN_DA_THUC_HIEN_REPOS,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Side = p_Side,
				ClientID = "ClientID_API29",
				MemberCounterFirm = "MemberCounterFirm_API26",
				EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
				SettleMethod = ORDER_SETTLMETHOD.PAYMENT_LASTDATE,
				SettleDate1 = DateTime.Now.ToString("yyyyMMdd"),
				SettleDate2 = DateTime.Now.ToString("yyyyMMdd"),
				EndDate = DateTime.Now.ToString("yyyyMMdd"),
				RepurchaseTerm = 1,
				RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST API 29"
			};

			//
			APIReposSideList _APIReposSideList = new APIReposSideList();
			_APIReposSideList.NumSide = 1;
			_APIReposSideList.Symbol = "XDCR12101";
			_APIReposSideList.OrderQty = 1000;
			_APIReposSideList.MergePrice = 1;
			_APIReposSideList.HedgeRate = 1;
			//
			var _SymbolFirmInfo = new List<APIReposSideList>();
			_SymbolFirmInfo.Add(_APIReposSideList);
			request.SymbolFirmInfo = _SymbolFirmInfo;
			//

			var response = new API29OrderReplaceDeal2ndTransactionReposCommonPutthroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API29OrderReplaceDeal2ndTransactionReposCommonPutthrough_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow(ORDER_SIDE.SIDE_BUY)]
		[DataRow(ORDER_SIDE.SIDE_SELL)]
		public async Task TestAPI30_SUCCESS(string p_Side)
		{
			//Arrange
			var request = new API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "NO_30",
				RefExchangeID = "XDCR12101",
				QuoteType = QuoteType_BCGDRepos.CHAP_NHAN_SUA_THOATHUAN_DA_THUC_HIEN_REPOS,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Side = p_Side,
				ClientID = "ClientID_API30",
				MemberCounterFirm = "MemberCounterFirm_API30",
				EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
				SettleMethod = ORDER_SETTLMETHOD.PAYMENT_LASTDATE,
				SettleDate1 = DateTime.Now.ToString("yyyyMMdd"),
				SettleDate2 = DateTime.Now.ToString("yyyyMMdd"),
				EndDate = DateTime.Now.ToString("yyyyMMdd"),
				RepurchaseTerm = 1,
				RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST API 30"
			};

			//
			APIReposSideList _APIReposSideList = new APIReposSideList();
			_APIReposSideList.NumSide = 1;
			_APIReposSideList.Symbol = "XDCR12101";
			_APIReposSideList.OrderQty = 1000;
			_APIReposSideList.MergePrice = 1;
			_APIReposSideList.HedgeRate = 1;
			//
			var _SymbolFirmInfo = new List<APIReposSideList>();
			_SymbolFirmInfo.Add(_APIReposSideList);
			request.SymbolFirmInfo = _SymbolFirmInfo;
			//

			var response = new API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthrough_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		[DataRow("NO_31_1", ORDER_SIDE.SIDE_BUY)]
		[DataRow("NO_31_2", ORDER_SIDE.SIDE_SELL)]
		public async Task TestAPI31_SUCCESS(string p_OrderNo, string p_Side)
		{
			var request = new API31OrderNewAutomaticOrderMatchingRequest()
			{
				OrderNo = p_OrderNo,
				ClientID = "ClientID_31",
				Symbol = "XDCR12101",
				Side = p_Side,
				OrderType = NORMAL_ORDERTYPE.LO,
				OrderQty = 1000,
				Price = 100000,
				SpecialType = 1,
				Text = "TEST API 31"
			};
			//

			var response = new API31OrderNewAutomaticOrderMatchingResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API31OrderNewAutomaticOrderMatching_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		public async Task TestAPI32_SUCCESS()
		{
			var request = new API32OrderReplaceAutomaticOrderMatchingRequest()
			{
				OrderNo = "NO_32",
				RefExchangeID = "RefExchangeID_32",
				ClientID = "ClientID_32",
				Symbol = "XDCR12101",
				OrderQty = 2000,
				OrgOrderQty = 1000,
				Price = 100000,
				Text = "TEST API 32"
			};
			//
			var response = new API32OrderReplaceAutomaticOrderMatchingResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API32OrderReplaceAutomaticOrderMatching_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}

		[TestMethod]
		public async Task TestAPI33_SUCCESS()
		{
			var request = new API33OrderCancelAutomaticOrderMatchingRequest()
			{
				OrderNo = "NO_33",
				RefExchangeID = "RefExchangeID_33",
				Symbol = "XDCR12101"
			};
			//

			var response = new API33OrderCancelAutomaticOrderMatchingResponse();
			var mockLogger = new Mock<ILogger>();
			var mockIHNXEntity = new Mock<iHNXClient>();
			var mockIProcessResponse = new Mock<IResponseInterface>();
			var _ProcessRevBussiness = new ProcessRevBusiness(mockIHNXEntity.Object, mockIProcessResponse.Object);
			//Act
			var result = await _ProcessRevBussiness.API33OrderCancelAutomaticOrderMatching_BU(request, response);
			mockIProcessResponse.Verify(helper => helper.ResponseApi2Kafka(It.IsAny<FIXMessageBase>(), It.IsAny<int>()), Times.Once);
			Assert.AreEqual(result.ReturnCode, ORDER_RETURNCODE.SUCCESS);
		}
	}
}