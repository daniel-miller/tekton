using System;

namespace Common
{
    public interface IExceptionMonitor
    {
        ReleaseOptions Release { get; }
        SentryOptions Sentry { get; }

        void Error(Exception ex);
    }
}