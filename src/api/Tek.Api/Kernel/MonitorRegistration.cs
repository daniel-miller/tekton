using Sentry.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class MonitorRegistration
{
    public static IServiceCollection AddMonitoring(this IServiceCollection services, MonitoringSettings sentry)
    {
        SentrySdk.Init(options =>
        {
            options.Dsn = sentry.Url;

            if (sentry.Debug)
            {
                // If Sentry's Debug setting is enabled with a FileDiagnosticLogger then the logger
                // sometimes throws an unhandled exception from here:
                // Sentry.Integrations.AppDomainProcessExitIntegration.HandleProcessExit.

                // The exception is a System.ObjectDisposedException with Message = "Cannot write to
                // a closed TextWriter". We need to disable Sentry's default automatic behaviour and
                // manually flush any queued events before application shutdown.

                options.Debug = true;
                options.DiagnosticLogger = new FileDiagnosticLogger(sentry.Path);
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
