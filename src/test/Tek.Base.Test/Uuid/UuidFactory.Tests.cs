namespace Tek.Base.Test;

public class UuidFactoryTests
{
    [Fact]
    public void CreateV3_ShouldGenerateDeterministicValue()
    {
        // Arrange
        string name = "example.com";

        // Act
        Guid uuid1 = UuidFactory.CreateV3(PredefinedUuids.DNS, name);
        Guid uuid2 = UuidFactory.CreateV3(PredefinedUuids.DNS, name);

        // Assert
        Assert.Equal(uuid1, uuid2); // UUIDs should match for the same input
    }

    [Fact]
    public void CreateV3_ShouldGenerateDifferentValueForDifferentName()
    {
        // Arrange
        string name1 = "example.com";
        string name2 = "test.com";

        // Act
        Guid uuid1 = UuidFactory.CreateV3(PredefinedUuids.DNS, name1);
        Guid uuid2 = UuidFactory.CreateV3(PredefinedUuids.DNS, name2);

        // Assert
        Assert.NotEqual(uuid1, uuid2); // UUIDs should differ for different names
    }

    [Fact]
    public void CreateV3_ShouldGenerateValueWithCorrectVersion()
    {
        // Arrange
        string name = "example.com";

        // Act
        Guid uuid = UuidFactory.CreateV3(PredefinedUuids.DNS, name);

        // Assert
        byte version = (byte)((uuid.ToByteArray()[7] >> 4) & 0xF);
        Assert.Equal(3, version); // Ensure version is 3
    }

    [Fact]
    public void CreateV3_ShouldGenerateValueWithCorrectVariant()
    {
        // Arrange
        string name = "example.com";

        // Act
        Guid uuid = UuidFactory.CreateV3(PredefinedUuids.DNS, name);

        // Assert
        byte variant = (byte)((uuid.ToByteArray()[8] & 0xC0) >> 6);
        Assert.Equal(0b10, variant); // Ensure variant is RFC 4122
    }

    [Fact]
    public void CreateV5_ShouldGenerateDeterministicValue()
    {
        // Arrange
        
        string example = "example.com";
        string shift = "shiftiq.com";

        // Act

        Guid uuid1 = UuidFactory.CreateV5(PredefinedUuids.DNS, example);
        Guid uuid2 = UuidFactory.CreateV5(PredefinedUuids.DNS, example);
        Guid uuid3 = UuidFactory.CreateV5(PredefinedUuids.DNS, shift);

        // Assert

        Assert.Equal(uuid1, uuid2); // UUIDs should match for the same input
        
        // An external third-party tool (https://www.uuidtools.com/v5) was used to generate the 
        // expected v5 UUID value for the shiftiq.com domain name.

        Assert.Equal(Guid.Parse("2bc0562e-b3bf-5fb7-b7bc-85c2b96d8459"), uuid3);
    }

    [Fact]
    public void CreateV5_ShouldGenerateDifferentValueForDifferentName()
    {
        // Arrange
        string name1 = "example.com";
        string name2 = "test.com";

        // Act
        Guid uuid1 = UuidFactory.CreateV5(PredefinedUuids.DNS, name1);
        Guid uuid2 = UuidFactory.CreateV5(PredefinedUuids.DNS, name2);

        // Assert
        Assert.NotEqual(uuid1, uuid2); // UUIDs should differ for different names
    }

    [Fact]
    public void CreateV5_ShouldGenerateValueWithCorrectVersion()
    {
        // Arrange
        string name = "example.com";

        // Act
        Guid uuid = UuidFactory.CreateV5(PredefinedUuids.DNS, name);

        // Assert
        byte version = (byte)((uuid.ToByteArray()[7] >> 4) & 0xF);
        Assert.Equal(5, version); // Ensure version is 5
    }

    [Fact]
    public void CreateV5_ShouldGenerateValueWithCorrectVariant()
    {
        // Arrange
        string name = "example.com";

        // Act
        Guid uuid = UuidFactory.CreateV5(PredefinedUuids.DNS, name);

        // Assert
        byte variant = (byte)((uuid.ToByteArray()[8] & 0xC0) >> 6);
        Assert.Equal(0b10, variant); // Ensure the variant is RFC 4122
    }

    [Fact]
    public void CreateV7_ShouldGenerateUniqueValue()
    {
        var list = Enumerable.Range(0, 100)
            .Select(i => UuidFactory.Create())
            .ToList();

        var uniqueCount = list.GroupBy(i => i).Count();

        Assert.Equal(100, uniqueCount);
    }
}