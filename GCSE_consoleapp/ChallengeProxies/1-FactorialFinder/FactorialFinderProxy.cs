using System;
using ChallengeLibrary.Challenges._1_FactorialFinder;

namespace GCSE_consoleapp.ChallengeProxies._1_FactorialFinder
{
	public class FactorialFinderProxy : ChallengeProxy
	{
		public override string[] VALIDNAMES => new string[] { proxiedChallenge.ToString(), "FF" };
		public override int MINARGS => 2;
		public override int MAXARGS => 3;

		public override void printUsage ()
		{
			Console.WriteLine ($"Calculates the factorial of a number.");
			Console.WriteLine ();
			Console.WriteLine ($"Usage:");
			Console.WriteLine ($"<{proxiedChallenge.ToString ()}|{(int) proxiedChallenge}> [-i|--iterative|-r|--recursive] <number>");
			Console.WriteLine ($"<{proxiedChallenge.ToString ()}|{(int) proxiedChallenge}> <-d|--description>");
			Console.WriteLine ();
			Console.WriteLine ($"	[-i|--iterative]: Calculates the factorial iteratively. This is also the default behaviour in case no flags are given.");
			Console.WriteLine ($"	[-r|--recursive]: Calculates the factorial recursively. Attempts tail-call optimisation, however this is not guranteed.");
			Console.WriteLine ($"	<number>: The number to calculate the factorial of.");
			Console.WriteLine ($"	<-d|--description>: Prints the description of this challenge, from the OCR 2016 Coding Challenges booklet.");
			Console.WriteLine ();
			Console.WriteLine ($"Remarks:");
			Console.WriteLine ($"An exception will be thrown if:");
			Console.WriteLine ($"	- <number> is negative.");
			Console.WriteLine ($"	- <number> is fractional.");
			Console.WriteLine ($"	- The factorial of <number> is greater than 18,446,744,073,709,551,615, which occurs if <number> is greater than 20.");
		}

		public override void printDescription ()
		{
			Console.WriteLine ($"Challenge number {(int) proxiedChallenge}: {proxiedChallenge.ToString ()}");
			Console.WriteLine ("'The Factorial of a positive integer, n, is defined as the product of the sequence n, n-1, n-2, ...1 and the factorial of zero, 0, is defined as being 1. Solve this using both loops and recursion.'");
		}

		protected override void do_execute (string[] args)
		{
			if (uint.TryParse (args[args.Length - 1], out uint value))
			{
				if (args.Length > MINARGS)
				{
					switch (args[MINARGS - 1])
					{
						case "-r":
						case "--recursive":
							Console.WriteLine ($"The recursive factorial of {value} is: {FactorialFinder.factorialFind_recursive (value)}.");
							break;

						case "-i":
						case "--iterative":
							Console.WriteLine ($"The iterative factorial of {value} is: {FactorialFinder.factorialFind_iterative (value)}.");
							break;

						default:
							throw new ArgumentException ($"Argument number {MINARGS - 1} must be one of: [-i|--iterative|-r|--recursive], but was {args[MINARGS - 1]} instead.", nameof(args));
					}
				}
				else
					Console.WriteLine ($"The iterative factorial of {value} is: {FactorialFinder.factorialFind_iterative (value)}.");
			}
			else
				throw new ArgumentException ($"Last argument must be a valid unsigned int, but was {args[args.Length - 1]} instead.", nameof(args));
		}
	}
}
