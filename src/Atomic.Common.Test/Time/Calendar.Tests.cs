namespace Atomic.Common.Test;

public class CalendarTests
{
    [Fact]
    public void GetSeason_JulyFirst_ReturnsSummer()
    {
        var calendar = new Calendar();
        Assert.Equal(Season.Summer, calendar.GetSeason(new DateTimeOffset(2024, 7, 1, 0, 0, 0, TimeSpan.Zero)));
    }
}