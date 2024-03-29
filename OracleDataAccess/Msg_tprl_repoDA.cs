using CommonLib;
using HNX.FIXMessage;
using ObjectInfo;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using static HNX.FIXMessage.MessageExecOrderRepos;
using static HNX.FIXMessage.MessageReposBCGDReport;

namespace OracleDataAccess
{
    public static class Msg_tprl_repoDA
    {
        private static int Insert_Repos(OracleTransaction transaction, Msg_Tprl_Repo_Info objData)
        {
            int result = CodeDefine.None_Process;
            try
            {
                long _timeRev = DateTime.Now.Ticks;
                Logger.log.Info($"Start process Msg_tprl_repoDA.Insert with Msgseqnum: {objData.Msgseqnum}, Msgtype: {objData.Msgtype}");
                //

                OracleParameter returnParam = new OracleParameter("p_return", OracleDbType.Int64, ParameterDirection.Output);

                OracleHelper.ExecuteNonQuery(transaction,
                    CommandType.StoredProcedure, "pkg_msg_tprl_repo.proc_insert",
                     new OracleParameter("p_id", OracleDbType.Decimal, objData.Id, ParameterDirection.Input),
                     new OracleParameter("p_sor", OracleDbType.Varchar2, objData.Sor, ParameterDirection.Input),
                     new OracleParameter("p_msgtype", OracleDbType.Varchar2, objData.Msgtype, ParameterDirection.Input),
                     new OracleParameter("p_msgseqnum", OracleDbType.Decimal, objData.Msgseqnum, ParameterDirection.Input),
                     new OracleParameter("p_sendercompid", OracleDbType.Varchar2, objData.Sendercompid, ParameterDirection.Input),
                     new OracleParameter("p_sendingtime", OracleDbType.Varchar2, objData.Sendingtime, ParameterDirection.Input),
                     new OracleParameter("p_targetcompid", OracleDbType.Varchar2, objData.Targetcompid, ParameterDirection.Input),
                     new OracleParameter("p_possdupflag", OracleDbType.Varchar2, objData.Possdupflag, ParameterDirection.Input),
                     new OracleParameter("p_text", OracleDbType.Varchar2, objData.Text, ParameterDirection.Input),
                     new OracleParameter("p_partyid", OracleDbType.Varchar2, objData.Partyid, ParameterDirection.Input),
                     new OracleParameter("p_copartyid", OracleDbType.Varchar2, objData.Copartyid, ParameterDirection.Input),
                     new OracleParameter("p_matchreporttype", OracleDbType.Decimal, objData.Matchreporttype, ParameterDirection.Input),
                     new OracleParameter("p_orderid", OracleDbType.Varchar2, objData.Orderid, ParameterDirection.Input),
                     new OracleParameter("p_buyorderid", OracleDbType.Varchar2, objData.Buyorderid, ParameterDirection.Input),
                     new OracleParameter("p_sellorderid", OracleDbType.Varchar2, objData.Sellorderid, ParameterDirection.Input),
                     new OracleParameter("p_repurchaserate", OracleDbType.Varchar2, objData.Repurchaserate, ParameterDirection.Input),
                     new OracleParameter("p_repurchaseterm", OracleDbType.Decimal, objData.Repurchaseterm, ParameterDirection.Input),
                     new OracleParameter("p_noside", OracleDbType.Decimal, objData.Noside, ParameterDirection.Input),
                     new OracleParameter("p_lastmsgseqnumprocessed", OracleDbType.Varchar2, objData.Lastmsgseqnumprocessed, ParameterDirection.Input),
                     new OracleParameter("p_quotetype", OracleDbType.Decimal, objData.Quotetype, ParameterDirection.Input),
                     new OracleParameter("p_multilegrpttypereq", OracleDbType.Decimal, objData.Multilegrpttypereq, ParameterDirection.Input),
                     new OracleParameter("p_ordtype", OracleDbType.Varchar2, objData.Ordtype, ParameterDirection.Input),
                     new OracleParameter("p_rfqreqid", OracleDbType.Varchar2, objData.Rfqreqid, ParameterDirection.Input),
                     new OracleParameter("p_orgorderid", OracleDbType.Varchar2, objData.Orgorderid, ParameterDirection.Input),
                     new OracleParameter("p_quoteid", OracleDbType.Varchar2, objData.Quoteid, ParameterDirection.Input),
                     new OracleParameter("p_side", OracleDbType.Varchar2, objData.Side, ParameterDirection.Input),
                     new OracleParameter("p_orderqty", OracleDbType.Decimal, objData.Orderqty, ParameterDirection.Input),
                     new OracleParameter("p_effectivetime", OracleDbType.Varchar2, objData.Effectivetime, ParameterDirection.Input),
                     new OracleParameter("p_coaccount", OracleDbType.Varchar2, objData.Coaccount, ParameterDirection.Input),
                     new OracleParameter("p_settldate", OracleDbType.Varchar2, objData.Settldate, ParameterDirection.Input),
                     new OracleParameter("p_registid", OracleDbType.Varchar2, objData.Registid, ParameterDirection.Input),
                     new OracleParameter("p_clordid", OracleDbType.Varchar2, objData.Clordid, ParameterDirection.Input),
                     new OracleParameter("p_settldate2", OracleDbType.Varchar2, objData.Settldate2, ParameterDirection.Input),
                     new OracleParameter("p_enddate", OracleDbType.Varchar2, objData.Enddate, ParameterDirection.Input),
                     new OracleParameter("p_settlmethod", OracleDbType.Varchar2, objData.Settlmethod, ParameterDirection.Input),
                     new OracleParameter("p_orderpartyid", OracleDbType.Varchar2, objData.Orderpartyid, ParameterDirection.Input),
                     new OracleParameter("p_inquirymember", OracleDbType.Varchar2, objData.Inquirymember, ParameterDirection.Input),
                     new OracleParameter("p_remark", OracleDbType.Varchar2, objData.Remark, ParameterDirection.Input),
                     new OracleParameter("p_lastchange", OracleDbType.Varchar2, objData.Lastchange, ParameterDirection.Input),
                     new OracleParameter("p_createtime", OracleDbType.Varchar2, objData.Createtime, ParameterDirection.Input),
                     new OracleParameter("p_account", OracleDbType.Varchar2, objData.Account, ParameterDirection.Input),
                    returnParam
                );

                if (int.TryParse(returnParam.Value?.ToString() ?? String.Empty, out int returnValue))
                {
                    result = returnValue;
                }
                //
                Logger.log.Info($"End process Msg_tprl_repoDA.Insert with Msgseqnum: {objData.Msgseqnum}, Msgtype: {objData.Msgtype}, response time: {DateTime.Now.Ticks - _timeRev}");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call Msg_tprl_repoDA.Insert with Msgseqnum: {objData.Msgseqnum}, Msgtype: {objData.Msgtype}, Object={System.Text.Json.JsonSerializer.Serialize(objData)}, Exception: {ex?.ToString()}");
            }

            return result;
        }

        //private static int Insert_ReposDetail(OracleTransaction transaction, Msg_Tprl_Repo_Detail_Info objData)
        //{
        //    int result = CodeDefine.None_Process;
        //    try
        //    {
        //        long _timeRev = DateTime.Now.Ticks;
        //        Logger.log.Info($"Start process Msg_tprl_repo_detailDA.Insert with Refrepoid: {objData.Refrepoid}");
        //        //

        //        OracleParameter returnParam = new OracleParameter("p_return", OracleDbType.Int64, ParameterDirection.Output);

        //        OracleHelper.ExecuteNonQuery(transaction,
        //            CommandType.StoredProcedure, "pkg_msg_tprl_repo_detail.proc_insert",
        //             new OracleParameter("p_id", OracleDbType.Decimal, objData.Id, ParameterDirection.Input),
        //             new OracleParameter("p_refrepoid", OracleDbType.Decimal, objData.Refrepoid, ParameterDirection.Input),
        //             new OracleParameter("p_numside", OracleDbType.Decimal, objData.Numside, ParameterDirection.Input),
        //             new OracleParameter("p_symbol", OracleDbType.Varchar2, objData.Symbol, ParameterDirection.Input),
        //             new OracleParameter("p_execqty", OracleDbType.Decimal, objData.Execqty, ParameterDirection.Input),
        //             new OracleParameter("p_execpx", OracleDbType.Decimal, objData.Execpx, ParameterDirection.Input),
        //             new OracleParameter("p_price", OracleDbType.Decimal, objData.Price, ParameterDirection.Input),
        //             new OracleParameter("p_reposinterest", OracleDbType.Decimal, objData.Reposinterest, ParameterDirection.Input),
        //             new OracleParameter("p_hedgerate", OracleDbType.Decimal, objData.Hedgerate, ParameterDirection.Input),
        //             new OracleParameter("p_settlvalue2", OracleDbType.Decimal, objData.Settlvalue2, ParameterDirection.Input),
        //             new OracleParameter("p_settlvalue", OracleDbType.Decimal, objData.Settlvalue, ParameterDirection.Input),
        //             new OracleParameter("p_price2", OracleDbType.Decimal, objData.Price2, ParameterDirection.Input),
        //             new OracleParameter("p_remark", OracleDbType.Varchar2, objData.Remark, ParameterDirection.Input),
        //             new OracleParameter("p_lastchange", OracleDbType.Varchar2, objData.Lastchange, ParameterDirection.Input),
        //             new OracleParameter("p_createtime", OracleDbType.Varchar2, objData.Createtime, ParameterDirection.Input),
        //            returnParam
        //        );

        //        if (int.TryParse(returnParam.Value?.ToString() ?? String.Empty, out int returnValue))
        //        {
        //            result = returnValue;
        //        }
        //        //
        //        Logger.log.Info($"End process Msg_tprl_repo_detailDA.Insert with Refrepoid: {objData.Refrepoid}, response time: {DateTime.Now.Ticks - _timeRev}");
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.log.Error($"Error call Msg_tprl_repo_detailDA.Insert with Refrepoid: {objData.Refrepoid}, Object={System.Text.Json.JsonSerializer.Serialize(objData)}, Exception: {ex?.ToString()}");
        //    }

        //    return result;
        //}

        private static int InsertBatch_ReposDetail(OracleTransaction transaction, long Id_Table_Msg_Tprl_Repos, List<Msg_Tprl_Repo_Detail_Info> listObjData)
        {
            int result = 1;
            try
            {
                long _timeRev = DateTime.Now.Ticks;
                Logger.log.Info($"Start process Msg_tprl_repoDA.InsertBatch_ReposDetail with Refrepoid: {Id_Table_Msg_Tprl_Repos}");
                //

                OracleParameter paraResult = new OracleParameter("p_return", OracleDbType.Int64, ParameterDirection.Output);
                OracleHelper.ExcuteBatchNonQuery(transaction, CommandType.StoredProcedure, "pkg_msg_tprl_repo_detail.proc_insert", listObjData.Count,
                    new OracleParameter("p_id", OracleDbType.Decimal, listObjData.Select(o => o.Id).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_refrepoid", OracleDbType.Decimal, listObjData.Select(o => o.Refrepoid).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_numside", OracleDbType.Decimal, listObjData.Select(o => o.Numside).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_symbol", OracleDbType.Varchar2, listObjData.Select(o => o.Symbol).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_execqty", OracleDbType.Decimal, listObjData.Select(o => o.Execqty).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_execpx", OracleDbType.Decimal, listObjData.Select(o => o.Execpx).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_price", OracleDbType.Decimal, listObjData.Select(o => o.Price).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_reposinterest", OracleDbType.Decimal, listObjData.Select(o => o.Reposinterest).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_hedgerate", OracleDbType.Decimal, listObjData.Select(o => o.Hedgerate).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_settlvalue2", OracleDbType.Decimal, listObjData.Select(o => o.Settlvalue2).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_settlvalue", OracleDbType.Decimal, listObjData.Select(o => o.Settlvalue).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_price2", OracleDbType.Decimal, listObjData.Select(o => o.Price2).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_remark", OracleDbType.Varchar2, listObjData.Select(o => o.Remark).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_lastchange", OracleDbType.Varchar2, listObjData.Select(o => o.Lastchange).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_createtime", OracleDbType.Varchar2, listObjData.Select(o => o.Createtime).ToArray(), ParameterDirection.Input),
                    paraResult
                );
                Oracle.ManagedDataAccess.Types.OracleDecimal[] totalReturn = (Oracle.ManagedDataAccess.Types.OracleDecimal[])paraResult.Value;
                foreach (Oracle.ManagedDataAccess.Types.OracleDecimal _item in totalReturn)
                {
                    if (Convert.ToInt32(_item.Value.ToString()) < 0)
                    {
                        return Convert.ToInt32(_item.Value.ToString());
                    }
                }
                //
                Logger.log.Info($"End process Msg_tprl_repoDA.InsertBatch_ReposDetail with Refrepoid: {Id_Table_Msg_Tprl_Repos}, response time: {DateTime.Now.Ticks - _timeRev}");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call Msg_tprl_repoDA.InsertBatch_ReposDetail with Refrepoid: {Id_Table_Msg_Tprl_Repos}, Object={System.Text.Json.JsonSerializer.Serialize(listObjData)}, Exception: {ex?.ToString()}");
            }

            return result;
        }

        public static long InsertRepos(Msg_Tprl_Repo_Info objRepos, bool repoDetailExist, string pSymbol, List<ReposSideExecOrder> listReposSideExecOrder_EE_N01, List<ReposSide> listReposSide_N03_N04_N05_MA_ME, List<ReposSideReposBCGDReport> listReposSideReposBCGDReportList_MR)
        {
            long result = -1;
            Logger.log.Info($"Start process Msg_tprl_repoDA.InsertRepos with objRepos.Msgtype = {objRepos.Msgtype}, repoDetailExist={repoDetailExist}");
            try
            {
                using (OracleConnection connection = new OracleConnection(ConfigData.gConnectionString))
                {
                    connection.Open();
                    using (OracleTransaction transaction = connection.BeginTransaction())
                    {
                        result = Insert_Repos(transaction, objRepos);
                        //
                        Logger.log.Info($"Continue process Msg_tprl_repoDA.InsertRepos with objRepos.Msgtype = {objRepos.Msgtype}, repoDetailExist={repoDetailExist}, result={result}");
                        //
                        if (repoDetailExist)
                        {
                            // 35 = EE, N01
                            if (objRepos.Msgtype == MessageType.ExecOrderRepos || objRepos.Msgtype == MessageType.ReposInquiry)
                            {
                                List<Msg_Tprl_Repo_Detail_Info> listReposDetail = new List<Msg_Tprl_Repo_Detail_Info>();
                                ReposSideExecOrder item;
                                for (int i = 0; i < listReposSideExecOrder_EE_N01.Count; i++)
                                {
                                    item = listReposSideExecOrder_EE_N01[i];

                                    Msg_Tprl_Repo_Detail_Info objRepoDetail = new Msg_Tprl_Repo_Detail_Info();
                                    //
                                    objRepoDetail.Refrepoid = result;
                                    objRepoDetail.Numside = item.NumSide;
                                    objRepoDetail.Symbol = item.Symbol;
                                    objRepoDetail.Execqty = item.ExecQty;
                                    objRepoDetail.Execpx = item.ExecPx;
                                    objRepoDetail.Price = item.Price;
                                    objRepoDetail.Reposinterest = item.ReposInterest;
                                    objRepoDetail.Hedgerate = item.HedgeRate;
                                    objRepoDetail.Settlvalue = item.SettlValue;
                                    objRepoDetail.Settlvalue2 = item.SettlValue2;
                                    objRepoDetail.Price2 = 0;
                                    objRepoDetail.Remark = "";
                                    objRepoDetail.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                                    objRepoDetail.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                                    //
                                    listReposDetail.Add(objRepoDetail);
                                }
                                //
                                result = InsertBatch_ReposDetail(transaction, result, listReposDetail);
                                //
                                Logger.log.Info($"Continue process InsertBatch_ReposDetail with objRepos.Msgtype = {objRepos.Msgtype}, result={result}");
                            }

                            // 35 = N03, N04, N05, MA, ME
                            if (objRepos.Msgtype == MessageType.ReposFirm ||
                                objRepos.Msgtype == MessageType.ReposFirmReport ||
                                objRepos.Msgtype == MessageType.ReposFirmAccept ||
                                objRepos.Msgtype == MessageType.ReposBCGD ||
                                objRepos.Msgtype == MessageType.ReposBCGDModify)
                            {
                                List<Msg_Tprl_Repo_Detail_Info> listReposDetail = new List<Msg_Tprl_Repo_Detail_Info>();
                                ReposSide item;
                                for (int i = 0; i < listReposSide_N03_N04_N05_MA_ME.Count; i++)
                                {
                                    item = listReposSide_N03_N04_N05_MA_ME[i];

                                    Msg_Tprl_Repo_Detail_Info objRepoDetail = new Msg_Tprl_Repo_Detail_Info();
                                    //
                                    objRepoDetail.Refrepoid = result;
                                    objRepoDetail.Numside = item.NumSide;
                                    objRepoDetail.Symbol = item.Symbol;
                                    objRepoDetail.Execqty = 0;
                                    objRepoDetail.Execpx = 0;
                                    objRepoDetail.Price = item.Price;
                                    objRepoDetail.Reposinterest = item.ReposInterest;
                                    objRepoDetail.Hedgerate = item.HedgeRate;
                                    objRepoDetail.Settlvalue = item.SettlValue;
                                    objRepoDetail.Settlvalue2 = item.SettlValue2;
                                    objRepoDetail.Price2 = item.ExecPrice;
                                    objRepoDetail.Remark = "";
                                    objRepoDetail.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                                    objRepoDetail.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                                    //
                                    listReposDetail.Add(objRepoDetail);
                                }
                                //
                                result = InsertBatch_ReposDetail(transaction, result, listReposDetail);
                                //
                                Logger.log.Info($"Continue process InsertBatch_ReposDetail with objRepos.Msgtype = {objRepos.Msgtype}, result={result}");
                            }
                            // 35 = MR
                            if (objRepos.Msgtype == MessageType.ReposBCGDReport)
                            {
                                List<Msg_Tprl_Repo_Detail_Info> listReposDetail = new List<Msg_Tprl_Repo_Detail_Info>();
                                ReposSideReposBCGDReport item;
                                for (int i = 0; i < listReposSideReposBCGDReportList_MR.Count; i++)
                                {
                                    item = listReposSideReposBCGDReportList_MR[i];

                                    Msg_Tprl_Repo_Detail_Info objRepoDetail = new Msg_Tprl_Repo_Detail_Info();
                                    //
                                    objRepoDetail.Refrepoid = result;
                                    objRepoDetail.Numside = item.NumSide;
                                    objRepoDetail.Symbol = item.Symbol;
                                    objRepoDetail.Execqty = 0;
                                    objRepoDetail.Execpx = 0;
                                    objRepoDetail.Price = item.Price;
                                    objRepoDetail.Reposinterest = item.ReposInterest;
                                    objRepoDetail.Hedgerate = item.HedgeRate;
                                    objRepoDetail.Settlvalue = item.SettlValue;
                                    objRepoDetail.Settlvalue2 = item.SettlValue2;
                                    objRepoDetail.Price2 = item.ExecPrice;
                                    objRepoDetail.Remark = "";
                                    objRepoDetail.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                                    objRepoDetail.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                                    //
                                    listReposDetail.Add(objRepoDetail);
                                }
                                //
                                result = InsertBatch_ReposDetail(transaction, result, listReposDetail);
                                //
                                Logger.log.Info($"Continue process InsertBatch_ReposDetail with objRepos.Msgtype = {objRepos.Msgtype}, result={result}");
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(pSymbol))
                            {
                                List<Msg_Tprl_Repo_Detail_Info> listReposDetail = new List<Msg_Tprl_Repo_Detail_Info>();
                                Msg_Tprl_Repo_Detail_Info objRepoDetail = new Msg_Tprl_Repo_Detail_Info();
                                //
                                objRepoDetail.Refrepoid = result;
                                objRepoDetail.Numside = 0;
                                objRepoDetail.Symbol = pSymbol;
                                objRepoDetail.Execqty = 0;
                                objRepoDetail.Execpx = 0;
                                objRepoDetail.Price = 0;
                                objRepoDetail.Reposinterest = 0;
                                objRepoDetail.Hedgerate = 0;
                                objRepoDetail.Settlvalue = 0;
                                objRepoDetail.Settlvalue2 = 0;
                                objRepoDetail.Price2 = 0;
                                objRepoDetail.Remark = "";
                                objRepoDetail.Lastchange = DateTime.Now.ToString(ConfigData.formatDateTime);
                                objRepoDetail.Createtime = DateTime.Now.ToString(ConfigData.formatDateTime);
                                //
                                listReposDetail.Add(objRepoDetail);
                                //
                                result = InsertBatch_ReposDetail(transaction, result, listReposDetail);
                                //
                                Logger.log.Info($"Continue process InsertBatch_ReposDetail with objRepos.Msgtype = {objRepos.Msgtype}, result={result}");
                            }
                        }
                        //
                        if (result > 0)
                        {
                            transaction.Commit();
                            Logger.log.Info($"Continue process Msg_tprl_repoDA.InsertRepos with objRepos.Msgtype = {objRepos.Msgtype}, result={result} ==>> transaction.Commit()");
                        }
                        else
                        {
                            transaction.Rollback();
                            Logger.log.Info($"Continue process Msg_tprl_repoDA.InsertRepos with objRepos.Msgtype = {objRepos.Msgtype}, result={result} ==>> transaction.Rollback()");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call Msg_tprl_repoDA.InsertRepos with  objRepos={System.Text.Json.JsonSerializer.Serialize(objRepos)}, repoDetailExist={repoDetailExist}, Exception: {ex?.ToString()}");
            }
            Logger.log.Info($"End process Msg_tprl_repoDA.InsertRepos with objRepos.Msgtype = {objRepos.Msgtype}, repoDetailExist={repoDetailExist}, result={result}");
            return result;
        }
    }
}