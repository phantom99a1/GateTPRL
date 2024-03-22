using ObjectInfo;
using OracleDataAccess;

namespace ManagedLayer
{
    public static class Msg_tprl_securitiesBL
    {
        public static int Insert(Msg_Tprl_Securities_Info objData)
        {
            int result = Msg_tprl_securitiesDA.Insert(objData);
            return result;
        }
    }
}