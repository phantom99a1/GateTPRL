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
                double maxSeqBusinessSend = ConfigData.MaxSeqBusinessSend;
                double seqBusinessAchieve = message.LastMsgSeqNumProcessed;
                double threshold = Math.Round((seqBusinessAchieve / maxSeqBusinessSend) * 100, 2);
                var sendingTime = message.GetSendingTime;                
                var warningPointPercent = ConfigData.WarningPointPercent;
                var warningThreshold = new GateTPRLWarningThreshold()
                {
                    MaxSeqBusinessSend = ConfigData.MaxSeqBusinessSend,
                    SeqBusinessAchieve = message.LastMsgSeqNumProcessed,
                    Threshold = threshold,
                    Description = threshold >= warningPointPercent ? $"Số lệnh đã đạt ngưỡng {warningPointPercent} của phiên đang giao dịch"
                    : "Số lượng lệnh vẫn chưa đạt ngưỡng của phiên giao dịch",
                    ProcessingTime = string.Format("{0:yyyy/MM/dd HH:mm:ss}", sendingTime)
                };
                DataMem.lstGateTPRLWarningThreshold.Add(warningThreshold);
                //End 2024/09/25
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.Message.ToString());
            }
        }
    }
}
