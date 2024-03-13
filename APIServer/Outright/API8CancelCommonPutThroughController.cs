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
    public class API8CancelCommonPutThroughController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API8CancelCommonPutThroughRequest> c_API8CancelCommonPutThrough_Validator;

        //
        public API8CancelCommonPutThroughController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API8CancelCommonPutThroughRequest> _api8CancelCommonPutThrough_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            //
            c_API8CancelCommonPutThrough_Validator = _api8CancelCommonPutThrough_Validator;
        }

        //

        // 1.8	API8: API hủy lệnh thỏa thuận báo cáo giao dịch Outright chưa thực hiện
        [HttpPost]
        [Route("order/cancel/outright-common-put-through")]
        public async Task<API8CancelCommonPutThroughResponse> API8CancelCommonPutThrough(API8CancelCommonPutThroughRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API8CancelCommonPutThroughResponse _response = new API8CancelCommonPutThroughResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API8CancelCommonPutThrough with OrderNo: {request.OrderNo}, RefExchangeID: {request.RefExchangeID}, OrderType: {request.OrderType}, Side: {request.Side}, Symbol: {request.Symbol}, CrossType: {request.CrossType}, Text: {request.Text}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API8CancelCommonPutThrough_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API8CancelCommonPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API8CancelCommonPutThrough_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API8CancelCommonPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message} Processed in {(DateTime.Now.Ticks - t1) / 10} us");
                LogStationFacade.RecordforPT("API1NewElectronicPutThrough_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API8CancelCommonPutThrough with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API8CancelCommonPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}