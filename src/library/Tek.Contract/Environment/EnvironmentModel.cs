using System;

namespace Tek.Contract
{
    public class EnvironmentModel : Model
    {
        new public EnvironmentName Name { get; set; }

        public EnvironmentModel(EnvironmentName name)
        {
            Initialize(name);
        }

        public EnvironmentModel(string name)
        {
            if (!Enum.TryParse<EnvironmentName>(name, true, out var environment))
                throw new ArgumentException($"The environment name {name} is not recognized.", nameof(name));

            Initialize(environment);
        }

        private void Initialize(EnvironmentName name)
        {
            Name = name;

            switch (Name)
            {
                case EnvironmentName.Production:
                    Type = Name.ToString();
                    Slug = "prod";
                    break;

                case EnvironmentName.Sandbox:
                    Type = Name.ToString();
                    Slug = "sandbox";
                    break;

                case EnvironmentName.Development:
                    Type = Name.ToString();
                    Slug = "dev";
                    break;

                case EnvironmentName.Local:
                default:
                    Type = Name.ToString();
                    Slug = "local";
                    break;
            }
        }
    }
}