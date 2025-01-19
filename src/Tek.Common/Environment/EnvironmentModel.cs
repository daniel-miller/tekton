namespace Tek.Common
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
}