using ObjectInfo;
using OracleDataAccess;

namespace ManagedLayer
{
    public static class Msg_tprl_outrightBL
    {
        public static int Insert(Msg_Tprl_Outright_Info objData)
        {
            int result = Msg_tprl_outrightDA.Insert(objData);
            return result;
        }
    }
}