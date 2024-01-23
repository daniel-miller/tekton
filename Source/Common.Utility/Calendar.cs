namespace Common.Utility
{
    public class Calendar
    {
        public DateTimeOffset? ConvertTimeZone(DateTimeOffset? dto, string tz)
        {
            const string utc = "UTC";

            if (!dto.HasValue)
                return null;

            if (!IsValidTimeZone(tz) && IsValidTimeZone(utc))
                tz = utc;

            var info = TimeZoneInfo.FindSystemTimeZoneById(tz);
            var converted = TimeZoneInfo.ConvertTime(dto.Value, info);
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