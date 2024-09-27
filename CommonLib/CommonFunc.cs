using HNX.FIXMessage;

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
    }
}
