namespace Tek.Api;

public class PrincipalSearch : IPrincipalSearch
{
    private readonly SecuritySettings _settings;

    private readonly IPrincipalAdapter _adapter;

    private readonly List<string> _errors;

    public List<string> Errors
        => _errors;

    public PrincipalSearch(SecuritySettings settings, IPrincipalAdapter adapter)
    {
        _settings = settings;

        _adapter = adapter;

        _errors = new List<string>();
    }

    public IPrincipal? GetPrincipal(string secret)
    {
        throw new NotImplementedException();
    }

    public IPrincipal? GetPrincipal(JwtRequest request, string ipAddress, string whitelist, int? lifetime)
    {
        var sentinel = GetSentinel(request, ipAddress, whitelist);
        if (sentinel != null)
            return _adapter.ToPrincipal(sentinel, ipAddress, null, request.Lifetime);

        var principal = GetPrincipal(request.Secret);
        if (principal == null)
            return null;

        return _adapter.ToPrincipal(principal, ipAddress, request.Lifetime, lifetime);
    }

    public IPrincipal? GetSentinel(JwtRequest request, string ipAddress, string whitelist)
    {
        if (_settings.Sentinels == null || _settings.Sentinels.Count() == 0)
            return null;

        var sentinel = _settings.Sentinels.SingleOrDefault(x => x.Actor.Secret == request.Secret);

        if (sentinel == null)
            return null;

        if (!IsWhitelisted(ipAddress, whitelist))
        {
            var error = $"Any request for a sentinel contact must originate from a whitelisted IP address. IP {ipAddress} is not whitelisted.";
            
            Errors.Add(error);

            return null;
        }

        var principal = new Principal();

        var actor = sentinel.Actor;

        principal.User = new Actor
        {
            Email = actor.Email,
            Name = actor.Name,
            Language = actor.Language ?? PrincipalAdapter.DefaultLanguage,
            TimeZone = actor.TimeZone ?? PrincipalAdapter.DefaultTimeZone
        };

        if (actor.Identifier != Guid.Empty)
            principal.User.Identifier = actor.Identifier;
        else
            principal.User.Identifier = UuidFactory.CreateV5(_adapter.NamespaceId, actor.Email);

        principal.IPAddress = ipAddress;

        if (request.Organization.HasValue)
            principal.Organization = new Model { Identifier = request.Organization.Value };

        if (sentinel.Roles != null && sentinel.Roles.Length > 0)
            principal.Roles = sentinel.Roles.Select(CreateRole).ToList();

        principal.Roles.Add(CreateRole(actor.Email));

        return principal;
    }

    private Role CreateRole(string name)
    {
        var role = new Role
        {
            Identifier = UuidFactory.CreateV5(_adapter.NamespaceId, name),
            Name = name
        };

        return role;
    }

    static private bool IsWhitelisted(string? ipAddress, string whitelist)
    {
        if (ipAddress.IsEmpty())
            return false;

        if (whitelist.IsEmpty())
            return true;

        var list = whitelist.Parse();

        return ipAddress.MatchesAny(list);
    }
}
