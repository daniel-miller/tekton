using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Tek.Common;

namespace Tek.Toolbox
{
    public class JwtPayloadConverter : JsonConverter<Dictionary<string, List<string>>>
    {
        public override void WriteJson(JsonWriter writer, Dictionary<string, List<string>> value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteStartObject();

            foreach (var kvp in value)
            {
                writer.WritePropertyName(kvp.Key);

                if (kvp.Value.Count == 1)
                {
                    if (kvp.Key.MatchesAny(new[] {"exp","iat","nbf","ttl"}) && long.TryParse(kvp.Value[0], out long numericDate))
                    {
                        writer.WriteValue(numericDate);
                    }
                    else
                    {
                        writer.WriteValue(kvp.Value[0]);
                    }
                }
                else
                {
                    writer.WriteStartArray();
                    foreach (var item in kvp.Value)
                    {
                        writer.WriteValue(item);
                    }
                    writer.WriteEndArray();
                }
            }

            writer.WriteEndObject();
        }

        public override Dictionary<string, List<string>> ReadJson(JsonReader reader, Type objectType, Dictionary<string, List<string>> existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var result = new Dictionary<string, List<string>>();

            var jObject = JObject.Load(reader);

            foreach (var property in jObject.Properties())
            {
                if (property.Value.Type == JTokenType.Array)
                {
                    result[property.Name] = property.Value.ToObject<List<string>>();
                }
                else if (property.Value.Type == JTokenType.String || property.Value.Type == JTokenType.Integer)
                {
                    result[property.Name] = new List<string> { property.Value.ToString() };
                }
                else
                {
                    throw new JsonSerializationException($"Unexpected token type: {property.Value.Type}");
                }
            }

            return result;
        }
    }
}