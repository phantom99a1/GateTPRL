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
    public class API1NewElectronicPutThroughController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API1NewElectronicPutThroughRequest> c_API1NewElectronicPutThrough_Validator;

        public API1NewElectronicPutThroughController(IProcessRevBussiness _ProcessRevBussiness, IValidator<API1NewElectronicPutThroughRequest> _api1NewElectronicPutThrough_Validator
            )
        {
            c_processRevBussiness = _ProcessRevBussiness;
            //
            c_API1NewElectronicPutThrough_Validator = _api1NewElectronicPutThrough_Validator;
        }

        // 1.1	API1: Api đặt lệnh thỏa thuận điện tử Outright
        [HttpPost]
        [Route("order/new/electronic-put-through")]
        public async Task<API1NewElectronicPutThroughResponse> API1NewElectronicPutThrough(API1NewElectronicPutThroughRequest request)
        {

            API1NewElectronicPutThroughResponse _response = new API1NewElectronicPutThroughResponse();
            try
            {
                long t1 = DateTime.Now.Ticks;
                //
                Logger.ApiLog.Info($"Start call api API1NewElectronicPutThrough with OrderNo: {request.OrderNo}, ClientID: {request.ClientID}, OrderType: {request.OrderType}, Side: {request.Side}, Symbol: {request.Symbol}, Price: {request.Price}, OrderQty: {request.OrderQty}, SettleDate: {request.SettleDate}, SettleMethod: {request.SettleMethod}, RegistID: {request.RegistID}, IsVisible: {request.IsVisible}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API1NewElectronicPutThrough_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
					//
					Logger.ApiLog.Info($"End call api API1NewElectronicPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API1NewElectronicPutThrough_BU(request, _response);
				//
				Logger.ApiLog.Info($"End call api API1NewElectronicPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}, Processed in {(DateTime.Now.Ticks - t1) *10} us");
                LogStationFacade.RecordforPT("API1NewElectronicPutThrough_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API1NewElectronicPutThrough with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API1NewElectronicPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}