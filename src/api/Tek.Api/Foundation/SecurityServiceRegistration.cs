using Microsoft.AspNetCore.Authorization;

namespace Microsoft.Extensions.DependencyInjection;

public static class SecurityServiceRegistration
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, SecuritySettings security)
    {
        var scheme = JwtAuthenticationOptions.DefaultScheme;

        services.AddAuthentication(scheme)
            .AddScheme<JwtAuthenticationOptions, JwtAuthenticationHandler>(scheme, null);

        services.AddAuthorization(options =>
        {
            var reflector = new Reflector();

            var relativeUrls = reflector.FindRelativeUrls(typeof(Endpoints));

            foreach (var relativeUrl in relativeUrls.Keys)
            {
                var requirement = new JwtAuthorizationRequirement(relativeUrl, relativeUrls[relativeUrl]);

                options.AddPolicy(relativeUrl, x => x.Requirements.Add(requirement));
            }
        });

        services.AddHttpContextAccessor();

        services.AddSingleton<IAuthorizationHandler, JwtAuthorizationHandler>();

        services.AddSingleton(typeof(Authorizer), provider =>
        {
            var authorizer = new Authorizer(security.Domain);

            if (security.Permissions != null)
            {
                foreach (var bundle in security.Permissions)
                {
                    authorizer.Add(bundle);
                }
            }

            var queryTypes = provider.GetRequiredService<QueryTypeCollection>();

            var queryResources = queryTypes.GetResources();

            authorizer.AddResources(queryResources);

            return authorizer;
        });

        services.AddTransient<IClaimConverter, ClaimConverter>();

        services.AddTransient<IPrincipalSearch, Tek.Service.PrincipalSearch>();

        return services;
    }
}