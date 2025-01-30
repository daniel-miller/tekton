using System.Collections.Generic;

namespace Tek.Bus
{
    public class AggregateImport
    {
        public AggregateRoot Aggregate { get; set; }
        public IEnumerable<IEvent> Events { get; set; }
    }
}