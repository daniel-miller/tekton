using System;

namespace Tek.Contract
{
    public class CreatePassword
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

    public class ModifyPassword
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

    public class DeletePassword
    {
        public Guid PasswordId { get; set; }
    }
}