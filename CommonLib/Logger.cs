namespace CommonLib
{
    public class Logger
    {
        public static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        public static readonly NLog.Logger HNXTcpLog = NLog.LogManager.GetLogger("loghnxtcp");
        public static readonly NLog.Logger ApiLog = NLog.LogManager.GetLogger("loghnxapi");
        public static readonly NLog.Logger ResponseLog = NLog.LogManager.GetLogger("loghnxrp");
    }
}
