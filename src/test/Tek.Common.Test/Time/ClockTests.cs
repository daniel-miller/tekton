namespace Tek.Common.Test;

public class ClockTests
{
    [Fact]
    public void FormatTime_2359_ReturnExpectedFormat()
    {
        var time = new DateTimeOffset(2024, 1, 1, 23, 59, 0, new TimeSpan(-7, 0, 0));

        var formatted = Clock.FormatTime(time, TimeZones.Mountain);

        Assert.Equal("11:59 PM MST", formatted);
    }
}
