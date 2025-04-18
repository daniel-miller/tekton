using System;
using System.Collections.Generic;

namespace Tek.Contract
{
    public class AssertSecret : Query<bool>
    {
        public Guid SecretId { get; set; }
    }

    public class FetchSecret : Query<SecretModel>
    {
        public Guid SecretId { get; set; }
    }

    public class CollectSecrets : Query<IEnumerable<SecretModel>>, ISecretCriteria
    {
        public Guid PasswordId { get; set; }

        public string SecretName { get; set; }
        public string SecretScope { get; set; }
        public string SecretType { get; set; }
        public string SecretValue { get; set; }

        public int? SecretLimetimeLimit { get; set; }

        public DateTime SecretExpiry { get; set; }
    }

    public class CountSecrets : Query<int>, ISecretCriteria
    {
        public Guid PasswordId { get; set; }

        public string SecretName { get; set; }
        public string SecretScope { get; set; }
        public string SecretType { get; set; }
        public string SecretValue { get; set; }

        public int? SecretLimetimeLimit { get; set; }

        public DateTime SecretExpiry { get; set; }
    }

    public class SearchSecrets : Query<IEnumerable<SecretMatch>>, ISecretCriteria
    {
        public Guid PasswordId { get; set; }

        public string SecretName { get; set; }
        public string SecretScope { get; set; }
        public string SecretType { get; set; }
        public string SecretValue { get; set; }

        public int? SecretLimetimeLimit { get; set; }

        public DateTime SecretExpiry { get; set; }
    }

    public interface ISecretCriteria
    {
        Filter Filter { get; set; }
        
        Guid PasswordId { get; set; }

        string SecretName { get; set; }
        string SecretScope { get; set; }
        string SecretType { get; set; }
        string SecretValue { get; set; }

        int? SecretLimetimeLimit { get; set; }

        DateTime SecretExpiry { get; set; }
    }

    public partial class SecretMatch
    {
        public Guid SecretId { get; set; }
    }

    public partial class SecretModel
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
}