namespace HNXTPRLGate.OtelTracing
{
    public class OtelAppSettings
    {
        public string SectionName { get; } = "OtelAppSettings";
        public string ServiceName { get; set; } = String.Empty;
        public string OtelEndpoint { get; set; } = String.Empty;
        public int OtelProtocol { get; set; } = 0;
        public bool EnableOtel { get; set; } = true;
    }
}
