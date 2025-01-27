using System;
using System.Collections.Generic;
using System.Linq;

using SecurityClaim = System.Security.Claims.Claim;

using Tek.Contract;

namespace Tek.Common
{
    public class PrincipalAdapter : IPrincipalAdapter
    {
        public const string DefaultLanguage = "en";

        public const string DefaultTimeZone = "UTC";

        public IEnumerable<SecurityClaim> ToClaims(IPrincipal principal, string ipAddress)
        {
            var claims = new List<SecurityClaim>
            {
                new SecurityClaim("user_id", principal.User.Identifier.ToString()),
                new SecurityClaim("user_email", principal.User.Email),
                new SecurityClaim("user_name", principal.User.Name)
            };

            if (principal.User.Phone.IsNotEmpty())
                claims.Add(new SecurityClaim("user_phone", principal.User.Phone));

            if (principal.User.Language != null)
                claims.Add(new SecurityClaim("user_language", principal.User.Language));
            else
                claims.Add(new SecurityClaim("user_language", DefaultLanguage));

            if (principal.User.TimeZone != null)
                claims.Add(new SecurityClaim("user_timezone", principal.User.TimeZone));
            else
                claims.Add(new SecurityClaim("user_timezone", DefaultTimeZone));

            var lifetime = principal.Claims.Lifetime;
            if (lifetime != null)
                claims.Add(new SecurityClaim("ttl", lifetime.Value.ToString()));

            if (ipAddress.IsNotEmpty())
                claims.Add(new SecurityClaim("user_ip", ipAddress));

            if (principal.Organization != null)
                claims.Add(new SecurityClaim("organization", principal.Organization.Identifier.ToString()));

            if (principal.Roles != null)
            {
                foreach (var role in principal.Roles)
                {
                    var value = role.Identifier != Guid.Empty
                        ? role.Identifier.ToString()
                        : role.Name;

                    claims.Add(new SecurityClaim("user_role", value));
                }
            }

            return claims.ToArray();
        }

        public Dictionary<string, List<string>> ToDictionary(IEnumerable<SecurityClaim> claims)
        {
            var dictionary = claims
                .GroupBy(claim => claim.Type)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(claim => claim.Value).ToList()
                );

            return dictionary;
        }

        public IPrincipal ToPrincipal(Dictionary<string, string> claims)
        {
            return ToPrincipal(claims.Select(x => new SecurityClaim(x.Key, x.Value)));
        }

        public IPrincipal ToPrincipal(IJwt jwt)
        {
            var list = new List<SecurityClaim>();

            var dictionary = jwt.ToDictionary();

            foreach (var key in dictionary.Keys)
            {
                var values = dictionary[key];
                foreach (var value in values)
                    list.Add(new SecurityClaim(key, value));
            }

            return ToPrincipal(list);
        }

        public IPrincipal ToPrincipal(IEnumerable<SecurityClaim> claims)
        {
            var principal = new Principal
            {
                User = new Actor
                {
                    Identifier = GetClaimAsGuid("user_id"),
                    Email = GetClaim("user_email"),
                    Name = GetClaim("user_name"),
                    Phone = GetClaim("user_phone"),
                    Language = GetClaim("user_language"),
                    TimeZone = GetClaim("user_timezone")
                },
                Organization = new Model { Identifier = GetClaimAsGuid("organization") },
                Roles = GetRoles(),
                IPAddress = GetClaim("user_ip"),
            };

            principal.Claims.Lifetime = GetClaimAsInt("ttl");

            return principal;

            string GetClaim(string type)
            {
                var claim = claims.FirstOrDefault(x => type == x.Type || x.Properties.Any(p => p.Value == type));
                if (claim != null)
                    return claim.Value;

                return null;
            }

            Guid GetClaimAsGuid(string type)
            {
                var claim = claims.FirstOrDefault(x => type == x.Type || x.Properties.Any(p => p.Value == type));
                if (claim != null)
                    return Guid.Parse(claim.Value);
                return Guid.Empty;
            }

            int GetClaimAsInt(string type)
            {
                var claim = claims.FirstOrDefault(x => type == x.Type || x.Properties.Any(p => p.Value == type));
                if (claim != null)
                    return int.Parse(claim.Value);
                return 0;
            }

            List<Role> GetRoles()
            {
                var list = new List<Role>();

                var roleClaims = claims.Where(x => "user_role" == x.Type);

                foreach (var roleClaim in roleClaims)
                {
                    var value = roleClaim.Value;

                    if (Guid.TryParse(value, out var id))
                        list.Add(new Role(id));
                    else
                        throw new ArgumentException($"{value} is not a unique identifier. UUID values are required for user_role security claims.");
                }

                return list;
            }
        }

        public IPrincipal ToPrincipal(IPrincipal principal, string ipAddress, int? requestedTokenLifetime, int? defaultTokenLifetime)
        {
            principal.IPAddress = ipAddress;

            principal.Claims.Lifetime = CalculateTokenLifetime(principal.Claims.Lifetime, requestedTokenLifetime, defaultTokenLifetime);

            return principal;
        }

        private static int CalculateTokenLifetime(int? assigned, int? requested, int? @default)
        {
            // If the token has a lifetime already assigned to it, and if a lifetime is explicitly requested, then use
            // the smaller of the two values. Otherwise, use the default lifetime.

            if (requested.HasValue && requested.Value > 0)
            {
                if (assigned.HasValue && assigned.Value > 0)
                    return Math.Min(requested.Value, assigned.Value);

                if (@default.HasValue && @default.Value > 0 && requested < @default)
                    return requested.Value;
            }

            return @default ?? JwtRequest.DefaultLifetimeLimit;
        }
    }
}