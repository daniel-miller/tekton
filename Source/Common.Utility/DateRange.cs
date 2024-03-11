using System;

namespace Common.Utility
{
    public class DateRange
    {
        public DateRange() { }

        public DateRange(DateRangeType type, DateTimeOffset when) 
        {
            var calendar = new Calendar();

            Until = when;

            switch (type)
            {
                case DateRangeType.Today:
                    Since = when.Date;
                    break;

                case DateRangeType.ThisWeek:
                    Since = calendar.GetStartOfWeek(when);
                    break;

                case DateRangeType.ThisMonth:
                    Since = calendar.GetStartOfMonth(when);
                    break;

                case DateRangeType.ThisYear:
                    Since = calendar.GetStartOfYear(when);
                    break;

                case DateRangeType.Yesterday:
                    Since = when.AddDays(-1).Date;
                    Until = when.Date;
                    break;

                case DateRangeType.LastWeek:
                    Since = calendar.GetStartOfWeek(when.AddDays(-7));
                    Until = calendar.GetStartOfWeek(when);
                    break;

                case DateRangeType.LastMonth:
                    Since = calendar.GetStartOfMonth(when.AddMonths(-1));
                    Until = calendar.GetStartOfMonth(when);
                    break;

                case DateRangeType.LastYear:
                    Since = calendar.GetStartOfYear(when.AddYears(-1));
                    Until = calendar.GetStartOfYear(when);
                    break;
            }
        }

        public DateTimeOffset? After { get; set; }
        public DateTimeOffset? Before { get; set; }
        public DateTimeOffset? Since { get; set; }
        public DateTimeOffset? Until { get; set; }
    }
}