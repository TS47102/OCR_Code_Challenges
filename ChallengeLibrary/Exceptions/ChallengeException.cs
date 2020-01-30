using System;
using System.Runtime.Serialization;

namespace ChallengeLibrary.Exceptions
{
	/// <summary>
	/// The base class for all handleable exceptions thrown by Challenges.
	/// </summary>
	/// <remarks>This exception type (And any derived types) are to be used by Challenges when
	/// the exception should be caught and suppressed by the caller, with little/no action required.</remarks>
	[Serializable]
	public class ChallengeException : Exception
	{
		public ChallengeException () : base ()
		{

		}

		public ChallengeException (string message) : base (message)
		{

		}

		public ChallengeException (string message, Exception innerException) : base (message, innerException)
		{

		}

		protected ChallengeException (SerializationInfo info, StreamingContext context) : base (info, context)
		{

		}
	}
}
