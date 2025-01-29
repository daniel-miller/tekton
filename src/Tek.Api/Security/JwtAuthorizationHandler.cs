using Microsoft.AspNetCore.Authorization;

namespace Tek.Api;

public class JwtAuthorizationHandler : AuthorizationHandler<JwtAuthorizationRequirement>
{
    private readonly Authorizer _authorizer;

    private readonly IClaimConverter _principalAdapter;

    public JwtAuthorizationHandler(Authorizer authorizer, IClaimConverter principalAdapter)
    {
        _authorizer = authorizer;

        _principalAdapter = principalAdapter;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtAuthorizationRequirement requirement)
    {
        var http = context.Resource as DefaultHttpContext;

        if (http == null)
            return;

        var principal = _principalAdapter.ToPrincipal(context.User.Claims);

        var access = Enum.Parse<HttpAccess>(http.Request.Method, true);

        if (await IsSatisfied(requirement, access, principal.Roles))
            context.Succeed(requirement);
    }

    public async Task<bool> IsSatisfied(JwtAuthorizationRequirement requirement, HttpAccess access, IEnumerable<Model> roles)
    {
        var relativeUrl = new RelativeUrl(requirement.Url);

        var granted = _authorizer.IsGranted(relativeUrl.Path, roles, access);

        while (!granted && relativeUrl.HasSegments())
        {
            relativeUrl.RemoveLastSegment();

            granted = _authorizer.IsGranted(relativeUrl.Path, roles, access);
        }

        return await Task.FromResult(granted);
    }
}