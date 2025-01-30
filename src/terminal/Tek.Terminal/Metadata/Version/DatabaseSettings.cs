using Spectre.Console.Cli;

public class DatabaseSettings : CommandSettings
{
    [CommandOption("--host")]
    public string? Host { get; set; }

    [CommandOption("--port")]
    public int? Port { get; set; }

    [CommandOption("--database")]
    public string? Database { get; set; }

    [CommandOption("--user")]
    public string? User { get; set; }

    [CommandOption("--password")]
    public string? Password { get; set; }
}