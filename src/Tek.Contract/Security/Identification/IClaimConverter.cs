using System.Collections.Generic;
using System.Security.Claims;

namespace Tek.Contract
{
    public interface IClaimConverter
    {
        int CalculateLifetime(int? assigned, int? requested, int? @default);

        IEnumerable<Claim> ToClaims(IPrincipal principal);

        Dictionary<string, List<string>> ToDictionary(IEnumerable<Claim> claims);

        IPrincipal ToPrincipal(IJwt jwt);
        IPrincipal ToPrincipal(Dictionary<string, string> claims);
        IPrincipal ToPrincipal(IEnumerable<Claim> claims);

        IPrincipal ToSentinel(JwtRequest request);
    }
}