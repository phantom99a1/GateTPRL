using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace HNXTPRLGate.OtelTracing
{
    /// <summary>
    /// It is recommended to use a custom type to hold references for
    /// ActivitySource and Instruments. This avoids possible type collisions
    /// with other components in the DI container.
    /// </summary>
    public class Instrumentation : IDisposable
    {
        /*internal const string ActivitySourceName = "Examples.AspNetCore";
        internal const string MeterName = "Examples.AspNetCore";*/
        private readonly Meter meter;

        public Instrumentation(OtelAppSettings settings)
        {
            string? version = typeof(Instrumentation).Assembly.GetName().Version?.ToString();
            this.ActivitySource = new ActivitySource(settings.ServiceName, version);
            this.meter = new Meter(settings.ServiceName, version);
            //this.FreezingDaysCounter = this.meter.CreateCounter<long>("Gate.HNX.TPRL", "count something???");
        }

        public ActivitySource ActivitySource { get; }

        //public Counter<long> FreezingDaysCounter { get; }

        public void Dispose()
        {
            this.ActivitySource.Dispose();
            this.meter.Dispose();
        }
    }
}