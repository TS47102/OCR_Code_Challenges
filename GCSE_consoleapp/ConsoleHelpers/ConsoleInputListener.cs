using System;

namespace GCSE_consoleapp.ConsoleHelpers
{
	public class ConsoleInputListener
	{
		public event EventHandler<PreConsoleInputEventArgs> preConsoleInputEvent;
		public event EventHandler<PostConsoleInputEventArgs> postConsoleInputEvent;

		private CustomConsole console;

		public ConsoleInputListener(CustomConsole console)
		{
			this.console = console;
		}

#pragma warning disable IDE0022 // Use block body for methods
		protected virtual void onPreConsoleInputEvent (PreConsoleInputEventArgs e) => preConsoleInputEvent?.Invoke (this, e);
		protected virtual void onPostConsoleInputEvent (PostConsoleInputEventArgs e) => postConsoleInputEvent?.Invoke (this, e);
#pragma warning restore IDE0022 // Use block body for methods

		/// <summary>
		/// Start repeatedly listening for console input. Uses <see cref="CustomConsole.ReadLine"/>,
		/// and blocks indefinitely until a subscriber of either the <see cref="preConsoleInputEvent"/> or <see cref="postConsoleInputEvent"/> requests a cancellation. 
		/// </summary>
		public void startListening()
		{
			PostConsoleInputEventArgs postInputArgs;
			do
			{
				PreConsoleInputEventArgs preInputArgs = new PreConsoleInputEventArgs (console);
				onPreConsoleInputEvent (preInputArgs);

				if (preInputArgs.cancelRequested)
					break;

				string input = null;
				bool interrupted = false;

				try
				{
					input = console.ReadLine ();
				}
				catch (InvalidOperationException)
				{
					interrupted = true;
				}
				catch (OperationCanceledException)
				{
					interrupted = true;
				}
				finally
				{
					postInputArgs = new PostConsoleInputEventArgs (console, input, interrupted);
					onPostConsoleInputEvent (postInputArgs);
				}
			}
			while (!postInputArgs.cancelRequested);
		}
	}
}
