using System;
using System.Collections.Generic;
using System.Linq;

using SecurityClaim = System.Security.Claims.Claim;

namespace Atomic.Common
{
    public class PrincipalAdapter
    {
        public IEnumerable<SecurityClaim> ToClaims(Principal principal, string ipAddress)
        {
            var claims = new List<SecurityClaim>
            {
                new SecurityClaim("user", principal.Actor.Identifier.ToString()),
                new SecurityClaim("email", principal.Actor.Email),
                new SecurityClaim("name", principal.Actor.Name)
            };

            if (!string.IsNullOrWhiteSpace(principal.Actor.Phone))
                claims.Add(new SecurityClaim("phone", principal.Actor.Phone));

            claims.Add(new SecurityClaim("language", principal.Language));
            claims.Add(new SecurityClaim("timezone", principal.TimeZone));
            claims.Add(new SecurityClaim("lifetime", principal.Token.Lifetime.ToString()));

            if (!string.IsNullOrWhiteSpace(ipAddress))
                claims.Add(new SecurityClaim("ip", ipAddress));

            if (principal.Organization != null)
                claims.Add(new SecurityClaim("organization", principal.Organization.Identifier.ToString()));

            if (principal.Roles != null && principal.Roles.Length > 0)
                claims.Add(new SecurityClaim("roles", string.Join(",", principal.Roles.Select(x => x.Identifier.ToString()))));

            return claims.ToArray();
        }

        public Principal ToPrincipal(Dictionary<string, string> claims)
        {
            return ToPrincipal(claims.Select(x => new SecurityClaim(x.Key, x.Value)));
        }

        public Principal ToPrincipal(IEnumerable<SecurityClaim> claims)
        {
            var model = new Principal
            {
                Actor = new Actor
                {
                    Identifier = GetClaimAsGuid("user"),
                    Email = GetClaim("email"),
                    Name = GetClaim("name"),
                    Phone = GetClaim("phone")
                },
                Roles = GetClaimAsModelArray("roles"),
                Organization = GetClaimAsModel("organization"),
                Language = GetClaim("language"),
                TimeZone = GetClaim("timezone"),
                IPAddress = GetClaim("ip"),
            };

            model.Token.Lifetime = GetClaimAsInt("lifetime");

            return model;

            string GetClaim(string type)
            {
                var claim = claims.FirstOrDefault(x => type == x.Type || x.Properties.Any(p => p.Value == type));
                if (claim != null)
                    return claim.Value;

                return string.Empty;
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

            Model GetClaimAsModel(string type)
            {
                var claim = claims.FirstOrDefault(x => type == x.Type || x.Properties.Any(p => p.Value == type));
                if (claim != null)
                    return new Model { Identifier = Guid.Parse(claim.Value) };
                return new Model { Identifier = Guid.Empty };
            }

            Model[] GetClaimAsModelArray(string type)
            {
                var claim = claims.FirstOrDefault(x => type == x.Type);
                if (claim != null)
                    return claim.Value.Split(',').Select(x => new Model { Identifier = Guid.Parse(x) }).ToArray();
                return new Model[0];
            }
        }

        public Principal ToPrincipal(Principal principal, string ipAddress, int? requestedTokenLifetime, int? defaultTokenLifetime)
        {
            principal.IPAddress = ipAddress;

            principal.Token.Lifetime = CalculateTokenLifetime(principal.Token.Lifetime, requestedTokenLifetime, defaultTokenLifetime);

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

            return @default ?? JsonWebToken.DefaultLifetimeLimit;
        }
    }
}
