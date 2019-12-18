using System;

namespace GCSE_consoleapp.ConsoleHelpers
{
	public class PreConsoleInputEventArgs
	{
		public CustomConsole consoleUsed { get; }
		public bool cancelRequested { get; set; }

		public PreConsoleInputEventArgs (CustomConsole consoleUsed)
		{
			this.consoleUsed = consoleUsed ?? throw new ArgumentNullException (nameof (consoleUsed), $"Cannot have a null {nameof (consoleUsed)}.");
		}
	}
}
