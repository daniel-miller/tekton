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

    public class AuthenticationModel
    {
        public string TokenSecret { get; set; }
        public DateTimeOffset? TokenExpiry { get; set; }
        public int? TokenLifetimeLimit { get; set; }

        public string UserEmail { get; set; }
        public string UserLanguage { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string UserTimeZone { get; set; }

        public Guid OrganizationIdentifier { get; set; }
        public Guid UserIdentifier { get; set; }
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