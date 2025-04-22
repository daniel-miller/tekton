namespace Tek.Api;

public interface IClientLockService
{
    bool IsLocked(string ipAddress);
    ClientLockStatus Success(string ipAddress);
    ClientLockStatus Fail(string ipAddress);
}