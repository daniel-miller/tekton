using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public class JsonWebToken
    {
        public string Header { get; set; }
        public string Payload { get; set; }
        public string Signature { get; set; }
        public string Token => $"{Header}.{Payload}.{Signature}";

        public Dictionary<string, string> Claims { get; set; }

        public JsonWebToken(IJsonSerializer serializer, string jwt)
        {
            try
            {
                if (string.IsNullOrEmpty(jwt))
                    throw new ArgumentException("JWT cannot be empty.");

                var parts = jwt.Split('.');

                if (parts.Length != 3)
                    throw new ArgumentException("JWT must be a string with 3 parts delimited by a period.");

                Header = parts[0];
                Payload = parts[1];
                Signature = parts[2];

                string headerJson = Encoding.UTF8.GetString(DecodeBase64(Header));
                string payloadJson = Encoding.UTF8.GetString(DecodeBase64(Payload));
                
                Claims = serializer.Deserialize<Dictionary<string, string>>(payloadJson);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("JWT parsing failed due to an unexpected error.", ex);
            }
        }

        public JsonWebToken(IJsonSerializer serializer, Dictionary<string, string> payload, string secret, string issuer, string subject, string audience, DateTimeOffset? expiry)
        {
            // Many external systems (including Moodle) use Firebase to verify authentication tokens. Firebase 
            // considers a token invalid when the value of the "iat" claim represents a time in the future. If there is
            // a slight clock skew between servers, this can cause a token to be rejected even though it is valid on 
            // the server that generated it. To mitigate this risk, we set the "iat" and "nbf" claims to a time that is
            // one minute in the past. Refer to https://firebase.google.com/docs/auth/admin/verify-id-tokens.

            var now = DateTimeOffset.UtcNow.AddMinutes(-1).ToUnixTimeSeconds();

            Claims = new Dictionary<string, string>(payload)
            {
                { "iss", issuer },
                { "sub", subject },
                { "aud", audience },
                { "iat", now.ToString() },
                { "nbf", now.ToString() }
            };

            if (expiry.HasValue)
                Claims.Add("exp", expiry.Value.ToUnixTimeSeconds().ToString());

            var header = new Dictionary<string, string>
            {
                { "alg", "HS256" },
                { "typ", "JWT" }
            };

            string headerJson = serializer.Serialize(header);
            string payloadJson = serializer.Serialize(SortByKey(Claims));

            Header = EncodeBase64(Encoding.UTF8.GetBytes(headerJson));
            Payload = EncodeBase64(Encoding.UTF8.GetBytes(payloadJson));
            Signature = CreateSignature(secret, $"{Header}.{Payload}");
        }

        private Dictionary<string, string> SortByKey(Dictionary<string, string> claims)
        {
            return claims.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Determine if the signature is valid
        /// </summary>
        public bool VerifySignature(string secret)
        {
            if (secret == null)
                return false;

            string input = $"{Header}.{Payload}";

            string expectedSignature = CreateSignature(secret, input);

            return Signature == expectedSignature;
        }

        /// <summary>
        /// Create an HMAC SHA 256 signature for an input string
        /// </summary>
        private string CreateSignature(string secret, string input)
        {
            var encoding = new UTF8Encoding();
            byte[] secretBytes = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(input);
            using (var hmacsha256 = new HMACSHA256(secretBytes))
            {
                byte[] hashMessage = hmacsha256.ComputeHash(messageBytes);
                return EncodeBase64(hashMessage);
            }
        }

        /// <summary>
        /// Decode a Base 64 input string
        /// </summary>
        private byte[] DecodeBase64(string input)
        {
            string paddedInput = input
                .Replace('-', '+')
                .Replace('_', '/');

            switch (paddedInput.Length % 4)
            {
                case 2: paddedInput += "=="; break;
                case 3: paddedInput += "="; break;
            }

            return Convert.FromBase64String(paddedInput);
        }

        /// <summary>
        /// Encode a Base 64 input array
        /// </summary>
        private string EncodeBase64(byte[] input)
        {
            return Convert.ToBase64String(input)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        /// <summary>
        /// Checks for the existence of a claim with a specific value
        /// </summary>
        public bool ContainsClaim(string type, string value)
        {
            if (type == "aud" || type == "Audience")
                return HasExpectedValue("aud", value);

            if (type == "iss" || type == "Issuer")
                return HasExpectedValue("iss", value);

            return ContainsClaim(type, value);
        }

        /// <summary>
        /// Returns true if the token contains the expected value for a claim
        /// </summary>
        private bool HasExpectedValue(string type, string expectedValue)
        {
            var key = Claims.Keys.FirstOrDefault(x => string.Compare(x, type, true) == 0);
            
            if (key == null)
                return false;

            var actualValue = Claims[key];

            return string.Compare(actualValue, expectedValue, true) == 0;
        }

        /// <summary>
        /// Determines if the token is now expired
        /// </summary>
        public bool IsExpired()
        {
            if (!Claims.ContainsKey("exp"))
                return false;

            var expiry = long.Parse(Claims["exp"]);

            return DateTimeOffset.FromUnixTimeSeconds(expiry) < DateTimeOffset.UtcNow;
        }
    }
}