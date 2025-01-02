using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Atomic.Common
{
    /// <summary>
    /// Implements a half-open interval of date/time values.
    /// </summary>
    /// <remarks>
    /// An interval contains a date/time value X if X occurs in this range:
    ///    (Effective) &le; X &lt; (Effective + Duration)
    /// For example, a 10-minute interval that starts at 9:00 AM contains all date/time values in 
    /// between 9:00 AM (inclusive) and 9:10 AM (exclusive). 
    /// </remarks>
    public class Interval
    {
        private const string RequiredDateFormat = "yyyy-MM-dd";
        private const string RequiredTimeFormat = "HH:mm";

        /// <summary>
        /// Date and time the interval opens for the first time.
        /// </summary>
        public DateTimeOffset Effective { get; set; }

        /// <summary>
        /// Date the interval opens for the first time. (Year/Month/Date Format = yyyy-MM-dd)
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Time of day the interval opens. (24-Hour Format = HH:mm)
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Time zone identifier.
        /// </summary>
        public string Zone { get; set; }

        /// <summary>
        /// Length of time the interval is open. (e.g. 1h, 30m, 2d)
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// Days of the week when the interval is open. (e.g. Sun, Mon, Tue)
        /// </summary>
        public List<string> Recurrences { get; set; }

        /// <summary>
        /// Constructs a default interval.
        /// </summary>
        /// <remarks>
        /// The default is a non-recurring 60-minute interval that opens Jan 1, 2100 at 00:00 UTC.
        /// This ensures it is in the distant future and is therefore closed by default for all 
        /// date/time values in the current century.
        /// </remarks>
        public Interval()
            : this(Clock.NextCentury, "UTC", "1h")
        {
            
        }

        /// <summary>
        /// Constructs an interval that opens on a specific date, at a specific time, within a 
        /// specific time zone, for a specific period of time.
        /// </summary>
        public Interval(DateTime effective, string timeZone, string duration)
        {
            Effective = effective;
            Date = effective.ToString(RequiredDateFormat, CultureInfo.CurrentCulture);
            Time = effective.ToString(RequiredTimeFormat, CultureInfo.CurrentCulture);
            Zone = timeZone;
            Duration = duration;
            Recurrences = new List<string>();
        }

        /// <summary>
        /// Determines if the interval contains a specific point in time.
        /// </summary>
        public bool Contains(DateTimeOffset when)
        {
            var effective = Effective.Date;

            var start = EffectiveTime(effective.Year, effective.Month, effective.Day);

            if (when < start)
                return false;

            var duration = DurationAsTimeSpan();

            var end = start.Add(duration);

            if (IsRecurring())
            {
                var days = RecurrencesAsDays();

                if (!days.Any(day => day == when.DayOfWeek))
                    return false;

                start = EffectiveTime(when.Year, when.Month, when.Day);

                end = start.Add(duration);
            }

            return start <= when && when < end;
        }

        /// <summary>
        /// Parses the duration string into a TimeSpan.
        /// </summary>
        public TimeSpan DurationAsTimeSpan()
        {
            if (string.IsNullOrWhiteSpace(Duration) || Duration.Length < 2)
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
            => Recurrences.Any(r => AreEqual(r, day));

        /// <summary>
        /// Determines if the interval recurs throughout the week.
        /// </summary>
        public bool IsRecurring()
            => Recurrences.Count > 0;

        /// <summary>
        /// Determines if the interval is valid.
        /// </summary>
        public bool IsValid()
            => !Validate().Any();

        /// <summary>
        /// Calculates exactly when the interval opens next, following a specific date and time.
        /// </summary>
        public int? MinutesBeforeOpenTime(DateTimeOffset when)
        {
            var open = NextOpenTime(when);

            if (open == null)
                return null;

            return (int)(open.Value - when).TotalMinutes;
        }

        /// <summary>
        /// Calculates exactly when the interval opens next, following a specific date and time.
        /// </summary>
        public DateTimeOffset? NextOpenTime(DateTimeOffset when)
        {
            // The simplest case is when the interval opens for the first time in the future.

            if (when < Effective)
                return Effective;

            // If the interval opened in the past, and there are no recurrences, then the interval
            // never opens again in the future.

            if (!Recurrences.Any())
                return null;

            // Otherwise, the interval opens next at the effective time on each day within the
            // recurrence pattern. Determine the first time this occurs in the future.

            var effective = EffectiveTime(when.Year, when.Month, when.Day);

            var recurrences = RecurrencesAsDays();

            var openings = Enumerable.Range(0, 8)
                .Select(i => effective.AddDays(i))
                .Where(day => recurrences.Contains(day.DayOfWeek))
                .ToList();

            return openings.FirstOrDefault(opening => when < opening);
        }

        /// <summary>
        /// Calculates the exact time when the interval might be in effect on a specific date.
        /// </summary>
        public DateTimeOffset EffectiveTime(DateTime when)
            => EffectiveTime(when.Year, when.Month, when.Day);

        /// <summary>
        /// Calculates the exact time when the interval might be in effect on a specific date.
        /// </summary>
        public DateTimeOffset EffectiveTime(int year, int month, int day)
        {
            var when = new DateTime(year, month, day);

            var time = Effective.TimeOfDay;

            var open = when.AddHours(time.Hours).AddMinutes(time.Minutes);
            
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(Zone);

            return TimeZoneInfo.ConvertTime(new DateTimeOffset(open), tz);
        }

        /// <summary>
        /// Parses the list of recurrences into an enumeration of weekdays.
        /// </summary>
        public List<DayOfWeek> RecurrencesAsDays()
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
            }).ToList();
        }

        /// <summary>
        /// Determines validation errors in the interval.
        /// </summary>
        public List<ValidationError> Validate()
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
            {
                errors.Add(new ValidationError { Property = nameof(Recurrences), Summary = "Recurrences cannot be null. Use an empty list for a non-recurring interval." });
            }
            else if (Recurrences.Any())
            {
                if (!RecurrencesAsDays().Contains(Effective.DayOfWeek))
                    errors.Add(new ValidationError { Property = nameof(Recurrences), Summary = $"Recurrences for this interval must include {Effective.DayOfWeek} because {Date} occurs on this day of the week." });
            }

            return errors;
        }

        /// <summary>
        /// Determines if two strings are equal, ignoring case.
        /// </summary>
        private bool AreEqual(string x, string y)
            => string.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0;
    }
}