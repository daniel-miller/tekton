using System.ComponentModel;

using Spectre.Console.Cli;

namespace Tek.Terminal;

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
    [Description("Execute pending DDL (definition) scripts to create database objects.")]
    [CommandOption("-d|--definitions")]
    public bool Definitions { get; set; }

    [Description("Execute pending DML (manipulation) scripts to load and/or modify data in the database.")]
    [CommandOption("-m|--manipulations")]
    public bool Manipulations { get; set; }

    [Description("Execute pending randomization scripts to load and/or anonymize data in the database.")]
    [CommandOption("-r|--randomizations")]
    public bool Randomizations { get; set; }

    internal void Copy(DatabaseSettings settings)
    {
        Host = settings.Host;
        Port = settings.Port;

        User = settings.User;
        Password = settings.Password;

        Database = settings.Database;
    }
}