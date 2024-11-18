using System;

namespace Common
{
    public interface IExceptionMonitor
    {
        Environment Environment { get; }
        SentryOptions Sentry { get; }

        void Error(Exception ex);
    }
}