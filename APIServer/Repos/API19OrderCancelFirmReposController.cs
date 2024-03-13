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
    /// 2.7	API19 – Hủy lệnh điện tử tùy chọn Firm Repos chưa thực hiện
    /// </summary>
    ///
    [Route("api/hnxtprl/v1")]
    [ApiController]
    [ServiceFilter(typeof(CustomActionFilter))]
    public class API19OrderCancelFirmReposController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API19OrderCancelFirmReposRequest> c_API19OrderCancelFirmReposRequest_Validator;

        public API19OrderCancelFirmReposController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API19OrderCancelFirmReposRequest> _API19OrderCancelFirmReposRequest_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API19OrderCancelFirmReposRequest_Validator = _API19OrderCancelFirmReposRequest_Validator;
        }

        [HttpPost]
        [Route("order/cancel/firm-repos")]
        public async Task<API19OrderCancelFirmReposResponse> API19OrderCancelFirmRepos(API19OrderCancelFirmReposRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API19OrderCancelFirmReposResponse _response = new API19OrderCancelFirmReposResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API19OrderCancelFirmRepos with OrderNo: {request.OrderNo}, RefExchangeID={request.RefExchangeID}, QuoteType: {request.QuoteType}, OrderType: {request.OrderType}, Text: {request.Text}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API19OrderCancelFirmReposRequest_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API19OrderCancelFirmRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API19OrderCancelFirmRepos_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API19OrderCancelFirmRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                //
                LogStationFacade.RecordforPT("API19OrderCancelFirmRepos_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API19OrderCancelFirmRepos with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API19OrderCancelFirmRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}