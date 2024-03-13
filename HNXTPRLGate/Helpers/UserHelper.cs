using CommonLib;
using HNXTPRLGate.Models;
using Newtonsoft.Json;
using static CommonLib.ConfigData;

namespace HNXTPRLGate.Helpers
{
    public class UserHelper
    {
        //public static List<UserInfo> GetListUsers()
        //{
        //    try
        //    {
        //        string _usersfile = AppContext.BaseDirectory + Path.DirectorySeparatorChar + "users.json";
        //        if (File.Exists(_usersfile))
        //        {
        //            string _text = File.ReadAllText(_usersfile);
        //            if (!string.IsNullOrEmpty(_text))
        //            {
        //                return JsonConvert.DeserializeObject<List<UserInfo>>(_text);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.log.Error(ex);
        //    }
        //    return null;
        //}

        public static UserInfo GetUserByUserName(string userName)
        {
            try
            {
                List<UserInfo> listUsers = ConfigData.listUsers;
                if (!string.IsNullOrEmpty(userName) && listUsers != null && listUsers.Count > 0)
                {
                    for (int i = 0; i < listUsers.Count; i++)
                    {
                        UserInfo objUser = listUsers[i];
                        if (objUser.Username.Equals(userName, StringComparison.OrdinalIgnoreCase))
                        {
                            return objUser;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
            return null;
        }

       

        public static bool VerifyPassword(string userName, string password, string hashedPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                {
                    return false;
                }
                return BCrypt.Net.BCrypt.Verify(string.Format("{0};#{1}", password, userName.ToLower()), hashedPassword);
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error VerifyPassword with userName={userName}, Exception: {ex?.ToString()}");
                return false;
            }

        }
    }
}