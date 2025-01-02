namespace Atomic.Common.Test;

public class FilterTests
{
    [Fact]
    public void Contructor_DefaultPage_Returns1()
    {
        var filter = new Filter();
        Assert.Equal(1, filter.Page);
    }

    [Fact]
    public void Contructor_DefaultTake_Returns20()
    {
        var filter = new Filter();
        Assert.Equal(20, filter.Take);
    }
}