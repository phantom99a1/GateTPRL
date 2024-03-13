/*
 * Project:
 * Author :
 * Summary: Lớp xử lý nghiệp vụ từ API gửi vào và đẩy dữ liệu vào Queue
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 *
 */

using CommonLib;
using HNX.FIXMessage;
using LocalMemory;
using BusinessProcessResponse;
using BusinessProcessAPIReq.RequestModels;
using BusinessProcessAPIReq.ResponseModels;
using static CommonLib.CommonData;
using static CommonLib.CommonDataInCore;
using LogStation;
namespace BusinessProcessAPIReq
{


    public partial class ProcessRevBusiness : IProcessRevBussiness
    {
       

        /// <summary>
        /// 1.1	Api đặt lệnh thỏa thuận điện tử Outright
        /// </summary>
        public async Task<API1NewElectronicPutThroughResponse> API1NewElectronicPutThrough_BU(API1NewElectronicPutThroughRequest request, API1NewElectronicPutThroughResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API1NewElectronicPutThrough_BU with OrderNo: {request.OrderNo} ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";
                bool resultTradingRule = TradingRuleData.CheckTradingRule_Input(request.Symbol, out _responseText, out _responseCode);
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
                // 35=S
                MessageQuote newQuote = new MessageQuote() { TimeInit = DateTime.Now.Ticks };
                newQuote.APIBussiness = ORDER_API.API_1;
                newQuote.ClOrdID = request.OrderNo.ToString();
                newQuote.Account = request.ClientID;
                newQuote.OrdType = CORE_OrderType.TTDTO;
                newQuote.Side = request.Side == ORDER_SIDE.SIDE_BUY ? CORE_OrderSide.SIDE_BUY : CORE_OrderSide.SIDE_SELL;
                newQuote.Symbol = request.Symbol;
                newQuote.Price2 = request.Price;
                newQuote.OrderQty = request.OrderQty;
                newQuote.SettDate = request.SettleDate;
                newQuote.SettlMethod = request.SettleMethod;
                newQuote.RegistID = request.RegistID;
                newQuote.IsVisible = request.IsVisible;
                newQuote.SettlValue = 0;
                newQuote.Text = request.Text;
                newQuote.IDRequest = ID;
                //
                // Add thông tin order vào memory
                LocalMemory.OrderInfo objOrder = new OrderInfo()
                {
                    RefMsgType = newQuote.GetMsgType,
                    OrderNo = request.OrderNo,
                    ClOrdID = newQuote.ClOrdID,
                    ExchangeID = "", // ?
                    RefExchangeID = "", // ?
                    SeqNum = 0,  // khi nào sở về mới update
                    Symbol = request.Symbol,
                    Side = request.Side,
                    Price = request.Price,
                    OrderQty = request.OrderQty,
                    CrossType = "", // ?
                    ClientID = request.ClientID,
                    ClientIDCounterFirm = "", // ?
                    MemberCounterFirm = "", // ?
                    OrderType = request.OrderType
                };
                newQuote.ApiOrderNo = request.OrderNo;
                OrderMemory.Add_NewOrder(objOrder);
                //
                /*long  enqueue = c_IProcessRevEntity.EnqueueData(newQuote);
                //
                if (enqueue >=0)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                    _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                    Logger.ApiLog.Info("Process API1NewElectronicPutThrough_BU OrderNo {0} Success", request.OrderNo);
                }    
                else
                {

                    _response.ReturnCode = ORDER_RETURNCODE.Application_Error;
                    _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                    Logger.ApiLog.Warn("Process API1NewElectronicPutThrough_BU OrderNo {0} fail because Queue is full", request.OrderNo);
                }  */
                long enqueue = c_IProcessRevEntity.EnqueueData(newQuote);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API1NewElectronicPutThrough_BU OrderNo {0} fail because Queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                Logger.ApiLog.Info("Process API1NewElectronicPutThrough_BU OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API1NewElectronicPutThrough_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 1.2	Api chấp nhận lệnh thỏa thuận điện tử Outright
        /// </summary>
        public async Task<API2AcceptElectronicPutThroughResponse> API2AcceptElectronicPutThrough_BU(API2AcceptElectronicPutThroughRequest request, API2AcceptElectronicPutThroughResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API2AcceptElectronicPutThrough_BU with OrderNo: {request.OrderNo} ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";
                bool resultTradingRule = TradingRuleData.CheckTradingRule_Input(request.Symbol, out _responseText, out _responseCode);
                if (!resultTradingRule)
                {
                    _response.ReturnCode = _responseCode;
                    _response.Message = _responseText;
                    _response.InData = null;
                    return _response;
                }
                //
                /*bool checkOrderExist = OrderMemory.IsExist_OrderNo(request.OrderNo);
                if (checkOrderExist)
                {
                    _response.ReturnCode = ErrorCodeDefine.OrderNo_Duplicated;
                    _response.Message = ORDER_RETURNMESSAGE.OrderNo_Duplicated;
                    _response.InData = null;
                    return _response;
                }*/
                //
                /*
                    "Lấy giá trị refExchangeID gửi lên so với giá trị exchangeID  trong mem và  refMsgType =S hoặc refMsgType =AJ  trong mem:
                    + Nếu tìm thấy bản ghi:
                               . case 1: Nếu chỉ tìm thấy 1 bản ghi có orderno # rỗng và  , refExchangeID = rỗng, refMsgType =S (xảy ra khi gửi xác nhận lần đầu của lệnh RE)--> insert bản ghi mới  vào mem (có các thông tin orderNo, refMsg Type, exchangeID= refExchangeID, refExchangeID, symbol, side, price, orderQty, clientID)
                               . case 2: Nếu chỉ tìm thấy 1 bản ghi  khác với case 1--> cập nhật orderNo, refexchangeID, refMsgType, side, clientID
                               . case 3: Nếu tìm thấy 2 bản ghi trong đó 1 bản ghi có orderno # rỗng, refExchangeID = rỗng, refMsgType =S và 1 bản ghi có orderNo # rỗng, refExchangeID = exchangeID, refMsgType =AJ-->cập nhật vào bản ghi có refMsgType =AJ trong mem các thông tin orderNo, symbol, side, price, orderQty, clientID
                               case 4: Nếu tìm thấy hơn 2 bản ghi  hoặc tìm thấy 2 bản ghi có điều kiện khác case 3cần ghi lỗi
                    + Nếu không tìm thấy bản ghi: --> insert bản ghi mới có có các thông tin orderNo, refMsgType,refExchangeID, symbol, side, price, orderqty, clientid, orderType)"

                */
                List<OrderInfo> listOrder = OrderMemory.GetListOrder_byRefMsgType_With_S_AJ(request.RefExchangeID);
                string GenClOrdID = Utils.GenOrderNo();
                if (listOrder.Count > 0)
                {
                    if (listOrder.Count == 1)
                    {
                        OrderInfo objOrder = listOrder[0];

                        if (!string.IsNullOrEmpty(objOrder.OrderNo) && string.IsNullOrEmpty(objOrder.RefExchangeID) && objOrder.RefMsgType == MessageType.Quote)
                        {
                            // case 1: Nếu chỉ tìm thấy 1 bản ghi có orderno # rỗng và  , refExchangeID = rỗng, refMsgType =S (xảy ra khi gửi xác nhận lần đầu của lệnh RE)--> insert bản ghi mới  vào mem (có các thông tin orderNo, refMsg Type, exchangeID= refExchangeID, refExchangeID, symbol, side, price, orderQty, clientID)
                            OrderMemory.Add_NewOrder(new OrderInfo()
                            {
                                OrderNo = request.OrderNo,
                                RefMsgType = MessageType.QuoteResponse,
                                ExchangeID = request.RefExchangeID,
                                RefExchangeID = request.RefExchangeID,
                                Symbol = request.Symbol,
                                Side = request.Side,
                                Price = request.Price,
                                ClOrdID = GenClOrdID, //request.OrderNo,
                                OrderQty = request.OrderQty,
                                ClientID = request.ClientID,
                                OrderType = request.OrderType
                            });
                        }
                        else
                        {
                            // case 2: Nếu chỉ tìm thấy 1 bản ghi  khác với case RfexchangeID, refMsgType, side, clientID
                            OrderMemory.UpdateOrderInfo(objOrder, p_RefMsgType: MessageType.QuoteResponse, p_OrderNo: request.OrderNo, p_RefExchangeID: request.RefExchangeID, p_ClOrdID: GenClOrdID,
                                                p_ClientID: request.ClientID, p_Side: request.Side, p_OrdType: request.OrderType);
                            //
                            bool isUpdate = OrderMemory.UpdateMap_ClOrdID_Index(objOrder.ClOrdID);
                            if (!isUpdate)
                            {
                                Logger.ApiLog.Warn("Cannot Update map ClOrID - Index for Api 6 ClOrdID {0}", objOrder.ClOrdID);
                            }
                        }
                    }
                    else if (listOrder.Count == 2)
                    {
                        OrderInfo objOrderRefMsgType_S = null;
                        OrderInfo objOrderRefMsgType_AJ = null;
                        //
                        for (int i = 0; i < listOrder.Count; i++)
                        {
                            OrderInfo objOrder = listOrder[i];
                            if (objOrder.RefMsgType == MessageType.Quote)
                            {
                                objOrderRefMsgType_S = objOrder;
                            }
                            else if (objOrder.RefMsgType == MessageType.QuoteResponse)
                            {
                                objOrderRefMsgType_AJ = objOrder;
                            }
                        }
                        bool checkConditions = false; // biến ktra xem có thoả mãn điều kiện case 3 k
                        //  case 3: Nếu tìm thấy 2 bản ghi trong đó 1 bản ghi có orderno # rỗng, refExchangeID = rỗng, refMsgType =S và 1 bản ghi có orderNo # rỗng, refExchangeID = exchangeID, refMsgType =AJ-->cập nhật vào bản ghi có refMsgType =AJ trong mem các thông tin orderNo, symbol, side, price, orderQty, clientID
                        if (objOrderRefMsgType_S != null &&
                            objOrderRefMsgType_S.RefMsgType == MessageType.Quote &&
                            !string.IsNullOrEmpty(objOrderRefMsgType_S.OrderNo) &&
                            string.IsNullOrEmpty(objOrderRefMsgType_S.RefExchangeID))
                        {
                            checkConditions = true; // tìm thấy set = true
                        }
                        //
                        if (objOrderRefMsgType_AJ != null &&
                            objOrderRefMsgType_AJ.RefMsgType == MessageType.QuoteResponse &&
                            !string.IsNullOrEmpty(objOrderRefMsgType_AJ.OrderNo) &&
                            request.RefExchangeID == objOrderRefMsgType_AJ.ExchangeID)
                        {
                            checkConditions = true; // tìm thấy set = true
                        }
                        else
                        {
                            // tới đây k tìm đc thì set =false để ghi log
                            checkConditions = false;
                        }
                        //
                        if (checkConditions)
                        {
                            OrderMemory.UpdateOrderInfo(objOrderRefMsgType_AJ, p_RefMsgType: MessageType.QuoteResponse, p_OrderNo: request.OrderNo, p_ClOrdID: GenClOrdID, p_Symbol: request.Symbol,
                                               p_Side: request.Side, p_Price: request.Price, p_OrderQty: request.OrderQty, p_ClientID: request.ClientID, p_OrdType: request.OrderType);
                            //
                            bool isUpdate = OrderMemory.UpdateMap_ClOrdID_Index(objOrderRefMsgType_AJ.ClOrdID);
                            if (!isUpdate)
                            {
                                Logger.ApiLog.Warn("Cannot Update map ClOrID - Index for Api 6 ClOrdID {0}", objOrderRefMsgType_AJ.ClOrdID);
                            }
                        }
                        else
                        {
                            // tìm thấy 2 bản ghi có điều kiện khác case 3 cần ghi lỗi
                            Logger.ApiLog.Warn($"Find orderInfo total = {listOrder.Count} row data with OrderNo={request.OrderNo}, RefExchangeID={request.RefExchangeID}, ClientID={request.ClientID}, OrderType={request.OrderType}, Side={request.Side}, Symbol={request.Symbol}, Price={request.Price}, OrderQty={request.OrderQty} ; Please check again when write data on memory");
                        }
                    }
                    else //  case 4: Nếu tìm thấy hơn 2 bản ghi  hoặc tìm thấy 2 bản ghi có điều kiện khác case 3cần ghi lỗi
                    {
                        Logger.ApiLog.Warn($"Find orderInfo total = {listOrder.Count} row data with OrderNo={request.OrderNo}, RefExchangeID={request.RefExchangeID}, ClientID={request.ClientID}, OrderType={request.OrderType}, Side={request.Side}, Symbol={request.Symbol}, Price={request.Price}, OrderQty={request.OrderQty} ; Please check again when write data on memory");
                    }
                }
                else
                {
                    // Nếu không tìm thấy bản ghi: --> insert bản ghi mới có có các thông tin orderNo, refMsgType,refExchangeID, symbol, side, price, orderqty, clientid, orderType)
                    OrderMemory.Add_NewOrder(new OrderInfo()
                    {
                        OrderNo = request.OrderNo,
                        RefMsgType = MessageType.QuoteResponse,
                        ExchangeID = request.RefExchangeID,
                        RefExchangeID = request.RefExchangeID,
                        Symbol = request.Symbol,
                        Side = request.Side,
                        Price = request.Price,
                        ClOrdID = GenClOrdID, //request.OrderNo,
                        OrderQty = request.OrderQty,
                        ClientID = request.ClientID,
                        OrderType = request.OrderType
                    });
                }

                // 35=AJ
                MessageQuoteResponse newQuoteResponse = new MessageQuoteResponse() { TimeInit = DateTime.Now.Ticks };
                newQuoteResponse.APIBussiness = ORDER_API.API_2;
                newQuoteResponse.ClOrdID = GenClOrdID; //request.OrderNo.ToString();
                newQuoteResponse.Account = request.ClientID;
                newQuoteResponse.CoAccount = request.ClientIDCounterFirm;
                newQuoteResponse.QuoteRespID = request.RefExchangeID;
                newQuoteResponse.OrdType = CORE_OrderType.TTDTO;
                newQuoteResponse.Side = request.Side == ORDER_SIDE.SIDE_BUY ? CORE_OrderSide.SIDE_BUY : CORE_OrderSide.SIDE_SELL;
                newQuoteResponse.Symbol = request.Symbol;
                newQuoteResponse.Price2 = request.Price;
                newQuoteResponse.OrderQty = request.OrderQty;
                newQuoteResponse.SettDate = request.SettleDate;
                newQuoteResponse.SettlMethod = request.SettleMethod;
                newQuoteResponse.QuoteRespType = 1;
                newQuoteResponse.SettlValue = 0;
                newQuoteResponse.Text = request.Text;
                newQuoteResponse.ApiOrderNo = request.OrderNo;
                newQuoteResponse.IDRequest = ID;
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newQuoteResponse);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API2AcceptElectronicPutThrough OrderNo: {0} fail because Queue is full", request.OrderNo);
                    return _response;
                }
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                Logger.ApiLog.Info("Process API2AcceptElectronicPutThrough OrderNo: {0} success", request.OrderNo);

                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API2AcceptElectronicPutThrough_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 1.3	Api sửa thỏa thuận điện tử Outright chưa thực hiện
        /// </summary>
        public async Task<API3ReplaceElectronicPutThroughResponse> API3ReplaceElectronicPutThrough_BU(API3ReplaceElectronicPutThroughRequest request, API3ReplaceElectronicPutThroughResponse _response)
        {
            try
            {

                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API3ReplaceElectronicPutThrough_BU with OrderNo: {request.OrderNo} ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";

                if (!OrderMemory.CheckValidOrderNoForReplaceCancel(request.OrderNo, request.RefExchangeID))
                {
                    _response.ReturnCode = ORDER_RETURNCODE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                    _response.Message = ORDER_RETURNMESSAGE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                    _response.InData = null;
                    return _response;
                }
                bool resultTradingRule = TradingRuleData.CheckTradingRule_Input(request.Symbol, out _responseText, out _responseCode);
                if (!resultTradingRule)
                {
                    _response.ReturnCode = _responseCode;
                    _response.Message = _responseText;
                    _response.InData = null;
                    return _response;
                }
                //
                string _ExOrderNo = Utils.GenOrderNo();
                // 35=R
                MessageQuoteRequest newQuoteRequest = new MessageQuoteRequest() { TimeInit = DateTime.Now.Ticks };
                newQuoteRequest.APIBussiness = ORDER_API.API_3;
                newQuoteRequest.ClOrdID = _ExOrderNo;
                newQuoteRequest.RFQReqID = request.RefExchangeID;
                newQuoteRequest.Account = request.ClientID;
                newQuoteRequest.OrdType = CORE_OrderType.TTDTO;
                newQuoteRequest.Side = request.Side == ORDER_SIDE.SIDE_BUY ? CORE_OrderSide.SIDE_BUY : CORE_OrderSide.SIDE_SELL;
                newQuoteRequest.Symbol = request.Symbol;
                newQuoteRequest.Price2 = request.Price;
                newQuoteRequest.OrderQty = request.OrderQty;
                newQuoteRequest.SettDate = request.SettleDate;
                newQuoteRequest.SettlMethod = request.SettleMethod;
                newQuoteRequest.RegistID = request.RegistID;
                newQuoteRequest.SettlValue = 0;
                newQuoteRequest.Text = request.Text;
                newQuoteRequest.IDRequest = ID;
                // Add thông tin order vào memory
                LocalMemory.OrderInfo objOrderMem = new OrderInfo()
                {
                    RefMsgType = MessageType.QuoteRequest,
                    OrderNo = request.OrderNo,
                    ClOrdID = _ExOrderNo,
                    ExchangeID = "",
                    RefExchangeID = request.RefExchangeID,
                    SeqNum = 0,
                    Symbol = request.Symbol,
                    Side = request.Side,
                    Price = request.Price,
                    OrderQty = request.OrderQty,
                    CrossType = "",
                    ClientID = request.ClientID,
                    ClientIDCounterFirm = "",
                    MemberCounterFirm = "",
                    OrderType = request.OrderType,
                };
                newQuoteRequest.ApiOrderNo = request.OrderNo;
                OrderMemory.Add_NewOrder(objOrderMem);
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newQuoteRequest);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API3ReplaceElectronicPutThrough OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                Logger.ApiLog.Info("Process API3ReplaceElectronicPutThrough OrderNo {0} Success", request.OrderNo);

                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API3ReplaceElectronicPutThrough_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 1.4	Api huỷ thỏa thuận điện tử Outright chưa thực hiện
        /// </summary>
        public async Task<API4CancelElectronicPutThroughResponse> API4CancelElectronicPutThrough_BU(API4CancelElectronicPutThroughRequest request, API4CancelElectronicPutThroughResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API4CancelElectronicPutThrough_BU with OrderNo: {request.OrderNo}  ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";
                if (!OrderMemory.CheckValidOrderNoForReplaceCancel(request.OrderNo, request.RefExchangeID))
                {
                    _response.ReturnCode = ORDER_RETURNCODE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                    _response.Message = ORDER_RETURNMESSAGE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                    _response.InData = null;
                    return _response;
                }
                bool resultTradingRule = TradingRuleData.CheckTradingRule_Input(request.Symbol, out _responseText, out _responseCode);
                if (!resultTradingRule)
                {
                    _response.ReturnCode = _responseCode;
                    _response.Message = _responseText;
                    _response.InData = null;
                    return _response;
                }
                //
                string _ExOrderNo = Utils.GenOrderNo();
                // 35=Z
                MessageQuoteCancel newQuoteCancel = new MessageQuoteCancel() { TimeInit = DateTime.Now.Ticks };
                newQuoteCancel.APIBussiness = ORDER_API.API_4;
                newQuoteCancel.ClOrdID = _ExOrderNo;
                newQuoteCancel.QuoteID = request.RefExchangeID;
                newQuoteCancel.OrdType = CORE_OrderType.TTDTO;
                newQuoteCancel.Symbol = request.Symbol;
                newQuoteCancel.QuoteCancelType = 4;
                newQuoteCancel.Text = request.Text;
                // Add thông tin order vào memory
                LocalMemory.OrderInfo objOrder = new OrderInfo()
                {
                    RefMsgType = newQuoteCancel.GetMsgType,
                    OrderNo = request.OrderNo,
                    ClOrdID = newQuoteCancel.ClOrdID,
                    ExchangeID = "", // ?
                    RefExchangeID = request.RefExchangeID,
                    SeqNum = 0,  // khi nào sở về mới update
                    Symbol = request.Symbol,
                    Side = "",
                    Price = 0,
                    OrderQty = 0,
                    CrossType = "", // ?
                    ClientID = "",
                    ClientIDCounterFirm = "", // ?
                    MemberCounterFirm = "", // ?
                    OrderType = request.OrderType
                };
                newQuoteCancel.ApiOrderNo = request.OrderNo;
                newQuoteCancel.IDRequest = ID;
                OrderMemory.Add_NewOrder(objOrder);
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newQuoteCancel);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API4CancelElectronicPutThrough OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                Logger.ApiLog.Info("Process API4CancelElectronicPutThrough OrderNo {0} Success", request.OrderNo);
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API4CancelElectronicPutThrough_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

   

    }
}