using CommonLib;
using HNXTPRLGate.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static CommonLib.ConfigData;

namespace HNXTPRLGate.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            var _user = HttpContext.GetCurrentUser();
            if (_user != null)
            {
                return Redirect("/");
            }
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            int _code = -1;
            string _message = "Username or Password invalid";
            try
            {
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    username = username.Trim();
                    password = password.Trim();
                    //
                    UserInfo userInfo = UserHelper.GetUserByUserName(username);
                    if (userInfo != null)
                    {
                        bool verifyPass = UserHelper.VerifyPassword(username, password, userInfo.Password);
                        if (verifyPass)
                        {
                            var claims = new List<Claim> {
                                new Claim(ClaimTypes.Name, userInfo.Username),
                                new Claim(ClaimTypes.Role, userInfo.Role),
                            };
                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                            //
                            _code = 1;
                            _message = "";
                            //
                            Logger.log.Info($"Login success with user name: {username} - IP Source: {HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()} - action: {HttpContext.Request.Method}");
                        }
                        else
                        {
                            _message = "Username or Password invalid";
                            Logger.log.Info($"Login fail with user name: {username} - IP Source: {HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()} - action: {HttpContext.Request.Method} when verify password");
                        }
                    }
                    else
                    {
                        _message = "Username or Password invalid";
                        //
                        Logger.log.Info($"Login fail with user name: {username} - IP Source: {HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()} - action: {HttpContext.Request.Method} when fill user name");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error when process Login with userName={username}, Exception: {ex?.ToString()}");
            }
            return Json(new { code = _code, message = _message });
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                UserInfo userInfo = HttpContext.GetCurrentUser();
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                //
                Logger.log.Info($"Login out with user name: {userInfo?.Username} - IP Source: {HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()} - action: {HttpContext.Request.Method}");

                return Json(new { code = 1, message = "logout success" });
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error when process LogOut, Exception: {ex?.ToString()}");
                return Json(new { code = -1, message = "logout fail" });
            }
        }
    }
}