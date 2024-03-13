using CommonLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APIMonitor.Helpers
{
    public class CustomActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Connection.LocalPort != ConfigData.APIMonitorPort)
            {
                Logger.ApiLog.Warn($"Warning call apiMonitor with port isvalid. LocalPort({context.HttpContext.Connection.LocalPort}) != APIMonitorPort({ConfigData.APIMonitorPort})");
                string p_Indata = null;

                context.Result = new JsonResult(new { returnCode = "400", message = "", inData = p_Indata }) { StatusCode = StatusCodes.Status200OK };
            }

            base.OnActionExecuting(context);
        }
    }
}