using System.Linq;

namespace Tek.Contract
{
    public static class Environments
    {
        public static EnvironmentModel[] All { get; set; }

        public static string[] Slugs
            => All.Select(x => x.Slug).ToArray();

        public static string[] Names
            => All.Select(x => x.Name.ToString()).ToArray();

        static Environments()
        {
            All = new[]
            {
                new EnvironmentModel(EnvironmentType.Local),
                new EnvironmentModel(EnvironmentType.Development),
                new EnvironmentModel(EnvironmentType.Sandbox),
                new EnvironmentModel(EnvironmentType.Production)
            };
        }
    }
}