using System.Linq;

namespace Atomic.Common
{
    public class EnvironmentModel : Model
    {
        public EnvironmentModel(EnvironmentName name)
        {
            switch (name)
            {
                case EnvironmentName.Local:
                    Type = name.ToString();
                    Name = name.ToString();
                    Slug = "local";
                    break;

                case EnvironmentName.Development:
                    Type = name.ToString();
                    Name = name.ToString();
                    Slug = "dev";
                    break;

                case EnvironmentName.Sandbox:
                    Type = name.ToString();
                    Name = name.ToString();
                    Slug = "sandbox";
                    break;

                case EnvironmentName.Production:
                    Type = name.ToString();
                    Name = name.ToString();
                    Slug = "prod";
                    break;
            }
        }
    }

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