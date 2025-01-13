using Atom.Common;

using Spectre.Console.Cli;

public class RandomGuidCommand : Command<RandomSettings>
{
    public override int Execute(CommandContext context, RandomSettings settings)
    {
        for (var i = 0; i < settings.Count; i++)
            Console.WriteLine(GuidFactory.Create());
        
        return 0;
    }
}

public class RandomSettings : CommandSettings
{
    [CommandOption("--count")]
    public int? Count { get; set; }
}