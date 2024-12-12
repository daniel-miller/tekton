using System;

namespace Common.Search
{
    public interface IQuery<TResult>
    {
        Guid OriginShard { get; set; }
        Guid OriginActor { get; set; }

        Guid QueryIdentifier { get; set; }
    }
}