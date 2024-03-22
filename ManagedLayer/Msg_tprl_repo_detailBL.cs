using ObjectInfo;
using OracleDataAccess;

namespace ManagedLayer
{
    public static class Msg_tprl_repo_detailBL
    {
        public static int Insert(Msg_Tprl_Repo_Detail_Info objData)
        {
            int result = Msg_tprl_repo_detailDA.Insert(objData);
            return result;
        }
    }
}