using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tek.Api;

[ApiController]
[ApiExplorerSettings(GroupName = "Security: Debug")]
public class DebugController : ControllerBase
{
    private readonly IClaimConverter _claimConverter;
    private readonly Authorizer _authorizer;

    public DebugController(IClaimConverter principalAdapter, Authorizer authorizer)
    {
        _authorizer = authorizer;
        _claimConverter = principalAdapter;
    }

    [HttpGet(Endpoints.Debug.Endpoints)]
    [Authorize(Endpoints.Debug.Endpoints)]
    [ApiExplorerSettings(GroupName = "Security: Debug")]
    public IActionResult DebugEndpoints()
    {
        var reflector = new Reflector();

        var endpoints = reflector.FindRelativeUrls(typeof(Endpoints))
            .OrderBy(x => x.Key)
            .ToDictionary(x => x.Key, x => x.Value);

        return Ok(endpoints);
    }

    [HttpGet(Endpoints.Debug.Permissions)]
    [Authorize(Endpoints.Debug.Permissions)]
    [ApiExplorerSettings(GroupName = "Security: Debug")]
    public IActionResult DebugPermissions()
    {
        var result = new
        {
            _authorizer.Domain,
            _authorizer.NamespaceId,
            Permissions = _authorizer.GetPermissions()
        };

        return Ok(result);
    }

    [HttpGet(Endpoints.Debug.Resources)]
    [Authorize(Endpoints.Debug.Resources)]
    [ApiExplorerSettings(GroupName = "Security: Debug")]
    public IActionResult DebugResources()
    {
        var result = new
        {
            _authorizer.Domain,
            _authorizer.NamespaceId,
            Resources = _authorizer.GetResources()
        };

        return Ok(result);
    }

    [HttpGet(Endpoints.Debug.Token)]
    [Authorize(Endpoints.Debug.Token)]
    [ApiExplorerSettings(GroupName = "Security: Debug")]
    public IActionResult DebugToken()
    {
        var encoder = new JwtEncoder();

        var token = encoder.Extract(JwtAuthenticationOptions.DefaultScheme, Request.Headers["Authorization"]);

        var jwt = encoder.Decode(token);

        var principal = _claimConverter.ToPrincipal(jwt);

        var result = new
        {
            Jwt = jwt,
            Principal = principal
        };

        return Ok(result);
    }
}