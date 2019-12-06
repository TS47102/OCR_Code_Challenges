using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OCR_code_challenges.Challenges._1_20;

namespace OCR_code_challenge_Tests.Tests._1_20
{
	[TestClass]
	public class FactorialFinder_tests
	{
		[TestMethod]
		public void test_iterative()
		{
			List<int> inputs = new List<int>() { 0, 1, 2, 3, 4, 5 };
			List<int> results = new List<int>() { 1, 1, 2, 6, 24, 120 };

			var outputs = inputs.Select(n => FactorialFinder.factorialFind_iterative(n));

			Assert.IsTrue(results.SequenceEqual(outputs));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException), "Negative input value was allowed.")]
		public void test_iterative_negative()
		{
			int input = -1;
			FactorialFinder.factorialFind_iterative(input);
		}

		[TestMethod]
		public void test_recursive()
		{
			List<int> inputs = new List<int>() { 0, 1, 2, 3, 4, 5 };
			List<int> results = new List<int>() { 1, 1, 2, 6, 24, 120 };

			var outputs = inputs.Select(n => FactorialFinder.factorialFind_iterative(n));

			Assert.IsTrue(results.SequenceEqual(outputs));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException), "Negative input value was allowed.")]
		public void test_recursive_negative()
		{
			int input = -1;
			FactorialFinder.factorialFind_recursive(input);
		}
	}
}
