namespace Tek.Api;

public class PrincipalSearch : QueryRunner, IPrincipalSearch
{
    private readonly IClaimConverter _converter;

    public PrincipalSearch(IClaimConverter converter)
    {
        _converter = converter;

        RegisterQuery<QueryAreYouAlive>(query => Execute((QueryAreYouAlive)query));
        RegisterQuery<QueryCountriesStartingWithC>(query => Execute((QueryCountriesStartingWithC)query));
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

    public string[] Execute(QueryCountriesStartingWithC query)
    {
        var list = new List<string>
        {
            "Cambodia",
            "Cameroon",
            "Canada",
            "Chad",
            "Chile",
            "China",
            "Colombia",
            "Costa Rica",
            "Croatia",
            "Cuba"
        };

        return list.ToArray();
    }

    public bool Execute(QueryAreYouAlive query)
    {
        return true;
    }
}

public class QueryAreYouAlive : Query<bool>
{

}

public class QueryCountriesStartingWithC : Query<string[]>
{

}