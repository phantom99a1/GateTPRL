using CommonLib;
using Disruptor;
using Disruptor.Dsl;
using LocalMemory;
using System.Collections.Concurrent;

namespace KafkaInterface
{
    public class KafkaClient : IKafkaClient, IEventHandler<KafkaObjectEvent>
    {
        public static readonly NLog.Logger log = CommonLib.Logger.ResponseLog;
        private ConcurrentDictionary<long, KafkaObjectEvent> c_DictKafkaObjectMissed = new ConcurrentDictionary<long, KafkaObjectEvent>();
        private int bufferSize = ConfigData.QueueSize;
        private Disruptor<KafkaObjectEvent> c_Disruptor;
        private RingBuffer<KafkaObjectEvent> c_RingBuffer;
        private IKafkaHelper Kafka_OrderStatus;
        private IKafkaHelper Kafka_ExecutionStatus;
        private IKafkaHelper Kafka_TradingInfoStatus;
        private int sequenceGateAccept;
        private int sequenceGateSendHNX;
        private int sequenceGateFwdFromHNX;
        private StoreMapSeqEvent c_StoreMapSeqEvent;
        private Thread ThreadResend;

        public int SequenceGateAccept
        {
            get
            {
                return sequenceGateAccept;
            }
        }

        public int SequenceGateSendHNX
        {
            get
            {
                return sequenceGateSendHNX;
            }
        }

        public int SequenceGateFwdFromHNX
        {
            get
            {
                return sequenceGateFwdFromHNX;
            }
        }

        public KafkaClient()
        {
            try
            {
                Kafka_OrderStatus = new KafkaHelper(ConfigData.KafkaConfig.KafkaAuth,
                                                               ConfigData.KafkaConfig.KafkaIp,
                                                               ConfigData.KafkaConfig.KafkaPort,
                                                               ConfigData.KafkaConfig.KafkaUser,
                                                               ConfigData.KafkaConfig.KafkaPassword,
                                                               ConfigData.KafkaConfig.KafkaCALocation,
                                                               ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus,
                                                               ConfigData.KafkaConfig.Kafka_Acks,
                                                               ConfigData.KafkaConfig.Kafka_CompressionType,
                                                               ConfigData.KafkaConfig.Kafka_LingerMs,
                                                               ConfigData.KafkaConfig.Kafka_BatchSize);

                Logger.log.Info("Init Kafka Topic {0} success", ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus);

                Kafka_ExecutionStatus = new KafkaHelper(ConfigData.KafkaConfig.KafkaAuth,
                                                                       ConfigData.KafkaConfig.KafkaIp,
                                                                       ConfigData.KafkaConfig.KafkaPort,
                                                                       ConfigData.KafkaConfig.KafkaUser,
                                                                       ConfigData.KafkaConfig.KafkaPassword,
                                                                       ConfigData.KafkaConfig.KafkaCALocation,
                                                                       ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution,
                                                                       ConfigData.KafkaConfig.Kafka_Acks,
                                                                       ConfigData.KafkaConfig.Kafka_CompressionType,
                                                                       ConfigData.KafkaConfig.Kafka_LingerMs,
                                                                       ConfigData.KafkaConfig.Kafka_BatchSize);

                Logger.log.Info("Init Kafka Topic {0} success", ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution);

                Kafka_TradingInfoStatus = new KafkaHelper(ConfigData.KafkaConfig.KafkaAuth,
                                                                       ConfigData.KafkaConfig.KafkaIp,
                                                                       ConfigData.KafkaConfig.KafkaPort,
                                                                       ConfigData.KafkaConfig.KafkaUser,
                                                                       ConfigData.KafkaConfig.KafkaPassword,
                                                                       ConfigData.KafkaConfig.KafkaCALocation,
                                                                       ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_TradingInfo,
                                                                       ConfigData.KafkaConfig.Kafka_Acks,
                                                                       ConfigData.KafkaConfig.Kafka_CompressionType,
                                                                       ConfigData.KafkaConfig.Kafka_LingerMs,
                                                                       ConfigData.KafkaConfig.Kafka_BatchSize);
                Logger.log.Info("Init Kafka Topic {0} success", ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_TradingInfo);

                c_Disruptor = new Disruptor<KafkaObjectEvent>(() => new KafkaObjectEvent(), bufferSize, TaskScheduler.Default,
                                                                                               ProducerType.Single, ConfigData.StrategyMode);
                //c_Disruptor.HandleEventsWith(new ProcessKafkaObjectEvent(Kafka_OrderStatus, Kafka_ExecutionStatus, Kafka_TradingInfoStatus));
                c_StoreMapSeqEvent = new StoreMapSeqEvent();
                c_Disruptor.HandleEventsWith(this).Then(c_StoreMapSeqEvent);
                ThreadResend = new Thread(ThrdResendObject);
                ThreadResend.IsBackground = true;
                c_RingBuffer = c_Disruptor.RingBuffer;
              
                c_Disruptor.Start();
                CommonLib.Logger.log.Info("Created  Instance KafkaClient:{0}", this.GetHashCode());
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }

        public int NumOfMsg()
        {
            return Kafka_OrderStatus.NumOfMsg() + Kafka_ExecutionStatus.NumOfMsg() + Kafka_TradingInfoStatus.NumOfMsg();
        }

        public (long, long) ItemsInQueue()
        {
            return (bufferSize - c_RingBuffer.GetRemainingCapacity(), bufferSize);
        }

        public long Send2KafkaObject(string p_TopicName, object p_Object, long p_StartTimeProcess, int p_MessageSequence, char p_EventFlag)
        {
            log.Info("Enqueue Data Message {0} to Kafka", p_Object.ToString());

            // (1) Claim the next sequence
            long sequence = c_RingBuffer.Next();
            try
            {
                // (2) Get and configure the event for the sequence
                c_RingBuffer[sequence].TopicName = p_TopicName;
                c_RingBuffer[sequence].Object = p_Object;
                c_RingBuffer[sequence].StartTimeProcess = p_StartTimeProcess;
                c_RingBuffer[sequence].MessageSequence = p_MessageSequence;
                c_RingBuffer[sequence].EventFlag = p_EventFlag;
            }
            finally
            {
                // (3) Publish the event
                c_RingBuffer.Publish(sequence);
                log.Info("Enqueue Data Message {0} Complete at sequence {1}", c_RingBuffer[sequence].Object.ToString(), sequence);
            }
            return sequence;
        }

        public void OnEvent(KafkaObjectEvent p_data, long sequence, bool endOfBatch)
        {
            try
            {
                int sendKafka = Send2Kafka(p_data, sequence);
                if (sendKafka == SendKafkaStatus.SEND_SUCCESS)
                {
                    log.Info("Send to Kafka topic {0} object {1} map with Messages sequence {2} success. Process in {3} us", p_data.TopicName, p_data.Object, p_data.MessageSequence, (DateTime.Now.Ticks - p_data.StartTimeProcess) / 10);
                }
                else
                {
                    log.Error("Send to Kafka topic {0} object {1} in sequence {2} fail", p_data.TopicName, p_data.Object, sequence);
                    c_DictKafkaObjectMissed.TryAdd(sequence, p_data);
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public void RecoverSeq()
        {
            sequenceGateAccept = (int)ShareMemoryData.c_FileStore.RecoverMapSeq(0);
            sequenceGateSendHNX = (int)ShareMemoryData.c_FileStore.RecoverMapSeq(8);
            sequenceGateFwdFromHNX = (int)ShareMemoryData.c_FileStore.RecoverMapSeq(16);
        }

        private int Send2Kafka(KafkaObjectEvent p_data, long sequence)
        {
            int sendKafka = SendKafkaStatus.SEND_FAILURE;

            if (p_data.TopicName == ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus)
            {
                sendKafka = Kafka_OrderStatus.SendToKafkaObject(p_data.Object);
            }
            else if (p_data.TopicName == ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution)
            {
                sendKafka = Kafka_ExecutionStatus.SendToKafkaObject(p_data.Object);
            }
            else if (p_data.TopicName == ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_TradingInfo)
            {
                sendKafka = Kafka_TradingInfoStatus.SendToKafkaObject(p_data.Object);
            }
            else
            {
                //Có gì đó sai sai, trường hợp này không được xảy ra nhưng nếu xảy ra thì không thoát được vòng while nếu ko xử lý
                //ghi log cảnh báo lại
                log.Warn("KafkaInterface: Something went wrong, can not find kakfa topic {0} to send object {1} in sequence {2} ", p_data.TopicName, p_data.Object, sequence);
            }
            return sendKafka;
        }

        public void ThrdResendObject()
        {
            while (true)
            {
                if (c_DictKafkaObjectMissed.Count > 0)
                {
                    KafkaObjectEvent kafkaObj;
                    foreach (long i in c_DictKafkaObjectMissed.Keys)
                    {
                        c_DictKafkaObjectMissed.TryRemove(i, out kafkaObj);
                        if (kafkaObj != null)
                        {
                            while (Send2Kafka(kafkaObj, i) != SendKafkaStatus.SEND_SUCCESS)
                            {
                                Thread.Sleep(1000);
                            }
                        }
                    }
                }
                else
                {
                    Thread.Sleep(30 * 1000);
                }
            }
        }
    }

    public class KafkaObjectEvent
    {
        public string TopicName;
        public object Object;
        public long StartTimeProcess;
        public int MessageSequence;
        public char EventFlag;
    }

    internal class StoreMapSeqEvent : IEventHandler<KafkaObjectEvent>
    {
        public void OnEvent(KafkaObjectEvent p_data, long sequence, bool EndofBatch)
        {
            switch (p_data.EventFlag)
            {
                case FlagSendKafka.ACCEPT_REQUEST:
                    ShareMemoryData.c_FileStore.StoreMapSeq(p_data.MessageSequence, 0);
                    break;

                case FlagSendKafka.REPORT_SEND_HNX:
                    ShareMemoryData.c_FileStore.StoreMapSeq(p_data.MessageSequence, 8);
                    break;

                case FlagSendKafka.FORWARD_FROM_HNX:
                    ShareMemoryData.c_FileStore.StoreMapSeq(p_data.MessageSequence, 16);
                    break;

                default:
                    break;
            }
        }
    }

    public class FlagSendKafka
    {
        public const char ACCEPT_REQUEST = 'A';
        public const char REPORT_SEND_HNX = 'R';
        public const char FORWARD_FROM_HNX = 'F';
    }
}