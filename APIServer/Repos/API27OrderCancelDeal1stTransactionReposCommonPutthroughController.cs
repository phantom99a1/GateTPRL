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
    /// 2.15	API27 – Hủy lệnh thỏa thuận Repos đã thực hiện trong ngày (hủy GD Repos)
    /// </summary>
    [Route("api/hnxtprl/v1")]
    [ApiController]
    [ServiceFilter(typeof(CustomActionFilter))]
    public class API27OrderCancelDeal1stTransactionReposCommonPutthroughController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest> c_API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest;

        public API27OrderCancelDeal1stTransactionReposCommonPutthroughController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest> _API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest = _API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest_Validator;
        }

        [HttpPost]
        [Route("order/cancel-deal/sale/repos-common-put-through")]
        public async Task<API27OrderCancelDeal1stTransactionReposCommonPutthroughResponse> API27OrderCancelDeal1stTransactionReposCommonPutthrough(API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API27OrderCancelDeal1stTransactionReposCommonPutthroughResponse _response = new API27OrderCancelDeal1stTransactionReposCommonPutthroughResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API27OrderCancelDeal1stTransactionReposCommonPutthrough with OrderNo: {request.OrderNo}, RefExchangeID={request.RefExchangeID}, QuoteType: {request.QuoteType}, OrderType: {request.OrderType}, Side: {request.Side}, Text: {request.Text}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API27OrderCancelDeal1stTransactionReposCommonPutthrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API27OrderCancelDeal1stTransactionReposCommonPutthrough_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API27OrderCancelDeal1stTransactionReposCommonPutthrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                //
                LogStationFacade.RecordforPT("API27OrderCancelDeal1stTransactionReposCommonPutthrough_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API27OrderCancelDeal1stTransactionReposCommonPutthrough with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API27OrderCancelDeal1stTransactionReposCommonPutthrough with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}