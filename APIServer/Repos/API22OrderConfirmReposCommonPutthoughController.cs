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
    /// 2.10	API22 – Xác nhận lệnh thỏa thuận báo cáo giao dịch Repos
    /// </summary>

    [Route("api/hnxtprl/v1")]
    [ApiController]
    [ServiceFilter(typeof(CustomActionFilter))]
    public class API22OrderConfirmReposCommonPutthoughController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API22OrderConfirmReposCommonPutthroughRequest> c_API22OrderConfirmReposCommonPutthroughRequest_Validator;

        public API22OrderConfirmReposCommonPutthoughController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API22OrderConfirmReposCommonPutthroughRequest> _API22OrderConfirmReposCommonPutthroughRequest_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API22OrderConfirmReposCommonPutthroughRequest_Validator = _API22OrderConfirmReposCommonPutthroughRequest_Validator;
        }

        [HttpPost]
        [Route("order/confirm/repos-common-put-through")]
        public async Task<API22OrderConfirmReposCommonPutthroughResponse> API22OrderConfirmReposCommonPutthrough(API22OrderConfirmReposCommonPutthroughRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API22OrderConfirmReposCommonPutthroughResponse _response = new API22OrderConfirmReposCommonPutthroughResponse();
            try
            {
                Logger.ApiLog.Info(message: $"Start call api API22OrderConfirmReposCommonPutthrough with OrderNo: {request.OrderNo}, RefExchangeID={request.RefExchangeID}, QuoteType: {request.QuoteType}, OrderType: {request.OrderType}, Side: {request.Side}, ClientID={request.ClientID}, EffectiveTime: {request.EffectiveTime}, SettleMethod: {request.SettleMethod}, SettleDate1: {request.SettleDate1}, SettleDate2: {request.SettleDate2}, EndDate={request.EndDate}, RepurchaseTerm: {request.RepurchaseTerm}, RepurchaseRate: {request.RepurchaseRate}, Text: {request.Text}, NoSide: {request.NoSide}, SymbolFirmInfo={JsonSerializer.Serialize(request.SymbolFirmInfo)}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API22OrderConfirmReposCommonPutthroughRequest_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API22OrderConfirmReposCommonPutthrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API22OrderConfirmReposCommonPutthrough_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API22OrderConfirmReposCommonPutthrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                //
                LogStationFacade.RecordforPT("API22OrderConfirmReposCommonPutthrough_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API22OrderConfirmReposCommonPutthrough with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API22OrderConfirmReposCommonPutthrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}