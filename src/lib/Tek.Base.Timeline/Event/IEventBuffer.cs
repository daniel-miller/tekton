using System.Collections.Generic;

namespace Tek.Base.Timeline
{
    public interface IEventBuffer
    {
        void Open();
        void Save(AggregateRoot aggregate, IEnumerable<IEvent> events);
        void Flush();
        void Close();
    }
}
