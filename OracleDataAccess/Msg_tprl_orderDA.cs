using CommonLib;
using ObjectInfo;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace OracleDataAccess
{
    public static class Msg_tprl_orderDA
    {
        public static int Insert(Msg_Tprl_Order_Info objData)
        {
            int result = CodeDefine.None_Process;
            try
            {
                long _timeRev = DateTime.Now.Ticks;
                Logger.log.Info($"Start process Msg_tprl_orderDA.Insert with Msgseqnum: {objData.Msgseqnum}, Msgtype: {objData.Msgtype}");
                //

                OracleParameter returnParam = new OracleParameter("p_return", OracleDbType.Int64, ParameterDirection.Output);

                OracleHelper.ExecuteNonQuery(ConfigData.gConnectionString,
                    CommandType.StoredProcedure, "pkg_msg_tprl_order.proc_insert",
                     new OracleParameter("p_id", OracleDbType.Decimal, objData.Id, ParameterDirection.Input),
                     new OracleParameter("p_sor", OracleDbType.Varchar2, objData.Sor, ParameterDirection.Input),
                     new OracleParameter("p_msgtype", OracleDbType.Varchar2, objData.Msgtype, ParameterDirection.Input),
                     new OracleParameter("p_sendercompid", OracleDbType.Varchar2, objData.Sendercompid, ParameterDirection.Input),
                     new OracleParameter("p_targetcompid", OracleDbType.Decimal, objData.Targetcompid, ParameterDirection.Input),
                     new OracleParameter("p_msgseqnum", OracleDbType.Varchar2, objData.Msgseqnum, ParameterDirection.Input),
                     new OracleParameter("p_sendingtime", OracleDbType.Varchar2, objData.Sendingtime, ParameterDirection.Input),
                     new OracleParameter("p_lastmsgseqnumprocessed", OracleDbType.Varchar2, objData.Lastmsgseqnumprocessed, ParameterDirection.Input),
                     new OracleParameter("p_text", OracleDbType.Varchar2, objData.Text, ParameterDirection.Input),
                     new OracleParameter("p_clordid", OracleDbType.Varchar2, objData.Clordid, ParameterDirection.Input),
                     new OracleParameter("p_account", OracleDbType.Varchar2, objData.Account, ParameterDirection.Input),
                     new OracleParameter("p_symbol", OracleDbType.Varchar2, objData.Symbol, ParameterDirection.Input),
                     new OracleParameter("p_side", OracleDbType.Varchar2, objData.Side, ParameterDirection.Input),
                     new OracleParameter("p_orderqty", OracleDbType.Varchar2, objData.Orderqty, ParameterDirection.Input),
                     new OracleParameter("p_ordtype", OracleDbType.Varchar2, objData.Ordtype, ParameterDirection.Input),
                     new OracleParameter("p_price2", OracleDbType.Decimal, objData.Price2, ParameterDirection.Input),
                     new OracleParameter("p_price", OracleDbType.Decimal, objData.Price, ParameterDirection.Input),
                     new OracleParameter("p_orderqty2", OracleDbType.Decimal, objData.Orderqty2, ParameterDirection.Input),
                     new OracleParameter("p_origclordid", OracleDbType.Varchar2, objData.Origclordid, ParameterDirection.Input),
                     new OracleParameter("p_orgorderqty", OracleDbType.Varchar2, objData.Orgorderqty, ParameterDirection.Input),
                     new OracleParameter("p_special_type", OracleDbType.Decimal, objData.Special_Type, ParameterDirection.Input),
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
                Logger.log.Info($"End process Msg_tprl_orderDA.Insert with Msgseqnum: {objData.Msgseqnum}, Msgtype: {objData.Msgtype}, response time: {DateTime.Now.Ticks - _timeRev}");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call Msg_tprl_orderDA.Insert with Msgseqnum: {objData.Msgseqnum}, Msgtype: {objData.Msgtype}, Object={System.Text.Json.JsonSerializer.Serialize(objData)}, Exception: {ex?.ToString()}");
            }

            return result;
        }
    }
}