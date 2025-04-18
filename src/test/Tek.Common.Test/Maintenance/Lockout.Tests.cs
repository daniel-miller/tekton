using Newtonsoft.Json;

namespace Tek.Common.Test;

public class LockoutTests
{
    [Fact]
    public void Contructor_DefaultLockout()
    {
        var lockout = new Lockout();
        
        Assert.Empty(lockout.Enterprises);
        Assert.Empty(lockout.Environments);
        Assert.Empty(lockout.Interfaces);

        Assert.NotNull(lockout.Interval);
    }

    [Fact]
    public void Contructor_WeeklyHotfix()
    {
        Lockout lockout = CreateWeeklyHotfix();

        // Assertions:

        Assert.True(lockout.IsValid());

        Assert.False(lockout.FilterEnterprises());

        Assert.True(lockout.FilterEnvironments());
        Assert.True(lockout.Environments[0].Matches("Local"));
        Assert.True(lockout.Environments[1].Matches("Development"));
        Assert.True(lockout.Environments[2].Matches("Sandbox"));

        Assert.True(lockout.FilterInterfaces());
        Assert.True(lockout.Interfaces[0].Matches("API"));

        var effective = lockout.Interval.GetEffective();

        Assert.Equal(20, effective.Hour);
        Assert.Equal(0, effective.Minute);
        Assert.Equal(60, lockout.Interval.GetDuration().TotalMinutes);

        var twoHoursBefore = CreateDto(2025, 1, 15, 18, 0, 0, -7);
        Assert.Equal(120, lockout.MinutesBeforeOpenTime(twoHoursBefore, "E01", "Local"));

        var sixtyOneMinutesBefore = CreateDto(2025, 1, 15, 18, 59, 0, -7);
        Assert.Equal(61, lockout.MinutesBeforeOpenTime(sixtyOneMinutesBefore, "E01", "Local"));

        var sixtyMinutesBefore = CreateDto(2025, 1, 15, 19, 0, 0, -7);
        Assert.Equal(60, lockout.MinutesBeforeOpenTime(sixtyMinutesBefore, "E01", "Local"));

        var fiftyNineMinutesBefore = CreateDto(2025, 1, 15, 19, 1, 0, -7);
        Assert.Equal(59, lockout.MinutesBeforeOpenTime(fiftyNineMinutesBefore, "E01", "Local"));

        var oneMinuteBefore = CreateDto(2025, 1, 15, 19, 59, 0, -7);
        Assert.Equal(1, lockout.MinutesBeforeOpenTime(oneMinuteBefore, "E01", "Local"));

        var oneSecondBefore = CreateDto(2025, 1, 15, 19, 59, 59, -7);
        Assert.Equal(1, lockout.MinutesBeforeOpenTime(oneSecondBefore, "E01", "Local"));

        var start = CreateDto(2025, 1, 15, 20, 0, 0, -7);
        Assert.Equal(7 * 24 * 60, lockout.MinutesBeforeOpenTime(start, "E01", "Local"));
    }

    [Fact]
    public void Contructor_DailyBuild()
    {
        var lockout = CreateDailyBuild();

        var effective = lockout.Interval.GetEffective();

        Assert.Equal(8, effective.Hour);
        Assert.Equal(0, effective.Minute);
        Assert.Equal(60, lockout.Interval.GetDuration().TotalMinutes);
    }

    [Fact]
    public void Contructor_DailyMaintenance()
    {
        var lockout = CreateDailyMaintenance();

        var effective = lockout.Interval.GetEffective();

        Assert.Equal(3, effective.Hour);
        Assert.Equal(0, effective.Minute);
        Assert.Equal(30, lockout.Interval.GetDuration().TotalMinutes);

        Assert.False(lockout.FilterEnterprises());
        Assert.False(lockout.FilterEnvironments());

        Assert.True(lockout.FilterInterfaces());
        Assert.True(lockout.Interfaces[0].Matches("API"));
        Assert.True(lockout.Interfaces[1].Matches("UI"));
    }

    [Fact]
    public void Contructor_Custom1()
    {
        var lockout = CreateCustom1();

        var effective = lockout.Interval.GetEffective();

        Assert.Equal("Pacific Standard Time", lockout.Interval.Zone);

        Assert.Equal(10, effective.Hour);
        Assert.Equal(0, effective.Minute);
        Assert.Equal(30, lockout.Interval.GetDuration().TotalMinutes);

        Assert.Single(lockout.Enterprises);
        Assert.True(lockout.Enterprises[0].Matches("E04"));

        Assert.Single(lockout.Interfaces);
        Assert.True(lockout.Interfaces[0].Matches("API"));
    }

    [Fact]
    public void Contructor_Custom2()
    {
        var lockout = CreateCustom2();
        
        var effective = lockout.Interval.GetEffective();

        Assert.Equal("Pacific Standard Time", lockout.Interval.Zone);

        Assert.Equal(11, effective.Hour);
        Assert.Equal(30, effective.Minute);
        Assert.Equal(30, lockout.Interval.GetDuration().TotalMinutes);

        Assert.Single(lockout.Enterprises);
        Assert.True(lockout.Enterprises[0].Matches("E04"));

        Assert.Single(lockout.Interfaces);
        Assert.True(lockout.Interfaces[0].Matches("API"));
    }

    [Fact]
    public void Contructor_Custom3()
    {
        var lockout = CreateCustom3();

        var effective = lockout.Interval.GetEffective();

        Assert.Equal("Pacific Standard Time", lockout.Interval.Zone);

        Assert.Equal(13, effective.Hour);
        Assert.Equal(30, effective.Minute);
        Assert.Equal(30, lockout.Interval.GetDuration().TotalMinutes);

        Assert.Single(lockout.Enterprises);
        Assert.True(lockout.Enterprises[0].Matches("E04"));

        Assert.Single(lockout.Interfaces);
        Assert.True(lockout.Interfaces[0].Matches("API"));
    }

    [Fact]
    public void Contructor_Custom4()
    {
        var lockout = CreateCustom4();

        var effective = lockout.Interval.GetEffective();

        Assert.Equal("Pacific Standard Time", lockout.Interval.Zone);

        Assert.Equal(15, effective.Hour);
        Assert.Equal(30, effective.Minute);
        Assert.Equal(30, lockout.Interval.GetDuration().TotalMinutes);

        Assert.Single(lockout.Enterprises);
        Assert.True(lockout.Enterprises[0].Matches("E04"));

        Assert.Single(lockout.Interfaces);
        Assert.True(lockout.Interfaces[0].Matches("API"));
    }

    [Fact(Skip = "Slow test. Needs ~50s to run.")]
    public void IsActive_AlternateStrategy()
    {
        string[] enterprises = ["E01", "E02", "E03", "E04", "E05", "E06", "E07"];

        var from = new DateTimeOffset(2024, 12, 1, 0, 0, 0, TimeSpan.Zero);
        var thru = new DateTimeOffset(2026, 2, 1, 0, 0, 0, TimeSpan.Zero);

        var hotfix = CreateWeeklyHotfix();
        var build = CreateDailyBuild();
        var maintenance = CreateDailyMaintenance();

        var custom1 = CreateCustom1();
        var custom2 = CreateCustom2();
        var custom3 = CreateCustom3();
        var custom4 = CreateCustom4();

        foreach (var enterprise in enterprises)
        {
            foreach (var environment in Environments.All)
            {
                for (var current = from; current <= thru; current = current.AddMinutes(5))
                {
                    Assert.Equal(IsActiveHotfixWindow(hotfix.Interval.GetEffective(), current, enterprise, environment.Name), hotfix.IsActive(current, enterprise, environment.Name.ToString()));
                    Assert.Equal(IsActiveDailyBuild(build.Interval.GetEffective(), current, enterprise, environment.Name), build.IsActive(current, enterprise, environment.Name.ToString()));
                    Assert.Equal(IsActiveDailyMaintenance(maintenance.Interval.GetEffective(), current, enterprise, environment.Name), maintenance.IsActive(current, enterprise, environment.Name.ToString()));

                    Assert.Equal(IsActiveCustom1(custom1.Interval.GetEffective(), current, enterprise, environment.Name), custom1.IsActive(current, enterprise, environment.Name.ToString()));
                    Assert.Equal(IsActiveCustom2(custom2.Interval.GetEffective(), current, enterprise, environment.Name), custom2.IsActive(current, enterprise, environment.Name.ToString()));
                    Assert.Equal(IsActiveCustom3(custom3.Interval.GetEffective(), current, enterprise, environment.Name), custom3.IsActive(current, enterprise, environment.Name.ToString()));
                    Assert.Equal(IsActiveCustom4(custom4.Interval.GetEffective(), current, enterprise, environment.Name), custom4.IsActive(current, enterprise, environment.Name.ToString()));
                }
            }
        }

        var test1 = CreateDailyLocalTest();
        var test2 = CreateDailyDevelopmentTest();
        var lockouts = new List<Lockout> { maintenance, build, hotfix, test1, test2, custom1, custom2, custom3, custom4 };
        var json = JsonConvert.SerializeObject(lockouts);
        var path = @"d:\temp\lockouts.json";
        File.WriteAllText(path, json);
    }

    [Fact]
    public void Lockouts_ExpectedBehavior()
    {
        var e01_loc_on_0101_at_0315 = CreateLockouts(CreateDto(2025, 1, 1, 3, 15, 0, -7), "E01", "Local");
        var e02_dev_on_0101_at_0830 = CreateLockouts(CreateDto(2025, 1, 1, 8, 30, 0, -7), "E02", "Development");
        var e03_san_on_0101_at_2045 = CreateLockouts(CreateDto(2025, 1, 1, 20, 45, 0, -7), "E03", "Sandbox");

        var e04_dev_on_0101_at_1115 = CreateLockouts(CreateDto(2025, 1, 1, 11, 15, 0, -7), "E04", "Development");
        var e04_dev_on_0101_at_1245 = CreateLockouts(CreateDto(2025, 1, 1, 12, 45, 0, -7), "E04", "Development");
        var e04_dev_on_0101_at_1445 = CreateLockouts(CreateDto(2025, 1, 1, 14, 45, 0, -7), "E04", "Development");
        var e04_dev_on_0101_at_1645 = CreateLockouts(CreateDto(2025, 1, 1, 16, 45, 0, -7), "E04", "Development");

        Assert.True(e01_loc_on_0101_at_0315.IsStandbyExpected());
        Assert.False(e02_dev_on_0101_at_0830.IsStandbyExpected());
        Assert.False(e03_san_on_0101_at_2045.IsStandbyExpected());

        Assert.True(e01_loc_on_0101_at_0315.IsUnavailableExpected());
        Assert.True(e02_dev_on_0101_at_0830.IsUnavailableExpected());
        Assert.True(e03_san_on_0101_at_2045.IsUnavailableExpected());

        Assert.False(e04_dev_on_0101_at_1115.IsStandbyExpected());
        Assert.False(e04_dev_on_0101_at_1245.IsStandbyExpected());
        Assert.False(e04_dev_on_0101_at_1445.IsStandbyExpected());
        Assert.False(e04_dev_on_0101_at_1645.IsStandbyExpected());

        Assert.True(e04_dev_on_0101_at_1115.IsUnavailableExpected());
        Assert.True(e04_dev_on_0101_at_1245.IsUnavailableExpected());
        Assert.True(e04_dev_on_0101_at_1445.IsUnavailableExpected());
        Assert.True(e04_dev_on_0101_at_1645.IsUnavailableExpected());
    }

    #region Helpers

    private DateTimeOffset ConvertToMountainTime(DateTimeOffset current)
    {
        TimeZoneInfo mountainTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");
        DateTimeOffset mountainTime = TimeZoneInfo.ConvertTime(current, mountainTimeZone);
        return mountainTime;
    }

    private DateTimeOffset CreateDto(int year, int month, int day, int hour, int minute, int second, int offset)
    {
        return new DateTimeOffset(new DateTime(year, month, day, hour, minute, second), new TimeSpan(offset, 0, 0));
    }

    private Lockout CreateLockout(string? enterprise = null)
    {
        var lockout = new Lockout
        {
            Environments = ["Local", "Development", "Sandbox"]
        };

        if (enterprise != null)
            lockout.Enterprises = [enterprise];

        return lockout;
    }

    private Lockout CreateWeeklyHotfix()
    {
        var lockout = CreateLockout();

        // January 1, 2025 at 8:00 PM MST (UTC-7h)
        var effective = new DateTimeOffset(new DateTime(2025, 1, 1, 20, 0, 0), new TimeSpan(-7, 0, 0));

        // Every Wednesday for 60 minutes.
        lockout.Interval = new Interval(effective, "1h", "Wed");

        // API only.
        lockout.Interfaces = ["API"];
        return lockout;
    }

    private Lockout CreateDailyBuild()
    {
        var lockout = CreateLockout();

        // January 1, 2025 at 8:00 AM MST (UTC-7h)
        var effective = new DateTimeOffset(new DateTime(2025, 1, 1, 8, 0, 0), new TimeSpan(-7, 0, 0));

        // Every weekday for 60 minutes.
        lockout.Interval = new Interval(effective, "60m", "Mon,Tue,Wed,Thu,Fri");

        // API only.
        lockout.Interfaces = ["API"];

        return lockout;
    }

    private Lockout CreateDailyMaintenance()
    {
        var lockout = new Lockout();

        // January 1, 2025 at 3:00 AM MST (UTC-7h)
        var effective = new DateTimeOffset(new DateTime(2025, 1, 1, 3, 0, 0), new TimeSpan(-7, 0, 0));

        // Every day for 30 minutes.
        lockout.Interval = new Interval(effective, "30m", "Sun,Mon,Tue,Wed,Thu,Fri,Sat");

        // API and UI.
        lockout.Interfaces = ["API", "UI"];

        return lockout;
    }

    private Lockout CreateDailyLocalTest()
    {
        var lockout = new Lockout();

        lockout.Enterprises = ["E01"];
        lockout.Environments = ["Local"];

        // January 1, 2025 at 17:00 PM MST (UTC-7h)
        var effective = new DateTimeOffset(new DateTime(2025, 1, 1, 17, 0, 0), new TimeSpan(-7, 0, 0));

        // Every Sunday for 2 hours.
        lockout.Interval = new Interval(effective, "2h", "Sun");

        // API and UI.
        lockout.Interfaces = ["API", "UI"];

        return lockout;
    }

    private Lockout CreateDailyDevelopmentTest()
    {
        var lockout = new Lockout();

        lockout.Enterprises = ["E01"];
        lockout.Environments = ["Development"];

        // January 1, 2025 at 12:00 PM MST (UTC-7h)
        var effective = new DateTimeOffset(new DateTime(2025, 1, 1, 12, 0, 0), new TimeSpan(-7, 0, 0));

        // Every weekday for 7 minutes.
        lockout.Interval = new Interval(effective, "7m", "Mon,Tue,Wed,Thu,Fri");

        // API and UI.
        lockout.Interfaces = ["API", "UI"];

        return lockout;
    }

    private Lockout CreateCustom1()
    {
        var lockout = new Lockout();
        
        lockout.Enterprises = ["E04"]; // STBC only.

        // January 1, 2025 at 10:00 AM PST (UTC-8h)
        var effective = new DateTimeOffset(new DateTime(2025, 1, 1, 10, 0, 0), new TimeSpan(-8, 0, 0));

        // Every day for 30 minutes.
        lockout.Interval = new Interval(effective, "30m", "Sun,Mon,Tue,Wed,Thu,Fri,Sat");

        // API only.
        lockout.Interfaces = ["API"];

        return lockout;
    }

    private Lockout CreateCustom2()
    {
        var lockout = new Lockout();

        lockout.Enterprises = ["E04"]; // STBC only.

        // January 1, 2025 at 11:30 AM PST (UTC-8h)
        var effective = new DateTimeOffset(new DateTime(2025, 1, 1, 11, 30, 0), new TimeSpan(-8, 0, 0));

        // Every day for 30 minutes.
        lockout.Interval = new Interval(effective, "30m", "Sun,Mon,Tue,Wed,Thu,Fri,Sat");

        // API only.
        lockout.Interfaces = ["API"];

        return lockout;
    }

    private Lockout CreateCustom3()
    {
        var lockout = new Lockout();

        lockout.Enterprises = ["E04"]; // STBC only.

        // January 1, 2025 at 13:30 AM PST (UTC-8h)
        var effective = new DateTimeOffset(new DateTime(2025, 1, 1, 13, 30, 0), new TimeSpan(-8, 0, 0));

        // Every day for 30 minutes.
        lockout.Interval = new Interval(effective, "30m", "Sun,Mon,Tue,Wed,Thu,Fri,Sat");

        // API only.
        lockout.Interfaces = ["API"];

        return lockout;
    }

    private Lockout CreateCustom4()
    {
        var lockout = new Lockout();

        lockout.Enterprises = ["E04"]; // STBC only.

        // January 1, 2025 at 15:30 AM PST (UTC-8h)
        var effective = new DateTimeOffset(new DateTime(2025, 1, 1, 15, 30, 0), new TimeSpan(-8, 0, 0));

        // Every day for 30 minutes.
        lockout.Interval = new Interval(effective, "30m", "Sun,Mon,Tue,Wed,Thu,Fri,Sat");

        // API only.
        lockout.Interfaces = ["API"];

        return lockout;
    }

    private Lockouts CreateLockouts(DateTimeOffset current, string enterprise, string environment)
    {
        var hotfix = CreateWeeklyHotfix();
        var build = CreateDailyBuild();
        var maintenance = CreateDailyMaintenance();

        var custom1 = CreateCustom1();
        var custom2 = CreateCustom2();
        var custom3 = CreateCustom3();
        var custom4 = CreateCustom4();

        var items = new Lockout[] { hotfix, build, maintenance, custom1, custom2, custom3, custom4 };

        return new Lockouts(items, current, enterprise, environment);
    }

    #endregion

    #region Hardcoded Strategies

    private bool IsActiveHotfixWindow(DateTimeOffset first, DateTimeOffset current, string enterprise, EnvironmentName environment)
    {
        if (environment == EnvironmentName.Production || first > current)
            return false;

        var when = ConvertToMountainTime(current);

        if (when.DayOfWeek != DayOfWeek.Wednesday)
            return false;

        var start = new TimeSpan(20, 0, 0); // 8:00 PM 
        var end = new TimeSpan(21, 0, 0);   // 9:00 PM
        var time = when.TimeOfDay;

        return start <= time && time < end;
    }

    private bool IsActiveDailyBuild(DateTimeOffset first, DateTimeOffset current, string enterprise, EnvironmentName environment)
    {
        if (environment == EnvironmentName.Production || first > current)
            return false;

        var when = ConvertToMountainTime(current);

        if (when.DayOfWeek == DayOfWeek.Sunday || when.DayOfWeek == DayOfWeek.Saturday)
            return false;

        var start = new TimeSpan(8, 0, 0); // 8:00 AM
        var end = new TimeSpan(9, 0, 0);   // 9:00 AM
        var time = when.TimeOfDay;

        return start <= time && time < end;
    }

    private bool IsActiveDailyMaintenance(DateTimeOffset first, DateTimeOffset current, string enterprise, EnvironmentName environment)
    {
        if (first > current)
            return false;

        var when = ConvertToMountainTime(current);

        var start = new TimeSpan(3, 0, 0); // 3:00 AM
        var end = new TimeSpan(3, 30, 0);  // 3:30 AM
        var time = when.TimeOfDay;

        return start <= time && time < end;
    }

    private bool IsActiveCustom1(DateTimeOffset first, DateTimeOffset current, string enterprise, EnvironmentName environment)
    {
        if (!enterprise.Matches("E04"))
            return false;

        if (first > current)
            return false;

        var when = ConvertToMountainTime(current);

        var start = new TimeSpan(11, 0, 0); // 11:00 AM Mountain
        var end = new TimeSpan(11, 30, 0);  // 11:30 AM Mountain
        var time = when.TimeOfDay;

        return start <= time && time < end;
    }

    private bool IsActiveCustom2(DateTimeOffset first, DateTimeOffset current, string enterprise, EnvironmentName environment)
    {
        if (!enterprise.Matches("E04"))
            return false;

        if (first > current)
            return false;

        var when = ConvertToMountainTime(current);

        var start = new TimeSpan(12, 30, 0); // 12:30 PM Mountain
        var end = new TimeSpan(13, 0, 0);    // 13:00 PM Mountain
        var time = when.TimeOfDay;

        return start <= time && time < end;
    }

    private bool IsActiveCustom3(DateTimeOffset first, DateTimeOffset current, string enterprise, EnvironmentName environment)
    {
        if (!enterprise.Matches("E04"))
            return false;

        if (first > current)
            return false;

        var when = ConvertToMountainTime(current);

        var start = new TimeSpan(14, 30, 0); // 14:30 PM Mountain
        var end = new TimeSpan(15, 0, 0);    // 15:00 PM Mountain
        var time = when.TimeOfDay;

        return start <= time && time < end;
    }

    private bool IsActiveCustom4(DateTimeOffset first, DateTimeOffset current, string enterprise, EnvironmentName environment)
    {
        if (!enterprise.Matches("E04"))
            return false;

        if (first > current)
            return false;

        var when = ConvertToMountainTime(current);

        var start = new TimeSpan(16, 30, 0); // 16:30 PM Mountain
        var end = new TimeSpan(17, 0, 0);    // 17:00 PM Mountain
        var time = when.TimeOfDay;

        return start <= time && time < end;
    }

    #endregion
}