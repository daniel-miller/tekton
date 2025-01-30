using Spectre.Console.Cli;

public class GenerateUuidCommand : Command<GenerateUuidSettings>
{
    public override int Execute(CommandContext context, GenerateUuidSettings settings)
    {
        var count = settings.Count > 0 ? settings.Count : 1;

        for (var i = 0; i < count; i++)
            Console.WriteLine(UuidFactory.Create());
        
        return 0;
    }
}

public class GenerateUuidSettings : CommandSettings
{
    [CommandOption("--count")]
    public int? Count { get; set; }
}