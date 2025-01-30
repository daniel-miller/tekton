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
    private readonly IClaimConverter _converter;

    public JwtAuthenticationHandler(IOptionsMonitor<JwtAuthenticationOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, IClaimConverter converter,
        ReleaseSettings releaseSettings, TokenSettings tokenSettings)
        
        : base(options, logger, encoder)
    {
        _converter = converter;

        _releaseSettings = releaseSettings;
        _tokenSettings = tokenSettings;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (AllowAnonymous())
            return Task.FromResult(AuthenticateResult.NoResult());

        var encoder = new JwtEncoder();

        var token = encoder.Extract(DefaultScheme, Request.Headers["Authorization"].ToString());

        if (token == null)
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));

        if (!encoder.Validate(token, DefaultScheme, _releaseSettings.Secret, _tokenSettings.Audience, _tokenSettings.Issuer, _converter, out var principal))
            return Task.FromResult(AuthenticateResult.Fail("Invalid Token"));

        var ticket = new AuthenticationTicket(principal, DefaultScheme);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    private bool AllowAnonymous()
    {
        var endpoint = Context.GetEndpoint();

        return endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;
    }
}
