using Spectre.Console.Cli;

public class Application
{
    public Application()
    {
        
    }

    public async Task<int> RunAsync(string[] args)
    {
        var app = new CommandApp();

        app.Configure(config =>
        {
            config.AddCommand<HelloCommand>("hello");
            config.AddCommand<GenerationCommand>("generation");
        });

        return app.Run(args);
    }
}