namespace Tek.Service.Security;

public partial class TSecretEntity
{
    public Guid PasswordId { get; set; }
    public Guid SecretId { get; set; }

    public string SecretName { get; set; } = null!;
    public string SecretScope { get; set; } = null!;
    public string SecretType { get; set; } = null!;
    public string SecretValue { get; set; } = null!;

    public int? SecretLimetimeLimit { get; set; }

    public DateTime SecretExpiry { get; set; }
}