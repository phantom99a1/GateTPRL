using BusinessProcessAPIReq.RequestModels;
using BusinessProcessAPIReq.ResponseModels;
using CommonLib;
using HNX.FIXMessage;
using LocalMemory;
using Microsoft.AspNetCore.Mvc;
using static CommonLib.CommonData;
using static CommonLib.CommonDataInCore;

namespace BusinessProcessAPIReq
{
    public partial class ProcessRevBusiness : IProcessRevBussiness
    {
        /// <summary>
        /// 2.1	API13 – Đặt lệnh điện tử tùy chọn Inquiry Repos
        /// </summary>
        public async Task<API13NewInquiryReposResponse> API13NewInquiryRepos_BU(API13NewInquiryReposRequest request, API13NewInquiryReposResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API13NewInquiryRepos_BU with OrderNo: {request.OrderNo} ID {ID}");
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
                // 35= N01
                MessageReposInquiry newInquiryRepos = new MessageReposInquiry() { TimeInit = DateTime.Now.Ticks };
                newInquiryRepos.APIBussiness = ORDER_API.API_13;
                newInquiryRepos.ClOrdID = request.OrderNo.ToString();
                newInquiryRepos.Symbol = request.Symbol;
                newInquiryRepos.QuoteType = request.QuoteType;
                newInquiryRepos.OrdType = request.OrderType;
                newInquiryRepos.Side = request.Side == ORDER_SIDE.SIDE_BUY ? CORE_OrderSide.SIDE_BUY : CORE_OrderSide.SIDE_SELL;
                newInquiryRepos.OrderQty = request.OrderValue;
                newInquiryRepos.EffectiveTime = request.EffectiveTime;
                newInquiryRepos.SettlMethod = request.SettleMethod;
                newInquiryRepos.SettlDate = request.SettleDate1;
                newInquiryRepos.SettlDate2 = request.SettleDate2;
                newInquiryRepos.EndDate = request.EndDate;
                newInquiryRepos.RepurchaseTerm = request.RepurchaseTerm;
                newInquiryRepos.RegistID = request.RegistID;
                newInquiryRepos.Text = request.Text;

                // Add thông tin order vào memory
                LocalMemory.OrderInfo objOrder = new OrderInfo()
                {
                    RefMsgType = MessageType.ReposInquiry,
                    OrderNo = newInquiryRepos.ClOrdID,
                    ClOrdID = newInquiryRepos.ClOrdID,
                    ExchangeID = "", // ?
                    RefExchangeID = "", // ?
                    SeqNum = 0,  // khi nào sở về mới update
                    Symbol = request.Symbol,
                    Side = request.Side,
                    Price = 0,
                    OrderQty = request.OrderValue,
                    QuoteType = request.QuoteType,
                    CrossType = "", // ?
                    ClientID = "",
                    ClientIDCounterFirm = "", // ?
                    MemberCounterFirm = "", // ?
                    OrderType = request.OrderType
                };
                newInquiryRepos.ApiOrderNo = request.OrderNo;
                OrderMemory.Add_NewOrder(objOrder);
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newInquiryRepos);
                //
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API13NewInquiryRepo OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                Logger.ApiLog.Info("Process API13NewInquiryRepo OrderNo {0} Success", request.OrderNo);

                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API13NewInquiryRepos_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 2.2	API14 – Sửa lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
        /// </summary>
        public async Task<API14ReplaceInquiryReposResponse> API14ReplaceInquiryRepos_BU(API14ReplaceInquiryReposRequest request, API14ReplaceInquiryReposResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API14ReplaceInquiryRepos_BU with OrderNo: {request.OrderNo} ID {ID}");
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
                OrderInfo objOrder_RefExchangeID = OrderMemory.GetOrder_byExchangeID(request.RefExchangeID);
                if (objOrder_RefExchangeID != null)
                {
                    if (objOrder_RefExchangeID.OrderNo != request.OrderNo)
                    {
                        _response.ReturnCode = ORDER_RETURNCODE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                        _response.Message = ORDER_RETURNMESSAGE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                        _response.InData = null;
                        return _response;
                    }
                }
                string _ExOrderNo = Utils.GenOrderNo();
                // 35= N01
                MessageReposInquiry newInquiryRepos = new MessageReposInquiry() { TimeInit = DateTime.Now.Ticks };
                newInquiryRepos.APIBussiness = ORDER_API.API_14;
                newInquiryRepos.ClOrdID = _ExOrderNo;
                newInquiryRepos.RFQReqID = request.RefExchangeID;
                newInquiryRepos.Symbol = request.Symbol;
                newInquiryRepos.QuoteType = request.QuoteType;
                newInquiryRepos.OrdType = request.OrderType;
                newInquiryRepos.Side = request.Side == ORDER_SIDE.SIDE_BUY ? CORE_OrderSide.SIDE_BUY : CORE_OrderSide.SIDE_SELL;
                newInquiryRepos.OrderQty = request.OrderValue;
                newInquiryRepos.EffectiveTime = request.EffectiveTime;
                newInquiryRepos.SettlMethod = request.SettleMethod;
                newInquiryRepos.SettlDate = request.SettleDate1;
                newInquiryRepos.SettlDate2 = request.SettleDate2;
                newInquiryRepos.EndDate = request.EndDate;
                newInquiryRepos.RepurchaseTerm = request.RepurchaseTerm;
                newInquiryRepos.RegistID = request.RegistID;
                newInquiryRepos.Text = request.Text;

                // Add thông tin order vào memory
                LocalMemory.OrderInfo objOrder = new OrderInfo()
                {
                    RefMsgType = MessageType.ReposInquiry,
                    OrderNo = request.OrderNo,
                    ClOrdID = _ExOrderNo,
                    ExchangeID = "", // ?
                    RefExchangeID = request.RefExchangeID, // ?
                    SeqNum = 0,  // khi nào sở về mới update
                    Symbol = request.Symbol,
                    Side = request.Side,
                    Price = 0,
                    OrderQty = request.OrderValue,
                    QuoteType = request.QuoteType,
                    CrossType = "", // ?
                    ClientID = "",
                    ClientIDCounterFirm = "", // ?
                    MemberCounterFirm = "", // ?
                    OrderType = request.OrderType,
                };
                newInquiryRepos.ApiOrderNo = request.OrderNo;
                OrderMemory.Add_NewOrder(objOrder);
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newInquiryRepos);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API14ReplaceInquiryRepos OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API14ReplaceInquiryRepos OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API14ReplaceInquiryRepos_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        ///  2.3	API15 – Hủy lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
        /// </summary>
        public async Task<API15CancelInquiryReposResponse> API15CancelInquiryRepos_BU(API15CancelInquiryReposRequest request, API15CancelInquiryReposResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API15CancelInquiryRepos_BU with OrderNo: {request.OrderNo} ID {ID}");
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
                OrderInfo objOrder_RefExchangeID = OrderMemory.GetOrder_byExchangeID(request.RefExchangeID);
                if (objOrder_RefExchangeID != null)
                {
                    if (objOrder_RefExchangeID.OrderNo != request.OrderNo)
                    {
                        _response.ReturnCode = ORDER_RETURNCODE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                        _response.Message = ORDER_RETURNMESSAGE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                        _response.InData = null;
                        return _response;
                    }
                }
                string _ExOrderNo = Utils.GenOrderNo();
                // 35= N01
                MessageReposInquiry newInquiryRepos = new MessageReposInquiry() { TimeInit = DateTime.Now.Ticks };
                newInquiryRepos.APIBussiness = ORDER_API.API_15;
                newInquiryRepos.ClOrdID = _ExOrderNo;
                newInquiryRepos.RFQReqID = request.RefExchangeID;
                newInquiryRepos.Symbol = request.Symbol;
                newInquiryRepos.QuoteType = request.QuoteType;
                newInquiryRepos.OrdType = request.OrderType;
                //
                newInquiryRepos.Side = 0;
                newInquiryRepos.OrderQty = 0;
                newInquiryRepos.EffectiveTime = "";
                newInquiryRepos.SettlMethod = 0;
                newInquiryRepos.SettlDate = "";
                newInquiryRepos.SettlDate2 = "";
                newInquiryRepos.EndDate = "";
                newInquiryRepos.RepurchaseTerm = 0;
                newInquiryRepos.RegistID = "";
                //
                newInquiryRepos.Text = request.Text;
                newInquiryRepos.IsAPI15_16 = true;
                // Add thông tin order vào memory
                LocalMemory.OrderInfo objOrder = new OrderInfo()
                {
                    RefMsgType = MessageType.ReposInquiry,
                    OrderNo = request.OrderNo,
                    ClOrdID = newInquiryRepos.ClOrdID,
                    ExchangeID = "", // ?
                    RefExchangeID = request.RefExchangeID, // ?
                    SeqNum = 0,  // khi nào sở về mới update
                    Symbol = request.Symbol,
                    Side = "",
                    Price = 0,
                    OrderQty = 0,
                    QuoteType = request.QuoteType,
                    CrossType = "", // ?
                    ClientID = "",
                    ClientIDCounterFirm = "", // ?
                    MemberCounterFirm = "", // ?
                    OrderType = request.OrderType
                };
                newInquiryRepos.ApiOrderNo = request.OrderNo;

                OrderMemory.Add_NewOrder(objOrder);
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newInquiryRepos);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API15CancelInquiryRepos OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API15CancelInquiryRepos OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API15CancelInquiryRepos_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        ///  2.4	API16 – Đóng lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
        /// </summary>
        public async Task<API16CloseInquiryReposResponse> API16CloseInquiryRepos_BU(API16CloseInquiryReposRequest request, API16CloseInquiryReposResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API16CloseInquiryRepos_BU with OrderNo: {request.OrderNo} ID {ID}");
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
                OrderInfo objOrder_RefExchangeID = OrderMemory.GetOrder_byExchangeID(request.RefExchangeID);
                if (objOrder_RefExchangeID != null)
                {
                    if (objOrder_RefExchangeID.OrderNo != request.OrderNo)
                    {
                        _response.ReturnCode = ORDER_RETURNCODE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                        _response.Message = ORDER_RETURNMESSAGE.INVALID_ORIGIN_ORDERNO_REFEXCHANGEID;
                        _response.InData = null;
                        return _response;
                    }
                }
                string _ExOrderNo = Utils.GenOrderNo();
                // 35= N01
                MessageReposInquiry newInquiryRepos = new MessageReposInquiry() { TimeInit = DateTime.Now.Ticks };
                newInquiryRepos.APIBussiness = ORDER_API.API_16;
                newInquiryRepos.ClOrdID = _ExOrderNo;
                newInquiryRepos.RFQReqID = request.RefExchangeID;
                newInquiryRepos.Symbol = request.Symbol;
                newInquiryRepos.QuoteType = request.QuoteType;
                newInquiryRepos.OrdType = request.OrderType;
                //
                newInquiryRepos.Side = 0;
                newInquiryRepos.OrderQty = 0;
                newInquiryRepos.EffectiveTime = "";
                newInquiryRepos.SettlMethod = 0;
                newInquiryRepos.SettlDate = "";
                newInquiryRepos.SettlDate2 = "";
                newInquiryRepos.EndDate = "";
                newInquiryRepos.RepurchaseTerm = 0;
                newInquiryRepos.RegistID = "";
                //
                newInquiryRepos.Text = request.Text;
                newInquiryRepos.IsAPI15_16 = true;

                // Add thông tin order vào memory
                LocalMemory.OrderInfo objOrder = new OrderInfo()
                {
                    RefMsgType = MessageType.ReposInquiry,
                    OrderNo = request.OrderNo,
                    ClOrdID = newInquiryRepos.ClOrdID,
                    ExchangeID = "", // ?
                    RefExchangeID = request.RefExchangeID, // ?
                    SeqNum = 0,  // khi nào sở về mới update
                    Symbol = request.Symbol,
                    Side = "",
                    Price = 0,
                    OrderQty = 0,
                    QuoteType = request.QuoteType,
                    CrossType = "", // ?
                    ClientID = "",
                    ClientIDCounterFirm = "", // ?
                    MemberCounterFirm = "", // ?
                    OrderType = request.OrderType
                };
                newInquiryRepos.ApiOrderNo = request.OrderNo;
                OrderMemory.Add_NewOrder(objOrder);
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newInquiryRepos);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API16CloseInquiryRepos OrderNo {0} fail because queue is full", request.OrderNo);
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API16CloseInquiryRepos OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API16CloseInquiryRepos_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 2.5	API17 – Đặt lệnh điện tử tùy chọn Firm Repos
        /// </summary>
        public async Task<API17OrderNewFirmReposResponse> API17OrderNewFirmRepos_BU(API17OrderNewFirmReposRequest request, API17OrderNewFirmReposResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API17OrderNewFirmRepos_BU with OrderNo: {request.OrderNo} ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";
                bool resultTradingRule = false;
                List<APIReposSideList> _ReposSideList = request.SymbolFirmInfo;
                if (_ReposSideList != null)
                {
                    for (int i = 0; i < _ReposSideList.Count; i++)
                    {
                        APIReposSideList objReposSide = _ReposSideList[i];
                        if (string.IsNullOrEmpty(objReposSide.Symbol))
                        {
                            resultTradingRule = TradingRuleData.CheckTradingSessionWithSymbolNull(out _responseText, out _responseCode);
                        }
                        else
                        {
                            resultTradingRule = TradingRuleData.CheckTradingRule_Input(objReposSide.Symbol, out _responseText, out _responseCode);
                        }

                        if (!resultTradingRule)
                        {
                            _response.ReturnCode = _responseCode;
                            _response.Message = _responseText;
                            _response.InData = null;
                            return _response;
                        }
                    }
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
                // 35= N03
                MessageReposFirm newFirmRepos = new MessageReposFirm() { TimeInit = DateTime.Now.Ticks };
                newFirmRepos.APIBussiness = ORDER_API.API_17;
                newFirmRepos.ClOrdID = request.OrderNo;
                newFirmRepos.RFQReqID = request.RefExchangeID;
                newFirmRepos.QuoteType = request.QuoteType;
                newFirmRepos.OrdType = request.OrderType;
                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newFirmRepos.Side = CORE_OrderSide.SIDE_BUY;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newFirmRepos.Side = CORE_OrderSide.SIDE_SELL;
                }
                newFirmRepos.Account = request.ClientID;
                newFirmRepos.EffectiveTime = request.EffectiveTime;
                newFirmRepos.SettlMethod = request.SettleMethod;
                newFirmRepos.SettlDate = request.SettleDate1;
                newFirmRepos.SettlDate2 = request.SettleDate2;
                newFirmRepos.EndDate = request.EndDate;
                newFirmRepos.RepurchaseTerm = request.RepurchaseTerm;
                newFirmRepos.RepurchaseRate = request.RepurchaseRate;
                newFirmRepos.Text = request.Text;
                newFirmRepos.NoSide = request.NoSide;
                //
                List<SymbolFirmObject> listSymbolFirmMem = null;
                if (_ReposSideList != null)
                {
                    listSymbolFirmMem = new List<SymbolFirmObject>();
                    for (int i = 0; i < _ReposSideList.Count; i++)
                    {
                        APIReposSideList objReposSide = _ReposSideList[i];
                        //
                        ReposSide _reposSide = new ReposSide();
                        _reposSide.NumSide = objReposSide.NumSide;
                        _reposSide.Symbol = objReposSide.Symbol;
                        _reposSide.OrderQty = objReposSide.OrderQty;
                        _reposSide.Price = objReposSide.MergePrice;
                        _reposSide.HedgeRate = objReposSide.HedgeRate;
                        newFirmRepos.RepoSideList.Add(_reposSide);
                        //
                        SymbolFirmObject symbolFirmMem = new SymbolFirmObject();
                        symbolFirmMem.NumSide = objReposSide.NumSide;
                        symbolFirmMem.Symbol = objReposSide.Symbol;
                        symbolFirmMem.OrderQty = objReposSide.OrderQty;
                        symbolFirmMem.HedgeRate = objReposSide.HedgeRate;
                        symbolFirmMem.MergePrice = objReposSide.MergePrice;
                        listSymbolFirmMem.Add(symbolFirmMem);
                    }
                }

                // Add thông tin order vào memory
                LocalMemory.OrderInfo objOrder = new OrderInfo()
                {
                    RefMsgType = MessageType.ReposFirm,
                    OrderNo = request.OrderNo,
                    ClOrdID = newFirmRepos.ClOrdID,
                    ExchangeID = "",
                    RefExchangeID = request.RefExchangeID,
                    SeqNum = 0,  // khi nào sở về mới update
                    Symbol = "",
                    Side = request.Side,
                    Price = 0,
                    OrderQty = 0,
                    QuoteType = request.QuoteType,
                    CrossType = "",
                    ClientID = request.ClientID,
                    ClientIDCounterFirm = "",
                    MemberCounterFirm = "",
                    SettlMethod = request.SettleMethod,
                    SettleDate1 = request.SettleDate1,
                    SettleDate2 = request.SettleDate2,
                    EndDate = request.EndDate,
                    RepurchaseTerm = request.RepurchaseTerm,
                    RepurchaseRate = request.RepurchaseRate,
                    NoSide = request.NoSide,
                    OrderType = request.OrderType,
                    //
                    SymbolFirmInfo = listSymbolFirmMem
                };
                newFirmRepos.ApiOrderNo = request.OrderNo;
                OrderMemory.Add_NewOrder(objOrder);
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newFirmRepos);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API17OrderNewFirmRepos OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API17OrderNewFirmRepos OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API17OrderNewFirmRepos_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 2.6	API18 – Sửa lệnh điện tử tùy chọn Firm Repos chưa thực hiện
        /// </summary>
        public async Task<API18OrderReplaceFirmReposResponse> API18OrderReplaceFirmRepos_BU(API18OrderReplaceFirmReposRequest request, API18OrderReplaceFirmReposResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API18OrderReplaceFirmRepos_BU with OrderNo: {request.OrderNo} ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";
                bool resultTradingRule = false;
                List<APIReposSideList> _ReposSideList = request.SymbolFirmInfo;
                if (_ReposSideList != null)
                {
                    for (int i = 0; i < _ReposSideList.Count; i++)
                    {
                        APIReposSideList objReposSide = _ReposSideList[i];
                        if (string.IsNullOrEmpty(objReposSide.Symbol))
                        {
                            resultTradingRule = TradingRuleData.CheckTradingSessionWithSymbolNull(out _responseText, out _responseCode);
                        }
                        else
                        {
                            resultTradingRule = TradingRuleData.CheckTradingRule_Input(objReposSide.Symbol, out _responseText, out _responseCode);
                        }

                        if (!resultTradingRule)
                        {
                            _response.ReturnCode = _responseCode;
                            _response.Message = _responseText;
                            _response.InData = null;
                            return _response;
                        }
                    }
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
                // 35= N03
                MessageReposFirm newFirmRepos = new MessageReposFirm() { TimeInit = DateTime.Now.Ticks };
                newFirmRepos.APIBussiness = ORDER_API.API_18;
                newFirmRepos.ClOrdID = _ExOrderNo;
                newFirmRepos.RFQReqID = request.RefExchangeID;
                newFirmRepos.QuoteType = request.QuoteType;
                newFirmRepos.OrdType = request.OrderType;
                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newFirmRepos.Side = CORE_OrderSide.SIDE_BUY;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newFirmRepos.Side = CORE_OrderSide.SIDE_SELL;
                }
                newFirmRepos.Account = request.ClientID;
                newFirmRepos.EffectiveTime = request.EffectiveTime;
                newFirmRepos.SettlMethod = request.SettleMethod;
                newFirmRepos.SettlDate = request.SettleDate1;
                newFirmRepos.SettlDate2 = request.SettleDate2;
                newFirmRepos.EndDate = request.EndDate;
                newFirmRepos.RepurchaseTerm = request.RepurchaseTerm;
                newFirmRepos.RepurchaseRate = request.RepurchaseRate;
                newFirmRepos.Text = request.Text;
                newFirmRepos.NoSide = request.NoSide;
                //
                List<SymbolFirmObject> listSymbolFirmMem = null;
                if (_ReposSideList != null)
                {
                    listSymbolFirmMem = new List<SymbolFirmObject>();
                    for (int i = 0; i < _ReposSideList.Count; i++)
                    {
                        APIReposSideList objReposSide = _ReposSideList[i];
                        //
                        ReposSide _reposSide = new ReposSide();
                        _reposSide.NumSide = objReposSide.NumSide;
                        _reposSide.Symbol = objReposSide.Symbol;
                        _reposSide.OrderQty = objReposSide.OrderQty;
                        _reposSide.Price = objReposSide.MergePrice;
                        _reposSide.HedgeRate = objReposSide.HedgeRate;
                        newFirmRepos.RepoSideList.Add(_reposSide);
                        //
                        SymbolFirmObject symbolFirmMem = new SymbolFirmObject();
                        symbolFirmMem.NumSide = objReposSide.NumSide;
                        symbolFirmMem.Symbol = objReposSide.Symbol;
                        symbolFirmMem.OrderQty = objReposSide.OrderQty;
                        symbolFirmMem.HedgeRate = objReposSide.HedgeRate;
                        symbolFirmMem.MergePrice = objReposSide.MergePrice;
                        listSymbolFirmMem.Add(symbolFirmMem);
                    }
                }

                // Add thông tin order vào memory
                LocalMemory.OrderInfo objOrder = new OrderInfo()
                {
                    RefMsgType = MessageType.ReposFirm,
                    OrderNo = request.OrderNo,
                    ClOrdID = newFirmRepos.ClOrdID,
                    ExchangeID = "",
                    RefExchangeID = request.RefExchangeID,
                    SeqNum = 0,  // khi nào sở về mới update
                    Symbol = "",
                    Side = request.Side,
                    Price = 0,
                    OrderQty = 0,
                    QuoteType = request.QuoteType,
                    CrossType = "",
                    ClientID = request.ClientID,
                    ClientIDCounterFirm = "",
                    MemberCounterFirm = "",
                    NoSide = request.NoSide,
                    SettlMethod = request.SettleMethod,
                    SettleDate1 = request.SettleDate1,
                    SettleDate2 = request.SettleDate2,
                    EndDate = request.EndDate,
                    RepurchaseTerm = request.RepurchaseTerm,
                    RepurchaseRate = request.RepurchaseRate,
                    OrderType = request.OrderType,
                    //
                    SymbolFirmInfo = listSymbolFirmMem
                };
                newFirmRepos.ApiOrderNo = request.OrderNo;
                OrderMemory.Add_NewOrder(objOrder);
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newFirmRepos);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API18OrderReplaceFirmRepos OrderNo {0} Success", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API18OrderReplaceFirmRepos OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API18OrderReplaceFirmRepos_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 2.7	API19 – Hủy lệnh điện tử tùy chọn Firm Repos chưa thực hiện
        /// </summary>
        public async Task<API19OrderCancelFirmReposResponse> API19OrderCancelFirmRepos_BU(API19OrderCancelFirmReposRequest request, API19OrderCancelFirmReposResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API19OrderCancelFirmRepos_BU with OrderNo: {request.OrderNo} ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";
                bool resultTradingRule = false;
                resultTradingRule = TradingRuleData.CheckTradingSessionWithSymbolNull(out _responseText, out _responseCode);

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

                //
                string _ExOrderNo = Utils.GenOrderNo();
                // 35= N03
                MessageReposFirm newFirmRepos = new MessageReposFirm() { TimeInit = DateTime.Now.Ticks };
                newFirmRepos.APIBussiness = ORDER_API.API_19;
                newFirmRepos.ClOrdID = _ExOrderNo;
                newFirmRepos.RFQReqID = request.RefExchangeID;
                newFirmRepos.QuoteType = request.QuoteType;
                newFirmRepos.OrdType = request.OrderType;
                newFirmRepos.Account = "";
                newFirmRepos.Text = request.Text;
                //
                newFirmRepos.Side = 0;
                newFirmRepos.Account = "";
                newFirmRepos.EffectiveTime = "00010101";
                newFirmRepos.SettlMethod = 0;
                newFirmRepos.SettlDate = "00010101";
                newFirmRepos.SettlDate2 = "00010101";
                newFirmRepos.EndDate = "00010101";
                newFirmRepos.RepurchaseTerm = 0;
                newFirmRepos.RepurchaseRate = 0;
                newFirmRepos.NoSide = 0;

                ReposSide _reposSide = new ReposSide();
                _reposSide.NumSide = 0;
                _reposSide.Symbol = "";
                _reposSide.OrderQty = 0;
                _reposSide.Price = 0;
                _reposSide.HedgeRate = 0;
                _reposSide.ReposInterest = 0;
                _reposSide.SettlValue = 0;
                _reposSide.SettlValue2 = 0;
                //
                newFirmRepos.RepoSideList.Add(_reposSide);

                // Add thông tin order vào memory
                LocalMemory.OrderInfo objOrder = new OrderInfo()
                {
                    RefMsgType = MessageType.ReposFirm,
                    OrderNo = request.OrderNo,
                    ClOrdID = newFirmRepos.ClOrdID,
                    ExchangeID = "", // ?
                    RefExchangeID = request.RefExchangeID, // ?
                    SeqNum = 0,  // khi nào sở về mới update
                    Symbol = "",
                    Side = "",
                    Price = 0,
                    OrderQty = 0,
                    QuoteType = request.QuoteType,
                    CrossType = "", // ?
                    ClientID = "",
                    ClientIDCounterFirm = "", // ?
                    MemberCounterFirm = "", // ?
                    OrderType = request.OrderType
                };
                newFirmRepos.ApiOrderNo = request.OrderNo;
                OrderMemory.Add_NewOrder(objOrder);
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newFirmRepos);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API19OrderCancelFirmRepos OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API19OrderCancelFirmRepos OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API19OrderCancelFirmRepos_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        ///  2.8	API20 – Xác nhận lệnh điện tử tùy chọn Firm Repos
        /// </summary>
        public async Task<API20OrderConfirmFirmReposResponse> API20OrderConfirmFirmRepos_BU(API20OrderConfirmFirmReposRequest request, API20OrderConfirmFirmReposResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API20OrderConfirmFirmRepos_BU with OrderNo: {request.OrderNo} ID {ID}");
				//
				string _responseText = "";
				string _responseCode = "";
				bool resultTradingRule = false;
				List<APIReposSideList> _ReposSideList = request.SymbolFirmInfo;
				if (_ReposSideList != null)
				{
					for (int i = 0; i < _ReposSideList.Count; i++)
					{
						APIReposSideList objReposSide = _ReposSideList[i];
						if (string.IsNullOrEmpty(objReposSide.Symbol))
						{
							resultTradingRule = TradingRuleData.CheckTradingSessionWithSymbolNull(out _responseText, out _responseCode);
						}
						else
						{
							resultTradingRule = TradingRuleData.CheckTradingRule_Input(objReposSide.Symbol, out _responseText, out _responseCode);
						}

						if (!resultTradingRule)
						{
							_response.ReturnCode = _responseCode;
							_response.Message = _responseText;
							_response.InData = null;
							return _response;
						}
					}
				}

				string _ExOrderNo = Utils.GenOrderNo();
                // 35= N05
                MessageReposFirmAccept newFirmRepos = new MessageReposFirmAccept() { TimeInit = DateTime.Now.Ticks };
                newFirmRepos.APIBussiness = ORDER_API.API_20;
                newFirmRepos.ClOrdID = _ExOrderNo;
                newFirmRepos.RFQReqID = request.RefExchangeID;
                newFirmRepos.QuoteType = request.QuoteType;
                newFirmRepos.OrdType = request.OrderType;
                newFirmRepos.Account = request.ClientID;
                newFirmRepos.CoAccount = request.ClientIDCounterFirm;
                newFirmRepos.Text = request.Text;
				newFirmRepos.RepurchaseRate = request.RepurchaseRate;
				newFirmRepos.NoSide = request.NoSide;
				//
				List<SymbolFirmObject> listSymbolFirmMem = null;
				if (_ReposSideList != null)
				{
					listSymbolFirmMem = new List<SymbolFirmObject>();
					for (int i = 0; i < _ReposSideList.Count; i++)
					{
						APIReposSideList objReposSide = _ReposSideList[i];
						//
						ReposSide _reposSide = new ReposSide();
						_reposSide.NumSide = objReposSide.NumSide;
						_reposSide.Symbol = objReposSide.Symbol;
						_reposSide.OrderQty = objReposSide.OrderQty;
						_reposSide.Price = objReposSide.MergePrice;
						_reposSide.HedgeRate = objReposSide.HedgeRate;
						newFirmRepos.RepoSideList.Add(_reposSide);
						//
						SymbolFirmObject symbolFirmMem = new SymbolFirmObject();
						symbolFirmMem.NumSide = objReposSide.NumSide;
						symbolFirmMem.Symbol = objReposSide.Symbol;
						symbolFirmMem.OrderQty = objReposSide.OrderQty;
						symbolFirmMem.HedgeRate = objReposSide.HedgeRate;
						symbolFirmMem.MergePrice = objReposSide.MergePrice;
						listSymbolFirmMem.Add(symbolFirmMem);
					}
				}

				//
				/*
               "Lấy giá trị refExchangeID gửi lên so với giá trị exchangeID  trong mem và  refMsgType =N03 hoặc refMsgType =N05 trong mem:
                    + Nếu tìm thấy bản ghi:
                               . case 1: Nếu chỉ tìm thấy 1 bản ghi có orderno # rỗng và refMsgType =N03 (xảy ra khi gửi xác nhận lần đầu của lệnh RE)--> insert bản ghi mới  vào mem (có các thông tin orderNo, refMsg Type, exchangeID= refExchangeID, refExchangeID,  side, quoteType, orderType, clientID)  ///bản ghi tìm đc thuộc case mình đặt firm dựa trên in đã đặt
                               . case 2: Nếu chỉ tìm thấy 1 bản ghi  orderno = rỗng và refMsgType =N03--> cập nhật orderNo, refexchangeID, refMsgType, side, quoteType, clientID ///bản ghi tìm đc thuộc case nhận FIRM dựa trên IN từ thành viên khác đặt sang
                               . case 3: Nếu tìm thấy 2 bản ghi trong đó 1 bản ghi có orderno # rỗng,  refMsgType =N03 và 1 bản ghi có orderNo # rỗng, refExchangeID = exchangeID, refMsgType =N05-->cập nhật vào bản ghi có refMsgType =N05 trong mem các thông tin orderNo, refexchangeID, refMsgType, side, quoteType, clientID ///bản ghi tìm đc thuộc case 1 và đặt N05 lên bị reject thực hiện lại
                     . case 3.1: Nếu chỉ tìm thấy 1 bản ghi có orderNo # rỗng,, refMsgType =N05-->cập nhật vào bản ghi có refMsgType =N05 trong mem các thông tin orderNo, refexchangeID, refMsgType, side, quoteType, clientID ///bản ghi tìm đc thuộc case 2 và đặt N05 lên bị reject thực hiện lại
                               case 4: Nếu tìm thấy hơn 2 bản ghi  hoặc tìm thấy 2 bản ghi có điều kiện khác case 3 cần ghi lỗi
                    + Nếu không tìm thấy bản ghi: --> insert bản ghi mới có có các thông tin orderNo, refMsgType,refExchangeID, symbol, side, price, orderqty, clientid, orderType)"
                */
				string p_Side = "";
                OrderInfo objOrder_RefExchangeID = OrderMemory.GetOrder_byExchangeID(request.RefExchangeID);
                if (objOrder_RefExchangeID != null)
                {
                    if (objOrder_RefExchangeID.Side == ORDER_SIDE.SIDE_BUY)
                    {
                        p_Side = ORDER_SIDE.SIDE_SELL;
                    }

                    if (objOrder_RefExchangeID.Side == ORDER_SIDE.SIDE_SELL)
                    {
                        p_Side = ORDER_SIDE.SIDE_BUY;
                    }
                }
                //
                List<OrderInfo> lstOrderMem = OrderMemory.GetListOrder_byExchangeID_RefMsgType(request.RefExchangeID);
                if (lstOrderMem != null && lstOrderMem.Count>0)
                {
                    if (lstOrderMem.Count == 1)
                    {
                        OrderInfo objOrder = lstOrderMem[0];
                        //  . case 1: Nếu chỉ tìm thấy 1 bản ghi có orderno # rỗng và refMsgType =N03 (xảy ra khi gửi xác nhận lần đầu của lệnh RE)--> insert bản ghi mới  vào mem (có các thông tin orderNo, refMsg Type, exchangeID= refExchangeID, refExchangeID,  side, quoteType, orderType, clientID)  ///bản ghi tìm đc thuộc case mình đặt firm dựa trên in đã đặt
                        if (!string.IsNullOrEmpty(objOrder.OrderNo) && objOrder.RefMsgType == MessageType.ReposFirm)
                        {
                            LocalMemory.OrderInfo objOrderMem = new OrderInfo()
                            {
                                RefMsgType = MessageType.ReposFirmAccept,
                                OrderNo = request.OrderNo,
                                ClOrdID = newFirmRepos.ClOrdID,
                                ExchangeID = request.RefExchangeID,
                                RefExchangeID = request.RefExchangeID,
                                SeqNum = 0,  // khi nào sở về mới update
                                Symbol = "",
                                Side = p_Side,
                                Price = 0,
                                OrderQty = 0,
                                QuoteType = request.QuoteType,
                                CrossType = "", // ?
                                ClientID = request.ClientID,
                                ClientIDCounterFirm = "", // ?
                                MemberCounterFirm = "", // ?
                                OrderType = request.OrderType,
                                NoSide=request.NoSide,
								 //
					            SymbolFirmInfo = listSymbolFirmMem
							};
                            OrderMemory.Add_NewOrder(objOrderMem);
                        }
                        //  . case 2: Nếu chỉ tìm thấy 1 bản ghi  orderno = rỗng và refMsgType =N03--> cập nhật orderNo, refexchangeID, refMsgType, side, quoteType, clientID ///bản ghi tìm đc thuộc case nhận FIRM dựa trên IN từ thành viên khác đặt sang
                        else if (string.IsNullOrEmpty(objOrder.OrderNo) && objOrder.RefMsgType == MessageType.ReposFirm)
                        {
                            OrderMemory.UpdateOrderInfo(objOrder, p_OrderNo: request.OrderNo, p_RefExchangeID: request.RefExchangeID, p_RefMsgType: MessageType.ReposFirmAccept,
                                                 p_Side: p_Side, p_QuoteType: request.QuoteType, p_ClientID: request.ClientID, p_ClOrdID: _ExOrderNo);
                            //
                            bool isUpdate = OrderMemory.UpdateMap_ClOrdID_Index(objOrder.ClOrdID);
                            if (!isUpdate)
                            {
                                Logger.ApiLog.Warn("Cannot Update map ClOrID - Index for Api 20 ClOrdID {0}", objOrder.ClOrdID);
                            }
                        }
                        // . case 3.1: Nếu chỉ tìm thấy 1 bản ghi có orderNo # rỗng,, refMsgType =N05-->cập nhật vào bản ghi có refMsgType =N05 trong mem các thông tin orderNo, refexchangeID, refMsgType, side, quoteType, clientID
                        else if (!string.IsNullOrEmpty(objOrder.OrderNo) && objOrder.RefMsgType == MessageType.ReposFirmAccept)
                        {
                            OrderMemory.UpdateOrderInfo(objOrder, p_OrderNo: request.OrderNo, p_RefExchangeID: request.RefExchangeID, p_RefMsgType: MessageType.ReposFirmAccept,
                                               p_Side: p_Side, p_QuoteType: request.QuoteType, p_ClientID: request.ClientID, p_ClOrdID: _ExOrderNo);
                            //
                            bool isUpdate = OrderMemory.UpdateMap_ClOrdID_Index(objOrder.ClOrdID);
                            if (!isUpdate)
                            {
                                Logger.ApiLog.Warn("Cannot Update map ClOrID - Index for Api 20 ClOrdID {0}", objOrder.ClOrdID);
                            }
                        }
                    }
                    else if (lstOrderMem.Count == 2)
                    {
                        bool checkN03 = false;
                        bool checkN05 = false;
                        OrderInfo objOrder = null;
                        for (int i = 0; i < lstOrderMem.Count; i++)
                        {
                            OrderInfo objOrderMem = lstOrderMem[i];
                            if (!string.IsNullOrEmpty(objOrderMem.OrderNo) && objOrderMem.RefMsgType == MessageType.ReposFirm)
                            {
								checkN03 = true;
                            }
                            if (!string.IsNullOrEmpty(objOrderMem.OrderNo) && objOrderMem.RefMsgType == MessageType.ReposFirmAccept)
                            {
                                objOrder = objOrderMem;
                                checkN05 = true;
                            }
                        }
                        // . case 3: Nếu tìm thấy 2 bản ghi trong đó 1 bản ghi có orderno # rỗng,  refMsgType =N03 và 1 bản ghi có orderNo # rỗng, refExchangeID = exchangeID, refMsgType =N05-->cập nhật vào bản ghi có refMsgType =N05 trong mem các thông tin orderNo, refexchangeID, refMsgType, side, quoteType, clientID
                        if (checkN03 && checkN05 && objOrder != null)
                        {
                            OrderMemory.UpdateOrderInfo(objOrder, p_OrderNo: request.OrderNo, p_RefExchangeID: request.RefExchangeID, p_RefMsgType: MessageType.ReposFirmAccept,
                                               p_Side: p_Side, p_QuoteType: request.QuoteType, p_ClientID: request.ClientID, p_ClOrdID: _ExOrderNo);
                            //
                            bool isUpdate = OrderMemory.UpdateMap_ClOrdID_Index(objOrder.ClOrdID);
                            if (!isUpdate)
                            {
                                Logger.ApiLog.Warn("Cannot Update map ClOrID - Index for Api 20 ClOrdID {0}", objOrder.ClOrdID);
                            }
                        }
                        else
                        {
                            // . case 4: Nếu tìm thấy hơn 2 bản ghi  hoặc tìm thấy 2 bản ghi có điều kiện khác case 3 cần ghi lỗi
                            Logger.ApiLog.Warn($"Case 3 conditions are not match => case 4 please check again.");
                        }
                    }
                }
                else //  + Nếu không tìm thấy bản ghi: --> insert bản ghi mới có có các thông tin orderNo, refMsgType,refExchangeID, symbol, side, price, orderqty, clientid, orderType)"
                {
                    // Add thông tin order vào memory
                    LocalMemory.OrderInfo objOrder = new OrderInfo()
                    {
                        RefMsgType = MessageType.ReposFirmAccept,
                        OrderNo = request.OrderNo,
                        ClOrdID = newFirmRepos.ClOrdID,
                        ExchangeID = request.RefExchangeID,
                        RefExchangeID = request.RefExchangeID,
                        SeqNum = 0,  // khi nào sở về mới update
                        Symbol = "",
                        Side = p_Side,
                        Price = 0,
                        OrderQty = 0,
                        QuoteType = request.QuoteType,
                        CrossType = "", // ?
                        ClientID = request.ClientID,
                        ClientIDCounterFirm = "", // ?
                        MemberCounterFirm = "", // ?
                        OrderType = request.OrderType,
						NoSide = request.NoSide,
						//
						SymbolFirmInfo = listSymbolFirmMem
					};
                    OrderMemory.Add_NewOrder(objOrder);
                }

                //
                newFirmRepos.ApiOrderNo = request.OrderNo;
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newFirmRepos);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API20OrderConfirmFirmRepos OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API20OrderConfirmFirmRepos OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API20OrderConfirmFirmRepos_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 2.9	API21 – Đặt lệnh thỏa thuận báo cáo giao dịch Repos
        /// </summary>
        public async Task<API21OrderNewReposCommonPutthroughResponse> API21OrderNewReposCommonPutthough_BU(API21OrderNewReposCommonPutthroughRequest request, API21OrderNewReposCommonPutthroughResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API21OrderNewReposCommonPutthough_BU with OrderNo: {request.OrderNo} ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";
                bool resultTradingRule = false;
                List<APIReposSideList> _ReposSideList = request.SymbolFirmInfo;
                if (_ReposSideList != null)
                {
                    for (int i = 0; i < _ReposSideList.Count; i++)
                    {
                        APIReposSideList objReposSide = _ReposSideList[i];
                        if (string.IsNullOrEmpty(objReposSide.Symbol))
                        {
                            resultTradingRule = TradingRuleData.CheckTradingSessionWithSymbolNull(out _responseText, out _responseCode);
                        }
                        else
                        {
                            resultTradingRule = TradingRuleData.CheckTradingRule_Input(objReposSide.Symbol, out _responseText, out _responseCode);
                        }

                        if (!resultTradingRule)
                        {
                            _response.ReturnCode = _responseCode;
                            _response.Message = _responseText;
                            _response.InData = null;
                            return _response;
                        }
                    }
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
                // 35= MA
                MessageReposBCGD newReposBCGD = new MessageReposBCGD() { TimeInit = DateTime.Now.Ticks };
                newReposBCGD.APIBussiness = ORDER_API.API_21;
                newReposBCGD.ClOrdID = request.OrderNo;
                newReposBCGD.QuoteType = request.QuoteType;
                newReposBCGD.OrdType = request.OrderType;
                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newReposBCGD.Side = CORE_OrderSide.SIDE_BUY;
                    newReposBCGD.CoAccount = request.ClientID;
                    newReposBCGD.Account = request.ClientIDCounterFirm;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newReposBCGD.Side = CORE_OrderSide.SIDE_SELL;
                    newReposBCGD.Account = request.ClientID;
                    newReposBCGD.CoAccount = request.ClientIDCounterFirm;
                }
                //
                if (request.MemberCounterFirm == ConfigData.FirmID)
                {
                    newReposBCGD.PartyID = ConfigData.FirmID;
                    newReposBCGD.CoPartyID = ConfigData.FirmID;
                }
                else if (request.MemberCounterFirm != ConfigData.FirmID)
                {
                    if (request.Side == ORDER_SIDE.SIDE_BUY)
                    {
                        newReposBCGD.PartyID = request.MemberCounterFirm;
                        newReposBCGD.CoPartyID = ConfigData.FirmID;
                    }
                    else if (request.Side == ORDER_SIDE.SIDE_SELL)
                    {
                        newReposBCGD.PartyID = ConfigData.FirmID; ;
                        newReposBCGD.CoPartyID = request.MemberCounterFirm;
                    }
                }

                newReposBCGD.EffectiveTime = request.EffectiveTime;
                newReposBCGD.SettlMethod = request.SettleMethod;
                newReposBCGD.SettlDate = request.SettleDate1;
                newReposBCGD.SettlDate2 = request.SettleDate2;
                newReposBCGD.EndDate = request.EndDate;
                newReposBCGD.RepurchaseTerm = request.RepurchaseTerm;
                newReposBCGD.RepurchaseRate = request.RepurchaseRate;
                newReposBCGD.Text = request.Text;
                newReposBCGD.NoSide = request.NoSide;
                //
                List<SymbolFirmObject> listSymbolFirmMem = null;
                if (_ReposSideList != null)
                {
                    listSymbolFirmMem = new List<SymbolFirmObject>();
                    for (int i = 0; i < _ReposSideList.Count; i++)
                    {
                        APIReposSideList objReposSide = _ReposSideList[i];
                        //
                        ReposSide _reposSide = new ReposSide();
                        _reposSide.NumSide = objReposSide.NumSide;
                        _reposSide.Symbol = objReposSide.Symbol;
                        _reposSide.OrderQty = objReposSide.OrderQty;
                        _reposSide.Price = objReposSide.MergePrice;
                        _reposSide.HedgeRate = objReposSide.HedgeRate;
                        newReposBCGD.RepoSideList.Add(_reposSide);
                        //
                        SymbolFirmObject symbolFirmMem = new SymbolFirmObject();
                        symbolFirmMem.NumSide = objReposSide.NumSide;
                        symbolFirmMem.Symbol = objReposSide.Symbol;
                        symbolFirmMem.OrderQty = objReposSide.OrderQty;
                        symbolFirmMem.HedgeRate = objReposSide.HedgeRate;
                        symbolFirmMem.MergePrice = objReposSide.MergePrice;
                        listSymbolFirmMem.Add(symbolFirmMem);
                    }
                }

                // Add thông tin order vào memory
                LocalMemory.OrderInfo objOrder = new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGD,
                    OrderNo = request.OrderNo,
                    ClOrdID = newReposBCGD.ClOrdID,
                    ExchangeID = "", // ?
                    RefExchangeID = "", // ?
                    SeqNum = 0,  // khi nào sở về mới update
                    Symbol = "",
                    Side = request.Side,
                    Price = 0,
                    OrderQty = 0,
                    QuoteType = request.QuoteType,
                    CrossType = "", // ?
                    ClientID = request.ClientID,
                    ClientIDCounterFirm = "", // ?
                    MemberCounterFirm = "", // ?
                    NoSide = request.NoSide,
                    SettlMethod = request.SettleMethod,
                    SettleDate1 = request.SettleDate1,
                    SettleDate2 = request.SettleDate2,
                    EndDate = request.EndDate,
                    RepurchaseTerm = request.RepurchaseTerm,
                    RepurchaseRate = request.RepurchaseRate,
                    OrderType = request.OrderType,
                    //
                    SymbolFirmInfo = listSymbolFirmMem
                };
                newReposBCGD.ApiOrderNo = request.OrderNo;
                OrderMemory.Add_NewOrder(objOrder);
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newReposBCGD);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API21OrderNewReposCommonPutthough OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API21OrderNewReposCommonPutthough OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API21OrderNewReposCommonPutthough_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 2.10	API22 – Xác nhận lệnh thỏa thuận báo cáo giao dịch Repos
        /// </summary>
        public async Task<API22OrderConfirmReposCommonPutthroughResponse> API22OrderConfirmReposCommonPutthrough_BU(API22OrderConfirmReposCommonPutthroughRequest request, API22OrderConfirmReposCommonPutthroughResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API22OrderConfirmReposCommonPutthrough_BU with OrderNo: {request.OrderNo} ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";
                bool resultTradingRule = false;
                List<APIReposSideList> _ReposSideList = request.SymbolFirmInfo;
                if (_ReposSideList != null)
                {
                    for (int i = 0; i < _ReposSideList.Count; i++)
                    {
                        APIReposSideList objReposSide = _ReposSideList[i];
                        if (string.IsNullOrEmpty(objReposSide.Symbol))
                        {
                            resultTradingRule = TradingRuleData.CheckTradingSessionWithSymbolNull(out _responseText, out _responseCode);
                        }
                        else
                        {
                            resultTradingRule = TradingRuleData.CheckTradingRule_Input(objReposSide.Symbol, out _responseText, out _responseCode);
                        }

                        if (!resultTradingRule)
                        {
                            _response.ReturnCode = _responseCode;
                            _response.Message = _responseText;
                            _response.InData = null;
                            return _response;
                        }
                    }
                }

                string _ExOrderNo = Utils.GenOrderNo();
                // 35= MA
                MessageReposBCGD newReposBCGD = new MessageReposBCGD() { TimeInit = DateTime.Now.Ticks };
                newReposBCGD.APIBussiness = ORDER_API.API_22;
                newReposBCGD.ClOrdID = _ExOrderNo;
                newReposBCGD.OrgOrderID = request.RefExchangeID;
                newReposBCGD.QuoteType = request.QuoteType;
                newReposBCGD.OrdType = request.OrderType;
                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newReposBCGD.Side = CORE_OrderSide.SIDE_BUY;
                    newReposBCGD.CoAccount = request.ClientID;
                    newReposBCGD.Account = request.ClientIDCounterFirm;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newReposBCGD.Side = CORE_OrderSide.SIDE_SELL;
                    newReposBCGD.Account = request.ClientID;
                    newReposBCGD.CoAccount = request.ClientIDCounterFirm;
                }
                //
                if (request.MemberCounterFirm == ConfigData.FirmID)
                {
                    newReposBCGD.PartyID = ConfigData.FirmID;
                    newReposBCGD.CoPartyID = ConfigData.FirmID;
                }
                else if (request.MemberCounterFirm != ConfigData.FirmID)
                {
                    if (request.Side == ORDER_SIDE.SIDE_BUY)
                    {
                        newReposBCGD.PartyID = request.MemberCounterFirm;
                        newReposBCGD.CoPartyID = ConfigData.FirmID;
                    }
                    else if (request.Side == ORDER_SIDE.SIDE_SELL)
                    {
                        newReposBCGD.PartyID = ConfigData.FirmID; ;
                        newReposBCGD.CoPartyID = request.MemberCounterFirm;
                    }
                }
                newReposBCGD.EffectiveTime = request.EffectiveTime;
                newReposBCGD.SettlMethod = request.SettleMethod;
                newReposBCGD.SettlDate = request.SettleDate1;
                newReposBCGD.SettlDate2 = request.SettleDate2;
                newReposBCGD.EndDate = request.EndDate;
                newReposBCGD.RepurchaseTerm = request.RepurchaseTerm;
                newReposBCGD.RepurchaseRate = request.RepurchaseRate;
                newReposBCGD.Text = request.Text;
                newReposBCGD.NoSide = request.NoSide;
                //
                List<SymbolFirmObject> listSymbolFirmMem = null;
                if (_ReposSideList != null)
                {
                    listSymbolFirmMem = new List<SymbolFirmObject>();
                    for (int i = 0; i < _ReposSideList.Count; i++)
                    {
                        APIReposSideList objReposSide = _ReposSideList[i];
                        //
                        ReposSide _reposSide = new ReposSide();
                        _reposSide.NumSide = objReposSide.NumSide;
                        _reposSide.Symbol = objReposSide.Symbol;
                        _reposSide.OrderQty = objReposSide.OrderQty;
                        _reposSide.Price = objReposSide.MergePrice;
                        _reposSide.HedgeRate = objReposSide.HedgeRate;
                        newReposBCGD.RepoSideList.Add(_reposSide);
                        //
                        SymbolFirmObject symbolFirmMem = new SymbolFirmObject();
                        symbolFirmMem.NumSide = objReposSide.NumSide;
                        symbolFirmMem.Symbol = objReposSide.Symbol;
                        symbolFirmMem.OrderQty = objReposSide.OrderQty;
                        symbolFirmMem.HedgeRate = objReposSide.HedgeRate;
                        symbolFirmMem.MergePrice = objReposSide.MergePrice;
                        listSymbolFirmMem.Add(symbolFirmMem);
                    }
                }
                OrderInfo objOrder = OrderMemory.GetOrderBy(p_RefMsgType: MessageType.ReposBCGD, p_ExchangeID: request.RefExchangeID);
                if (objOrder != null)
                {
                    //Nếu tìm thấy bản ghi: cập nhật orderNo, refexchangeID, clientID
                    Logger.ApiLog.Info($"ProcessRevBussiness | GetOrder_byExchangeID with request_OrderNo: {request.OrderNo}, objOrder_OrderNo:{objOrder.OrderNo}, objOrder_RefMsgType:{objOrder.RefMsgType}");
                    //
                    OrderMemory.UpdateOrderInfo(objOrder, p_RefMsgType: MessageType.ReposBCGD, p_OrderNo: request.OrderNo, p_ClOrdID: _ExOrderNo, p_RefExchangeID: request.RefExchangeID, p_QuoteType: request.QuoteType);
                    //
                    bool isUpdate = OrderMemory.UpdateMap_ClOrdID_Index(objOrder.ClOrdID);
                    if (!isUpdate)
                    {
                        Logger.ApiLog.Warn($"Cannot Update map ClOrID - Index for Api {ORDER_API.API_22} ClOrdID {objOrder.ClOrdID}");
                    }
                }
                else
                {
                    // Add thông tin order vào memory
                    LocalMemory.OrderInfo objOrderMem = new OrderInfo()
                    {
                        RefMsgType = MessageType.ReposBCGD,
                        OrderNo = request.OrderNo,
                        ClOrdID = newReposBCGD.ClOrdID,
                        ExchangeID = "", // ?
                        RefExchangeID = request.RefExchangeID, // ?
                        SeqNum = 0,  // khi nào sở về mới update
                        Symbol = "",
                        Side = request.Side,
                        Price = 0,
                        OrderQty = 0,
                        QuoteType = request.QuoteType,
                        CrossType = "", // ?
                        ClientID = request.ClientID,
                        ClientIDCounterFirm = "", // ?
                        MemberCounterFirm = "", // ?
                        NoSide = request.NoSide,
                        SettlMethod = request.SettleMethod,
                        SettleDate1 = request.SettleDate1,
                        SettleDate2 = request.SettleDate2,
                        EndDate = request.EndDate,
                        RepurchaseTerm = request.RepurchaseTerm,
                        RepurchaseRate = request.RepurchaseRate,
                        OrderType = request.OrderType,
                        //
                        SymbolFirmInfo = listSymbolFirmMem
                    };
                    newReposBCGD.ApiOrderNo = request.OrderNo;
                    OrderMemory.Add_NewOrder(objOrderMem);
                }
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newReposBCGD);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API22OrderConfirmReposCommonPutthrough OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API22OrderConfirmReposCommonPutthrough OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API22OrderConfirmReposCommonPutthrough_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 2.11	API23 – Sửa lệnh thỏa thuận báo cáo giao dịch Repos chưa thực hiện
        /// </summary>
        public async Task<API23OrderReplaceReposCommonPutthroughResponse> API23OrderReplaceReposCommonPutthrough_BU(API23OrderReplaceReposCommonPutthroughRequest request, API23OrderReplaceReposCommonPutthroughResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API23OrderReplaceReposCommonPutthrough_BU with OrderNo: {request.OrderNo} ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";
                bool resultTradingRule = false;
                List<APIReposSideList> _ReposSideList = request.SymbolFirmInfo;
                if (_ReposSideList != null)
                {
                    for (int i = 0; i < _ReposSideList.Count; i++)
                    {
                        APIReposSideList objReposSide = _ReposSideList[i];
                        if (string.IsNullOrEmpty(objReposSide.Symbol))
                        {
                            resultTradingRule = TradingRuleData.CheckTradingSessionWithSymbolNull(out _responseText, out _responseCode);
                        }
                        else
                        {
                            resultTradingRule = TradingRuleData.CheckTradingRule_Input(objReposSide.Symbol, out _responseText, out _responseCode);
                        }

                        if (!resultTradingRule)
                        {
                            _response.ReturnCode = _responseCode;
                            _response.Message = _responseText;
                            _response.InData = null;
                            return _response;
                        }
                    }
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
                // 35= ME
                MessageReposBCGDModify newReposBCGDModify = new MessageReposBCGDModify() { TimeInit = DateTime.Now.Ticks };
                newReposBCGDModify.APIBussiness = ORDER_API.API_23;
                newReposBCGDModify.ClOrdID = _ExOrderNo;
                newReposBCGDModify.OrgOrderID = request.RefExchangeID;
                newReposBCGDModify.QuoteType = request.QuoteType;
                newReposBCGDModify.OrdType = request.OrderType;
                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newReposBCGDModify.Side = CORE_OrderSide.SIDE_BUY;
                    newReposBCGDModify.CoAccount = request.ClientID;
                    newReposBCGDModify.Account = request.ClientIDCounterFirm;
                    newReposBCGDModify.PartyID = request.MemberCounterFirm;
                    newReposBCGDModify.CoPartyID = ConfigData.FirmID;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newReposBCGDModify.Side = CORE_OrderSide.SIDE_SELL;
                    newReposBCGDModify.Account = request.ClientID;
                    newReposBCGDModify.CoAccount = request.ClientIDCounterFirm;
                    newReposBCGDModify.PartyID = ConfigData.FirmID;
                    newReposBCGDModify.CoPartyID = request.MemberCounterFirm;
                }
                //
                newReposBCGDModify.EffectiveTime = request.EffectiveTime;
                newReposBCGDModify.SettlMethod = request.SettleMethod;
                newReposBCGDModify.SettlDate = request.SettleDate1;
                newReposBCGDModify.SettlDate2 = request.SettleDate2;
                newReposBCGDModify.EndDate = request.EndDate;
                newReposBCGDModify.RepurchaseTerm = request.RepurchaseTerm;
                newReposBCGDModify.RepurchaseRate = request.RepurchaseRate;
                newReposBCGDModify.Text = request.Text;
                newReposBCGDModify.NoSide = request.NoSide;
                //
                List<SymbolFirmObject> listSymbolFirmMem = null;
                if (_ReposSideList != null)
                {
                    listSymbolFirmMem = new List<SymbolFirmObject>();
                    for (int i = 0; i < _ReposSideList.Count; i++)
                    {
                        APIReposSideList objReposSide = _ReposSideList[i];
                        //
                        ReposSide _reposSide = new ReposSide();
                        _reposSide.NumSide = objReposSide.NumSide;
                        _reposSide.Symbol = objReposSide.Symbol;
                        _reposSide.OrderQty = objReposSide.OrderQty;
                        _reposSide.Price = objReposSide.MergePrice;
                        _reposSide.HedgeRate = objReposSide.HedgeRate;
                        newReposBCGDModify.RepoSideList.Add(_reposSide);
                        //
                        //
                        SymbolFirmObject symbolFirmMem = new SymbolFirmObject();
                        symbolFirmMem.NumSide = objReposSide.NumSide;
                        symbolFirmMem.Symbol = objReposSide.Symbol;
                        symbolFirmMem.OrderQty = objReposSide.OrderQty;
                        symbolFirmMem.HedgeRate = objReposSide.HedgeRate;
                        symbolFirmMem.MergePrice = objReposSide.MergePrice;
                        listSymbolFirmMem.Add(symbolFirmMem);
                    }
                }

                // Add thông tin order vào memory
                LocalMemory.OrderInfo objOrderMem = new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDModify,
                    OrderNo = request.OrderNo,
                    ClOrdID = newReposBCGDModify.ClOrdID,
                    ExchangeID = "", // ?
                    RefExchangeID = request.RefExchangeID, // ?
                    SeqNum = 0,  // khi nào sở về mới update
                    Symbol = "",
                    Side = request.Side,
                    Price = 0,
                    OrderQty = 0,
                    QuoteType = request.QuoteType,
                    CrossType = "", // ?
                    ClientID = request.ClientID,
                    ClientIDCounterFirm = "", // ?
                    MemberCounterFirm = "", // ?
                    NoSide = request.NoSide,
                    SettlMethod = request.SettleMethod,
                    SettleDate1 = request.SettleDate1,
                    SettleDate2 = request.SettleDate2,
                    EndDate = request.EndDate,
                    RepurchaseTerm = request.RepurchaseTerm,
                    RepurchaseRate = request.RepurchaseRate,
                    OrderType = request.OrderType,
                    //
                    SymbolFirmInfo = listSymbolFirmMem
                };
                newReposBCGDModify.ApiOrderNo = request.OrderNo;
                OrderMemory.Add_NewOrder(objOrderMem);
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newReposBCGDModify);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API23OrderReplaceReposCommonPutthrough OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API23OrderReplaceReposCommonPutthrough OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API23OrderReplaceReposCommonPutthrough_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        ///  2.12	API24 – Hủy lệnh thỏa thuận báo cáo giao dịch Repos chưa thực hiện
        /// </summary>
        public async Task<API24OrderCancelReposCommonPutthroughResponse> API24OrderCancelReposCommonPutthrough_BU(API24OrderCancelReposCommonPutthroughRequest request, API24OrderCancelReposCommonPutthroughResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API24OrderCancelReposCommonPutthrough_BU with OrderNo: {request.OrderNo} ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";
                bool resultTradingRule = TradingRuleData.CheckTradingSessionWithSymbolNull(out _responseText, out _responseCode);
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
                // 35= MC
                MessageReposBCGDCancel newFirmRepos = new MessageReposBCGDCancel() { TimeInit = DateTime.Now.Ticks };
                newFirmRepos.APIBussiness = ORDER_API.API_24;
                newFirmRepos.ClOrdID = _ExOrderNo;
                newFirmRepos.OrgOrderID = request.RefExchangeID;
                newFirmRepos.QuoteType = request.QuoteType;
                newFirmRepos.OrdType = request.OrderType;
                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newFirmRepos.Side = CORE_OrderSide.SIDE_BUY;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newFirmRepos.Side = CORE_OrderSide.SIDE_SELL;
                }
                newFirmRepos.Text = request.Text;

                // Add thông tin order vào memory
                LocalMemory.OrderInfo objOrder = new OrderInfo()
                {
                    RefMsgType = MessageType.ReposBCGDCancel,
                    OrderNo = request.OrderNo,
                    ClOrdID = newFirmRepos.ClOrdID,
                    ExchangeID = "", // ?
                    RefExchangeID = request.RefExchangeID, // ?
                    SeqNum = 0,  // khi nào sở về mới update
                    Symbol = "",
                    Side = request.Side,
                    Price = 0,
                    OrderQty = 0,
                    QuoteType = request.QuoteType,
                    CrossType = "", // ?
                    ClientID = "",
                    ClientIDCounterFirm = "", // ?
                    MemberCounterFirm = "", // ?
                    OrderType = request.OrderType
                };
                newFirmRepos.ApiOrderNo = request.OrderNo;
                OrderMemory.Add_NewOrder(objOrder);
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newFirmRepos);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API24OrderCancelReposCommonPutthrough OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API24OrderCancelReposCommonPutthrough OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API24OrderCancelReposCommonPutthrough_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 2.13	API25 – Sửa lệnh thỏa thuận Repos đã thực hiện trong ngày (sửa GD Repos)
        /// </summary>
        public async Task<API25OrderReplaceDeal1stTransactionReposCommonPutthroughResponse> API25OrderReplaceDeal1stTransactionReposCommonPutthrough_BU(API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest request, API25OrderReplaceDeal1stTransactionReposCommonPutthroughResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API25OrderReplaceDeal1stTransactionReposCommonPutthrough_BU with OrderNo: {request.OrderNo} ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";
                bool resultTradingRule = false;
                List<APIReposSideList> _ReposSideList = request.SymbolFirmInfo;
                if (_ReposSideList != null)
                {
                    for (int i = 0; i < _ReposSideList.Count; i++)
                    {
                        APIReposSideList objReposSide = _ReposSideList[i];
                        if (string.IsNullOrEmpty(objReposSide.Symbol))
                        {
                            resultTradingRule = TradingRuleData.CheckTradingSessionWithSymbolNull(out _responseText, out _responseCode);
                        }
                        else
                        {
                            resultTradingRule = TradingRuleData.CheckTradingRule_Input(objReposSide.Symbol, out _responseText, out _responseCode);
                        }

                        if (!resultTradingRule)
                        {
                            _response.ReturnCode = _responseCode;
                            _response.Message = _responseText;
                            _response.InData = null;
                            return _response;
                        }
                    }
                }
                //
                bool checkExistOrderNo = false;
                List<OrderInfo> lstOrderMem = OrderMemory.GetListOrder_byExchangeID(request.RefExchangeID);
                if (lstOrderMem != null && lstOrderMem.Count > 0)
                {
                    for (int i = 0; i < lstOrderMem.Count; i++)
                    {
                        OrderInfo _objOrder = lstOrderMem[i];
                        if (request.OrderNo == _objOrder.OrderNo)
                        {
                            checkExistOrderNo = true;
                            break;
                        }
                    }
                    if (!checkExistOrderNo)
                    {
                        _response.ReturnCode = ErrorCodeDefine.OrderNoAPI_IsValid;
                        _response.Message = ORDER_RETURNMESSAGE.OrderNoAPI_IsValid;
                        _response.InData = null;
                        return _response;
                    }
                }

                //
                string _ExOrderNo = Utils.GenOrderNo();
                // 35= ME
                MessageReposBCGDModify newReposBCGD = new MessageReposBCGDModify() { TimeInit = DateTime.Now.Ticks };
                newReposBCGD.APIBussiness = ORDER_API.API_25;
                newReposBCGD.ClOrdID = _ExOrderNo;
                newReposBCGD.OrgOrderID = request.RefExchangeID;
                newReposBCGD.QuoteType = request.QuoteType;
                newReposBCGD.OrdType = request.OrderType;

                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newReposBCGD.Side = CORE_OrderSide.SIDE_BUY;
                    newReposBCGD.CoAccount = request.ClientID;
                    newReposBCGD.Account = request.ClientIDCounterFirm;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newReposBCGD.Side = CORE_OrderSide.SIDE_SELL;
                    newReposBCGD.Account = request.ClientID;
                    newReposBCGD.CoAccount = request.ClientIDCounterFirm;
                }
                //
                if (request.MemberCounterFirm == ConfigData.FirmID)
                {
                    newReposBCGD.PartyID = ConfigData.FirmID;
                    newReposBCGD.CoPartyID = ConfigData.FirmID;
                }
                else if (request.MemberCounterFirm != ConfigData.FirmID)
                {
                    if (request.Side == ORDER_SIDE.SIDE_BUY)
                    {
                        newReposBCGD.PartyID = request.MemberCounterFirm;
                        newReposBCGD.CoPartyID = ConfigData.FirmID;
                    }
                    else if (request.Side == ORDER_SIDE.SIDE_SELL)
                    {
                        newReposBCGD.PartyID = ConfigData.FirmID; ;
                        newReposBCGD.CoPartyID = request.MemberCounterFirm;
                    }
                }

                newReposBCGD.EffectiveTime = request.EffectiveTime;
                newReposBCGD.SettlMethod = request.SettleMethod;
                newReposBCGD.SettlDate = request.SettleDate1;
                newReposBCGD.SettlDate2 = request.SettleDate2;
                newReposBCGD.EndDate = request.EndDate;
                newReposBCGD.RepurchaseTerm = request.RepurchaseTerm;
                newReposBCGD.RepurchaseRate = request.RepurchaseRate;
                newReposBCGD.Text = request.Text;
                newReposBCGD.NoSide = request.NoSide;
                //
                List<SymbolFirmObject> listSymbolFirmMem = null;
                if (_ReposSideList != null)
                {
                    listSymbolFirmMem = new List<SymbolFirmObject>();
                    for (int i = 0; i < _ReposSideList.Count; i++)
                    {
                        APIReposSideList objReposSide = _ReposSideList[i];
                        //
                        ReposSide _reposSide = new ReposSide();
                        _reposSide.NumSide = objReposSide.NumSide;
                        _reposSide.Symbol = objReposSide.Symbol;
                        _reposSide.OrderQty = objReposSide.OrderQty;
                        _reposSide.Price = objReposSide.MergePrice;
                        _reposSide.HedgeRate = objReposSide.HedgeRate;
                        newReposBCGD.RepoSideList.Add(_reposSide);
                        //
                        //
                        SymbolFirmObject symbolFirmMem = new SymbolFirmObject();
                        symbolFirmMem.NumSide = objReposSide.NumSide;
                        symbolFirmMem.Symbol = objReposSide.Symbol;
                        symbolFirmMem.OrderQty = objReposSide.OrderQty;
                        symbolFirmMem.HedgeRate = objReposSide.HedgeRate;
                        symbolFirmMem.MergePrice = objReposSide.MergePrice;
                        listSymbolFirmMem.Add(symbolFirmMem);
                    }
                }
                //
                /*
                 * "Lấy refExchangeID gửi lên so với refExchangeID trong mem và và refMsgType =ME trong mem -->
                    TH1: Nếu tìm được bản ghi và exchangeID = trống --> không sinh bản ghi mới, cập nhật orderNo, orderType trên bản ghi tìm được

                    TH2: Nếu tìm được bản ghi và exchangeID # trống -->  sinh bản ghi mới có các thông tin orderNo,refMsgType, refExchangeID, quoteType,  side, clientID, clientIDCounterFirm, memberCounterFirm, settleDate1, settleDate2, endDate, repurchaseTerm, repurchaseRate, orderType, noSide, symbolFirminfo(numSide, symbol, orderQty, hedgeRate, mergePrice)

                    TH3: Nếu không tìm thấy  --> tạo 1 bản ghi trên memory (có các thông tin orderNo,refMsgType, refExchangeID, quoteType,  side, clientID, clientIDCounterFirm, memberCounterFirm, settleDate1, settleDate2, endDate, repurchaseTerm, repurchaseRate, orderType, noSide, symbolFirminfo(numSide, symbol, orderQty, hedgeRate, mergePrice)"
                 */

                OrderInfo objOrderMem = OrderMemory.GetOrderBy(p_RefExchangeID: request.RefExchangeID, p_RefMsgType: MessageType.ReposBCGDModify);
                if (objOrderMem != null)
                {
                    if (string.IsNullOrEmpty(objOrderMem.ExchangeID))
                    {
                        OrderMemory.UpdateOrderInfo(objOrderMem, p_OrderNo: request.OrderNo, p_OrdType: request.OrderType, p_ClOrdID: _ExOrderNo);
                        //
                        bool isUpdate = OrderMemory.UpdateMap_ClOrdID_Index(objOrderMem.ClOrdID);
                        if (!isUpdate)
                        {
                            Logger.ApiLog.Warn($"Cannot Update map ClOrID - Index for Api {ORDER_API.API_25} ClOrdID {0}", objOrderMem.ClOrdID);
                        }
                    }
                    else
                    {
                        // Add thông tin order vào memory
                        LocalMemory.OrderInfo objOrder = new OrderInfo()
                        {
                            RefMsgType = MessageType.ReposBCGDModify,
                            OrderNo = request.OrderNo,
                            ClOrdID = newReposBCGD.ClOrdID,
                            ExchangeID = "", // ?
                            RefExchangeID = request.RefExchangeID, // ?
                            SeqNum = 0,  // khi nào sở về mới update
                            Symbol = "",
                            Side = request.Side,
                            Price = 0,
                            OrderQty = 0,
                            QuoteType = request.QuoteType,
                            CrossType = "", // ?
                            ClientID = request.ClientID,
                            ClientIDCounterFirm = request.ClientIDCounterFirm,
                            MemberCounterFirm = request.MemberCounterFirm, // ?
                            NoSide = request.NoSide,
                            SettlMethod = request.SettleMethod,
                            SettleDate1 = request.SettleDate1,
                            SettleDate2 = request.SettleDate2,
                            EndDate = request.EndDate,
                            RepurchaseTerm = request.RepurchaseTerm,
                            RepurchaseRate = request.RepurchaseRate,
                            OrderType = request.OrderType,
                            //
                            SymbolFirmInfo = listSymbolFirmMem
                        };
                        OrderMemory.Add_NewOrder(objOrder);
                    }
                }
                else
                {
                    // Add thông tin order vào memory
                    LocalMemory.OrderInfo objOrder = new OrderInfo()
                    {
                        RefMsgType = MessageType.ReposBCGDModify,
                        OrderNo = request.OrderNo,
                        ClOrdID = newReposBCGD.ClOrdID,
                        ExchangeID = "", // ?
                        RefExchangeID = request.RefExchangeID, // ?
                        SeqNum = 0,  // khi nào sở về mới update
                        Symbol = "",
                        Side = request.Side,
                        Price = 0,
                        OrderQty = 0,
                        QuoteType = request.QuoteType,
                        CrossType = "", // ?
                        ClientID = request.ClientID,
                        ClientIDCounterFirm = request.ClientIDCounterFirm,
                        MemberCounterFirm = request.MemberCounterFirm, // ?
                        NoSide = request.NoSide,
                        SettlMethod = request.SettleMethod,
                        SettleDate1 = request.SettleDate1,
                        SettleDate2 = request.SettleDate2,
                        EndDate = request.EndDate,
                        RepurchaseTerm = request.RepurchaseTerm,
                        RepurchaseRate = request.RepurchaseRate,
                        OrderType = request.OrderType,
                        //
                        SymbolFirmInfo = listSymbolFirmMem
                    };
                    OrderMemory.Add_NewOrder(objOrder);
                }

                newReposBCGD.ApiOrderNo = request.OrderNo;
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newReposBCGD);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API25OrderReplaceDeal1stTransactionReposCommonPutthrough OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API25OrderReplaceDeal1stTransactionReposCommonPutthrough OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API25OrderReplaceDeal1stTransactionReposCommonPutthrough_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 2.14	API26 – Phản hồi sửa lệnh thỏa thuận Repos đã thực hiện trong ngày (phản hồi sửa GD Repos)
        /// </summary>
        public async Task<API26OrderReplaceDeal1stTransactionReposCommonPutthroughResponse> API26OrderReplaceDeal1stTransactionReposCommonPutthrough_BU(API26OrderReplaceDeal1stTransactionReposCommonPutthroughRequest request, API26OrderReplaceDeal1stTransactionReposCommonPutthroughResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API26OrderReplaceDeal1stTransactionReposCommonPutthrough_BU with OrderNo: {request.OrderNo} ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";
                bool resultTradingRule = false;
                List<APIReposSideList> _ReposSideList = request.SymbolFirmInfo;
                if (_ReposSideList != null)
                {
                    for (int i = 0; i < _ReposSideList.Count; i++)
                    {
                        APIReposSideList objReposSide = _ReposSideList[i];
                        if (string.IsNullOrEmpty(objReposSide.Symbol))
                        {
                            resultTradingRule = TradingRuleData.CheckTradingSessionWithSymbolNull(out _responseText, out _responseCode);
                        }
                        else
                        {
                            resultTradingRule = TradingRuleData.CheckTradingRule_Input(objReposSide.Symbol, out _responseText, out _responseCode);
                        }

                        if (!resultTradingRule)
                        {
                            _response.ReturnCode = _responseCode;
                            _response.Message = _responseText;
                            _response.InData = null;
                            return _response;
                        }
                    }
                }
                //
                bool checkExistOrderNo = false;
                List<OrderInfo> lstOrderMem = OrderMemory.GetListOrder_byExchangeID(request.RefExchangeID);
                if (lstOrderMem != null && lstOrderMem.Count > 0)
                {
                    for (int i = 0; i < lstOrderMem.Count; i++)
                    {
                        OrderInfo _objOrder = lstOrderMem[i];
                        if (request.OrderNo == _objOrder.OrderNo)
                        {
                            checkExistOrderNo = true;
                            break;
                        }
                    }
                    if (!checkExistOrderNo)
                    {
                        _response.ReturnCode = ErrorCodeDefine.OrderNoAPI_IsValid;
                        _response.Message = ORDER_RETURNMESSAGE.OrderNoAPI_IsValid;
                        _response.InData = null;
                        return _response;
                    }
                }
                string _ExOrderNo = Utils.GenOrderNo();
                // 35= ME
                MessageReposBCGDModify newReposBCGD = new MessageReposBCGDModify() { TimeInit = DateTime.Now.Ticks };
                newReposBCGD.APIBussiness = ORDER_API.API_26;
                newReposBCGD.ClOrdID = _ExOrderNo;
                newReposBCGD.OrgOrderID = request.RefExchangeID;
                newReposBCGD.QuoteType = request.QuoteType;
                newReposBCGD.OrdType = request.OrderType;

                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newReposBCGD.Side = CORE_OrderSide.SIDE_BUY;
                    newReposBCGD.CoAccount = request.ClientID;
                    newReposBCGD.Account = request.ClientIDCounterFirm;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newReposBCGD.Side = CORE_OrderSide.SIDE_SELL;
                    newReposBCGD.Account = request.ClientID;
                    newReposBCGD.CoAccount = request.ClientIDCounterFirm;
                }
                //
                if (request.MemberCounterFirm == ConfigData.FirmID)
                {
                    newReposBCGD.PartyID = ConfigData.FirmID;
                    newReposBCGD.CoPartyID = ConfigData.FirmID;
                }
                else if (request.MemberCounterFirm != ConfigData.FirmID)
                {
                    if (request.Side == ORDER_SIDE.SIDE_BUY)
                    {
                        newReposBCGD.PartyID = request.MemberCounterFirm;
                        newReposBCGD.CoPartyID = ConfigData.FirmID;
                    }
                    else if (request.Side == ORDER_SIDE.SIDE_SELL)
                    {
                        newReposBCGD.PartyID = ConfigData.FirmID; ;
                        newReposBCGD.CoPartyID = request.MemberCounterFirm;
                    }
                }

                newReposBCGD.EffectiveTime = request.EffectiveTime;
                newReposBCGD.SettlMethod = request.SettleMethod;
                newReposBCGD.SettlDate = request.SettleDate1;
                newReposBCGD.SettlDate2 = request.SettleDate2;
                newReposBCGD.EndDate = request.EndDate;
                newReposBCGD.RepurchaseTerm = request.RepurchaseTerm;
                newReposBCGD.RepurchaseRate = request.RepurchaseRate;
                newReposBCGD.Text = request.Text;
                newReposBCGD.NoSide = request.NoSide;
                //
                List<SymbolFirmObject> listSymbolFirmMem = null;
                if (_ReposSideList != null)
                {
                    listSymbolFirmMem = new List<SymbolFirmObject>();
                    for (int i = 0; i < _ReposSideList.Count; i++)
                    {
                        APIReposSideList objReposSide = _ReposSideList[i];
                        //
                        ReposSide _reposSide = new ReposSide();
                        _reposSide.NumSide = objReposSide.NumSide;
                        _reposSide.Symbol = objReposSide.Symbol;
                        _reposSide.OrderQty = objReposSide.OrderQty;
                        _reposSide.Price = objReposSide.MergePrice;
                        _reposSide.HedgeRate = objReposSide.HedgeRate;
                        newReposBCGD.RepoSideList.Add(_reposSide);
                        //
                        //
                        SymbolFirmObject symbolFirmMem = new SymbolFirmObject();
                        symbolFirmMem.NumSide = objReposSide.NumSide;
                        symbolFirmMem.Symbol = objReposSide.Symbol;
                        symbolFirmMem.OrderQty = objReposSide.OrderQty;
                        symbolFirmMem.HedgeRate = objReposSide.HedgeRate;
                        symbolFirmMem.MergePrice = objReposSide.MergePrice;
                        listSymbolFirmMem.Add(symbolFirmMem);
                    }
                }
                /*
                 * "Lấy refExchangeID gửi lên so với exchangeID trong mem và và refMsgType =ME trong mem -->
                    TH1: Nếu tìm được bản ghi-->không sinh bản ghi mới, cập nhật orderNo, orderType trên bản ghi tìm được
                    TH2: Nếu không tìm thấy  --> tạo 1 bản ghi trên memory (có các thông tin orderNo,refMsgType, refExchangeID, quoteType,  side, clientID, clientIDCounterFirm, memberCounterFirm, settleDate1, settleDate2, endDate, repurchaseTerm, repurchaseRate, orderType, noSide, symbolFirminfo(numSide, symbol, orderQty, hedgeRate, mergePrice)"
                 */
                OrderInfo objOrderMem = OrderMemory.GetOrderBy(p_ExchangeID: request.RefExchangeID, p_RefMsgType: MessageType.ReposBCGDModify);
                if (objOrderMem != null)
                {
                    // TH1: Nếu tìm được bản ghi-->không sinh bản ghi mới, cập nhật orderNo, orderType trên bản ghi tìm được
                    OrderMemory.UpdateOrderInfo(objOrderMem, p_OrderNo: request.OrderNo, p_OrdType: request.OrderType, p_QuoteType: request.QuoteType, p_ClOrdID: _ExOrderNo);
                    //
                    bool isUpdate = OrderMemory.UpdateMap_ClOrdID_Index(objOrderMem.ClOrdID);
                    if (!isUpdate)
                    {
                        Logger.ApiLog.Warn($"Cannot Update map ClOrID - Index for Api {ORDER_API.API_26} ClOrdID {0}", objOrderMem.ClOrdID);
                    }
                }
                else
                {
                    // TH2: Nếu không tìm thấy  --> tạo 1 bản ghi trên memory (có các thông tin orderNo,refMsgType, refExchangeID, quoteType,  side, clientID, clientIDCounterFirm, memberCounterFirm, settleDate1, settleDate2, endDate, repurchaseTerm, repurchaseRate, orderType, noSide, symbolFirminfo(numSide, symbol, orderQty, hedgeRate, mergePrice)
                    LocalMemory.OrderInfo objOrder = new OrderInfo()
                    {
                        RefMsgType = MessageType.ReposBCGDModify,
                        OrderNo = request.OrderNo,
                        ClOrdID = newReposBCGD.ClOrdID,
                        ExchangeID = "", // ?
                        RefExchangeID = request.RefExchangeID, // ?
                        SeqNum = 0,  // khi nào sở về mới update
                        Symbol = "",
                        Side = request.Side,
                        Price = 0,
                        OrderQty = 0,
                        QuoteType = request.QuoteType,
                        CrossType = "", // ?
                        ClientID = request.ClientID,
                        ClientIDCounterFirm = request.ClientIDCounterFirm,
                        MemberCounterFirm = request.MemberCounterFirm, // ?
                        NoSide = request.NoSide,
                        SettlMethod = request.SettleMethod,
                        SettleDate1 = request.SettleDate1,
                        SettleDate2 = request.SettleDate2,
                        EndDate = request.EndDate,
                        RepurchaseTerm = request.RepurchaseTerm,
                        RepurchaseRate = request.RepurchaseRate,
                        OrderType = request.OrderType,
                        //
                        SymbolFirmInfo = listSymbolFirmMem
                    };
                    OrderMemory.Add_NewOrder(objOrder);
                }
                newReposBCGD.ApiOrderNo = request.OrderNo;
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newReposBCGD);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API26OrderReplaceDeal1stTransactionReposCommonPutthrough OrderNo {0} fail because queue is full0", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API26OrderReplaceDeal1stTransactionReposCommonPutthrough OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API26OrderReplaceDeal1stTransactionReposCommonPutthrough_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        ///  2.15	API27 – Hủy lệnh thỏa thuận Repos đã thực hiện trong ngày (hủy GD Repos)
        /// </summary>
        public async Task<API27OrderCancelDeal1stTransactionReposCommonPutthroughResponse> API27OrderCancelDeal1stTransactionReposCommonPutthrough_BU(API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest request, API27OrderCancelDeal1stTransactionReposCommonPutthroughResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API27OrderCancelDeal1stTransactionReposCommonPutthrough_BU with OrderNo: {request.OrderNo} ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";
                bool resultTradingRule = false;
                resultTradingRule = TradingRuleData.CheckTradingSessionWithSymbolNull(out _responseText, out _responseCode);

                if (!resultTradingRule)
                {
                    _response.ReturnCode = _responseCode;
                    _response.Message = _responseText;
                    _response.InData = null;
                    return _response;
                }
                //
                //
                bool checkExistOrderNo = false;
                List<OrderInfo> lstOrderMem = OrderMemory.GetListOrder_byExchangeID(request.RefExchangeID);
                if (lstOrderMem != null && lstOrderMem.Count > 0)
                {
                    for (int i = 0; i < lstOrderMem.Count; i++)
                    {
                        OrderInfo _objOrder = lstOrderMem[i];
                        if (request.OrderNo == _objOrder.OrderNo)
                        {
                            checkExistOrderNo = true;
                            break;
                        }
                    }
                    if (!checkExistOrderNo)
                    {
                        _response.ReturnCode = ErrorCodeDefine.OrderNoAPI_IsValid;
                        _response.Message = ORDER_RETURNMESSAGE.OrderNoAPI_IsValid;
                        _response.InData = null;
                        return _response;
                    }
                }
                string _ExOrderNo = Utils.GenOrderNo();
                // 35= MC
                MessageReposBCGDCancel newFirmRepos = new MessageReposBCGDCancel() { TimeInit = DateTime.Now.Ticks };
                newFirmRepos.APIBussiness = ORDER_API.API_27;
                newFirmRepos.ClOrdID = _ExOrderNo;
                newFirmRepos.OrgOrderID = request.RefExchangeID;
                newFirmRepos.QuoteType = request.QuoteType;
                newFirmRepos.OrdType = request.OrderType;
                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newFirmRepos.Side = CORE_OrderSide.SIDE_BUY;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newFirmRepos.Side = CORE_OrderSide.SIDE_SELL;
                }
                newFirmRepos.Text = request.Text;
                /*
                 * "Lấy refExchangeID gửi lên so với refExchangeID trong mem và và refMsgType =MC trong mem -->
                        TH1: Nếu tìm được bản ghi và exchangeID = trống --> không sinh bản ghi mới, cập nhật orderNo, orderType trên bản ghi tìm được
                        TH2: Nếu tìm được bản ghi và exchangeID # trống -->  sinh bản ghi mới có các thông tin (orderNo,refMsgType, refExchangeID, quoteType,  side, orderType)
                        TH3: Nếu không tìm thấy  --> tạo 1 bản ghi trên memory (có các thông tin orderNo,refMsgType, refExchangeID, quoteType,  side, orderType)"

                 */
                OrderInfo objOrderMem = OrderMemory.GetOrderBy(p_RefExchangeID: request.RefExchangeID, p_RefMsgType: MessageType.ReposBCGDCancel);
                if (objOrderMem != null)
                {
                    if (string.IsNullOrEmpty(objOrderMem.ExchangeID))
                    {
                        OrderMemory.UpdateOrderInfo(objOrderMem, p_OrderNo: request.OrderNo, p_OrdType: request.OrderType, p_ClOrdID: _ExOrderNo);
                        //
                        bool isUpdate = OrderMemory.UpdateMap_ClOrdID_Index(objOrderMem.ClOrdID);
                        if (!isUpdate)
                        {
                            Logger.ApiLog.Warn($"Cannot Update map ClOrID - Index for Api {ORDER_API.API_27} ClOrdID {0}", objOrderMem.ClOrdID);
                        }
                    }
                    else
                    {
                        // Add thông tin order vào memory
                        LocalMemory.OrderInfo objOrder = new OrderInfo()
                        {
                            RefMsgType = MessageType.ReposBCGDCancel,
                            OrderNo = request.OrderNo,
                            ClOrdID = newFirmRepos.ClOrdID,
                            ExchangeID = "", // ?
                            RefExchangeID = request.RefExchangeID, // ?
                            SeqNum = 0,  // khi nào sở về mới update
                            Symbol = "",
                            Side = request.Side,
                            Price = 0,
                            OrderQty = 0,
                            QuoteType = request.QuoteType,
                            CrossType = "", // ?
                            ClientID = "",
                            ClientIDCounterFirm = "",
                            MemberCounterFirm = "", // ?
                            NoSide = 0,
                            SettlMethod = 0,
                            SettleDate1 = "",
                            SettleDate2 = "",
                            EndDate = "",
                            RepurchaseTerm = 0,
                            RepurchaseRate = 0,
                            OrderType = request.OrderType
                        };
                        OrderMemory.Add_NewOrder(objOrder);
                    }
                }
                else
                {
                    // Add thông tin order vào memory
                    LocalMemory.OrderInfo objOrder = new OrderInfo()
                    {
                        RefMsgType = MessageType.ReposBCGDCancel,
                        OrderNo = request.OrderNo,
                        ClOrdID = newFirmRepos.ClOrdID,
                        ExchangeID = "", // ?
                        RefExchangeID = request.RefExchangeID, // ?
                        SeqNum = 0,  // khi nào sở về mới update
                        Symbol = "",
                        Side = request.Side,
                        Price = 0,
                        OrderQty = 0,
                        QuoteType = request.QuoteType,
                        CrossType = "", // ?
                        ClientID = "",
                        ClientIDCounterFirm = "",
                        MemberCounterFirm = "", // ?
                        NoSide = 0,
                        SettlMethod = 0,
                        SettleDate1 = "",
                        SettleDate2 = "",
                        EndDate = "",
                        RepurchaseTerm = 0,
                        RepurchaseRate = 0,
                        OrderType = request.OrderType
                    };
                    OrderMemory.Add_NewOrder(objOrder);
                }
                //
                newFirmRepos.ApiOrderNo = request.OrderNo;
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newFirmRepos);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API27OrderCancelDeal1stTransactionReposCommonPutthrough OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API27OrderCancelDeal1stTransactionReposCommonPutthrough OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API27OrderCancelDeal1stTransactionReposCommonPutthrough_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        ///  2.16	API28 – Phản hồi hủy lệnh thỏa thuận Repos đã thực hiện trong ngày (phản hồi hủy GD Repos)
        /// </summary>
        public async Task<API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughResponse> API28OrderConfirmCancelDeal1stTransactionReposCommonPutthrough_BU(API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughRequest request, API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API28OrderConfirmCancelDeal1stTransactionReposCommonPutthrough_BU with OrderNo: {request.OrderNo} ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";
                bool resultTradingRule = false;
                resultTradingRule = TradingRuleData.CheckTradingSessionWithSymbolNull(out _responseText, out _responseCode);

                if (!resultTradingRule)
                {
                    _response.ReturnCode = _responseCode;
                    _response.Message = _responseText;
                    _response.InData = null;
                    return _response;
                }
                //
                //
                bool checkExistOrderNo = false;
                List<OrderInfo> lstOrderMem = OrderMemory.GetListOrder_byExchangeID(request.RefExchangeID);
                if (lstOrderMem != null && lstOrderMem.Count > 0)
                {
                    for (int i = 0; i < lstOrderMem.Count; i++)
                    {
                        OrderInfo _objOrder = lstOrderMem[i];
                        if (request.OrderNo == _objOrder.OrderNo)
                        {
                            checkExistOrderNo = true;
                            break;
                        }
                    }
                    if (!checkExistOrderNo)
                    {
                        _response.ReturnCode = ErrorCodeDefine.OrderNoAPI_IsValid;
                        _response.Message = ORDER_RETURNMESSAGE.OrderNoAPI_IsValid;
                        _response.InData = null;
                        return _response;
                    }
                }
                string _ExOrderNo = Utils.GenOrderNo();
                // 35= MC
                MessageReposBCGDCancel newFirmRepos = new MessageReposBCGDCancel() { TimeInit = DateTime.Now.Ticks };
                newFirmRepos.APIBussiness = ORDER_API.API_28;
                newFirmRepos.ClOrdID = _ExOrderNo;
                newFirmRepos.OrgOrderID = request.RefExchangeID;
                newFirmRepos.QuoteType = request.QuoteType;
                newFirmRepos.OrdType = request.OrderType;
                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newFirmRepos.Side = CORE_OrderSide.SIDE_BUY;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newFirmRepos.Side = CORE_OrderSide.SIDE_SELL;
                }
                newFirmRepos.Text = request.Text;
                //
                /*
                 *
                 * "Lấy refExchangeID gửi lên so với exchangeID trong mem và và refMsgType =MC trong mem -->
                    TH1: Nếu tìm được bản ghi-->không sinh bản ghi mới, cập nhật orderNo, orderType trên bản ghi tìm được
                    TH2: Nếu không tìm thấy  --> tạo 1 bản ghi trên memory (có các thông tin orderNo,refMsgType, refExchangeID, quoteType,  side, clientID, clientIDCounterFirm, memberCounterFirm, settleDate1, settleDate2, endDate, repurchaseTerm, repurchaseRate, orderType, noSide, symbolFirminfo(numSide, symbol, orderQty, hedgeRate, mergePrice)"
                 */
                OrderInfo objOrderMem = OrderMemory.GetOrderBy(p_ExchangeID: request.RefExchangeID, p_RefMsgType: MessageType.ReposBCGDCancel);
                if (objOrderMem != null)
                {
                    OrderMemory.UpdateOrderInfo(objOrderMem, p_OrderNo: request.OrderNo, p_OrdType: request.OrderType, p_QuoteType: request.QuoteType, p_ClOrdID: _ExOrderNo);
                    //
                    bool isUpdate = OrderMemory.UpdateMap_ClOrdID_Index(objOrderMem.ClOrdID);
                    if (!isUpdate)
                    {
                        Logger.ApiLog.Warn($"Cannot Update map ClOrID - Index for Api {ORDER_API.API_28} ClOrdID {0}", objOrderMem.ClOrdID);
                    }
                }
                else
                {
                    // Add thông tin order vào memory
                    LocalMemory.OrderInfo objOrder = new OrderInfo()
                    {
                        RefMsgType = MessageType.ReposBCGDCancel,
                        OrderNo = request.OrderNo,
                        ClOrdID = newFirmRepos.ClOrdID,
                        ExchangeID = "", // ?
                        RefExchangeID = request.RefExchangeID, // ?
                        SeqNum = 0,  // khi nào sở về mới update
                        Symbol = "",
                        Side = "",
                        Price = 0,
                        OrderQty = 0,
                        QuoteType = request.QuoteType,
                        CrossType = "", // ?
                        ClientID = "",
                        ClientIDCounterFirm = "", // ?
                        MemberCounterFirm = "", // ?
                        OrderType = request.OrderType
                    };
                    OrderMemory.Add_NewOrder(objOrder);
                }

                newFirmRepos.ApiOrderNo = request.OrderNo;
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newFirmRepos);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API28OrderConfirmCancelDeal1stTransactionReposCommonPutthrough OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API28OrderConfirmCancelDeal1stTransactionReposCommonPutthrough OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API28OrderConfirmCancelDeal1stTransactionReposCommonPutthrough_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 2.17	API29 – Sửa lệnh thỏa thuận Repos mua lại đảo ngược (sửa GD Reverse Repos)
        /// </summary>
        public async Task<API29OrderReplaceDeal2ndTransactionReposCommonPutthroughResponse> API29OrderReplaceDeal2ndTransactionReposCommonPutthrough_BU(API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest request, API29OrderReplaceDeal2ndTransactionReposCommonPutthroughResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API29OrderReplaceDeal2ndTransactionReposCommonPutthrough_BU with OrderNo: {request.OrderNo} ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";
                bool resultTradingRule = false;
                List<APIReposSideList> _ReposSideList = request.SymbolFirmInfo;
                if (_ReposSideList != null)
                {
                    for (int i = 0; i < _ReposSideList.Count; i++)
                    {
                        APIReposSideList objReposSide = _ReposSideList[i];
                        if (string.IsNullOrEmpty(objReposSide.Symbol))
                        {
                            resultTradingRule = TradingRuleData.CheckTradingSessionWithSymbolNull(out _responseText, out _responseCode);
                        }
                        else
                        {
                            resultTradingRule = TradingRuleData.CheckTradingRule_Input(objReposSide.Symbol, out _responseText, out _responseCode);
                        }

                        if (!resultTradingRule)
                        {
                            _response.ReturnCode = _responseCode;
                            _response.Message = _responseText;
                            _response.InData = null;
                            return _response;
                        }
                    }
                }

                string _ExOrderNo = Utils.GenOrderNo();
                // 35= ME
                MessageReposBCGDModify newReposBCGDModify = new MessageReposBCGDModify() { TimeInit = DateTime.Now.Ticks };
                newReposBCGDModify.APIBussiness = ORDER_API.API_29;
                newReposBCGDModify.ClOrdID = _ExOrderNo;
                newReposBCGDModify.OrgOrderID = request.RefExchangeID;
                newReposBCGDModify.QuoteType = request.QuoteType;
                newReposBCGDModify.OrdType = request.OrderType;
                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newReposBCGDModify.Side = CORE_OrderSide.SIDE_BUY;
                    newReposBCGDModify.CoAccount = request.ClientID;
                    newReposBCGDModify.Account = request.ClientIDCounterFirm;
                    newReposBCGDModify.PartyID = request.MemberCounterFirm;
                    newReposBCGDModify.CoPartyID = ConfigData.FirmID;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newReposBCGDModify.Side = CORE_OrderSide.SIDE_SELL;
                    newReposBCGDModify.Account = request.ClientID;
                    newReposBCGDModify.CoAccount = request.ClientIDCounterFirm;
                    newReposBCGDModify.PartyID = ConfigData.FirmID;
                    newReposBCGDModify.CoPartyID = request.MemberCounterFirm;
                }
                newReposBCGDModify.EffectiveTime = request.EffectiveTime;
                newReposBCGDModify.SettlMethod = request.SettleMethod;
                newReposBCGDModify.SettlDate = request.SettleDate1;
                newReposBCGDModify.SettlDate2 = request.SettleDate2;
                newReposBCGDModify.EndDate = request.EndDate;
                newReposBCGDModify.RepurchaseTerm = request.RepurchaseTerm;
                newReposBCGDModify.RepurchaseRate = request.RepurchaseRate;
                newReposBCGDModify.Text = request.Text;
                newReposBCGDModify.NoSide = request.NoSide;
                //
                List<SymbolFirmObject> listSymbolFirmMem = null;
                if (_ReposSideList != null)
                {
                    listSymbolFirmMem = new List<SymbolFirmObject>();
                    for (int i = 0; i < _ReposSideList.Count; i++)
                    {
                        APIReposSideList objReposSide = _ReposSideList[i];
                        //
                        ReposSide _reposSide = new ReposSide();
                        _reposSide.NumSide = objReposSide.NumSide;
                        _reposSide.Symbol = objReposSide.Symbol;
                        _reposSide.OrderQty = objReposSide.OrderQty;
                        _reposSide.Price = objReposSide.MergePrice;
                        _reposSide.HedgeRate = objReposSide.HedgeRate;
                        newReposBCGDModify.RepoSideList.Add(_reposSide);
                        //
                        //
                        SymbolFirmObject symbolFirmMem = new SymbolFirmObject();
                        symbolFirmMem.NumSide = objReposSide.NumSide;
                        symbolFirmMem.Symbol = objReposSide.Symbol;
                        symbolFirmMem.OrderQty = objReposSide.OrderQty;
                        symbolFirmMem.HedgeRate = objReposSide.HedgeRate;
                        symbolFirmMem.MergePrice = objReposSide.MergePrice;
                        listSymbolFirmMem.Add(symbolFirmMem);
                    }
                }
                /*
                 * "Lấy refExchangeID gửi lên so với refExchangeID trong mem và và refMsgType =ME trong mem -->
                    TH1: Nếu tìm được bản ghi và exchangeID = trống --> không sinh bản ghi mới, cập nhật orderNo, orderType trên bản ghi tìm được
                    TH2: Nếu tìm được bản ghi và exchangeID # trống -->  sinh bản ghi mới có các thông tin orderNo,refMsgType, refExchangeID, quoteType,  side, clientID, clientIDCounterFirm, memberCounterFirm, settleDate1, settleDate2, endDate, repurchaseTerm, repurchaseRate, orderType, noSide, symbolFirminfo(numSide, symbol, orderQty, hedgeRate, mergePrice)
                    TH3: Nếu không tìm thấy  --> tạo 1 bản ghi trên memory (có các thông tin orderNo,refMsgType, refExchangeID, quoteType,  side, clientID, clientIDCounterFirm, memberCounterFirm, settleDate1, settleDate2, endDate, repurchaseTerm, repurchaseRate, orderType, noSide, symbolFirminfo(numSide, symbol, orderQty, hedgeRate, mergePrice)"

                 */

                OrderInfo objOrderMem = OrderMemory.GetOrderBy(p_RefExchangeID: request.RefExchangeID, p_RefMsgType: MessageType.ReposBCGDModify);
                if (objOrderMem != null)
                {
                    if (string.IsNullOrEmpty(objOrderMem.ExchangeID))
                    {
                        OrderMemory.UpdateOrderInfo(objOrderMem, p_OrderNo: request.OrderNo, p_OrdType: request.OrderType, p_ClOrdID: _ExOrderNo);
                        //
                        bool isUpdate = OrderMemory.UpdateMap_ClOrdID_Index(objOrderMem.ClOrdID);
                        if (!isUpdate)
                        {
                            Logger.ApiLog.Warn($"Cannot Update map ClOrID - Index for Api {ORDER_API.API_29} ClOrdID {0}", objOrderMem.ClOrdID);
                        }
                    }
                    else
                    {
                        // Add thông tin order vào memory
                        LocalMemory.OrderInfo objOrder = new OrderInfo()
                        {
                            RefMsgType = MessageType.ReposBCGDModify,
                            OrderNo = request.OrderNo,
                            ClOrdID = newReposBCGDModify.ClOrdID,
                            ExchangeID = "", // ?
                            RefExchangeID = request.RefExchangeID, // ?
                            SeqNum = 0,  // khi nào sở về mới update
                            Symbol = "",
                            Side = request.Side,
                            Price = 0,
                            OrderQty = 0,
                            QuoteType = request.QuoteType,
                            CrossType = "", // ?
                            ClientID = request.ClientID,
                            ClientIDCounterFirm = "",
                            MemberCounterFirm = "", // ?
                            NoSide = request.NoSide,
                            SettlMethod = request.SettleMethod,
                            SettleDate1 = request.SettleDate1,
                            SettleDate2 = request.SettleDate2,
                            EndDate = request.EndDate,
                            RepurchaseTerm = request.RepurchaseTerm,
                            RepurchaseRate = request.RepurchaseRate,
                            OrderType = request.OrderType,
                            //
                            SymbolFirmInfo = listSymbolFirmMem
                        };
                        OrderMemory.Add_NewOrder(objOrder);
                    }
                }
                else
                {
                    // Add thông tin order vào memory
                    LocalMemory.OrderInfo objOrder = new OrderInfo()
                    {
                        RefMsgType = MessageType.ReposBCGDModify,
                        OrderNo = request.OrderNo,
                        ClOrdID = newReposBCGDModify.ClOrdID,
                        ExchangeID = "", // ?
                        RefExchangeID = request.RefExchangeID, // ?
                        SeqNum = 0,  // khi nào sở về mới update
                        Symbol = "",
                        Side = request.Side,
                        Price = 0,
                        OrderQty = 0,
                        QuoteType = request.QuoteType,
                        CrossType = "", // ?
                        ClientID = request.ClientID,
                        ClientIDCounterFirm = request.ClientIDCounterFirm,
                        MemberCounterFirm = request.MemberCounterFirm, // ?
                        NoSide = request.NoSide,
                        SettlMethod = request.SettleMethod,
                        SettleDate1 = request.SettleDate1,
                        SettleDate2 = request.SettleDate2,
                        EndDate = request.EndDate,
                        RepurchaseTerm = request.RepurchaseTerm,
                        RepurchaseRate = request.RepurchaseRate,
                        OrderType = request.OrderType,
                        //
                        SymbolFirmInfo = listSymbolFirmMem
                    };
                    OrderMemory.Add_NewOrder(objOrder);
                }
                newReposBCGDModify.ApiOrderNo = request.OrderNo;
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newReposBCGDModify);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API29OrderReplaceDeal2ndTransactionReposCommonPutthrough OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API29OrderReplaceDeal2ndTransactionReposCommonPutthrough OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API29OrderReplaceDeal2ndTransactionReposCommonPutthrough_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }

        /// <summary>
        /// 2.18	API30 – Phản hồi sửa lệnh thỏa thuận Repos mua lại đảo ngược (phản hồi sửa GD Reverse Repos)
        /// </summary>
        public async Task<API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughResponse> API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthrough_BU(API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughRequest request, API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughResponse _response)
        {
            try
            {
                string ID = Guid.NewGuid().ToString();
                Logger.ApiLog.Info($"ProcessRevBussiness | Start call API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthrough_BU with OrderNo: {request.OrderNo} ID {ID}");
                //
                string _responseText = "";
                string _responseCode = "";
                bool resultTradingRule = false;
                List<APIReposSideList> _ReposSideList = request.SymbolFirmInfo;
                if (_ReposSideList != null)
                {
                    for (int i = 0; i < _ReposSideList.Count; i++)
                    {
                        APIReposSideList objReposSide = _ReposSideList[i];
                        if (string.IsNullOrEmpty(objReposSide.Symbol))
                        {
                            resultTradingRule = TradingRuleData.CheckTradingSessionWithSymbolNull(out _responseText, out _responseCode);
                        }
                        else
                        {
                            resultTradingRule = TradingRuleData.CheckTradingRule_Input(objReposSide.Symbol, out _responseText, out _responseCode);
                        }

                        if (!resultTradingRule)
                        {
                            _response.ReturnCode = _responseCode;
                            _response.Message = _responseText;
                            _response.InData = null;
                            return _response;
                        }
                    }
                }

                string _ExOrderNo = Utils.GenOrderNo();
                // 35= ME
                MessageReposBCGDModify newReposBCGDModify = new MessageReposBCGDModify() { TimeInit = DateTime.Now.Ticks };
                newReposBCGDModify.APIBussiness = ORDER_API.API_29;
                newReposBCGDModify.ClOrdID = _ExOrderNo;
                newReposBCGDModify.OrgOrderID = request.RefExchangeID;
                newReposBCGDModify.QuoteType = request.QuoteType;
                newReposBCGDModify.OrdType = request.OrderType;
                if (request.Side == ORDER_SIDE.SIDE_BUY)
                {
                    newReposBCGDModify.Side = CORE_OrderSide.SIDE_BUY;
                    newReposBCGDModify.Account = request.ClientIDCounterFirm;
                    newReposBCGDModify.CoAccount = request.ClientID;
                    newReposBCGDModify.PartyID = request.MemberCounterFirm;
                    newReposBCGDModify.CoPartyID = ConfigData.FirmID;
                }
                else if (request.Side == ORDER_SIDE.SIDE_SELL)
                {
                    newReposBCGDModify.Side = CORE_OrderSide.SIDE_SELL;
                    newReposBCGDModify.Account = request.ClientID;
                    newReposBCGDModify.CoAccount = request.ClientIDCounterFirm;
                    newReposBCGDModify.PartyID = ConfigData.FirmID;
                    newReposBCGDModify.CoPartyID = request.MemberCounterFirm;
                }
                newReposBCGDModify.EffectiveTime = request.EffectiveTime;
                newReposBCGDModify.SettlMethod = request.SettleMethod;
                newReposBCGDModify.SettlDate = request.SettleDate1;
                newReposBCGDModify.SettlDate2 = request.SettleDate2;
                newReposBCGDModify.EndDate = request.EndDate;
                newReposBCGDModify.RepurchaseTerm = request.RepurchaseTerm;
                newReposBCGDModify.RepurchaseRate = request.RepurchaseRate;
                newReposBCGDModify.Text = request.Text;
                newReposBCGDModify.NoSide = request.NoSide;
                //
                List<SymbolFirmObject> listSymbolFirmMem = null;
                if (_ReposSideList != null)
                {
                    listSymbolFirmMem = new List<SymbolFirmObject>();
                    for (int i = 0; i < _ReposSideList.Count; i++)
                    {
                        APIReposSideList objReposSide = _ReposSideList[i];
                        //
                        ReposSide _reposSide = new ReposSide();
                        _reposSide.NumSide = objReposSide.NumSide;
                        _reposSide.Symbol = objReposSide.Symbol;
                        _reposSide.OrderQty = objReposSide.OrderQty;
                        _reposSide.Price = objReposSide.MergePrice;
                        _reposSide.HedgeRate = objReposSide.HedgeRate;
                        newReposBCGDModify.RepoSideList.Add(_reposSide);
                        //
                        //
                        SymbolFirmObject symbolFirmMem = new SymbolFirmObject();
                        symbolFirmMem.NumSide = objReposSide.NumSide;
                        symbolFirmMem.Symbol = objReposSide.Symbol;
                        symbolFirmMem.OrderQty = objReposSide.OrderQty;
                        symbolFirmMem.HedgeRate = objReposSide.HedgeRate;
                        symbolFirmMem.MergePrice = objReposSide.MergePrice;
                        listSymbolFirmMem.Add(symbolFirmMem);
                    }
                }
                /*
                * "Lấy refExchangeID gửi lên so với exchangeID trong mem và và refMsgType =ME trong mem -->
                    TH1: Nếu tìm được bản ghi-->không sinh bản ghi mới, cập nhật orderNo, orderType trên bản ghi tìm được
                    TH2: Nếu không tìm thấy  --> tạo 1 bản ghi trên memory (có các thông tin orderNo,refMsgType, refExchangeID, quoteType,  side, clientID, clientIDCounterFirm, memberCounterFirm, settleDate1, settleDate2, endDate, repurchaseTerm, repurchaseRate, orderType, noSide, symbolFirminfo(numSide, symbol, orderQty, hedgeRate, mergePrice)"
                */

                OrderInfo objOrderMem = OrderMemory.GetOrderBy(p_ExchangeID: request.RefExchangeID, p_RefMsgType: MessageType.ReposBCGDModify);
                if (objOrderMem != null)
                {
                    OrderMemory.UpdateOrderInfo(objOrderMem, p_OrderNo: request.OrderNo, p_OrdType: request.OrderType, p_QuoteType: request.QuoteType, p_ClOrdID: _ExOrderNo);
                    //
                    bool isUpdate = OrderMemory.UpdateMap_ClOrdID_Index(objOrderMem.ClOrdID);
                    if (!isUpdate)
                    {
                        Logger.ApiLog.Warn($"Cannot Update map ClOrID - Index for Api {ORDER_API.API_30} ClOrdID {0}", objOrderMem.ClOrdID);
                    }
                }
                else
                {
                    // Add thông tin order vào memory
                    LocalMemory.OrderInfo objOrder = new OrderInfo()
                    {
                        RefMsgType = MessageType.ReposBCGDModify,
                        OrderNo = request.OrderNo,
                        ClOrdID = newReposBCGDModify.ClOrdID,
                        ExchangeID = "", // ?
                        RefExchangeID = request.RefExchangeID, // ?
                        SeqNum = 0,  // khi nào sở về mới update
                        Symbol = "",
                        Side = request.Side,
                        Price = 0,
                        OrderQty = 0,
                        QuoteType = request.QuoteType,
                        CrossType = "", // ?
                        ClientID = request.ClientID,
                        ClientIDCounterFirm = request.ClientIDCounterFirm,
                        MemberCounterFirm = request.MemberCounterFirm, // ?
                        NoSide = request.NoSide,
                        SettlMethod = request.SettleMethod,
                        SettleDate1 = request.SettleDate1,
                        SettleDate2 = request.SettleDate2,
                        EndDate = request.EndDate,
                        RepurchaseTerm = request.RepurchaseTerm,
                        RepurchaseRate = request.RepurchaseRate,
                        OrderType = request.OrderType,
                        //
                        SymbolFirmInfo = listSymbolFirmMem
                    };
                    OrderMemory.Add_NewOrder(objOrder);
                }
                newReposBCGDModify.ApiOrderNo = request.OrderNo;
                //
                long enqueue = c_IProcessRevEntity.EnqueueData(newReposBCGDModify);
                if (enqueue == -1)
                {
                    _response.ReturnCode = ORDER_RETURNCODE.QUEUE_FULL;
                    _response.Message = ORDER_RETURNMESSAGE.QUEUE_FULL;
                    Logger.ApiLog.Info("Process API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthrough OrderNo {0} fail because queue is full", request.OrderNo);
                    return _response;
                }
                //
                _response.ReturnCode = ORDER_RETURNCODE.SUCCESS;
                _response.Message = ORDER_RETURNMESSAGE.SUCCESS;
                //
                Logger.ApiLog.Info("Process API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthrough OrderNo {0} Success", request.OrderNo);
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthrough_BU with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                return _response;
            }
        }
    }
}