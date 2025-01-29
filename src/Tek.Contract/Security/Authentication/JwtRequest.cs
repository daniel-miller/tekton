using System;

namespace Tek.Contract
{
    public class JwtRequest
    {
        public const int DefaultLifetimeLimit = 20; // Minutes

        public bool Debug { get; set; }

        public string Secret { get; set; }

        public int? Lifetime { get; set; }
        
        public Guid? Organization { get; set; }

        public Guid? Agent { get; set; }

        public Guid? Subject { get; set; }
    }
}