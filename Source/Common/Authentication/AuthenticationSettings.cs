using System;

namespace Common
{
    public class AuthenticationSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Lifetime { get; set; }
        public string[] Whitelist { get; set; }
    }

    public class ContactSettings
    {
        public RootSettings Root { get; set; }
    }

    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
    }

    public class RootSettings
    {
        public Guid Identifier { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Secret { get; set; }
    }
}