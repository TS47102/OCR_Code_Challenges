using System;
using System.Collections.Generic;
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
		public const char DEFAULT_ARG_SEPARATOR = ' ';
		public const char DEFAULT_ARG_DELIMITER = '"';
		public const char DEFAULT_ARG_ESCAPER = '\\';

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

		/// <summary>
		/// Split a <see cref="string"/> into an array of arguments.
		/// </summary>
		/// <param name="rawArgs">The <see cref="string"/> to split into arguments.</param>
		/// <param name="argSeparator">The <see cref="char"/> that indicates the end of one argument and the start of the next.</param>
		/// <param name="argDelimiter">The <see cref="char"/> that indicates the start and end of a delimited argument.</param>
		/// <param name="escapeChar">The <see cref="char"/> that indicates to ignore any special properties of the following <see cref="char"/>.</param>
		/// <returns>An array of the arguments in <paramref name="rawArgs"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="rawArgs"/> is <see langword="null"/>.</exception>
		public static string [] parseArgs (string rawArgs, char argSeparator, char argDelimiter, char escapeChar)
		{
			if (rawArgs == null)
				throw new ArgumentNullException (nameof (rawArgs), "Cannot parse null arguments.");

			List<string> blocks = new List<string> ();

			bool inBlock = false;
			bool escapeNextChar = false;
			string currentBlock = "";

			foreach (char currentChar in rawArgs)
			{
				if (escapeNextChar)
				{
					currentBlock += currentChar;
					escapeNextChar = false;
				}
				else
				{
					if (currentChar == argSeparator)
					{
						if (inBlock)
							currentBlock += currentChar;
						else
						{
							blocks.Add (currentBlock);
							currentBlock = "";
						}
					}
					else if (currentChar == argDelimiter)
						inBlock = !inBlock;
					else if (currentChar == escapeChar)
						escapeNextChar = true;
					else
						currentBlock += currentChar;
				}
			}

			blocks.Add (currentBlock);

			return blocks.ToArray ();
		}

		/// <summary>
		/// Splits a <see cref="string"/> into an array of arguments, using default values for special <see cref="char"/>s.
		/// </summary>
		/// <remarks>This overload is a shorthand for calling <see cref="parseArgs(string, char, char, char)"/> with
		/// <paramref name="rawArgs"/>, <see cref="DEFAULT_ARG_SEPARATOR"/>, <see cref="DEFAULT_ARG_DELIMITER"/> and <see cref="DEFAULT_ARG_ESCAPER"/>.</remarks>
		/// <param name="rawArgs">The <see cref="string"/> to split into arguments.</param>
		/// <returns>An array of the arguments in <paramref name="rawArgs"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="rawArgs"/> is <see langword="null"/>.</exception>
		public static string [] parseArgs (string rawArgs)
		{
			return parseArgs (rawArgs, DEFAULT_ARG_SEPARATOR, DEFAULT_ARG_DELIMITER, DEFAULT_ARG_ESCAPER);
		}
	}
}
