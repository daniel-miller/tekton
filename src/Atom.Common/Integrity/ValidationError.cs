namespace Atom.Common
{
    public class ValidationError : Error
    {
        public string Property { get; set; }
    }
}