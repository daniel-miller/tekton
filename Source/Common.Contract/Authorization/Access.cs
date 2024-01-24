using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Contract.Security
{
    public class Access
    {
        public AccessFunctions Functions { get; set; }
        public Dictionary<string, Function> Metadata { get; internal set; } = new Dictionary<string, Function>();

        public bool Execute => Functions.HasFlag(AccessFunctions.Execute);
        public bool Read => Functions.HasFlag(AccessFunctions.Read);
        public bool Write => Functions.HasFlag(AccessFunctions.Write);
        public bool Create => Functions.HasFlag(AccessFunctions.Create);
        public bool Delete => Functions.HasFlag(AccessFunctions.Delete);
        public bool Administrate => Functions.HasFlag(AccessFunctions.Administrate);
        public bool Configure => Functions.HasFlag(AccessFunctions.Configure);

        public string Abbreviate()
        {
            if (!AnyGranted())
                return "-";

            var functions = new StringBuilder();

            if (Execute)
                functions.Append(Metadata[nameof(Execute)].Slug);

            if (Read)
                functions.Append(Metadata[nameof(Read)].Slug);

            if (Write)
                functions.Append(Metadata[nameof(Write)].Slug);

            if (Create)
                functions.Append(Metadata[nameof(Create)].Slug);

            if (Delete)
                functions.Append(Metadata[nameof(Delete)].Slug);

            if (Administrate)
                functions.Append(Metadata[nameof(Administrate)].Slug);

            if (Configure)
                functions.Append(Metadata[nameof(Configure)].Slug);

            return functions.ToString();
        }

        public string Describe()
        {
            if (!AnyGranted())
                return "-";

            var functions = new List<string>();

            if (Execute)
                functions.Add(nameof(Execute));

            if (Read)
                functions.Add(nameof(Read));

            if (Write)
                functions.Add(nameof(Write));

            if (Create)
                functions.Add(nameof(Create));

            if (Delete)
                functions.Add(nameof(Delete));

            if (Administrate)
                functions.Add(nameof(Administrate));

            if (Configure)
                functions.Add(nameof(Configure));

            return string.Join(", ", functions);
        }

        public bool AllGranted()
        {
            return Execute && Read && Write && Create && Delete && Administrate && Configure;
        }

        public bool AnyGranted()
        {
            return Execute || Read || Write || Create || Delete || Administrate || Configure;
        }

        public Function GetFunction(string function)
        {
            if (!Metadata.ContainsKey(function))
                throw new ArgumentOutOfRangeException(nameof(function), $"Your parameter value here is unexpected ({function}).");

            return Metadata[function];
        }

        public Function GetFunction(AccessFunctions function)
        {
            if (!Enum.IsDefined(typeof(AccessFunctions), function))
                throw new ArgumentOutOfRangeException(nameof(function), $"Your parameter value here specifies multiple values. This method supports single-value input parameters only.");

            var key = function.ToString();

            if (!Metadata.ContainsKey(key))
                throw new ArgumentOutOfRangeException(nameof(function), $"Your parameter value here is unexpected ({key}).");

            return Metadata[key];
        }

        public Guid GetIdentifier(string function)
        {
            if (!Metadata.ContainsKey(function))
                throw new ArgumentOutOfRangeException(nameof(function), $"Your parameter value here is unexpected ({function}).");

            return Metadata[function].Identifier;
        }

        public bool IsGranted(string function)
        {
            switch (function)
            {
                case nameof(Execute):
                    return Execute;

                case nameof(Read):
                    return Read;

                case nameof(Write):
                    return Write;

                case nameof(Create):
                    return Create;

                case nameof(Delete):
                    return Delete;

                case nameof(Administrate):
                    return Administrate;

                case nameof(Configure):
                    return Configure;

                default:
                    return false;
            }
        }

        public Access()
        {
            Metadata = new Dictionary<string, Function>
            {
                {
                    nameof(Execute), new Function
                    {
                        Identifier = new Guid("63a8a35d-2c64-49b2-99ee-fc6e64a2e3cd"),
                        Name = nameof(Execute),
                        Slug = "x"
                    }
                },
                {
                    nameof(Read), new Function
                    {
                        Identifier = new Guid("3ecc66a6-dcdf-4e64-a411-adc3e89aa9ed"),
                        Name = nameof(Read),
                        Slug = "r"
                    }
                },
                {
                    nameof(Write), new Function
                    {
                        Identifier = new Guid("1ed65a14-0f94-4265-bd61-54eed674b4f7"),
                        Name = nameof(Write),
                        Slug = "w"
                    }
                },
                {
                    nameof(Create), new Function
                    {
                        Identifier = new Guid("7233b05a-79d5-4d18-a9f5-0c7c65cd7bdb"),
                        Name = nameof(Create),
                        Slug = "c"
                    }
                },
                {
                    nameof(Delete), new Function
                    {
                        Identifier = new Guid("0882f080-33c0-4e1c-9096-10501ace4ccb"),
                        Name = nameof(Delete),
                        Slug = "d"
                    }
                },
                {
                    nameof(Administrate), new Function
                    {
                        Identifier = new Guid("aeb535bf-c018-4102-9b6c-780e4632dec9"),
                        Name = nameof(Administrate),
                        Slug = "a"
                    }
                },
                {
                    nameof(Configure), new Function
                    {
                        Identifier = new Guid("de01a2f3-be2a-4573-95fe-8e7bd64d2e01"),
                        Name = nameof(Configure),
                        Slug = "f"
                    }
                }
            };

            foreach(var value in Metadata.Values)
            {
                value.Category = FunctionCategory.Access.ToString();
            }
        }
    }
}