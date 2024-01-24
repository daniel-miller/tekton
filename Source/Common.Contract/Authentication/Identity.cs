using System;

namespace Common.Contract
{
    public class Identity
    {
        public Actor Actor { get; set; }
        public Actor Impersonator { get; set; }

        public Guid Organization { get; set; }
        public Guid[] Roles { get; set; }

        public DateTimeOffset? Expiry { get; set; }
        public int Lifetime { get; set; }

        public string Language { get; set; }
        public string TimeZone { get; set; }

        public string Phone { get; set; }
        public string IPAddress { get; set; }
    }
}