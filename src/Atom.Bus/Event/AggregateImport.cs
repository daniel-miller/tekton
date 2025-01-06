using System.Collections.Generic;

namespace Atom.Bus
{
    public class AggregateImport
    {
        public AggregateRoot Aggregate { get; set; }
        public IEnumerable<IEvent> Events { get; set; }
    }
}