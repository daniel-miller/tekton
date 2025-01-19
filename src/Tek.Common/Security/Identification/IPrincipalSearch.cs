namespace Tek.Common
{
    public interface IPrincipalSearch
    {
        Principal GetBySecret(string secret);
    }
}