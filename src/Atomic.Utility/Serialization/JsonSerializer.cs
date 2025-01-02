using System;
using System.Collections.Generic;

using Atomic.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Atomic.Utility
{
    public class JsonSerializer : IJsonSerializer
    {
        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public string Serialize(object value, JsonPurpose mode, string[] excludeProperties = null, bool disablePropertyConverters = false)
        {
            var settings = CreateSerializerSettings(mode, excludeProperties, disablePropertyConverters);

            return JsonConvert.SerializeObject(value, settings);
        }

        public T Deserialize<T>(string value, Type type, JsonPurpose mode, string[] excludeProperties = null, bool disablePropertyConverters = false)
        {
            if (string.IsNullOrWhiteSpace(value))
                return default;

            var settings = CreateSerializerSettings(mode, excludeProperties, disablePropertyConverters);

            return (T)JsonConvert.DeserializeObject(value, type, settings);
        }

        private JsonSerializerSettings CreateSerializerSettings(JsonPurpose mode, string[] excludeProperties, bool disablePropertyConverters)
        {
            var settings = new JsonSerializerSettings();

            if (mode == JsonPurpose.Storage)
            {
                settings.ContractResolver = new JsonResolver(excludeProperties, disablePropertyConverters);
                settings.DefaultValueHandling = DefaultValueHandling.Include;
                settings.Formatting = Formatting.None;
                settings.NullValueHandling = NullValueHandling.Include;
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                settings.TypeNameHandling = TypeNameHandling.None;
            }
            else if (mode == JsonPurpose.Display)
            {
                settings.ContractResolver = new JsonResolver(excludeProperties, disablePropertyConverters);
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;
                settings.Formatting = Formatting.Indented;
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                settings.TypeNameHandling = TypeNameHandling.None;
            }

            settings.Converters.Add(new StringEnumConverter());

            return settings;
        }

        static readonly Dictionary<string, JsonResolver> DefaultResolvers = new Dictionary<string, JsonResolver>();

        static readonly Dictionary<string, JsonResolver> ResolversWithoutPropertyConverters = new Dictionary<string, JsonResolver>();

        static readonly JsonResolver ResolverWithoutPropertyConverters = new JsonResolver(null, true);

        private JsonResolver GetResolver(string excludeProperties, bool disablePropertyConverters)
        {
            var resolvers = disablePropertyConverters
                ? ResolversWithoutPropertyConverters
                : DefaultResolvers;

            return GetResolver(excludeProperties, disablePropertyConverters, resolvers);
        }

        private JsonResolver GetResolver(string excludeProperties, bool disablePropertyConverters, Dictionary<string, JsonResolver> resolvers)
        {
            lock (this)
            {
                if (!resolvers.ContainsKey(excludeProperties))
                    resolvers.Add(excludeProperties, new JsonResolver(excludeProperties.Split(','), disablePropertyConverters));
            }

            return resolvers[excludeProperties];
        }
    }
}