using System.ComponentModel;

using Spectre.Console.Cli;

namespace Tek.Terminal;

[Description("Say hello to the terminal.")]
public class StatusCommand : Command<StatusSettings>
{
    private readonly ReleaseSettings _releaseSettings;

    public StatusCommand(ReleaseSettings releaseSettings)
    {
        _releaseSettings = releaseSettings;
    }

    public override int Execute(CommandContext context, StatusSettings settings)
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

        var version = typeof(StatusCommand).Assembly.GetName().Version;
        var status = $"Tekton Terminal version {version} is online. The {_releaseSettings.Environment} environment says {hello[language]}."
            + $" The terminal can say Hello in these {hello.Count} languages: {string.Join(", ", hello.Languages)}.";

        Output(status);

        return 0;
    }

    private void Output(string line)
    {
        Spectre.Console.AnsiConsole.WriteLine(line);
    }
}

public class StatusSettings : CommandSettings
{
    [CommandOption("--language")]
    public string? Language { get; set; }
}
