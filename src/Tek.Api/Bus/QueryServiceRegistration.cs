namespace Microsoft.Extensions.DependencyInjection;

public static class QueryServiceRegistration
{
    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(classes => classes.AssignableTo<IQueryRunner>())
            .As<IQueryRunner>()
            .WithScopedLifetime());

        services.AddScoped<QueryDispatcher>();

        services.AddSingleton(x => new QueryTypeCollection(typeof(PrincipalSearch).Assembly));

        return services;
    }
}