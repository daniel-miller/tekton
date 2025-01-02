using System;

namespace Atomic.Common
{
    public interface IErrorMonitor
    {
        ReleaseSettings Release { get; }
        ErrorMonitorSettings Settings { get; }

        void Error(Exception ex);
    }
}