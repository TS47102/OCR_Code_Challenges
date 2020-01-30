using System;
using System.Runtime.Serialization;
using ChallengeLibrary.Reflection;

namespace ChallengeLibrary.Exceptions
{
	/// <summary>
	/// Indicates that an <see cref="IConsoleChallenge"/> has not been provided with enough arguments.
	/// </summary>
	[Serializable]
	public class InsufficientChallengeArgumentsException : ChallengeException
	{
		public int providedArgumentCount { get; }
		public int expectedArgumentCount { get; }

		public InsufficientChallengeArgumentsException () : base ()
		{

		}

		public InsufficientChallengeArgumentsException (string message) : base (message)
		{

		}

		public InsufficientChallengeArgumentsException (string message, Exception innerException) : base (message, innerException)
		{

		}

		public InsufficientChallengeArgumentsException (int providedArgumentCount, int expectedArgumentCount) : base ($"Recieved {providedArgumentCount} arguments when {expectedArgumentCount} were expected.")
		{
			this.providedArgumentCount = providedArgumentCount;
			this.expectedArgumentCount = expectedArgumentCount;
		}

		public InsufficientChallengeArgumentsException (int providedArgumentCount, int expectedArgumentCount, Exception innerException) : base ($"Recieved {providedArgumentCount} arguments when {expectedArgumentCount} were expected.", innerException)
		{
			this.providedArgumentCount = providedArgumentCount;
			this.expectedArgumentCount = expectedArgumentCount;
		}

		public InsufficientChallengeArgumentsException (string message, int providedArgumentCount, int expectedArgumentCount) : base (message)
		{
			this.providedArgumentCount = providedArgumentCount;
			this.expectedArgumentCount = expectedArgumentCount;
		}

		public InsufficientChallengeArgumentsException (string message, int providedArgumentCount, int expectedArgumentCount, Exception innerException) : base (message, innerException)
		{
			this.providedArgumentCount = providedArgumentCount;
			this.expectedArgumentCount = expectedArgumentCount;
		}

		protected InsufficientChallengeArgumentsException (SerializationInfo info, StreamingContext context) : base (info, context)
		{

		}
	}
}
