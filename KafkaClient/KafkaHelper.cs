using Confluent.Kafka;
using Confluent.Kafka.Extensions.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KafkaInterface
{
    public interface IKafkaHelper
    {
        public int SendToKafkaObject(Object p_object);

        public int NumOfMsg();
    }

    public class KafkaHelper : IKafkaHelper
    {
        public static readonly NLog.Logger log = CommonLib.Logger.ResponseLog;

        private readonly ProducerConfig c_KafkaConfig;
        private readonly string c_KafkaTopic;
        private IProducer<Null, string> producer;
        private int numOfMsg = 0;

        private JsonSerializerSettings jsonCamelCaseSetting = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        public KafkaHelper(bool p_KafkaAuth, string p_KafkaIP, string p_KafkaPort, string p_KafkaUser, string p_KafkaPassword, string p_CALocation, string p_KafkaTopic, Acks p_Kafka_Acks, CompressionType p_Kafka_CompressionType, double p_Kafka_LingerMs, int p_Kafka_BatchSize, bool sslKafka)
        {
            if (p_KafkaAuth)
            {
                if (sslKafka == true)
                {
                    c_KafkaConfig = new ProducerConfig
                    {
                        BootstrapServers = $"{p_KafkaIP}:{p_KafkaPort}",
                        // Note: The AutoOffsetReset property determines the start offset in the event
                        // there are not yet any committed offsets for the consumer group for the
                        // topic/partitions of interest. By default, offsets are committed
                        // automatically, so in this example, consumption will only start from the
                        // earliest message in the topic 'my-topic' the first time you run the program.
                        SaslUsername = p_KafkaUser,
                        Acks = p_Kafka_Acks,  //default Acks.All
                        CompressionType = p_Kafka_CompressionType, //default CompressionType.None
                        LingerMs = p_Kafka_LingerMs, //default 5
                        BatchSize = p_Kafka_BatchSize, //default 1000000
                        SaslPassword = p_KafkaPassword,
                        SaslMechanism = SaslMechanism.Plain,
                        SecurityProtocol = SecurityProtocol.SaslSsl,
                        SslCaLocation = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + p_CALocation // Thư mục chứa file CARoot.pem
                    };
                }
                else
                {
                    c_KafkaConfig = new ProducerConfig
                    {
                        BootstrapServers = $"{p_KafkaIP}:{p_KafkaPort}",
                        SaslUsername = p_KafkaUser,
                        Acks = p_Kafka_Acks,  //default Acks.All
                        CompressionType = p_Kafka_CompressionType, //default CompressionType.None
                        LingerMs = p_Kafka_LingerMs, //default 5
                        BatchSize = p_Kafka_BatchSize, //default 1000000
                        SaslPassword = p_KafkaPassword,
                        SaslMechanism = SaslMechanism.Plain
                        //SecurityProtocol = SecurityProtocol.SaslSsl,
                        //SslCaLocation = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + p_CALocation // Thư mục chứa file CARoot.pem
                    };
                }
            }
            else
            {
                c_KafkaConfig = new ProducerConfig
                {
                    BootstrapServers = $"{p_KafkaIP}:{p_KafkaPort}"
                };
            }
            c_KafkaTopic = p_KafkaTopic;
            producer = new ProducerBuilder<Null, string>(c_KafkaConfig).BuildWithInstrumentation();
            //
            CommonLib.Logger.log.Info($"KafkaHelper Init with p_KafkaAuth= {(p_KafkaAuth == true ? "True" : "False")}, p_KafkaIP={p_KafkaIP}, p_KafkaPort={p_KafkaPort}, p_KafkaUser={p_KafkaUser}, p_CALocation={p_CALocation}, p_KafkaTopic={p_KafkaTopic},sslKafka={(sslKafka==true?"True":"False")}");
        }

        public int SendToKafkaObject(Object p_object)
        {
            try
            {
                bool checkStatusSend = false;
                string _value = JsonConvert.SerializeObject(p_object, jsonCamelCaseSetting);
                producer.Produce(c_KafkaTopic, new Message<Null, string> { Value = _value }, (deliveryReport) =>
                {
                    if (deliveryReport.Error.Code != ErrorCode.NoError)
                    {
                        log.Error("Send to Kafka error. Reason : {0} ", deliveryReport.Error.Reason);
                    }
                    else
                    {
                        checkStatusSend = true;
                        log.Info("Send to Kafka success. Message : {0}. Topic : {1}", _value, c_KafkaTopic);
                        Interlocked.Increment(ref numOfMsg);
                    }
                });
                //
                producer.Flush(TimeSpan.FromSeconds(1));
                //
                if (checkStatusSend)
                {
                    return SendKafkaStatus.SEND_SUCCESS;
                }
                else
                {
                    return SendKafkaStatus.SEND_FAILURE;
                }
            }
            catch (ProduceException<Null, string> exc)
            {
                log.Error("Send to Kafka failed. Message : {0} Error : {1}", JsonConvert.SerializeObject(p_object), exc);
                return SendKafkaStatus.SEND_FAILURE;
            }
        }

        public int NumOfMsg()
        {
            return numOfMsg;
        }
    }
}