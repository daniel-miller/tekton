using System;

namespace Common
{
    public interface IExceptionMonitor
    {
        PlatformOptions Platform { get; }
        SentryOptions Sentry { get; }

        void Error(Exception ex);
    }
}