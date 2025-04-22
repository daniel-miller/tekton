namespace Tek.Service.Metadata;

public partial class TVersionEntity
{
    public string ScriptContent { get; set; } = null!;
    public string ScriptPath { get; set; } = null!;
    public string VersionName { get; set; } = null!;
    public string VersionType { get; set; } = null!;

    public int VersionNumber { get; set; }

    public DateTime ScriptExecuted { get; set; }
}