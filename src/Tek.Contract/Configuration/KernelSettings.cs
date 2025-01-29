namespace Tek.Contract
{
    public class KernelSettings
    {
        public DatabaseSettings Database { get; set; }
        public ReleaseSettings Release { get; set; }
        public TelemetrySettings Telemetry { get; set; }
    }
}
