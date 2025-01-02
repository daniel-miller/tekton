using System;

namespace Atomic.Common.Bus
{
    public interface IQuery<TResult>
    {
        int OriginShard { get; set; }
        Guid OriginActor { get; set; }

        Guid QueryIdentifier { get; set; }
    }
}