using System.Collections.Generic;

namespace Tek.Contract
{
    public class PermissionBundle
    {
        public string Type { get; set; }
        public string Access { get; set; }
        public List<string> Resources { get; set; }
        public List<string> Roles { get; set; }

        public PermissionBundle()
        {
            Resources = new List<string>();
            Roles = new List<string>();
        }
    }
}
