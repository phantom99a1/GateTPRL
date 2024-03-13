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
    /// 2.17	API29 – Sửa lệnh thỏa thuận Repos mua lại đảo ngược (sửa GD Reverse Repos)
    /// </summary>
    [Route("api/hnxtprl/v1")]
    [ApiController]
    [ServiceFilter(typeof(CustomActionFilter))]
    public class API29OrderReplaceDeal2ndTransactionReposCommonPutthroughController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest> c_API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest_Validator;

        public API29OrderReplaceDeal2ndTransactionReposCommonPutthroughController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest> _API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest_Validator = _API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest_Validator;
        }

        [HttpPost]
        [Route("order/replace-deal/repurchase/repos-common-put-through")]
        public async Task<API29OrderReplaceDeal2ndTransactionReposCommonPutthroughResponse> API29OrderReplaceDeal2ndTransactionReposCommonPutthrough(API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API29OrderReplaceDeal2ndTransactionReposCommonPutthroughResponse _response = new API29OrderReplaceDeal2ndTransactionReposCommonPutthroughResponse();
            try
            {
                Logger.ApiLog.Info(message: $"Start call api API29OrderReplaceDeal2ndTransactionReposCommonPutthrough with OrderNo: {request.OrderNo}, RefExchangeID={request.RefExchangeID}, QuoteType: {request.QuoteType}, OrderType: {request.OrderType}, Side: {request.Side}, ClientID={request.ClientID}, EffectiveTime: {request.EffectiveTime}, SettleMethod: {request.SettleMethod}, SettleDate1: {request.SettleDate1}, SettleDate2: {request.SettleDate2}, EndDate={request.EndDate}, RepurchaseTerm: {request.RepurchaseTerm}, RepurchaseRate: {request.RepurchaseRate}, Text: {request.Text}, NoSide: {request.NoSide}, SymbolFirmInfo={JsonSerializer.Serialize(request.SymbolFirmInfo)}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API29OrderReplaceDeal2ndTransactionReposCommonPutthrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API29OrderReplaceDeal2ndTransactionReposCommonPutthrough_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API29OrderReplaceDeal2ndTransactionReposCommonPutthrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                //
                LogStationFacade.RecordforPT("API29OrderReplaceDeal2ndTransactionReposCommonPutthrough_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API29OrderReplaceDeal2ndTransactionReposCommonPutthrough with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API29OrderReplaceDeal2ndTransactionReposCommonPutthrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}