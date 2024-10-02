using CommonLib;
using ObjectInfo;

namespace APIMonitor
{
    public class WorkerITMonitor
    {
        public static void Start()
        {
            try
            {
                Logger.log.Info($"WorkerITMonitor Starting!");
                Thread threadItMonitor = new Thread(FuncGateTPRLWarningThreshold);
                threadItMonitor.IsBackground = true;
                threadItMonitor.Start();
                Logger.log.Info($"WorkerITMonitor Started!");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call Start() in ApiMonitorController, Exception: {ex?.ToString()}");
            }
        }


        /// <summary>
        /// Function lấy thông tin trả về cho phần thông báo khi gặp vượt ngưỡng, hệ thống bị lỗi
        /// </summary>
        /// <param name="message"></param>
        private static void FuncGateTPRLWarningThreshold()
        {
            bool isActive = true;
            int percent = 100;
            int numNumber = 4;
            var timeSpan = new TimeSpan(11, 30, 0);
            while (isActive)
            {
                GateTPRLWarningThreshold? warningThreshold;
                try
                {
                    //2024.09.25 add data on memory
                    bool isMorningSession = DataMem.lastTimeMsgSend.TimeOfDay <= timeSpan;
                    double maxSeqBusinessSend = ConfigData.MaxSeqBusinessSend;
                    string session = isMorningSession ? "Sáng" : "Chiều";
                    double maxSeqBusinessSendMorning = 60 * maxSeqBusinessSend / percent;
                    double maxSeqBusinessSendAfternon = maxSeqBusinessSend - maxSeqBusinessSendMorning;
                    double maxSeqBusinessSendOfSession = isMorningSession ? maxSeqBusinessSendMorning : maxSeqBusinessSendAfternon;
                    double seqBusinessAchieveMorning = 0;
                    double seqBusinessAchieveAfternoon = 0;
                    var warningPointPercent = ConfigData.WarningPointPercent;
                    double seqBusinessAchieveDay =( DataMem.NumMsgSend + DataMem.NumMsgSendApi2);
                    if (maxSeqBusinessSendOfSession == 0) { maxSeqBusinessSendOfSession = 1; };
                    if (maxSeqBusinessSend == 0) { maxSeqBusinessSend = 1; };
                    if (DataMem.warningThreshold != null)
                    {
                        var gateTPRLWarningThreshold = DataMem.warningThreshold;
                        bool isPrevMorningSession = DateTime.Parse(gateTPRLWarningThreshold.ProcessingTime).TimeOfDay <= new TimeSpan(11, 30, 0);
                        seqBusinessAchieveMorning = isMorningSession ? DataMem.NumMsgSend : gateTPRLWarningThreshold.SeqBusinessSendMorning;
                        seqBusinessAchieveAfternoon = DataMem.NumMsgSend - seqBusinessAchieveMorning;
                        double seqBusinessAchieve = isMorningSession ? seqBusinessAchieveMorning : seqBusinessAchieveAfternoon;
                        double thresholdSession = Math.Round((seqBusinessAchieve / maxSeqBusinessSendOfSession) * percent, numNumber);
                        double thresholdDay = Math.Round((seqBusinessAchieveDay / maxSeqBusinessSend) * percent, numNumber);
                        warningThreshold = new GateTPRLWarningThreshold()
                        {
                            MaxSeqBusinessSendDay = maxSeqBusinessSend,
                            MaxSeqBusinessSendSession = maxSeqBusinessSendOfSession,
                            SeqBusinessSendMorning = seqBusinessAchieveMorning,
                            SeqBusinessAchieve = seqBusinessAchieve,
                            ThresholdSession = $"{thresholdSession}%",
                            ThresholdDay = $"{thresholdDay}%",
                            DescriptionSession = thresholdSession >= warningPointPercent ? $"Số lệnh đã đạt ngưỡng {warningPointPercent}% của phiên {session.ToLower()} đang giao dịch"
                            : $"Số lượng lệnh vẫn chưa đạt ngưỡng của phiên {session.ToLower()} đang giao dịch",
                            DescriptionDay = thresholdDay >= warningPointPercent ? $"Số lệnh đã đạt ngưỡng {warningPointPercent}% của ngày đang giao dịch"
                            : $"Số lượng lệnh vẫn chưa đạt ngưỡng của ngày đang giao dịch",
                            ProcessingTime = string.Format("{0:yyyy/MM/dd HH:mm:ss}", DataMem.lastTimeMsgSend),
                            Session = session,
                        };
                    }
                    else
                    {
                        double seqBusinessAchieve = (DataMem.NumMsgSend + DataMem.NumMsgSendApi2);
                        double thresholdSession = Math.Round((seqBusinessAchieve / maxSeqBusinessSendOfSession) * percent, numNumber);
                        double thresholdDay = Math.Round((seqBusinessAchieveDay / maxSeqBusinessSend) * percent, numNumber);
                        warningThreshold = new GateTPRLWarningThreshold()
                        {
                            MaxSeqBusinessSendDay = maxSeqBusinessSend,
                            MaxSeqBusinessSendSession = maxSeqBusinessSendOfSession,
                            SeqBusinessSendMorning = isMorningSession ? seqBusinessAchieve : 0,
                            SeqBusinessAchieve = seqBusinessAchieve,
                            ThresholdSession = $"{thresholdSession}%",
                            ThresholdDay = $"{thresholdDay}%",
                            DescriptionSession = thresholdSession >= warningPointPercent ? $"Số lệnh đã đạt ngưỡng {warningPointPercent}% của phiên {session.ToLower()} đang giao dịch"
                            : $"Số lượng lệnh vẫn chưa đạt ngưỡng của phiên {session.ToLower()} đang giao dịch",
                            DescriptionDay = thresholdDay >= warningPointPercent ? $"Số lệnh đã đạt ngưỡng {warningPointPercent}% của ngày đang giao dịch"
                            : $"Số lượng lệnh vẫn chưa đạt ngưỡng của ngày đang giao dịch",
                            ProcessingTime = string.Format("{0:yyyy/MM/dd HH:mm:ss}", DataMem.lastTimeMsgSend),
                            Session = session,
                        };
                    }
                    //End 2024/09/25

                    lock (DataMem.lockObj)
                    {
                        DataMem.warningThreshold = warningThreshold;
                    }
                }
                catch (Exception ex)
                {
                    Logger.log.Error(ex.Message.ToString());
                }
                //
                Thread.Sleep(ConfigData.SeqBusinessIncrementPeriod);
            }
        }
    }
}
