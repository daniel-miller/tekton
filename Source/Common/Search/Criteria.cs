namespace Common
{
    public class Criteria
    {
        public int Page { get; set; } = 1;
        public int Take { get; set; } = 20;

        public string[] Sort { get; set; }
        public string[] Filter { get; set; }
    }
}