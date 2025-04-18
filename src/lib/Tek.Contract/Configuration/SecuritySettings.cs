using System.Collections.Generic;

namespace Tek.Contract
{
    public class SecuritySettings
    {
        public string Domain { get; set; }
        public List<PermissionBundle> Permissions { get; set; }
        public string Secret { get; set; }
        public SentinelsSettings Sentinels { get; set; }
        public TokenSettings Token { get; set; }
        public CookieSettings Cookie { get; set; }
    }
}
