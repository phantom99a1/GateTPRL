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
    /// 2.1	API13 – Đặt lệnh điện tử tùy chọn Inquiry Repos
    /// </summary>

    [Route("api/hnxtprl/v1")]
    [ApiController]
    [ServiceFilter(typeof(CustomActionFilter))]
    public class API13NewInquiryReposController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API13NewInquiryReposRequest> c_API13NewInquiryReposRequest_Validator;

        public API13NewInquiryReposController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API13NewInquiryReposRequest> _API13NewInquiryReposRequest_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API13NewInquiryReposRequest_Validator = _API13NewInquiryReposRequest_Validator;
        }

        //  2.1	API13 – Đặt lệnh điện tử tùy chọn Inquiry Repos
        [HttpPost]
        [Route("order/new/inquiry-repos")]
        public async Task<API13NewInquiryReposResponse> API13NewInquiryRepos(API13NewInquiryReposRequest request)
        {
            API13NewInquiryReposResponse _response = new API13NewInquiryReposResponse();
            try
            {
                long t1 = DateTime.Now.Ticks;
                Logger.ApiLog.Info($"Start call api API13NewInquiryRepos with OrderNo: {request.OrderNo}, Symbol: {request.Symbol}, QuoteType: {request.QuoteType}, OrderType: {request.OrderType}, Side: {request.Side}, OrderValue: {request.OrderValue}, EffectiveTime: {request.EffectiveTime}, SettleMethod: {request.SettleMethod}, SettleDate1: {request.SettleDate1}, SettleDate2: {request.SettleDate2}, EndDate: {request.EndDate}, RepurchaseTerm: {request.RepurchaseTerm}, RegistID: {request.RegistID}, Text: {request.Text}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API13NewInquiryReposRequest_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API13NewInquiryRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API13NewInquiryRepos_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API13NewInquiryRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                //
                LogStationFacade.RecordforPT("API13NewInquiryRepos_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API13NewInquiryRepos with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API13NewInquiryRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}