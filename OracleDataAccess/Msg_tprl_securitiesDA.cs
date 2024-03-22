using CommonLib;
using ObjectInfo;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace OracleDataAccess
{
    public static class Msg_tprl_securitiesDA
    {
        public static int Insert(Msg_Tprl_Securities_Info objData)
        {
            int result = CodeDefine.None_Process;
            try
            {
                long _timeRev = DateTime.Now.Ticks;
                Logger.log.Info($"Start process Msg_tprl_securitiesDA.Insert with Msgseqnum: {objData.Msgseqnum}, Msgtype: {objData.Msgtype}");
                //

                OracleParameter returnParam = new OracleParameter("p_return", OracleDbType.Int64, ParameterDirection.Output);

                OracleHelper.ExecuteNonQuery(ConfigData.gConnectionString,
                    CommandType.StoredProcedure, "pkg_msg_tprl_securities.proc_insert",
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
                     new OracleParameter("p_tradingsessionsubid", OracleDbType.Varchar2, objData.Tradingsessionsubid, ParameterDirection.Input),
                     new OracleParameter("p_securitystatusreqid", OracleDbType.Varchar2, objData.Securitystatusreqid, ParameterDirection.Input),
                     new OracleParameter("p_symbol", OracleDbType.Varchar2, objData.Symbol, ParameterDirection.Input),
                     new OracleParameter("p_securitytype", OracleDbType.Varchar2, objData.Securitytype, ParameterDirection.Input),
                     new OracleParameter("p_maturitydate", OracleDbType.Varchar2, objData.Maturitydate, ParameterDirection.Input),
                     new OracleParameter("p_issuedate", OracleDbType.Varchar2, objData.Issuedate, ParameterDirection.Input),
                     new OracleParameter("p_issuer", OracleDbType.Varchar2, objData.Issuer, ParameterDirection.Input),
                     new OracleParameter("p_highpx", OracleDbType.Decimal, objData.Highpx, ParameterDirection.Input),
                     new OracleParameter("p_lowpx", OracleDbType.Decimal, objData.Lowpx, ParameterDirection.Input),
                     new OracleParameter("p_highpxout", OracleDbType.Decimal, objData.Highpxout, ParameterDirection.Input),
                     new OracleParameter("p_lowpxout", OracleDbType.Decimal, objData.Lowpxout, ParameterDirection.Input),
                     new OracleParameter("p_highpxrep", OracleDbType.Decimal, objData.Highpxrep, ParameterDirection.Input),
                     new OracleParameter("p_lowpxrep", OracleDbType.Decimal, objData.Lowpxrep, ParameterDirection.Input),
                     new OracleParameter("p_lastpx", OracleDbType.Decimal, objData.Lastpx, ParameterDirection.Input),
                     new OracleParameter("p_securitytradingstatus", OracleDbType.Decimal, objData.Securitytradingstatus, ParameterDirection.Input),
                     new OracleParameter("p_buyvolume", OracleDbType.Decimal, objData.Buyvolume, ParameterDirection.Input),
                     new OracleParameter("p_dateno", OracleDbType.Varchar2, objData.Dateno, ParameterDirection.Input),
                     new OracleParameter("p_totallistingqtty", OracleDbType.Decimal, objData.Totallistingqtty, ParameterDirection.Input),
                     new OracleParameter("p_typerule", OracleDbType.Decimal, objData.Typerule, ParameterDirection.Input),
                     new OracleParameter("p_allowed_trading_subject", OracleDbType.Varchar2, objData.Allowed_Trading_Subject, ParameterDirection.Input),
                     new OracleParameter("p_allowed_trading_subject_sell", OracleDbType.Varchar2, objData.Allowed_Trading_Subject_Sell, ParameterDirection.Input),
                     new OracleParameter("p_subscriptionrequesttype", OracleDbType.Varchar2, objData.Subscriptionrequesttype, ParameterDirection.Input),
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
                Logger.log.Info($"End process Msg_tprl_securitiesDA.Insert with Msgseqnum: {objData.Msgseqnum}, Msgtype: {objData.Msgtype}, response time: {DateTime.Now.Ticks - _timeRev}");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call Msg_tprl_securitiesDA.Insert with Msgseqnum: {objData.Msgseqnum}, Msgtype: {objData.Msgtype}, Object={System.Text.Json.JsonSerializer.Serialize(objData)}, Exception: {ex?.ToString()}");
            }

            return result;
        }
    }
}