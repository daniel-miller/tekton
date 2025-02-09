using System;

namespace Tek.Contract
{
    public class EnvironmentModel
    {
        public EnvironmentType Type { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        public EnvironmentModel(EnvironmentType type)
        {
            Initialize(type);
        }

        public EnvironmentModel(string type)
        {
            if (!Enum.TryParse<EnvironmentType>(type, true, out var environment))
                throw new ArgumentException($"The environment type {type} is not recognized.", nameof(type));

            Initialize(environment);
        }

        private void Initialize(EnvironmentType type)
        {
            Type = type;

            switch (Type)
            {
                case EnvironmentType.Production:
                    Name = Type.ToString();
                    Slug = "prod";
                    break;

                case EnvironmentType.Sandbox:
                    Name = Type.ToString();
                    Slug = "sandbox";
                    break;

                case EnvironmentType.Development:
                    Name = Type.ToString();
                    Slug = "dev";
                    break;

                case EnvironmentType.Local:
                default:
                    Name = Type.ToString();
                    Slug = "local";
                    break;
            }
        }
    }
}