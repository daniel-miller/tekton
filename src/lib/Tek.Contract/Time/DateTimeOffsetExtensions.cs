using System;

namespace Tek.Contract
{
    public static class DateTimeOffsetExtensions
    {
        public static bool IsEmpty(DateTimeOffset? dto)
            => dto == null || dto == DateTimeOffset.MinValue;
    }
}