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
    /// 2.12	API24 – Hủy lệnh thỏa thuận báo cáo giao dịch Repos chưa thực hiện
    /// </summary>
    [Route("api/hnxtprl/v1")]
    [ApiController]
    [ServiceFilter(typeof(CustomActionFilter))]
    public class API24OrderCancelReposCommonPutthroughController:ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API24OrderCancelReposCommonPutthroughRequest> c_API24OrderCancelReposCommonPutthroughRequest;

        public API24OrderCancelReposCommonPutthroughController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API24OrderCancelReposCommonPutthroughRequest> _API24OrderCancelReposCommonPutthroughRequest_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API24OrderCancelReposCommonPutthroughRequest = _API24OrderCancelReposCommonPutthroughRequest_Validator;
        }

        [HttpPost]
        [Route("order/cancel/repos-common-put-through")]
        public async Task<API24OrderCancelReposCommonPutthroughResponse> API24OrderCancelReposCommonPutthrough(API24OrderCancelReposCommonPutthroughRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API24OrderCancelReposCommonPutthroughResponse _response = new API24OrderCancelReposCommonPutthroughResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API24OrderCancelReposCommonPutthrough with OrderNo: {request.OrderNo}, RefExchangeID={request.RefExchangeID}, QuoteType: {request.QuoteType}, OrderType: {request.OrderType}, Side: {request.Side}, Text: {request.Text}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API24OrderCancelReposCommonPutthroughRequest, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API24OrderCancelReposCommonPutthrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API24OrderCancelReposCommonPutthrough_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API24OrderCancelReposCommonPutthrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                //
                LogStationFacade.RecordforPT("API24OrderCancelReposCommonPutthrough_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API24OrderCancelReposCommonPutthrough with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API24OrderCancelReposCommonPutthrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}
