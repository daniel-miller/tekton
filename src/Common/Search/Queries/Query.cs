using System;

namespace Common.Search
{
    public class Query<TResult> : IQuery<TResult>
    {
        public Guid OriginOrganization { get; set; }
        public Guid OriginUser { get; set; }

        public Guid QueryIdentifier { get; set; }

        public Query()
        {
            QueryIdentifier = GuidGenerator.NewGuid();
        }
    }
}