using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Tek.Contract
{
    public interface IPrincipalAdapter
    {
        IEnumerable<Claim> ToClaims(IPrincipal principal, string ipAddress);
        Dictionary<string, List<string>> ToDictionary(IEnumerable<Claim> claims);
        IPrincipal ToPrincipal(Dictionary<string, string> claims);
        IPrincipal ToPrincipal(IEnumerable<Claim> claims);
        IPrincipal ToPrincipal(IJwt jwt);
        IPrincipal ToPrincipal(IPrincipal principal, string ipAddress, int? requestedTokenLifetime, int? defaultTokenLifetime);
    }
}