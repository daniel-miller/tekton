
using Spectre.Console.Cli;

public class DatabaseSettings : CommandSettings
{
    [CommandOption("--host")]
    public string? Host { get; set; }

    [CommandOption("--port")]
    public int? Port { get; set; }

    [CommandOption("--user")]
    public string? User { get; set; }

    [CommandOption("--password")]
    public string? Password { get; set; }

    [CommandOption("--database")]
    public string? Database { get; set; }
}

public class UpgradeDatabaseSettings : DatabaseSettings
{
    [CommandOption("--definitions")]
    public bool Definitions { get; set; } = true;

    [CommandOption("--manipulations")]
    public bool Manipulations { get; set; } = true;

    [CommandOption("--randomizations")]
    public bool Randomizations { get; set; } = true;

    internal void Copy(DatabaseSettings settings)
    {
        Host = settings.Host;
        Port = settings.Port;

        User = settings.User;
        Password = settings.Password;

        Database = settings.Database;
    }
}