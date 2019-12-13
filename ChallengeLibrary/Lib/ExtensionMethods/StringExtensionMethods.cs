using System;

namespace ChallengeLibrary.Lib.ExtensionMethods
{
	public static class StringExtensionMethods
	{
		/// <summary>
		/// Returns a shuffled version of this string.
		/// </summary>
		/// <param name="str">The given string.</param>
		/// <returns>The given string, with the characters randomly shuffled.</returns>
		public static string shuffle (this string str)
		{
			char[] array = str.ToCharArray ();
			Random rng = new Random ();
			int n = array.Length;
			while (n > 1)
			{
				n--;
				int k = rng.Next (n + 1);
				char value = array[k];
				array[k] = array[n];
				array[n] = value;
			}
			return new string (array);
		}

		/// <summary>
		/// Returns a string composed of a given number of random characters from this string. Can contain duplicate characters.
		/// </summary>
		/// <param name="str">The given string.</param>
		/// <param name="num">The length of the output string.</param>
		/// <returns>A string of length num, composed of random characters from the given string.</returns>
		public static string randomSlice (this string str, int num)
		{
			Random random = new Random ();
			string result = "";
			while (result.Length < num)
			{
				result += str[random.Next (str.Length)];
			}
			return result;
		}

		/// <summary>
		/// Returns a string composed of random characters from this string. Can contain duplicate characters.
		/// </summary>
		/// <param name="str">The given string.</param>
		/// <returns>A string composed of random characters from the given string.</returns>
		public static string randomSlice (this string str)
		{
			return randomSlice(str, str.Length);
		}
	}
}
