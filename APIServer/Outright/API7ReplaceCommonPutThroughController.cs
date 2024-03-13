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
    public class API7ReplaceCommonPutThroughController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API7ReplaceCommonPutThroughRequest> c_API7ReplaceCommonPutThrough_Validator;

        public API7ReplaceCommonPutThroughController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API7ReplaceCommonPutThroughRequest> _api7ReplaceCommonPutThrough_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            //
            c_API7ReplaceCommonPutThrough_Validator = _api7ReplaceCommonPutThrough_Validator;
        }

        // 1.7	API7: API sửa lệnh thỏa thuận báo cáo giao dịch Outright chưa thực hiện
        [HttpPost]
        [Route("order/replace/outright-common-put-through")]
        public async Task<API7ReplaceCommonPutThroughResponse> API7ReplaceCommonPutThrough(API7ReplaceCommonPutThroughRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API7ReplaceCommonPutThroughResponse _response = new API7ReplaceCommonPutThroughResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API7ReplaceCommonPutThrough with OrderNo: {request.OrderNo}, RefExchangeID: {request.RefExchangeID}, ClientIDBuy: {request.ClientID}, ClientIDCounterFirm: {request.ClientIDCounterFirm}, MemberCounterFirm: {request.MemberCounterFirm}, OrderType: {request.OrderType}, Side: {request.Side}, Symbol: {request.Symbol}, Price: {request.Price}, OrderQty: {request.OrderQty}, SettleDate: {request.SettleDate}, SettleMethod: {request.SettleMethod}, CrossType: {request.CrossType}, EffectiveDate: {request.EffectiveTime}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API7ReplaceCommonPutThrough_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API7ReplaceCommonPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API7ReplaceCommonPutThrough_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API7ReplaceCommonPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message} Processed in {(DateTime.Now.Ticks - t1) / 10} us");
                LogStationFacade.RecordforPT("API7ReplaceCommonPutThrough", DateTime.Now.Ticks - t1, true, "ApiServer");
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API7ReplaceCommonPutThrough with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API7ReplaceCommonPutThrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}