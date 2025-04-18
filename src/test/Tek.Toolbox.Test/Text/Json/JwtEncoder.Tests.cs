using Tek.Toolbox;

namespace Tek.Common.Test;

public class JwtEncoderTests
{
    private Jwt CreateAdvancedClaims()
    {
        var dictionary = new Dictionary<string, List<string>>
        {
            { "Name", ["Alice"] },
            { "Email", ["alice@example.com"] },
            { "Roles", ["Developer", "Operator", "Administrator", "Learner", "Instructor"] }
        };

        var claims = new Jwt(dictionary);

        return claims;
    }

    [Fact]
    public void Constructor_DictionaryOfLists()
    {
        var original = CreateAdvancedClaims();

        Assert.Equal(2 + 7, original.CountClaims());

        var encoder = new JwtEncoder();

        var token = encoder.Encode(original, "password.123");

        var decoded = encoder.Decode(token);

        Assert.Equal(original.CountClaims(), decoded.CountClaims());
        Assert.Equal(original.GetClaimValue("Name"), decoded.GetClaimValue("Name"));
        Assert.Equal(original.GetClaimValue("Email"), decoded.GetClaimValue("Email"));
    }
}