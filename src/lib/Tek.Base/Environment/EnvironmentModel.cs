using System;

namespace Tek.Base
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
                case EnvironmentName.Live:
                    Type = Name.ToString();
                    Slug = "live";
                    break;

                case EnvironmentName.Stage:
                    Type = Name.ToString();
                    Slug = "stage";
                    break;

                case EnvironmentName.QA:
                    Type = Name.ToString();
                    Slug = "qa";
                    break;

                case EnvironmentName.Development:
                default:
                    Type = Name.ToString();
                    Slug = "dev";
                    break;
            }
        }
    }
}