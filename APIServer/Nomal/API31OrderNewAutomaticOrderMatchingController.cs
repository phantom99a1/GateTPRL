using APIServer.Helpers;
using APIServer.Validation;
using BusinessProcessAPIReq;
using BusinessProcessAPIReq.RequestModels;
using BusinessProcessAPIReq.ResponseModels;
using CommonLib;
using FluentValidation;
using LogStation;
using Microsoft.AspNetCore.Mvc;
using static CommonLib.CommonData;

namespace APIServer
{
    /// <summary>
    /// 3.1	API31 – Đặt lệnh giao dịch khớp lệnh
    /// </summary>

    [Route("api/hnxtprl/v1")]
    [ApiController]
    [ServiceFilter(typeof(CustomActionFilter))]
    public class API31OrderNewAutomaticOrderMatchingController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API31OrderNewAutomaticOrderMatchingRequest> c_API31OrderNewAutomaticOrderMatchingRequest_Validator;

        public API31OrderNewAutomaticOrderMatchingController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API31OrderNewAutomaticOrderMatchingRequest> _API31OrderNewAutomaticOrderMatchingRequest_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API31OrderNewAutomaticOrderMatchingRequest_Validator = _API31OrderNewAutomaticOrderMatchingRequest_Validator;
        }

        [HttpPost]
        [Route("order/new/automatic-order-matching")]
        public async Task<API31OrderNewAutomaticOrderMatchingResponse> API31OrderNewAutomaticOrderMatching(API31OrderNewAutomaticOrderMatchingRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API31OrderNewAutomaticOrderMatchingResponse _response = new API31OrderNewAutomaticOrderMatchingResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API31OrderNewAutomaticOrderMatching with OrderNo: {request.OrderNo}, ClientID={request.ClientID}, Symbol: {request.Symbol}, Side: {request.Side}, OrderType: {request.OrderType}, OrderQty: {request.OrderQty}, Price={request.Price}, OrderQtyMM2: {request.OrderQtyMM2}, PriceMM2: {request.PriceMM2}, SpecialType: {request.SpecialType}, Text: {request.Text}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API31OrderNewAutomaticOrderMatchingRequest_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API31OrderNewAutomaticOrderMatching with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }                
			    //
			    _response.InData = request;
                _response = await c_processRevBussiness.API31OrderNewAutomaticOrderMatching_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API31OrderNewAutomaticOrderMatching with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                //
                LogStationFacade.RecordforPT("API31OrderNewAutomaticOrderMatching_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API31OrderNewAutomaticOrderMatching with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API31OrderNewAutomaticOrderMatching with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}