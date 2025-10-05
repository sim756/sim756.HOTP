using System;
using System.Security.Cryptography;

namespace sim756.HOTP
{
    public class HOTPGenerator
    {
        private const int DefaultLength = 6;

        public int Length { get; set; } = DefaultLength;

        private byte[] key;
        private string keyString;

        public byte[] Key
        {
            get => key;
            set
            {
                key = value;
                keyString = Base32Encoding.ToString(value);
            }
        }

        public string KeyString
        {
            get => keyString;
            set
            {
                keyString = value;
                Key = Base32Encoding.ToBytes(value);
            }
        }

        public HOTPGenerator()
        {
        }

        public HOTPGenerator(byte[] key, int hotpLength = DefaultLength)
        {
            Key = key;
            Length = hotpLength;
        }

        public HOTPGenerator(string key, int hotpLength = DefaultLength)
        {
            Key = Base32Encoding.ToBytes(key);
            Length = hotpLength;
        }

        /// <summary>
        /// Computes the HOTP value for the given counter.
        /// </summary>
        /// <param name="key">Secret key (optional, uses instance key if null)</param>
        /// <param name="counter">Counter value</param>
        /// <returns>HOTP code as string</returns>
        public string Compute(byte[] key = null, long counter = 0)
        {
            byte[] counterBytes = GetBigEndianBytes(counter);
            byte[] hmacComputedHash = new HMACSHA1(key ?? Key).ComputeHash(counterBytes);

            int offset = hmacComputedHash[hmacComputedHash.Length - 1] & 0x0F;
            int binaryCode = ((hmacComputedHash[offset] & 0x7f) << 24)
                             | ((hmacComputedHash[offset + 1] & 0xff) << 16)
                             | ((hmacComputedHash[offset + 2] & 0xff) << 8)
                             | (hmacComputedHash[offset + 3] & 0xff);

            int otp = binaryCode % (int)Math.Pow(10, Length);
            return otp.ToString().PadLeft(Length, '0');
        }

        /// <summary>
        /// Computes the HOTP value for the given counter using the instance key.
        /// </summary>
        /// <param name="counter">Counter value</param>
        /// <returns>HOTP code as string</returns>
        public string Compute(long counter)
        {
            return Compute(Key, counter);
        }

        private byte[] GetBigEndianBytes(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return bytes;
        }
    }
}
