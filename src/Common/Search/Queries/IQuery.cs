using System;

namespace Common.Search
{
    public interface IQuery<TResult>
    {
        Guid OriginOrganization { get; set; }
        Guid OriginUser { get; set; }

        Guid QueryIdentifier { get; set; }
    }
}