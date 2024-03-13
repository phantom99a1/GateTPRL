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
    /// 2.8	API20 – Xác nhận lệnh điện tử tùy chọn Firm Repos
    /// </summary>

    [Route("api/hnxtprl/v1")]
    [ApiController]
    [ServiceFilter(typeof(CustomActionFilter))]
    public class API20OrderConfirmFirmReposController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API20OrderConfirmFirmReposRequest> c_API20OrderConfirmFirmReposRequest_Validator;

        public API20OrderConfirmFirmReposController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API20OrderConfirmFirmReposRequest> _API20OrderConfirmFirmReposRequest_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API20OrderConfirmFirmReposRequest_Validator = _API20OrderConfirmFirmReposRequest_Validator;
        }

        [HttpPost]
        [Route("order/confirm/firm-repos")]
        public async Task<API20OrderConfirmFirmReposResponse> API20OrderConfirmFirmRepos(API20OrderConfirmFirmReposRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API20OrderConfirmFirmReposResponse _response = new API20OrderConfirmFirmReposResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API20OrderConfirmFirmRepos with OrderNo: {request.OrderNo}, RefExchangeID={request.RefExchangeID}, QuoteType: {request.QuoteType}, OrderType: {request.OrderType}, Text: {request.Text}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API20OrderConfirmFirmReposRequest_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API20OrderConfirmFirmRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API20OrderConfirmFirmRepos_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API20OrderConfirmFirmRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                //
                LogStationFacade.RecordforPT("API20OrderConfirmFirmRepos_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API20OrderConfirmFirmRepos with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API20OrderConfirmFirmRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}