using BusinessProcessAPIReq;
using CommonLib;
using HNXInterface;
using KafkaInterface;
using Microsoft.JSInterop;
using PendingQueue;

namespace HNXTPRLGate.Workers
{
    public class HNXTCPService : BackgroundService
    {
        private readonly iHNXClient __HNXEntity;
        private IKafkaClient _kafkaClient;
        private IProcessRevBussiness _processRevBussiness;

        public HNXTCPService(iHNXClient p_HNXEntity, IKafkaClient kafkaClient, IProcessRevBussiness processRevBussiness)
        {
            // Cấu hình chạy kafka hay không
            if (ConfigData.KafkaConfig.EnableKafka)
            {
                _kafkaClient = kafkaClient;
            }
            __HNXEntity = p_HNXEntity;
            _processRevBussiness = processRevBussiness;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var a = new CheckPendingQueue(_kafkaClient, _processRevBussiness);
            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Cấu hình chạy kafka hay không
            if (ConfigData.KafkaConfig.EnableKafka)
            {
                _kafkaClient.RecoverSeq();//Recover Kafka
            }
            __HNXEntity.Recovery();
            __HNXEntity.StartHNXTCPClient();
            _processRevBussiness.RecoverData();
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            CommonLib.Logger.log.Info("Stopping!");
            _processRevBussiness.StopReceiveApi();
            CommonLib.Logger.log.Info("Stopped enqueue order to queue");
            CommonLib.Logger.log.Info("Send Logout");
            __HNXEntity.Logout();
            CommonLib.Logger.log.Info(Thread.CurrentThread.ManagedThreadId + "|Stop CancellationToken cancellationToken");
            return Task.CompletedTask;
        }
    }
}