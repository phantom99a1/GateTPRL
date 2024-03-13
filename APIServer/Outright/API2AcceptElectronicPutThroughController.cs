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
    public class API2AcceptElectronicPutThroughController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API2AcceptElectronicPutThroughRequest> c_API2AcceptElectronicPutThrough_Validator;

        public API2AcceptElectronicPutThroughController(IProcessRevBussiness _ProcessRevBussiness,
           IValidator<API2AcceptElectronicPutThroughRequest> _api2AcceptElectronicPutThrough_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            //
            c_API2AcceptElectronicPutThrough_Validator = _api2AcceptElectronicPutThrough_Validator;
        }

        // 1.2	API2: Api xác nhận lệnh thỏa thuận điện tử Outright
        [HttpPost]
        [Route("order/confirm/electronic-put-through")]
        public async Task<API2AcceptElectronicPutThroughResponse> API2AcceptElectronicPutThrough(API2AcceptElectronicPutThroughRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API2AcceptElectronicPutThroughResponse _response = new API2AcceptElectronicPutThroughResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API2AcceptElectronicPutThrough with OrderNo: {request.OrderNo}, RefExchangeID: {request.RefExchangeID}, ClientID: {request.ClientID}, OrderType: {request.OrderType}, Side: {request.Side}, Symbol: {request.Symbol}, Price: {request.Price}, OrderQty: {request.OrderQty}, SettleDate: {request.SettleDate}, SettleMethod: {request.SettleMethod}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API2AcceptElectronicPutThrough_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API2AcceptElectronicPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API2AcceptElectronicPutThrough_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API2AcceptElectronicPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message} Processed in {(DateTime.Now.Ticks - t1) * 10} us");
                LogStationFacade.RecordforPT("API1NewElectronicPutThrough_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API2AcceptElectronicPutThrough with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API2AcceptElectronicPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}