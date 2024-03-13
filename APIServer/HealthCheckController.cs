using APIServer.Helpers;
using APIServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace APIServer
{
    [Route("api/hnxtprl/v1")]
    [ApiController]
    [ServiceFilter(typeof(CustomActionFilter))]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        [Route("healthcheck")]
        public ActionResult<HealthCheckModel> HealthCheck()
        {
            HealthCheckModel healthCheckModel = new HealthCheckModel();
            healthCheckModel.Version = Assembly.GetEntryAssembly().GetName().Version.ToString();
            healthCheckModel.TimeServer = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return healthCheckModel;
        }
    }
}