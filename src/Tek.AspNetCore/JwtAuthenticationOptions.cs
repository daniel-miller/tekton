using Microsoft.AspNetCore.Authentication;

namespace Tek.AspNetCore
{
    public class JwtAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "Bearer";
        public static string Scheme => DefaultScheme;
        public static string AuthenticationType => DefaultScheme;
    }
}