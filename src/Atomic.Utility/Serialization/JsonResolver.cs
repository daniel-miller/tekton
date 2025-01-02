using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Atomic.Utility
{
    internal sealed class JsonResolver : DefaultContractResolver
    {
        private readonly string[] _excludeProperties;

        private readonly bool _disablePropertyConverters;

        public JsonResolver(string[] excludeProperties = null, bool disablePropertyConverters = false)
        {
            _excludeProperties = excludeProperties;
            _disablePropertyConverters = disablePropertyConverters;
        }

        /// <summary>
        /// Exclude properties that we don't want in the serialized JSON output, and sort properties alphabetically.
        /// </summary>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);

            if (_excludeProperties != null && _excludeProperties.Length > 0)
            {
                properties = properties
                    .Where(p => !_excludeProperties.Contains(p.PropertyName, new CaseInsensitiveStringComparer()))
                    .OrderBy(p => p.PropertyName)
                    .ToList();
            }

            if (_disablePropertyConverters)
            {
                foreach (var prop in properties)
                {
                    prop.Converter = null;
                    prop.Ignored = false;
                }
            }

            return properties;
        }

        private class CaseInsensitiveStringComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y) => string.Equals(x, y, StringComparison.OrdinalIgnoreCase);
            public int GetHashCode(string obj) => obj.GetHashCode();
        }
    }
}