using System;

namespace Tek.Contract
{
    public interface IErrorMonitor
    {
        ReleaseSettings Release { get; }

        ErrorMonitorSettings Settings { get; }

        void Error(Exception ex);
    }
}