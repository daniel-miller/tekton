using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using Tek.Contract;

namespace Tek.Common
{
    public class ClaimConverter : IClaimConverter
    {
        public const string DefaultLanguage = "en";

        public const string DefaultTimeZone = "UTC";

        private readonly SecuritySettings _settings;

        public Guid NamespaceId { get; set; }

        public ClaimConverter(SecuritySettings securitySettings)
        {
            _settings = securitySettings;

            NamespaceId = UuidFactory.CreateV5ForDns(securitySettings.Domain);
        }

        public IEnumerable<Claim> ToClaims(IPrincipal principal)
        {
            var claims = new List<Claim>
            {
                new Claim("user_id", principal.User.Identifier.ToString()),
                new Claim("user_email", principal.User.Email),
                new Claim("user_name", principal.User.Name)
            };

            if (principal.User.Phone.IsNotEmpty())
                claims.Add(new Claim("user_phone", principal.User.Phone));

            if (principal.User.Language != null)
                claims.Add(new Claim("user_language", principal.User.Language));
            else
                claims.Add(new Claim("user_language", DefaultLanguage));

            if (principal.User.TimeZone != null)
                claims.Add(new Claim("user_timezone", principal.User.TimeZone));
            else
                claims.Add(new Claim("user_timezone", DefaultTimeZone));

            var lifetime = principal.Claims.Lifetime;
            if (lifetime != null)
                claims.Add(new Claim("ttl", lifetime.Value.ToString()));

            if (principal.IPAddress.IsNotEmpty())
                claims.Add(new Claim("user_ip", principal.IPAddress));

            if (principal.Organization != null)
                claims.Add(new Claim("organization", principal.Organization.Identifier.ToString()));

            if (principal.Proxy?.Agent != null && principal.Proxy?.Subject != null)
            {
                claims.Add(new Claim("proxy_agent", principal.Proxy.Agent.Identifier.ToString()));
                claims.Add(new Claim("proxy_subject", principal.Proxy.Subject.Identifier.ToString()));
            }

            if (principal.Roles != null)
            {
                foreach (var role in principal.Roles)
                {
                    var value = role.Identifier != Guid.Empty
                        ? role.Identifier.ToString()
                        : role.Name;

                    claims.Add(new Claim("user_role", value));
                }
            }

            return claims.ToArray();
        }

        public ClaimsIdentity ToClaimsIdentity(IJwt claims, string authenticationType)
        {
            var list = new List<Claim>();

            var dictionary = claims.ToDictionary();

            foreach (var key in dictionary.Keys)
            {
                var values = dictionary[key];

                foreach (var value in values)
                {
                    list.Add(new Claim(key, value));
                }
            }

            return new ClaimsIdentity(list, authenticationType);
        }

        public Dictionary<string, List<string>> ToDictionary(IEnumerable<Claim> claims)
        {
            var dictionary = claims
                .GroupBy(claim => claim.Type)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(claim => claim.Value).ToList()
                );

            return dictionary;
        }

        public IPrincipal ToPrincipal(IJwt jwt)
        {
            var list = new List<Claim>();

            var dictionary = jwt.ToDictionary();

            foreach (var key in dictionary.Keys)
            {
                var values = dictionary[key];
                foreach (var value in values)
                    list.Add(new Claim(key, value));
            }

            return ToPrincipal(list);
        }

        public IPrincipal ToPrincipal(Dictionary<string, string> claims)
        {
            return ToPrincipal(claims.Select(x => new Claim(x.Key, x.Value)));
        }

        public IPrincipal ToPrincipal(IEnumerable<Claim> claims)
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
                Roles = GetRoles("user_role"),
                IPAddress = GetClaim("user_ip"),
            };

            principal.Proxy = GetProxy("proxy_agent", "proxy_subject");

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

            List<Role> GetRoles(string type)
            {
                var list = new List<Role>();

                var roleClaims = claims.Where(x => type == x.Type);

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

            Proxy GetProxy(string typeAgent, string typeSubject)
            {
                var agent = GetClaimAsGuid(typeAgent);
                var subject = GetClaimAsGuid(typeSubject);
                if (agent != Guid.Empty && subject != Guid.Empty)
                    return new Proxy(agent, subject);
                return null;
            }
        }

        public IPrincipal ToSentinel(JwtRequest request)
        {
            if (_settings.Sentinels == null)
                return null;

            var sentinel = _settings.Sentinels.ToArray().FirstOrDefault(s => s?.Secret == request.Secret);

            if (sentinel == null)
                return null;

            var principal = new Principal();

            var actor = sentinel;

            principal.User = new Actor
            {
                Email = actor.Email,
                Name = actor.Name,
                Language = actor.Language ?? DefaultLanguage,
                TimeZone = actor.TimeZone ?? DefaultTimeZone
            };

            if (actor.Identifier != Guid.Empty)
                principal.User.Identifier = actor.Identifier;
            else
                principal.User.Identifier = UuidFactory.CreateV5(NamespaceId, actor.Email);

            if (request.Organization.HasValue)
                principal.Organization = new Model { Identifier = request.Organization.Value };

            // The meaning of a request that explicitly specifies both a proxy agent and a proxy
            // subject is ambiguous, and therefore disallowed, because the security implications are
            // significant. For example, both these statements cannot be true at the same time in
            // the same context:
            //   * Alice (Proxy Agent)   acts on behalf of  John (Proxy Subject).
            //   * Alice (Proxy Subject) is impersonated by John (Proxy Agent).
            
            if (request.Agent.HasValue && request.Subject.HasValue)
                throw new ArgumentException($"This is an invalid JWT request because it specifies a Proxy Agent {request.Agent} and a Proxy Subject {request.Subject}. A JWT request is permitted to specify one or the other (or neither), but not both.");
            
            else if (request.Agent.HasValue)
                principal.Proxy = new Proxy
                {
                    Agent = new Actor { Identifier = request.Agent.Value },
                    Subject = new Actor { Identifier = principal.User.Identifier }
                }; 
            
            else if (request.Subject.HasValue)
                principal.Proxy = new Proxy
                {
                    Agent = new Actor { Identifier = principal.User.Identifier },
                    Subject = new Actor { Identifier = request.Subject.Value }
                };

            if (sentinel.Roles != null && sentinel.Roles.Length > 0)
                principal.Roles = sentinel.Roles.Select(CreateRole).ToList();

            principal.Roles.Add(CreateRole(actor.Email));

            return principal;
        }

        public int CalculateLifetime(int? assigned, int? requested, int? @default)
        {
            // If the token has a lifetime already assigned to it, and if a lifetime is explicitly
            // requested, then use the smaller of the two values. Otherwise, use the default.

            if (requested.HasValue && requested.Value > 0)
            {
                if (assigned.HasValue && assigned.Value > 0)
                    return Math.Min(assigned.Value, requested.Value);

                if (@default.HasValue && @default.Value > 0 && requested < @default)
                    return requested.Value;
            }

            return @default ?? JwtRequest.DefaultLifetimeLimit;
        }

        private Role CreateRole(string name)
        {
            var role = new Role
            {
                Identifier = UuidFactory.CreateV5(NamespaceId, name),
                Name = name
            };

            return role;
        }
    }
}