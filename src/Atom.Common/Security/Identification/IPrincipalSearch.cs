namespace Atom.Common
{
    public interface IPrincipalSearch
    {
        Principal GetBySecret(string secret);
    }
}