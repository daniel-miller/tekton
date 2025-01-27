using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Tek.Common;
using Tek.Contract;

namespace Tek.Toolbox
{
    public class JwtEncoder : IJwtEncoder
    {
        private readonly JsonSerializerSettings _settings;

        public JwtEncoder()
        {
            _settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new JwtPayloadConverter() },
                Formatting = Formatting.Indented
            };
        }

        public IJwt Decode(string token)
        {
            try
            {
                var jwt = new EncodedJwt(token);

                var decodedPayload = Encoding.UTF8.GetString(DecodeBase64(jwt.Payload));

                var claims = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(decodedPayload, _settings);

                return new Jwt(claims);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("JWT parsing failed due to an unexpected error.", ex);
            }
        }

        public string Encode(IJwt claims, string secret)
        {
            var header = new Dictionary<string, string>
            {
                { "alg", "HS256" },
                { "typ", "JWT" }
            };

            string serializedHeader = JsonConvert.SerializeObject(header);

            string serializedClaims = JsonConvert.SerializeObject(claims.ToDictionary(), _settings);

            var encodedHeader = EncodeBase64(Encoding.UTF8.GetBytes(serializedHeader));

            var encodedPayload = EncodeBase64(Encoding.UTF8.GetBytes(serializedClaims));

            var input = $"{encodedHeader}.{encodedPayload}";

            var encodedSignature = CreateSignature(input, secret);

            var jwt = new EncodedJwt(encodedHeader, encodedPayload, encodedSignature);

            return jwt.ToString();
        }

        /// <summary>
        /// Determine if the signature is valid.
        /// </summary>
        public bool VerifySignature(string token, string secret)
        {
            if (token.IsEmpty() || secret.IsEmpty())
                return false;

            try
            {
                var jwt = new EncodedJwt(token);

                var input = $"{jwt.Header}.{jwt.Payload}";

                var expectedSignature = CreateSignature(input, secret);

                return jwt.Signature == expectedSignature;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Create an HMAC SHA 256 signature for an input string.
        /// </summary>
        private static string CreateSignature(string input, string secret)
        {
            var encoding = new UTF8Encoding();
            var secretBytes = encoding.GetBytes(secret);
            var messageBytes = encoding.GetBytes(input);
            using (var hmacsha256 = new HMACSHA256(secretBytes))
            {
                var hashMessage = hmacsha256.ComputeHash(messageBytes);
                return EncodeBase64(hashMessage);
            }
        }

        /// <summary>
        /// Encode a Base 64 input array.
        /// </summary>
        private static string EncodeBase64(byte[] input)
        {
            return Convert.ToBase64String(input)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        /// <summary>
        /// Decode a Base 64 input string.
        /// </summary>
        private static byte[] DecodeBase64(string input)
        {
            var paddedInput = input
                .Replace('-', '+')
                .Replace('_', '/');

            switch (paddedInput.Length % 4)
            {
                case 2: paddedInput += "=="; break;
                case 3: paddedInput += "="; break;
            }

            return Convert.FromBase64String(paddedInput);
        }
    }

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
                    writer.WriteValue(kvp.Value[0]);
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
                else if (property.Value.Type == JTokenType.String)
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