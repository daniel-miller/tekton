using System;
using System.Linq;

namespace Common
{
    public class Clock
    {
        public DateTimeOffset? ConvertTimeZone(DateTimeOffset? when, string tz)
        {
            const string utc = "UTC";

            if (!when.HasValue)
                return null;

            if (!IsValidTimeZone(tz) && IsValidTimeZone(utc))
                tz = utc;

            var info = TimeZoneInfo.FindSystemTimeZoneById(tz);
            var converted = TimeZoneInfo.ConvertTime(when.Value, info);
            return converted;
        }

        public bool IsValidTimeZone(string tz)
        {
            if (string.IsNullOrWhiteSpace(tz))
                return false;

            return TimeZoneInfo.GetSystemTimeZones().Any(x => x.Id == tz);
        }
    }
}