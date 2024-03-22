using ObjectInfo;
using OracleDataAccess;

namespace ManagedLayer
{
    public static class Msg_tprl_repoBL
    {
        public static int Insert(Msg_Tprl_Repo_Info objData)
        {
            int result = Msg_tprl_repoDA.Insert(objData);
            return result;
        }
    }
}