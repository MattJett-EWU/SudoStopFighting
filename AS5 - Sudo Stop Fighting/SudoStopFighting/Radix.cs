using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Radix
{
	[DebuggerStepThrough]
	public static class Extensions
	{
		// GLOBAL VARIABLES
		private const string UNIVERSE = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";


		
		/// <summary>
		/// Converts the given int number to the base numeral system with the
		/// specified radix (in the range [2, 36]).
		/// </summary>
		/// <param name="radix">The radix of the destination numeral system (in the range [2, 36]).</param>
		/// <see cref="https://stackoverflow.com/questions/923771/quickest-way-to-convert-a-base-10-number-to-any-base-in-net"/>
		/// <see cref="https://gist.github.com/davidruhmann/98d51e413d1b6b0b2899"/>
		/// <returns>A string representation of the integer.</returns>
		public static string ToBase(this int n, int radix)
		{
			// EXCEPTION HANDLING
			radix = Math.Abs(radix);
			if (radix < 2 || radix > UNIVERSE.Length)
				throw new ArgumentException("The radix must be >= 2 and <= " + UNIVERSE.Length.ToString());
			if (n == 0)
				return "0";

			// ALGORITHM
			string result = "";
			int quotient = Math.Abs(n);
			while (quotient > 0)
			{
				result = UNIVERSE[quotient % radix] + result;
				quotient /= radix;
			}
			return result;
		}


		/// <summary>
		/// Converts the given base numeral in string form to the decimal integer value with the specified radix (in the range [2, 36]).
		/// </summary>
		/// <param name="radix">The radix of the destination numeral system (in the range [2, 36]).</param>
		/// <see cref="https://stackoverflow.com/questions/923771/quickest-way-to-convert-a-base-10-number-to-any-base-in-net"/>
		/// <see cref="https://gist.github.com/davidruhmann/98d51e413d1b6b0b2899"/>
		/// <returns>An integer long type literal of the base numeral.</returns>
		public static int FromBase(this string s, int radix)
		{
			// EXCEPTION HANDLING
			if (s.Length == 0 || string.IsNullOrWhiteSpace(s))
				throw new NullReferenceException(string.Format("Cannot convert a null value, converts only work with valus of: [0-9|A-Z]."));
			if (s.Any(c => !UNIVERSE.Contains(c)))
				throw new KeyNotFoundException(string.Format("This Base{0} number contains a character not used in the base conversion.", radix));
			radix = Math.Abs(radix);
			if (radix < 2 || radix > UNIVERSE.Length)
				throw new ArgumentOutOfRangeException(string.Format("Radix base must be between 2 and {0}.", UNIVERSE.Length));

			// ALGORITHM
			int n = 0;
			int result = 0;
			int m = s.Length - 1;
			foreach (char c in s)
			{
				n = char.IsNumber(c) ? int.Parse(c.ToString()) : Convert.ToInt32(char.ToUpper(c)) - 55;
				result += n * Convert.ToInt32(Math.Pow(radix, m));
				m--;
			}
			return result;			
		}
	}
}
