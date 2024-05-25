namespace Common.Test
{
    public class CriteriaTests
    {
        [Fact]
        public void Contructor_DefaultPage_Returns1()
        {
            var criteria = new Criteria();
            Assert.Equal(1, criteria.Page);
        }

        [Fact]
        public void Contructor_DefaultTake_Returns20()
        {
            var criteria = new Criteria();
            Assert.Equal(20, criteria.Take);
        }
    }
}