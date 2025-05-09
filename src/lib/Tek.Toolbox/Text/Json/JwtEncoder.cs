﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Newtonsoft.Json;

using Tek.Base;

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

        public string Extract(string scheme, string authorizationHeader)
        {
            var header = authorizationHeader;

            if (string.IsNullOrEmpty(header) || !header.StartsWith($"{scheme} "))
                return null;

            var token = header.Substring(scheme.Length + 1).Trim();

            return token;
        }

        public bool Validate(string scheme, string token, string secret, string audience, string issuer, IClaimConverter converter, out ClaimsPrincipal principal)
        {
            try
            {
                var jwt = Decode(token);

                principal = new ClaimsPrincipal(converter.ToClaimsIdentity(jwt, scheme));

                var isSignatureVerified = VerifySignature(token, secret);

                var isAudienceVerified = jwt.Audience == audience;

                var isIssuerVerified = jwt.Issuer == issuer;

                return isSignatureVerified && !jwt.IsExpired() && isAudienceVerified && isIssuerVerified;
            }
            catch (ArgumentException ex)
            {
                if (ex.Message.StartsWith("JWT parsing failed"))
                {
                    principal = null;

                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public string Encode(IJwt jwt, string secret)
        {
            var header = new Dictionary<string, string>
            {
                { "alg", "HS256" },
                { "typ", "JWT" }
            };

            string serializedHeader = JsonConvert.SerializeObject(header);

            string serializedClaims = JsonConvert.SerializeObject(jwt.ToDictionary(), _settings);

            var encodedHeader = EncodeBase64(Encoding.UTF8.GetBytes(serializedHeader));

            var encodedPayload = EncodeBase64(Encoding.UTF8.GetBytes(serializedClaims));

            var input = $"{encodedHeader}.{encodedPayload}";

            var encodedSignature = CreateSignature(input, secret);

            var encoded = new EncodedJwt(encodedHeader, encodedPayload, encodedSignature);

            return encoded.ToString();
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
}