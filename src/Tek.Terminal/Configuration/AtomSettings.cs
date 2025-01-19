using Tek.Common;

namespace Tek.Terminal
{
    public class AtomSettings
    {
        public ReleaseSettings Release { get; set; } = null!;
        public KernelSettings Kernel { get; set; } = null!;
        public PluginSettings Plugin { get; set; } = null!;
    }

    public class KernelSettings
    {
        public TelemetrySettings Telemetry { get; set; } = null!;
    }

    public class TelemetrySettings
    {
        public LoggingSettings Logging { get; set; } = null!;
        public MonitoringSettings Monitoring { get; set; } = null!;
    }

    public class LoggingSettings
    {
        public string Path { get; set; } = null!;
    }

    public class MonitoringSettings
    {
        public string Path { get; set; } = null!;
    }

    public class PluginSettings
    {
        public IntegrationSettings Integration { get; set; } = null!;
    }

    public class IntegrationSettings
    {
        public AstronomyApiSettings AstronomyApi { get; set; } = null!;
        public VisualCrossingSettings VisualCrossing { get; set; } = null!;
    }

    public class AstronomyApiSettings
    {
        public string Host { get; set; } = null!;
        public string App { get; set; } = null!;
        public string Secret { get; set; } = null!;
    }

    public class VisualCrossingSettings
    {
        public string Host { get; set; } = null!;
        public string Secret { get; set; } = null!;
    }
}
