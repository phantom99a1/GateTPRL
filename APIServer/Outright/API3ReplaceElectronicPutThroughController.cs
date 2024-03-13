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
    public class API3ReplaceElectronicPutThroughController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API3ReplaceElectronicPutThroughRequest> c_API3ReplaceElectronicPutThrough_Validator;

        public API3ReplaceElectronicPutThroughController(IProcessRevBussiness _ProcessRevBussiness,
           IValidator<API3ReplaceElectronicPutThroughRequest> _api3ReplaceElectronicPutThrough_Validator
           )
        {
            c_processRevBussiness = _ProcessRevBussiness;
            //
            c_API3ReplaceElectronicPutThrough_Validator = _api3ReplaceElectronicPutThrough_Validator;
        }

        // 1.3	API3: Api sửa thỏa thuận điện tử Outright chưa thực hiện
        [HttpPost]
        [Route("order/replace/electronic-put-through")]
        public async Task<API3ReplaceElectronicPutThroughResponse> API3ReplaceElectronicPutThrough(API3ReplaceElectronicPutThroughRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API3ReplaceElectronicPutThroughResponse _response = new API3ReplaceElectronicPutThroughResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API3ReplaceElectronicPutThrough with OrderNo: {request.OrderNo}, RefExchangeID: {request.RefExchangeID}, ClientID: {request.ClientID}, OrderType: {request.OrderType}, Side: {request.Side}, Symbol: {request.Symbol}, Price: {request.Price}, OrderQty: {request.OrderQty}, SettleDate: {request.SettleDate}, SettleMethod: {request.SettleMethod}, RegistID: {request.RegistID}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API3ReplaceElectronicPutThrough_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API3ReplaceElectronicPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API3ReplaceElectronicPutThrough_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API3ReplaceElectronicPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message} Processed in {(DateTime.Now.Ticks - t1) * 10} us");
                LogStationFacade.RecordforPT("API3ReplaceElectronicPutThrough", DateTime.Now.Ticks - t1, true, "ApiServer");
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API3ReplaceElectronicPutThrough with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API3ReplaceElectronicPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}