using System;

namespace Atom.Common
{
    public interface IErrorMonitor
    {
        ReleaseSettings Release { get; }
        ErrorMonitorSettings Settings { get; }

        void Error(Exception ex);
    }
}