namespace Atomic.Common.Test;

public class ClockTests
{
    const string MountainStandardTime = "Mountain Standard Time";
    const string PacificStandardTime = "Pacific Standard Time";
    const string UTC = "UTC";

    [Fact]
    public void ConvertTimeZone_InvalidTimeZone_ReturnUTC()
    {
        var clock = new Clock();
        var now = DateTimeOffset.UtcNow;
        Assert.Equal(now, clock.ConvertTimeZone(now, "XYZ"));
    }

    [Fact]
    public void ConvertTimeZone_MountainToPacific_ReturnUTC()
    {
        var clock = new Clock();
        var now = DateTimeOffset.UtcNow;

        var mst = clock.ConvertTimeZone(now, MountainStandardTime);
        var pst = clock.ConvertTimeZone(now, PacificStandardTime);
        Assert.Equal(mst, pst);

        var mstMinusOneHour = mst.AddHours(-1);
        var differenceInHours = (mstMinusOneHour - pst).Hours;
        Assert.Equal(-1, differenceInHours);
    }

    [Fact]
    public void ConvertTimeZone_UtcToMst_ReturnUTC()
    {
        var clock = new Clock();

        var utc = new DateTimeOffset(2024, 1, 24, 22, 03, 0, TimeSpan.Zero);
        var mst = new DateTimeOffset(2024, 1, 24, 15, 03, 0, TimeSpan.FromHours(-7));
        var converted = clock.ConvertTimeZone(utc, MountainStandardTime);

        Assert.Equal(utc, mst);
        Assert.Equal(mst, converted);
        Assert.Equal(15, converted.Hour);
    }

    [Fact]
    public void IsValidTimeZone_UTC_ReturnTrue()
    {
        var clock = new Clock();
        Assert.True(clock.IsValidTimeZone(UTC));
    }

    [Fact]
    public void IsValidTimeZone_MST_ReturnTrue()
    {
        var clock = new Clock();
        Assert.True(clock.IsValidTimeZone(MountainStandardTime));
    }

    [Fact]
    public void IsValidTimeZone_EmptyString_ReturnFalse()
    {
        var clock = new Clock();
        Assert.False(clock.IsValidTimeZone(string.Empty));
        Assert.False(clock.IsValidTimeZone(" "));
    }

    [Fact]
    public void IsValidTimeZone_InvalidZone_ReturnFalse()
    {
        var clock = new Clock();
        Assert.False(clock.IsValidTimeZone("XYZ"));
    }

    [Fact]
    public void IsValidTimeZone_AllTimeZones_Success()
    {
        var clock = new Clock();
        foreach (var zone in TimeZoneInfo.GetSystemTimeZones())
        { 
            Assert.True(clock.IsValidTimeZone(zone.Id)); 
        }
    }
}