namespace Tek.Contract
{
    public class DatabaseConnectionSettings
    {
        public const string DefaultHost = "localhost";

        public const string DefaultDatabase = "postgres";

        public const int DefaultPort = 5432;

        public string Host { get; set; } = DefaultHost;

        public int Port { get; set; } = DefaultPort;

        public string Database { get; set; } = DefaultDatabase;

        public string User { get; set; }

        public string Password { get; set; }
    }
}