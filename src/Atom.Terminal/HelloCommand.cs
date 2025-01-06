using Atom.Common;

using Spectre.Console.Cli;

public class HelloCommand : Command<HelloSettings>
{
    public override int Execute(CommandContext context, HelloSettings settings)
    {
        Text hello = new Text();

        hello["en"] = "Hello";
        hello["fr"] = "Bonjour";
        hello["it"] = "Salve";

        var language = settings.Language ?? Text.DefaultLanguage;

        Console.WriteLine($"{hello[language]}! Atomic can say Hello in {hello.Count} languages: {string.Join(", ", hello.Languages)}.");

        return 0;
    }
}

public class HelloSettings : CommandSettings
{
    [CommandOption("--language")]
    public string? Language { get; set; }
}
