using Dapper;

namespace Tek.Service.Security
{
    public class PasswordSearch : PostgresDbContext
    {
        public PasswordSearch(DatabaseConnectionSettings settings) : base(settings) { }

        public async Task<PasswordEntity?> FetchAsync(Guid passwordId)
        {
            const string sql = "SELECT * FROM security.t_password WHERE password_id = @PasswordId";
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<PasswordEntity>(sql, new { PasswordId = passwordId });
        }

        public async Task<IEnumerable<PasswordEntity>> CollectAsync()
        {
            const string sql = "SELECT * FROM security.t_password";
            using var connection = CreateConnection();
            return await connection.QueryAsync<PasswordEntity>(sql);
        }
    }

    public class PasswordStore : PostgresDbContext
    {
        public PasswordStore(DatabaseConnectionSettings settings) : base(settings) { }
    
        public async Task<int> InsertAsync(PasswordEntity password)
        {
            const string sql = @"
            INSERT INTO security.t_password (
                password_id, email_id, email_address, password_hash, password_expiry,
                default_plaintext, default_expiry, created_when, last_forgotten_when, last_modified_when
            )
            VALUES (
                @PasswordId, @EmailId, @EmailAddress, @PasswordHash, @PasswordExpiry,
                @DefaultPlaintext, @DefaultExpiry, @CreatedWhen, @LastForgottenWhen, @LastModifiedWhen
            )";

            using var connection = CreateConnection();
            return await connection.ExecuteAsync(sql, password);
        }

        public async Task<int> UpdateAsync(PasswordEntity password)
        {
            const string sql = @"
            UPDATE security.t_password 
            SET 
                email_id = @EmailId,
                email_address = @EmailAddress,
                password_hash = @PasswordHash,
                password_expiry = @PasswordExpiry,
                default_plaintext = @DefaultPlaintext,
                default_expiry = @DefaultExpiry,
                created_when = @CreatedWhen,
                last_forgotten_when = @LastForgottenWhen,
                last_modified_when = @LastModifiedWhen
            WHERE password_id = @PasswordId";

            using var connection = CreateConnection();
            return await connection.ExecuteAsync(sql, password);
        }

        public async Task<int> DeleteAsync(Guid passwordId)
        {
            const string sql = "DELETE FROM security.t_password WHERE password_id = @PasswordId";
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(sql, new { PasswordId = passwordId });
        }
    }

    public class PasswordEntity
    {
        public Guid PasswordId { get; set; }
        public Guid EmailId { get; set; }
        public string? EmailAddress { get; set; }
        public string PasswordHash { get; set; } = null!;
        public DateTimeOffset PasswordExpiry { get; set; }
        public string? DefaultPlaintext { get; set; }
        public DateTimeOffset? DefaultExpiry { get; set; }
        public DateTimeOffset CreatedWhen { get; set; }
        public DateTimeOffset? LastForgottenWhen { get; set; }
        public DateTimeOffset? LastModifiedWhen { get; set; }
    }
}