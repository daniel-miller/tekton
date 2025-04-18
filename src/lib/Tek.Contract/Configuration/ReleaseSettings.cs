namespace Tek.Contract
{
    public class ReleaseSettings
    {
        public string Directory { get; set; }
        public string Environment { get; set; }
        public string Version { get; set; }

        public EnvironmentModel GetEnvironment()
            => new EnvironmentModel(Environment);

        public bool IsLocal()
            => GetEnvironment().Name == EnvironmentName.Local;
    }
}
