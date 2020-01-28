using System;
using System.Collections.Generic;
using System.Globalization;
using ChallengeLibrary.Challenges;
using ChallengeLibrary.Reflection;
using GCSE_ConsoleApp.ChallengeProxies;
using PixelLib.ConsoleHelpers;
using PixelLib.ExtensionMethods;

namespace GCSE_ConsoleApp.Browser
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

		/// <summary>
		/// The <see cref="string"/> to display just before polling for user input.
		/// </summary>
		private string inputPrompt { get; set; }

		/// <summary>
		/// Creates a new <see cref="ChallengeBrowser"/> with the specified input prompt.
		/// </summary>
		/// <param name="inputPrompt">The <see cref="string"/> to display just before polling for user input.</param>
		public ChallengeBrowser (string inputPrompt)
		{
			this.inputPrompt = inputPrompt;
		}

		/// <summary>
		/// Begin interacting with the user.
		/// </summary>
		/// <param name="console">The <see cref="ColourConsole"/> through which to interact with the user.</param>
		public void startBrowsing (ColourConsole console)
		{
			displayHelpInformation (console);

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
				displayHelpInformation (e.consoleUsed as ColourConsole);
			else if (INFOCOMMANDS.contains (input, StringComparison.OrdinalIgnoreCase))
				displayChallengeInformation (e.consoleUsed as ColourConsole);
			else if (EXITCOMMANDS.contains (input, StringComparison.OrdinalIgnoreCase))
				confirmExit (e);
			else
				invokeProxy (e);
		}

		/// <summary>
		/// Display help information, e.g. available commands.
		/// </summary>
		/// <param name="console">The <see cref="ColourConsole"/> to write the help information to.</param>
		private void displayHelpInformation (ColourConsole console)
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
		/// <param name="console">The <see cref="ColourConsole"/> to write the information to.</param>
		private static void displayChallengeInformation (ColourConsole console)
		{
			for (ChallengeIndex i = ChallengeIndex.FactorialFinder; i <= ChallengeIndex.HappyHopper; i++)
				console.WriteLine (string.Format (CultureInfo.CurrentCulture, "{{Yellow:}}{0,-2:d} {{Gray:}}: {{White:}}{1}", (int) i, i.ToString ()));
		}

		/// <summary>
		/// Exit the program.
		/// </summary>
		/// <param name="e">The event args.</param>
		private void confirmExit (PostConsoleInputEventArgs e)
		{
			ColourConsole colourConsole = e.consoleUsed as ColourConsole;
			if (confirmExit (colourConsole))
			{
				colourConsole.WriteLine ("{0:1}Requesting to exit program...", ConsoleColor.Cyan, ConsoleColor.Red);
				e.cancelRequested = true;
			}
			else
				colourConsole.WriteLine ("{0:1}Aborted program exit.", ConsoleColor.Cyan, ConsoleColor.Red);
		}

		/// <summary>
		/// Invoke a <see cref="ChallengeProxy"/>.
		/// </summary>
		/// <param name="e">The event args.</param>
		private static void invokeProxy (PostConsoleInputEventArgs e)
		{
			if (string.IsNullOrWhiteSpace (e.consoleInput))
				throw new ArgumentException ("Cannot handle empty input.", nameof (e));

			string [] args = parseArgs (e.consoleInput);

			IConsoleChallenge challenge = null;

			try
			{ challenge = ChallengeReflector.createChallenge (args [0]); }
			catch (ArgumentException ex) { e.consoleUsed.WriteLine ("{0:}" +  ex.Message, ConsoleColor.Red); }

			try
			{ challenge?.execute (args); }
			catch (ChallengeException ex) { e.consoleUsed.WriteLine ("{0:}" +  ex.Message, ConsoleColor.Red); }
		}

		/// <summary>
		/// Split a raw <see cref="string"/> input into an array of arguments.
		/// </summary>
		/// <param name="rawArgs">The <see cref="string"/> to parse into an array of arguments.</param>
		/// <returns>An array of the arguments in <paramref name="rawArgs"/>.</returns>
		private static string [] parseArgs (string rawArgs)
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
		private bool confirmExit (ColourConsole console)
		{
			console.WriteLine ("{0:1}Are you sure you want to exit the program? (Y/N)", ConsoleColor.Cyan, ConsoleColor.Red);
			console.Write (inputPrompt);
			string confirmation = console.ReadKey ().Key.ToString ();
			console.WriteLine ();
			return confirmation.Equals ("y", StringComparison.OrdinalIgnoreCase);
		}
	}
}
