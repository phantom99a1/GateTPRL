using APIServer.Helpers;
using APIServer.Validation;
using BusinessProcessAPIReq;
using BusinessProcessAPIReq.RequestModels;
using BusinessProcessAPIReq.ResponseModels;
using CommonLib;
using FluentValidation;
using LogStation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static CommonLib.CommonData;

namespace APIServer
{
    /// <summary>
    /// 2.5	API17 – Đặt lệnh điện tử tùy chọn Firm Repos
    /// </summary>

    [Route("api/hnxtprl/v1")]
    [ApiController]
    [ServiceFilter(typeof(CustomActionFilter))]
    public class API17OrderNewFirmReposController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API17OrderNewFirmReposRequest> c_API17OrderNewFirmReposRequest_Validator;

        public API17OrderNewFirmReposController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API17OrderNewFirmReposRequest> _API17OrderNewFirmReposRequest_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API17OrderNewFirmReposRequest_Validator = _API17OrderNewFirmReposRequest_Validator;
        }

        //  2.5	API17 – Đặt lệnh điện tử tùy chọn Firm Repos
        [HttpPost]
        [Route("order/new/firm-repos")]
        public async Task<API17OrderNewFirmReposResponse> API17OrderNewFirmRepos(API17OrderNewFirmReposRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API17OrderNewFirmReposResponse _response = new API17OrderNewFirmReposResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API17OrderNewFirmRepos with OrderNo: {request.OrderNo}, RefExchangeID={request.RefExchangeID}, QuoteType: {request.QuoteType}, OrderType: {request.OrderType}, Side: {request.Side}, ClientID={request.ClientID}, EffectiveTime: {request.EffectiveTime}, SettleMethod: {request.SettleMethod}, SettleDate1: {request.SettleDate1}, SettleDate2: {request.SettleDate2}, EndDate={request.EndDate}, RepurchaseTerm: {request.RepurchaseTerm}, RepurchaseRate: {request.RepurchaseRate}, Text: {request.Text}, NoSide: {request.NoSide}, SymbolFirmInfo={JsonSerializer.Serialize(request.SymbolFirmInfo)}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API17OrderNewFirmReposRequest_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API17OrderNewFirmRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API17OrderNewFirmRepos_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API17OrderNewFirmRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                //
                LogStationFacade.RecordforPT("API17OrderNewFirmRepos_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API17OrderNewFirmRepos with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API17OrderNewFirmRepos with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}