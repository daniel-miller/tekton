using Common.Utility;

namespace Common.Tests.Auto.Utility
{
    public class CalendarTests
    {
        const string MountainStandardTime = "Mountain Standard Time";
        const string PacificStandardTime = "Pacific Standard Time";
        const string UTC = "UTC";

        [Fact]
        public void ConvertTimeZone_NullDateTimeOffset_ReturnNull()
        {
            var calendar = new Calendar();
            Assert.Null(calendar.ConvertTimeZone(null, UTC));
        }

        [Fact]
        public void ConvertTimeZone_InvalidTimeZone_ReturnUTC()
        {
            var calendar = new Calendar();
            var now = DateTimeOffset.UtcNow;
            Assert.Equal(now, calendar.ConvertTimeZone(now, "XYZ"));
        }

        [Fact]
        public void ConvertTimeZone_MountainToPacific_ReturnUTC()
        {
            var calendar = new Calendar();
            var now = DateTimeOffset.UtcNow;

            var mst = calendar.ConvertTimeZone(now, MountainStandardTime);
            var pst = calendar.ConvertTimeZone(now, PacificStandardTime);
            Assert.Equal(mst, pst);

            if (mst.HasValue && pst.HasValue)
            {
                var mstMinusOneHour = mst.Value.AddHours(-1);
                var differenceInHours = (mstMinusOneHour - pst).Value.Hours;
                Assert.Equal(-1, differenceInHours);
            }
        }

        [Fact]
        public void ConvertTimeZone_UtcToMst_ReturnUTC()
        {
            var calendar = new Calendar();

            var utc = new DateTimeOffset(2024, 1, 24, 22, 03, 0, TimeSpan.Zero);
            var mst = new DateTimeOffset(2024, 1, 24, 15, 03, 0, TimeSpan.FromHours(-7));
            var converted = calendar.ConvertTimeZone(utc, MountainStandardTime);

            Assert.Equal(utc, mst);
            Assert.Equal(mst, converted);
            Assert.Equal(15, converted.Value.Hour);
        }

        [Fact]
        public void IsValidTimeZone_UTC_ReturnTrue()
        {
            var calendar = new Calendar();
            Assert.True(calendar.IsValidTimeZone(UTC));
        }

        [Fact]
        public void IsValidTimeZone_MST_ReturnTrue()
        {
            var calendar = new Calendar();
            Assert.True(calendar.IsValidTimeZone(MountainStandardTime));
        }

        [Fact]
        public void IsValidTimeZone_EmptyString_ReturnFalse()
        {
            var calendar = new Calendar();
            Assert.False(calendar.IsValidTimeZone(string.Empty));
            Assert.False(calendar.IsValidTimeZone(" "));
        }

        [Fact]
        public void IsValidTimeZone_InvalidZone_ReturnFalse()
        {
            var calendar = new Calendar();
            Assert.False(calendar.IsValidTimeZone("XYZ"));
        }

        [Fact]
        public void IsValidTimeZone_AllTimeZones_Success()
        {
            var calendar = new Calendar();
            foreach (var zone in TimeZoneInfo.GetSystemTimeZones())
            { 
                Assert.True(calendar.IsValidTimeZone(zone.Id)); 
            }
        }
    }
}