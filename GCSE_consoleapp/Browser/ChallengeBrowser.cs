using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GCSE_consoleapp.ChallengeProxies;
using PixelLib.ConsoleHelpers;
using PixelLib.ExtensionMethods;

namespace GCSE_consoleapp.Browser
{
	/// <summary>
	/// Allows the user to browse and interact with <see cref="ChallengeProxy"/>s via the command line.
	/// </summary>
	public class ChallengeBrowser
	{
		/// <summary>
		/// The <see cref="char"/> to signal the start/end of a string literal in an argument.
		/// </summary>
		private const char INPUT_BLOCK_DELIMITER = '"';

		/// <summary>
		/// The <see cref="char"/> to seperate arguments with.
		/// </summary>
		private const char INPUT_ARG_DELIMITER = ' ';

		/// <summary>
		/// The <see cref="char"/> to escape the next <see cref="char"/> in an argument.
		/// </summary>
		private const char INPUT_ESCAPE_CHAR = '\\';
		
		/// <summary>
		/// Commands to exit the browser.
		/// </summary>
		private readonly string[] EXITCOMMANDS = new string[]
		{
			"e",
			"exit",
			"q",
			"quit"
		};

		/// <summary>
		/// Commands to display help information.
		/// </summary>
		private readonly string[] HELPCOMMANDS = new string[]
		{
			"/?",
			"?",
			"help"
		};

		/// <summary>
		/// Commands to list challenge information.
		/// </summary>
		private readonly string[] INFOCOMMANDS = new string[]
		{
			"l",
			"--list",
			"challenges",
		};

		private ColourConsole console { get; }
		private string inputPrompt { get; }

		/// <summary>
		/// Creates a new <see cref="ChallengeBrowser"/> with the specified <see cref="ColourConsole"/>.
		/// </summary>
		/// <param name="console">The <see cref="ColourConsole"/> to use.</param>
		public ChallengeBrowser (ColourConsole console, string inputPrompt)
		{
			this.console = console;
			this.inputPrompt = inputPrompt;
		}

		/// <summary>
		/// Begin interacting with the user.
		/// </summary>
		public void startBrowsing ()
		{
			handleHelpCommand (this, new PostConsoleInputEventArgs (console, null));

			ConsoleInputListener listener = new ConsoleInputListener (console);

			listener.preConsoleInputEvent += handlePreInputEvent;
			listener.postConsoleInputEvent += handlePostInputEvent;

			listener.startListening ();

			
		}

		/// <summary>
		/// Display the input prompt just before polling for input.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that invoked the event.</param>
		/// <param name="e">The event args.</param>
		private void handlePreInputEvent (object sender, PreConsoleInputEventArgs e)
		{
			e.consoleUsed.Write (inputPrompt);
		}

		/// <summary>
		/// Process user input.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that invoked the event.</param>
		/// <param name="e">The event args.</param>
		private void handlePostInputEvent (object sender, PostConsoleInputEventArgs e)
		{
			string input = e.consoleInput;
			if (string.IsNullOrWhiteSpace (input))
				e.consoleUsed.WriteLine ();
			else if (HELPCOMMANDS.contains (input, StringComparison.OrdinalIgnoreCase))
				handleHelpCommand (sender, e);
			else if (INFOCOMMANDS.contains (input, StringComparison.OrdinalIgnoreCase))
				handleInfoCommand (sender, e);
			else if (EXITCOMMANDS.contains (input, StringComparison.OrdinalIgnoreCase))
				handleExitCommand (sender, e);
			else
				handleProxyCommand (sender, e);
		}

		/// <summary>
		/// Display help information, e.g. available commands.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that invoked the event.</param>
		/// <param name="e">The event args.</param>
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
				console.Write ("{0:}" + HELPCOMMANDS [i], flagColour);
			}
			console.Write ("{0:}>", separatorColour);
			console.WriteLine ("{0:}: Display this help menu.", descriptionColour);


			console.Write ("{0:}	<", separatorColour);
			for (int i = 0; i < INFOCOMMANDS.Length; i++)
			{
				if (i > 0)
					console.Write ("{0:}|", separatorColour);
				console.Write ("{0:}" + INFOCOMMANDS [i], flagColour);
			}
			console.Write ("{0:}>", separatorColour);
			console.WriteLine ("{0:}: Display a list of all challenges.", descriptionColour);


			console.Write ("{0:}	<", separatorColour);
			for (int i = 0; i < EXITCOMMANDS.Length; i++)
			{
				if (i > 0)
					console.Write ("{0:}|", separatorColour);
				console.Write ("{0:}" + EXITCOMMANDS [i], flagColour);
			}
			console.Write ("{0:}>", separatorColour);
			console.WriteLine ("{0:}: Quit the program.", descriptionColour);
		}

		/// <summary>
		/// List information about <see cref="ChallengeProxy"/>s.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that invoked the event.</param>
		/// <param name="e">The event args.</param>
		private void handleInfoCommand (object sender, PostConsoleInputEventArgs e)
		{
			for (ChallengeIndex i = ChallengeIndex.FactorialFinder; i <= ChallengeIndex.HappyHopper; i++)
				console.WriteLine (string.Format ("{{Yellow:}}{0,-2:d} {{Gray:}}: {{White:}}{1}", (int) i, i.ToString ()));
		}

		/// <summary>
		/// Exit the program.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that invoked the event.</param>
		/// <param name="e">The event args.</param>
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

		/// <summary>
		/// Invoke a <see cref="ChallengeProxy"/>.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that invoked the event.</param>
		/// <param name="e">The event args.</param>
		private void handleProxyCommand (object sender, PostConsoleInputEventArgs e)
		{
			if (string.IsNullOrWhiteSpace (e.consoleInput))
				throw new ArgumentException ("Cannot handle empty input.", nameof (e));

			string [] args = parseArgs (e.consoleInput);

			ChallengeProxy proxy = null;

			try
			{ proxy = ChallengeProxyFactory.getProxy (args [0]); }
			catch (SystemException ex) when (ex is ArgumentException || ex is InvalidCastException || ex is NullReferenceException)
			{ console.WriteLine ("{0:}" +  ex.Message, ConsoleColor.Red); }

			try
			{ proxy.execute (args); }
			catch (Exception ex)
			{ console.WriteLine ("{0:}" +  ex.Message, ConsoleColor.Red); }
		}

		/// <summary>
		/// Split a raw <see cref="string"/> input into an array of arguments.
		/// </summary>
		/// <param name="rawArgs">The <see cref="string"/> to parse into an array of arguments.</param>
		/// <returns>An array of the arguments in <paramref name="rawArgs"/>.</returns>
		private string [] parseArgs (string rawArgs)
		{
			List<string> blocks = new List<string> ();

			bool inBlock = false;
			bool escapeNextChar = false;
			string currentBlock = "";

			for (int i = 0; i < rawArgs.Length; i++)
			{
				char currentChar = rawArgs [i];

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

		/// <summary>
		/// Double-check if the user really wants to exit.
		/// </summary>
		/// <returns>A <see cref="bool"/> representing whether or not the user confirmed the exit request.</returns>
		private bool confirmExit ()
		{
			console.WriteLine ("{0:1}Are you sure you want to exit the program? (Y/N)", ConsoleColor.Cyan, ConsoleColor.Red);
			console.Write (inputPrompt);
			string confirmation = console.ReadKey ().Key.ToString ();
			console.WriteLine ();
			return confirmation.Equals ("y", StringComparison.OrdinalIgnoreCase);
		}
	}
}
