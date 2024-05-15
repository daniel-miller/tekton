namespace Common.Contract
{
    public class ValidationError : Error
    {
        public string Property { get; set; }
    }
}