using Sentry.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class MonitorRegistration
{
    public static IServiceCollection AddMonitoring(this IServiceCollection services, MonitoringSettings monitoring, ReleaseSettings release)
    {
        services.AddSingleton<ILog, Log>();
        services.AddSingleton<IMonitor, Monitor>();

        SentrySdk.Init(options =>
        {
            options.AutoSessionTracking = true;
            options.DiagnosticLevel = SentryLevel.Info;
            options.Dsn = monitoring.Url;
            options.Environment = release.Environment.ToString().ToLower();
            options.Release = release.Version;
            options.SendDefaultPii = true;
            
            if (0.0 <= monitoring.Rate && monitoring.Rate <= 1.0)
            {
                options.TracesSampleRate = monitoring.Rate;
                options.ProfilesSampleRate = monitoring.Rate;
            }

            if (monitoring.Debug)
            {
                var folder = Path.GetDirectoryName(monitoring.File);
                if (folder != null)
                {
                    Directory.CreateDirectory(folder);

                    options.Debug = true;
                    options.DiagnosticLogger = new FileDiagnosticLogger(monitoring.File);
                }
            }

            services.AddSentryTunneling();

            // If Sentry's Debug setting is enabled with a FileDiagnosticLogger then the logger
            // sometimes throws an unhandled exception from here:
            // Sentry.Integrations.AppDomainProcessExitIntegration.HandleProcessExit.

            // The exception is a System.ObjectDisposedException with Message = "Cannot write to a
            // closed TextWriter". We need to disable Sentry's default automatic behaviour and
            // manually flush any queued events before application shutdown.

            options.DisableAppDomainProcessExitFlush();
        });

        return services;
    }

    public class Log : ILog
    {
        public readonly ILogger<Log> Logger;

        public Log(ILogger<Log> logger)
        {
            Logger = logger;
        }

        public void Trace(string message)
        {
            Logger.LogTrace(message);
        }

        public void Debug(string message)
        {
            Logger.LogDebug(message);
        }

        public void Information(string message)
        {
            Logger.LogInformation(message);
        }

        public void Warning(string message)
        {
            Logger.LogWarning(message);
        }

        public void Error(string message)
        {
            Logger.LogError(message);
        }

        public void Critical(string message)
        {
            Logger.LogCritical(message);
        }
    }

    public class Monitor : IMonitor
    {
        private readonly MonitoringSettings _monitoring;

        public ILog Log { get; set; }

        public Monitor(ILog debugger, MonitoringSettings monitoring)
        {
            Log = debugger;
            _monitoring = monitoring;
        }

        public void Trace(string message)
        {
            Log.Trace(message);
            if (_monitoring.Enabled)
                SentrySdk.CaptureMessage(message, SentryLevel.Debug);
        }

        public void Debug(string message)
        {
            Log.Debug(message);
            if (_monitoring.Enabled)
                SentrySdk.CaptureMessage(message, SentryLevel.Debug);
        }

        public void Information(string message)
        {
            Log.Information(message);
            if (_monitoring.Enabled)
                SentrySdk.CaptureMessage(message, SentryLevel.Info);
        }

        public void Warning(string message)
        {
            Log.Warning(message);
            if (_monitoring.Enabled)
                SentrySdk.CaptureMessage(message, SentryLevel.Warning);
        }

        public void Error(string message)
        {
            Log.Error(message);
            if (_monitoring.Enabled)
                SentrySdk.CaptureMessage(message, SentryLevel.Error);
        }

        public void Critical(string message)
        {
            Log.Critical(message);
            if (_monitoring.Enabled)
                SentrySdk.CaptureMessage(message, SentryLevel.Fatal);
        }

        public void Flush()
        {
            SentrySdk.Flush(TimeSpan.FromSeconds(5));
        }

        public async Task FlushAsync()
        {
            await SentrySdk.FlushAsync(TimeSpan.FromSeconds(5));
        }
    }
}