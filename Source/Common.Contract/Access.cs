using System.Collections.Generic;
using System.Text;

namespace Common.Contract
{
    public class Access
    {
        public bool Execute { get; set; }

        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Create { get; set; }
        public bool Delete { get; set; }

        public bool Administrate { get; set; }
        public bool Configure { get; set; }

        public string Abbreviate()
        {
            if (!AnyGranted())
                return "-";

            var operations = new StringBuilder();

            if (Execute)
                operations.Append("X");

            if (Read)
                operations.Append("R");

            if (Write)
                operations.Append("W");

            if (Create)
                operations.Append("C");

            if (Delete)
                operations.Append("D");

            if (Administrate)
                operations.Append("A");

            if (Configure)
                operations.Append("F");

            return operations.ToString();
        }

        public string Describe()
        {
            if (!AnyGranted())
                return "-";

            var operations = new List<string>();

            if (Execute)
                operations.Add("Execute");

            if (Read)
                operations.Add("Read");

            if (Write)
                operations.Add("Write");

            if (Create)
                operations.Add("Create");

            if (Delete)
                operations.Add("Delete");

            if (Administrate)
                operations.Add("Administrate");

            if (Configure)
                operations.Add("Configure");

            return string.Join(", ", operations);
        }

        public bool AllGranted()
        {
            return Execute && Read && Write && Create && Delete && Administrate && Configure;
        }

        public bool AnyGranted()
        {
            return Execute || Read || Write || Create || Delete || Administrate || Configure;
        }

        public bool IsGranted(string operation)
        {
            switch (operation)
            {
                case "Execute":
                    return Execute;
                case "Read":
                    return Read;
                case "Write":
                    return Write;
                case "Create":
                    return Create;
                case "Delete":
                    return Delete;
                case "Administrate":
                    return Administrate;
                case "Configure":
                    return Configure;
                default:
                    return false;
            }
        }
    }
}