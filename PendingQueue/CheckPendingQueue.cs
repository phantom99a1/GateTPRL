using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KafkaInterface;
using BusinessProcessAPIReq;
using CommonLib;
namespace PendingQueue
{
    public class CheckPendingQueue : ICheckPendingQueue
    {

        public CheckPendingQueue(IKafkaClient kafkaClient, IProcessRevBussiness processRevBusiness) 
        {
            Thread t1 = new Thread(() => 
            ThreadCheckPendingQueue(kafkaClient, processRevBusiness));
            t1.IsBackground = true;
            t1.Name = "CheckPendingQueue";
            t1.Start();
        }



        public void ThreadCheckPendingQueue(IKafkaClient kafkaClient, IProcessRevBussiness processRevBusiness)
        {
            long t1, t2;
            while (true)
            {
                (t1, t2) = kafkaClient.ItemsInQueue();
                Logger.log.Info("Kakfka pending queue: {0}/{1}", t1, t2);
                if (t1 > t2 * 4 / 5)
                {
                    Logger.log.Warn("Queue gui Kafka co nhieu hon 80% phan tu trong queue");
                }

                (t1, t2) = processRevBusiness.ItemsInQueue();
                Logger.log.Info("HNX pending queue: {0}/{1}", t1, t2);
                if (t1 > t2 * 4 / 5)
                {
                    Logger.log.Warn("Queue gui HNX co nhieu hon 80% phan tu trong queue");
                }
                LogStation.LogStationFacade.WritetoLog();
                Thread.Sleep(1000 * ConfigData.PendingQueueTime);
            }
        }
    }
}
