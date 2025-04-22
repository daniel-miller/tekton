using System.Text;
using System.Text.RegularExpressions;

namespace Tek.Terminal;

public sealed class UpgradeScript
{
    private const string ScriptFileNamePattern = @"(Definition|Manipulation|Randomization)\\v(\d{2}\.\d{4}\.\d{4}\.\d{4}.*)\.sql$";
    private static Encoding DefaultEncoding => new UTF8Encoding();
    private static readonly Regex ScriptFileNameRegex = new Regex(ScriptFileNamePattern, RegexOptions.Compiled);

    public string? Type { get; private set; }
    public string? Name { get; private set; }

    public string Path { get; private set; }
    public string? Content { get; private set; }

    public bool IsLoaded { get; private set; }

    public UpgradeScript(string path)
    {
        Path = path;

        var match = ScriptFileNameRegex.Match(Path);

        if (!match.Success)
            return;

        Type = match.Groups[1].Value;
        Name = match.Groups[2].Value;

        Content = File.ReadAllText(path, DefaultEncoding);

        if (Content.IsEmpty())
            return;

        IsLoaded = true;
    }
}
