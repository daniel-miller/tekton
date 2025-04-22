using System;

namespace Tek.Contract
{
    public class CreateVersion
    {
        public string ScriptContent { get; set; }
        public string ScriptPath { get; set; }
        public string VersionName { get; set; }
        public string VersionType { get; set; }

        public int VersionNumber { get; set; }

        public DateTime ScriptExecuted { get; set; }
    }

    public class ModifyVersion
    {
        public string ScriptContent { get; set; }
        public string ScriptPath { get; set; }
        public string VersionName { get; set; }
        public string VersionType { get; set; }

        public int VersionNumber { get; set; }

        public DateTime ScriptExecuted { get; set; }
    }

    public class DeleteVersion
    {
        public int VersionNumber { get; set; }
    }
}