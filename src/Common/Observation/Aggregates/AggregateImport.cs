using System.Collections.Generic;

namespace Common.Observation
{
    public class AggregateImport
    {
        public AggregateRoot Aggregate { get; set; }
        public IEnumerable<IEvent> Events { get; set; }
    }
}