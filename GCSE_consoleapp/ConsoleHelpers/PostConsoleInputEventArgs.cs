using System;

namespace GCSE_consoleapp.ConsoleHelpers
{
	public class PostConsoleInputEventArgs : EventArgs
	{
		public CustomConsole consoleUsed { get; }
		public string consoleInput { get; }
		public bool wasInterrupted { get; }
		public bool cancelRequested { get; set; }

		public PostConsoleInputEventArgs (CustomConsole consoleUsed, string consoleInput, bool wasInterrupted)
		{
			this.consoleUsed = consoleUsed ?? throw new ArgumentNullException (nameof (consoleUsed), $"Cannot have a null {nameof (consoleUsed)}.");
			this.consoleInput = consoleInput;
			this.wasInterrupted = wasInterrupted;
		}
	}
}
