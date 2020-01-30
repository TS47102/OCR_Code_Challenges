using System;
using ChallengeLibrary.Exceptions;
using PixelLib.ConsoleHelpers;
using PixelLib.ExtensionMethods;

namespace ChallengeLibrary.Utils
{
	/// <summary>
	/// Provides static helper methods for behaviour that is commonly reused in challenges.
	/// </summary>
	public static class ChallengeUtils
	{
		/// <summary>
		/// Perform common argument checks, throwing exceptions if any fail.
		/// </summary>
		/// <param name="console">The <see cref="CustomConsole"/> to validate.</param>
		/// <param name="args">The string array to validate.</param>
		/// <param name="minimumArgs">The minimum number of arguments expected.</param>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="console"/> or <paramref name="args"/> are <see langword="null"/>.</exception>
		/// <exception cref="ChallengeArgumentCountException">Thrown when the length of <paramref name="args"/> is less than <paramref name="minimumArgs"/>.</exception>
		public static void validateArgs (CustomConsole console, string [] args, int minimumArgs)
		{
			if (console == null)
				throw new ArgumentNullException (nameof (console), "Cannot write to a null console.");

			if (args == null)
				throw new ArgumentNullException (nameof (args), "Cannot execute with null arguments.");

			if (args.Length < minimumArgs)
				throw new ChallengeArgumentCountException ($"Expected at least {minimumArgs} arguments, but got {args.Length} instead.", args.Length, minimumArgs);
		}

		/// <summary>
		/// Checks whether the second given argument (The first argument after the challenge identifier) is a help command.
		/// </summary>
		/// <remarks>A help command flag is considered to be one of the following: <c>'?', '/?', 'help', '--help'</c>.</remarks>
		/// <param name="args">The given arguments.</param>
		/// <returns>Whether or not <paramref name="args"/>[1] is flag indicating a help command.</returns>
		public static bool isHelpCommand (string [] args)
		{
			string [] helpCommands = { "?", "/?", "help", "--help" };
			return args != null && args.Length >= 2 && helpCommands.contains (args [1], StringComparison.OrdinalIgnoreCase);
		}
	}
}
