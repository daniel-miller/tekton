using System;

namespace Tek.Contract
{
    public class CreateSecret
    {
        public Guid PasswordId { get; set; }
        public Guid SecretId { get; set; }

        public string SecretName { get; set; }
        public string SecretScope { get; set; }
        public string SecretType { get; set; }
        public string SecretValue { get; set; }

        public int? SecretLimetimeLimit { get; set; }

        public DateTime SecretExpiry { get; set; }
    }

    public class ModifySecret
    {
        public Guid PasswordId { get; set; }
        public Guid SecretId { get; set; }

        public string SecretName { get; set; }
        public string SecretScope { get; set; }
        public string SecretType { get; set; }
        public string SecretValue { get; set; }

        public int? SecretLimetimeLimit { get; set; }

        public DateTime SecretExpiry { get; set; }
    }

    public class DeleteSecret
    {
        public Guid SecretId { get; set; }
    }
}