using System;

namespace Atomic.Common.Bus
{
    public class Query<TResult> : IQuery<TResult>
    {
        public Filter Filter { get; set; }

        public int OriginShard { get; set; }
        public Guid OriginActor { get; set; }

        public Guid QueryIdentifier { get; set; }

        public Query()
        {
            QueryIdentifier = GuidGenerator.NewGuid();
        }
    }
}