using CommonLib;
using HNXInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Xml.Linq;

namespace APIServer.Helpers
{
    public class CustomActionFilter : ActionFilterAttribute
    {
        private iHNXClient c_iHNXClient;

        public CustomActionFilter(iHNXClient p_HNXClient)
        {
            c_iHNXClient = p_HNXClient;
        }

        //
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Connection.LocalPort != ConfigData.APIBusinessPort)
            {
                context.Result = new JsonResult(new { }) { StatusCode = StatusCodes.Status404NotFound };
            }
            //
            string authorization = context.HttpContext.Request.Headers["service_sec_key"].FirstOrDefault();
            string p_Indata = null;
            if (!string.Equals(authorization, ConfigData.TokenSecret))
            {
                Logger.ApiLog.Warn($"Warning call apiServer with TokenSecret isvalid!");
                context.Result = new JsonResult(new { returnCode = ErrorCodeDefine.Error_UnAuthorized, message = "ERROR_UNAUTHORIZED", inData = p_Indata }) { StatusCode = StatusCodes.Status200OK };
            }

            if (c_iHNXClient.ClientStatus() != enumClientStatus.DATA_TRANSFER)
            {
                Logger.ApiLog.Warn($"Warning call apiServer with ClientStatus != DATA_TRANSFER ");
                context.Result = new JsonResult(new { returnCode = CommonData.ORDER_RETURNCODE.BOND_DMA_DISCONNECTED, message = CommonData.ORDER_RETURNMESSAGE.BOND_DMA_DISCONNECTED, inData = p_Indata }) { StatusCode = StatusCodes.Status200OK };
            }
            //
            base.OnActionExecuting(context);
        }
    }
}