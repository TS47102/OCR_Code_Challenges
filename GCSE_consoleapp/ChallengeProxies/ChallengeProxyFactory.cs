using System;
using System.Collections.Immutable;
using System.Globalization;

namespace GCSE_ConsoleApp.ChallengeProxies
{
	public static class ChallengeProxyFactory
	{
		public const string CHALLENGEPROXY_QUALIFIEDNAME_NAMESPACE = "GCSE_ConsoleApp.ChallengeProxies.";
		public const string CHALLENGEPROXY_QUALIFIEDNAME_CLASSNAME = "_{0:d}_{1}.{1}";
		public const string CHALLENGEPROXY_QUALIFIEDNAME_SUFFIX = "Proxy";

		// The ImmutableDictionary, unlike the normal Dictionary, is immutable - so suppressing CA2104 here is fine.
		[System.Diagnostics.CodeAnalysis.SuppressMessage ("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly ImmutableDictionary<ChallengeIndex, ChallengeIndex> challengeProxyAliases = initChallengeProxyAliases ();

		private static ImmutableDictionary<ChallengeIndex, ChallengeIndex> initChallengeProxyAliases ()
		{
			ImmutableDictionary<ChallengeIndex, ChallengeIndex>.Builder builder = ImmutableDictionary.CreateBuilder<ChallengeIndex, ChallengeIndex> ();
			builder.Add (ChallengeIndex.FindTheFactorial, ChallengeIndex.FactorialFinder);
			builder.Add (ChallengeIndex.HappyNumbers2, ChallengeIndex.HappyNumbers);
			return builder.ToImmutable ();
		}

		public static string getQualifiedChallengeProxyName (int challengeIndex, string challengeName)
		{
			return CHALLENGEPROXY_QUALIFIEDNAME_NAMESPACE + string.Format (CultureInfo.InvariantCulture, CHALLENGEPROXY_QUALIFIEDNAME_CLASSNAME, challengeIndex, challengeName) + CHALLENGEPROXY_QUALIFIEDNAME_SUFFIX;
		}

		public static string getQualifiedChallengeProxyName (ChallengeIndex challengeIndex)
		{
			return getQualifiedChallengeProxyName ((int) challengeIndex, challengeIndex.ToString ());
		}

		// The exception message will be displayed to the user if it gets thrown, so the Current Culture must be used. String interpolation defaults to the Current Culture, so suppressing this is fine,
		// as there is no way to explicitly (redundantly) specify the Current Culture for string interpolation.
		[System.Diagnostics.CodeAnalysis.SuppressMessage ("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)")]
		public static ChallengeProxy getProxy (ChallengeIndex challengeIndex)
		{
			if (Enum.IsDefined (typeof (ChallengeIndex), challengeIndex)
				&& challengeIndex != ChallengeIndex.Invalid)
			{
				if (challengeProxyAliases.ContainsKey (challengeIndex))
					challengeIndex = challengeProxyAliases [challengeIndex];

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
