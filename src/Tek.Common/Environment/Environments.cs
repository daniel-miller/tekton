using System.Linq;

namespace Tek.Common
{
    public static class Environments
    {
        public static EnvironmentModel[] All { get; set; }

        public static string[] Slugs
            => All.Select(x => x.Slug).ToArray();

        public static string[] Names
            => All.Select(x => x.Name).ToArray();

        static Environments()
        {
            All = new[]
            {
                new EnvironmentModel(EnvironmentName.Local),
                new EnvironmentModel(EnvironmentName.Development),
                new EnvironmentModel(EnvironmentName.Sandbox),
                new EnvironmentModel(EnvironmentName.Production)
            };
        }
    }
}