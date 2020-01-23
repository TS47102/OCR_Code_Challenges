using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace ChallengeLibrary.Reflection
{
	public static class ChallengeReflector
	{
		// The ImmutableDictionary, unlike the normal Dictionary, is immutable - so suppressing CA2104 here is fine.
		// [System.Diagnostics.CodeAnalysis.SuppressMessage ("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		private static readonly ImmutableDictionary<string, Type> challengeTypeDictionary = getChallengeTypes ();

		private const string challengeNamespace = "ChallengeLibrary.Challenges";

		private static ImmutableDictionary<string, Type> getChallengeTypes ()
		{
			IEnumerable<Type> types = getLoadableTypes (typeof (ChallengeReflector).Assembly).Where (t => t.IsClass
																									&& !t.IsAbstract
																									&& t.Namespace.StartsWith (challengeNamespace, StringComparison.Ordinal)
																									&& typeof (IConsoleChallenge).IsAssignableFrom (t));

			ImmutableDictionary<string, Type>.Builder builder = ImmutableDictionary.CreateBuilder<string, Type> ();

			foreach (Type t in types)
				builder.Add (t.Name, t);

			return builder.ToImmutable ();
		}

		public static IConsoleChallenge createChallenge (string challengeName)
		{
			if (!challengeTypeDictionary.ContainsKey (challengeName))
				throw new ArgumentException ($"The given challenge '{challengeName}' does not exist.", nameof (challengeName));
			return (IConsoleChallenge) Activator.CreateInstance (challengeTypeDictionary [challengeName]);
		}

		// https://stackoverflow.com/a/29379834
		private static IEnumerable<Type> getLoadableTypes (Assembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException (nameof (assembly), "Cannot get types from a null reference.");
			try
			{ return assembly.GetTypes (); }
			catch (ReflectionTypeLoadException e)
			{ return e.Types.Where (t => t != null); }
		}
	}
}
