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
		public InsufficientChallengeArgumentsException () : base ()
		{

		}

		public InsufficientChallengeArgumentsException (string message) : base (message)
		{

		}

		public InsufficientChallengeArgumentsException (string message, Exception innerException) : base (message, innerException)
		{

		}

		protected InsufficientChallengeArgumentsException (SerializationInfo info, StreamingContext context) : base (info, context)
		{

		}
	}
}
