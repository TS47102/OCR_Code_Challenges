﻿using System;

namespace GCSE_consoleapp.ChallengeProxies
{
	public abstract class ChallengeProxy
	{
		public ChallengeIndex proxiedChallenge => (ChallengeIndex)
			Enum.Parse (typeof (ChallengeIndex), GetType ().Name.Substring (0, GetType ().Name.Length - ChallengeProxyFactory.CHALLENGEPROXY_QUALIFIEDNAME_SUFFIX.Length));

		public virtual string[] VALIDNAMES => new string[] { proxiedChallenge.ToString () };
		public virtual int MAXARGS => -1;

		public abstract int MINARGS { get; }

		public abstract void printUsage ();
		public abstract void printDescription ();

		/// <summary>
		/// If this gets invoked, <see cref="doCommonPreexecChecks(string[])"/> gurantees:
		/// The length of <paramref name="args"/> is at least <see cref="MINARGS"/>,
		/// If <see cref="MAXARGS"/> is greater than 0, the length of <paramref name="args"/> is at most <see cref="MAXARGS"/>,
		/// The first value of <paramref name="args"/> (That at index 0) is <see cref="StringComparison.InvariantCultureIgnoreCase"/>-equal to at least one value in <see cref="VALIDNAMES"/>,
		/// and that the second value of <paramref name="args"/> (That at index 1) is not one of '-d' or '--description'.
		/// </summary>
		/// <param name="args"></param>
		protected abstract void do_execute (string[] args);

		internal ChallengeProxy () { }

		private bool validateName (string name)
		{
			foreach (string s in VALIDNAMES)
				if (name.Equals (s, StringComparison.InvariantCultureIgnoreCase))
					return true;
			return false;
		}

		private bool doCommonPreexecChecks (string[] args)
		{
			if (args.Length > 1 && (args[1].Equals ("-d") || args[1].Equals ("--description")))
			{
				printDescription ();
				return false;
			}

			if (args.Length < MINARGS)
				throw new ArgumentException ($"Number of arguments must be at least {MINARGS}, but only {args.Length} were given.", nameof (args));

			if (MAXARGS > 0 && args.Length > MAXARGS)
				throw new ArgumentException ($"Number of arguments must be at most {MAXARGS}, but {args.Length} were given.", nameof (args));

			if (args.Length > 0 && !validateName (args[0]))
				throw new ArgumentException ("Arguments must start with a valid challenge identifier.", nameof (args));

			return true;
		}

		public void execute (string[] args)
		{
			if (doCommonPreexecChecks (args))
				do_execute (args);
		}
	}
}