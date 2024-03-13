using APIServer.Helpers;
using APIServer.Validation;
using CommonLib;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using BusinessProcessAPIReq;
using BusinessProcessAPIReq.RequestModels;
using BusinessProcessAPIReq.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CommonLib.CommonData;
using LogStation;

namespace APIServer
{
    /// <summary>
    /// 3.2	API32 – Sửa lệnh giao dịch khớp lệnh
    /// </summary>

    [Route("api/hnxtprl/v1")]
    [ApiController]
    [ServiceFilter(typeof(CustomActionFilter))]
    public class API33OrderCancelAutomaticOrderMatchingController:ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API33OrderCancelAutomaticOrderMatchingRequest> c_API33OrderCancelAutomaticOrderMatchingRequest_Validator;

        public API33OrderCancelAutomaticOrderMatchingController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API33OrderCancelAutomaticOrderMatchingRequest> _API33OrderCancelAutomaticOrderMatchingRequest_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API33OrderCancelAutomaticOrderMatchingRequest_Validator = _API33OrderCancelAutomaticOrderMatchingRequest_Validator;
        }

        [HttpPost]
        [Route("order/cancel/automatic-order-matching")]
        public async Task<API33OrderCancelAutomaticOrderMatchingResponse> API33OrderCancelAutomaticOrderMatching(API33OrderCancelAutomaticOrderMatchingRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API33OrderCancelAutomaticOrderMatchingResponse _response = new API33OrderCancelAutomaticOrderMatchingResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API33OrderCancelAutomaticOrderMatching with OrderNo: {request.OrderNo}, RefExchangeID={request.RefExchangeID},  Text: {request.Text}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API33OrderCancelAutomaticOrderMatchingRequest_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API33OrderCancelAutomaticOrderMatching with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API33OrderCancelAutomaticOrderMatching_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API33OrderCancelAutomaticOrderMatching with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                //
                LogStationFacade.RecordforPT("API33OrderCancelAutomaticOrderMatching_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API33OrderCancelAutomaticOrderMatching with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API33OrderCancelAutomaticOrderMatching with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}
