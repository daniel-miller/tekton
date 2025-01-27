using System.Collections.Generic;

namespace Tek.Contract
{
    public class TektonSettings
    {
        public ReleaseSettings Release { get; set; }
        public KernelSettings Kernel { get; set; }
        public ModuleSettings Module { get; set; }
        public PluginSettings Plugin { get; set; }
        public PackageSettings Package { get; set; }
        public SecuritySettings Security { get; set; }
    }

    public class SecuritySettings
    {
        public string Domain { get; set; }
        public List<PermissionBundle> Permissions { get; set; }
        public SentinelsSettings[] Sentinels { get; set; }
        public TokenSettings Token { get; set; }
    }

    public class PermissionBundle
    {
        public string Type { get; set; }
        public string Access { get; set; }
        public List<string> Resources { get; set; }
        public List<string> Roles { get; set; }

        public PermissionBundle()
        {
            Resources = new List<string>();
            Roles = new List<string>();
        }
    }

    public class SentinelsSettings
    {
        public Actor Actor { get; set; }
        public string[] Roles { get; set; }
    }

    public class PackageSettings
    {
        public TekApiSettings TekApi { get; set; }
    }

    public class TekApiSettings
    {
        
    }

    public class TokenSettings
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Whitelist { get; set; }
        public int Lifetime { get; set; }
    }

    public class ReleaseSettings
    {
        public string Directory { get; set; }
        public string Environment { get; set; }
        public string Secret { get; set; }
        public string Version { get; set; }
    }

    public class KernelSettings
    {
        public TelemetrySettings Telemetry { get; set; }
    }

    public class ModuleSettings
    {
        public ContactSettings Contact { get; set; }
    }

    public class ContactSettings
    {
        
    }

    public class TelemetrySettings
    {
        public LogSettings Logging { get; set; }
        public MonitorSettings Monitoring { get; set; }
    }

    public class LogSettings
    {
        public string Path { get; set; }
    }

    public class MonitorSettings
    {
        public bool Debug { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
        public double? Rate { get; set; }
    }

    public class PluginSettings
    {
        public IntegrationSettings Integration { get; set; }
    }

    public class IntegrationSettings
    {
        public AstronomyApiSettings AstronomyApi { get; set; }
        public VisualCrossingSettings VisualCrossing { get; set; }
    }

    public class AstronomyApiSettings
    {
        public string Host { get; set; }
        public string App { get; set; }
        public string Secret { get; set; }
    }

    public class VisualCrossingSettings
    {
        public string Host { get; set; }
        public string Secret { get; set; }
    }
}
