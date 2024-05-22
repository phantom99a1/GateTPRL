using CommonLib;
using ObjectInfo;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace OracleDataAccess
{
    public static class Msg_tprl_outrightDA
    {
        public static int Insert(Msg_Tprl_Outright_Info objData)
        {
            int result = CodeDefine.None_Process;
            try
            {
                long _timeRev = DateTime.Now.Ticks;
                Logger.log.Info($"Start process Msg_tprl_outrightDA.Insert with Msgseqnum: {objData.Msgseqnum}, Msgtype: {objData.Msgtype}");
                //

                OracleParameter returnParam = new OracleParameter("p_return", OracleDbType.Int64, ParameterDirection.Output);

                OracleHelper.ExecuteNonQuery(ConfigData.gConnectionString,
                    CommandType.StoredProcedure, "pkg_msg_tprl_outright.proc_insert",
                     new OracleParameter("p_id", OracleDbType.Decimal, objData.Id, ParameterDirection.Input),
                     new OracleParameter("p_sor", OracleDbType.Varchar2, objData.Sor, ParameterDirection.Input),
                     new OracleParameter("p_msgtype", OracleDbType.Varchar2, objData.Msgtype, ParameterDirection.Input),
                     new OracleParameter("p_sendercompid", OracleDbType.Varchar2, objData.Sendercompid, ParameterDirection.Input),
                     new OracleParameter("p_targetcompid", OracleDbType.Varchar2, objData.Targetcompid, ParameterDirection.Input),
                     new OracleParameter("p_msgseqnum", OracleDbType.Decimal, objData.Msgseqnum, ParameterDirection.Input),
                     new OracleParameter("p_sendingtime", OracleDbType.Varchar2, objData.Sendingtime, ParameterDirection.Input),
                     new OracleParameter("p_text", OracleDbType.Varchar2, objData.Text, ParameterDirection.Input),
                     new OracleParameter("p_ordtype", OracleDbType.Varchar2, objData.Ordtype, ParameterDirection.Input),
                     new OracleParameter("p_crosstype", OracleDbType.Decimal, objData.Crosstype, ParameterDirection.Input),
                     new OracleParameter("p_clordid", OracleDbType.Varchar2, objData.Clordid, ParameterDirection.Input),
                     new OracleParameter("p_crossid", OracleDbType.Varchar2, objData.Crossid, ParameterDirection.Input),
                     new OracleParameter("p_account", OracleDbType.Varchar2, objData.Account, ParameterDirection.Input),
                     new OracleParameter("p_coaccount", OracleDbType.Varchar2, objData.Coaccount, ParameterDirection.Input),
                     new OracleParameter("p_partyid", OracleDbType.Varchar2, objData.Partyid, ParameterDirection.Input),
                     new OracleParameter("p_copartyid", OracleDbType.Varchar2, objData.Copartyid, ParameterDirection.Input),
                     new OracleParameter("p_orderqty", OracleDbType.Decimal, objData.Orderqty, ParameterDirection.Input),
                     new OracleParameter("p_effectivetime", OracleDbType.Varchar2, objData.Effectivetime, ParameterDirection.Input),
                     new OracleParameter("p_side", OracleDbType.Decimal, objData.Side, ParameterDirection.Input),
                     new OracleParameter("p_symbol", OracleDbType.Varchar2, objData.Symbol, ParameterDirection.Input),
                     new OracleParameter("p_price2", OracleDbType.Varchar2, objData.Price2, ParameterDirection.Input),
                     new OracleParameter("p_settlvalue", OracleDbType.Decimal, objData.Settlvalue, ParameterDirection.Input),
                     new OracleParameter("p_settldate", OracleDbType.Varchar2, objData.Settldate, ParameterDirection.Input),
                     new OracleParameter("p_settlmethod", OracleDbType.Decimal, objData.Settlmethod, ParameterDirection.Input),
                     new OracleParameter("p_lastmsgseqnumprocessed", OracleDbType.Decimal, objData.Lastmsgseqnumprocessed, ParameterDirection.Input),
                     new OracleParameter("p_orderid", OracleDbType.Varchar2, objData.Orderid, ParameterDirection.Input),
                     new OracleParameter("p_origcrossid", OracleDbType.Varchar2, objData.Origcrossid, ParameterDirection.Input),
                     new OracleParameter("p_registid", OracleDbType.Varchar2, objData.Registid, ParameterDirection.Input),
                     new OracleParameter("p_rfqreqid", OracleDbType.Varchar2, objData.Rfqreqid, ParameterDirection.Input),
                     new OracleParameter("p_quoterespid", OracleDbType.Varchar2, objData.Quoterespid, ParameterDirection.Input),
                     new OracleParameter("p_quoteresptype", OracleDbType.Varchar2, objData.Quoteresptype, ParameterDirection.Input),
                     new OracleParameter("p_quoteid", OracleDbType.Varchar2, objData.Quoteid, ParameterDirection.Input),
                     new OracleParameter("p_quotecanceltype", OracleDbType.Varchar2, objData.Quotecanceltype, ParameterDirection.Input),
                     new OracleParameter("p_orderpartyid", OracleDbType.Varchar2, objData.Orderpartyid, ParameterDirection.Input),
                     new OracleParameter("p_quotereqid", OracleDbType.Varchar2, objData.Quotereqid, ParameterDirection.Input),
                     new OracleParameter("p_quotetype", OracleDbType.Varchar2, objData.Quotetype, ParameterDirection.Input),
                     new OracleParameter("p_remark", OracleDbType.Varchar2, objData.Remark, ParameterDirection.Input),
                     new OracleParameter("p_lastchange", OracleDbType.Varchar2, objData.Lastchange, ParameterDirection.Input),
                     new OracleParameter("p_createtime", OracleDbType.Varchar2, objData.Createtime, ParameterDirection.Input),
                     new OracleParameter("p_orderno", OracleDbType.Varchar2, objData.OrderNo, ParameterDirection.Input),
                    returnParam
                );

                if (int.TryParse(returnParam.Value?.ToString() ?? String.Empty, out int returnValue))
                {
                    result = returnValue;
                }
                //
                Logger.log.Info($"End process Msg_tprl_outrightDA.Insert with Msgseqnum: {objData.Msgseqnum}, Msgtype: {objData.Msgtype}, response time: {DateTime.Now.Ticks - _timeRev}");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call Msg_tprl_outrightDA.Insert with Msgseqnum: {objData.Msgseqnum}, Msgtype: {objData.Msgtype}, Object={System.Text.Json.JsonSerializer.Serialize(objData)}, Exception: {ex?.ToString()}");
            }

            return result;
        }
    }
}