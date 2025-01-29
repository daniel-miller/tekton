using System.Collections.Generic;

namespace Tek.Contract
{
    public interface IPrincipal : System.Security.Principal.IIdentity, System.Security.Principal.IPrincipal
    {
        Actor User { get; set; }

        Proxy Proxy { get; set; }

        string IPAddress { get; set; }
        
        Model Organization { get; set; }

        Model Partition { get; set; }

        List<Role> Roles { get; set; }

        IJwt Claims { get; set; }
    }
}