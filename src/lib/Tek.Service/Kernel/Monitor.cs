using Microsoft.Extensions.Logging;

namespace Tek.Service;

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
    public ILog Log { get; set; }

    public Monitor(ILog debugger)
    {
        Log = debugger;
    }

    public void Trace(string message)
    {
        Log.Trace(message);
        SentrySdk.CaptureMessage(message, SentryLevel.Debug);
    }

    public void Debug(string message)
    {
        Log.Debug(message);
        SentrySdk.CaptureMessage(message, SentryLevel.Debug);
    }

    public void Information(string message)
    {
        Log.Information(message);
        SentrySdk.CaptureMessage(message, SentryLevel.Info);
    }

    public void Warning(string message)
    {
        Log.Warning(message);
        SentrySdk.CaptureMessage(message, SentryLevel.Warning);
    }

    public void Error(string message)
    {
        Log.Error(message);
        SentrySdk.CaptureMessage(message, SentryLevel.Error);
    }

    public void Critical(string message)
    {
        Log.Critical(message);
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