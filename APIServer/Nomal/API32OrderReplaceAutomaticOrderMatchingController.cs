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
    public class API32OrderReplaceAutomaticOrderMatchingController : ControllerBase
    {
        private readonly IProcessRevBussiness c_processRevBussiness;
        private readonly IValidator<API32OrderReplaceAutomaticOrderMatchingRequest> c_API32OrderReplaceAutomaticOrderMatchingRequest_Validator;

        public API32OrderReplaceAutomaticOrderMatchingController(IProcessRevBussiness _ProcessRevBussiness,
            IValidator<API32OrderReplaceAutomaticOrderMatchingRequest> _API32OrderReplaceAutomaticOrderMatchingRequest_Validator)
        {
            c_processRevBussiness = _ProcessRevBussiness;
            c_API32OrderReplaceAutomaticOrderMatchingRequest_Validator = _API32OrderReplaceAutomaticOrderMatchingRequest_Validator;
        }

        [HttpPost]
        [Route("order/replace/automatic-order-matching")]
        public async Task<API32OrderReplaceAutomaticOrderMatchingResponse> API32OrderReplaceAutomaticOrderMatching(API32OrderReplaceAutomaticOrderMatchingRequest request)
        {
            long t1 = DateTime.Now.Ticks;
            API32OrderReplaceAutomaticOrderMatchingResponse _response = new API32OrderReplaceAutomaticOrderMatchingResponse();
            try
            {
                Logger.ApiLog.Info($"Start call api API32OrderReplaceAutomaticOrderMatching with OrderNo: {request.OrderNo}, RefExchangeID={request.RefExchangeID}, ClientID={request.ClientID}, Symbol: {request.Symbol}, OrderQty: {request.OrderQty}, OrgOrderQty={request.OrgOrderQty}, Price: {request.Price},  Text: {request.Text}");
                //
                request.TrimStringProperty();
                //
                DataResponseValidator validatorResponse = await new ProcessApiValidator().ValidateMessageRequestAsync(c_API32OrderReplaceAutomaticOrderMatchingRequest_Validator, request);
                if (validatorResponse != null)
                {
                    _response.ReturnCode = validatorResponse.ErrorCode;
                    _response.Message = validatorResponse.ErrorMessage;
                    //
                    Logger.ApiLog.Info($"End call api API32OrderReplaceAutomaticOrderMatching with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                    //
                    return _response;
                }
                //
                _response.InData = request;
                _response = await c_processRevBussiness.API32OrderReplaceAutomaticOrderMatching_BU(request, _response);
                //
                Logger.ApiLog.Info($"End call api API32OrderReplaceAutomaticOrderMatching with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                //
                LogStationFacade.RecordforPT("API32OrderReplaceAutomaticOrderMatching_BU", DateTime.Now.Ticks - t1, true, "ApiServer");
                //
                return _response;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api API32OrderReplaceAutomaticOrderMatching with OrderNo: {request.OrderNo}, Exception: {ex?.ToString()}");
                //
                _response.ReturnCode = ErrorCodeDefine.Error_Application;
                _response.Message = ORDER_RETURNMESSAGE.Application_Error;
                //
                Logger.ApiLog.Info($"End call api API32OrderReplaceAutomaticOrderMatching with OrderNo: {request.OrderNo}; Repsonse =>   ReturnCode: {_response.ReturnCode}, Message: {_response.Message}");
                return _response;
            }
        }
    }
}