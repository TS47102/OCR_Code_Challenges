using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeLibrary.Challenges
{
	/// <summary>
	/// The base class for all handleable exceptions thrown by Challenges.
	/// </summary>
	/// <remarks>This exception type (And any derived types) are to be used by Challenges when
	/// the exception should be caught and suppressed by the caller, with little/no action required.</remarks>
	public class ChallengeException : Exception
	{
		public ChallengeException () : base ()
		{

		}
		
		public ChallengeException (string message) : base (message)
		{

		}

		public ChallengeException (string message, Exception inner) : base (message, inner)
		{

		}
	}
}
