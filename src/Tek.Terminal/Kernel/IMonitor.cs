namespace Tek.Terminal
{
    public interface ILog
    {
        void Critical(string message);
        void Debug(string message);
        void Error(string message);
        void Information(string message);
        void Trace(string message);
        void Warning(string message);
    }

    public interface IMonitor
    {
        ILog Log { get; set; }

        void Critical(string message);
        void Debug(string message);
        void Error(string message);
        void Information(string message);
        void Trace(string message);
        void Warning(string message);

        Task Flush();
    }
}