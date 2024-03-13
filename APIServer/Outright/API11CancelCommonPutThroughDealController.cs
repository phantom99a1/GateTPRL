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
    public class API11CancelCommonPutThroughDealController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API11CancelCommonPutThroughDealRequest> c_API11CancelCommonPutThroughDeal_Validator;

        //
        public API11CancelCommonPutThroughDealController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API11CancelCommonPutThroughDealRequest> _api11CancelCommonPutThroughDeal_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API11CancelCommonPutThroughDeal_Validator = _api11CancelCommonPutThroughDeal_Validator;
        }

        //
        // 1.11	API11: API hủy lệnh thỏa thuận Outright đã thực hiện
        [HttpPost]
        [Route("order/cancel-deal/outright-common-put-through")]
        public async Task<API11CancelCommonPutThroughDealResponse> API11CancelCommonPutThroughDeal(API11CancelCommonPutThroughDealRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API11CancelCommonPutThroughDealResponse _response = new API11CancelCommonPutThroughDealResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API11CancelCommonPutThroughDeal with OrderNo: {request.OrderNo}, RefExchangeID: {request.RefExchangeID}, OrderType: {request.OrderType}, Side: {request.Side}, Symbol: {request.Symbol}, CrossType: {request.CrossType}, Text: {request.Text}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API11CancelCommonPutThroughDeal_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API11CancelCommonPutThroughDeal with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API11CancelCommonPutThroughDeal_BU(request, _response);
                //

                LogStationFacade.RecordforPT("API1NewElectronicPutThrough_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                Logger.ApiLog.Info($"End call api API11CancelCommonPutThroughDeal with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message} Processed in {(DateTime.Now.Ticks - t1)/ 10} us");
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API11CancelCommonPutThroughDeal with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API11CancelCommonPutThroughDeal with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}