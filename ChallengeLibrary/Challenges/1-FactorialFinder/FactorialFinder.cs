using System;

namespace ChallengeLibrary.Challenges._1_FactorialFinder
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
		/// Calculates the factorial of <c>number</c> iteratively, using a <c>while</c> loop.
		/// </summary>
		/// <param name="number">The value to calculate the factorial of.</param>
		/// <returns>The factorial of <c>number</c>.</returns>
		/// <exception cref="OverflowException">Thrown when the factorial of <c>number</c> is greater than 18,446,744,073,709,551,615.
		/// (Thus meaning whenever <c>number</c> is greater than 20.)</exception>
		public static ulong factorialFind_iterative (uint number)
		{
			ulong product = 1;
			while (number > 0)
				product = checked (product * number--);
			return product;
		}

		/// <summary>
		/// Calculates the factorial of <c>number</c> recursively, using tail-call optimisation. (However this is not guranteed, see <see cref="https://stackoverflow.com/q/491376"/>.)
		/// </summary>
		/// <param name="number">The value to calculate the factorial of.</param>
		/// <returns>The factorial of <c>number</c>.</returns>
		/// <exception cref="OverflowException">Thrown when the factorial of <c>number</c> is greater than 18,446,744,073,709,551,615.
		/// (Thus meaning whenever <c>number</c> is greater than 20.)</exception>
		public static ulong factorialFind_recursive (uint number, ulong accumulator = 1)
		{
			if (accumulator == 0)
				throw new ArgumentOutOfRangeException (nameof (accumulator), accumulator, "Accumulator must not be less than 1.");

			return number == 0 ? accumulator : factorialFind_recursive (number - 1, checked (number * accumulator));
		}
	}
}
