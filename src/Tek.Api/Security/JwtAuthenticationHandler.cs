using System.Security.Claims;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Tek.Api;

public class JwtAuthenticationHandler : AuthenticationHandler<JwtAuthenticationOptions>
{
    public const string DefaultScheme = JwtAuthenticationOptions.DefaultScheme;

    private readonly ReleaseSettings _releaseSettings;
    private readonly TokenSettings _tokenSettings;

    public JwtAuthenticationHandler(IOptionsMonitor<JwtAuthenticationOptions> options,
        ILoggerFactory logger, UrlEncoder encoder,
        ReleaseSettings releaseSettings, TokenSettings tokenSettings)
        : base(options, logger, encoder)
    {
        _releaseSettings = releaseSettings;
        _tokenSettings = tokenSettings;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (AllowAnonymous())
            return Task.FromResult(AuthenticateResult.NoResult());

        var token = ExtractToken();

        if (token == null)
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));

        if (!ValidateToken(token, out var principal))
            return Task.FromResult(AuthenticateResult.Fail("Invalid Token"));

        var ticket = new AuthenticationTicket(principal, DefaultScheme);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    private bool AllowAnonymous()
    {
        var endpoint = Context.GetEndpoint();

        return endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;
    }

    private string? ExtractToken()
    {
        var header = Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(header) || !header.StartsWith($"{DefaultScheme} "))
            return null;

        var token = header[$"{DefaultScheme} ".Length..].Trim();

        return token;
    }

    private bool ValidateToken(string token, out ClaimsPrincipal principal)
    {
        var encoder = new JwtEncoder();

        var jwt = encoder.Decode(token);

        principal = new ClaimsPrincipal(CreateClaimsIdentity(jwt, DefaultScheme));

        var isSignatureVerified = encoder.VerifySignature(token, _releaseSettings.Secret);

        var isAudienceVerified = jwt.Audience == _tokenSettings.Audience;

        var isIssuerVerified = jwt.Issuer == _tokenSettings.Issuer;

        return isSignatureVerified && !jwt.IsExpired() && isAudienceVerified && isIssuerVerified;
    }

    private ClaimsIdentity CreateClaimsIdentity(IJwt claims, string authenticationType)
    {
        var list = new List<Claim>();

        var dictionary = claims.ToDictionary();

        foreach (var key in dictionary.Keys)
        {
            var values = dictionary[key];

            foreach (var value in values)
            {
                list.Add(new Claim(key, value));
            }
        }

        return new ClaimsIdentity(list, authenticationType);
    }
}
