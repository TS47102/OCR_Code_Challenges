using System;

namespace OCR_code_challenges.Challenges._1_20
{
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
