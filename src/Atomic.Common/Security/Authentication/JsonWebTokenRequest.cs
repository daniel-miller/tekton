using System;

namespace Atomic.Common
{
    public class JsonWebTokenRequest
    {
        public string Secret { get; set; }
        public int? Lifetime { get; set; }
        public Guid? Organization { get; set; }
    }
}