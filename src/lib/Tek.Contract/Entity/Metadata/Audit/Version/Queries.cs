using System;
using System.Collections.Generic;

namespace Tek.Contract
{
    public class AssertVersion : Query<bool>
    {
        public int VersionNumber { get; set; }
    }

    public class FetchVersion : Query<VersionModel>
    {
        public int VersionNumber { get; set; }
    }

    public class CollectVersions : Query<IEnumerable<VersionModel>>, IVersionCriteria
    {
        public string ScriptContent { get; set; }
        public string ScriptPath { get; set; }
        public string VersionName { get; set; }
        public string VersionType { get; set; }

        public DateTime ScriptExecuted { get; set; }
    }

    public class CountVersions : Query<int>, IVersionCriteria
    {
        public string ScriptContent { get; set; }
        public string ScriptPath { get; set; }
        public string VersionName { get; set; }
        public string VersionType { get; set; }

        public DateTime ScriptExecuted { get; set; }
    }

    public class SearchVersions : Query<IEnumerable<VersionMatch>>, IVersionCriteria
    {
        public string ScriptContent { get; set; }
        public string ScriptPath { get; set; }
        public string VersionName { get; set; }
        public string VersionType { get; set; }

        public DateTime ScriptExecuted { get; set; }
    }

    public interface IVersionCriteria
    {
        Filter Filter { get; set; }
        
        string ScriptContent { get; set; }
        string ScriptPath { get; set; }
        string VersionName { get; set; }
        string VersionType { get; set; }

        DateTime ScriptExecuted { get; set; }
    }

    public partial class VersionMatch
    {
        public int VersionNumber { get; set; }
    }

    public partial class VersionModel
    {
        public string ScriptContent { get; set; }
        public string ScriptPath { get; set; }
        public string VersionName { get; set; }
        public string VersionType { get; set; }

        public int VersionNumber { get; set; }

        public DateTime ScriptExecuted { get; set; }
    }
}