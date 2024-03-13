using CommonLib;
using Vault;
using Vault.Client;
using Vault.Model;

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
                // Call sang Vault để lấy thông tin
                VaultConfiguration config = new VaultConfiguration(ConfigData.VaultAddress);
                VaultClient vaultClient = new VaultClient(config);
                //
                try
                {
                    if (string.IsNullOrEmpty(VaultSetting.Token))
                    {
                        VaultResponse<Object> resp = vaultClient.Auth.UserpassLogin(username: ConfigData.VaultUsername, new UserpassLoginRequest(Password: ConfigData.VaultPassword));
                        VaultSetting.Token = resp.ResponseAuth.ClientToken;
                    }
                    vaultClient.SetToken(token: VaultSetting.Token);

                    // Read a secret
                    VaultResponse<KvV2ReadResponse> kvResp = vaultClient.Secrets.KvV2Read(ConfigData.VaultPath, kvV2MountPath: "secrets");
                    //
                    Logger.log.Debug($"VaultResponse when write data = {kvResp.Data.Data} when call function AutoCallVaultService ");
                    if (kvResp.Data.Data != null)
                    {
                        VaultDataResponse res = JsonHelper.Deserialize<VaultDataResponse>(kvResp.Data.Data.ToString());

                        Logger.log.Debug($"Response from Vault success when call function AutoCallVaultService with data = {kvResp.Data.Data.ToString()}, VaultDataResponse= {JsonHelper.Serialize(res)}");

                        if (!string.IsNullOrEmpty(res.MemberPass))
                        {
                            ConfigData.Password = res.MemberPass;
                        }
                        else
                        {
                            Logger.log.Error($"Response from Vault when call function AutoCallVaultService process read data with key={VaultSetting.DataKey}, respone MemberPass is null.");
                        }
                        //
                        Logger.log.Debug($"Response from Vault success when call function AutoCallVaultService. VaultDataResponse={System.Text.Json.JsonSerializer.Serialize(res)}");
                    }
                    else
                    {
                        Logger.log.Error($"Error responsee from vault when call function AutoCallVaultService with kvResp={kvResp} .");
                    }
                }
                catch (VaultApiException e)
                {
                    Logger.log.Error("Failed to read secret with message {0} when call function AutoCallVaultService", e.Message);
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call AutoCallVaultService when process get info data ,Exception: {ex?.ToString()}");
            }
        }
    }
}