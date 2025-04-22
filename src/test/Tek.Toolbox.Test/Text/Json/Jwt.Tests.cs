namespace Tek.Toolbox.Test;

public class JwtTests
{
    private Jwt CreateBasicClaims()
    {
        var dictionary = new Dictionary<string, string>
        {
            { "Name", "John" },
            { "Email", "john@example.com" }
        };

        var claims = new Jwt(dictionary);

        return claims;
    }

    private Jwt CreateAdvancedClaims()
    {
        var dictionary = new Dictionary<string, List<string>>
        {
            { "Name", ["John"] },
            { "Email", ["john@example.com"] },
            { "User_Role", ["Developer", "Administrator"] }
        };

        var claims = new Jwt(dictionary);

        return claims;
    }

    [Fact]
    public void Constructor_NoParameters()
    {
        var claims = new Jwt();

        // The iat and nbf claims are always present in every JWT Claims object.

        Assert.Equal(2, claims.CountClaims());

        var dictionary = claims.ToDictionary();

        Assert.Equal("iat", dictionary.Keys.First());

        Assert.Equal("nbf", dictionary.Keys.Last());
    }

    [Fact]
    public void Constructor_StringDictionary()
    {
        var claims = CreateBasicClaims();

        // Expected claims in alphabetical order = email, iat, name, nbf

        Assert.Equal(2 + 2, claims.CountClaims());

        var keys = claims.ToDictionary().Keys.ToList();

        Assert.Equal("email", keys.First());

        Assert.Equal("iat", keys[1]);

        Assert.Equal("name", keys[2]);

        Assert.Equal("nbf", keys.Last());
    }

    [Fact]
    public void GetClaimValue()
    {
        var claims = CreateBasicClaims();

        Assert.Equal("John", claims.GetClaimValue("name"));

        Assert.Equal("john@example.com", claims.GetClaimValue("email"));
    }

    [Fact]
    public void GetClaimValues()
    {
        var claims = CreateBasicClaims();

        var values = claims.GetClaimValues("name");

        Assert.Single(values);
        
        Assert.Equal("John", values.Single());
    }

    [Fact]
    public void ContainsClaim_CaseInsensitiveKey()
    {
        var claims = CreateBasicClaims();

        Assert.True(claims.ContainsClaim("Name"));
        Assert.True(claims.ContainsClaim("name"));
        Assert.True(claims.ContainsClaim("NAME"));

        Assert.True(claims.ContainsClaim("Email"));
        Assert.True(claims.ContainsClaim("email"));
        Assert.True(claims.ContainsClaim("EMAIL"));
        Assert.True(claims.ContainsClaim("EmaiL"));
    }

    [Fact]
    public void HasClaimValue()
    {
        var claims = CreateBasicClaims();

        Assert.True(claims.HasExpectedClaimValue("name", "John"));

        Assert.True(claims.HasExpectedClaimValue("email", "john@example.com"));

        Assert.False(claims.HasExpectedClaimValue("e-mail", "john@example.com"));
    }

    [Fact]
    public void Constructor_DictionaryOfLists()
    {
        var claims = CreateAdvancedClaims();

        // Expected claims in alphabetical order = email, iat, name, nbf, roles

        // There are two roles, so we should have six claims in total (i.e., there are five distinct
        // keys and last key has two values).

        Assert.Equal(6, claims.CountClaims());

        var keys = claims.ToDictionary().Keys.ToList();

        Assert.Equal("user_role", keys.Last());

        var roles = claims.Roles;

        Assert.Equal(2, roles.Count());
    }

    [Fact]
    public void Sanitize_ValuesAreSortedAutomatically()
    {
        var claims = CreateAdvancedClaims();

        var roles = claims.Roles;

        Assert.Equal("Administrator", roles.First());
        
        Assert.Equal("Developer", roles.Last());
    }
}