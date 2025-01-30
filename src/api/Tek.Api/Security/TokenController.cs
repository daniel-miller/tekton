using Microsoft.AspNetCore.Mvc;

namespace Tek.Api;

[ApiController]
[ApiExplorerSettings(GroupName = "Security: Authentication")]
public class TokenController : ControllerBase
{
    private readonly ReleaseSettings _releaseSettings;
    private readonly TokenSettings _tokenSettings;

    private readonly IClaimConverter _claimConverter;
    private readonly IPrincipalSearch _principalSearch;

    public TokenController(ReleaseSettings releaseSettings, TokenSettings tokenSettings,
        IClaimConverter claimConverter, IPrincipalSearch principalSearch)
    {
        _releaseSettings = releaseSettings;
        _tokenSettings = tokenSettings;

        _principalSearch = principalSearch;
        _claimConverter = claimConverter;
    }

    [HttpPost("api/token")]
    public IActionResult GenerateToken([FromBody] JwtRequest request)
    {
        var ip = GetClientIPAddress();

        if (ip.IsEmpty())
            return Unauthorized("Missing IP address");

        if (request.Secret.IsEmpty())
            return Unauthorized("Missing secret");

        var errors = new List<string>();

        var principal = _principalSearch.GetPrincipal(request, ip, _tokenSettings.Whitelist, _tokenSettings.Lifetime, errors);

        if (principal == null)
        {
            return errors.Count > 0
                ? Unauthorized(string.Join(". ", errors))
                : Unauthorized("Invalid secret");
        }

        var jwt = CreateToken(principal, ip, request.Debug);

        return Ok(jwt);
    }

    [HttpGet("api/version")]
    public IActionResult GetVersion()
    {
        var version = typeof(TokenController).Assembly.GetName().Version;

        return Ok(version);
    }

    [HttpPost("api/validate")]
    public async Task<IActionResult> ValidateToken()
    {
        var token = string.Empty;

        using (var reader = new StreamReader(Request.Body))
        {
            token = await reader.ReadToEndAsync();
        }

        var encoder = new JwtEncoder();

        var jwt = encoder.Decode(token);

        var audience = _tokenSettings.Audience;

        var issuer = _tokenSettings.Issuer;

        var result = new
        {
            jwt.Subject,

            SignatureVerification = encoder.VerifySignature(token, _releaseSettings.Secret) ? "Passed" : "Failed",

            ExpiryVerification = !jwt.IsExpired() ? $"Not Expired ({jwt.GetMinutesUntilExpiry():n0} minutes remaining)" : "Expired",

            AudienceVerification = jwt.Audience == audience ? $"Passed" : $"Failed (expected {audience})",

            IssuerVerification = jwt.Issuer == issuer ? $"Passed" : $"Failed (expected {issuer})",
        };

        return Ok(result);
    }

    private string CreateToken(IPrincipal principal, string ip, bool debug)
    {
        var securityClaims = _claimConverter.ToClaims(principal);

        var principalClaims = _claimConverter.ToDictionary(securityClaims);

        if (debug)
            AddDebugClaims(principal, principalClaims);

        var secret = _releaseSettings.Secret;

        var subject = principal.User.Email;

        var audience = _tokenSettings.Audience;

        var issuer = _tokenSettings.Issuer;

        var lifetime = principal.Claims.Lifetime ?? JwtRequest.DefaultLifetimeLimit;

        var expiry = DateTime.UtcNow.Add(TimeSpan.FromMinutes(lifetime));

        var claims = new Jwt(principalClaims, subject, issuer, audience, expiry);

        var encoder = new JwtEncoder();

        var jwt = encoder.Encode(claims, secret);

        return jwt;
    }

    private void AddDebugClaims(IPrincipal principal, Dictionary<string, List<string>> claims)
    {
        if (principal.Roles != null && principal.Roles.Any())
        {
            var list = principal.Roles
                .Select(x => $"{x.Identifier} (v{UuidFactory.GetVersion(x.Identifier)} UUID) {x.Name}")
                .ToList();

            claims.Add("user_role_debug", list);
        }

        var user = principal.User;

        var userId = principal.User.Identifier;

        if (userId != Guid.Empty)
        {
            claims.Add("user_debug", [$"{userId} (v{UuidFactory.GetVersion(userId)} UUID) {user.Name} <{user.Email}>"]);
        }
    }

    private string GetClientIPAddress()
        => Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "::1";
}