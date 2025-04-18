using Sentry.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class SentryRegistration
{
    public static IServiceCollection AddMonitoringServices(this IServiceCollection services, MonitoringSettings sentry)
    {
        SentrySdk.Init(options =>
        {
            options.Dsn = sentry.Url;

            if (sentry.Debug)
            {
                options.Debug = true;

                options.DiagnosticLogger = new FileDiagnosticLogger(sentry.File);

                // If Debug is enabled with a FileDiagnosticLogger then the logger throws an
                // unhandled exception from Sentry.Integrations.AppDomainProcessExitIntegration.HandleProcessExit.
                // The exception is a System.ObjectDisposedException with Message = "Cannot write to
                // a closed TextWriter". We need to disable Sentry's default automatic behaviour
                // here and manually flush any queued events before application shutdown.

                options.DisableAppDomainProcessExitFlush();
            }

            options.AutoSessionTracking = true;

            if (0.0 <= sentry.Rate && sentry.Rate <= 1.0)
            {
                options.TracesSampleRate = sentry.Rate;

                options.ProfilesSampleRate = sentry.Rate;
            }
        });

        return services;
    }
}
