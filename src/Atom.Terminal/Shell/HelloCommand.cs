using System.ComponentModel;

using Atom.Common;

using Spectre.Console.Cli;

[Description("Say hello to the terminal.")]
public class HelloCommand : Command<HelloSettings>
{
    public override int Execute(CommandContext context, HelloSettings settings)
    {
        var hello = new Text();

        hello["ar"] = "السلام عليكم";
        hello["de"] = "Guten Tag";
        hello["el"] = "Γειά σας";
        hello["en"] = "Hello";
        hello["es"] = "Hola";
        hello["fr"] = "Bonjour";
        hello["hi"] = "नमस्ते";
        hello["it"] = "Salve";
        hello["ja"] = "こんにちは";
        hello["ko"] = "안녕하세요";
        hello["pt"] = "Olá";
        hello["ru"] = "Здравствуйте";

        var language = settings.Language ?? Text.DefaultLanguage;

        Output($"{hello[language]}. The terminal can say Hello in {hello.Count} languages: {string.Join(", ", hello.Languages)}.");

        return 0;
    }

    private void Output(string line)
    {
        Spectre.Console.AnsiConsole.WriteLine(line);
    }
}

public class HelloSettings : CommandSettings
{
    [CommandOption("--language")]
    public string? Language { get; set; }
}
