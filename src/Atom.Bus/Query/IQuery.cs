using System;

namespace Atom.Bus
{
    public interface IQuery<TResult>
    {
        int OriginShard { get; set; }
        Guid OriginActor { get; set; }

        Guid QueryIdentifier { get; set; }
    }
}