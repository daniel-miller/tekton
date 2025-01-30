using System;

namespace Tek.Common
{
    public enum LockoutState { Closed, Pending, Open }

    public class Lockouts
    {
        public LockoutState State { get; set; }

        public string Description { get; set; }

        public Lockout[] Items { get; set; }

        public Lockout Current { get; set; }

        public Lockout Pending { get; set; }

        public Lockouts()
        {
            Items = new Lockout[0];
        }

        public Lockouts(Lockout[] items, DateTimeOffset current, string enterprise, string environment)
        {
            if (items == null || items.Length == 0)
                return;

            Items = items;

            // If there is an active lockout, then it is the current lockout.

            foreach (var lockout in items)
            {
                if (lockout.IsActive(current, enterprise, environment))
                {
                    State = LockoutState.Open;

                    Current = lockout;

                    var duration = lockout.Interval.GetDuration();

                    var start = lockout.Interval.NextOpenTime(current).Value;

                    var end = start.Add(duration);

                    var tz = TimeZones.GetZone(lockout.Interval.Zone);

                    Description = $"Scheduled system maintenance started at {DescribeTime(start, tz)} "
                        + $"and is expected to complete within {DescribeDuration(duration)}, so you "
                        + $"can sign in again any time after {DescribeTime(end, tz)}."
                        + "\n\n"
                        + lockout.Description;

                    return;
                }
            }

            // If there is a lockout upcoming within the next hour, then it is the pending lockout.

            foreach (var lockout in items)
            {
                var next = lockout.MinutesBeforeOpenTime(current, enterprise, environment);

                if (next != null && next.Value < 60)
                {
                    State = LockoutState.Pending;

                    Pending = lockout;

                    var duration = lockout.Interval.GetDuration();

                    var start = lockout.Interval.NextOpenTime(current).Value;

                    var end = start.Add(duration);

                    var tz = TimeZones.GetZone(lockout.Interval.Zone);

                    Description = $"Scheduled system maintenance begins in "
                        + $"**{ToQuantity(next.Value, "minute")}**. Please save your work **before "
                        + $"{DescribeTime(start, tz)}** and then sign out. Maintenance is expected to "
                        + $"complete within {DescribeDuration(duration)}, so you can sign in again "
                        + $"any time after {DescribeTime(end, tz)}."
                        + "\n\n"
                        + lockout.Description;

                    return;
                }
            }

            // Otherwise, there is no current lockout and no pending lockout.
        }

        private string DescribeDuration(TimeSpan duration)
        {
            if (duration.TotalDays >= 1)
                return ToQuantity((int)duration.TotalDays, "day");

            if (duration.TotalHours >= 1 && IsInteger(duration.TotalHours))
                return ToQuantity((int)duration.TotalHours, "hour");

            return ToQuantity((int)duration.TotalMinutes, "minute");
        }

        private string DescribeTime(DateTimeOffset when, TimeZoneInfo tz)
        {
            return Clock.FormatTime(when, tz);
        }

        private string ToQuantity(int count, string noun)
        {
            if (count == 1)
                return $"1 {noun}";

            return $"{count:N0} {noun}s";
        }

        private bool IsInteger(double value)
        {
            return value % 1 == 0;
        }

        public bool IsStandbyExpected()
        {
            if (Current == null)
                return false;

            if (!Current.FilterInterfaces())
                return true;

            return "UI".MatchesAny(Current.Interfaces);
        }

        public bool IsUnavailableExpected()
        {
            if (Current == null)
                return false;

            if (!Current.FilterInterfaces())
                return true;

            return "API".MatchesAny(Current.Interfaces);
        }
    }
}