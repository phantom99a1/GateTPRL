using APIServer.Helpers;
using APIServer.Validation;
using CommonLib;
using FluentValidation;
using LogStation;
using Microsoft.AspNetCore.Mvc;
using BusinessProcessAPIReq;
using BusinessProcessAPIReq.RequestModels;
using BusinessProcessAPIReq.ResponseModels;
using static CommonLib.CommonData;

namespace APIServer
{
    [Route("api/hnxtprl/v1")]
    [ApiController]
    [ServiceFilter(typeof(CustomActionFilter))]
    public class API12ResponseForCancelingCommonPutThroughDealController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;

        private readonly IValidator<API12ResponseForCancelingCommonPutThroughDealRequest> c_API12ResponseForCancelingCommonPutThroughDeal_Validator;

        public API12ResponseForCancelingCommonPutThroughDealController(IProcessRevBussiness _ProcessRevBussiness,
             IValidator<API12ResponseForCancelingCommonPutThroughDealRequest> _api12ResponseForCancelingCommonPutThroughDeal_Validator
            )
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API12ResponseForCancelingCommonPutThroughDeal_Validator = _api12ResponseForCancelingCommonPutThroughDeal_Validator;
        }

        // 1.12	API12: API phản hồi hủy lệnh thỏa thuận Outright đã thực
        [HttpPost]
        [Route("order/confirm-cancel-deal/outright-common-put-through")]
        public async Task<API12ResponseForCancelingCommonPutThroughDealResponse> API12ResponseForCancelingCommonPutThroughDeal(API12ResponseForCancelingCommonPutThroughDealRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API12ResponseForCancelingCommonPutThroughDealResponse _response = new API12ResponseForCancelingCommonPutThroughDealResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API12ResponseForCancelingCommonPutThroughDeal with OrderNo: {request.OrderNo}, RefExchangeID: {request.RefExchangeID}, OrderType: {request.OrderType}, Side: {request.Side}, Symbol: {request.Symbol}, CrossType: {request.CrossType}, Text: {request.Text}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API12ResponseForCancelingCommonPutThroughDeal_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API12ResponseForCancelingCommonPutThroughDeal with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API12ResponseForCancelingCommonPutThroughDeal_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API12ResponseForCancelingCommonPutThroughDeal with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message} Processed in {(DateTime.Now.Ticks - t1) / 10} us");
                LogStationFacade.RecordforPT("API1NewElectronicPutThrough_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API12ResponseForCancelingCommonPutThroughDeal with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API12ResponseForCancelingCommonPutThroughDeal with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}