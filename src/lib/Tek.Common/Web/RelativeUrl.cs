using System.Collections.Generic;

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
            => Path.IsNotEmpty() && Path.Contains("/");

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

    public class RelativeUrlCollection
    {
        public static void AddParents(Dictionary<string, string> relativeUrls)
        {
            var parentUrls = new Dictionary<string, string>();

            foreach (var relativeUrl in relativeUrls.Keys)
            {
                var url = new RelativeUrl(relativeUrl);

                while (url.HasSegments())
                {
                    url.RemoveLastSegment();

                    if (!parentUrls.ContainsKey(url.Path))
                    {
                        parentUrls.Add(url.Path, $"Parent path derived from {relativeUrl}");
                    }
                }
            }

            foreach (var parentUrl in parentUrls.Keys)
            {
                var derivedValue = parentUrl;

                if (!relativeUrls.ContainsKey(derivedValue))
                    relativeUrls.Add(derivedValue, parentUrls[parentUrl]);
            }
        }
    }
}