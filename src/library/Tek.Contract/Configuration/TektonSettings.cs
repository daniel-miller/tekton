namespace Tek.Contract
{
    public class TektonSettings
    {
        public ApiSettings Api { get; set; }

        public DatabaseSettings Database { get; set; }

        public ReleaseSettings Release { get; set; }

        public SecuritySettings Security { get; set; }

        public TelemetrySettings Telemetry { get; set; }

        public IntegrationSettings Integration { get; set; }
    }
}
