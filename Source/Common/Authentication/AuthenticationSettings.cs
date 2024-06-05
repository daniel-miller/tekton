using System;

namespace Common
{
    public class AuthenticationSettings
    {
        public TokenSettings Token { get; set; }
        public RootSettings Root { get; set; }
    }

    public class TokenSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Lifetime { get; set; }
        public string[] Whitelist { get; set; }
    }

    public class RootSettings
    {
        public Guid Identifier { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Secret { get; set; }
    }
}