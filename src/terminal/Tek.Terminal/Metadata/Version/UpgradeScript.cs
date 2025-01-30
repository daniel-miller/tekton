using System.Text;
using System.Text.RegularExpressions;

using Tek.Common;

public sealed class UpgradeScript
{
    private const string ScriptFileNamePattern = @"^v(\d{2}\.\d{4}\.\d{4}\.\d{4})(.*)\.sql$";
    private static Encoding DefaultEncoding => new UTF8Encoding();
    private static readonly Regex ScriptFileNameRegex = new Regex(ScriptFileNamePattern, RegexOptions.Compiled);

    public string File { get; private set; }
    public string Name { get; private set; }
    public string? Content { get; private set; }
    public string? Version { get; private set; }
    public bool IsLoaded { get; private set; }

    public UpgradeScript(string path)
    {
        File = path;

        Name = Path.GetFileName(path);

        var match = ScriptFileNameRegex.Match(Name);

        if (!match.Success)
            return;

        Version = match.Groups[1].Value;

        Content = System.IO.File.ReadAllText(path, DefaultEncoding);

        if (Content.IsEmpty())
            return;

        IsLoaded = true;
    }
}
