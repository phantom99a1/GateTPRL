using CommonLib;
using ObjectInfo;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Security.Principal;

namespace OracleDataAccess
{
    public static class Msg_tprl_hnx_confirmDA
    {
        public static int Insert(Msg_Tprl_Hnx_Confirm_Info objData)
        {
            int result = CodeDefine.None_Process;
            try
            {
                long _timeRev = DateTime.Now.Ticks;
                Logger.log.Info($"Start process Msg_tprl_hnx_confirmDA.Insert with Msgseqnum: {objData.Msgseqnum}, Msgtype: {objData.Msgtype}");
                //

                OracleParameter returnParam = new OracleParameter("p_return", OracleDbType.Int64, ParameterDirection.Output);

                OracleHelper.ExecuteNonQuery(ConfigData.gConnectionString,
                    CommandType.StoredProcedure, "pkg_msg_tprl_hnx_confirm.proc_insert",
                    new OracleParameter("p_id", OracleDbType.Decimal, objData.Id, ParameterDirection.Input),
                    new OracleParameter("p_sor", OracleDbType.Varchar2, objData.Sor, ParameterDirection.Input),
                    new OracleParameter("p_msgtype", OracleDbType.Varchar2, objData.Msgtype, ParameterDirection.Input),
                    new OracleParameter("p_sendercompid", OracleDbType.Varchar2, objData.Sendercompid, ParameterDirection.Input),
                    new OracleParameter("p_targetcompid", OracleDbType.Varchar2, objData.Targetcompid, ParameterDirection.Input),
                    new OracleParameter("p_msgseqnum", OracleDbType.Decimal, objData.Msgseqnum, ParameterDirection.Input),
                    new OracleParameter("p_possdupflag", OracleDbType.Varchar2, objData.Possdupflag, ParameterDirection.Input),
                    new OracleParameter("p_sendingtime", OracleDbType.Varchar2, objData.Sendingtime, ParameterDirection.Input),
                    new OracleParameter("p_text", OracleDbType.Varchar2, objData.Text, ParameterDirection.Input),
                    new OracleParameter("p_exectype", OracleDbType.Decimal, objData.Exectype, ParameterDirection.Input),
                    new OracleParameter("p_lastmsgseqnumprocessed", OracleDbType.Varchar2, objData.Lastmsgseqnumprocessed, ParameterDirection.Input),
                    new OracleParameter("p_ordstatus", OracleDbType.Varchar2, objData.Ordstatus, ParameterDirection.Input),
                    new OracleParameter("p_orderid", OracleDbType.Varchar2, objData.Orderid, ParameterDirection.Input),
                    new OracleParameter("p_clordid", OracleDbType.Varchar2, objData.Clordid, ParameterDirection.Input),
                    new OracleParameter("p_symbol", OracleDbType.Varchar2, objData.Symbol, ParameterDirection.Input),
                    new OracleParameter("p_side", OracleDbType.Varchar2, objData.Side, ParameterDirection.Input),
                    new OracleParameter("p_orderqty", OracleDbType.Decimal, objData.Orderqty, ParameterDirection.Input),
                    new OracleParameter("p_ordtype", OracleDbType.Varchar2, objData.Ordtype, ParameterDirection.Input),
                    new OracleParameter("p_price", OracleDbType.Decimal, objData.Price, ParameterDirection.Input),
                    new OracleParameter("p_account", OracleDbType.Varchar2, objData.Account, ParameterDirection.Input),
                    new OracleParameter("p_settlvalue", OracleDbType.Decimal, objData.Settlvalue, ParameterDirection.Input),
                    new OracleParameter("p_leavesqty", OracleDbType.Decimal, objData.Leavesqty, ParameterDirection.Input),
                    new OracleParameter("p_origclordid", OracleDbType.Varchar2, objData.Origclordid, ParameterDirection.Input),
                    new OracleParameter("p_lastqty", OracleDbType.Decimal, objData.Lastqty, ParameterDirection.Input),
                    new OracleParameter("p_lastpx", OracleDbType.Decimal, objData.Lastpx, ParameterDirection.Input),
                    new OracleParameter("p_execid", OracleDbType.Varchar2, objData.Execid, ParameterDirection.Input),
                    new OracleParameter("p_reciprocalmember", OracleDbType.Varchar2, objData.Reciprocalmember, ParameterDirection.Input),
                    new OracleParameter("p_ordrejreason", OracleDbType.Varchar2, objData.Ordrejreason, ParameterDirection.Input),
                    new OracleParameter("p_underlyinglastqty", OracleDbType.Varchar2, objData.Underlyinglastqty, ParameterDirection.Input),
                    new OracleParameter("p_remark", OracleDbType.Varchar2, objData.Remark, ParameterDirection.Input),
                    new OracleParameter("p_lastchange", OracleDbType.Varchar2, objData.Lastchange, ParameterDirection.Input),
                    new OracleParameter("p_createtime", OracleDbType.Varchar2, objData.Createtime, ParameterDirection.Input),
                    returnParam
                );

                if (int.TryParse(returnParam.Value?.ToString() ?? String.Empty, out int returnValue))
                {
                    result = returnValue;
                }
                //
                Logger.log.Info($"End process Msg_tprl_hnx_confirmDA.Insert with Msgseqnum: {objData.Msgseqnum}, Msgtype: {objData.Msgtype}, response time: {DateTime.Now.Ticks - _timeRev}");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call Msg_tprl_hnx_confirmDA.Insert with Msgseqnum: {objData.Msgseqnum}, Msgtype: {objData.Msgtype}, Object={System.Text.Json.JsonSerializer.Serialize(objData)}, Exception: {ex?.ToString()}");
            }

            return result;
        }
    }
}