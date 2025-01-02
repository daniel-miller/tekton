namespace Atomic.Common
{
    public interface IPrincipalContext
    {
        Principal Current { get; }
    }
}