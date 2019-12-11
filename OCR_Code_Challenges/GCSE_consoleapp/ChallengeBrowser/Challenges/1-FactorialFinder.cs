using System;
using ChallengeLibrary.Challenges._1_FactorialFinder;

namespace GCSE_consoleapp.ChallengeBrowser.Challenges
{
    public class FactorialFinderMapper : ConsoleMapper
    {
        public override void Main(string[] args)
        {
            Console.WriteLine("FactorialFinder");
            int input;
            if (int.TryParse(args[1], out input))
            {
                int output = FactorialFinder.factorialFind_iterative(input);
                Console.WriteLine(output);
            }
        }
    }
}
