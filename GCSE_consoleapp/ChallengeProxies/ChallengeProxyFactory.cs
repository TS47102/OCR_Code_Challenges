using System;

namespace GCSE_ConsoleApp.ChallengeProxies
{
	public static class ChallengeProxyFactory
	{
		public const string CHALLENGEPROXY_QUALIFIEDNAME_NAMESPACE = "GCSE_consoleapp.ChallengeProxies.";
		public const string CHALLENGEPROXY_QUALIFIEDNAME_CLASSNAME = "_{0:d}_{1}.{1}";
		public const string CHALLENGEPROXY_QUALIFIEDNAME_SUFFIX = "Proxy";

		public static readonly System.Collections.Generic.Dictionary<ChallengeIndex, ChallengeIndex> CHALLENGEPROXY_ALIASES = new System.Collections.Generic.Dictionary<ChallengeIndex, ChallengeIndex> (2)
		{
			{ ChallengeIndex.FindTheFactorial, ChallengeIndex.FactorialFinder },
			{ ChallengeIndex.HappyNumbers2, ChallengeIndex.HappyNumbers }
		};
		
		public static string getQualifiedChallengeProxyName (int challengeIndex, string challengeName)
		{
			return CHALLENGEPROXY_QUALIFIEDNAME_NAMESPACE + string.Format (CHALLENGEPROXY_QUALIFIEDNAME_CLASSNAME, challengeIndex, challengeName) + CHALLENGEPROXY_QUALIFIEDNAME_SUFFIX;
		}

		public static string getQualifiedChallengeProxyName (ChallengeIndex challengeIndex)
		{
			return getQualifiedChallengeProxyName ((int) challengeIndex, challengeIndex.ToString ());
		}

		public static ChallengeProxy getProxy (ChallengeIndex challengeIndex)
		{
			if (Enum.IsDefined (typeof(ChallengeIndex), challengeIndex)
				&& challengeIndex != ChallengeIndex.Invalid)
			{
				if (CHALLENGEPROXY_ALIASES.ContainsKey (challengeIndex))
					challengeIndex = CHALLENGEPROXY_ALIASES[challengeIndex];

				string qualifiedClassName = getQualifiedChallengeProxyName (challengeIndex);

				Type classType = Type.GetType (qualifiedClassName) ?? throw new ArgumentNullException (nameof (challengeIndex), $"Attempted to create an invalid ChallengeProxy object: '{qualifiedClassName}'. ({nameof (ChallengeIndex)} value existed, but could not get class Type.)");

				if (Activator.CreateInstance (classType) is ChallengeProxy proxyInstance)
				{
					if (proxyInstance.proxiedChallenge != challengeIndex)
						throw new ArgumentException ($"Attempted to create an invalid ChallengeProxy object: '{qualifiedClassName}'. ({nameof (ChallengeIndex)} value did not match with class '{nameof (proxyInstance.proxiedChallenge)}'.)", nameof (challengeIndex));
					
					return proxyInstance;
				}
				else
					throw new InvalidCastException ($"Attempted to create an invalid ChallengeProxy object: '{qualifiedClassName}'. ({nameof (ChallengeIndex)} value existed, but could not cast class to {nameof (ChallengeProxy)}.)");
			}
			else
				throw new ArgumentException ($"Attempted to create an invalid ChallengeProxy object: '{challengeIndex}'. ({nameof (ChallengeIndex)} value does not exist.)", nameof (challengeIndex));
		}

		public static ChallengeProxy getProxy (string challengeName)
		{
			if (Enum.TryParse (challengeName, true, out ChallengeIndex challengeIndex))
				return getProxy (challengeIndex);
			else
				throw new ArgumentException ($"Attempted to create an invalid ChallengeProxy object: '{challengeName}'. ({nameof (ChallengeIndex)} value does not exist.)", nameof (challengeName));
		}

		public static ChallengeProxy getProxy (int challengeIndex)
		{
			return getProxy ((ChallengeIndex) challengeIndex);
		}
	}
}
