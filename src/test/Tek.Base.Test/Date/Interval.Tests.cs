namespace Tek.Base.Test;

public class IntervalTests
{
    private DateTimeOffset January1 { get; set; } // Note: Jan 1, 2025 is a Wednesday
    private DateTimeOffset January2 { get; set; } // Note: Jan 2, 2025 is a Thursday
    private DateTimeOffset January8 { get; set; } // Note: Jan 8, 2025 is a Wednesday
    private DateTimeOffset January9 { get; set; } // Note: Jan 9, 2025 is a Wednesday

    private Interval OneTime { get; set; }

    private Interval RecurringTWR { get; set; } // Recurring every Tue, Wed, Thu

    public IntervalTests()
    {
        January1 = CreateDto(new DateTime(2025, 1, 1, 0, 0, 0), -7);
        January2 = CreateDto(new DateTime(2025, 1, 2, 0, 0, 0), -7);
        January8 = CreateDto(new DateTime(2025, 1, 8, 0, 0, 0), -7);
        January9 = CreateDto(new DateTime(2025, 1, 9, 0, 0, 0), -7);

        OneTime = new Interval(January1.DateTime, TimeZones.Mountain.Id, "30m");

        RecurringTWR = new Interval(January1.DateTime, TimeZones.Mountain.Id, "30m");
        RecurringTWR.Recurrences.Add("Tue");
        RecurringTWR.Recurrences.Add("Wed");
        RecurringTWR.Recurrences.Add("Thu");
    }

    private DateTimeOffset CreateUtc(DateTime dt)
    {
        return new DateTimeOffset(dt, TimeSpan.Zero);
    }

    private DateTimeOffset CreateDto(DateTime dt, int offset)
    {
        return new DateTimeOffset(dt, new TimeSpan(offset,0,0));
    }

    [Fact]
    public void Constructor_Default()
    {
        var i = new Interval();

        Assert.Equal(i.GetEffective(), Clock.NextCentury);

        Assert.Equal("UTC", i.Zone);
        
        Assert.Equal(60, i.GetDuration().TotalMinutes);

        Assert.True(i.IsValid());
        
        Assert.False(i.IsRecurring());
    }

    [Fact]
    public void Constructor_WithNoRecurrence()
    {
        var i = OneTime;

        Assert.Equal("2025-01-01", i.Date);
        Assert.Equal("00:00", i.Time);
        Assert.Equal("Mountain Standard Time", i.Zone);
        Assert.Equal(30, i.GetDuration().TotalMinutes);

        Assert.True(i.IsValid());
        Assert.False(i.IsRecurring());
    }

    [Fact]
    public void Constructor_WithValidRecurrence()
    {
        var i = RecurringTWR;

        Assert.Equal("2025-01-01", i.Date);
        Assert.Equal("00:00", i.Time);
        Assert.Equal("Mountain Standard Time", i.Zone);
        Assert.Equal(30, i.GetDuration().Minutes);

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
        var i = new Interval(January1, "30m");
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
    public void Contains_PointBeforeInterval_ReturnsFalse()
    {
        var i = OneTime;

        var before = January1.AddSeconds(-1);

        Assert.False(i.Contains(before));
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

        var mid = January1.AddMinutes(i.GetDuration().Minutes/2);

        Assert.True(i.Contains(mid));
    }

    [Fact]
    public void Contains_PointAtIntervalEnd_ReturnsFalse()
    {
        var i = OneTime;

        var end = January1.AddMinutes(i.GetDuration().Minutes);

        Assert.False(i.Contains(end));
    }

    [Fact]
    public void Contains_PointAfterInterval_ReturnsTrue()
    {
        var i = OneTime;

        var after = January1.AddMinutes(i.GetDuration().TotalMinutes).AddSeconds(1);

        Assert.False(i.Contains(after));
    }

    [Fact]
    public void GetStart_AllScenarios_TimeOfDayNeverChanges()
    {
        var intervals = new[] { OneTime, RecurringTWR };

        foreach (var i in intervals)
        {
            var effective = i.GetEffective();

            Assert.Equal(effective.TimeOfDay, i.GetStart(January1.DateTime.AddSeconds(-1)).TimeOfDay);
            Assert.Equal(effective.TimeOfDay, i.GetStart(January1.DateTime.AddDays(-1)).TimeOfDay);
            Assert.Equal(effective.TimeOfDay, i.GetStart(January1.DateTime.AddMonths(-6)).TimeOfDay);

            Assert.Equal(effective.TimeOfDay, i.GetStart(January1.DateTime).TimeOfDay);

            Assert.Equal(effective.TimeOfDay, i.GetStart(January1.DateTime.AddSeconds(1)).TimeOfDay);
            Assert.Equal(effective.TimeOfDay, i.GetStart(January1.DateTime.AddDays(1)).TimeOfDay);
            Assert.Equal(effective.TimeOfDay, i.GetStart(January1.DateTime.AddMonths(6)).TimeOfDay);
        }
    }

    [Fact]
    public void NextOpenTime_NoRecurrence()
    {
        var i = OneTime;

        var before = January1.AddSeconds(-1);
        var start = January1;
        var inside = January1.AddSeconds(1);
        var end = January1.AddMinutes(i.GetDuration().Minutes);
        var after = end.AddSeconds(1);

        var effective = i.GetEffective();

        Assert.Equal(effective, i.NextOpenTime(before));
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
        var end = January1.AddMinutes(i.GetDuration().Minutes);
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
        var end = January8.AddMinutes(i.GetDuration().Minutes);
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

        var February14 = CreateUtc(new DateTime(2025, 2, 14, 0, 0, 0)); // This is a Friday.
        var February18 = CreateUtc(new DateTime(2025, 2, 18, 7, 0, 0));

        Assert.Equal(February18, i.NextOpenTime(February14));
    }

    /// <remarks>
    /// This unit test runs an extensive series of tests using all date/time values between 
    /// Dec 1, 2024 and Feb 1, 2026, checking every 5 minutes to ensure hotfix and maintenance 
    /// windows are always open when expected, and closed when expected. Each test compares the 
    /// output of the Contains method with the output of a hard-coded method here in the test 
    /// fixture, so the results are verified using two different algorithms.
    /// </remarks>
    [Fact]
    public void Contains_AlternateStrategy()
    {
        // The hotfix window is 1 hour every Wednesday.
        var hotfix = new Interval(CreateDto(new DateTime(2025, 1, 1, 20, 0, 0), -7), "1h", "Wed");

        // The maintenance window is 30 minutes every day.
        var maintenance = new Interval(CreateDto(new DateTime(2025, 1, 1, 3, 0, 0), -7), "30m");
        
        var from = new DateTimeOffset(2024, 12, 1, 0, 0, 0, TimeSpan.Zero);
        var thru = new DateTimeOffset(2026, 2, 1, 0, 0, 0, TimeSpan.Zero);

        for (var current = from; current <= thru; current = current.AddMinutes(5))
        {
            Assert.Equal(IsInHotfixWindow(current, hotfix.GetEffective()), hotfix.Contains(current));
            Assert.Equal(IsInMaintenanceWindow(current, maintenance.GetEffective()), maintenance.Contains(current));
        }
    }

    /// <remarks>
    /// The previous unit test identified problems with the code in my original implementation. This
    /// unit test verifies those problems are not reintroduced by any future code changes.
    /// </remarks>
    [Fact]
    public void Contains_SpecialCases()
    {
        // The hotfix window is 1 hour every Wednesday. Notice Jan 1, 2025 is a MST date/time value.
        var hotfix = new Interval(CreateDto(new DateTime(2025, 1, 1, 20, 0, 0), -7), "1h", "Wed");

        // The maintenance window is 30 minutes every day.
        var maintenance = new Interval(CreateDto(new DateTime(2025, 1, 1, 3, 0, 0), -7), "30m");

        // Confirm the windows are valid.
        Assert.True(IsInHotfixWindow(hotfix.GetEffective(), hotfix.GetEffective()));
        Assert.True(IsInMaintenanceWindow(maintenance.GetEffective(), maintenance.GetEffective()));

        // Confirm the windows do not overlap.
        Assert.False(hotfix.Contains(maintenance.GetEffective()));
        Assert.False(maintenance.Contains(hotfix.GetEffective()));

        // Special Case #1: Jan 3, 2025 3:00 AM UTC = Jan 2, 2025 8:00 PM MST. This is not a hotfix
        // window because Jan 2, 2025 is a Thursday.
        
        var current = CreateDto(new DateTime(2025, 1, 3, 3, 0, 0), 0);
        Assert.False(IsInHotfixWindow(current, hotfix.GetEffective()));
        Assert.False(hotfix.Contains(current));

        // Special Case #2: Mar 9, 2025 9:00 AM UTC = Mar 9, 2025 3:00 AM MST. This is potentially
        // problematic because on Sunday, March 9, 2025, 2:00:00 AM MDT clocks are turned forward 1
        // hour to Sunday, March 9, 2025, 3:00:00 AMD MST. MST Offset = -7h, and MDT Offset = -6h.
        // When the current time is in dayling savings time, and the interval's effective time is
        // not (or vice versa), then the Contains method needs to adjust for this variance. 
        
        current = CreateDto(new DateTime(2025, 3, 9, 9, 0, 0), 0);
        Assert.True(IsInMaintenanceWindow(current, maintenance.GetEffective()));
        Assert.True(maintenance.Contains(current));
    }

    private bool IsInHotfixWindow(DateTimeOffset current, DateTimeOffset first)
    {
        if (current < first)
            return false;

        TimeZoneInfo mountainTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");
        DateTimeOffset mountainTime = TimeZoneInfo.ConvertTime(current, mountainTimeZone);

        if (mountainTime.DayOfWeek != DayOfWeek.Wednesday)
            return false;

        TimeSpan start = new TimeSpan(20, 0, 0); // 8:00 PM
        TimeSpan end = new TimeSpan(21, 0, 0);   // 9:00 PM
        TimeSpan timeOfDay = mountainTime.TimeOfDay;

        return timeOfDay >= start && timeOfDay < end;
    }

    private bool IsInMaintenanceWindow(DateTimeOffset current, DateTimeOffset first)
    {
        if (current < first)
            return false;

        TimeZoneInfo mountainTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");
        DateTimeOffset mountainTime = TimeZoneInfo.ConvertTime(current, mountainTimeZone);

        TimeSpan start = new TimeSpan(3, 0, 0);  // 3:00 AM Mountain Time
        TimeSpan end = new TimeSpan(3, 30, 0);   // 3:30 AM Mountain Time
        TimeSpan timeOfDay = mountainTime.TimeOfDay;

        return timeOfDay >= start && timeOfDay < end;
    }
}