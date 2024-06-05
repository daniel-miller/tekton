using System;

namespace Common
{
    public class TimeZoneAbbreviation
    {
        public string Generic { get; }

        public string Standard { get; }

        public string Daylight { get; }

        public string Moment { get; }

        private readonly TimeZoneInfo _zone;

        public TimeZoneAbbreviation(TimeZoneInfo zone, string generic, string standard, string daylight, string moment)
        {
            _zone = zone;

            Generic = generic;
            Standard = standard;
            Daylight = daylight;
            Moment = moment;
        }

        public string GetAbbreviation(DateTimeOffset time) 
            => _zone.IsDaylightSavingTime(time) ? Daylight : Standard;
    }
}