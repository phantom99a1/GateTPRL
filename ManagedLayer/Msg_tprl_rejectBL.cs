using ObjectInfo;
using OracleDataAccess;

namespace ManagedLayer
{
    public static class Msg_tprl_rejectBL
    {
        public static int Insert(Msg_Tprl_Reject_Info objData)
        {
            int result = Msg_tprl_rejectDA.Insert(objData);
            return result;
        }
    }
}