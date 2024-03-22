using CommonLib;
using ObjectInfo;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace OracleDataAccess
{
    public static class Msg_tprl_sesionDA
    {
        public static int Insert(Msg_Tprl_Sesion_Info objData)
        {
            int result = CodeDefine.None_Process;
            try
            {
                long _timeRev = DateTime.Now.Ticks;
                Logger.log.Info($"Start process Msg_tprl_sesionDA.Insert with Msgseqnum: {objData.Msgseqnum}, Msgtype: {objData.Msgtype}");
                //

                OracleParameter returnParam = new OracleParameter("p_return", OracleDbType.Int64, ParameterDirection.Output);

                OracleHelper.ExecuteNonQuery(ConfigData.gConnectionString,
                    CommandType.StoredProcedure, "pkg_msg_tprl_sesion.proc_insert",
                     new OracleParameter("p_id", OracleDbType.Decimal, objData.Id, ParameterDirection.Input),
                     new OracleParameter("p_sor", OracleDbType.Varchar2, objData.Sor, ParameterDirection.Input),
                     new OracleParameter("p_msgtype", OracleDbType.Varchar2, objData.Msgtype, ParameterDirection.Input),
                     new OracleParameter("p_sendercompid", OracleDbType.Varchar2, objData.Sendercompid, ParameterDirection.Input),
                     new OracleParameter("p_targetcompid", OracleDbType.Varchar2, objData.Targetcompid, ParameterDirection.Input),
                     new OracleParameter("p_msgseqnum", OracleDbType.Decimal, objData.Msgseqnum, ParameterDirection.Input),
                     new OracleParameter("p_possdupflag", OracleDbType.Varchar2, objData.Possdupflag, ParameterDirection.Input),
                     new OracleParameter("p_sendingtime", OracleDbType.Varchar2, objData.Sendingtime, ParameterDirection.Input),
                     new OracleParameter("p_text", OracleDbType.Varchar2, objData.Text, ParameterDirection.Input),
                     new OracleParameter("p_lastmsgseqnumprocessed", OracleDbType.Decimal, objData.Lastmsgseqnumprocessed, ParameterDirection.Input),
                     new OracleParameter("p_tradsesreqid", OracleDbType.Varchar2, objData.Tradsesreqid, ParameterDirection.Input),
                     new OracleParameter("p_tradingsessionid", OracleDbType.Varchar2, objData.Tradingsessionid, ParameterDirection.Input),
                     new OracleParameter("p_tradsesmode", OracleDbType.Varchar2, objData.Tradsesmode, ParameterDirection.Input),
                     new OracleParameter("p_tradsesstatus", OracleDbType.Varchar2, objData.Tradsesstatus, ParameterDirection.Input),
                     new OracleParameter("p_tradsesstarttime", OracleDbType.Varchar2, objData.Tradsesstarttime, ParameterDirection.Input),
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
                Logger.log.Info($"End process Msg_tprl_sesionDA.Insert with Msgseqnum: {objData.Msgseqnum}, Msgtype: {objData.Msgtype}, response time: {DateTime.Now.Ticks - _timeRev}");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call Msg_tprl_sesionDA.Insert with Msgseqnum: {objData.Msgseqnum}, Msgtype: {objData.Msgtype}, Object={System.Text.Json.JsonSerializer.Serialize(objData)}, Exception: {ex?.ToString()}");
            }

            return result;
        }
    }
}