using System;
using System.Runtime.Serialization;
using ChallengeLibrary.Reflection;

namespace ChallengeLibrary.Exceptions
{
	/// <summary>
	/// Indicates that an <see cref="IConsoleChallenge"/> has not been provided with enough arguments.
	/// </summary>
	[Serializable]
	public class ChallengeArgumentCountException : ChallengeException
	{
		public int providedArgumentCount { get; }
		public int expectedArgumentCount { get; }

		public ChallengeArgumentCountException () : base ()
		{

		}

		public ChallengeArgumentCountException (string message) : base (message)
		{

		}

		public ChallengeArgumentCountException (string message, Exception innerException) : base (message, innerException)
		{

		}

		public ChallengeArgumentCountException (int providedArgumentCount, int expectedArgumentCount) : base ($"Recieved {providedArgumentCount} arguments when {expectedArgumentCount} were expected.")
		{
			this.providedArgumentCount = providedArgumentCount;
			this.expectedArgumentCount = expectedArgumentCount;
		}

		public ChallengeArgumentCountException (int providedArgumentCount, int expectedArgumentCount, Exception innerException) : base ($"Recieved {providedArgumentCount} arguments when {expectedArgumentCount} were expected.", innerException)
		{
			this.providedArgumentCount = providedArgumentCount;
			this.expectedArgumentCount = expectedArgumentCount;
		}

		public ChallengeArgumentCountException (string message, int providedArgumentCount, int expectedArgumentCount) : base (message)
		{
			this.providedArgumentCount = providedArgumentCount;
			this.expectedArgumentCount = expectedArgumentCount;
		}

		public ChallengeArgumentCountException (string message, int providedArgumentCount, int expectedArgumentCount, Exception innerException) : base (message, innerException)
		{
			this.providedArgumentCount = providedArgumentCount;
			this.expectedArgumentCount = expectedArgumentCount;
		}

		protected ChallengeArgumentCountException (SerializationInfo info, StreamingContext context) : base (info, context)
		{

		}
	}
}
