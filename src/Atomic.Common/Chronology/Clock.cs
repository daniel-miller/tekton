using System;
using System.Linq;

namespace Atomic.Common
{
    public class Clock
    {
        public static DateTimeOffset NextCentury = new DateTimeOffset(new DateTime(2100, 1, 1, 0, 0, 0), TimeSpan.Zero);

        public DateTimeOffset ConvertTimeZone(DateTimeOffset when, string tz)
        {
            const string utc = "UTC";

            if (!IsValidTimeZone(tz) && IsValidTimeZone(utc))
                tz = utc;

            var info = TimeZoneInfo.FindSystemTimeZoneById(tz);

            var converted = TimeZoneInfo.ConvertTime(when, info);

            return converted;
        }

        public TimeZoneInfo GetTimeZone(string tz)
        {
            if (!IsValidTimeZone(tz))
                return null;

            var info = TimeZoneInfo.FindSystemTimeZoneById(tz);
            
            return info;
        }

        public TimeZoneInfo[] GetTimeZones(DateTimeOffset when)
        {
            var offset = when.Offset;

            var zones = TimeZoneInfo.GetSystemTimeZones()
                .Where(tz => tz.BaseUtcOffset == offset)
                .OrderBy(x => x.StandardName)
                .ToArray();

            return zones;
        }

        public bool IsValidTimeZone(string tz)
        {
            if (string.IsNullOrWhiteSpace(tz))
                return false;

            return TimeZoneInfo.GetSystemTimeZones().Any(x => x.Id == tz);
        }
    }
}