using System;
using System.Threading.Tasks;
using PixelLib.ConsoleHelpers;

namespace GCSE_ConsoleApp.Browser
{
	/// <summary>
	/// Static class that contains the entry point and default fields.
	/// </summary>
	public static class ConsoleApp
	{
		/// <summary>
		/// The time, in milliseconds, before the program automatically closes after exiting.
		/// </summary>
		private static readonly int AUTOEXIT_MILLIS = 5000;

		/// <summary>
		/// The <see cref="string"/> to display whenever user input is polled.
		/// </summary>
		private static readonly string DEFAULT_INPUT_PROMPT = "> ";

		/// <summary>
		/// The default foreground colour to use for printed text.
		/// </summary>
		private static readonly ConsoleColor FOREGROUND_COLOUR = ConsoleColor.White;

		/// <summary>
		/// The default background colour to use for printed text.
		/// </summary>
		private static readonly ConsoleColor BACKGROUND_COLOUR = ConsoleColor.Black;

#pragma warning disable IDE1006 // Naming Styles - Main function requires this exact signature.
		public static void Main ()
		{
			ColourConsole console = new ColourConsole (FOREGROUND_COLOUR, BACKGROUND_COLOUR);

			console.WriteLine ("{0:1}Command-line Challenge Browser by Pixelstorm.", ConsoleColor.Black, ConsoleColor.White);

			new ChallengeBrowser (DEFAULT_INPUT_PROMPT).startBrowsing (console);

			finishExit (console);
		}
#pragma warning restore IDE1006 // Naming Styles

		/// <summary>
		/// Write some explanatory messages and stall for a bit, before halting the program.
		/// </summary>
		/// <param name="console">The console to write to.</param>
		// Suppressing CA1305 here is OK: The text will be displayed to the user, so the Current Culture should be used, and string interpolation defaults to the Current Culture.
		// There is also no way to explicitly specify any culture for string interpolation other than the Invariant culture.
		[System.Diagnostics.CodeAnalysis.SuppressMessage ("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)")]
		private static void finishExit (ColourConsole console)
		{
			console.WriteLine ($"{{0:1}}Program finished. Press any key to quit, or wait {{2:1}}{AUTOEXIT_MILLIS / 1000}{{0:1}} seconds for the program to automatically exit. ", ConsoleColor.Black, ConsoleColor.White, ConsoleColor.DarkMagenta);

			Task.Delay (AUTOEXIT_MILLIS).ContinueWith (_ =>
			{
				console.WriteLine ("{0:1}Time expired. Automatically exiting program.", ConsoleColor.Black, ConsoleColor.White);
				Environment.Exit (0);
			});

			console.ReadKey (true);
			console.WriteLine ("{0:1}Exiting program.", ConsoleColor.Black, ConsoleColor.White);
		}
	}
}
