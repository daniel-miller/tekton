using System;
using System.Security.Principal;

namespace Tek.Common
{
    public class Principal: IIdentity, IPrincipal
    {
        public Actor Actor { get; set; }
        public Actor Impersonator { get; set; }

        public Model Enterprise { get; set; }
        public Model Organization { get; set; }
        public Model[] Roles { get; set; }

        public JsonWebToken Token { get; set; }

        public string Language { get; set; }
        public string TimeZone { get; set; }

        public string Phone { get; set; }
        public string IPAddress { get; set; }

        public Principal()
        {
            Token = new JsonWebToken();
        }

        #region IIdentity and IPrincipal

        public string AuthenticationType { get; set; }

        IIdentity IPrincipal.Identity => this;

        public bool IsAuthenticated { get; set; }
        
        public string Name => Actor.Email;
        
        public bool IsInRole(string role)
        {
            if (Roles == null || Roles.Length == 0)
                return false;

            return Array.Exists(Roles, r => r.Name == role);
        }

        #endregion
    }
}