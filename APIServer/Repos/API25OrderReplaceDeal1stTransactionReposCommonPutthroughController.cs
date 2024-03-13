using APIServer.Helpers;
using APIServer.Validation;
using BusinessProcessAPIReq;
using BusinessProcessAPIReq.RequestModels;
using BusinessProcessAPIReq.ResponseModels;
using CommonLib;
using FluentValidation;
using LogStation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static CommonLib.CommonData;

namespace APIServer
{
    /// <summary>
    /// 2.13	API25 – Sửa lệnh thỏa thuận Repos đã thực hiện trong ngày (sửa GD Repos)
    /// </summary>
    [Route("api/hnxtprl/v1")]
    [ApiController]
    [ServiceFilter(typeof(CustomActionFilter))]
    public class API25OrderReplaceDeal1stTransactionReposCommonPutthroughController
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest> c_API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest_Validator;

        public API25OrderReplaceDeal1stTransactionReposCommonPutthroughController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest> _API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest_Validator = _API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest_Validator;
        }

        [HttpPost]
        [Route("order/replace-deal/sale/repos-common-put-through")]
        public async Task<API25OrderReplaceDeal1stTransactionReposCommonPutthroughResponse> API25OrderReplaceDeal1stTransactionReposCommonPutthrough(API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API25OrderReplaceDeal1stTransactionReposCommonPutthroughResponse _response = new API25OrderReplaceDeal1stTransactionReposCommonPutthroughResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API25OrderReplaceDeal1stTransactionReposCommonPutthrough with OrderNo: {request.OrderNo}, RefExchangeID: {request.RefExchangeID}, QuoteType: {request.QuoteType}, OrderType: {request.OrderType}, Side: {request.Side}, ClientID={request.ClientID}, EffectiveTime: {request.EffectiveTime}, SettleMethod: {request.SettleMethod}, SettleDate1: {request.SettleDate1}, SettleDate2: {request.SettleDate2}, EndDate={request.EndDate}, RepurchaseTerm: {request.RepurchaseTerm}, RepurchaseRate: {request.RepurchaseRate}, Text: {request.Text}, NoSide: {request.NoSide}, SymbolFirmInfo={JsonSerializer.Serialize(request.SymbolFirmInfo)}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API25OrderReplaceDeal1stTransactionReposCommonPutthrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API25OrderReplaceDeal1stTransactionReposCommonPutthrough_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API25OrderReplaceDeal1stTransactionReposCommonPutthrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                //
                LogStationFacade.RecordforPT("API25OrderReplaceDeal1stTransactionReposCommonPutthrough_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API25OrderReplaceDeal1stTransactionReposCommonPutthrough with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API25OrderReplaceDeal1stTransactionReposCommonPutthrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}