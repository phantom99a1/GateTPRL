using ObjectInfo;
using OracleDataAccess;

namespace ManagedLayer
{
    public static class Msg_tprl_sesionBL
    {
        public static int Insert(Msg_Tprl_Sesion_Info objData)
        {
            int result = Msg_tprl_sesionDA.Insert(objData);
            return result;
        }
    }
}