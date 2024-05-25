namespace Common.Test
{
    public class DateRangeTests
    {
        [Fact]
        public void Constructor_Default_HalfOpenInterval()
        {
            var range = new DateRange();

            Assert.Equal(IntervalType.Closed, range.LowerBound);
            Assert.Equal(IntervalType.Open, range.UpperBound);
        }

        [Fact]
        public void Set_ThisMonth_HalfOpenInterval()
        {
            var MarchThird2024 = new DateTimeOffset(2024, 3, 3, 0, 0, 0, TimeSpan.Zero);
            var MarchFirst2024 = new DateTimeOffset(2024, 3, 1, 0, 0, 0, TimeSpan.Zero);
            var AprilFirst2024 = new DateTimeOffset(2024, 4, 1, 0, 0, 0, TimeSpan.Zero);

            var range = new DateRange();

            range.Set(DateRangeType.ThisMonth, MarchThird2024);

            Assert.Equal(IntervalType.Closed, range.LowerBound);
            Assert.Equal(IntervalType.Open, range.UpperBound);

            Assert.Equal(DateRangeType.ThisMonth, range.Type);

            Assert.Equal(MarchFirst2024, range.Since);
            Assert.Equal(AprilFirst2024, range.Before);
        }

        [Fact]
        public void Set_ThisWeek_HalfOpenInterval()
        {
            var MarchTwentyThird2024 = new DateTimeOffset(2024, 3, 22, 0, 0, 0, TimeSpan.Zero);
            var MarchSeventeenth2024 = new DateTimeOffset(2024, 3, 17, 0, 0, 0, TimeSpan.Zero);
            var MarchTwentyFourth2024 = new DateTimeOffset(2024, 3, 24, 0, 0, 0, TimeSpan.Zero);

            var range = new DateRange();

            range.Set(DateRangeType.ThisWeek, MarchTwentyThird2024);

            Assert.Equal(DateRangeType.ThisWeek, range.Type);

            Assert.Equal(MarchSeventeenth2024, range.Since);
            Assert.Equal(MarchTwentyFourth2024, range.Before);
        }

        [Fact]
        public void Set_ThisYear_OpenInterval()
        {
            var MarchTwentyThird2024 = new DateTimeOffset(2024, 3, 22, 0, 0, 0, TimeSpan.Zero);
            var JanuaryFirst2024 = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var JanuaryFirst2025 = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);

            var range = new DateRange(IntervalType.Open, IntervalType.Open);

            range.Set(DateRangeType.ThisYear, MarchTwentyThird2024);

            Assert.Equal(DateRangeType.ThisYear, range.Type);

            Assert.Equal(JanuaryFirst2024, range.After);
            Assert.Equal(JanuaryFirst2025, range.Before);
        }

        [Fact]
        public void Set_ArbitraryDates_ClosedInterval()
        {
            var MarchTwentyThird2024 = new DateTimeOffset(2024, 3, 22, 0, 0, 0, TimeSpan.Zero);
            var SeptemberNinth2024 = new DateTimeOffset(2024, 9, 9, 0, 0, 0, TimeSpan.Zero);

            var range = new DateRange(IntervalType.Closed, IntervalType.Closed);

            range.Set(MarchTwentyThird2024, SeptemberNinth2024);

            Assert.Equal(DateRangeType.None, range.Type);

            Assert.Equal(MarchTwentyThird2024, range.Since);
            Assert.Equal(SeptemberNinth2024, range.Until);
        }
    }
}