using BusinessProcessAPIReq.RequestModels;
using BusinessProcessAPIReq.ResponseModels;
using CommonLib;
using HNX.FIXMessage;
using LocalMemory;
using static CommonLib.CommonData;
using static CommonLib.CommonDataInCore;

namespace BusinessProcessAPIReq
{
    public partial class ProcessRevBusiness : IProcessRevBussiness
    {
        /// <summary>
        /// 3.1	API31 – Đặt lệnh giao dịch khớp lệnh
        /// </summary>
        public async Task<API31OrderNewAutomaticOrderMatchingResponse> API31OrderNewAutomaticOrderMatching_BU(API31OrderNewAutomaticOrderMatchingRequest request, API31OrderNewAutomaticOrderMatchingResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API31OrderNewAutomaticOrderMatching_BU with OrderNo: {request.OrderNo} ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";
                bool resultTradingRule = false;
                if (string.IsNullOrEmpty(request.Symbol))
                {
                    resultTradingRule = TradingRuleData.CheckTradingSessionWithSymbolNull(out _responseText, out _responseCode);
                }
                else
                {
                    resultTradingRule = TradingRuleData.CheckTradingRule_Input(request.Symbol, out _responseText, out _responseCode);
                }

                if (!resultTradingRule)
                {
                    _response.ReturnCode = _responseCode;
                    _response.Message = _responseText;
                    _response.InData = null;
                    return _response;
                }

				//
				bool checkOrderExist = OrderMemory.IsExist_OrderNo(request.OrderNo);
				if (checkOrderExist)
				{
					_response.ReturnCode = ErrorCodeDefine.OrderNo_Duplicated;
					_response.Message = ORDER_RETURNMESSAGE.OrderNo_Duplicated;
					_response.InData = null;
					return _response;
				}

				//string _ExOrderNo = Utils.GenOrderNo();
				// 35= D
				MessageNewOrder newNewOrder = new MessageNewOrder() { TimeInit = DateTime.Now.Ticks };
                newNewOrder.APIBussiness = ORDER_API.API_31;
                newNewOrder.ClOrdID = request.OrderNo;
                newNewOrder.Account = request.ClientID;
                newNewOrder.Symbol = request.Symbol;
                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newNewOrder.Side = CORE_OrderSide.SIDE_BUY;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newNewOrder.Side = CORE_OrderSide.SIDE_SELL;
                }
                newNewOrder.OrdType = request.OrderType;
                newNewOrder.OrderQty = request.OrderQty;
                newNewOrder.OrderQty2 = request.OrderQtyMM2;
                newNewOrder.Price = request.PriceMM2;
                newNewOrder.Price2 = request.Price;
                newNewOrder.SpecialType = request.SpecialType;
                newNewOrder.Text = request.Text;

                // Add thông tin order vào memory
                LocalMemory.OrderInfo objOrder = new OrderInfo()
                {
                    RefMsgType = MessageType.NewOrder,
                    OrderNo = request.OrderNo,
                    ClOrdID = newNewOrder.ClOrdID,
                    ExchangeID = "", // ?
                    RefExchangeID = "", // ?
                    SeqNum = 0,  // khi nào sở về mới update
                    Symbol = request.Symbol,
                    Side = request.Side,
                    Price = request.Price,
                    PriceMM2= request.PriceMM2,
                    SpecialType= request.SpecialType,
                    OrderQty = 0,
                    QuoteType = 0,
                    CrossType = "", // ?
                    ClientID = "",
                    ClientIDCounterFirm = "", // ?
                    MemberCounterFirm = "", // ?
                    OrderType = request.OrderType
                };
                newNewOrder.ApiOrderNo = request.OrderNo;
                newNewOrder.OrderNo = request.OrderNo;
                OrderMemory.Add_NewOrder(objOrder);
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newNewOrder);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API31OrderNewAutomaticOrderMatching OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API31OrderNewAutomaticOrderMatching OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API31OrderNewAutomaticOrderMatching_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 3.2	API32 – Sửa lệnh giao dịch khớp lệnh
        /// </summary>
        public async Task<API32OrderReplaceAutomaticOrderMatchingResponse> API32OrderReplaceAutomaticOrderMatching_BU(API32OrderReplaceAutomaticOrderMatchingRequest request, API32OrderReplaceAutomaticOrderMatchingResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API32OrderReplaceAutomaticOrderMatching_BU with OrderNo: {request.OrderNo} ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";
                bool resultTradingRule = false;
                if (string.IsNullOrEmpty(request.Symbol))
                {
                    resultTradingRule = TradingRuleData.CheckTradingSessionWithSymbolNull(out _responseText, out _responseCode);
                }
                else
                {
                    resultTradingRule = TradingRuleData.CheckTradingRule_Input(request.Symbol, out _responseText, out _responseCode);
                }

                if (!resultTradingRule)
                {
                    _response.ReturnCode = _responseCode;
                    _response.Message = _responseText;
                    _response.InData = null;
                    return _response;
                }

				//
				bool checkOrderExist = OrderMemory.IsExist_OrderNo_RefMsgType(request.OrderNo, request.RefExchangeID);
				if (!checkOrderExist)
				{
					_response.ReturnCode = ErrorCodeDefine.OrderNoAPI_IsValid;
					_response.Message = ORDER_RETURNMESSAGE.OrderNoAPI_IsValid;
					_response.InData = null;
					return _response;
				}

				string _ExOrderNo = Utils.GenOrderNo();
                // 35= G
                MessageReplaceOrder newReplaceOrder = new MessageReplaceOrder() { TimeInit = DateTime.Now.Ticks };
                newReplaceOrder.APIBussiness = ORDER_API.API_32;
                newReplaceOrder.ClOrdID = _ExOrderNo;
                newReplaceOrder.OrigClOrdID = request.RefExchangeID;
                newReplaceOrder.Account = request.ClientID;
                newReplaceOrder.Symbol = request.Symbol;
                newReplaceOrder.OrderQty = request.OrderQty;
                newReplaceOrder.OrgOrderQty = request.OrgOrderQty;
                newReplaceOrder.Price2 = request.Price;
                newReplaceOrder.Text = request.Text;

                // Add thông tin order vào memory
                LocalMemory.OrderInfo objOrder = new OrderInfo()
                {
                    RefMsgType = MessageType.ReplaceOrder,
                    OrderNo = request.OrderNo,
                    ClOrdID = newReplaceOrder.ClOrdID,
                    ExchangeID = "", // ?
                    RefExchangeID = request.RefExchangeID, // ?
                    SeqNum = 0,  // khi nào sở về mới update
                    Symbol = request.Symbol,
                    Side = "",
                    Price = request.Price,
                    PriceMM2=0,
                    OrderQty = request.OrderQty,
                    OrderQtyMM2 = 0,
                    SpecialType=0,
                    QuoteType = 0,
                    CrossType = "", // ?
                    ClientID = "",
                    ClientIDCounterFirm = "", // ?
                    MemberCounterFirm = "", // ?
                    OrderType = ""
                };
                newReplaceOrder.ApiOrderNo = request.OrderNo;
                newReplaceOrder.OrderNo = request.OrderNo;
                OrderMemory.Add_NewOrder(objOrder);
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newReplaceOrder);
                //
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API32OrderReplaceAutomaticOrderMatching OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API32OrderReplaceAutomaticOrderMatching OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API32OrderReplaceAutomaticOrderMatching_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }


        /// <summary>
        /// 3.3	API33 – Hủy lệnh giao dịch khớp lệnh
        /// </summary>
        public async Task<API33OrderCancelAutomaticOrderMatchingResponse> API33OrderCancelAutomaticOrderMatching_BU(API33OrderCancelAutomaticOrderMatchingRequest request, API33OrderCancelAutomaticOrderMatchingResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API33OrderCancelAutomaticOrderMatching_BU with OrderNo: {request.OrderNo} ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";
                bool resultTradingRule = false;
                if (string.IsNullOrEmpty(request.Symbol))
                {
                    resultTradingRule = TradingRuleData.CheckTradingSessionWithSymbolNull(out _responseText, out _responseCode);
                }
                else
                {
                    resultTradingRule = TradingRuleData.CheckTradingRule_Input(request.Symbol, out _responseText, out _responseCode);
                }

                if (!resultTradingRule)
                {
                    _response.ReturnCode = _responseCode;
                    _response.Message = _responseText;
                    _response.InData = null;
                    return _response;
                }

				//
				bool checkOrderExist = OrderMemory.IsExist_OrderNo_RefMsgType(request.OrderNo, request.RefExchangeID);
				if (!checkOrderExist)
				{
					_response.ReturnCode = ErrorCodeDefine.OrderNoAPI_IsValid;
					_response.Message = ORDER_RETURNMESSAGE.OrderNoAPI_IsValid;
					_response.InData = null;
					return _response;
				}

				string _ExOrderNo = Utils.GenOrderNo();
                // 35= F
                MessageCancelOrder newCancelOrder = new MessageCancelOrder() { TimeInit = DateTime.Now.Ticks };
                newCancelOrder.APIBussiness = ORDER_API.API_33;
                newCancelOrder.ClOrdID = _ExOrderNo;
                newCancelOrder.OrigClOrdID = request.RefExchangeID;
                newCancelOrder.Symbol = request.Symbol;
                newCancelOrder.Text = request.Text;

                // Add thông tin order vào memory
                LocalMemory.OrderInfo objOrder = new OrderInfo()
                {
                    RefMsgType = MessageType.CancelOrder,
                    OrderNo = request.OrderNo,
                    ClOrdID = newCancelOrder.ClOrdID,
                    ExchangeID = "", // ?
                    RefExchangeID = request.RefExchangeID, // ?
                    SeqNum = 0,  // khi nào sở về mới update
                    Symbol = request.Symbol,
                    Side = "",
                    Price = 0,
                    OrderQty = 0,
                    QuoteType = 0,
                    CrossType = "", // ?
                    ClientID = "",
                    ClientIDCounterFirm = "", // ?
                    MemberCounterFirm = "", // ?
                    OrderType = ""
                };
                newCancelOrder.ApiOrderNo = request.OrderNo;
                newCancelOrder.OrderNo = request.OrderNo;

                OrderMemory.Add_NewOrder(objOrder);
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newCancelOrder);
                //
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API33OrderCancelAutomaticOrderMatching OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API33OrderCancelAutomaticOrderMatching OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API33OrderCancelAutomaticOrderMatching_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }
    }
}