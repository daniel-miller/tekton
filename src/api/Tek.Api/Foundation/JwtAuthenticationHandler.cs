using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Tek.Api;

public class JwtAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "Bearer";
    public static string Scheme => DefaultScheme;
    public static string AuthenticationType => DefaultScheme;
}

public class JwtAuthenticationHandler : AuthenticationHandler<JwtAuthenticationOptions>
{
    public const string DefaultScheme = JwtAuthenticationOptions.DefaultScheme;

    private readonly SecuritySettings _securitySettings;
    private readonly IClaimConverter _converter;

    public JwtAuthenticationHandler(IOptionsMonitor<JwtAuthenticationOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, IClaimConverter converter,
        SecuritySettings securitySettings)
        
        : base(options, logger, encoder)
    {
        _converter = converter;

        _securitySettings = securitySettings;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (AllowAnonymous())
            return Task.FromResult(AuthenticateResult.NoResult());

        var encoder = new JwtEncoder();

        var token = encoder.Extract(DefaultScheme, Request.Headers["Authorization"].ToString());

        if (token == null)
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));

        if (!encoder.Validate(DefaultScheme, token, _securitySettings.Secret, _securitySettings.Token.Audience, _securitySettings.Token.Issuer, _converter, out var principal))
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
