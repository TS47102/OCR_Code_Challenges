using System;
using GCSE_consoleapp.ChallengeProxies;
using GCSE_consoleapp.ConsoleHelpers;

namespace GCSE_consoleapp.ChallengeBrowser
{
    public class ChallengeBrowser
    {
		private const char INPUT_ARG_SEPARATOR = ' ';
		private const string DEFAULT_INPUT_PROMPT = "> ";

		private static readonly string[] EXITCOMMANDS = new string[]
		{
			"e",
			"exit",
			"q",
			"quit"
		};

		private static CustomConsole console;

#pragma warning disable IDE1006 // Naming Styles, Entry point Main function must have this exact signature
		public static void Main (string[] args)
		{
			console = new ColourConsole ();

            console.WriteLine ("GCSE-level Response Browser for OCR 2016 Coding Challenges, by Pixelstorm.");

			ConsoleInputListener listener = new ConsoleInputListener (console);

			listener.preConsoleInputEvent += handlePreInputEvent;
			listener.postConsoleInputEvent += handlePostInputEvent;

			listener.startListening ();
		}
#pragma warning restore IDE1006 // Naming Styles

		private static void handlePreInputEvent (object sender, PreConsoleInputEventArgs e)
		{
			e.consoleUsed.Write (DEFAULT_INPUT_PROMPT);
		}

		private static void handlePostInputEvent (object sender, PostConsoleInputEventArgs e)
		{
			string input = e.consoleInput;

			if (isExitCommand (input))
			{
				e.cancelRequested = true;
				return;
			}

			if (string.IsNullOrWhiteSpace (input))
				return;
			
			executeChallengeProxy (input);
		}

		private static void executeChallengeProxy (string userInput)
		{
			string[] inputargs = userInput.Split (INPUT_ARG_SEPARATOR);

			if (inputargs.Length > 0)
			{
				ChallengeProxy proxy = null;

				// Separate try/catch blocks for fetching the Proxy and executing it, for later when we can provide more detailed information about whichever is appropriate:
				// An exception while fetching the Proxy indicates the user requesting an invalid Proxy.
				// An exception while executing the Proxy indicates either the user providing invalid arguments, or some other execution exception by the Proxy itself.

				try
				{
					proxy = ChallengeProxyFactory.getProxy (inputargs[0]);
				}
				catch (Exception e)
				{
					console.WriteLine (e.GetType().FullName + ": " + e.Message);
				}

				try
				{
					proxy?.execute (inputargs);
				}
				catch (Exception e)
				{
					console.WriteLine (e.GetType ().FullName + ": " + e.Message);
				}
			}
		}

		private static bool isExitCommand(string command)
		{
			foreach (string exitCommand in EXITCOMMANDS)
				if (exitCommand.Equals (command, StringComparison.InvariantCultureIgnoreCase))
					return true;
			return false;
		}
    }
}
