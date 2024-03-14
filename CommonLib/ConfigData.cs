using Confluent.Kafka;
using Disruptor;
using Microsoft.Extensions.Configuration;

namespace CommonLib
{
    public class KafkaConfig
    {
        public bool KafkaAuth { get; set; } = false;
        public string KafkaUser { get; set; } = string.Empty;
        public string KafkaPassword { get; set; } = string.Empty;
        public string KafkaCALocation { get; set; } = string.Empty;
        public string KafkaIp { get; set; } = string.Empty;
        public string KafkaPort { get; set; } = string.Empty;
        public string KafkaTopic_HNXTPRL_OrderStatus { get; set; } = string.Empty;
        public string KafkaTopic_HNXTPRL_OrderExecution { get; set; } = string.Empty;
        public string KafkaTopic_HNXTPRL_TradingInfo { get; set; } = string.Empty;

        //
        public Confluent.Kafka.Acks Kafka_Acks { get; set; }

        public Confluent.Kafka.CompressionType Kafka_CompressionType { get; set; }

        public double Kafka_LingerMs { get; set; } = 5;
        public int Kafka_BatchSize { get; set; } = 1000000;
    }

    public class RetryQueue
    {
        public bool Enable { get; set; } = true;
        public int Interval { get; set; } = 2000;
        public int MaxTimes { get; set; } = 50;
    }

    public static class ConfigData

    {
        public static string MainBoard = string.Empty; //Bảng chính - vì hệ thống trái phiếu chỉ có 1 bảng, nên sẽ dùng phiên, của bảng này cho việc check phiên

        //Security
        public static string AES_Key = ""; // 87c580ef1e5dfe7f89c3b869eb00c67c

        public static string AES_IV = ""; //eb00c67cd56bd758

        //File Path
        public static string LogHNXDataPath = "LogHNXPData";

        public static string LogOrderPath = "LogOrderPath";

        //thông tin TCP
        public static string IPServer = "";

        public static int PortServer = 48000;
        public static string FirmID = "003";
        public static string Username = "003.01GW";
        public static string TraderID = "003.01GW";
        public static string Password = "";
        public static string PasswordInConfig = ""; // để tạm để test
        public static string TargetCompID = "";
        public static string TargetSubID = "";
        public static int Heartbeat = 30;
        public static int SafeWindowSize = 100;
        public static int BackupCapacity = 1000;
        public static Dictionary<string, string> DictError_Code_Text = new Dictionary<string, string>();

        //
        public static int DefaultBufferSize = 512;

        public static int KeapAliveInterval = 20;//chu kỳ

        public static int APIBusinessPort = 0;

        public static string APIMonitorDomain = "";
        public static int APIMonitorPort = 0;
        public static string TokenSecret = "";

        //Config cho queue
        public static int QueueSize = 2048;

        public static int PendingQueueTime = 30;
        public static RetryQueue RetryQ = new RetryQueue();

        //Config cho Kafka
        public static KafkaConfig KafkaConfig = new KafkaConfig();

        public static List<UserInfo> listUsers = null;

        public static IWaitStrategy StrategyMode = new BlockingSpinWaitWaitStrategy();

        //
        public static string VaultAddress = "";

        public static string VaultUsername = "";
        public static string VaultPassword = "";
        public static string VaultPath = "";
        public static bool EnableVault { get; set; } = true;
        public static bool EnableMaskSensitiveData { get; set; } = false;

        public static bool ConnectExchange { get; set; } = false;


        public static void InitConfig(IConfiguration configuration)
        {
            if (configuration != null)
            {
                AES_Key = configuration["AES_Key"].ToString();
                AES_IV = configuration["AES_IV"].ToString();
                //
                APIBusinessPort = CommonLib.Utils.ParseInt(configuration["Kestrel:Endpoints:APIBusiness:Url"]?.Split(':')?.Last());
                //
                APIMonitorDomain = configuration["APIDomain"];
                APIMonitorPort = CommonLib.Utils.ParseInt(configuration["Kestrel:Endpoints:APIMonitor:Url"]?.Split(':')?.Last());

                LogHNXDataPath = configuration["LogHNXDataPath"];
                LogOrderPath = configuration["LogOrderDataPath"];
                //
                if (configuration["HNXConfig:DefaultBufferSize"] != null) { DefaultBufferSize = CommonLib.Utils.ParseInt(configuration["HNXConfig:DefaultBufferSize"]); }

                IPServer = configuration["HNXConfig:IPServer"];
                if (configuration["HNXConfig:PortServer"] != null) { PortServer = CommonLib.Utils.ParseInt(configuration["HNXConfig:PortServer"]); }
                FirmID = configuration["HNXConfig:FirmID"];
                Username = configuration["HNXConfig:Username"];
                PasswordInConfig = Utils.DecryptAES(configuration["HNXConfig:Password"], AES_Key, AES_IV);
                TraderID = configuration["HNXConfig:TraderID"];
                DefaultBufferSize = Utils.ParseInt(configuration["HNXConfig:DefaultBufferSize"]);
                Heartbeat = Utils.ParseInt(configuration["HNXConfig:Heartbeat"]);
                SafeWindowSize = Utils.ParseInt(configuration["HNXConfig:SafeWindowSize"]);
                BackupCapacity = Utils.ParseInt(configuration["HNXConfig:BackupCapacity"]);
                //

                TokenSecret = configuration["TokenSecret"].ToString();
                // Kafka

                KafkaConfig = new KafkaConfig()
                {
                    KafkaAuth = configuration["KafkaConfig:KafkaAuth"]?.ToString().ToUpper() == "TRUE" ? true : false,
                    KafkaIp = configuration["KafkaConfig:KafkaIP"],
                    KafkaPort = configuration["KafkaConfig:KafkaPort"],
                    KafkaUser = configuration["KafkaConfig:KafkaUser"].ToString(),
                    KafkaPassword = Utils.DecryptAES(configuration["KafkaConfig:KafkaPassword"].ToString(), AES_Key, AES_IV),
                    KafkaCALocation = configuration["KafkaConfig:KafkaCALocation"],
                    KafkaTopic_HNXTPRL_OrderStatus = configuration["KafkaConfig:KafkaTopic_HNXTPRL_OrderStatus"],
                    KafkaTopic_HNXTPRL_OrderExecution = configuration["KafkaConfig:KafkaTopic_HNXTPRL_OrderExecution"],
                    KafkaTopic_HNXTPRL_TradingInfo = configuration["KafkaConfig:KafkaTopic_HNXTPRL_TradingInfo"]
                };
                // default Acks
                KafkaConfig.Kafka_Acks = Acks.All;
                //
                if (configuration["KafkaConfig:Acks"] == "0")
                {
                    KafkaConfig.Kafka_Acks = Acks.None;
                }
                else if (configuration["KafkaConfig:Acks"] == "1")
                {
                    KafkaConfig.Kafka_Acks = Acks.Leader;
                }

                // default CompressionType
                KafkaConfig.Kafka_CompressionType = CompressionType.None;
                //
                if (configuration["KafkaConfig:CompressionType"] == "1")
                {
                    KafkaConfig.Kafka_CompressionType = CompressionType.Gzip;
                }
                else if (configuration["KafkaConfig:CompressionType"] == "2")
                {
                    KafkaConfig.Kafka_CompressionType = CompressionType.Snappy;
                }
                else if (configuration["KafkaConfig:CompressionType"] == "3")
                {
                    KafkaConfig.Kafka_CompressionType = CompressionType.Lz4;
                }
                else if (configuration["KafkaConfig:CompressionType"] == "4")
                {
                    KafkaConfig.Kafka_CompressionType = CompressionType.Zstd;
                }
                // LingerMs
                if (configuration["KafkaConfig:LingerMs"] != null)
                {
                    KafkaConfig.Kafka_LingerMs = Utils.ParseDoubleSpan(configuration["KafkaConfig:LingerMs"]);
                }
                // BatchSize
                if (configuration["KafkaConfig:BatchSize"] != null)
                {
                    KafkaConfig.Kafka_BatchSize = Utils.ParseInt(configuration["KafkaConfig:BatchSize"]);
                }
                QueueSize = Utils.ParseInt(configuration["QueueSize"]);
                PendingQueueTime = Utils.ParseInt(configuration["PendingQueueTime"]);
                RetryQ.Enable = bool.Parse(configuration["RetryQueue:Enable"]);
                RetryQ.Interval = Utils.ParseInt(configuration["RetryQueue:Interval"]);
                RetryQ.MaxTimes = Utils.ParseInt(configuration["RetryQueue:MaxTimes"]);
                MainBoard = configuration["MainBoard"].ToString();
                //
                //List<UserInfo> myTestUsers = configuration.GetSection("UserListInfo").GetSection("ListUser").Get<List<UserInfo>>();
                listUsers = configuration.GetSection("Users").Get<List<UserInfo>>();
                int ModeWaitStrategy = Utils.ParseInt(configuration["ModeStrategy"]);
                switch (ModeWaitStrategy)
                {
                    case 0:
                        StrategyMode = new BlockingSpinWaitWaitStrategy();
                        break;

                    case 1:
                        StrategyMode = new YieldingWaitStrategy();
                        break;

                    case 2:
                        StrategyMode = new BusySpinWaitStrategy();
                        break;

                    default:
                        StrategyMode = new BlockingSpinWaitWaitStrategy();
                        break;
                }
                VaultAddress = configuration["VaultAppSettings:Address"];
                VaultUsername = configuration["VaultAppSettings:Username"];
                VaultPassword = Utils.DecryptAES(configuration["VaultAppSettings:Password"], AES_Key, AES_IV);
                VaultPath = configuration["VaultAppSettings:Path"];
                EnableVault = bool.Parse(configuration["VaultAppSettings:EnableVault"]);
                EnableMaskSensitiveData = bool.Parse(configuration["EnableMaskSensitiveData"]);

                ConnectExchange = bool.Parse(configuration["ConnectExchange"]);


            }
        }

        public class UserInfo
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
        }

        public static void ReadErrorCodeData(string PathName)
        {
            List<string> _result = new List<string>();
            char[] char_remove = new char[] { '\"' };
            try
            {
                if (System.IO.File.Exists(PathName))
                {
                    System.IO.FileStream _FileStream = System.IO.File.Open(PathName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
                    StreamReader _StreamReader = new StreamReader(_FileStream);
                    while (!_StreamReader.EndOfStream)
                    {
                        string s = _StreamReader.ReadLine();
                        string[] codeText = s.Split(',', 2);
                        if (codeText.Length > 1)
                            DictError_Code_Text.TryAdd(codeText[0], codeText[1].Trim(char_remove));
                    }
                    _StreamReader.Close();
                    _FileStream.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }
    }
}