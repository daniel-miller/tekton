using Microsoft.Extensions.Configuration;

namespace Tek.Service
{
    public static class AppSettingsHelper
    {
        public static T GetSettings<T>(string name)
        {
            var configuration = BuildConfiguration();
            var section = configuration.GetRequiredSection(name);
            var settings = section.Get<T>();
            return settings!;
        }

        public static IConfigurationRoot BuildConfiguration()
        {
            var path = AppContext.BaseDirectory;

            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            AddLocalSettings(builder, path);

            return builder.Build();
        }

        private static void AddLocalSettings(IConfigurationBuilder builder, string path)
        {
            if (AddLocalFile(builder, path))
                return;

            if (AddLocalFile(builder, Path.Combine(path, "..")))
                return;

            if (AddLocalFile(builder, Path.Combine(path, "..", "..")))
                return;

            AddLocalFile(builder, Path.Combine(path, "..", "..", ".."));
        }

        private static bool AddLocalFile(IConfigurationBuilder builder, string folder)
        {
            var file = Path.Combine(folder, "appsettings.local.json");

            if (!File.Exists(file))
                return false;

            builder = builder.AddJsonFile(file);

            return true;
        }
    }
}
