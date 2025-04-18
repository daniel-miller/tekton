using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microsoft.Extensions.DependencyInjection;

public static class SwaggerExtensions
{
    public static void AddDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen(options =>
        {
            options.DocumentFilter<PathLowercaseDocumentFilter>();

            // By Default, all endpoints are grouped by the controller name. We want to group first
            // by "Group Name", then by controller name (if no group is provided).
            options.TagActionsBy((api) => [api.GroupName ?? api.ActionDescriptor.RouteValues["controller"]]);

            // Include all endpoints available in the documentation.
            options.DocInclusionPredicate((docName, apiDesc) => { return true; });

            // Include XML comments from source code.
            var xmlCommentsFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
            options.IncludeXmlComments(xmlCommentsPath);
        });
    }

    public static void UseDocumentation(this WebApplication host)
    {
        host.UseSwagger();

        host.UseSwaggerUI(options =>
        {
            options.DefaultModelsExpandDepth(-1);
        });
    }
}

public class PathLowercaseDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument document, DocumentFilterContext context)
    {
        var oldPaths = document.Paths.ToDictionary(x => ToLowercase(x.Key), x => x.Value);

        var newPaths = new OpenApiPaths();

        foreach (var path in oldPaths)
        {
            newPaths.Add(path.Key, path.Value);
        }

        document.Paths = newPaths;
    }

    private static string ToLowercase(string key)
    {
        var parts = key.Split('/')
            .Select(part => part.Contains("}") ? part : part.ToLower());

        return string.Join('/', parts);
    }
}