using Spectre.Console.Cli;

public class DatabaseSettings : CommandSettings
{
    public const string DefaultHost = "localhost";

    public const string DefaultDatabase = "postgres";

    public const int DefaultPort = 5432;

    [CommandArgument(0, "<User>")]
    public string User { get; set; } = null!;

    [CommandArgument(1, "<Password>")]
    public string Password { get; set; } = null!;

    [CommandOption("--host")]
    public string Host { get; set; } = DefaultHost;

    [CommandOption("--port")]
    public int Port { get; set; } = DefaultPort;

    [CommandOption("--database")]
    public string Database { get; set; } = DefaultDatabase;
}