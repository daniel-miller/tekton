using Newtonsoft.Json;

namespace Tek.Base.Test;

public class PermissionSetTests
{
    [Fact]
    public void PermissionSet_AddBundle_ConfiguresMultiplePermissions()
    {
        // Arrange

        var json = "{\"Type\":\"Data\",\"Access\":\"Read, Write, Delete\",\"Resources\":[\"A\",\"B\",\"C\"],\"Roles\":[\"X\",\"Y\",\"Z\"]}";

        var bundle = JsonConvert.DeserializeObject<PermissionBundle>(json);

        var domain = "example.com";

        var permissions = new Authorizer(domain);

        // Act

        var items = permissions.Add(bundle);

        // Assert

        // The set should have a deterministic UUID calculated from the domain.
        Assert.Equal(UuidFactory.CreateV5ForDns(domain), permissions.NamespaceId);

        // The set should contain 9 permissions: one for each Resource/Role combination.
        Assert.Equal(9, items.Count); //  AX, AY, AZ, BX, BY, BZ, CX, CY, CZ

        // The sort order for items in the set is non-deterministic, so we should not rely on any
        // specific item's location within the set. However, all permutations should exist.
        for (char resource = 'A'; resource <= 'C'; resource++)
            for (char role = 'X'; role <= 'Z'; role++)
                Assert.True(permissions.Contains(resource.ToString(), role.ToString()));
        
        foreach (var item in items)
        {
            // Every permission should have the same access.
            Assert.Equal(AccessType.Data, item.Access.Type);
            Assert.Equal(DataAccess.Read | DataAccess.Write | DataAccess.Delete, item.Access.Data);

            // Every resource should have a deterministic v5 UUID calculated from the domain and resource name.
            Assert.Equal(UuidFactory.CreateV5(permissions.NamespaceId, item.Resource.Name), item.Resource.Identifier);

            // Every role should have a deterministic v5 UUID calculated from the domain and role name.
            Assert.Equal(UuidFactory.CreateV5(permissions.NamespaceId, item.Role.Name), item.Role.Identifier);
        }
    }

    [Fact]
    public void PermissionSet_AddRoleWithV7Uuid_IdentifierIsNotOverwrittenWithV5()
    {
        // Arrange

        var domain = "example.com";

        var permissions = new Authorizer(domain);

        var resource = new Resource
        {
            Identifier = UuidFactory.CreateV7(),
            Name = "api/contact/groups"
        };

        var role = new Role
        {
            Identifier = UuidFactory.CreateV7(),
            Name = "Developer"
        };

        // Act

        var item = permissions.Add(resource, role);

        // Assert

        // The permission set does not override the identifier already assigned to the role. It
        // assigns a UUID (v5) only if the role added to the permission set does not already have
        // a unique identifier.
        Assert.Equal(item.Role.Identifier, role.Identifier);
        Assert.NotEqual(UuidFactory.CreateV5(permissions.NamespaceId, item.Role.Name), item.Role.Identifier);
    }
}