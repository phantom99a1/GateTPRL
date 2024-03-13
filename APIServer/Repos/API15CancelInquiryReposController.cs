using APIServer.Helpers;
using APIServer.Validation;
using CommonLib;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using BusinessProcessAPIReq;
using BusinessProcessAPIReq.RequestModels;
using BusinessProcessAPIReq.ResponseModels;
using static CommonLib.CommonData;
using LogStation;

namespace APIServer
{
    /// <summary>
    /// 2.3	API15 – Hủy lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
    /// </summary>

    [Route("api/hnxtprl/v1")]
    [ApiController]
    [ServiceFilter(typeof(CustomActionFilter))]
    public class API15CancelInquiryReposController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API15CancelInquiryReposRequest> c_API15CancelInquiryReposRequest_Validator;

        public API15CancelInquiryReposController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API15CancelInquiryReposRequest> _API15CancelInquiryReposRequest_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API15CancelInquiryReposRequest_Validator = _API15CancelInquiryReposRequest_Validator;
        }

        //  2.3	API15 – Hủy lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
        [HttpPost]
        [Route("order/cancel/inquiry-repos")]
        public async Task<API15CancelInquiryReposResponse> API15CancelInquiryRepos(API15CancelInquiryReposRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API15CancelInquiryReposResponse _response = new API15CancelInquiryReposResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API15CancelInquiryRepos with OrderNo: {request.OrderNo}, RefExchangeID={request.RefExchangeID}, Symbol: {request.Symbol}, QuoteType: {request.QuoteType}, OrderType: {request.OrderType}, Text: {request.Text}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API15CancelInquiryReposRequest_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API15CancelInquiryRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API15CancelInquiryRepos_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API15CancelInquiryRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                //
                LogStationFacade.RecordforPT("API15CancelInquiryRepos_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API15CancelInquiryRepos with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API15CancelInquiryRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}