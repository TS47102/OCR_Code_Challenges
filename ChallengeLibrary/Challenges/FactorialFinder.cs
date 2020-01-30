using System;
using ChallengeLibrary.Reflection;

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
		/// <summary>
		/// Calculates the factorial of <paramref name="number"/> iteratively.
		/// </summary>
		/// <param name="number">The value to calculate the factorial of.</param>
		/// <returns>The factorial of <paramref name="number"/>.</returns>
		/// <exception cref="OverflowException">Thrown whenever the resulting factorial of <paramref name="number"/> exceeds <see cref="long.MaxValue"/>.
		/// In practice, this occurs whenever <paramref name="number"/> is greater than <c>20</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown whenever <paramref name="number"/> is less than <c>0</c>.</exception>
		public static long factorialFind_iterative (int number)
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
		public static long factorialFind_recursive (int number)
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

		// Suppressing CA1305 here is fine as string interpolation defaults to the Current Culture.
		[System.Diagnostics.CodeAnalysis.SuppressMessage ("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage ("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)")]
		public void execute (string [] args)
		{
			if (args == null)
				throw new ArgumentNullException (nameof (args), "Cannot execute on null arguments.");
			if (args.Length < 2)
				throw new ChallengeException ("Not enough arguments.");
			if (int.TryParse (args [1], out int i))
			{
				try { Console.WriteLine (factorialFind_iterative (i)); }
				catch (OverflowException e)
					{ throw new ChallengeException ($"The factorial of {i} is greater than {int.MaxValue}, and caused an integer overflow.", e); }
				catch (ArgumentOutOfRangeException e)
					{ throw new ChallengeException ("Cannot calculate factorial of negative values.", e); }
			}
			else
				throw new ChallengeException ($"'{i}' is not a valid integer.");
		}
	}
}
