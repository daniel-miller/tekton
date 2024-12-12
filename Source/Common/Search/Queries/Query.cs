using System;

namespace Common.Search
{
    public class Query<TResult> : IQuery<TResult>
    {
        public Guid OriginShard { get; set; }
        public Guid OriginActor { get; set; }

        public Guid QueryIdentifier { get; set; }

        public Query()
        {
            QueryIdentifier = GuidGenerator.NewGuid();
        }
    }
}