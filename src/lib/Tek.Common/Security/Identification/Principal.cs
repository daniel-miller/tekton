﻿using System.Collections.Generic;
using System.Linq;

using Tek.Contract;

namespace Tek.Common
{
    public class Principal : IPrincipal
    {
        public Actor User { get; set; }
        public Proxy Proxy { get; set; }

        public string IPAddress { get; set; }

        public Model Organization { get; set; }
        public Model Partition { get; set; }
        public List<Role> Roles { get; set; }

        public IJwt Claims { get; set; }

        public Principal()
        {
            Claims = new Jwt();
            Roles = new List<Role>();
        }

        #region IIdentity and IPrincipal

        public string AuthenticationType { get; set; }

        System.Security.Principal.IIdentity System.Security.Principal.IPrincipal.Identity => this;

        public bool IsAuthenticated { get; set; }

        public string Name => User.Email;

        public bool IsInRole(string role)
        {
            if (Roles == null || Roles.Count == 0)
                return false;

            var names = Roles.Select(x => x.Name).ToArray();

            if (role.MatchesAny(names))
                return true;

            var identifiers = Roles.Select(x => x.Identifier.ToString()).ToArray();

            if (role.MatchesAny(identifiers))
                return true;

            return false;
        }

        #endregion
    }
}