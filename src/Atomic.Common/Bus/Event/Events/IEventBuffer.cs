using System.Collections.Generic;

namespace Atomic.Common.Bus
{
    public interface IEventBuffer
    {
        void Open();
        void Save(AggregateRoot aggregate, IEnumerable<IEvent> events);
        void Flush();
        void Close();
    }
}
