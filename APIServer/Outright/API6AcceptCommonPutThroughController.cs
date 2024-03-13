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
    public class API6AcceptCommonPutThroughController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API6AcceptCommonPutThroughRequest> c_API6AcceptCommonPutThrough_Validator;

        public API6AcceptCommonPutThroughController(IProcessRevBussiness _ProcessRevBussiness,
           IValidator<API6AcceptCommonPutThroughRequest> _api6AcceptCommonPutThrough_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API6AcceptCommonPutThrough_Validator = _api6AcceptCommonPutThrough_Validator;
        }

        // 1.6	API6: Api chấp nhận thỏa thuận Báo cáo giao dịch Outright
        [HttpPost]
        [Route("order/confirm/outright-common-put-through")]
        public async Task<API6AcceptCommonPutThroughResponse> API6AcceptCommonPutThrough(API6AcceptCommonPutThroughRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API6AcceptCommonPutThroughResponse _response = new API6AcceptCommonPutThroughResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API6AcceptCommonPutThrough with OrderNo: {request.OrderNo}, RefExchangeID: {request.RefExchangeID}, ClientIDBuy: {request.ClientID}, ClientIDCounterFirm: {request.ClientIDCounterFirm}, MemberCounterFirm: {request.MemberCounterFirm}, OrderType: {request.OrderType}, Side: {request.Side}, Symbol: {request.Symbol}, Price: {request.Price}, OrderQty: {request.OrderQty}, SettleDate: {request.SettleDate}, SettleMethod: {request.SettleMethod}, CrossType: {request.CrossType}, EffectiveTime: {request.EffectiveTime}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API6AcceptCommonPutThrough_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API6AcceptCommonPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message} Processed in {(DateTime.Now.Ticks - t1) /10} us");
                    LogStationFacade.RecordforPT("API1NewElectronicPutThrough_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API6AcceptCommonPutThrough_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API6AcceptCommonPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message} Processed in {(DateTime.Now.Ticks - t1) / 10} us"); 
                LogStationFacade.RecordforPT("API6AcceptCommonPutThrough", DateTime.Now.Ticks - t1, true, "ApiServer");
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API6AcceptCommonPutThrough with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API6AcceptCommonPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}