using System;
using System.Runtime.InteropServices;

namespace Atomic.Common
{
    public class GuidFactory
    {
        [DllImport("rpcrt4.dll", SetLastError = true)]
        private static extern int UuidCreateSequential(out Guid guid);

        public static Guid Create()
        {
            Guid sequentialGuid;
            int result = UuidCreateSequential(out sequentialGuid);

            if (result != 0)
            {
                throw new InvalidOperationException("UuidCreateSequential failed: " + result);
            }

            byte[] guidBytes = sequentialGuid.ToByteArray();


            SwapByteOrder(guidBytes);

            return new Guid(guidBytes);
        }

        /// <summary>
        /// Swap the timestamp part with the sequential part
        /// </summary>
        private static void SwapByteOrder(byte[] guidBytes)
        {
            SwapBytes(guidBytes, 0, 3);
            SwapBytes(guidBytes, 1, 2);
            SwapBytes(guidBytes, 4, 5);
            SwapBytes(guidBytes, 6, 7);
        }

        private static void SwapBytes(byte[] guidBytes, int left, int right)
        {
            byte temp = guidBytes[left];
            guidBytes[left] = guidBytes[right];
            guidBytes[right] = temp;
        }
    }
}