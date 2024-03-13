using CommonLib;
using HNXTPRLGate.Helpers;
using HNXTPRLGate.OtelTracing;
using HNXTPRLGate.Startups;
using HNXTPRLGate.Workers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

string ConfigFolder = "./ConfigApp"; //can be relative or fullpath
string ConfigLogFile = "ConfigLog/nlog-windows.config";
string ConfigFile = "appsettings.json";
if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
	ConfigFile = "appsettings.json";
	ConfigLogFile = "../ConfigLog/nlog.config";
	ConfigFolder = "../ConfigApp";
}
var logger = NLogBuilder.ConfigureNLog(ConfigLogFile).GetCurrentClassLogger();

#if DEBUG
logger.Info($"Run on Platform [{Environment.OSVersion.Platform}] - Debug version");
#elif RELEASE
logger.Info ($"Run on Platform [{Environment.OSVersion.Platform}] - Release version[2201]");
#endif

Version version = Assembly.GetEntryAssembly().GetName().Version;

#if DEBUG
logger.Info($"Start on DEBUG[2301] mode - Version:{version.ToString()} -  Platform:{Environment.OSVersion.Platform} - PID:{Process.GetCurrentProcess().Id.ToString()} - App start: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff")}");
#elif RELEASE
logger.Info($"Start on RELEASE[2301] mode - Version:{version.ToString()} - Platform:{Environment.OSVersion.Platform} - PID:{Process.GetCurrentProcess().Id.ToString()} - App start: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff")}");
#endif

var ErrorTextPath = Path.Combine(ConfigFolder, "ErrorCodeText.csv"); ;
IConfiguration configuration = null; //so it can be used on other configuration functions bellow

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
	Args = args,
	WebRootPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot")), //point where if serving static files from default location. Works for my case. May need some adjustments in other cases.
});

builder.WebHost.ConfigureAppConfiguration(
(hostingContext, config) =>
{
	var path = Path.Combine(Path.GetFullPath(ConfigFolder), ConfigFile);
	config.AddJsonFile(path, optional: false, reloadOnChange: true);
	config.AddEnvironmentVariables();
	configuration = config.Build();
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opts =>
{
	opts.Cookie.Name = "MONITOR_HNXTPRL_Auth";
	opts.ExpireTimeSpan = TimeSpan.FromMinutes(30);
	opts.SlidingExpiration = true;
});
// Add services to the container.
builder.Services.AddControllersWithViews();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "JWTToken_Auth_API",
		Version = "v1"
	});
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
	{
		Name = "TOKEN",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer string_token\"",
	});
	c.AddSecurityRequirement(new OpenApiSecurityRequirement {
		{
			new OpenApiSecurityScheme {
				Reference = new OpenApiReference {
					Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
				}
			},
			new string[] {}
		}
	});
	c.OperationFilter<CustomHeaderSwaggerAttribute>();
});
builder.Host.UseNLog();
// Init configs
CommonLib.ConfigData.InitConfig(builder.Configuration);
CommonLib.ConfigData.ReadErrorCodeData(ErrorTextPath);
LogStation.LogStationFacade.InitConfig(30, CommonLib.Logger.ApiLog);



Starting.InitServices(builder.Services);

builder.Services.RegisterOpentelemetry(builder.Configuration);

builder.Services.AddSingleton<Instrumentation>(provider =>
{
	var options = new OtelAppSettings();
	builder.Configuration.GetSection(options.SectionName).Bind(options);
	return new Instrumentation(options);
});

var app = builder.Build();
//
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAllCors");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.Lifetime.ApplicationStopped.Register(WriteLogStopApplication);
app.MapControllers();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();

void WriteLogStopApplication()
{
	logger.Info($"[GateHNXTPRL] Stop Application - {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff")}");
}