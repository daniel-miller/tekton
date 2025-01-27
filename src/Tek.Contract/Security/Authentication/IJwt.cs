using System;
using System.Collections.Generic;

namespace Tek.Contract
{
    /// <summary>
    /// Defines the interface for a decoded JSON web token.
    /// </summary>
    public interface IJwt
    {
        string Audience { get; set; }
        string Issuer { get; set; }
        string Subject { get; set; }
        List<string> Roles { get; set; }
        DateTimeOffset? Expiry { get; set; }
        int? Lifetime { get; set; }

        int Count();

        bool Contains(string claim);
        string GetValue(string claim);
        List<string> GetValues(string claim);
        bool HasExpectedValue(string claim, string value);
        Dictionary<string, List<string>> ToDictionary();

        bool IsExpired();
        double GetMinutesUntilExpiry();
    }
}