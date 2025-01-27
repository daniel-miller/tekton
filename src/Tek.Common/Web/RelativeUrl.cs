namespace Tek.Common
{
    public class RelativeUrl
    {
        public string Path { get; set; }

        public bool IsCurrentRelative
            => Path.IsNotEmpty() && !IsRootRelative;

        public bool IsRootRelative
            => Path.IsNotEmpty() && Path.StartsWith("/");

        public RelativeUrl(string relativeUrl)
        {
            Path = relativeUrl;
        }

        public bool HasSegments()
            => !string.IsNullOrEmpty(Path) && Path.Contains("/");

        public void RemoveLastSegment()
        {
            if (!HasSegments())
                return;

            var trimmedUrl = Path.TrimEnd('/');

            var lastSlashIndex = trimmedUrl.LastIndexOf('/');

            if (lastSlashIndex > -1)
                Path = trimmedUrl.Substring(0, lastSlashIndex);
        }
    }
}