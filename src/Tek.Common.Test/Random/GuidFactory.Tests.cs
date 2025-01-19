namespace Tek.Common.Test;

public class GuidFactoryTests
{
    [Fact]
    public void Create_ConfirmUniqueness()
    {
        var list = Enumerable.Range(0, 100)
            .Select(i => GuidFactory.Create())
            .ToList();

        var uniqueCount = list.GroupBy(i => i).Count();

        Assert.Equal(100, uniqueCount);
    }
}