using CommonLib;
using HNXTPRLGate.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static CommonLib.ConfigData;

namespace HNXTPRLGate.Helpers
{
    public class AuthActionFilter : ActionFilterAttribute
    {
        private string[] Roles { get; set; }

        public AuthActionFilter(params string[] roles)
        {
            Roles = roles;
        }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                // our code before action executes
                AuthenticateResult authResult = await context.HttpContext.AuthenticateAsync();
                bool _IsLoggedInAndHadRight = true;
                string _RedirectTo = "/";

                //
                if (authResult == null || authResult.Succeeded == false)
                {
                    _IsLoggedInAndHadRight = false;
                    _RedirectTo = "/login";
                }
                else
                {
                    UserInfo userInfo = context.HttpContext.GetCurrentUser();
                    if (userInfo == null || string.IsNullOrEmpty(userInfo.Username))
                    {
                        _IsLoggedInAndHadRight = false;
                        _RedirectTo = "/login";
                    }
                    else if (Roles?.Length > 0 && Roles.Contains(userInfo.Role) == false)
                    {
                        _IsLoggedInAndHadRight = false;
                        _RedirectTo = "/hasnoright";
                    }
                }

                //
                if (_IsLoggedInAndHadRight == false)
                {
                    if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        var objJson = new { code = -1, message = "Bạn không có quyền thực hiện chức năng này!", redirectTo = _RedirectTo };
                        context.Result = new JsonResult(objJson);
                    }
                    else
                    {
                        context.Result = new RedirectResult(_RedirectTo);
                    }
                }
                else
                {
                    // Continue action
                    base.OnActionExecuting(context);
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var objJson = new { code = -1, message = "Error", redirectTo = "/error" };
                    context.Result = new JsonResult(objJson);
                }
                else
                {
                    context.Result = new RedirectResult("/error");
                }
            }
        }
    }
}