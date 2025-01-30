namespace Tek.Contract
{
    public class TektonSettings
    {
        public KernelSettings Kernel { get; set; }

        public MetadataSettings Metadata { get; set; }
        
        public PluginSettings Plugin { get; set; }

        public SecuritySettings Security { get; set; }
    }
}
