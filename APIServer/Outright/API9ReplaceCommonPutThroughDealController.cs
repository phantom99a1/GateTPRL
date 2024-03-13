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
    public class API9ReplaceCommonPutThroughDealController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API9ReplaceCommonPutThroughDealRequest> c_API9ReplaceCommonPutThroughDeal_Validator;

        //
        public API9ReplaceCommonPutThroughDealController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API9ReplaceCommonPutThroughDealRequest> _api9ReplaceCommonPutThroughDeal_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API9ReplaceCommonPutThroughDeal_Validator = _api9ReplaceCommonPutThroughDeal_Validator;
        }

        //

        // 1.9	API9: API sửa lệnh thỏa thuận Outright đã thực hiện
        [HttpPost]
        [Route("order/replace-deal/outright-common-put-through")]
        public async Task<API9ReplaceCommonPutThroughDealResponse> API9ReplaceCommonPutThroughDeal(API9ReplaceCommonPutThroughDealRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API9ReplaceCommonPutThroughDealResponse _response = new API9ReplaceCommonPutThroughDealResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API9ReplaceCommonPutThroughDeal with OrderNo: {request.OrderNo}, RefExchangeID: {request.RefExchangeID}, ClientIDBuy: {request.ClientID}, ClientIDCounterFirm: {request.ClientIDCounterFirm}, MemberCounterFirm: {request.MemberCounterFirm}, OrderType: {request.OrderType}, Side: {request.Side}, Symbol: {request.Symbol}, Price: {request.Price}, OrderQty: {request.OrderQty}, SettleDate: {request.SettleDate}, SettleMethod: {request.SettleMethod}, CrossType: {request.CrossType}, EffectiveDate: {request.EffectiveTime}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API9ReplaceCommonPutThroughDeal_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API9ReplaceCommonPutThroughDeal with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API9ReplaceCommonPutThroughDeal_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API9ReplaceCommonPutThroughDeal with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message} Processed in {(DateTime.Now.Ticks - t1) /10} us");
                LogStationFacade.RecordforPT("API9ReplaceCommonPutThroughDeal", DateTime.Now.Ticks - t1, true, "ApiServer");
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API9ReplaceCommonPutThroughDeal with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API9ReplaceCommonPutThroughDeal with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}