namespace Tek.Integration.PreMailer.Api;

public class EngineSettings
{
    public ReleaseSettings Release { get; set; } = null!;

    public TelemetrySettings Telemetry { get; set; } = null!;
}
