using Microsoft.AspNetCore.Authorization;

namespace Tek.Api;

public class JwtAuthorizationRequirement : IAuthorizationRequirement
{
    public string Url { get; set; }
    public string Source { get; set; }

    public JwtAuthorizationRequirement(string url, string source)
    {
        Url = url;
        Source = source;
    }
}