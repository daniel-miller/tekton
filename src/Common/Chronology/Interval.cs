using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Common
{
    public class Interval
    {
        private const string RequiredDateFormat = "yyyy-MM-dd";
        private const string RequiredTimeFormat = "HH:mm";

        /// <summary>
        /// Date the interval becomes effective for the first time (yyyy-MM-dd).
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Start time (HH:mm).
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Time zone identifier.
        /// </summary>
        public string Zone { get; set; }

        /// <summary>
        /// Length of time the interval is active (e.g. 1h, 30m, 2d).
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// Days of the week the interval is active (e.g. Sun, Mon, Tue).
        /// </summary>
        public List<string> Recurrences { get; set; }

        public Interval()
        {
            Recurrences = new List<string>();
        }

        public Interval(DateTime effective, string timeZone, string duration)
            : this()
        {
            Date = effective.ToString(RequiredDateFormat, CultureInfo.InvariantCulture);
            Time = effective.ToString(RequiredTimeFormat, CultureInfo.InvariantCulture);
            Zone = timeZone;
            Duration = duration;
        }

        /// <summary>
        /// Determines if the interval contains a specific point in time.
        /// </summary>
        public bool Contains(DateTimeOffset point)
        {
            var effective = DateTime.ParseExact(Date, RequiredDateFormat, null);

            var start = NextPotentialStartTime(effective.Year, effective.Month, effective.Day);

            if (point < start)
                return false;

            var duration = DurationAsTimeSpan();

            var end = start.Add(duration);

            if (IsRecurring())
            {
                var days = RecurrencesAsDays();

                if (!days.Any(day => day == point.DayOfWeek))
                    return false;

                start = NextPotentialStartTime(point.Year, point.Month, point.Day);

                end = start.Add(duration);
            }

            return start <= point && point < end;
        }

        /// <summary>
        /// Parses the duration string into a TimeSpan.
        /// </summary>
        public TimeSpan DurationAsTimeSpan()
        {
            if (string.IsNullOrEmpty(Duration) || Duration.Length < 2)
                throw new ArgumentException("Invalid interval format");

            var pattern = @"(\d+)\s*([dhm])";

            var match = System.Text.RegularExpressions.Regex.Match(Duration, pattern);

            if (!match.Success)
                throw new ArgumentException("Invalid interval format");

            string numberPart = match.Groups[1].Value;

            if (!int.TryParse(numberPart, out int number))
                throw new ArgumentException("Invalid number in duration");

            string unit = match.Groups[2].Value.ToLower();

            switch (unit)
            {
                case "m":
                    return TimeSpan.FromMinutes(number);
                case "h":
                    return TimeSpan.FromHours(number);
                case "d":
                    return TimeSpan.FromDays(number);
                default:
                    throw new ArgumentException($"{unit} is not a supported unit for duration. Please use m (minutes), h (hours), or d (days).");
            }
        }

        /// <summary>
        /// Determines if the interval includes a recurrence for a specific day of the week.
        /// </summary>
        public bool IncludesRecurrence(string day)
        {
            return Recurrences.Any(r => AreEqual(r, day));
        }

        /// <summary>
        /// Determines if the interval recurs throughout the week.
        /// </summary>
        public bool IsRecurring()
        {
            return Recurrences.Count > 0;
        }

        /// <summary>
        /// Determines if the interval is valid.
        /// </summary>
        public bool IsValid()
            => !Validate().Any();

        /// <summary>
        /// Calculates the minutes until the next actual start time for the interval.
        /// </summary>
        public int MinutesUntilNextActualStartTime(DateTimeOffset when)
        {
            return (int)(NextActualStartTime(when) - when).TotalMinutes;
        }

        /// <summary>
        /// Calculates the exact date and time the interval is next active.
        /// </summary>
        public DateTimeOffset NextActualStartTime(DateTimeOffset when)
        {
            var effective = DateTime.ParseExact(Date, "yyyy-MM-dd", null);

            var start = NextPotentialStartTime(effective.Year, effective.Month, effective.Day);

            if (!IsRecurring())
                return start;

            var recurrences = RecurrencesAsDays();

            start = NextPotentialStartTime(when.Year, when.Month, when.Day);

            var intervals = new List<DateTimeOffset>();

            var upcoming = Enumerable.Range(0, 7)
                .Select(i => start.AddDays(i))
                .Where(i => recurrences.Any(recurrence => recurrence == i.DayOfWeek));

            intervals.AddRange(upcoming);

            return intervals.First();
        }

        /// <summary>
        /// Calculates when the interval starts next if recurrences are ignored.
        /// </summary>
        public DateTimeOffset NextPotentialStartTime(DateTime when)
            => NextPotentialStartTime(when.Year, when.Month, when.Day);

        /// <summary>
        /// Calculates when the interval starts next if recurrences are ignored.
        /// </summary>
        public DateTimeOffset NextPotentialStartTime(int year, int month, int day)
        {
            var date = new DateTime(year, month, day);

            var time = DateTime.ParseExact(Time, "HH:mm", null);
            
            var effective = date.AddHours(time.Hour).AddMinutes(time.Minute);
            
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(Zone);

            var start = TimeZoneInfo.ConvertTime(new DateTimeOffset(effective), tz);

            return start;
        }

        /// <summary>
        /// Parses the list of recurrences into an enumeration of weekdays.
        /// </summary>
        public IEnumerable<DayOfWeek> RecurrencesAsDays()
        {
            return Recurrences.Select(r =>
            {
                switch (r.ToLower())
                {
                    case "sun":
                        return DayOfWeek.Sunday;
                    case "mon":
                        return DayOfWeek.Monday;
                    case "tue":
                        return DayOfWeek.Tuesday;
                    case "wed":
                        return DayOfWeek.Wednesday;
                    case "thu":
                        return DayOfWeek.Thursday;
                    case "fri":
                        return DayOfWeek.Friday;
                    case "sat":
                        return DayOfWeek.Saturday;
                    default:
                        throw new ArgumentException($"{r} is not a valid day of the week.");
                }
            });
        }

        /// <summary>
        /// Determines validation errors in the interval.
        /// </summary>
        public IEnumerable<ValidationError> Validate()
        {
            var errors = new List<ValidationError>();

            if (!DateTime.TryParseExact(Date, RequiredDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                errors.Add(new ValidationError { Property = nameof(Date), Summary = $"{Date} is not an expected date format. Please use {RequiredDateFormat}." });

            if (!DateTime.TryParseExact(Time, RequiredTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
                errors.Add(new ValidationError { Property = nameof(Time), Summary = $"{Time} is not an expected time format. Please use {RequiredTimeFormat}." });

            if (!TimeZoneInfo.GetSystemTimeZones().Any(tz => AreEqual(tz.Id, Zone)))
                errors.Add(new ValidationError { Property = nameof(Zone), Summary = $"{Zone} is not a valid time zone." });

            try
            {
                DurationAsTimeSpan();
            }
            catch (ArgumentException ex)
            {
                errors.Add(new ValidationError { Property = nameof(Duration), Summary = ex.Message });
            }

            if (Recurrences == null)
                errors.Add(new ValidationError { Property = nameof(Recurrences), Summary = "Recurrences cannot be null. Use an empty list for a non-recurring interval." });

            return errors;
        }

        /// <summary>
        /// Determines if two strings are equal, ignoring case.
        /// </summary>
        private bool AreEqual(string x, string y)
            => string.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0;
    }
}