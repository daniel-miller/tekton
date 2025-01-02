namespace Atomic.Common.Test;

public class FilterTests
{
    [Fact]
    public void Contructor_DefaultPage_Returns1()
    {
        var criteria = new Filter();
        Assert.Equal(1, criteria.Page);
    }

    [Fact]
    public void Contructor_DefaultTake_Returns20()
    {
        var criteria = new Filter();
        Assert.Equal(20, criteria.Take);
    }
}