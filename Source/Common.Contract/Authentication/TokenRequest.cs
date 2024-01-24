using System;

namespace Common.Contract
{
    public class TokenRequest
    {
        public string Secret { get; set; }
        public int? Lifetime { get; set; }
        public Guid? Organization { get; set; }
    }
}