using System;
using ChallengeLibrary.Utils;
using PixelLib.ConsoleHelpers;

namespace ChallengeLibrary.Challenges
{
	/// <summary>
	/// #1 - 'FactorialFinder'
	/// The Factorial of a positive integer, n, is defined as the product of the sequence n, n-1, n-2, ...1 and the factorial of zero, 0, is defined as being 1. Solve this using both loops and
	/// recursion.
	/// </summary>
	/// <remarks>
	/// Challenge number #45 - 'Find the factorial' is a duplicate of this challenge.
	/// </remarks>
	public class FactorialFinder : IConsoleChallenge
	{
		public const string USE_RECURSIVE_FLAG = "-r";

		/// <summary>
		/// Calculates the factorial of <paramref name="number"/> iteratively.
		/// </summary>
		/// <param name="number">The value to calculate the factorial of.</param>
		/// <returns>The factorial of <paramref name="number"/>.</returns>
		/// <exception cref="OverflowException">Thrown whenever the resulting factorial of <paramref name="number"/> exceeds <see cref="long.MaxValue"/>.
		/// In practice, this occurs whenever <paramref name="number"/> is greater than <c>20</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown whenever <paramref name="number"/> is less than <c>0</c>.</exception>
		public static long factorialFindIterative (int number)
		{
			if (number < 0)
				throw new ArgumentOutOfRangeException (nameof (number), number, "Factorial is not defined for negative values.");

			long product = 1;
			while (number > 0)
				product = checked (product * number--);
			return product;
		}

		/// <summary>
		/// Calculates the factorial of <paramref name="number"/> recursively.
		/// </summary>
		/// <remarks>This function is designed to take advantage of tail-call optimisation, but this optimisation actually being applied is not guranteed by C#.</remarks>
		/// <param name="number">The value to calculate the factorial of.</param>
		/// <returns>The factorial of <paramref name="number"/>.</returns>
		/// <exception cref="OverflowException">Thrown whenever the resulting factorial of <paramref name="number"/> exceeds <see cref="long.MaxValue"/>.
		/// In practice, this occurs whenever <paramref name="number"/> is greater than <c>20</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown whenever <paramref name="number"/> is less than <c>0</c>.</exception>
		public static long factorialFindRecursive (int number)
		{
			if (number < 0)
				throw new ArgumentOutOfRangeException (nameof (number), number, "Factorial is not defined for negative values.");

			return factorialFind_recursive (number, 1);
		}

		private static long factorialFind_recursive (int number, long accumulator)
		{
			if (accumulator == 0)
				throw new ArgumentOutOfRangeException (nameof (accumulator), accumulator, "Accumulator must not be less than 1.");

			return number == 0 ? accumulator : factorialFind_recursive (number - 1, checked (number * accumulator));
		}

		public void execute (CustomConsole console, string [] args)
		{
			ChallengeUtils.validateArgs (console, args, 2);

			if (ChallengeUtils.isHelpCommand (args))
				printHelp (console);
			else
			{
				if (string.Equals (args [1], USE_RECURSIVE_FLAG, StringComparison.OrdinalIgnoreCase))
					calculate (console, args, 2, true);
				else
					calculate (console, args, 1, false);
			}
		}

		private void calculate (CustomConsole console, string [] args, int startIndex, bool useRecursive)
		{
			if (useRecursive)
				console.WriteLine ("{Magenta:}Computing recursively.");
			else
				console.WriteLine ("{Magenta:}Computing iteratively.");

			for (int i = startIndex; i < args.Length; ++i)
			{
				if (int.TryParse (args [i], out int num))
				{
					try { console.WriteLine (num + " {Gray:}-> {Yellow:}" + (useRecursive ? factorialFindRecursive (num) : factorialFindIterative (num))); }
					catch (OverflowException)
						{ console.WriteLine ($"{{Red:}}The factorial of {num} is greater than {long.MaxValue}, and caused a long overflow."); }
					catch (ArgumentOutOfRangeException)
						{ console.WriteLine ($"{{Red:}}{num} is negative, and factorial is not defined for negative values."); }
				}
				else
					console.WriteLine ($"{{Red:}}'{args [i]}' is not a valid integer.");
			}
		}

		private void printHelp (CustomConsole console)
		{
			console.WriteLine ("{:0}FactorialFinder", ConsoleColor.DarkGray);
			console.WriteLine ("{0:}Calculates the factorial of a number.", ConsoleColor.Gray);
			console.WriteLine ("{0:}The factorial of a number is defined as the product of all (whole) positive numbers less than or equal to that number.", ConsoleColor.Gray);
			console.WriteLine ();
			console.WriteLine ("{0:}Usage:", ConsoleColor.Gray);
			console.WriteLine ($"	{{0:}}FactorialFinder [{{1:}}{USE_RECURSIVE_FLAG}{{0:}}] <{{1:}}numbers{{0:}}>", ConsoleColor.White, ConsoleColor.Cyan);
			console.WriteLine ($"	{{0:}}[{{1:}}{USE_RECURSIVE_FLAG}{{0:}}]{{2:}} : {{3:}}Optional flag. {{2:}}Specifies to use a recursive calculation rather than an iterative calculation.", ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Gray, ConsoleColor.Magenta);
			console.WriteLine ("	{0:}<{1:}numbers{0:}>{2:} : One or more positive integers separated by spaces, to calculate the factorials of.", ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Gray);
		}
	}
}
