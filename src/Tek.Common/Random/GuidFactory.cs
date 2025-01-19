using System;
using System.Security.Cryptography;

namespace Tek.Common
{
    public class GuidFactory
    {
        /// <summary>
        /// Generates a Version 7 (timestamp and random) UUID value.
        /// </summary>
        /// <remarks>
        /// The Guid.NewGuid() method generates a Version 4 UUID, which is suitable for scenarios 
        /// where randomness is sufficient and there are no strict requirements for sequential or 
        /// time-based ordering. Version 7 UUIDs (UUIDv7) are designed for keys in high-load 
        /// databases and distributed systems. For more information about UUIDv7 values, refer to
        /// https://en.wikipedia.org/wiki/Universally_unique_identifier#Version_7_(timestamp_and_random)
        /// </remarks>
        public static Guid Create()
        {
            // Get current timestamp in milliseconds since Unix epoch
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // Convert timestamp to byte array
            byte[] timestampBytes = BitConverter.GetBytes(timestamp);

            // Ensure big-endian order for timestamp
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }

            // Create a 16-byte array for the UUID
            byte[] uuidBytes = new byte[16];

            // Set the first 6 bytes to the timestamp (most significant bits)
            Array.Copy(timestampBytes, 2, uuidBytes, 0, 6);

            // Fill the remaining bytes with random data
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(uuidBytes, 6, 10);
            }

            // Set the version to 7 (in the 4 most significant bits of the 7th byte)
            uuidBytes[6] = (byte)((uuidBytes[6] & 0x0F) | 0x70);

            // Set the variant to RFC 4122 (most significant bits of the 9th byte)
            uuidBytes[8] = (byte)((uuidBytes[8] & 0x3F) | 0x80);

            // Return the UUID as a Guid
            return new Guid(uuidBytes);
        }
    }
}