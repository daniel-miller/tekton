using System.Threading.Tasks;

namespace Tek.Contract
{
    public interface IMonitor
    {
        ILog Log { get; set; }

        void Critical(string message);
        void Debug(string message);
        void Error(string message);
        void Information(string message);
        void Trace(string message);
        void Warning(string message);

        void Flush();
        Task FlushAsync();
    }
}
