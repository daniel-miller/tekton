using System;

namespace Atom.Bus
{
    /// <summary>
    /// Provides a serialization wrapper for queries.
    /// </summary>
    public class SerializedQuery
    {
        public int OriginShard { get; set; }
        public Guid OriginActor { get; set; }

        public string QueryClass { get; set; }
        public string QueryType { get; set; }

        public string QueryCriteria { get; set; }
        public string QueryDescription { get; set; }

        public Guid QueryIdentifier { get; set; }

        public string ExecutionStatus { get; set; }
        public string ExecutionError { get; set; }
    }
}