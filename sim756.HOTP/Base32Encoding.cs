using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sim756.HOTP
{
	internal class Base32Encoding
	{
		public static byte[] ToBytes(string value)
		{
			value = value.TrimEnd('=');

			int byteCount = value.Length * 5 / 8; //Truncated
			byte[] result = new byte[byteCount];
			int arrayIndex = 0;
			byte remainingBits = 8;
			byte currentByte = 0;
			int mask = 0;


			int CharToAscii(char character)
			{
				int asciiCharValue = (int)character;

				return asciiCharValue switch
				{
					< 91 and > 64 => asciiCharValue - 65,
					< 56 and > 49 => asciiCharValue - 24,
					< 123 and > 96 => asciiCharValue - 97,
					_ => throw new ArgumentException("Character is not a Base32 character.")
				};
			}

			for (int i = 0; i < value.Length; i++)
			{
				int currentValue = CharToAscii(value[i]);

				if (remainingBits > 5)
				{
					mask = currentValue << (remainingBits - 5);
					currentByte = (byte)(currentByte | mask);
					remainingBits -= 5;
				}
				else
				{
					mask = currentValue >> (5 - remainingBits);
					currentByte = (byte)(currentByte | mask);
					result[arrayIndex++] = currentByte;
					currentByte = (byte)(currentValue << (3 + remainingBits));
					remainingBits += 3;
				}
			}

			if (arrayIndex != byteCount)
			{
				result[arrayIndex] = currentByte;
			}

			return result;
		}
	}
}
