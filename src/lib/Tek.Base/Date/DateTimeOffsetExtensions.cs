using System;

namespace Tek.Base
{
    public static class DateTimeOffsetExtensions
    {
        public static bool IsEmpty(DateTimeOffset? dto)
            => dto == null || dto == DateTimeOffset.MinValue;
    }
}