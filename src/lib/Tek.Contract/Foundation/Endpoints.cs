namespace Tek.Contract
{
    public static partial class Endpoints
    {
        public const string Token = "token";
        public const string Validate = "token/validate";
        public const string Status = "status";
        public const string Version = "version";

        public static class Debug
        {
            public const string Endpoints = "debug/endpoints";
            public const string Permissions = "debug/permissions";
            public const string Resources = "debug/resources";
            public const string Token = "debug/token";
        }

        public static class React
        {
            public const string Commands = "react/commands";
            public const string Queries = "react/queries";
        }
    }
}