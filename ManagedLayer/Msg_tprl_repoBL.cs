using HNX.FIXMessage;
using ObjectInfo;
using OracleDataAccess;
using static HNX.FIXMessage.MessageExecOrderRepos;
using static HNX.FIXMessage.MessageReposBCGDReport;

namespace ManagedLayer
{
    public static class Msg_tprl_repoBL
    {
        public static long ProcessInsertRepos(Msg_Tprl_Repo_Info objRepos, bool repoDetailExist, List<ReposSideExecOrder> listReposSideExecOrder_EE_N01, List<ReposSide> listReposSide_N03_N04_N05_MA_ME, List<ReposSideReposBCGDReport> listReposSideReposBCGDReportList_MR)
        {
            long result = Msg_tprl_repoDA.InsertRepos(objRepos, repoDetailExist, listReposSideExecOrder_EE_N01, listReposSide_N03_N04_N05_MA_ME, listReposSideReposBCGDReportList_MR);
            return result;
        }
    }
}