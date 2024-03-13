using BusinessProcessResponse;
using CommonLib;
using KafkaInterface;
using Moq;
using System.Runtime.InteropServices;

namespace HNXUnitTest
{
    [TestClass]
    public class KafkaClientTest
    {
        [TestMethod]
        public void Send2KafkaObject_MsgAI()
        {
            IKafkaClient c_service = new KafkaClient();

            var _return = c_service.Send2KafkaObject("Topic_Test", new ResponseMessageKafka(), 0, 0, It.IsAny<char>());
            
            c_service.NumOfMsg();
            c_service.ItemsInQueue();

            Assert.AreEqual(_return >= 0, true);
        }

        [TestMethod]
        public void KafkaHelper_Test()
        {
            IKafkaHelper Kafka_OrderStatus = new KafkaHelper(ConfigData.KafkaConfig.KafkaAuth,
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
            //
            IKafkaHelper Kafka_ExecutionStatus = new KafkaHelper(ConfigData.KafkaConfig.KafkaAuth,
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
            //
            IKafkaHelper Kafka_TradingInfoStatus = new KafkaHelper(ConfigData.KafkaConfig.KafkaAuth,
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
            //
            Kafka_OrderStatus.SendToKafkaObject(new ResponseMessageKafka());
            Kafka_OrderStatus.NumOfMsg();
            //
            Kafka_ExecutionStatus.SendToKafkaObject(new ResponseMessageKafka());
            Kafka_ExecutionStatus.NumOfMsg();
            //
            Kafka_TradingInfoStatus.SendToKafkaObject(new ResponseMessageKafka());
            Kafka_TradingInfoStatus.NumOfMsg();
            //
            Assert.IsTrue(true);
        }
    }
}