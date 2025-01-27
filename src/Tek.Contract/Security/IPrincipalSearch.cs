using System.Collections.Generic;

namespace Tek.Contract
{
    public interface IPrincipalSearch
    {
        List<string> Errors { get; }

        IPrincipal GetPrincipal(string secret);

        IPrincipal GetPrincipal(JwtRequest request, string ipAddress, string whitelist, int? lifetime);

        IPrincipal GetSentinel(JwtRequest request, string ipAddress, string whitelist);
    }
}