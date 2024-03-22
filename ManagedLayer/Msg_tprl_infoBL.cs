using ObjectInfo;
using OracleDataAccess;

namespace ManagedLayer
{
    public static class Msg_tprl_infoBL
    {
        public static int Insert(Msg_Tprl_Info_Info objData)
        {
            int result = Msg_tprl_infoDA.Insert(objData);
            return result;
        }
    }
}