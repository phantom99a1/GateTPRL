using ObjectInfo;
using OracleDataAccess;

namespace ManagedLayer
{
    public static class Msg_tprl_orderBL
    {
        public static int Insert(Msg_Tprl_Order_Info objData)
        {
            int result = Msg_tprl_orderDA.Insert(objData);
            return result;
        }
    }
}