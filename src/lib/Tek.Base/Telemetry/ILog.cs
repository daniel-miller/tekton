namespace Tek.Base
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
}
