namespace Common
{
    public interface IPrincipalContext
    {
        Principal Current { get; }
    }
}