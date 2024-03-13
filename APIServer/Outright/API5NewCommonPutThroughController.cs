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
    public class API5NewCommonPutThroughController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API5NewCommonPutThroughRequest> c_API5NewCommonPutThrough_Validator;

        public API5NewCommonPutThroughController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API5NewCommonPutThroughRequest> _api5NewCommonPutThrough_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            //
            c_API5NewCommonPutThrough_Validator = _api5NewCommonPutThrough_Validator;
        }

        // 1.5	API5: Api đặt lệnh thỏa thuận Báo cáo giao dịch Outright
        [HttpPost]
        [Route("order/new/outright-common-put-through")]
        public async Task<API5NewCommonPutThroughResponse> API5NewCommonPutThrough(API5NewCommonPutThroughRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API5NewCommonPutThroughResponse _response = new API5NewCommonPutThroughResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API5NewCommonPutThrough with OrderNo: {request.OrderNo}, ClientID: {request.ClientID}, ClientIDCounterfirm: {request.ClientIDCounterFirm}, MemberCounterFirm: {request.MemberCounterFirm}, OrderType: {request.OrderType}, Side: {request.Side}, Symbol: {request.Symbol}, Price: {request.Price}, OrderQty: {request.OrderQty}, SettleDate: {request.SettleDate}, SettleMethod: {request.SettleMethod}, CrossType: {request.CrossType}, EffectiveTime: {request.EffectiveTime}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API5NewCommonPutThrough_Validator, request);

                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API5NewCommonPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API5NewCommonPutThrough_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API5NewCommonPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message} Processed in {(DateTime.Now.Ticks - t1)/10} us");
                LogStationFacade.RecordforPT("API5NewCommonPutThrough", DateTime.Now.Ticks - t1, true, "ApiServer");
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API5API55NewCommonPutThrough with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API5NewCommonPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}