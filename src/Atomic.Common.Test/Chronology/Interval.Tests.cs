namespace Atomic.Common.Test;

public class IntervalTests
{
    private DateTime January1 { get; set; } // Note: Jan 1, 2025 is a Wednesday
    private DateTime January2 { get; set; } // Note: Jan 2, 2025 is a Thursday
    private DateTime January8 { get; set; } // Note: Jan 8, 2025 is a Wednesday
    private DateTime January9 { get; set; } // Note: Jan 9, 2025 is a Wednesday

    private Interval OneTime { get; set; }

    private Interval RecurringTWR { get; set; } // Recurring every Tue, Wed, Thu

    public IntervalTests()
    {
        January1 = new DateTime(2025, 1, 1, 0, 0, 0);
        January2 = new DateTime(2025, 1, 2, 0, 0, 0);
        January8 = new DateTime(2025, 1, 8, 0, 0, 0);
        January9 = new DateTime(2025, 1, 9, 0, 0, 0);

        OneTime = new Interval(January1, "Mountain Standard Time", "30m");

        RecurringTWR = new Interval(January1, "Mountain Standard Time", "30m");
        RecurringTWR.Recurrences.Add("Tue");
        RecurringTWR.Recurrences.Add("Wed");
        RecurringTWR.Recurrences.Add("Thu");
    }

    [Fact]
    public void Constructor_Default()
    {
        var i = new Interval();

        Assert.Equal(i.Effective, Clock.NextCentury);

        Assert.Equal("UTC", i.Zone);
        
        Assert.Equal(60, i.DurationAsTimeSpan().TotalMinutes);

        Assert.True(i.IsValid());
        
        Assert.False(i.IsRecurring());
    }

    [Fact]
    public void Constructor_NoRecurrence()
    {
        var i = OneTime;

        Assert.Equal("2025-01-01", i.Date);
        Assert.Equal("00:00", i.Time);
        Assert.Equal("Mountain Standard Time", i.Zone);
        Assert.Equal(30, i.DurationAsTimeSpan().TotalMinutes);

        Assert.True(i.IsValid());
        Assert.False(i.IsRecurring());
    }

    [Fact]
    public void Contains_PointBeforeInterval_ReturnsFalse()
    {
        var i = OneTime;

        var before = January1.AddSeconds(-1);

        Assert.False(i.Contains(before));
    }

    [Fact]
    public void Contains_PointAfterInterval_ReturnsTrue()
    {
        var i = OneTime;

        var after = January1.AddMinutes(i.DurationAsTimeSpan().TotalMinutes).AddSeconds(1);

        Assert.False(i.Contains(after));
    }

    [Fact]
    public void Contains_PointAtIntervalStart_ReturnsTrue()
    {
        var i = OneTime;

        var start = January1;

        Assert.True(i.Contains(start));
    }

    [Fact]
    public void Contains_PointInsideInterval_ReturnsTrue()
    {
        var i = OneTime;

        var mid = January1.AddMinutes(i.DurationAsTimeSpan().Minutes/2);

        Assert.True(i.Contains(mid));
    }

    [Fact]
    public void Contains_PointAtIntervalEnd_ReturnsFalse()
    {
        var i = OneTime;

        var end = January1.AddMinutes(i.DurationAsTimeSpan().Minutes);

        Assert.False(i.Contains(end));
    }

    [Fact]
    public void Constructor_WithRecurrence()
    {
        var i = RecurringTWR;

        Assert.Equal("2025-01-01", i.Date);
        Assert.Equal("00:00", i.Time);
        Assert.Equal("Mountain Standard Time", i.Zone);
        Assert.Equal(30, i.DurationAsTimeSpan().Minutes);

        Assert.True(i.IsValid());
        Assert.True(i.IsRecurring());

        Assert.False(i.IncludesRecurrence("Sun"));
        Assert.False(i.IncludesRecurrence("Mon"));
        Assert.True(i.IncludesRecurrence("TUE"));
        Assert.True(i.IncludesRecurrence("Wed"));
        Assert.True(i.IncludesRecurrence("thu"));
        Assert.False(i.IncludesRecurrence("Fri"));
        Assert.False(i.IncludesRecurrence("Sat"));
    }

    [Fact]
    public void Constructor_WithInvalidRecurrence_NotValid()
    {
        var i = new Interval(January1, "Mountain Standard Time", "30m");
        i.Recurrences.Add("Tue");
        i.Recurrences.Add("Thu");

        Assert.True(i.IsRecurring());

        Assert.False(i.IncludesRecurrence("Sun"));
        Assert.False(i.IncludesRecurrence("Mon"));
        Assert.True(i.IncludesRecurrence("TUE"));
        Assert.False(i.IncludesRecurrence("Wed"));
        Assert.True(i.IncludesRecurrence("thu"));
        Assert.False(i.IncludesRecurrence("Fri"));
        Assert.False(i.IncludesRecurrence("Sat"));

        Assert.False(i.IsValid());
    }

    [Fact]
    public void EffectiveTime_AllScenarios_TimeOfDayNeverChanges()
    {
        var intervals = new[] { OneTime, RecurringTWR };

        foreach (var i in intervals)
        {
            Assert.Equal(i.Effective.TimeOfDay, i.EffectiveTime(January1.AddSeconds(-1)).TimeOfDay);
            Assert.Equal(i.Effective.TimeOfDay, i.EffectiveTime(January1.AddDays(-1)).TimeOfDay);
            Assert.Equal(i.Effective.TimeOfDay, i.EffectiveTime(January1.AddMonths(-6)).TimeOfDay);

            Assert.Equal(i.Effective.TimeOfDay, i.EffectiveTime(January1).TimeOfDay);

            Assert.Equal(i.Effective.TimeOfDay, i.EffectiveTime(January1.AddSeconds(1)).TimeOfDay);
            Assert.Equal(i.Effective.TimeOfDay, i.EffectiveTime(January1.AddDays(1)).TimeOfDay);
            Assert.Equal(i.Effective.TimeOfDay, i.EffectiveTime(January1.AddMonths(6)).TimeOfDay);
        }
    }

    [Fact]
    public void NextOpenTime_NoRecurrence()
    {
        var i = OneTime;

        var before = January1.AddSeconds(-1);
        var start = January1;
        var inside = January1.AddSeconds(1);
        var end = January1.AddMinutes(i.DurationAsTimeSpan().Minutes);
        var after = end.AddSeconds(1);

        Assert.Equal(i.Effective, i.NextOpenTime(before));
        Assert.Null(i.NextOpenTime(start));
        Assert.Null(i.NextOpenTime(inside));
        Assert.Null(i.NextOpenTime(end));
        Assert.Null(i.NextOpenTime(after));
    }

    [Fact]
    public void NextOpenTime_Recurrence_CurrentWeek()
    {
        var i = RecurringTWR;

        var before = January1.AddSeconds(-1);
        var start = January1;
        var inside = January1.AddSeconds(1);
        var end = January1.AddMinutes(i.DurationAsTimeSpan().Minutes);
        var after = end.AddSeconds(1);

        Assert.Equal(January1, i.NextOpenTime(before));
        Assert.Equal(January2, i.NextOpenTime(start));
        Assert.Equal(January2, i.NextOpenTime(inside));
        Assert.Equal(January2, i.NextOpenTime(end));
        Assert.Equal(January2, i.NextOpenTime(after));
    }

    [Fact]
    public void NextOpenTime_Recurrence_FutureWeek()
    {
        var i = RecurringTWR;

        var before = January8.AddSeconds(-1);
        var start = January8;
        var inside = January8.AddSeconds(1);
        var end = January8.AddMinutes(i.DurationAsTimeSpan().Minutes);
        var after = end.AddSeconds(1);

        Assert.Equal(January8, i.NextOpenTime(before));
        Assert.Equal(January9, i.NextOpenTime(start));
        Assert.Equal(January9, i.NextOpenTime(inside));
        Assert.Equal(January9, i.NextOpenTime(end));
        Assert.Equal(January9, i.NextOpenTime(after));
    }

    [Fact]
    public void NextOpenTime_Recurrence_FutureMonth()
    {
        var i = RecurringTWR;

        // This interval starts Jan 1, 2025 and recurs every Tue Wed Thu. Therefore the interval
        // opens at 00:00 MDT on these dates: Jan 1, 2, 7, 8, 9, 14, 15, 16, ..., Feb 18, 19, 20.

        var February14 = new DateTime(2025, 2, 14, 0, 0, 0); // This is a Friday.
        var February18 = new DateTime(2025, 2, 18, 0, 0, 0);

        Assert.Equal(February18, i.NextOpenTime(February14));
    }
}