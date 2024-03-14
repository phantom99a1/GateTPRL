using APIMonitor.Helpers;
using CommonLib;

namespace HNXTPRLGate.Workers
{
    public static class VaultService
    {
        //protected override Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //	Logger.log.Info($"Start thread VaultService");
        //	try
        //	{
        //		Task.Run(() =>
        //		{
        //		}, stoppingToken);
        //	}
        //	catch (Exception ex)
        //	{
        //		Logger.log.Error($"Error start service VaultService when execute ExecuteAsync auto, Exception: {ex?.ToString()}");
        //	}
        //	Logger.log.Info($"END thread VaultService");
        //	return Task.CompletedTask;
        //}

        public static void AutoCallVaultService()
        {
            try
            {
                string _kvRespData = VaultHelper.ReadData();
                VaultDataResponse res = JsonHelper.Deserialize<VaultDataResponse>(_kvRespData);
                if (!string.IsNullOrEmpty(res.MemberPass))
                {
                    ConfigData.Password = res.MemberPass;
                }
                else
                {
                    Logger.log.Error($"Response from Vault when call function AutoCallVaultService process read data with key={VaultSetting.DataKey}, respone MemberPass is null.");
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call AutoCallVaultService when process get info data ,Exception: {ex?.ToString()}");
            }
        }
    }
}