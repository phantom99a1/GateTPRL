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
        /// 1.5	Api đặt lệnh thỏa thuận Báo cáo giao dịch Outright
        /// </summary>
        public async Task<API5NewCommonPutThroughResponse> API5NewCommonPutThrough_BU(API5NewCommonPutThroughRequest request, API5NewCommonPutThroughResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API5NewCommonPutThrough_BU with OrderNo: {request.OrderNo} ID {ID}");
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
                // 35=s
                MessageNewOrderCross newOrderCross = new MessageNewOrderCross() { TimeInit = DateTime.Now.Ticks };
                newOrderCross.APIBussiness = ORDER_API.API_5;
                newOrderCross.ClOrdID = request.OrderNo.ToString();
                newOrderCross.CrossID = "";
                newOrderCross.OrdType = CORE_OrderType.BCGDO;
                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newOrderCross.Side = CORE_OrderSide.SIDE_BUY;
                    newOrderCross.Account = request.ClientIDCounterFirm;
                    newOrderCross.CoAccount = request.ClientID;
                    newOrderCross.PartyID = request.MemberCounterFirm;
                    newOrderCross.CoPartyID = ConfigData.FirmID;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newOrderCross.Side = CORE_OrderSide.SIDE_SELL;
                    newOrderCross.Account = request.ClientID;
                    newOrderCross.CoAccount = request.ClientIDCounterFirm;
                    newOrderCross.PartyID = ConfigData.FirmID;
                    newOrderCross.CoPartyID = request.MemberCounterFirm;
                }
                newOrderCross.Symbol = request.Symbol;
                newOrderCross.Price2 = request.Price;
                newOrderCross.OrderQty = request.OrderQty;
                newOrderCross.CrossType = request.CrossType;
                newOrderCross.SettDate = request.SettleDate;
                newOrderCross.SettlMethod = request.SettleMethod;
                newOrderCross.EffectiveTime = request.EffectiveTime;
                newOrderCross.SettlValue = 0;
                newOrderCross.Text = request.Text;
                // Add thông tin order vào memory
                LocalMemory.OrderInfo objOrder = new OrderInfo()
                {
                    RefMsgType = newOrderCross.GetMsgType,
                    OrderNo = request.OrderNo,
                    ClOrdID = newOrderCross.ClOrdID,
                    ExchangeID = "", // ?
                    RefExchangeID = "", // ?
                    SeqNum = 0,  // khi nào sở về mới update-
                    Symbol = request.Symbol,
                    Side = request.Side,
                    Price = request.Price,
                    OrderQty = request.OrderQty,
                    CrossType = request.CrossType.ToString(),
                    ClientID = request.ClientID,
                    ClientIDCounterFirm = request.ClientIDCounterFirm,
                    MemberCounterFirm = request.MemberCounterFirm,
                    OrderType = request.OrderType
                };
                newOrderCross.ApiOrderNo = request.OrderNo;
                newOrderCross.OrderNo = request.OrderNo;
                newOrderCross.IDRequest = ID;
                OrderMemory.Add_NewOrder(objOrder);
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newOrderCross);
                //
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API5NewCommonPutThrough OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }    
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                Logger.ApiLog.Info("Process API5NewCommonPutThrough OrderNo {0} Success", request.OrderNo);
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API5NewCommonPutThrough_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 1.6	Api chấp nhận thỏa thuận Báo cáo giao dịch Outright
        /// </summary>
        public async Task<API6AcceptCommonPutThroughResponse> API6AcceptCommonPutThrough_BU(API6AcceptCommonPutThroughRequest request, API6AcceptCommonPutThroughResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API6AcceptCommonPutThrough_BU with OrderNo: {request.OrderNo}, ID {ID}");
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
                string GenClOrdID = Utils.GenOrderNo();
                //
                /*bool checkOrderExist = OrderMemory.IsExist_OrderNo(request.OrderNo);
                if (checkOrderExist)
                {
                    _response.ReturnCode = ErrorCodeDefine.OrderNo_Duplicated;
                    _response.Message = ORDER_RETURNMESSAGE.OrderNo_Duplicated;
                    _response.InData = null;
                    return _response;
                }*/
                // 35=s
                MessageNewOrderCross newOrderCross = new MessageNewOrderCross() { TimeInit = DateTime.Now.Ticks };
                newOrderCross.APIBussiness = ORDER_API.API_6;
                newOrderCross.ClOrdID = GenClOrdID;
                newOrderCross.CrossID = request.RefExchangeID;
                newOrderCross.OrdType = CORE_OrderType.BCGDO;
                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newOrderCross.Side = CORE_OrderSide.SIDE_BUY;
                    newOrderCross.Account = request.ClientIDCounterFirm;
                    newOrderCross.CoAccount = request.ClientID;
                    newOrderCross.CoPartyID = ConfigData.FirmID;
                    newOrderCross.PartyID = request.MemberCounterFirm;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newOrderCross.Side = CORE_OrderSide.SIDE_SELL;
                    newOrderCross.Account = request.ClientID;
                    newOrderCross.CoAccount = request.ClientIDCounterFirm;
                    newOrderCross.PartyID = ConfigData.FirmID;
                    newOrderCross.CoPartyID = request.MemberCounterFirm;
                }
                newOrderCross.Symbol = request.Symbol;
                newOrderCross.Price2 = request.Price;
                newOrderCross.OrderQty = request.OrderQty;
                newOrderCross.CrossType = request.CrossType;
                newOrderCross.SettDate = request.SettleDate;
                newOrderCross.SettlMethod = request.SettleMethod;
                newOrderCross.EffectiveTime = request.EffectiveTime;
                newOrderCross.SettlValue = 0;
                newOrderCross.Text = request.Text;
                newOrderCross.ApiOrderNo = request.OrderNo;
                newOrderCross.OrderNo = request.OrderNo;

                newOrderCross.IDRequest = ID;
                // Cập nhật thông tin order vào memory
                // Cập nhật orderNo, refexchangeID, clientID ( so sánh refExchangeID gửi lên với exchangeID lưu mem với  refMsgType =s)
                OrderInfo objOrder = OrderMemory.GetOrderBy(p_RefMsgType: MessageType.NewOrderCross, p_ExchangeID: request.RefExchangeID);
                if (objOrder != null)
                {
                    //Nếu tìm thấy bản ghi: cập nhật orderNo, refexchangeID, clientID
                    Logger.ApiLog.Info($"ProcessRevBussiness | GetOrder_byExchangeID with request_OrderNo: {request.OrderNo}, objOrder_OrderNo:{objOrder.OrderNo}, objOrder_RefMsgType:{objOrder.RefMsgType}");
                    //
                    OrderMemory.UpdateOrderInfo(objOrder, p_RefMsgType: MessageType.NewOrderCross, p_OrderNo: request.OrderNo, p_ClOrdID: GenClOrdID, p_RefExchangeID: request.RefExchangeID,
                                                p_ClientID: request.ClientID, p_OrdType: request.OrderType);
                    //
                    bool isUpdate = OrderMemory.UpdateMap_ClOrdID_Index(objOrder.ClOrdID);
                    if (!isUpdate)
                    {
                        Logger.ApiLog.Warn("Cannot Update map ClOrID - Index for Api 6 ClOrdID {0}", objOrder.ClOrdID);
                    }
                }
                else
                {
                    //Nếu không tin thấy bản ghi, sinh mem mới  (có các thông tin orderNo, refMsgType, refExchangeID, symbol, side, price, orderqty, crossType, clientid, clientIDCounterFirm, memberCounterFirm, orderType)
                    LocalMemory.OrderInfo objOrderMem = new OrderInfo()
                    {
                        OrderNo = request.OrderNo,
                        RefMsgType = MessageType.NewOrderCross,
                        RefExchangeID = request.RefExchangeID,
                        Symbol = request.Symbol,
                        Side = request.Side,
                        Price = request.Price,
                        OrderQty = request.OrderQty,
                        CrossType = request.CrossType.ToString(),
                        ClientID = request.ClientID,
                        ClientIDCounterFirm = request.ClientIDCounterFirm,
                        MemberCounterFirm = request.MemberCounterFirm,
                        OrderType = request.OrderType,
                        ClOrdID = GenClOrdID,
                        SeqNum = 0
                    };
                    OrderMemory.Add_NewOrder(objOrderMem);
                }
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newOrderCross);
                //
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API6AcceptCommonPutThrough OrderNo {0} fail because Queue is full", request.OrderNo);
                    return _response;
                }
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API6AcceptCommonPutThrough OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API6AcceptCommonPutThrough_BU, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 1.7	API7: API sửa lệnh thỏa thuận báo cáo giao dịch Outright chưa thực hiện
        /// </summary>
        public async Task<API7ReplaceCommonPutThroughResponse> API7ReplaceCommonPutThrough_BU(API7ReplaceCommonPutThroughRequest request, API7ReplaceCommonPutThroughResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API7ReplaceCommonPutThrough_BU with OrderNo: {request.OrderNo} ID {ID}");
                //

                if (!OrderMemory.CheckValidOrderNoForReplaceCancel(request.OrderNo, request.RefExchangeID))
                {
                    _response.ReturnCode = ORDER_RETURNCODE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                    _response.Message = ORDER_RETURNMESSAGE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                    _response.InData = null;
                    return _response;
                }
                string _ExOrderNo = Utils.GenOrderNo();
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
                //35=t
                CrossOrderCancelReplaceRequest newOrderCross = new CrossOrderCancelReplaceRequest() { TimeInit = DateTime.Now.Ticks };
                newOrderCross.APIBussiness = ORDER_API.API_7;
                newOrderCross.ClOrdID = _ExOrderNo;
                newOrderCross.OrgCrossID = request.RefExchangeID;
                newOrderCross.OrdType = CORE_OrderType.BCGDO;
                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newOrderCross.Side = CORE_OrderSide.SIDE_BUY;
                    newOrderCross.Account = request.ClientIDCounterFirm;
                    newOrderCross.CoAccount = request.ClientID;
                    newOrderCross.CoPartyID = ConfigData.FirmID;
                    newOrderCross.PartyID = request.MemberCounterFirm;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newOrderCross.Side = CORE_OrderSide.SIDE_SELL;
                    newOrderCross.Account = request.ClientID;
                    newOrderCross.CoAccount = request.ClientIDCounterFirm;
                    newOrderCross.PartyID = ConfigData.FirmID;
                    newOrderCross.CoPartyID = request.MemberCounterFirm;
                }
                newOrderCross.Symbol = request.Symbol;
                newOrderCross.Price2 = request.Price;
                newOrderCross.OrderQty = request.OrderQty;
                newOrderCross.CrossType = request.CrossType;
                newOrderCross.SettDate = request.SettleDate;
                newOrderCross.SettlMethod = request.SettleMethod;
                newOrderCross.EffectiveTime = request.EffectiveTime;
                newOrderCross.SettlValue = 0;
                newOrderCross.Text = request.Text;
                newOrderCross.ApiOrderNo = request.OrderNo;
                newOrderCross.OrderNo = request.OrderNo;
                newOrderCross.IDRequest = ID;
                // Add thông tin order vào memory
                LocalMemory.OrderInfo objOrder = new OrderInfo()
                {
                    RefMsgType = MessageType.CrossOrderCancelReplaceRequest,
                    OrderNo = request.OrderNo,
                    ClOrdID = newOrderCross.ClOrdID,
                    ExchangeID = "", // ?
                    RefExchangeID = request.RefExchangeID,
                    SeqNum = 0,  // khi nào sở về mới update-
                    Symbol = request.Symbol,
                    Side = request.Side,
                    Price = request.Price,
                    OrderQty = request.OrderQty,
                    CrossType = request.CrossType.ToString(),
                    ClientID = request.ClientID,
                    ClientIDCounterFirm = request.ClientIDCounterFirm,
                    MemberCounterFirm = request.MemberCounterFirm,
                    OrderType = request.OrderType
                };
                OrderMemory.Add_NewOrder(objOrder);
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newOrderCross);
                //
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API7ReplaceCommonPutThrough OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API7ReplaceCommonPutThrough OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API7ReplaceCommonPutThrough_BU, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 1.8	API8: API hủy lệnh thỏa thuận báo cáo giao dịch Outright chưa thực hiện
        /// </summary>
        public async Task<API8CancelCommonPutThroughResponse> API8CancelCommonPutThrough_BU(API8CancelCommonPutThroughRequest request, API8CancelCommonPutThroughResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API8CancelCommonPutThrough_BU with OrderNo: {request.OrderNo} ID: {ID}");
                //
                if (!OrderMemory.CheckValidOrderNoForReplaceCancel(request.OrderNo, request.RefExchangeID))
                {
                    _response.ReturnCode = ORDER_RETURNCODE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                    _response.Message = ORDER_RETURNMESSAGE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                    _response.InData = null;
                    return _response;
                }
                string _ExOrderNo = Utils.GenOrderNo();
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
                // 35=u
                CrossOrderCancelRequest newOrderCross = new CrossOrderCancelRequest() { TimeInit = DateTime.Now.Ticks };
                newOrderCross.APIBussiness = ORDER_API.API_8;
                newOrderCross.ClOrdID = _ExOrderNo;
                newOrderCross.OrgCrossID = request.RefExchangeID;
                newOrderCross.OrdType = request.OrderType;
                newOrderCross.Symbol = request.Symbol;
                newOrderCross.IDRequest = ID;
                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newOrderCross.Side = CORE_OrderSide.SIDE_BUY;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newOrderCross.Side = CORE_OrderSide.SIDE_SELL;
                }
                newOrderCross.CrossType = request.CrossType;
                newOrderCross.Text = request.Text;
                newOrderCross.ApiOrderNo = request.OrderNo;
                newOrderCross.OrderNo = request.OrderNo;
                // Add thông tin order vào memory
                LocalMemory.OrderInfo objOrder = new OrderInfo()
                {
                    RefMsgType = newOrderCross.GetMsgType,
                    OrderNo = request.OrderNo,
                    ClOrdID = newOrderCross.ClOrdID,
                    ExchangeID = "", // ?
                    RefExchangeID = request.RefExchangeID,
                    SeqNum = 0,  // khi nào sở về mới update
                    Symbol = request.Symbol,
                    Side = request.Side,
                    Price = 0,
                    OrderQty = 0,
                    CrossType = request.CrossType.ToString(), // ?
                    ClientID = "",
                    ClientIDCounterFirm = "", // ?
                    MemberCounterFirm = "", // ?
                    OrderType = request.OrderType
                };
                OrderMemory.Add_NewOrder(objOrder);
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newOrderCross);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API8CancelCommonPutThrough OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API8CancelCommonPutThrough OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API8CancelCommonPutThrough_BU, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 1.9	API9: API sửa lệnh thỏa thuận Outright đã thực hiện
        /// </summary>
        public async Task<API9ReplaceCommonPutThroughDealResponse> API9ReplaceCommonPutThroughDeal_BU(API9ReplaceCommonPutThroughDealRequest request, API9ReplaceCommonPutThroughDealResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API9ReplaceCommonPutThroughDeal_BU with OrderNo: {request.OrderNo} ID: {ID}");
                //
                if (!OrderMemory.CheckValidOrderNoForReplaceCancel(request.OrderNo, request.RefExchangeID))
                {
                    _response.ReturnCode = ORDER_RETURNCODE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                    _response.Message = ORDER_RETURNMESSAGE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                    _response.InData = null;
                    return _response;
                }
                string _ExOrderNo = Utils.GenOrderNo();
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
                // 35=t
                CrossOrderCancelReplaceRequest newOrderCross = new CrossOrderCancelReplaceRequest() { TimeInit = DateTime.Now.Ticks };
                newOrderCross.APIBussiness = ORDER_API.API_9;
                newOrderCross.ClOrdID = _ExOrderNo;
                newOrderCross.OrgCrossID = request.RefExchangeID;
                newOrderCross.OrdType = request.OrderType;
                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newOrderCross.Side = CORE_OrderSide.SIDE_BUY;
                    newOrderCross.Account = request.ClientIDCounterFirm;
                    newOrderCross.CoAccount = request.ClientID;
                    newOrderCross.CoPartyID = ConfigData.FirmID;
                    newOrderCross.PartyID = request.MemberCounterFirm;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newOrderCross.Side = CORE_OrderSide.SIDE_SELL;
                    newOrderCross.Account = request.ClientID;
                    newOrderCross.CoAccount = request.ClientIDCounterFirm;
                    newOrderCross.PartyID = ConfigData.FirmID;
                    newOrderCross.CoPartyID = request.MemberCounterFirm;
                }
                newOrderCross.Symbol = request.Symbol;
                newOrderCross.Price2 = request.Price;
                newOrderCross.OrderQty = request.OrderQty;
                newOrderCross.CrossType = request.CrossType;
                newOrderCross.SettDate = request.SettleDate;
                newOrderCross.SettlMethod = request.SettleMethod;
                newOrderCross.EffectiveTime = request.EffectiveTime;
                newOrderCross.SettlValue = 0;
                newOrderCross.Text = request.Text;
                newOrderCross.ApiOrderNo = request.OrderNo;
                newOrderCross.OrderNo = request.OrderNo;
                newOrderCross.IDRequest = ID;
                //
                OrderInfo objOrder = OrderMemory.GetOrderBy(p_RefMsgType: MessageType.CrossOrderCancelReplaceRequest, p_RefExchangeID: request.RefExchangeID);
                if (objOrder != null)
                {
                    if (string.IsNullOrEmpty(objOrder.ExchangeID))
                    {
                        //TH1: Nếu tìm được bản ghi và exchangeID = trống --> không sinh bản ghi mới, cập nhật orderNo, orderType trên bản ghi tìm được
                        OrderMemory.UpdateOrderInfo(objOrder, p_OrderNo: request.OrderNo, p_ClOrdID: _ExOrderNo, p_OrdType: request.OrderType);
                        //
                        bool isUpdate = OrderMemory.UpdateMap_ClOrdID_Index(objOrder.ClOrdID);
                        if (!isUpdate)
                        {
                            Logger.ApiLog.Warn("Cannot Update map ClOrID - Index for Api 9 ClOrdID {0}", objOrder.ClOrdID);
                        }
                    }
                    else
                    {
                        //TH2: Nếu tìm được bản ghi và exchangeID # trống -->  sinh bản ghi mới (orderNo, refMsgType, refExchangeID, symbol, side, price, orderQty,crossType, clientID, clientIdCounterFirm, memberCounterFirm, orderType)
                        OrderMemory.Add_NewOrder(new OrderInfo()
                        {
                            OrderNo = request.OrderNo,
                            RefMsgType = MessageType.CrossOrderCancelReplaceRequest,
                            RefExchangeID = request.RefExchangeID,
                            Symbol = request.Symbol,
                            Side = request.Side,
                            Price = request.Price,
                            OrderQty = request.OrderQty,
                            CrossType = request.CrossType.ToString(),
                            ClientID = request.ClientID,
                            ClientIDCounterFirm = request.ClientIDCounterFirm,
                            MemberCounterFirm = request.MemberCounterFirm,
                            OrderType = request.OrderType,
                            ClOrdID = _ExOrderNo,
                        });
                    } 
                        
                    
                }
                else
                {
                    // TH3: Nếu không tìm thấy  --> tạo 1 bản ghi trên memory (orderNo, refMsgType, refExchangeID, symbol, side, price, orderQty,crossType, clientID, clientIdCounterFirm, memberCounterFirm, orderType)
                    objOrder = new OrderInfo()
                    {
                        RefMsgType = MessageType.CrossOrderCancelReplaceRequest,
                        OrderNo = request.OrderNo,
                        ClOrdID = newOrderCross.ClOrdID,
                        ExchangeID = "", // ?
                        RefExchangeID = request.RefExchangeID,
                        SeqNum = 0,  // khi nào sở về mới update
                        Symbol = request.Symbol,
                        Side = request.Side,
                        Price = request.Price,
                        OrderQty = request.OrderQty,
                        CrossType = request.CrossType.ToString(),
                        ClientID = request.ClientID,
                        ClientIDCounterFirm = request.ClientIDCounterFirm,
                        MemberCounterFirm = request.MemberCounterFirm,
                        OrderType = request.OrderType
                    };
                    OrderMemory.Add_NewOrder(objOrder);
                }

                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newOrderCross);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API9ReplaceCommonPutThroughDeal OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                Logger.ApiLog.Info("Process API9ReplaceCommonPutThroughDeal OrderNo {0} Success", request.OrderNo);
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API9 ReplaceCommonPutThroughDeal_BU, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 1.10	API10: API phản hồi yêu cầu sửa thỏa thuận Outright đã thực hiện
        /// </summary>
        public async Task<API10ResponseForReplacingCommonPutThroughDealResponse> API10ResponseForReplacingCommonPutThroughDeal_BU(API10ResponseForReplacingCommonPutThroughDealRequest request, API10ResponseForReplacingCommonPutThroughDealResponse _response)
        {
            try
            {
                long t1 = DateTime.Now.Ticks;
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API10ResponseForReplacingCommonPutThroughDeal_BU with OrderNo: {request.OrderNo}");
                //
                if (!OrderMemory.CheckValidOrderNoForReplaceCancel(request.OrderNo, request.RefExchangeID))
                {
                    _response.ReturnCode = ORDER_RETURNCODE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                    _response.Message = ORDER_RETURNMESSAGE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                    _response.InData = null;
                    return _response;
                }
                string _ExOrderNo = Utils.GenOrderNo();
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
                int OldSeqNum = -1;
                // 35=t
                CrossOrderCancelReplaceRequest newOrderCross = new CrossOrderCancelReplaceRequest() { TimeInit = DateTime.Now.Ticks };
                newOrderCross.APIBussiness = ORDER_API.API_10;
                newOrderCross.ClOrdID = _ExOrderNo;
                newOrderCross.OrgCrossID = request.RefExchangeID;
                newOrderCross.OrdType = request.OrderType;
                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newOrderCross.Side = CORE_OrderSide.SIDE_BUY;
                    newOrderCross.Account = request.ClientIDCounterFirm;
                    newOrderCross.CoAccount = request.ClientID;
                    newOrderCross.CoPartyID = ConfigData.FirmID;
                    newOrderCross.PartyID = request.MemberCounterFirm;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newOrderCross.Side = CORE_OrderSide.SIDE_SELL;
                    newOrderCross.Account = request.ClientID;
                    newOrderCross.CoAccount = request.ClientIDCounterFirm;
                    newOrderCross.PartyID = ConfigData.FirmID;
                    newOrderCross.CoPartyID = request.MemberCounterFirm;
                }
                newOrderCross.Symbol = request.Symbol;
                newOrderCross.Price2 = request.Price;
                newOrderCross.OrderQty = request.OrderQty;
                newOrderCross.CrossType = request.CrossType;
                newOrderCross.SettDate = request.SettleDate;
                newOrderCross.SettlMethod = request.SettleMethod;
                newOrderCross.EffectiveTime = request.EffectiveTime;
                newOrderCross.SettlValue = 0;
                newOrderCross.Text = request.Text;
                newOrderCross.ApiOrderNo = request.OrderNo;
                newOrderCross.OrderNo = request.OrderNo;
                newOrderCross.IDRequest = ID;
                // Cập nhật thông tin order vào memory
                // cập nhật orderNo, refexchangeID ( so sánh refExchangeID gửi lên với exchangeID lưu mem với orderno = rỗng)
                OrderInfo objOrder = OrderMemory.GetOrderBy(p_RefMsgType: MessageType.CrossOrderCancelReplaceRequest, p_ExchangeID: request.RefExchangeID);
                if (objOrder != null)
                {
                    OrderMemory.UpdateOrderInfo(objOrder, p_OrderNo: request.OrderNo, p_ClOrdID: _ExOrderNo, p_OrdType: request.OrderType);
                    //
                    bool isUpdate = OrderMemory.UpdateMap_ClOrdID_Index(objOrder.ClOrdID);
                }
                //2023-10-09 DungNT thêm
                else
                {
                    OrderMemory.Add_NewOrder(new OrderInfo()
                    {
                        ClOrdID = _ExOrderNo,
                        OrderNo = request.OrderNo,
                        RefMsgType = MessageType.CrossOrderCancelReplaceRequest,
                        RefExchangeID = request.RefExchangeID,
                        Symbol = request.Symbol,
                        Side = request.Side,
                        Price = request.Price,
                        OrderQty = request.OrderQty,
                        CrossType = request.CrossType.ToString(),
                        ClientID = request.ClientID,
                        ClientIDCounterFirm = request.ClientIDCounterFirm,
                        MemberCounterFirm = request.MemberCounterFirm,
                        OrderType = request.OrderType,
                    });
                }
                //
                newOrderCross.ApiSeqNum = OldSeqNum;
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newOrderCross);
                //
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API10ResponseForReplacingCommonPutThroughDeal OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                Logger.ApiLog.Info("Process API10ResponseForReplacingCommonPutThroughDeal OrderNo {0} Success", request.OrderNo);
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API10ResponseForReplacingCommonPutThroughDeal_BU, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 1.11	API11: API hủy lệnh thỏa thuận Outright đã thực hiện
        /// </summary>
        public async Task<API11CancelCommonPutThroughDealResponse> API11CancelCommonPutThroughDeal_BU(API11CancelCommonPutThroughDealRequest request, API11CancelCommonPutThroughDealResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API11CancelCommonPutThroughDeal_BU with OrderNo: {request.OrderNo} ID: {ID}");
                //
                if (!OrderMemory.CheckValidOrderNoForReplaceCancel(request.OrderNo, request.RefExchangeID))
                {
                    _response.ReturnCode = ORDER_RETURNCODE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                    _response.Message = ORDER_RETURNMESSAGE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                    _response.InData = null;
                    return _response;
                }
                string _ExOrderNo = Utils.GenOrderNo();
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
                // 35=u
                CrossOrderCancelRequest newOrderCross = new CrossOrderCancelRequest() { TimeInit = DateTime.Now.Ticks };
                newOrderCross.APIBussiness = ORDER_API.API_11;
                newOrderCross.ClOrdID = _ExOrderNo;
                newOrderCross.OrgCrossID = request.RefExchangeID;
                newOrderCross.OrdType = request.OrderType;
                newOrderCross.Symbol = request.Symbol;

                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newOrderCross.Side = CORE_OrderSide.SIDE_BUY;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newOrderCross.Side = CORE_OrderSide.SIDE_SELL;
                }
                newOrderCross.CrossType = request.CrossType;
                newOrderCross.Text = request.Text;
                newOrderCross.ApiOrderNo = request.OrderNo;
                newOrderCross.OrderNo = request.OrderNo;
                newOrderCross.IDRequest = ID;
                //
                OrderInfo objOrder = OrderMemory.GetOrderBy(p_RefMsgType: MessageType.CrossOrderCancelRequest, p_RefExchangeID: request.RefExchangeID);
                if (objOrder != null)
                {                    
                    if (string.IsNullOrEmpty(objOrder.ExchangeID))
                    {
                        //TH1: Nếu tìm được bản ghi và exchangeID = trống --> không sinh bản ghi mới, cập nhật orderNo, orderType trên bản ghi tìm được
                        OrderMemory.UpdateOrderInfo(objOrder, p_OrderNo: request.OrderNo, p_ClOrdID: _ExOrderNo, p_OrdType: request.OrderType, p_ExchangeID: string.Empty);
                        //
                        bool isUpdate = OrderMemory.UpdateMap_ClOrdID_Index(objOrder.ClOrdID);
                        if (!isUpdate)
                        {
                            Logger.ApiLog.Warn("Cannot Update map ClOrID - Index for Api 6 ClOrdID {0}", objOrder.ClOrdID);
                        }
                    }
                    else
                    {
                        //TH2: Nếu tìm được bản ghi và exchangeID # trống --> sinh bản ghi mới (orderNo, refMsgType, refExchangeID, symbol, side, crossType, orderType)
                        OrderMemory.Add_NewOrder(new OrderInfo()
                        {
                            OrderNo = request.OrderNo,
                            RefMsgType = MessageType.CrossOrderCancelRequest,
                            RefExchangeID = request.RefExchangeID,
                            Symbol = request.Symbol,
                            Side = request.Side,
                            CrossType = request.CrossType.ToString(),
                            OrderType = request.OrderType,
                            ClOrdID = _ExOrderNo,
                        });
                    }
                }
                else
                {
                    //TH3: Nếu không tìm thấy --> tạo 1 bản ghi trên memory (orderNo, refMsgType, refExchangeID, symbol, side, crossType, orderType)
                    // Add thông tin order vào memory
                    objOrder = new OrderInfo()
                    {
                        RefMsgType = newOrderCross.GetMsgType,
                        OrderNo = request.OrderNo,
                        ClOrdID = newOrderCross.ClOrdID,
                        ExchangeID = "", // ?
                        RefExchangeID = request.RefExchangeID,
                        SeqNum = 0,  // khi nào sở về mới update
                        Symbol = request.Symbol,
                        Side = request.Side,
                        Price = 0,
                        OrderQty = 0,
                        CrossType = request.CrossType.ToString(),
                        ClientID = "",
                        ClientIDCounterFirm = "", // ?
                        MemberCounterFirm = "", // ?
                        OrderType = request.OrderType
                    };
                    OrderMemory.Add_NewOrder(objOrder);
                }

                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newOrderCross);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API11CancelCommonPutThroughDeal OrderNo {0} fail bcause queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API11CancelCommonPutThroughDeal OrderNo {0} Success");
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API11CancelCommonPutThroughDeal_BU, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 1.12	API12: API phản hồi hủy lệnh thỏa thuận Outright đã thực
        /// </summary>
        public async Task<API12ResponseForCancelingCommonPutThroughDealResponse> API12ResponseForCancelingCommonPutThroughDeal_BU(API12ResponseForCancelingCommonPutThroughDealRequest request, API12ResponseForCancelingCommonPutThroughDealResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API12ResponseForCancelingCommonPutThroughDeal_BU with OrderNo: {request.OrderNo} ID: {ID}");
                //
                if (!OrderMemory.CheckValidOrderNoForReplaceCancel(request.OrderNo, request.RefExchangeID))
                {
                    _response.ReturnCode = ORDER_RETURNCODE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                    _response.Message = ORDER_RETURNMESSAGE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                    _response.InData = null;
                    return _response;
                }
                string _ExOrderNo = Utils.GenOrderNo();
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
                int OldSeqNum = -1;
                // 35=u
                CrossOrderCancelRequest newOrderCross = new CrossOrderCancelRequest() { TimeInit = DateTime.Now.Ticks };
                newOrderCross.APIBussiness = ORDER_API.API_12;
                newOrderCross.ClOrdID = _ExOrderNo;
                newOrderCross.OrgCrossID = request.RefExchangeID;
                newOrderCross.OrdType = request.OrderType;
                newOrderCross.Symbol = request.Symbol;

                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newOrderCross.Side = CORE_OrderSide.SIDE_BUY;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newOrderCross.Side = CORE_OrderSide.SIDE_SELL;
                }
                newOrderCross.CrossType = request.CrossType;
                newOrderCross.Text = request.Text;
                newOrderCross.ApiOrderNo = request.OrderNo;
                newOrderCross.OrderNo = request.OrderNo;
                newOrderCross.IDRequest = ID;
                // Cập nhật thông tin order vào memory
                // cập nhật orderNo, refexchangeID ( so sánh refExchangeID gửi lên với exchangeID lưu mem với orderno = rỗng)
                OrderInfo objOrder = OrderMemory.GetOrderBy(p_RefMsgType: MessageType.CrossOrderCancelRequest, p_ExchangeID: request.RefExchangeID);
                if (objOrder != null)
                {
                    OrderMemory.UpdateOrderInfo(objOrder, p_OrderNo: request.OrderNo, p_ClOrdID: _ExOrderNo, p_OrdType: request.OrderType);
                    //
                    bool isUpdate = OrderMemory.UpdateMap_ClOrdID_Index(objOrder.ClOrdID);
                    if (!isUpdate)
                    {
                        Logger.ApiLog.Warn("Cannot Update map ClOrID - Index for Api 12 ClOrdID {0}", objOrder.ClOrdID);
                    }
                }
                else
                {
                    OrderMemory.Add_NewOrder(new OrderInfo()
                    {
                        OrderNo = request.OrderNo,
                        ClOrdID = _ExOrderNo,
                        RefMsgType = MessageType.CrossOrderCancelRequest,
                        RefExchangeID = request.RefExchangeID,
                        Symbol = request.Symbol,
                        Side = request.Side,
                        CrossType = request.CrossType.ToString(),
                        OrderType = request.OrderType,
                    });
                }

                //
                newOrderCross.ApiSeqNum = OldSeqNum;
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newOrderCross);
                //
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API12ResponseForCancelingCommonPutThroughDeal OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API12ResponseForCancelingCommonPutThroughDeal OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API12ResponseForCancelingCommonPutThroughDeal_BU, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }
    }
}