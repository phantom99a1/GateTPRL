using CommonLib;
using System.Diagnostics;

namespace HNXTPRLGate
{
    public class WorkerTCP : BackgroundService
    {
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
            Logger.log.Info("Starting WorkerTCP! on PID" + Process.GetCurrentProcess().Id.ToString());

            //
            Logger.log.Info("Started WorkerTCP! on ManageThreadID" + Thread.CurrentThread.ManagedThreadId.ToString());
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "|StartAsync CancellationToken cancellationToken");
            return Task.CompletedTask;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "|ExcuteAsync CancellationToken cancellationToken");

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.log.Info("Stopping!");
            //Logger.log.Info("Send Logout");
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "|Stop CancellationToken cancellationToken");
            return Task.CompletedTask;
        }
    }
}