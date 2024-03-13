using APIServer.Helpers;
using APIServer.Validation;
using CommonLib;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using BusinessProcessAPIReq.RequestModels;
using BusinessProcessAPIReq.ResponseModels;
using BusinessProcessAPIReq;
using static CommonLib.CommonData;
using LogStation;

namespace APIServer
{
    /// <summary>
    /// 2.2	API14 – Sửa lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
    /// </summary>
    [Route("api/hnxtprl/v1")]
    [ApiController]
    [ServiceFilter(typeof(CustomActionFilter))]
    public class API14ReplaceInquiryReposController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API14ReplaceInquiryReposRequest> c_API14ReplaceInquiryReposRequest_Validator;

        public API14ReplaceInquiryReposController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API14ReplaceInquiryReposRequest> _API14ReplaceInquiryReposRequest_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API14ReplaceInquiryReposRequest_Validator = _API14ReplaceInquiryReposRequest_Validator;
        }

        //  2.2	API14 – Sửa lệnh điện tử tùy chọn Inquiry Repos chờ chào giá
        [HttpPost]
        [Route("order/replace/inquiry-repos")]
        public async Task<API14ReplaceInquiryReposResponse> API14ReplaceInquiryRepos(API14ReplaceInquiryReposRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API14ReplaceInquiryReposResponse _response = new API14ReplaceInquiryReposResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API14ReplaceInquiryRepos with OrderNo: {request.OrderNo}, RefExchangeID={request.RefExchangeID}, Symbol: {request.Symbol}, QuoteType: {request.QuoteType}, OrderType: {request.OrderType}, Side: {request.Side}, OrderValue: {request.OrderValue}, EffectiveTime: {request.EffectiveTime}, SettleMethod: {request.SettleMethod}, SettleDate1: {request.SettleDate1}, SettleDate2: {request.SettleDate2}, EndDate: {request.EndDate}, RepurchaseTerm: {request.RepurchaseTerm}, RegistID: {request.RegistID}, Text: {request.Text}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API14ReplaceInquiryReposRequest_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API14ReplaceInquiryRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
               
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API14ReplaceInquiryRepos_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API14ReplaceInquiryRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                //
                LogStationFacade.RecordforPT("API14ReplaceInquiryRepos_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API14ReplaceInquiryRepos with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API14ReplaceInquiryRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}