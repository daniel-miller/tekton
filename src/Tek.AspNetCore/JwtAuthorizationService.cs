using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Tek.AspNetCore
{
    public class JwtAuthorizationService : AuthorizationHandler<JwtAuthorizationRequirement>
    {
        private readonly PermissionSet _authorizer;

        private readonly IPrincipalAdapter _principalAdapter;

        public JwtAuthorizationService(PermissionSet authorizer, IPrincipalAdapter principalAdapter)
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
}
