using System;
using GCSE_consoleapp.ChallengeBrowser.ExtensionMethods;
using GCSE_consoleapp.ChallengeProxies;
using GCSE_consoleapp.ConsoleHelpers;

namespace GCSE_consoleapp.ChallengeBrowser
{
    public class ChallengeBrowser
    {
		private const char INPUT_ARG_SEPARATOR = ' ';
		private const string DEFAULT_INPUT_PROMPT = "> ";

		private readonly string[] EXITCOMMANDS = new string[]
		{
			"e",
			"exit",
			"q",
			"quit"
		};

		private readonly string[] HELPCOMMANDS = new string[]
		{
			"/?",
			"?",
			"help"
		};

		private CustomConsole console;

		public ChallengeBrowser (CustomConsole console)
		{
			this.console = console;
		}

#pragma warning disable IDE1006 // Naming Styles, Entry point Main function must have this exact signature
		public static void Main (string[] args)
		{
			new ChallengeBrowser (new ColourConsole ()).startBrowsing ();
		}
#pragma warning restore IDE1006 // Naming Styles

		public void startBrowsing ()
		{
			console.WriteLine ("GCSE-level Response Browser for OCR 2016 Coding Challenges, by Pixelstorm.");

			ConsoleInputListener listener = new ConsoleInputListener (console);

			listener.preConsoleInputEvent += handlePreInputEvent;
			listener.postConsoleInputEvent += handlePostInputEvent;

			listener.startListening ();
		}

		private void handlePreInputEvent (object sender, PreConsoleInputEventArgs e)
		{
			e.consoleUsed.Write (DEFAULT_INPUT_PROMPT);
		}

		private void handlePostInputEvent (object sender, PostConsoleInputEventArgs e)
		{
			string input = e.consoleInput;
			if (string.IsNullOrWhiteSpace (input))
				e.consoleUsed.WriteLine ();
			else if (HELPCOMMANDS.contains (input))
				handleHelpCommand (sender, e);
			else if (EXITCOMMANDS.contains (input))
				handleExitCommand (sender, e);
			else
				handleProxyCommand (sender, e);
		}

		private void handleHelpCommand (object sender, PostConsoleInputEventArgs e)
		{
			if (e.consoleUsed is ColourConsole c)
				foreach (ChallengeIndex i in Enum.GetValues (typeof (ChallengeIndex)))
					c.WriteLine (string.Format ("{{Cyan:}}{0,-2:d} {{Gray:}}: {{White:}}{1}", (int) i, i.ToString ()));
			else
				foreach (ChallengeIndex i in Enum.GetValues (typeof (ChallengeIndex)))
					e.consoleUsed.WriteLine (string.Format ("{0,-2:d} : {1}", (int) i, i.ToString ()));
		}

		private void handleExitCommand (object sender, PostConsoleInputEventArgs e)
		{
			e.cancelRequested = true;
		}

		private void handleProxyCommand (object sender, PostConsoleInputEventArgs e)
		{
			if (string.IsNullOrWhiteSpace (e.consoleInput))
				throw new ArgumentException ("Cannot handle empty input.", nameof (e.consoleInput));

			string[] args = e.consoleInput.Split (INPUT_ARG_SEPARATOR);

			ChallengeProxy proxy = null;

			try
			{ proxy = ChallengeProxyFactory.getProxy (args[0]); }
			catch (SystemException ex) when (ex is ArgumentException || ex is InvalidCastException || ex is NullReferenceException)
			{ e.consoleUsed.WriteLine (ex.Message); }
			
			try
			{ proxy.execute (args); }
			catch (Exception ex)
			{ e.consoleUsed.WriteLine (ex.Message); }
		}

		private bool isExitCommand(string command)
		{
			foreach (string exitCommand in EXITCOMMANDS)
				if (exitCommand.Equals (command, StringComparison.InvariantCultureIgnoreCase))
					return true;
			return false;
		}
    }
}
