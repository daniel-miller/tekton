using System.Collections.Generic;

namespace Tek.Contract
{
    public interface IPrincipalSearch
    {
        IPrincipal GetPrincipal(string secret);

        IPrincipal GetPrincipal(JwtRequest request, string ipAddress, string whitelist, int? lifetime, List<string> errors);
    }
}