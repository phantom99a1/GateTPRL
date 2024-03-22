using ObjectInfo;
using OracleDataAccess;

namespace ManagedLayer
{
    public static class Msg_tprl_hnx_confirmBL
    {
        public static int Insert(Msg_Tprl_Hnx_Confirm_Info objData)
        {
            int result = Msg_tprl_hnx_confirmDA.Insert(objData);
            return result;
        }
    }
}