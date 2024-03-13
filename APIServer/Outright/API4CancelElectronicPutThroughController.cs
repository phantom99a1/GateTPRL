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
    public class API4CancelElectronicPutThroughController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API4CancelElectronicPutThroughRequest> c_API4CancelElectronicPutThrough_Validator;

        public API4CancelElectronicPutThroughController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API4CancelElectronicPutThroughRequest> _api4CancelElectronicPutThrough_Validator
            )
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API4CancelElectronicPutThrough_Validator = _api4CancelElectronicPutThrough_Validator;
        }

        // 1.4	API4: Api huỷ thỏa thuận điện tử Outright chưa thực hiện
        [HttpPost]
        [Route("order/cancel/electronic-put-through")]
        public async Task<API4CancelElectronicPutThroughResponse> API4CancelElectronicPutThrough(API4CancelElectronicPutThroughRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API4CancelElectronicPutThroughResponse _response = new API4CancelElectronicPutThroughResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API4CancelElectronicPutThrough with OrderNo: {request.OrderNo}, RefExchangeID: {request.RefExchangeID}, OrderType: {request.OrderType}, Symbol: {request.Symbol}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API4CancelElectronicPutThrough_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API4CancelElectronicPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API4CancelElectronicPutThrough_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API4CancelElectronicPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message} Processed in {(DateTime.Now.Ticks - t1) / 10} us");
                LogStationFacade.RecordforPT("API4CancelElectronicPutThrough", DateTime.Now.Ticks - t1, true, "ApiServer");
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API4CancelElectronicPutThrough with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API4CancelElectronicPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}