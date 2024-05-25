using Common.Timeline.Snapshots;

namespace Common.Timeline.Test
{
    public class SnapshotStrategyTests
    {
        [Fact]
        public void ShouldTakeSnapShot_ZeroChanges_ReturnsFalse()
        {
            var strategy = new SnapshotStrategy(1);

            var mock = new MockAggregate();

            Assert.False(strategy.ShouldTakeSnapShot(mock, 0));
        }

        [Fact]
        public void ShouldTakeSnapShot_ManyChanges_ReturnsTrue()
        {
            const int interval = 10;

            var strategy = new SnapshotStrategy(interval);

            var mock = new MockAggregate();
            for (var i = 0; i < interval + 1; i++)
                mock.Apply(new MockChange(i));

            Assert.True(strategy.ShouldTakeSnapShot(mock, mock.AggregateVersion));
        }

        [Fact]
        public void ShouldTakeSnapShot_SomeChanges_ReturnsFalse()
        {
            const int interval = 10;

            var strategy = new SnapshotStrategy(interval);

            var mock = new MockAggregate();
            for (var i = 0; i < interval - 1; i++)
                mock.Apply(new MockChange(i));

            Assert.False(strategy.ShouldTakeSnapShot(mock, mock.AggregateVersion));
        }
    }
}