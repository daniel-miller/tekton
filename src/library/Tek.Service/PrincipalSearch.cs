namespace Tek.Service;

public class PrincipalSearch : QueryRunner, IPrincipalSearch
{
    private readonly IClaimConverter _converter;

    public PrincipalSearch(IClaimConverter converter)
    {
        _converter = converter;
    }

    public IPrincipal? GetPrincipal(string secret)
    {
        throw new NotImplementedException();
    }

    public IPrincipal? GetPrincipal(JwtRequest request, string ipAddress, string whitelist, int? lifetime, List<string> errors)
    {
        IPrincipal? principal = null;

        if (IsWhitelisted(ipAddress, whitelist))
            principal = _converter.ToSentinel(request);

        if (principal == null)
            principal = GetPrincipal(request.Secret);

        if (principal == null)
            return null;

        principal.IPAddress = ipAddress;

        principal.Claims.Lifetime = _converter.CalculateLifetime(principal.Claims.Lifetime, request.Lifetime, lifetime);

        return principal;
    }

    private bool IsWhitelisted(string ipAddress, string whitelist)
    {
        if (ipAddress.IsEmpty())
            return false;

        if (whitelist.IsEmpty())
            return true;

        var list = whitelist.Parse();

        return ipAddress.MatchesAny(list);
    }
}