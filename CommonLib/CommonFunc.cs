using HNX.FIXMessage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public class CommonFunc
    {
        /// <summary>
        /// Function Add Log Hien thi len man hinh Gate ITMonitor
        /// </summary>
        /// <param name="message"></param>
        public static void FuncAddMessageRejectForITMonitor(FIXMessageBase message)
        {
            try
            {
                //2024.09.05 add msg reject on memory
                MessageReject messageReject = (MessageReject)message;
                DataMem.lstAllMsgRejectOnMemory.Add(messageReject);
                // end 2024.09.05 
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Function Add thông tin trả về cho phần thông báo khi gặp vượt ngưỡng, hệ thống bị lỗi
        /// </summary>
        /// <param name="message"></param>
        public static void FuncAddGateTPRLWarningThreshold(FIXMessageBase message)
        {
            try
            {
                //2024.09.25 add data on memory               
				double seqBusinessAchieveMorning = 0;
				double seqBusinessAchieveAfternoon = 0;
                double seqBusinessAchieveDay = 0;
				double maxSeqBusinessSend = ConfigData.MaxSeqBusinessSend;
                bool isMorningSession = message.GetSendingTime.TimeOfDay <= new TimeSpan(11, 30, 0);
				string session = isMorningSession ? "Sáng" : "Chiều";
				double maxSeqBusinessSendMorning = 60 * maxSeqBusinessSend /100;
                double maxSeqBusinessSendAfternon = maxSeqBusinessSend - maxSeqBusinessSendMorning;
                double maxSeqBusinessSendOfSession = isMorningSession ? maxSeqBusinessSendMorning : maxSeqBusinessSendAfternon;				
                var sendingTime = message.GetSendingTime;                
                var warningPointPercent = ConfigData.WarningPointPercent;
                if (isMorningSession)
                {
                    seqBusinessAchieveMorning = message.LastMsgSeqNumProcessed;
				}
                else
                {
					seqBusinessAchieveDay = message.LastMsgSeqNumProcessed;
				}
                seqBusinessAchieveAfternoon = seqBusinessAchieveDay - seqBusinessAchieveMorning;
				double seqBusinessAchieve = isMorningSession ? seqBusinessAchieveMorning : seqBusinessAchieveAfternoon;
				double thresholdSession = Math.Round((seqBusinessAchieve / maxSeqBusinessSendOfSession) * 100, 4);
				double thresholdDay = Math.Round((seqBusinessAchieveDay / maxSeqBusinessSend) * 100, 4);
				var warningThreshold = new GateTPRLWarningThreshold()
                {
                    MaxSeqBusinessSendDay = maxSeqBusinessSend,
					MaxSeqBusinessSendSession = maxSeqBusinessSendOfSession,
                    SeqBusinessAchieve = isMorningSession ? seqBusinessAchieveMorning : seqBusinessAchieveAfternoon,
					ThresholdSession = $"{thresholdSession}%",
                    ThresholdDay = $"{thresholdDay}%",
                    DescriptionSession = thresholdSession >= warningPointPercent ? $"Số lệnh đã đạt ngưỡng {warningPointPercent}% của phiên {session.ToLower()} đang giao dịch"
                    : $"Số lượng lệnh vẫn chưa đạt ngưỡng của phiên {session.ToLower()} đang giao dịch",
					DescriptionDay = thresholdDay >= warningPointPercent ? $"Số lệnh đã đạt ngưỡng {warningPointPercent}% của ngày đang giao dịch"
					: $"Số lượng lệnh vẫn chưa đạt ngưỡng của ngày đang giao dịch",
					ProcessingTime = string.Format("{0:yyyy/MM/dd HH:mm:ss}", sendingTime),
                    Session = session,
				};
                DataMem.lstGateTPRLWarningThreshold?.Add(warningThreshold);
                //End 2024/09/25
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.Message.ToString());
            }
        }
    }
}
