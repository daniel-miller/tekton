namespace Atomic.Common
{
    public interface IPrincipalSearch
    {
        Principal GetBySecret(string secret);
    }
}