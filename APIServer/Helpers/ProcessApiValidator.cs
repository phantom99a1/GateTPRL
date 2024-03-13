using APIServer.Validation;
using CommonLib;
using FluentValidation;
using static CommonLib.CommonData;

namespace APIServer.Helpers
{
    public class ProcessApiValidator
    {
        public async Task<DataResponseValidator> ValidateMessageRequestAsync<T>(IValidator<T> validator, T request)
        {
            try
            {
                var validResult = await validator.ValidateAsync(request);
                if (!validResult.IsValid)
                {
                    DataResponseValidator _dataResponse = new DataResponseValidator();
                    _dataResponse.ErrorCode = validResult.Errors?.FirstOrDefault().ErrorCode;
                    _dataResponse.ErrorMessage = validResult.Errors?.FirstOrDefault().ErrorMessage;
                    return _dataResponse;
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.ApiLog.Error($"Error call api ValidateMessageRequestAsync with request: {request}, Exception: {ex?.ToString()}");
                //
                DataResponseValidator _dataResponse = new DataResponseValidator();
                _dataResponse.ErrorCode = ErrorCodeDefine.Error_Application;
                _dataResponse.ErrorMessage = ORDER_RETURNMESSAGE.Application_Error;
                return _dataResponse;
            }
        }
    }
}