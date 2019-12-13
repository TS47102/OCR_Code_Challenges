using System;
using GCSE_consoleapp.ChallengeProxies;

namespace GCSE_consoleapp.ChallengeBrowser
{
    public class ChallengeBrowser
    {
		public static readonly string[] EXITCOMMANDS = new string[]
		{
			"e",
			"exit",
			"q",
			"quit"
		};

        public static void Main (string[] args)
        {
            Console.WriteLine ("GCSE-level Response Browser for OCR 2016 Coding Challenges, by Pixelstorm.");
			string input;

			do
			{
				Console.Write ("> ");
				input = Console.ReadLine ();
				string[] inputargs = input.Split (' ');

				if (inputargs.Length > 0)
				{
					ChallengeProxyFactory.getProxy (inputargs[0]).execute (inputargs);
				}
			} while (!isExitCommand(input));
        }

		public static bool isExitCommand(string command)
		{
			foreach (string exitCommand in EXITCOMMANDS)
				if (command.Equals (exitCommand, StringComparison.InvariantCultureIgnoreCase))
					return true;
			return false;
		}
    }
}
