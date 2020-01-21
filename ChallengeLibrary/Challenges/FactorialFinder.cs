﻿using System;

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
	public static class FactorialFinder
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
	}
}