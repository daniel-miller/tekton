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

        int CountClaims();

        bool ContainsClaim(string claim);
        string GetClaimValue(string claim);
        List<string> GetClaimValues(string claim);
        bool HasExpectedClaimValue(string claim, string value);
        
        bool IsExpired();
        double GetMinutesUntilExpiry();

        Dictionary<string, List<string>> ToDictionary();
    }
}