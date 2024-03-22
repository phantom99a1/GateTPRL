using ObjectInfo;
using OracleDataAccess;

namespace ManagedLayer
{
    public static class Msg_tprl_requestBL
    {
        public static int Insert(Msg_Tprl_Request_Info objData)
        {
            int result = Msg_tprl_requestDA.Insert(objData);
            return result;
        }
    }
}