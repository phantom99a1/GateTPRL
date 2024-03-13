global using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonLib;
using Microsoft.Extensions.Configuration;

namespace HNXUnitTest
{
    public class InitObject
    {

        //public static ServerCallContext getMockServerCallContext()
        //{

        //    ConfigData.TokenSecret = "7ajntYzxGns8TVaodIgl8w==";
        //    string ct_method = "SendMT598DKGDTPRL";

        //    string ct_host = "localhost";
        //    DateTime ct_deadline = DateTime.Now;
        //    Metadata ct_requestHeaders = new Metadata();
        //    ct_requestHeaders.Add("service_sec_key", "7ajntYzxGns8TVaodIgl8w==");

        //    return TestServerCallContext.Create(ct_method, ct_host, ct_deadline, ct_requestHeaders, new CancellationToken(true), "", null, null, null, null, null);

        //}

        public static void ReadConfigTest()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            ConfigData.InitConfig(config);
        }


    }


}