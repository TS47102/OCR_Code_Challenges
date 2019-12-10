using System;

namespace ChallengeLibrary.Challenges._1_FactorialFinder
{
	/// <summary>
	/// The Factorial of a positive integer, n, is defined as the product of the sequence n, n-1, n-2, ...1 and the factorial of zero, 0, is defined as being 1. Solve this using both loops and
	/// recursion.
	/// </summary>
	public static class FactorialFinder
	{
		public static int factorialFind_iterative(int num)
		{
			if (num < 0)
				throw new ArgumentOutOfRangeException($"Factorial is not defined for negative values. (Given: {num})");

			int product = 1;
			while (num > 0)
				product *= num--;
			return product;
		}

		public static int factorialFind_recursive(int num)
		{
			if (num < 0)
				throw new ArgumentOutOfRangeException($"Factorial is not defined for negative values. (Given: {num})");

			return (num == 0) ? 1 : num * factorialFind_recursive(num - 1);
		}
	}
}
