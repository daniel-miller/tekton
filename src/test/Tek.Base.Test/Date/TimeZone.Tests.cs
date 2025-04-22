namespace Tek.Base.Test;

public class TimeZoneTests
{
    const string MountainStandardTime = "Mountain Standard Time";
    const string PacificStandardTime = "Pacific Standard Time";
    const string Utc = "UTC";

    [Fact]
    public void Convert_InvalidTimeZone_ReturnUtc()
    {
        var now = DateTimeOffset.UtcNow;

        Assert.Equal(now, TimeZones.Convert(now, "XYZ"));
    }

    [Fact]
    public void Convert_MountainToPacific_ReturnUtc()
    {
        var now = DateTimeOffset.UtcNow;

        var mst = TimeZones.Convert(now, MountainStandardTime);
        var pst = TimeZones.Convert(now, PacificStandardTime);
        Assert.Equal(mst, pst);

        var mstMinusOneHour = mst.AddHours(-1);
        var differenceInHours = (mstMinusOneHour - pst).Hours;
        Assert.Equal(-1, differenceInHours);
    }

    [Fact]
    public void Convert_UtcToMst_ReturnUtc()
    {
        var utc = new DateTimeOffset(2024, 1, 24, 22, 03, 0, TimeSpan.Zero);
        var mst = new DateTimeOffset(2024, 1, 24, 15, 03, 0, TimeSpan.FromHours(-7));
        var converted = TimeZones.Convert(utc, MountainStandardTime);

        Assert.Equal(utc, mst);
        Assert.Equal(mst, converted);
        Assert.Equal(15, converted.Hour);
    }

    [Fact]
    public void IsValidZone_Utc_ReturnTrue()
    {
        Assert.True(TimeZones.IsValidZone(Utc));
    }

    [Fact]
    public void IsValidZone_MST_ReturnTrue()
    {
        Assert.True(TimeZones.IsValidZone(MountainStandardTime));
    }

    [Fact]
    public void IsValidZone_EmptyString_ReturnFalse()
    {
        Assert.False(TimeZones.IsValidZone(string.Empty));
        Assert.False(TimeZones.IsValidZone(" "));
    }

    [Fact]
    public void IsValidZone_InvalidZone_ReturnFalse()
    {
        Assert.False(TimeZones.IsValidZone("XYZ"));
    }

    [Fact]
    public void IsValidZone_AllTimeZones_Success()
    {
        foreach (var zone in TimeZoneInfo.GetSystemTimeZones())
        { 
            Assert.True(TimeZones.IsValidZone(zone.Id)); 
        }
    }
}