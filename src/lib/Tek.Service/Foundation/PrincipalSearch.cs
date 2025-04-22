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
        return null;
    }

    public IPrincipal? GetPrincipal(JwtRequest request, string ipAddress, bool isWhitelisted, int? lifetime, List<string> errors)
    {
        IPrincipal? principal = null;

        if (isWhitelisted)
            principal = _converter.ToSentinel(request);

        if (principal == null)
            principal = GetPrincipal(request.Secret);

        if (principal == null)
            return null;

        principal.IPAddress = ipAddress;

        principal.Claims.Lifetime = _converter.CalculateLifetime(principal.Claims.Lifetime, request.Lifetime, lifetime);

        return principal;
    }
}