using CommonLib;
using HNXTPRLGate.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;
using static CommonLib.ConfigData;

namespace HNXTPRLGate.Helpers
{
    public static class HelperExtension
    {
        public static UserInfo GetCurrentUser(this HttpContext context)
        {
            try
            {
                var claimsIdentity = context.User.Identities.FirstOrDefault(x => x.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme);
                string _username = claimsIdentity?.Claims?.FirstOrDefault(p => p.Type == ClaimTypes.Name)?.Value ?? "";
                string _role = claimsIdentity?.Claims?.FirstOrDefault(p => p.Type == ClaimTypes.Role)?.Value ?? "";
                if (!string.IsNullOrEmpty(_username))
                {
                    return new UserInfo()
                    {
                        Username = _username,
                        Role = _role
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// Render a partial view to string.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="controller"></param>
        /// <param name="viewNamePath"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<string> RenderViewToStringAsync<TModel>(this Controller controller, string viewNamePath, TModel model)
        {
            if (string.IsNullOrEmpty(viewNamePath))
                viewNamePath = controller.ControllerContext.ActionDescriptor.ActionName;

            controller.ViewData.Model = model;

            using (StringWriter writer = new StringWriter())
            {
                try
                {
                    IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

                    ViewEngineResult viewResult = null;

                    if (viewNamePath.EndsWith(".cshtml"))
                        viewResult = viewEngine.GetView(viewNamePath, viewNamePath, false);
                    else
                        viewResult = viewEngine.FindView(controller.ControllerContext, viewNamePath, false);

                    if (!viewResult.Success)
                    {
                        //return $"A view with the name '{viewNamePath}' could not be found";
                        return string.Empty;
                    }

                    ViewContext viewContext = new ViewContext(
                        controller.ControllerContext,
                        viewResult.View,
                        controller.ViewData,
                        controller.TempData,
                        writer,
                        new HtmlHelperOptions()
                    );

                    await viewResult.View.RenderAsync(viewContext);

                    return writer.GetStringBuilder().ToString();
                }
                catch (Exception ex)
                {
                    //return $"Failed - {exc.Message}";
                    return string.Empty;
                }
            }
        }
    }
}