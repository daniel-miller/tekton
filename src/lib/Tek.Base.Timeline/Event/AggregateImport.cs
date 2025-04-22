using System.Collections.Generic;

namespace Tek.Base.Timeline
{
    public class AggregateImport
    {
        public AggregateRoot Aggregate { get; set; }
        public IEnumerable<IEvent> Events { get; set; }
    }
}