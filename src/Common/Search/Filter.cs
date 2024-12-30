namespace Common
{
    public class Filter
    {
        public int Page { get; set; } = 1;
        public int Take { get; set; } = 20;

        public string Sort { get; set; }

        public string Excludes { get; set; }
        public string Includes { get; set; }
    }
}