using CommonLib;
using Vault;
using Vault.Client;
using Vault.Model;

namespace APIMonitor.Helpers
{
    public static class VaultHelper
    {
        /// <summary>
        /// Hàm đọc dữ liệu với Vault
        /// </summary>
        /// <returns></returns>
        public static string ReadData()
        {
            try
            {
                // config
                VaultConfiguration config = new VaultConfiguration(ConfigData.VaultAddress);
                //
                VaultClient vaultClient = new VaultClient(config);
                if (string.IsNullOrEmpty(VaultSetting.Token))
                {
                    VaultResponse<Object> resp = vaultClient.Auth.UserpassLogin(username: ConfigData.VaultUsername, new UserpassLoginRequest(Password: ConfigData.VaultPassword));
                    VaultSetting.Token = resp.ResponseAuth.ClientToken;
                }
                vaultClient.SetToken(token: VaultSetting.Token);
                //
                VaultResponse<KvV2ReadResponse> kvResp = vaultClient.Secrets.KvV2Read(ConfigData.VaultPath, kvV2MountPath: "secrets");
                //
                Logger.log.Debug($"VaultResponse when read data = {kvResp.Data.Data}");
                if (kvResp.Data.Data != null)
                {
                    Logger.log.Debug($"Response from Vault success when read data.");
                    return kvResp.Data.Data.ToString();
                }
                else
                {
                    Logger.log.Error($"Error responsee from vault when read data with kvResp={kvResp.Data.Data}.");
                    //
                    return "";
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call ReadData, Exception: {ex?.ToString()}");
                return "";
            }
        }

        /// <summary>
        /// Hàm save dữ liệu với Vault 
        /// </summary>
        /// <param name="p_data"></param>
        /// <returns></returns>
        public static bool WriteData(string p_data)
        {
            try
            {
                // config
                VaultConfiguration config = new VaultConfiguration(ConfigData.VaultAddress);
                //
                VaultClient vaultClient = new VaultClient(config);
                //
                if (string.IsNullOrEmpty(VaultSetting.Token))
                {
                    VaultResponse<Object> resp = vaultClient.Auth.UserpassLogin(username: ConfigData.VaultUsername, new UserpassLoginRequest(Password: ConfigData.VaultPassword));
                    VaultSetting.Token = resp.ResponseAuth.ClientToken;
                }
                vaultClient.SetToken(token: VaultSetting.Token);

                //
                try
                {
                    var secretData = new Dictionary<string, string> { { VaultSetting.DataKey, p_data } };
                    // Write a secret
                    var kvRequestData = new KvV2WriteRequest(secretData);
                    vaultClient.Secrets.KvV2Write(ConfigData.VaultPath, kvRequestData, kvV2MountPath: "secrets");
                    Thread.Sleep(1000); // ssi bao sau khi write thì slep lai 1s
                                        // Read a secret
                    VaultResponse<KvV2ReadResponse> kvResp = vaultClient.Secrets.KvV2Read(ConfigData.VaultPath, kvV2MountPath: "secrets");
                    //
                    Logger.log.Debug($"VaultResponse when write data = {kvResp.Data.Data}");
                    if (kvResp.Data.Data != null)
                    {
                        VaultDataResponse res = JsonHelper.Deserialize<VaultDataResponse>(kvResp.Data.Data.ToString());
                        //
                        Logger.log.Debug($"Response from Vault success when execute write data with password with data = {kvResp.Data.Data.ToString()}, VaultDataResponse= {JsonHelper.Serialize(res)}");
                        //
                        if (!string.IsNullOrEmpty(res.MemberPass))
                        {
                            ConfigData.Password = res.MemberPass;
                            return true;
                        }
                        else
                        {
                            Logger.log.Error($"Response from Vault when execute write data with key={VaultSetting.DataKey}, respone MemberPass is null.");
                            return false;
                        }
                    }
                    else
                    {
                        Logger.log.Error($"Error response from Vault fail when execute write data with password new.");
                        //
                        return false;
                    }
                }
                catch (VaultApiException e)
                {
                    Logger.log.Error("Failed to read secret with message {0}", e.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call WriteData,Exception: {ex?.ToString()}");
                return false;
            }
        }
    }
}