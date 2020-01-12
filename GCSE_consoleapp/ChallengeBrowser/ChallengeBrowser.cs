using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GCSE_consoleapp.ChallengeProxies;
using PixelLib.ConsoleHelpers;
using PixelLib.ExtensionMethods;

namespace GCSE_consoleapp.ChallengeBrowser
{
	public class ConsoleBrowser
    {
		private const char INPUT_BLOCK_DELIMITER = '"';
		private const char INPUT_ARG_DELIMITER = ' ';
		private const char INPUT_ESCAPE_CHAR = '\\';
		private const string DEFAULT_INPUT_PROMPT = "> ";
		private const int AUTOEXIT_MILLIS = 5000;
		private const StringComparison COMPARISON_OPTIONS = StringComparison.OrdinalIgnoreCase;

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

		private readonly string[] INFOCOMMANDS = new string[]
		{
			"l",
			"--list",
			"challenges",
		};

		private ColourConsole console;

		public ConsoleBrowser (ColourConsole console)
		{
			this.console = console;
		}

#pragma warning disable IDE1006 // Naming Styles, Entry point Main function must have this exact signature
		public static void Main (string[] args)
		{
			new ConsoleBrowser (new ColourConsole ()).startBrowsing ();
		}
#pragma warning restore IDE1006 // Naming Styles

		public void startBrowsing ()
		{
			handleHelpCommand (this, new PostConsoleInputEventArgs (console, ""));

			ConsoleInputListener listener = new ConsoleInputListener (console);

			listener.preConsoleInputEvent += handlePreInputEvent;
			listener.postConsoleInputEvent += handlePostInputEvent;

			listener.startListening ();

			console.WriteLine ($"{{0:1}}Program finished. Press any key to quit, or wait {{2:1}}{AUTOEXIT_MILLIS / 1000}{{0:1}} seconds for the program to automatically exit. ", ConsoleColor.Black, ConsoleColor.White, ConsoleColor.DarkMagenta);

			Task.Delay (AUTOEXIT_MILLIS).ContinueWith (_ =>
			{
				console.WriteLine ("{0:1}Time expired. Automatically closing program.", ConsoleColor.Black, ConsoleColor.White);
				Environment.Exit (0);
			});

			console.ReadKey (true);
			console.WriteLine ("{0:1}Closing program.", ConsoleColor.Black, ConsoleColor.White);
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
			else if (HELPCOMMANDS.contains (input, COMPARISON_OPTIONS))
				handleHelpCommand (sender, e);
			else if (INFOCOMMANDS.contains (input, COMPARISON_OPTIONS))
				handleInfoCommand (sender, e);
			else if (EXITCOMMANDS.contains (input, COMPARISON_OPTIONS))
				handleExitCommand (sender, e);
			else
				handleProxyCommand (sender, e);
		}

		private void handleHelpCommand (object sender, PostConsoleInputEventArgs e)
		{
			ConsoleColor flagColour = ConsoleColor.Cyan;
			ConsoleColor separatorColour = ConsoleColor.White;
			ConsoleColor descriptionColour = ConsoleColor.Gray;

			console.WriteLine ("{:0}GCSE-level Response Browser for OCR 2016 Coding Challenges, by Pixelstorm.", ConsoleColor.DarkGray);
			console.WriteLine ("{0:}Available commands:", ConsoleColor.Gray);

			console.Write ("{0:}	<", separatorColour);
			for (int i = 0; i < HELPCOMMANDS.Length; i++)
			{
				if (i > 0)
					console.Write ("{0:}|", separatorColour);
				console.Write ("{0:}" + HELPCOMMANDS[i], flagColour);
			}
			console.Write ("{0:}>", separatorColour);
			console.WriteLine ("{0:}: Display this help menu.", descriptionColour);


			console.Write ("{0:}	<", separatorColour);
			for (int i = 0; i < INFOCOMMANDS.Length; i++)
			{
				if (i > 0)
					console.Write ("{0:}|", separatorColour);
				console.Write ("{0:}" + INFOCOMMANDS[i], flagColour);
			}
			console.Write ("{0:}>", separatorColour);
			console.WriteLine ("{0:}: Display a list of all challenges.", descriptionColour);


			console.Write ("{0:}	<", separatorColour);
			for (int i = 0; i < EXITCOMMANDS.Length; i++)
			{
				if (i > 0)
					console.Write ("{0:}|", separatorColour);
				console.Write ("{0:}" + EXITCOMMANDS[i], flagColour);
			}
			console.Write ("{0:}>", separatorColour);
			console.WriteLine ("{0:}: Quit the program.", descriptionColour);
		}

		private void handleInfoCommand (object sender, PostConsoleInputEventArgs e)
		{
			for (ChallengeIndex i = ChallengeIndex.FactorialFinder; i <= ChallengeIndex.HappyHopper; i++)
				console.WriteLine (string.Format ("{{Yellow:}}{0,-2:d} {{Gray:}}: {{White:}}{1}", (int) i, i.ToString ()));
		}

		private void handleExitCommand (object sender, PostConsoleInputEventArgs e)
		{
			if (confirmExit ())
			{
				console.WriteLine ("{0:1}Requesting to exit program...", ConsoleColor.Cyan, ConsoleColor.Red);
				e.cancelRequested = true;
			}
			else
				console.WriteLine ("{0:1}Aborted program exit.", ConsoleColor.Cyan, ConsoleColor.Red);
		}
		
		private void handleProxyCommand (object sender, PostConsoleInputEventArgs e)
		{
			if (string.IsNullOrWhiteSpace (e.consoleInput))
				throw new ArgumentException ("Cannot handle empty input.", nameof (e));

			string[] args = parseArgs (e.consoleInput);

			ChallengeProxy proxy = null;

			try
			{ proxy = ChallengeProxyFactory.getProxy (args[0]); }
			catch (SystemException ex) when (ex is ArgumentException || ex is InvalidCastException || ex is NullReferenceException)
			{ console.WriteLine ("{0:}" +  ex.Message, ConsoleColor.Red); }
			
			try
			{ proxy.execute (args); }
			catch (Exception ex)
			{ console.WriteLine ("{0:}" +  ex.Message, ConsoleColor.Red); }
		}

		private string[] parseArgs (string rawArgs)
		{
			List<string> blocks = new List<string> (5);

			bool inBlock = false;
			bool escapeNextChar = false;
			string currentBlock = "";

			for (int i = 0; i < rawArgs.Length; i++)
			{
				char currentChar = rawArgs[i];

				if (escapeNextChar)
				{
					currentBlock += currentChar;
					escapeNextChar = false;
				}
				else
				{
					switch (currentChar)
					{
						case INPUT_ESCAPE_CHAR:
							escapeNextChar = true;
							break;

						case INPUT_ARG_DELIMITER:
							if (inBlock)
								currentBlock += currentChar;
							else
							{
								blocks.Add (currentBlock);
								currentBlock = "";
							}
							break;

						case INPUT_BLOCK_DELIMITER:
							inBlock = !inBlock;
							break;

						default:
							currentBlock += currentChar;
							break;
					}
				}
			}

			blocks.Add (currentBlock);

			return blocks.ToArray ();
		}

		private bool confirmExit ()
		{
			console.WriteLine ("{0:1}Are you sure you want to exit the program? (Y/N)", ConsoleColor.Cyan, ConsoleColor.Red);
			console.Write (DEFAULT_INPUT_PROMPT);
			string confirmation = console.ReadKey ().Key.ToString ();
			console.WriteLine ();
			return confirmation.Equals ("y", COMPARISON_OPTIONS);
		}
	}
}
