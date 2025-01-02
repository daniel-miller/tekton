using System;

namespace Atomic.Common
{
    public class DateRange
    {
        /// <remarks>
        /// By default, a date range is a half-open interval, where the lower bound is closed and the upper bound is 
        /// open. The interval notation for this is [lower,upper). The inequality notation is lower &lte; x &lt; upper. 
        /// We use the term "since" to indicate the closed endpoint for the lower bound on a date range, and we use the 
        /// term "until" to indicate the closed endpoint for the upper bound. We use the terms "before" and "after" for
        /// the open endpoints on upper and lower bounds, respectively. (These terms are the same used on the git 
        /// command line for date-related queries.)
        /// </remarks>
        public DateRange() 
        {
            LowerBound = IntervalType.Closed;
            UpperBound = IntervalType.Open;
        }

        public DateRange(IntervalType lower, IntervalType upper)
        {
            LowerBound = lower;
            UpperBound = upper;
        }

        public void Clear()
        {
            After = null;
            Before = null;
            Since = null;
            Until = null;
        }

        public void Set(DateTimeOffset? a, DateTimeOffset? b)
        {
            if (LowerBound == IntervalType.Open)
                After = a;

            else if (LowerBound == IntervalType.Closed)
                Since = a;

            if (UpperBound == IntervalType.Open)
                Before = b;

            else if (UpperBound == IntervalType.Closed)
                Until = b;
        }

        public void Set(DateRangeType type, DateTimeOffset when)
        {
            Type = type;

            var calendar = new Calendar();

            DateTimeOffset? a = null;
            DateTimeOffset? b = null;

            switch (type)
            {
                case DateRangeType.Today:
                    a = when.Date;
                    b = a.Value.AddDays(1);
                    break;

                case DateRangeType.ThisWeek:
                    a = calendar.GetStartOfWeek(when);
                    b = a.Value.AddDays(7);
                    break;

                case DateRangeType.ThisMonth:
                    a = calendar.GetStartOfMonth(when);
                    b = a.Value.AddMonths(1);
                    break;

                case DateRangeType.ThisYear:
                    a = calendar.GetStartOfYear(when);
                    b = a.Value.AddYears(1);
                    break;

                case DateRangeType.Yesterday:
                    a = when.AddDays(-1).Date;
                    b = a.Value.AddDays(1);
                    break;

                case DateRangeType.LastWeek:
                    a = calendar.GetStartOfWeek(when.AddDays(-7));
                    b = a.Value.AddDays(7);
                    break;

                case DateRangeType.LastMonth:
                    a = calendar.GetStartOfMonth(when.AddMonths(-1));
                    b = a.Value.AddMonths(1);
                    break;

                case DateRangeType.LastYear:
                    a = calendar.GetStartOfYear(when.AddYears(-1));
                    b = a.Value.AddYears(1);
                    break;
            }

            Set(a, b);
        }

        public DateRangeType Type { get; set; }

        public DateTimeOffset? After { get; set; }
        public DateTimeOffset? Before { get; set; }
        public DateTimeOffset? Since { get; set; }
        public DateTimeOffset? Until { get; set; }

        public IntervalType LowerBound { get; set; }
        public IntervalType UpperBound { get; set; }
    }
}