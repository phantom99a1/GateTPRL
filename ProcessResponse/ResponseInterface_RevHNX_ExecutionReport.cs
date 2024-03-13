using CommonLib;
using HNX.FIXMessage;
using KafkaInterface;
using LocalMemory;
using static CommonLib.CommonData;
using static CommonLib.CommonDataInCore;
using static HNX.FIXMessage.MessageExecOrderRepos;

namespace BusinessProcessResponse
{
	public partial class ResponseInterface : IResponseInterface
	{
		// Xử lý khi nhận được 35=8, 150=3 | Khớp
		public void HNXResponseExcQuote(MessageER_ExecOrder p_Message)
		{
			try
			{
				Logger.ResponseLog.Info($"HNXResponseExcQuote -> start process when received exchange 35=8|150=3|39={p_Message.OrdStatus}|40={p_Message.OrdType}; with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, SecondaryClOrdID(526)={p_Message.SecondaryClOrdID},  SendingTime(52)={HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime)}, Text(58)={p_Message.Text}, OrderID(37)={p_Message.OrderID},  OrigClOrdID(41)={p_Message.OrigClOrdID}, Symbol(55)={p_Message.Symbol}, Side(54)={p_Message.Side}, LastQty(32)={p_Message.LastQty}, LastPx(31)={p_Message.LastPx}, SettleValue(6464)={p_Message.SettlValue}, ExecID(17)={p_Message.ExecID}, TransactTime(60)={p_Message.TransactTime},ReciprocalMember(448)={p_Message.ReciprocalMember}");

				// Map từ  message Map từ  message 35=8, 150=3, 39=2, 40 = D  => Xử lý cho lệnh khớp thông thường
				string p_OrderType = "";
				if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
				{
					OrderInfo objOrderInfo_Buy = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.SecondaryClOrdID, p_Side: ORDER_SIDE.SIDE_BUY);
					if (objOrderInfo_Buy != null)
					{
						p_OrderType = objOrderInfo_Buy.RefMsgType;
					}

					OrderInfo objOrderInfo_Sell = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.OrigClOrdID, p_Side: ORDER_SIDE.SIDE_SELL);
					if (string.IsNullOrEmpty(p_OrderType) && objOrderInfo_Sell != null)
					{
						p_OrderType = objOrderInfo_Sell.RefMsgType;
					}
				}
				else if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
				{
					OrderInfo objOrderInfo_Buy = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.SecondaryClOrdID, p_Side: ORDER_SIDE.SIDE_BUY);
					if (objOrderInfo_Buy != null)
					{
						p_OrderType = objOrderInfo_Buy.RefMsgType;
					}
				}

				if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_2 && p_OrderType == CORE_RefMsgType.RefMsgType_D) // Xử lý cho lệnh thông thường
				{
					/*
                     * "*** Nếu thành viên nhận được lệnh khớp có 54=2
                            Lấy lần lượt giá trị tag 41, 526 so với exchangeID được lưu mem,
                            + Nếu (tag 526 = giá trị exchangeID trong mem và side của lệnh trong mem =B) --> lấy orderNo của lệnh tìm được trả ra NE
                            + Nếu (tag 41 = giá trị exchangeID trong mem và side của lệnh trong mem =S) --> lấy orderNo của lệnh tìm được trả ra NE
                            ***Nếu thành viên nhận được lệnh khớp có 54=1
                            Lấy giá trị tag 526 so với exchangeID được lưu mem để tìm ra lệnh --> lấy orderNo của lệnh tìm được"
                     */
					if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
					{
						bool checkSendKafka = false;
						//
						string p_OrderNo = "";
						OrderInfo objOrderSideBuy = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.SecondaryClOrdID, p_Side: ORDER_SIDE.SIDE_BUY);
						if (objOrderSideBuy != null)
						{
							p_OrderNo = objOrderSideBuy.OrderNo;
							//
							NormalOrderExecutionObjectModel _Response = new NormalOrderExecutionObjectModel();
							_Response.MsgType = CORE_MsgType.MsgNE;
							_Response.OrderNo = p_OrderNo;
							_Response.OrderID = p_Message.OrderID;
							_Response.BuyOrderID = p_Message.SecondaryClOrdID;
							_Response.SellOrderID = p_Message.OrigClOrdID;
							_Response.Symbol = p_Message.Symbol;
							_Response.Side = ORDER_SIDE.SIDE_BUY;
							//
							_Response.LastQty = p_Message.LastQty;
							_Response.LastPx = p_Message.LastPx;
							_Response.SettleValue = p_Message.SettlValue;
							_Response.ExecID = p_Message.ExecID;
							_Response.MemberCounterFirm = p_Message.ReciprocalMember;
							_Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
							_Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
							//
							c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);
							checkSendKafka = true;
							//
							Logger.ResponseLog.Info($"HNXResponse_EROrderCancel -> process when received exchange 35=8|150=5|39={p_Message.OrdStatus}|40={p_Message.OrdType} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, OrigClOrdID(41)={p_Message.OrigClOrdID}, SendingTime={HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime)}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution}, MsgType={_Response.MsgType}");
						}

						//
						// +Nếu(tag 41 = giá trị exchangeID trong mem và side của lệnh trong mem = S)-- > lấy orderNo của lệnh tìm được trả ra NE
						string p_OrderNoSideS = "";
						OrderInfo objOrderSideSell = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.OrigClOrdID, p_Side: ORDER_SIDE.SIDE_SELL);
						if (objOrderSideSell != null)
						{
							p_OrderNoSideS = objOrderSideSell.OrderNo;
							//
							NormalOrderExecutionObjectModel _ResponseSideS = new NormalOrderExecutionObjectModel();
							_ResponseSideS.MsgType = CORE_MsgType.MsgNE;
							_ResponseSideS.OrderNo = p_OrderNoSideS;
							_ResponseSideS.OrderID = p_Message.OrderID;
							_ResponseSideS.BuyOrderID = p_Message.SecondaryClOrdID;
							_ResponseSideS.SellOrderID = p_Message.OrigClOrdID;
							_ResponseSideS.Symbol = p_Message.Symbol;
							_ResponseSideS.Side = ORDER_SIDE.SIDE_SELL;
							//
							_ResponseSideS.LastQty = p_Message.LastQty;
							_ResponseSideS.LastPx = p_Message.LastPx;
							_ResponseSideS.SettleValue = p_Message.SettlValue;
							_ResponseSideS.ExecID = p_Message.ExecID;
							_ResponseSideS.MemberCounterFirm = p_Message.ReciprocalMember;
							_ResponseSideS.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
							_ResponseSideS.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
							//
							c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution, _ResponseSideS, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);
							checkSendKafka = true;
							//
							Logger.ResponseLog.Info($"HNXResponse_EROrderCancel -> process when received exchange 35=8|150=5|39={p_Message.OrdStatus}|40={p_Message.OrdType} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, OrigClOrdID(41)={p_Message.OrigClOrdID}, SendingTime={HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime)}; sended queue kafka -> topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution}, MsgType={_ResponseSideS.MsgType}");
						}
						//
						if(!checkSendKafka)
						{
							Logger.ResponseLog.Warn($"HNXResponse_EROrderCancel -> process when received exchange 35=8|150=5|39={p_Message.OrdStatus}|40={p_Message.OrdType} with MsgSeqNum(34)={p_Message.MsgSeqNum}, SecondaryClOrdID(526)={p_Message.SecondaryClOrdID}, OrigClOrdID(41)={p_Message.OrigClOrdID} ==> Can't find memory data to send Kafka ");
						}
					}
					if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
					{
						string p_OrderNo = "";
						string p_Side = "";
						OrderInfo objOrder = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.SecondaryClOrdID);
						if (objOrder != null)
						{
							p_OrderNo = objOrder.OrderNo;
							p_Side = objOrder.Side;
						}
						NormalOrderExecutionObjectModel _Response = new NormalOrderExecutionObjectModel();
						_Response.MsgType = CORE_MsgType.MsgNE;
						_Response.OrderNo = p_OrderNo;
						_Response.OrderID = p_Message.OrderID;
						_Response.BuyOrderID = p_Message.SecondaryClOrdID;
						_Response.SellOrderID = p_Message.OrigClOrdID;
						_Response.Symbol = p_Message.Symbol;
						_Response.Side = ORDER_SIDE.SIDE_BUY;
						//
						_Response.LastQty = p_Message.LastQty;
						_Response.LastPx = p_Message.LastPx;
						_Response.SettleValue = p_Message.SettlValue;
						_Response.ExecID = p_Message.ExecID;
						_Response.MemberCounterFirm = p_Message.ReciprocalMember;
						_Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
						_Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
						//
						c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);
						//
						Logger.ResponseLog.Info($"HNXResponse_EROrderCancel -> process when received exchange 35=8|150=5|39={p_Message.OrdStatus}|40={p_Message.OrdType} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, OrigClOrdID(41)={p_Message.OrigClOrdID}, SendingTime={HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime)}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution}, MsgType={_Response.MsgType}");
					}
				}
				else // Xử lý các lệnh khớp khác
				{
					string _OrderNo = "";
					// ***Nếu thành viên nhận được lệnh khớp có 54=1
					if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
					{
						Logger.ResponseLog.Info($"HNXResponseExcQuote -> process when received exchange 35=8|150=3|54={p_Message.Side}, MsgSeqNum(34)={p_Message.MsgSeqNum}, SecondaryClOrdID(526)={p_Message.SecondaryClOrdID}");
						//Lấy giá trị tag 526 so với exchangeID được lưu mem để tìm ra lệnh --> lấy orderNo của lệnh tìm được
						OrderInfo objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.SecondaryClOrdID);
						if (objOrder != null)
						{
							_OrderNo = objOrder.OrderNo;
							Logger.ResponseLog.Info($"HNXResponseExcQuote -> process when received exchange 35=8|150=3|54={p_Message.Side}, MsgSeqNum(34)={p_Message.MsgSeqNum}, SecondaryClOrdID(526)={p_Message.SecondaryClOrdID} find orderNo={_OrderNo}");
						}
						else
						{
							Logger.ResponseLog.Warn($"HNXResponseExcQuote -> process when received exchange 35=8|150=3|54={p_Message.Side}, MsgSeqNum(34)={p_Message.MsgSeqNum}, SecondaryClOrdID(526)={p_Message.SecondaryClOrdID}; Can't find GetOrder_byExchangeID({p_Message.SecondaryClOrdID}) on memory!");
						}
						//
						ResponseOrderFilledToKafka _response = new ResponseOrderFilledToKafka();
						_response.MsgType = CORE_MsgType.MsgOE;
						_response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
						_response.Text = p_Message.Text;
						_response.OrderID = p_Message.OrderID;
						_response.OrderNo = _OrderNo;
						_response.OrigClOrdID = p_Message.OrigClOrdID;
						_response.SecondaryClOrdID = p_Message.SecondaryClOrdID;
						_response.Symbol = p_Message.Symbol;
						_response.Side = ORDER_SIDE.SIDE_BUY;
						_response.LastQty = p_Message.LastQty;
						_response.LastPx = p_Message.LastPx;
						_response.SettleValue = p_Message.SettlValue;
						_response.ExecID = p_Message.ExecID;
						_response.MemberCounterFirm = p_Message.ReciprocalMember != null ? p_Message.ReciprocalMember : string.Empty;
						//
						c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution, _response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);
						//
						Logger.ResponseLog.Info($"HNXResponseExcQuote -> end process message when received exchange 35=8|150=3|54={p_Message.Side}, MsgSeqNum(34)={p_Message.MsgSeqNum}, SecondaryClOrdID(526)={p_Message.SecondaryClOrdID}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution}, MsgType={_response.MsgType}");
					}
					// ***Nếu thành viên nhận được lệnh khớp có 54=2
					else if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
					{
						Logger.ResponseLog.Info($"HNXResponseExcQuote -> process when received exchange 35=8|150=3|54={p_Message.Side}, MsgSeqNum(34)={p_Message.MsgSeqNum}, SecondaryClOrdID(526)={p_Message.SecondaryClOrdID}");
						//

						// p_Message.SecondaryClOrdID = 526
						// p_Message.OrigClOrdID = 41

						/*
                         *  + TH1: Lấy lần lượt giá trị tag 41, 526 so với mem, nếu trong memory lệnh vào chưa có exchangeID -> tìm lệnh đầu tiên đặt vào trùng mã ck, giá, khối lượng -> orderNo = orderNo của lệnh tìm được

                            + TH2:  Lấy lần lượt giá trị tag 41, 526 so với exchangeID được lưu mem:
                            Nếu (tag 526 = giá trị exchangeID trong mem và side của lệnh trong mem =B) --> lấy orderNo của lệnh tìm được trả ra OE
                            Nếu (tag 41 = giá trị exchangeID trong mem và side của lệnh trong mem =S) --> lấy orderNo của lệnh tìm được trả ra OE
                         */

						// Nếu (tag 526 = giá trị exchangeID trong mem và side của lệnh trong mem =B) --> lấy orderNo của lệnh tìm được trả ra OE
						//OrderInfo objOrder_Tag526 = OrderMemory.GetOrder_byExchangeID_Side(p_Message.SecondaryClOrdID, ORDER_SIDE.SIDE_BUY);
						OrderInfo objOrder_Tag526 = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.SecondaryClOrdID, p_Side: ORDER_SIDE.SIDE_BUY);
						if (objOrder_Tag526 != null)
						{
							Logger.ResponseLog.Info($"HNXResponseExcQuote -> process when received exchange 35=8|150=3|54={p_Message.Side}, MsgSeqNum(34)={p_Message.MsgSeqNum}, SecondaryClOrdID(526)={p_Message.SecondaryClOrdID}");
							//
							ResponseOrderFilledToKafka _response = new ResponseOrderFilledToKafka();
							_response.MsgType = CORE_MsgType.MsgOE;
							_response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
							_response.Text = p_Message.Text;
							_response.OrderID = p_Message.OrderID;
							_response.OrderNo = objOrder_Tag526.OrderNo;
							_response.OrigClOrdID = p_Message.OrigClOrdID;
							_response.SecondaryClOrdID = p_Message.SecondaryClOrdID;
							_response.Symbol = p_Message.Symbol;
							_response.Side = objOrder_Tag526.Side;
							_response.LastQty = p_Message.LastQty;
							_response.LastPx = p_Message.LastPx;
							_response.SettleValue = p_Message.SettlValue;
							_response.ExecID = p_Message.ExecID;
							_response.MemberCounterFirm = p_Message.ReciprocalMember != null ? p_Message.ReciprocalMember : string.Empty;
							//
							if (objOrder_Tag526.Side == ORDER_SIDE.SIDE_BUY)
								OrderMemory.UpdateOrderInfo(objOrder_Tag526, p_ExchangeID: p_Message.SecondaryClOrdID);
							else
								OrderMemory.UpdateOrderInfo(objOrder_Tag526, p_ExchangeID: p_Message.OrigClOrdID);

							//
							c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution, _response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

							//
							Logger.ResponseLog.Info($"HNXResponseExcQuote -> end process when received exchange 35=8|150=3|54={p_Message.Side}MsgSeqNum(34)={p_Message.MsgSeqNum}, SecondaryClOrdID(526)={p_Message.SecondaryClOrdID}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution}, MsgType={_response.MsgType}");
						}

						// Nếu (tag 41 = giá trị exchangeID trong mem và side của lệnh trong mem =S) --> lấy orderNo của lệnh tìm được trả ra OE
						//OrderInfo objOrder_Tag41 = OrderMemory.GetOrder_byExchangeID_Side(p_Message.OrigClOrdID, ORDER_SIDE.SIDE_SELL);
						OrderInfo objOrder_Tag41 = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.OrigClOrdID, p_Side: ORDER_SIDE.SIDE_SELL);
						if (objOrder_Tag41 != null)
						{
							Logger.ResponseLog.Info($"HNXResponseExcQuote -> process when received exchange 35=8|150=3|54={p_Message.Side}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrigClOrdID(41)={p_Message.OrigClOrdID}");
							//
							ResponseOrderFilledToKafka _response = new ResponseOrderFilledToKafka();
							_response.MsgType = CORE_MsgType.MsgOE;
							_response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
							_response.Text = p_Message.Text;
							_response.OrderID = p_Message.OrderID;
							_response.OrderNo = objOrder_Tag41.OrderNo;
							_response.OrigClOrdID = p_Message.OrigClOrdID;
							_response.SecondaryClOrdID = p_Message.SecondaryClOrdID;
							_response.Symbol = p_Message.Symbol;
							_response.Side = objOrder_Tag41.Side;
							_response.LastQty = p_Message.LastQty;
							_response.LastPx = p_Message.LastPx;
							_response.SettleValue = p_Message.SettlValue;
							_response.ExecID = p_Message.ExecID;
							_response.MemberCounterFirm = p_Message.ReciprocalMember != null ? p_Message.ReciprocalMember : string.Empty;
							//
							if (objOrder_Tag41.Side == ORDER_SIDE.SIDE_BUY)
								OrderMemory.UpdateOrderInfo(objOrder_Tag41, p_ExchangeID: p_Message.SecondaryClOrdID);
							else
								OrderMemory.UpdateOrderInfo(objOrder_Tag41, p_ExchangeID: p_Message.OrigClOrdID);

							c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution, _response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);
							//
							Logger.ResponseLog.Info($"HNXResponseExcQuote -> process when received exchange 35=8|150=3|54={p_Message.Side}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrigClOrdID(41)={p_Message.OrigClOrdID}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution}, MsgType={_response.MsgType}");
							//
							Logger.ResponseLog.Info($"HNXResponseExcQuote -> end process 35=8|150=3|54={p_Message.Side}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrigClOrdID(41)={p_Message.OrigClOrdID} when received exchange");
						}
						//
						if (objOrder_Tag526 == null && objOrder_Tag41 == null)
						{
							Logger.ResponseLog.Info($"HNXResponseExcQuote start process when received exchange 35=8|150=3|54={p_Message.Side}|SecondaryClOrdID(526)=null|OrigClOrdID(41)=null, MsgSeqNum(34)={p_Message.MsgSeqNum} - start process Symbol={p_Message.Symbol}, LastPx={p_Message.LastPx}, LastQty={p_Message.LastQty}, LastMsgSeqNumProcessed={p_Message.LastMsgSeqNumProcessed}");

							OrderInfo _objOrder = OrderMemory.GetOrderBy(p_RefMsgType: MessageType.NewOrderCross, p_ExchangeID: string.Empty, p_Price: p_Message.LastPx, p_OrderQty: p_Message.LastQty, p_Symbol: p_Message.Symbol, p_ClOrdID: p_Message.ClOrdID);

							//OrderInfo _objOrder = OrderMemory.GetOrder_By_Symbol_Price_Volume_RefMsgType_OrderNo(p_Message.Symbol, p_Message.LastPx, p_Message.LastQty, MessageType.NewOrderCross, p_Message.ClOrdID);
							if (_objOrder != null)
							{
								Logger.ResponseLog.Info($"HNXResponseExcQuote -> process when received exchange 35=8|150=3|54={p_Message.Side}|SecondaryClOrdID(526)=null|OrigClOrdID(41)=null, MsgSeqNum(34)={p_Message.MsgSeqNum}, find order in memory with OrderNo={_objOrder.OrderNo}");
								//

								// Cập nhật exchangeID của lệnh, lấy exchangeID side mua/bán
								// Nếu side của lệnh tìm được là mua --> lấy giá trị tag 526 của msg 8 cập nhật vào exchangeID
								// Nếu side của lệnh tìm đc là bán --> lấy giá trị tag 41 của msg 8 cập nhật
								if (_objOrder.Side == ORDER_SIDE.SIDE_BUY)
								{
									OrderMemory.UpdateOrderInfo(_objOrder, p_ExchangeID: p_Message.SecondaryClOrdID);
								}
								else
								{
									OrderMemory.UpdateOrderInfo(_objOrder, p_ExchangeID: p_Message.OrigClOrdID);
								}
								//
								ResponseOrderFilledToKafka _response = new ResponseOrderFilledToKafka();
								_response.MsgType = CORE_MsgType.MsgOE;
								_response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
								_response.Text = p_Message.Text;
								_response.OrderID = p_Message.OrderID;
								_response.OrderNo = _objOrder.OrderNo;
								_response.OrigClOrdID = p_Message.OrigClOrdID;
								_response.SecondaryClOrdID = p_Message.SecondaryClOrdID;
								_response.Symbol = p_Message.Symbol;
								_response.Side = _objOrder.Side;
								_response.LastQty = p_Message.LastQty;
								_response.LastPx = p_Message.LastPx;
								_response.SettleValue = p_Message.SettlValue;
								_response.ExecID = p_Message.ExecID;
								_response.MemberCounterFirm = p_Message.ReciprocalMember != null ? p_Message.ReciprocalMember : string.Empty;
								//
								c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution, _response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);
								//
								Logger.ResponseLog.Info($"HNXResponseExcQuote -> process when received exchange 35=8|150=3|54={p_Message.Side}|SecondaryClOrdID(526)=null|OrigClOrdID(41)=null, MsgSeqNum(34)={p_Message.MsgSeqNum}; Send kafka success -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution}, MsgType={_response.MsgType}");
							}
						}
					}
				}
				//
				Logger.ResponseLog.Info($"HNXResponseExcQuote -> End process when received exchange 35=8|150=3|54={p_Message.Side}; with MsgSeqNum(34)={p_Message.MsgSeqNum}, SecondaryClOrdID(526)={p_Message.SecondaryClOrdID},  SendingTime={HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime)}, Text={p_Message.Text}, OrderID={p_Message.OrderID},  OrigClOrdID={p_Message.OrigClOrdID}, Symbol={p_Message.Symbol}, LastQty={p_Message.LastQty}, LastPx={p_Message.LastPx}, SettleValue={p_Message.SettlValue}, ExecID={p_Message.ExecID}");
			}
			catch (Exception ex)
			{
				Logger.ResponseLog.Error($"HNXResponseExcQuote ->  Error process when received exchange 35=8|150=3|54={p_Message.Side}; with MsgSeqNum(34)={p_Message.MsgSeqNum}, SecondaryClOrdID(526)={p_Message.SecondaryClOrdID},  SendingTime={HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime)}, Text={p_Message.Text}, OrderID={p_Message.OrderID},  OrigClOrdID={p_Message.OrigClOrdID}, Symbol={p_Message.Symbol}, LastQty={p_Message.LastQty}, LastPx={p_Message.LastPx}, SettleValue={p_Message.SettlValue}, ExecID={p_Message.ExecID}, Exception: {ex?.ToString()}");
			}
		}

		// Xử lý khi nhận được 35=8, 150=4
		public void HNXResponse_EROrderCancel(MessageER_OrderCancel p_Message)
		{
			try
			{
				Logger.ResponseLog.Info($"HNXResponse_EROrderCancel -> start process when received exchange 35=8|150=4|39={p_Message.OrdStatus}; with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrderID(37)={p_Message.OrderID}, OrigClOrdID(41)={p_Message.OrigClOrdID}, ClOrdID(11)={p_Message.ClOrdID}, SendingTime(52)={HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime)}, OrdStatus(39)={p_Message.OrdStatus}, TransactTime(60)={p_Message.TransactTime}, LeavesQty(151)={p_Message.LeavesQty}, Symbol(55)={p_Message.Symbol}, Side(54)={p_Message.Side}, OrdType(40)={p_Message.OrdType}, Price(44)={p_Message.Price}, Account(1)={p_Message.Account}");

				// Map từ  message 35=8, 150=4, 39=3, 40#{U, R, T, S}  => Xử lý cho lệnh thông thường
				if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_3 &&
				  p_Message.OrdType != CORE_OrdType.OrdType_U && p_Message.OrdType != CORE_OrdType.OrdType_R && p_Message.OrdType != CORE_OrdType.OrdType_T && p_Message.OrdType != CORE_OrdType.OrdType_S)
				{
					OrderInfo objOrder = null;
					string _OrderNo = "";
					objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
					if (objOrder != null)
					{
						_OrderNo = objOrder.OrderNo;
					}

					NormalObjectModel _Response = new NormalObjectModel();
					_Response.MsgType = CORE_MsgType.MsgNS;
					_Response.RefMsgType = CORE_RefMsgType.RefMsgType_F;
					_Response.OrderNo = _OrderNo;
					_Response.ExchangeID = p_Message.OrderID;
					_Response.RefExchangeID = p_Message.OrigClOrdID;
					_Response.OrderType = p_Message.OrdType;
					_Response.OrderStatus = CORE_OrderStatus.OrderStatus_AC;

					if (p_Message.Side == CORE_OrderSide.SIDE_BUY.ToString())
						_Response.Side = ORDER_SIDE.SIDE_BUY;
					if (p_Message.Side == CORE_OrderSide.SIDE_SELL.ToString())
						_Response.Side = ORDER_SIDE.SIDE_SELL;
					//
					_Response.Symbol = p_Message.Symbol;
					_Response.OrderQty = 0;
					_Response.OrgOrderQty = 0;
					_Response.LeavesQty = p_Message.LeavesQty;
					_Response.LastQty = 0;
					_Response.Price = p_Message.Price;
					_Response.ClientID = p_Message.Account;
					_Response.SettleValue = 0.0;
					_Response.OrderQtyMM2 = 0;
					_Response.PriceMM2 = 0;
					_Response.SpecialType = 0;
					//
					_Response.RejectReasonCode = "";
					_Response.RejectReason = "";
					_Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
					_Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
					//
					c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);
					//
					Logger.ResponseLog.Info($"HNXResponse_EROrderCancel -> process when received exchange 35=8|150=5|39={p_Message.OrdStatus}|40={p_Message.OrdType} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, OrigClOrdID(41)={p_Message.OrigClOrdID}, SendingTime={HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime)}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
				}
				else // Xử lý các lệnh khác
				{
					//
					OrderInfo objOrder = null;
					string _OrderNo = "";
					//
					ResponseMessageKafka _Response = new ResponseMessageKafka();
					_Response.MsgType = CORE_MsgType.MsgOS;
					_Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
					_Response.OrderPartyID = "";
					// Tag 39=A,3, 10
					if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_A || p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_3 || p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_10)
					{
						// Lấy tag 37 trong message 35=8 map với thông tin exchangeID trong memory để tìm bản ghi
						// TAG_OrderID=37
						objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.OrderID);
						if (objOrder == null)
						{
							// Nếu không thấy --> Lấy tag 41 trong msg 35=8, tìm thông tin refExchangeID trong mem, lấy bản ghi có refExchangeID = giá trị tag 41 và exchangeID chưa có và refMsgType =u
							// TAG_OrigClOrdID=41
							//objOrder = OrderMemory.GetOrderBy(p_RefExchangeID: p_Message.OrigClOrdID, p_RefMsgType: MessageType.CrossOrderCancelRequest, p_ExchangeID: string.Empty);
							objOrder = OrderMemory.GetOrder_ByRefExchangeID_ExChangeIDNull_RefMsgType(p_Message.OrigClOrdID, MessageType.CrossOrderCancelRequest);

							if (objOrder != null)
							{
								//objOrder.ExchangeID = p_Message.OrderID;
								OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
							}
						}
					}
					// Tag 39=5
					else if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_5)
					{
						/*
                         * lấy tag 41 trong message 35=8, 150=4, 39=5 map với thông tin exchangeID trong memory để tìm bản ghi đặt ban đầu.
                            - nếu không thấy-> map trực tiếp từ mesage nhận được, gửi về kafka, không lưu mem
                            Lấy tag 41 trong message 35=8, 150=4, 39=5 map với thông tin exchangeID trong memory để tìm bản ghi lệnh đặt --> lấy ra orderNo
                            ( về cơ bản case này chắc chắn "orderNo":"" vì toàn là nhận về, chưa gửi cái gì lên nên k có số orderNo, dev dev xem xét cần lấy thông tin mem hay không vì nó hủy rồi)
                            - nếu thấy, lấy thông tin trong memory để tra ra. Cập nhật exchangeID vào thông tin memory vừa tìm được
                         */
						objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.OrigClOrdID);
						if (objOrder != null)
						{
							//objOrder.ExchangeID = p_Message.OrderID;
							OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
						}
					}
					// Tag 39=9
					else if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_9)
					{
						// lấy tag 37 trong message 35=8 map với thông tin exchangeID trong memory để tìm bản ghi --> Lấy thông tin orderNo trả ra cho KAFKA Map trực tiếp, k lưu mem
						objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.OrderID);
					}
					// Tag39=11
					else if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_11)
					{
						// Lấy tag 41 trong message 35=8, 39=11 map với thông tin exchangeID trong memory để tìm bản ghi lệnh đặt --> lấy ra orderNo
						objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.OrigClOrdID);
					}
					//
					//string ordType = p_Message.OrdType;
					int crossType = 0;
					if (objOrder != null)
					{
						_OrderNo = objOrder.OrderNo;
						//ordType = objOrder.OrderType;
						crossType = Utils.ParseInt(objOrder.CrossType);
					}
					//
					_Response.OrderNo = _OrderNo;
					_Response.RefExchangeID = p_Message.OrigClOrdID;
					_Response.ExchangeID = p_Message.OrderID;
					//
					if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_A)
					{
						_Response.OrderStatus = CORE_OrderStatus.OrderStatus_EC;
						_Response.RefMsgType = MessageType.CrossOrderCancelRequest;
						_Response.RejectReasonCode = "";
						_Response.RejectReason = "";
					}
					else if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_3)
					{
						_Response.OrderStatus = CORE_OrderStatus.OrderStatus_AC;
						_Response.RefMsgType = MessageType.CrossOrderCancelRequest;
						_Response.RejectReasonCode = "";
						_Response.RejectReason = "";
					}
					else if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_5)
					{
						_Response.OrderStatus = CORE_OrderStatus.OrderStatus_AC;
						_Response.RefMsgType = MessageType.CrossOrderCancelRequest;
						_Response.RejectReasonCode = "";
						_Response.RejectReason = "";
					}
					else if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_9)
					{
						_Response.OrderStatus = CORE_OrderStatus.OrderStatus_EJ;
						_Response.RefMsgType = MessageType.CrossOrderCancelRequest;
						_Response.RejectReasonCode = CORE_RejectReasonCode.Code_50000;
						_Response.RejectReason = CORE_RejectReason.RejectReason_50000;
					}
					else if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_10)
					{
						_Response.OrderStatus = CORE_OrderStatus.OrderStatus_EJ;
						_Response.RefMsgType = MessageType.CrossOrderCancelRequest;
						_Response.RejectReasonCode = CORE_RejectReasonCode.Code_50002;
						_Response.RejectReason = CORE_RejectReason.RejectReason_50002;
					}
					else if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_11)
					{
						_Response.OrderStatus = CORE_OrderStatus.OrderStatus_CL;
						_Response.RefMsgType = MessageType.NewOrderCross;
						_Response.RejectReasonCode = CORE_RejectReasonCode.Code_50005;
						_Response.RejectReason = CORE_RejectReason.RejectReason_50005;
					}
					//
					_Response.OrdType = p_Message.OrdType;
					_Response.CrossType = crossType;
					_Response.ClientID = !string.IsNullOrEmpty(p_Message.Account) ? p_Message.Account : "";
					_Response.ClientIDCounterFirm = "";
					_Response.MemberCounterFirm = "";
					if (p_Message.Side == CORE_OrderSide.SIDE_BUY.ToString())
						_Response.Side = ORDER_SIDE.SIDE_BUY;
					else
						_Response.Side = ORDER_SIDE.SIDE_SELL;
					_Response.OrderQty = 0;
					_Response.Price = p_Message.Price;
					_Response.SettleValue = 0;
					_Response.SettleDate = "";
					_Response.Symbol = p_Message.Symbol;
					_Response.SettleMethod = 0;
					_Response.RegistID = "";
					_Response.EffectiveTime = "";
					_Response.Text = p_Message.Text;
					//
					c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);
					//
					Logger.ResponseLog.Info($"HNXResponse_EROrderCancel -> process when received exchange 35=8|150=4|39={p_Message.OrdStatus}; with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrderID(37)={p_Message.OrderID}, OrigClOrdID(41)={p_Message.OrigClOrdID}, ClOrdID(11)={p_Message.ClOrdID}, SendingTime={HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime)}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
				}

				//
				Logger.ResponseLog.Info($"HNXResponse_EROrderCancel -> End process when received exchange 335=8|150=4|39={p_Message.OrdStatus}; with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrderID(37)={p_Message.OrderID}, OrigClOrdID(41)={p_Message.OrigClOrdID}, ClOrdID(11)={p_Message.ClOrdID}, SendingTime(52)={HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime)}, OrdStatus(39)={p_Message.OrdStatus}, TransactTime(60)={p_Message.TransactTime}, LeavesQty(151)={p_Message.LeavesQty}, Symbol(55)={p_Message.Symbol}, Side(54)={p_Message.Side}, OrdType(40)={p_Message.OrdType}, Price(44)={p_Message.Price}, Account(1)={p_Message.Account}");
				//
			}
			catch (Exception ex)
			{
				Logger.ResponseLog.Error($"HNXResponse_EROrderCancel -> Error process when received exchange 35=8|150=4|39={p_Message.OrdStatus}; with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrderID(37)={p_Message.OrderID}, OrigClOrdID(41)={p_Message.OrigClOrdID}, ClOrdID(11)={p_Message.ClOrdID}, SendingTime(52)={HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime)}, OrdStatus(39)={p_Message.OrdStatus}, TransactTime(60)={p_Message.TransactTime}, LeavesQty(151)={p_Message.LeavesQty}, Symbol(55)={p_Message.Symbol}, Side(54)={p_Message.Side}, OrdType(40)={p_Message.OrdType}, Price(44)={p_Message.Price}, Account(1)={p_Message.Account}, Exception: {ex?.ToString()}");
			}
		}

		// Xử lý khi nhận được 35=8, 150=5
		public void HNXResponse_ExecReplace(MessageER_OrderReplace p_Message)
		{
			try
			{
				Logger.ResponseLog.Info($"HNXResponse_ExecReplace -> start process when received exchange 35=8|150=5|39={p_Message.OrdStatus}|40={p_Message.OrdType} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, OrigClOrdID(41)={p_Message.OrigClOrdID}, OrdStatus(39)={p_Message.OrdStatus}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, LastQty={p_Message.LastQty}, LastPx={p_Message.LastPx}, LeavesQty={p_Message.LeavesQty}, SettlValue={p_Message.SettlValue}");

				// Map từ  message 35=8, 150=5, 39=3, 40#{U, R, T, S} => Xử lý cho lệnh thông thường
				if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_3 &&
				  p_Message.OrdType != CORE_OrdType.OrdType_U && p_Message.OrdType != CORE_OrdType.OrdType_R && p_Message.OrdType != CORE_OrdType.OrdType_T && p_Message.OrdType != CORE_OrdType.OrdType_S)
				{
					OrderInfo objOrder = null;
					string _OrderNo = "";
					objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
					if (objOrder != null)
					{
						_OrderNo = objOrder.OrderNo;
					}

					NormalObjectModel _Response = new NormalObjectModel();
					_Response.MsgType = CORE_MsgType.MsgNS;
					_Response.RefMsgType = CORE_RefMsgType.RefMsgType_G;
					_Response.OrderNo = _OrderNo;
					_Response.ExchangeID = p_Message.OrderID;
					_Response.RefExchangeID = p_Message.OrigClOrdID;
					_Response.OrderType = p_Message.OrdType;
					_Response.OrderStatus = CORE_OrderStatus.OrderStatus_AM;
					if (p_Message.Side == CORE_OrderSide.SIDE_BUY.ToString())
						_Response.Side = ORDER_SIDE.SIDE_BUY;
					if (p_Message.Side == CORE_OrderSide.SIDE_SELL.ToString())
						_Response.Side = ORDER_SIDE.SIDE_SELL;
					//
					_Response.Symbol = p_Message.Symbol;
					_Response.OrderQty = 0;
					_Response.OrgOrderQty = 0;
					_Response.LeavesQty = p_Message.LeavesQty;
					_Response.LastQty = p_Message.LastQty;
					_Response.Price = p_Message.LastPx;
					_Response.ClientID = p_Message.Account;
					_Response.SettleValue = p_Message.SettlValue;
					_Response.OrderQtyMM2 = 0;
					_Response.PriceMM2 = 0;
					_Response.SpecialType = 0;
					//
					_Response.RejectReasonCode = "";
					_Response.RejectReason = "";
					_Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
					_Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
					//
					c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);
					Logger.ResponseLog.Info($"HNXResponse_ExecReplace -> process when received exchange 35=8|150=5|39={p_Message.OrdStatus}|40={p_Message.OrdType} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, OrigClOrdID(41)={p_Message.OrigClOrdID}, SendingTime={HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime)}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
				}
				else // xử lý các lệnh phản hồi khác
				{
					//
					OrderInfo objOrder = null;
					string _OrderNo = "";
					// 39= A,3,4
					if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_A || p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_3 || p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_4)
					{
						/*
                         * *****Với 35=8, 150=5, 39={4, A, 3}
                            Lấy tag 37 trong message 35=8 map với thông tin exchangeID trong memory để tìm bản ghi
                            + Nếu thấy --> Từ thông tin bản ghi đó lấy ra orderNo để trả ra KAFKA
                            + Nếu không thấy --> Lấy tag 41 trong msg 35=8, tìm thông tin refExchangeID trong mem, lấy bản ghi có refExchangeID = giá trị tag 41 và exchangeID chưa có và refMsgType =t. Tìm được bản ghi -->  Lấy ra orderNo để trả ra KAFKA
                         */
						objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.OrderID);
						if (objOrder == null)
						{
							//+ Nếu không thấy --> Lấy tag 41 trong msg 35=8, tìm thông tin refExchangeID trong mem, lấy bản ghi có refExchangeID = giá trị tag 41 và exchangeID chưa có và refMsgType =t. Tìm được bản ghi -->  cập nhật exchangeID và lấy ra orderNo để trả ra KAFKA
							objOrder = OrderMemory.GetOrder_ByRefExchangeID_ExChangeIDNull_RefMsgType(p_Message.OrigClOrdID, MessageType.CrossOrderAdjustRequest);
							//objOrder = OrderMemory.GetOrderBy(p_RefExchangeID: p_Message.OrigClOrdID, p_RefMsgType: MessageType.CrossOrderAdjustRequest, p_ExchangeID: string.Empty);
							if (objOrder != null)
							{
								//objOrder.ExchangeID = p_Message.OrderID;
								OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
							}
						}
					}
					// 39= 9
					else if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_9)
					{
						/*
                         * ******Với 35=8, 150=5, 39=9
                            Lấy giá trị tag 37, so sánh với dữ liệu exchangeID trong danh sách lệnh lưu mem của gate. Tìm được bản ghi, lấy thông tin orderNo trả ra cho KAFKA
                         */
						objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.OrderID);
					}
					//
					//string ordType = p_Message.OrdType;
					int crossType = 0;
					if (objOrder != null)
					{
						_OrderNo = objOrder.OrderNo;
						//ordType = objOrder.OrderType;
						crossType = Utils.ParseInt(objOrder.CrossType);
					}
					//
					ResponseMessageKafka _Response = new ResponseMessageKafka();
					_Response.MsgType = CORE_MsgType.MsgOS;
					_Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
					_Response.OrderPartyID = "";
					_Response.OrderNo = _OrderNo;
					_Response.RefExchangeID = p_Message.OrigClOrdID;
					_Response.ExchangeID = p_Message.OrderID;
					if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_A)
					{
						_Response.OrderStatus = CORE_OrderStatus.OrderStatus_EC;
						_Response.RejectReasonCode = "";
						_Response.RejectReason = "";
					}
					else if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_3)
					{
						_Response.OrderStatus = CORE_OrderStatus.OrderStatus_AM;
						_Response.RejectReasonCode = "";
						_Response.RejectReason = "";
					}
					else if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_9)
					{
						_Response.OrderStatus = CORE_OrderStatus.OrderStatus_EJ;
						_Response.RejectReasonCode = CORE_RejectReasonCode.Code_50000;
						_Response.RejectReason = CORE_RejectReason.RejectReason_50000;
					}
					else if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_4)
					{
						_Response.OrderStatus = CORE_OrderStatus.OrderStatus_EJ;
						_Response.RejectReasonCode = CORE_RejectReasonCode.Code_50001;
						_Response.RejectReason = CORE_RejectReason.RejectReason_50001;
					}
					_Response.RefMsgType = MessageType.CrossOrderAdjustRequest;
					_Response.OrdType = p_Message.OrdType;
					_Response.CrossType = crossType;
					_Response.ClientID = p_Message.Account;
					_Response.ClientIDCounterFirm = "";
					_Response.MemberCounterFirm = "";
					if (p_Message.Side == CORE_OrderSide.SIDE_BUY.ToString())
						_Response.Side = ORDER_SIDE.SIDE_BUY;
					else
						_Response.Side = ORDER_SIDE.SIDE_SELL;
					_Response.OrderQty = p_Message.LastQty;
					_Response.Price = p_Message.LastPx;
					_Response.SettleValue = p_Message.SettlValue;
					_Response.SettleDate = "";
					_Response.Symbol = p_Message.Symbol;
					_Response.SettleMethod = 0;
					_Response.RegistID = "";
					_Response.EffectiveTime = "";
					_Response.Text = p_Message.Text;
					//Gọi hàm gửi kafka
					c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);
					//
					Logger.ResponseLog.Info($"HNXResponse_ExecReplace -> process when received exchange 35=8|150=5|39={p_Message.OrdStatus}|40={p_Message.OrdType} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, OrigClOrdID(41)={p_Message.OrigClOrdID}, SendingTime={HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime)}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
				}

				//
				Logger.ResponseLog.Info($"HNXResponse_ExecReplace -> End process when received exchange 35=8|150=5|39={p_Message.OrdStatus}|40={p_Message.OrdType} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, OrigClOrdID(41)={p_Message.OrigClOrdID}, OrdStatus(39)={p_Message.OrdStatus}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, LastQty={p_Message.LastQty}, LastPx={p_Message.LastPx}, LeavesQty={p_Message.LeavesQty}, SettlValue={p_Message.SettlValue}");
				//
			}
			catch (Exception ex)
			{
				Logger.ResponseLog.Error($"HNXResponse_ExecReplace -> Error process when received exchange 35=8|150=5|39={p_Message.OrdStatus}|40={p_Message.OrdType} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, OrigClOrdID(41)={p_Message.OrigClOrdID}, OrdStatus(39)={p_Message.OrdStatus}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, LastQty={p_Message.LastQty}, LastPx={p_Message.LastPx}, LeavesQty={p_Message.LeavesQty}, SettlValue={p_Message.SettlValue}, Exception: {ex?.ToString()}");
			}
		}

		// Xử lý khi nhận được 35=8, 150=0
		public void HNXResponse_ExecOrder(MessageER_Order p_Message)
		{
			try
			{
				Logger.ResponseLog.Info($"HNXResponse_ExecOrder -> start process when received exchange 35={MessageType.ExecutionReport}|150={ExecutionReportType.ER_Order_0}|(OrdStatus)39={p_Message.OrdStatus}; with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, Symbol(55)={p_Message.Symbol}, Side(54)={p_Message.Side}, OrderQty(38)={p_Message.OrderQty}, OrdType(40)={p_Message.OrdType}, Price(44)={p_Message.Price},  Account(1)={p_Message.Account}, SettlValue(6464)={p_Message.SettlValue}");

				// Map từ  message 35=8, 150=0, 39=A, 40#{U, R, T, S}
				if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_A &&
					p_Message.OrdType != CORE_OrdType.OrdType_U && p_Message.OrdType != CORE_OrdType.OrdType_R && p_Message.OrdType != CORE_OrdType.OrdType_T && p_Message.OrdType != CORE_OrdType.OrdType_S)
				{
					/*
                     * "lấy tag 11 trong message 35=8, 150=0, 39=A map với thông tin clOrdID trong memory để tìm bản ghi.
                        - nếu không thấy-> lý thuyết không thể xảy ra tình huống này. Cần ghi lỗi
                        - nếu thấy, lấy thông tin trong memory để trả ra. Cập nhật exchangeID vào thông tin memory vừa tìm được. Trả thông tin cho kafka"
                     */
					OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
					if (objOrder != null)
					{
						OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
						//objOrder.ExchangeID = p_Message.OrderID;
					}
					else
					{
						Logger.ResponseLog.Warn($"HNXResponse_ExecOrder -> Error can't find order info when received 35={MessageType.ExecutionReport}|150={ExecutionReportType.ER_Order_0}|(OrdStatus)39={p_Message.OrdStatus}; with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, Symbol(55)={p_Message.Symbol} when received exchange");
					}

					NormalObjectModel _Response = new NormalObjectModel();
					_Response.MsgType = CORE_MsgType.MsgNS;
					_Response.RefMsgType = CORE_RefMsgType.RefMsgType_D;
					_Response.OrderNo = p_Message.ClOrdID;
					_Response.ExchangeID = p_Message.OrderID;
					_Response.RefExchangeID = "";
					_Response.OrderType = p_Message.OrdType;
					_Response.OrderStatus = CORE_OrderStatus.OrderStatus_RE;
					if (p_Message.Side == CORE_OrderSide.SIDE_BUY.ToString())
						_Response.Side = ORDER_SIDE.SIDE_BUY;
					if (p_Message.Side == CORE_OrderSide.SIDE_SELL.ToString())
						_Response.Side = ORDER_SIDE.SIDE_SELL;
					//
					_Response.Symbol = p_Message.Symbol;
					_Response.OrderQty = p_Message.OrderQty;
					_Response.OrgOrderQty = 0;
					_Response.LeavesQty = 0;
					_Response.LastQty = 0;
					_Response.Price = p_Message.Price;
					_Response.ClientID = p_Message.Account;
					_Response.SettleValue = p_Message.SettlValue;
					_Response.OrderQtyMM2 = 0;
					_Response.PriceMM2 = 0;
					_Response.SpecialType = 0;
					//
					_Response.RejectReasonCode = "";
					_Response.RejectReason = "";
					_Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
					_Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
					//
					c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);
				}
				// Map từ  message 35=8, 150=0, 39=M, 40#{U, R, T, S}
				if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_M && p_Message.OrdType != CORE_OrdType.OrdType_U && p_Message.OrdType != CORE_OrdType.OrdType_R && p_Message.OrdType != CORE_OrdType.OrdType_T && p_Message.OrdType != CORE_OrdType.OrdType_S)
				{
					//
					NormalObjectModel _Response = new NormalObjectModel();
					_Response.MsgType = CORE_MsgType.MsgNS;
					_Response.RefMsgType = CORE_RefMsgType.RefMsgType_D;
					_Response.OrderNo = p_Message.ClOrdID;
					_Response.ExchangeID = p_Message.OrderID;
					_Response.RefExchangeID = "";
					_Response.OrderType = "2";
					_Response.OrderStatus = CORE_OrderStatus.OrderStatus_RE;
					if (p_Message.Side == CORE_OrderSide.SIDE_BUY.ToString())
						_Response.Side = ORDER_SIDE.SIDE_BUY;
					if (p_Message.Side == CORE_OrderSide.SIDE_SELL.ToString())
						_Response.Side = ORDER_SIDE.SIDE_SELL;
					//
					_Response.Symbol = p_Message.Symbol;
					_Response.OrderQty = p_Message.OrderQty;
					_Response.OrgOrderQty = 0;
					_Response.LeavesQty = 0;
					_Response.LastQty = 0;
					_Response.Price = p_Message.Price;
					_Response.ClientID = p_Message.Account;
					_Response.SettleValue = 0.0;
					_Response.OrderQtyMM2 = 0;
					_Response.PriceMM2 = 0;
					_Response.SpecialType = 0;
					//
					_Response.RejectReasonCode = "";
					_Response.RejectReason = "";
					_Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
					_Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
					//
					c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);
					//
					Logger.ResponseLog.Info($"HNXResponseExcQuote -> End process when received exchange 35={MessageType.ExecutionReport}|150={ExecutionReportType.ER_Order_0}|39={p_Message.OrdStatus}; with MsgSeqNum(34)={p_Message.MsgSeqNum} sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
				}
				//
				Logger.ResponseLog.Info($"HNXResponseExcQuote -> End process when received exchange 35={MessageType.ExecutionReport}|150={ExecutionReportType.ER_Order_0}|39={p_Message.OrdStatus}; with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, Symbol(55)={p_Message.Symbol}, Side(54)={p_Message.Side}, OrderQty(38)={p_Message.OrderQty}, OrdType(40)={p_Message.OrdType}, Price(44)={p_Message.Price},  Account(1)={p_Message.Account}, SettlValue(6464)={p_Message.SettlValue}");
			}
			catch (Exception ex)
			{
				Logger.ResponseLog.Error($"HNXResponseExcQuote ->  Error process when received exchange 35={MessageType.ExecutionReport}|150={ExecutionReportType.ER_Order_0}|39={p_Message.OrdStatus}; with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, Symbol(55)={p_Message.Symbol}, Side(54)={p_Message.Side}, OrderQty(38)={p_Message.OrderQty}, OrdType(40)={p_Message.OrdType}, Price(44)={p_Message.Price},  Account(1)={p_Message.Account}, SettlValue(6464)={p_Message.SettlValue}, Exception: {ex?.ToString()}");
			}
		}

		// Xử lý khi nhận được 35=8, 150=8
		public void HNXResponse_ExecOrderReject(MessageER_OrderReject p_Message)
		{
			try
			{
				Logger.ResponseLog.Info($"HNXResponse_ExecOrderReject -> start process when received exchange 35={MessageType.ExecutionReport}|150={ExecutionReportType.ER_Rejected_8}|OrdStatus(39)={p_Message.OrdStatus}; with MsgSeqNum(34)={p_Message.MsgSeqNum},  ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, Symbol(55)={p_Message.Symbol}, Side(54)={p_Message.Side}, OrderQty(38)={p_Message.OrderQty}, OrdType(40)={p_Message.OrdType}, Price(44)={p_Message.Price},  Account(1)={p_Message.Account}, SettlValue(6464)={p_Message.SettlValue}, UnderlyingLastQty(652)={p_Message.UnderlyingLastQty}, OrdRejReason(103)={p_Message.OrdRejReason}");

				// Map từ  message 35=8, 150=8, 39=8 , 40#{U, R, T, S}
				if (p_Message.OrdStatus == CORE_OrdStatus.OrdStatus_8 && p_Message.OrdType != CORE_OrdType.OrdType_U && p_Message.OrdType != CORE_OrdType.OrdType_R && p_Message.OrdType != CORE_OrdType.OrdType_T && p_Message.OrdType != CORE_OrdType.OrdType_S)
				{
					//
					NormalObjectModel _Response = new NormalObjectModel();
					_Response.MsgType = CORE_MsgType.MsgNS;
					_Response.RefMsgType = CORE_RefMsgType.RefMsgType_D;
					_Response.OrderNo = p_Message.ClOrdID;
					_Response.ExchangeID = p_Message.OrderID;
					_Response.RefExchangeID = "";
					_Response.OrderType = "";
					_Response.OrderStatus = CORE_OrderStatus.OrderStatus_EJ;
					if (p_Message.Side == CORE_OrderSide.SIDE_BUY.ToString())
						_Response.Side = ORDER_SIDE.SIDE_BUY;
					if (p_Message.Side == CORE_OrderSide.SIDE_SELL.ToString())
						_Response.Side = ORDER_SIDE.SIDE_SELL;
					//
					_Response.Symbol = "";
					_Response.OrderQty = 0;
					_Response.OrgOrderQty = 0;
					_Response.LeavesQty = p_Message.UnderlyingLastQty;
					_Response.LastQty = 0;
					_Response.Price = 0;
					_Response.ClientID = "";
					_Response.SettleValue = 0.0;
					_Response.OrderQtyMM2 = 0;
					_Response.PriceMM2 = 0;
					_Response.SpecialType = 0;
					//
					if (p_Message.OrdRejReason == CORE_OrdRejReason.OrdRejReason_1)
					{
						_Response.RejectReasonCode = CORE_RejectReasonCode.Code_50011;
						_Response.RejectReason = CORE_RejectReason.RejectReason_50011;
					}
					if (p_Message.OrdRejReason == CORE_OrdRejReason.OrdRejReason_2)
					{
						_Response.RejectReasonCode = CORE_RejectReasonCode.Code_50012;
						_Response.RejectReason = CORE_RejectReason.RejectReason_50012;
					}
					if (p_Message.OrdRejReason == CORE_OrdRejReason.OrdRejReason_3)
					{
						_Response.RejectReasonCode = CORE_RejectReasonCode.Code_50013;
						_Response.RejectReason = CORE_RejectReason.RejectReason_50013;
					}
					if (p_Message.OrdRejReason == CORE_OrdRejReason.OrdRejReason_4)
					{
						_Response.RejectReasonCode = CORE_RejectReasonCode.Code_50014;
						_Response.RejectReason = CORE_RejectReason.RejectReason_50014;
					}
					if (p_Message.OrdRejReason == CORE_OrdRejReason.OrdRejReason_5)
					{
						_Response.RejectReasonCode = CORE_RejectReasonCode.Code_50015;
						_Response.RejectReason = CORE_RejectReason.RejectReason_50015;
					}
					if (p_Message.OrdRejReason == CORE_OrdRejReason.OrdRejReason_9)
					{
						_Response.RejectReasonCode = CORE_RejectReasonCode.Code_50017;
						_Response.RejectReason = CORE_RejectReason.RejectReason_50017;
					}
					_Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
					_Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
					//
					c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

					Logger.ResponseLog.Info($"HNXResponse_ExecOrderReject -> process send kafka  35={MessageType.ExecutionReport}|150={ExecutionReportType.ER_Rejected_8}|OrdStatus(39)={p_Message.OrdStatus}, MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID} when received exchange; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
				}
				//
				Logger.ResponseLog.Info($"HNXResponse_ExecOrderReject -> End process when received exchange 35={MessageType.ExecutionReport}|150={ExecutionReportType.ER_Rejected_8}|OrdStatus(39)={p_Message.OrdStatus}; with MsgSeqNum(34)={p_Message.MsgSeqNum},  ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, Symbol(55)={p_Message.Symbol}, Side(54)={p_Message.Side}, OrderQty(38)={p_Message.OrderQty}, OrdType(40)={p_Message.OrdType}, Price(44)={p_Message.Price},  Account(1)={p_Message.Account}, SettlValue(6464)={p_Message.SettlValue}, UnderlyingLastQty(652)={p_Message.UnderlyingLastQty}, OrdRejReason(103)={p_Message.OrdRejReason}");
			}
			catch (Exception ex)
			{
				Logger.ResponseLog.Error($"HNXResponse_ExecOrderReject ->  Error process when received exchange 35={MessageType.ExecutionReport}|150={ExecutionReportType.ER_Rejected_8}|OrdStatus(39)={p_Message.OrdStatus}; with MsgSeqNum(34)={p_Message.MsgSeqNum},  ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, Symbol(55)={p_Message.Symbol}, Side(54)={p_Message.Side}, OrderQty(38)={p_Message.OrderQty}, OrdType(40)={p_Message.OrdType}, Price(44)={p_Message.Price},  Account(1)={p_Message.Account}, SettlValue(6464)={p_Message.SettlValue}, UnderlyingLastQty(652)={p_Message.UnderlyingLastQty}, OrdRejReason(103)={p_Message.OrdRejReason}, Exception: {ex?.ToString()}");
			}
		}

		// Xử lý khi nhận được 35=EE từ sở  | Khớp
		public void HNXResponse_ExecOrderRepos(MessageExecOrderRepos p_Message)
		{
			try
			{
				Logger.ResponseLog.Info($"HNXResponse_ExecOrderRepos -> start process when received exchange 35={MessageType.ExecOrderRepos} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, PartyID(448)={p_Message.PartyID},  CoPartyID(449)={p_Message.CoPartyID}, OrderID(37)={p_Message.OrderID}, BuyOrderID(41)={p_Message.BuyOrderID}, SellOrderID(526)={p_Message.SellOrderID},  RepurchaseTerm(226)={p_Message.RepurchaseTerm}, RepurchaseRate(227)={p_Message.RepurchaseRate}, SettDate(64)={p_Message.SettDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(917)={p_Message.EndDate}, MatchReportType(5362)={p_Message.MatchReportType}, NoSide(552)={p_Message.NoSide}");
				//
				// Duyệt gửi ra
				List<ReposSideListExecOrderReposResponse> listSymbolFirmInfo = null;
				List<SymbolFirmObject> listSymbolFirmObject = null;

				if (p_Message.ReposSideList != null && p_Message.ReposSideList.Count > 0)
				{
					listSymbolFirmInfo = new List<ReposSideListExecOrderReposResponse>();
					listSymbolFirmObject = new List<SymbolFirmObject>();
					ReposSideExecOrder itemSite;
					for (int i = 0; i < p_Message.ReposSideList.Count; i++)
					{
						itemSite = p_Message.ReposSideList[i];
						//
						ReposSideListExecOrderReposResponse _ReposSideListResponse = new ReposSideListExecOrderReposResponse();
						_ReposSideListResponse.NumSide = itemSite.NumSide;
						_ReposSideListResponse.Symbol = itemSite.Symbol;
						_ReposSideListResponse.ExecQty = itemSite.ExecQty;
						_ReposSideListResponse.ExecPx = itemSite.ExecPx;
						_ReposSideListResponse.MergePrice = itemSite.Price;
						_ReposSideListResponse.ReposInterest = itemSite.ReposInterest;
						_ReposSideListResponse.HedgeRate = itemSite.HedgeRate;
						_ReposSideListResponse.SettleValue1 = itemSite.SettlValue;
						_ReposSideListResponse.SettleValue2 = itemSite.SettlValue2;
						listSymbolFirmInfo.Add(_ReposSideListResponse);
						//
						SymbolFirmObject symbolFirmObject = new SymbolFirmObject();
						symbolFirmObject.NumSide = itemSite.NumSide;
						symbolFirmObject.Symbol = itemSite.Symbol;
						symbolFirmObject.OrderQty = itemSite.ExecQty;
						symbolFirmObject.HedgeRate = itemSite.HedgeRate;
						symbolFirmObject.MergePrice = itemSite.Price;
						listSymbolFirmObject.Add(symbolFirmObject);
					}
					Logger.ResponseLog.Info($"HNXResponse_ExecOrderRepos -> process when received exchange 35={MessageType.ExecOrderRepos} with MsgSeqNum(34)={p_Message.MsgSeqNum}, PartyID(448)={p_Message.PartyID},  CoPartyID(449)={p_Message.CoPartyID}, OrderID(37)={p_Message.OrderID}, BuyOrderID(41)={p_Message.BuyOrderID}, SellOrderID(526)={p_Message.SellOrderID}, listSymbolFirmObject={System.Text.Json.JsonSerializer.Serialize(listSymbolFirmObject)}");
				}
				else
				{
					Logger.ResponseLog.Debug($"HNXResponse_ExecOrderRepos -> process when received exchange 35={MessageType.ExecOrderRepos} with MsgSeqNum(34)={p_Message.MsgSeqNum} with p_Message.ReposSideList is null or  p_Message.ReposSideList.Count = 0");
				}

				// Lấy thông tin từ memory để gửi ra kafka
				string p_OrderNo = "";
				string p_Side = "";
				OrderInfo objOrder = null;
				/*
                 * "*****Nhận được 35=EE, 5632=1
                 TH1: Nếu tag 448=tag 449= mã thành viên của Gate
                 - Nếu tag 448=003: Lấy giá trị tag 526 so sánh với exchangeID trong mem, lấy bản ghi có exchangeID=tag 526 và side =S --> trả ra orderNo cho KAFKA
                 - Nếu tag 449=003: Lấy giá trị tag 41 so sánh với exchangeID trong mem, lấy bản ghi có exchangeID=tag 41 và side =B --> trả ra orderNo cho KAFKA
                 - Nếu so sánh tg 41 và 526 với trong mem không tìm được bản ghi, so sánh các thông tin của EE gửi về với các thông tin trong mem, tìm lệnh đầu tiên đặt vào trùng các thông tin: memberCounterFirm, repurchaseRate, repurchaseTerm, settleDate1, settleDate2, endDate, noSide, numSide, symbolFirmInfo(numSide, symbol, orderQty, hedgeRate, mergePrice) và có exchangeID rỗng và orderType =T --> cập nhật exchangeID vào bản ghi tìm được và trả ra KAFKA
                 TH2: Nếu tag 448#tag 449
                 - Nếu tag 448=003: Lấy giá trị tag 526 so sánh với exchangeID trong mem, lấy bản ghi có exchangeID=tag 526 và side =S --> trả ra orderNo cho KAFKA
                 - Nếu tag 449=003: Lấy giá trị tag 41 so sánh với exchangeID trong mem, lấy bản ghi có exchangeID=tag 41 và side =B --> trả ra orderNo cho KAFKA
                 *****Nhận được 35=EE, 5632=2 --> ""orderNo"" ="""""
                 */
				if (p_Message.MatchReportType == 1)
				{
					if (p_Message.PartyID == p_Message.CoPartyID)
					{
						bool checkSendKafka = false;
						if (p_Message.PartyID == ConfigData.FirmID)
						{
							objOrder = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.SellOrderID, p_Side: ORDER_SIDE.SIDE_SELL);
							if (objOrder != null)
							{
								p_OrderNo = objOrder.OrderNo;
								p_Side = objOrder.Side;
								//
								ProcessEE_BuilMsgRE(p_Message, p_OrderNo, p_Side, listSymbolFirmInfo);
								checkSendKafka = true;
							}
						}
						if (p_Message.CoPartyID == ConfigData.FirmID)
						{
							objOrder = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.BuyOrderID, p_Side: ORDER_SIDE.SIDE_BUY);
							if (objOrder != null)
							{
								p_OrderNo = objOrder.OrderNo;
								p_Side = objOrder.Side;

								//
								ProcessEE_BuilMsgRE(p_Message, p_OrderNo, p_Side, listSymbolFirmInfo);
								checkSendKafka = true;
							}
						}

						//-- - Nếu so sánh tg 41 và 526 với trong mem không tìm được bản ghi, lấy giá trị tag 11 của msg EE so sánh với clOrdID --> lấy thông tin trả ra KAFKA đồng thời cập nhật exchangeID vào bản ghi tìm được
						if (objOrder == null)
						{
							objOrder = OrderMemory.GetOrderBy(p_ClOrdID: p_Message.ClOrdID);
							if (objOrder != null)
							{
								p_OrderNo = objOrder.OrderNo;
								p_Side = objOrder.Side;
								if (objOrder.Side == ORDER_SIDE.SIDE_BUY)
								{
									OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.BuyOrderID);
									//objOrder.ExchangeID = p_Message.BuyOrderID;
								}
								if (objOrder.Side == ORDER_SIDE.SIDE_SELL)
								{
									OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.SellOrderID);
									//objOrder.ExchangeID = p_Message.SellOrderID;
								}
								//
								ProcessEE_BuilMsgRE(p_Message, p_OrderNo, p_Side, listSymbolFirmInfo);
								checkSendKafka = true;
							}
						}

						if (!checkSendKafka)
						{
							Logger.ResponseLog.Warn($"HNXResponse_ExecOrderRepos -> Error can't send kakfka when received 35={MessageType.ExecOrderRepos} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, PartyID(448)={p_Message.PartyID},  CoPartyID(449)={p_Message.CoPartyID}, OrderID(37)={p_Message.OrderID} when received exchange");
						}
					}
					else if (p_Message.PartyID != p_Message.CoPartyID)
					{
						bool checkSendKafka = false;
						if (p_Message.PartyID == ConfigData.FirmID)
						{
							objOrder = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.SellOrderID, p_Side: ORDER_SIDE.SIDE_SELL);
							if (objOrder != null)
							{
								p_OrderNo = objOrder.OrderNo;
								p_Side = ORDER_SIDE.SIDE_SELL;
								//
								ProcessEE_BuilMsgRE(p_Message, p_OrderNo, p_Side, listSymbolFirmInfo);
								checkSendKafka = true;
							}
						}
						if (p_Message.CoPartyID == ConfigData.FirmID)
						{
							objOrder = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.BuyOrderID, p_Side: ORDER_SIDE.SIDE_BUY);
							if (objOrder != null)
							{
								p_OrderNo = objOrder.OrderNo;
								p_Side = ORDER_SIDE.SIDE_BUY;
								//
								ProcessEE_BuilMsgRE(p_Message, p_OrderNo, p_Side, listSymbolFirmInfo);
								checkSendKafka = true;
							}
						}

						if (!checkSendKafka)
						{
							Logger.ResponseLog.Warn($"HNXResponse_ExecOrderRepos -> Error can't send kakfka when received 35={MessageType.ExecOrderRepos} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, PartyID(448)={p_Message.PartyID},  CoPartyID(449)={p_Message.CoPartyID}, OrderID(37)={p_Message.OrderID} when received exchange");
						}
					}
				}
				else
				{
					ProcessEE_BuilMsgRE(p_Message, p_OrderNo, p_Side, listSymbolFirmInfo);
				}
			}
			catch (Exception ex)
			{
				Logger.ResponseLog.Error($"HNXResponse_ExecOrderRepos -> Error process when received exchange 35={MessageType.ExecOrderRepos} with MsgSeqNum(34)={p_Message.MsgSeqNum}, PartyID(448)={p_Message.PartyID},  CoPartyID(449)={p_Message.CoPartyID}, OrderID(37)={p_Message.OrderID}, BuyOrderID(41)={p_Message.BuyOrderID}, SellOrderID(526)={p_Message.SellOrderID},  RepurchaseTerm(226)={p_Message.RepurchaseTerm}, RepurchaseRate(227)={p_Message.RepurchaseRate}, SettDate(64)={p_Message.SettDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(917)={p_Message.EndDate}, MatchReportType(5362)={p_Message.MatchReportType}, NoSide(552)={p_Message.NoSide},  Exception: {ex?.ToString()}");
			}
		}

		private void ProcessEE_BuilMsgRE(MessageExecOrderRepos p_Message, string p_OrderNo, string p_Side, List<ReposSideListExecOrderReposResponse> listSymbolFirmInfo)
		{
			//
			ExecOrderReposModel _Response = new ExecOrderReposModel();
			_Response.MsgType = CORE_MsgType.MsgRE;//
			_Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
			_Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";

			_Response.OrderNo = p_OrderNo;//
			_Response.OrderID = p_Message.OrderID;//
			_Response.SellOrderID = p_Message.SellOrderID;//
			_Response.BuyOrderID = p_Message.BuyOrderID;//
			_Response.Side = p_Side;//

			_Response.RepurchaseTerm = p_Message.RepurchaseTerm;
			_Response.RepurchaseRate = p_Message.RepurchaseRate;

			_Response.SettleDate1 = p_Message.SettDate;
			_Response.SettleDate2 = p_Message.SettlDate2;
			_Response.EndDate = p_Message.EndDate;
			_Response.MatchReportType = p_Message.MatchReportType;
			_Response.NoSide = p_Message.NoSide;

			//
			/*
             * "- Nếu tag 448=tag 449: memberCounterFirm = giá trị tag 448
                - Nếu tag 448 #449
                + Nếu tag 448# mã thành viên của Gate --> memberCounterFirm lấy = giá trị tag 448
                + Nếu tag 449# mã thành viên của Gate --> memberCounterFirm lấy = giá trị tag 449"
             */
			if (p_Message.PartyID == p_Message.CoPartyID)
			{
				_Response.MemberCounterFirm = p_Message.PartyID;
			}
			else if (p_Message.PartyID != p_Message.CoPartyID)
			{
				if (p_Message.PartyID != ConfigData.FirmID)
				{
					_Response.MemberCounterFirm = p_Message.PartyID;
				}
				if (p_Message.CoPartyID != ConfigData.FirmID)
				{
					_Response.MemberCounterFirm = p_Message.CoPartyID;
				}
			}
			//
			_Response.SymbolFirmInfo = listSymbolFirmInfo;
			//
			// Gọi hàm gửi sang Kafka
			c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);
			//
			Logger.ResponseLog.Info($"HNXResponse_ExecOrderRepos -> start process when received exchange 35={MessageType.ExecOrderRepos} with MsgSeqNum(34)={p_Message.MsgSeqNum}, PartyID(448)={p_Message.PartyID},  CoPartyID(449)={p_Message.CoPartyID}, OrderID(37)={p_Message.OrderID}, BuyOrderID(41)={p_Message.BuyOrderID}, SellOrderID(526)={p_Message.SellOrderID}, SendingTime={HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime)}; send queue kafka  -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution}, MsgType={_Response.MsgType}");

			//
			Logger.ResponseLog.Info($"HNXResponse_ExecOrderRepos -> start process when received exchange 35={MessageType.ExecOrderRepos} with MsgSeqNum(34)={p_Message.MsgSeqNum}, PartyID(448)={p_Message.PartyID},  CoPartyID(449)={p_Message.CoPartyID}, OrderID(37)={p_Message.OrderID}, BuyOrderID(41)={p_Message.BuyOrderID}, SellOrderID(526)={p_Message.SellOrderID}");
		}
	}
}