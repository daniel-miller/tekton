namespace Tek.Base
{
    public class Access
    {
        public AccessType Type { get; set; }
        public BasicAccess Basic { get; set; }
        public DataAccess Data { get; set; }
        public HttpAccess Http { get; set; }
    }
}