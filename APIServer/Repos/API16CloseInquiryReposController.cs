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
    /// 2.4	API16 – Đóng lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
    /// </summary>

    [Route("api/hnxtprl/v1")]
    [ApiController]
    [ServiceFilter(typeof(CustomActionFilter))]
    public class API16CloseInquiryReposController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API16CloseInquiryReposRequest> c_API16CloseInquiryReposRequest_Validator;

        public API16CloseInquiryReposController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API16CloseInquiryReposRequest> _API16CloseInquiryReposRequest_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API16CloseInquiryReposRequest_Validator = _API16CloseInquiryReposRequest_Validator;
        }

        //  2.4	API16 – Đóng lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
        [HttpPost]
        [Route("order/close/inquiry-repos")]
        public async Task<API16CloseInquiryReposResponse> API16CloseInquiryRepos(API16CloseInquiryReposRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API16CloseInquiryReposResponse _response = new API16CloseInquiryReposResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API16CloseInquiryRepos with OrderNo: {request.OrderNo}, RefExchangeID={request.RefExchangeID}, Symbol: {request.Symbol}, QuoteType: {request.QuoteType}, OrderType: {request.OrderType}, Text: {request.Text}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API16CloseInquiryReposRequest_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API16CloseInquiryRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API16CloseInquiryRepos_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API16CloseInquiryRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                //
                LogStationFacade.RecordforPT("API16CloseInquiryRepos_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API16CloseInquiryRepos with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API16CloseInquiryRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}