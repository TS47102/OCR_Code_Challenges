using System;
using System.Collections.Generic;
using ChallengeLibrary.Challenges;
using GCSE_consoleapp.ChallengeBrowser.Challenges;

namespace GCSE_consoleapp.ChallengeBrowser
{
    public class ChallengeBrowser : IConsoleMapper
    {
        public static readonly Dictionary<(int, string), IConsoleMapper> inputMaps = new Dictionary<(int, string), IConsoleMapper>(2)
        {
            { (0, "ListChallenges"), new ChallengeBrowser() },
            { (1, "FactorialFinder"), new FactorialFinderMapper() }
        };

        public static void Main(string[] args)
        {
            Console.WriteLine("Challenge Browser by Pixelstorm");
            string input = Console.ReadLine();
            string[] inputargs = input.Split(' ');
            if(inputMaps.ContainsKey(inputargs[0]))
            {
                inputMaps[inputargs[0]].Main(inputargs);
            }
        }

        void IConsoleMapper.Main(string[] args)
        {
            foreach (string s in inputMaps.Keys)
            {
                Console.WriteLine(s);
            }
        }
    }
}
