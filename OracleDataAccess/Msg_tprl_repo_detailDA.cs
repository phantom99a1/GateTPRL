using CommonLib;
using ObjectInfo;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDataAccess
{
    public static class Msg_tprl_repo_detailDA
    {
        public static int Insert(Msg_Tprl_Repo_Detail_Info objData)
        {
            int result = CodeDefine.None_Process;
            try
            {
                long _timeRev = DateTime.Now.Ticks;
                Logger.log.Info($"Start process Msg_tprl_repo_detailDA.Insert with Refrepoid: {objData.Refrepoid}");
                //

                OracleParameter returnParam = new OracleParameter("p_return", OracleDbType.Int64, ParameterDirection.Output);

                OracleHelper.ExecuteNonQuery(ConfigData.gConnectionString,
                    CommandType.StoredProcedure, "pkg_msg_tprl_repo_detail.proc_insert",
                     new OracleParameter("p_id", OracleDbType.Decimal, objData.Id, ParameterDirection.Input),
                     new OracleParameter("p_refrepoid", OracleDbType.Decimal, objData.Refrepoid, ParameterDirection.Input),
                     new OracleParameter("p_numside", OracleDbType.Decimal, objData.Numside, ParameterDirection.Input),
                     new OracleParameter("p_symbol", OracleDbType.Varchar2, objData.Symbol, ParameterDirection.Input),
                     new OracleParameter("p_execqty", OracleDbType.Decimal, objData.Execqty, ParameterDirection.Input),
                     new OracleParameter("p_execpx", OracleDbType.Decimal, objData.Execpx, ParameterDirection.Input),
                     new OracleParameter("p_price", OracleDbType.Decimal, objData.Price, ParameterDirection.Input),
                     new OracleParameter("p_reposinterest", OracleDbType.Decimal, objData.Reposinterest, ParameterDirection.Input),
                     new OracleParameter("p_hedgerate", OracleDbType.Decimal, objData.Hedgerate, ParameterDirection.Input),
                     new OracleParameter("p_settlvalue2", OracleDbType.Decimal, objData.Settlvalue2, ParameterDirection.Input),
                     new OracleParameter("p_settlvalue", OracleDbType.Decimal, objData.Settlvalue, ParameterDirection.Input),
                     new OracleParameter("p_price2", OracleDbType.Decimal, objData.Price2, ParameterDirection.Input),
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
                Logger.log.Info($"End process Msg_tprl_repo_detailDA.Insert with Refrepoid: {objData.Refrepoid}, response time: {DateTime.Now.Ticks - _timeRev}");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call Msg_tprl_repo_detailDA.Insert with Refrepoid: {objData.Refrepoid}, Object={System.Text.Json.JsonSerializer.Serialize(objData)}, Exception: {ex?.ToString()}");
            }

            return result;
        }
    }
}