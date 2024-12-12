using System.Collections.Generic;

namespace Common.Observation
{
    public interface IEventBuffer
    {
        void Open();
        void Save(AggregateRoot aggregate, IEnumerable<IEvent> events);
        void Flush();
        void Close();
    }
}
