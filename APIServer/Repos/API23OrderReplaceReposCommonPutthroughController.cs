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
    /// 2.11	API23 – Sửa lệnh thỏa thuận báo cáo giao dịch Repos chưa thực hiện
    /// </summary>

    [Route("api/hnxtprl/v1")]
    [ApiController]
    [ServiceFilter(typeof(CustomActionFilter))]
    public class API23OrderReplaceReposCommonPutthroughController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API23OrderReplaceReposCommonPutthroughRequest> c_API23OrderReplaceReposCommonPutthroughRequest_Validator;

        public API23OrderReplaceReposCommonPutthroughController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API23OrderReplaceReposCommonPutthroughRequest> _API23OrderReplaceReposCommonPutthroughRequest_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API23OrderReplaceReposCommonPutthroughRequest_Validator = _API23OrderReplaceReposCommonPutthroughRequest_Validator;
        }

        [HttpPost]
        [Route("order/replace/repos-common-put-through")]
        public async Task<API23OrderReplaceReposCommonPutthroughResponse> API23OrderReplaceReposCommonPutthrough(API23OrderReplaceReposCommonPutthroughRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API23OrderReplaceReposCommonPutthroughResponse _response = new API23OrderReplaceReposCommonPutthroughResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API23OrderReplaceReposCommonPutthrough with OrderNo: {request.OrderNo}, RefExchangeID={request.RefExchangeID}, QuoteType: {request.QuoteType}, OrderType: {request.OrderType}, Side: {request.Side}, ClientID={request.ClientID}, EffectiveTime: {request.EffectiveTime}, SettleMethod: {request.SettleMethod}, SettleDate1: {request.SettleDate1}, SettleDate2: {request.SettleDate2}, EndDate={request.EndDate}, RepurchaseTerm: {request.RepurchaseTerm}, RepurchaseRate: {request.RepurchaseRate}, Text: {request.Text}, NoSide: {request.NoSide}, SymbolFirmInfo={JsonSerializer.Serialize(request.SymbolFirmInfo)}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API23OrderReplaceReposCommonPutthroughRequest_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API23OrderReplaceReposCommonPutthrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API23OrderReplaceReposCommonPutthrough_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API23OrderReplaceReposCommonPutthrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                //
                LogStationFacade.RecordforPT("API23OrderReplaceReposCommonPutthrough_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API23OrderReplaceReposCommonPutthrough with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API23OrderReplaceReposCommonPutthrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}