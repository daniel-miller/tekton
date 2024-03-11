﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Utility
{
    public class Calendar
    {
        /// <remarks>
        /// Calculates the current date plus/minus a specific number of business days. For example, if today is Friday, 
        /// then today plus one business day is the following Monday.
        /// </remark>
        public DateTimeOffset AddBusinessDays(DateTimeOffset when, int days, IEnumerable<DateTimeOffset> holidays = null)
        {
            var sign = Math.Sign(days);
            var unsignedDays = Math.Abs(days);
            for (var i = 0; i < unsignedDays; i++)
            {
                do
                {
                    when = when.AddDays(sign);
                }
                while (when.DayOfWeek == DayOfWeek.Saturday
                    || when.DayOfWeek == DayOfWeek.Sunday
                    || (holidays != null && holidays.Any(holiday => holiday == when.Date)));
            }
            return when;
        }

        public DateTimeOffset GetStartOfMonth(DateTimeOffset when)
        {
            return new DateTimeOffset(when.Year, when.Month, 1, 0, 0, 0, TimeSpan.Zero);
        }

        public DateTimeOffset GetStartOfWeek(DateTimeOffset when)
        {
            var sunday = when.AddDays(-(int)when.DayOfWeek);
            return new DateTimeOffset(sunday.Year, sunday.Month, sunday.Day, 0, 0, 0, TimeSpan.Zero);
        }

        public DateTimeOffset GetStartOfYear(DateTimeOffset when)
        {
            return new DateTimeOffset(when.Year, 1, 1, 0, 0, 0, TimeSpan.Zero);
        }

        public CalendarSeason GetSeason(DateTimeOffset when)
        {
            var today = $"{when.Month:D2}/{when.Day:D2}";

            if (today.CompareTo("03/21") >= 0 && today.CompareTo("06/20") <= 0)
                return CalendarSeason.Spring;

            else if (today.CompareTo("06/21") >= 0 && today.CompareTo("09/20") <= 0)
                return CalendarSeason.Summer;

            else if (today.CompareTo("09/21") >= 0 && today.CompareTo("12/20") <= 0)
                return CalendarSeason.Autumn;

            else
                return CalendarSeason.Winter;
        }
    }
}