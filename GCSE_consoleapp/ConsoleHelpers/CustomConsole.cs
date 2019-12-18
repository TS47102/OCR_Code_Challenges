﻿using System;
using System.IO;
using System.Text;

namespace GCSE_consoleapp.ConsoleHelpers
{
	/// <summary>
	/// A class that acts as a wrapper around <see cref="Console"/>, to allow for implementing custom console behaviour.
	/// Provides mirrors of all* Properties and Methods in <see cref="Console"/>.
	/// Also provides a wrapper event for <see cref="Console.CancelKeyPress"/>.
	/// This class is not <see langword="abstract"/>, with all members being virtual instead, to allow subclasses to only override the implementations for what they want,
	/// and to allow consumers to use a <see cref="CustomConsole"/> that simply mimics normal <see cref="Console"/> behaviour.
	/// *Some methods, such as the non-params versions of <see cref="Console.Write(string, object[])"/> and <see cref="Console.WriteLine(string, object[])"/> are omitted for convenience to subclasses.
	/// *All of the single parameter versions of <see cref="Console.Write(object)"/> and <see cref="Console.WriteLine(object)"/> have been substituted with a single Generic method.
	/// </summary>
	public class CustomConsole
	{
		#pragma warning disable IDE1006 // Naming styles, member identifiers are exact mirrors of System.Console's.
		public virtual Encoding		InputEncoding			{ get	=> Console.InputEncoding;			set => Console.InputEncoding = value; }
		public virtual Encoding		OutputEncoding			{ get	=> Console.OutputEncoding;			set => Console.OutputEncoding = value; }
		public virtual ConsoleColor	BackgroundColour		{ get	=> Console.BackgroundColor;			set => Console.BackgroundColor = value; }
		public virtual ConsoleColor	ForegroundColour		{ get	=> Console.ForegroundColor;			set => Console.ForegroundColor = value; }
		public virtual int			BufferWidth				{ get	=> Console.BufferWidth;				set => Console.BufferWidth = value; }
		public virtual int			BufferHeight			{ get	=> Console.BufferHeight;			set => Console.BufferHeight = value; }
		public virtual int			WindowWidth				{ get	=> Console.WindowWidth;				set => Console.WindowWidth = value; }
		public virtual int			WindowHeight			{ get	=> Console.WindowHeight;			set => Console.WindowHeight = value; }
		public virtual int			WindowLeft				{ get	=> Console.WindowLeft;				set => Console.WindowLeft = value; }
		public virtual int			WindowTop				{ get	=> Console.WindowTop;				set => Console.WindowTop = value; }
		public virtual int			CursorLeft				{ get	=> Console.CursorLeft;				set => Console.CursorLeft = value; }
		public virtual int			CursorTop				{ get	=> Console.CursorTop;				set => Console.CursorTop = value; }
		public virtual int			CursorSize				{ get	=> Console.CursorSize;				set => Console.CursorSize = value; }
		public virtual bool			CursorVisible			{ get	=> Console.CursorVisible;			set => Console.CursorVisible = value; }
		public virtual bool			TreatControlCAsInput	{ get	=> Console.TreatControlCAsInput;	set => Console.TreatControlCAsInput = value; }
		public virtual string		Title					{ get	=> Console.Title;					set => Console.Title = value; }

		public virtual TextReader	In					=> Console.In;
		public virtual TextWriter	Out					=> Console.Out;
		public virtual TextWriter	Error				=> Console.Error;
		public virtual int			LargestWindowWidth	=> Console.LargestWindowWidth;
		public virtual int			LargestWindowHeight	=> Console.LargestWindowHeight;
		public virtual bool			IsInputRedirected	=> Console.IsInputRedirected;
		public virtual bool			IsOutputRedirected	=> Console.IsOutputRedirected;
		public virtual bool			IsErrorRedirected	=> Console.IsErrorRedirected;
		public virtual bool			KeyAvailable		=> Console.KeyAvailable;
		public virtual bool			NumberLock			=> Console.NumberLock;
		public virtual bool			CapsLock			=> Console.CapsLock;

		/// <summary>
		/// Invoked whenever the <see cref="Console.CancelKeyPress"/> event is invoked, by default.
		/// </summary>
		public virtual event ConsoleCancelEventHandler CancelKeyPressEvent;

		/// <summary>
		/// Invoked just before <see cref="Write{T}(T)"/>, or any of its overloads, gets called.
		/// </summary>
		protected virtual event EventHandler PreWriteEvent;

		/// <summary>
		/// Invoked just after <see cref="Write{T}(T)"/>, or any of its overloads, returns.
		/// </summary>
		protected virtual event EventHandler PostWriteEvent;
		
		/// <summary>
		/// Invoked just before <see cref="WriteLine"/>, or any of its overloads, gets called.
		/// </summary>
		protected virtual event EventHandler PreWriteLineEvent;

		/// <summary>
		/// Invoked just after <see cref="WriteLine"/>, or any of its overloads, returns.
		/// </summary>
		protected virtual event EventHandler PostWriteLineEvent;

		/// <summary>
		/// Invoked just before <see cref="Read"/>, <see cref="ReadKey(bool)"/> or <see cref="ReadLine"/> gets called.
		/// </summary>
		protected virtual event EventHandler PreReadEvent;

		/// <summary>
		/// Invoked just after <see cref="Read"/>, <see cref="ReadKey(bool)"/> or <see cref="ReadLine"/> stops blocking and returns.
		/// </summary>
		protected virtual event EventHandler PostReadEvent;

		public CustomConsole()
		{
			Console.CancelKeyPress += onRaiseConsoleCancelKeyPress;
		}

#pragma warning disable IDE0022 // Use block body for methods
		protected virtual void onRaiseConsoleCancelKeyPress	(object sender, ConsoleCancelEventArgs e)	=> CancelKeyPressEvent?.Invoke (sender, e);
		protected virtual void onPreWriteEvent		()	=> PreWriteEvent?.Invoke (this, EventArgs.Empty);
		protected virtual void onPostWriteEvent		()	=> PostWriteEvent?.Invoke (this, EventArgs.Empty);
		protected virtual void onPreWriteLineEvent	()	=> PreWriteLineEvent?.Invoke (this, EventArgs.Empty);
		protected virtual void onPostWriteLineEvent	()	=> PostWriteLineEvent?.Invoke (this, EventArgs.Empty);
		protected virtual void onPreReadEvent		()	=> PreReadEvent?.Invoke (this, EventArgs.Empty);
		protected virtual void onPostReadEvent		()	=> PostReadEvent?.Invoke (this, EventArgs.Empty);

		public virtual Stream OpenStandardError		()					=> Console.OpenStandardError ();
		public virtual Stream OpenStandardError		(int bufferSize)	=> Console.OpenStandardError (bufferSize);
		public virtual Stream OpenStandardInput		(int bufferSize)	=> Console.OpenStandardInput (bufferSize);
		public virtual Stream OpenStandardInput		()					=> Console.OpenStandardInput ();
		public virtual Stream OpenStandardOutput	(int bufferSize)	=> Console.OpenStandardOutput (bufferSize);
		public virtual Stream OpenStandardOutput	()					=> Console.OpenStandardOutput ();
		
		public virtual void Beep				()								=> Console.Beep ();
		public virtual void Beep				(int frequency, int duration)	=> Console.Beep (frequency, duration);
		public virtual void Clear				()								=> Console.Clear ();
		public virtual void ResetColour			()								=> Console.ResetColor ();
		public virtual void SetBufferSize		(int width, int height)			=> Console.SetBufferSize (width, height);
		public virtual void SetCursorPosition	(int left, int top)				=> Console.SetCursorPosition (left, top);
		public virtual void SetError			(TextWriter newError)			=> Console.SetError (newError);
		public virtual void SetIn				(TextReader newIn)				=> Console.SetIn (newIn);
		public virtual void SetOut				(TextWriter newOut)				=> Console.SetOut (newOut);
		public virtual void SetWindowPosition	(int left, int top)				=> Console.SetWindowPosition (left, top);
		public virtual void SetWindowSize		(int width, int height)			=> Console.SetWindowSize (width, height);

		public virtual void Write<T>	(T value)								{ onPreWriteEvent (); Console.Write (value);				onPostWriteEvent (); }
		public virtual void Write		(char[] buffer)							{ onPreWriteEvent (); Console.Write (buffer);				onPostWriteEvent (); }
		public virtual void	Write		(char[] buffer, int index, int count)	{ onPreWriteEvent (); Console.Write (buffer, index, count);	onPostWriteEvent (); }
		public virtual void	Write		(string format, params object[] arg)	{ onPreWriteEvent (); Console.Write (format, arg);			onPostWriteEvent (); }

		public virtual void	WriteLine		()										{ onPreWriteLineEvent (); Console.WriteLine ();						onPostWriteLineEvent (); }
		public virtual void	WriteLine<T>	(T value)								{ onPreWriteLineEvent (); Console.WriteLine (value);				onPostWriteLineEvent (); }
		public virtual void	WriteLine		(char[] buffer)							{ onPreWriteLineEvent (); Console.WriteLine (buffer);				onPostWriteLineEvent (); }
		public virtual void	WriteLine		(char[] buffer, int index, int count)	{ onPreWriteLineEvent (); Console.WriteLine (buffer, index, count);	onPostWriteLineEvent (); }
		public virtual void	WriteLine		(string format, params object[] arg)	{ onPreWriteLineEvent (); Console.WriteLine (format, arg);			onPostWriteLineEvent (); }

		public virtual void MoveBufferArea (int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop)
			=> Console.MoveBufferArea (sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop);

		public virtual void MoveBufferArea (int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
			=> Console.MoveBufferArea (sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop, sourceChar, sourceForeColor, sourceBackColor);
#pragma warning restore IDE0022 // Use block body for methods

		public virtual int Read ()
		{
			onPreReadEvent ();
			int result = Console.Read ();
			onPostReadEvent ();
			return result;
		}

		public virtual ConsoleKeyInfo ReadKey (bool intercept = false)
		{
			onPreReadEvent ();
			ConsoleKeyInfo result = Console.ReadKey (intercept);
			onPostReadEvent ();
			return result;
		}

		public virtual string ReadLine ()
		{
			onPreReadEvent ();
			string result = Console.ReadLine ();
			onPostReadEvent ();
			return result;
		}
	}
}
