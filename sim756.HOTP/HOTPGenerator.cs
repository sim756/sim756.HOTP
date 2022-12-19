using System;
using System.Security.Cryptography;

namespace sim756.HOTP
{
	public class HOTPGenerator
	{
		private const long UnixEpochTicks = 621355968000000000L;
		private const long TicksToSeconds = 10000000L;

		private const int DefaultLength = 6;
		private const int DefaultSteps = 30;

		public int Length { get; set; } = DefaultLength;
		public long Steps { get; set; } = DefaultSteps;

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

		public HOTPGenerator(byte[] key, int hotpLength = DefaultLength, int hotpStep = DefaultSteps)
		{
			Key = key;
			Length = hotpLength;
			Steps = hotpStep;
		}

		public HOTPGenerator(string key, int hotpLength = DefaultLength, int hotpStep = DefaultSteps)
		{
			Key = Base32Encoding.ToBytes(key);
			Length = hotpLength;
			Steps = hotpStep;
		}

		public string Compute(string key, DateTime? dateTime = null)
		{
			return Compute(Base32Encoding.ToBytes(key), dateTime);
		}

		public string Compute(byte[] key = null, DateTime? dateTime = null)
		{
			byte[] hmacComputedHash =
				new HMACSHA1(key ?? Key).ComputeHash(
					GetBigEndianBytes(GetTimeStepFromTimeStamp(dateTime ?? DateTime.UtcNow)));

			int offset = hmacComputedHash[hmacComputedHash.Length - 1] & 0x0F;
			int otp = (hmacComputedHash[offset] & 0x7f) << 24
					  | (hmacComputedHash[offset + 1] & 0xff) << 16
					  | (hmacComputedHash[offset + 2] & 0xff) << 8
					  | (hmacComputedHash[offset + 3] & 0xff) % 1000000;

			return TruncateANDPad(otp, Length);
		}

		public long RemainingSeconds()
		{
			return Steps - ((DateTime.UtcNow.Ticks - UnixEpochTicks) / TicksToSeconds) % Steps;
		}

		private byte[] GetBigEndianBytes(long value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			Array.Reverse(bytes);
			return bytes;
		}

		private long GetTimeStepFromTimeStamp(DateTime timestamp)
		{
			long unixTimestamp = (timestamp.Ticks - UnixEpochTicks) / TicksToSeconds;
			long window = unixTimestamp / Steps;
			return window;
		}

		private string TruncateANDPad(long input, int digitCount)
		{
			int truncatedValue = ((int)input % (int)Math.Pow(10, digitCount));
			return truncatedValue.ToString().PadLeft(digitCount, '0');
		}
	}
}
