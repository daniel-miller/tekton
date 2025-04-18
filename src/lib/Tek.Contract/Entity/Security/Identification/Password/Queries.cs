using System;
using System.Collections.Generic;

namespace Tek.Contract
{
    public class AssertPassword : Query<bool>
    {
        public Guid PasswordId { get; set; }
    }

    public class FetchPassword : Query<PasswordModel>
    {
        public Guid PasswordId { get; set; }
    }

    public class CollectPasswords : Query<IEnumerable<PasswordModel>>, IPasswordCriteria
    {
        public Guid EmailId { get; set; }

        public string DefaultPlaintext { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }

        public DateTime CreatedWhen { get; set; }
        public DateTime? DefaultExpiry { get; set; }
        public DateTime? LastForgottenWhen { get; set; }
        public DateTime? LastModifiedWhen { get; set; }
        public DateTime PasswordExpiry { get; set; }
    }

    public class CountPasswords : Query<int>, IPasswordCriteria
    {
        public Guid EmailId { get; set; }

        public string DefaultPlaintext { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }

        public DateTime CreatedWhen { get; set; }
        public DateTime? DefaultExpiry { get; set; }
        public DateTime? LastForgottenWhen { get; set; }
        public DateTime? LastModifiedWhen { get; set; }
        public DateTime PasswordExpiry { get; set; }
    }

    public class SearchPasswords : Query<IEnumerable<PasswordMatch>>, IPasswordCriteria
    {
        public Guid EmailId { get; set; }

        public string DefaultPlaintext { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }

        public DateTime CreatedWhen { get; set; }
        public DateTime? DefaultExpiry { get; set; }
        public DateTime? LastForgottenWhen { get; set; }
        public DateTime? LastModifiedWhen { get; set; }
        public DateTime PasswordExpiry { get; set; }
    }

    public interface IPasswordCriteria
    {
        Filter Filter { get; set; }
        
        Guid EmailId { get; set; }

        string DefaultPlaintext { get; set; }
        string EmailAddress { get; set; }
        string PasswordHash { get; set; }

        DateTime CreatedWhen { get; set; }
        DateTime? DefaultExpiry { get; set; }
        DateTime? LastForgottenWhen { get; set; }
        DateTime? LastModifiedWhen { get; set; }
        DateTime PasswordExpiry { get; set; }
    }

    public partial class PasswordMatch
    {
        public Guid PasswordId { get; set; }
    }

    public partial class PasswordModel
    {
        public Guid EmailId { get; set; }
        public Guid PasswordId { get; set; }

        public string DefaultPlaintext { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }

        public DateTime CreatedWhen { get; set; }
        public DateTime? DefaultExpiry { get; set; }
        public DateTime? LastForgottenWhen { get; set; }
        public DateTime? LastModifiedWhen { get; set; }
        public DateTime PasswordExpiry { get; set; }
    }
}