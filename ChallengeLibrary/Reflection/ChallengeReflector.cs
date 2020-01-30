using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace ChallengeLibrary.Reflection
{
	public static class ChallengeReflector
	{
		private const string challengeNamespace = "ChallengeLibrary.Challenges";
		private static bool staticInitDone = false;

		public static ImmutableDictionary<string, Type> challengeTypeDictionary { get; } = getChallengeTypes ();

		private static ImmutableDictionary<string, Type> getChallengeTypes ()
		{
			if (staticInitDone)
				return challengeTypeDictionary;

			IEnumerable<Type> types = getLoadableTypes (typeof (ChallengeReflector).Assembly).Where (t => t.IsClass
																									&& !t.IsAbstract
																									&& t.Namespace.StartsWith (challengeNamespace, StringComparison.Ordinal)
																									&& typeof (IConsoleChallenge).IsAssignableFrom (t));

			ImmutableDictionary<string, Type>.Builder builder = ImmutableDictionary.CreateBuilder<string, Type> ();

			foreach (Type t in types)
				builder.Add (t.Name, t);

			ImmutableDictionary<string, Type> result = builder.ToImmutable ();

			staticInitDone = true;

			return result;
		}

		/// <summary>
		/// Creates an <see cref="IConsoleChallenge"/> from a <see cref="string"/> identifier.
		/// </summary>
		/// <param name="challengeName">The identifier of the <see cref="IConsoleChallenge"/> to be created.</param>
		/// <returns>The <see cref="IConsoleChallenge"/> that matches <paramref name="challengeName"/>.</returns>
		/// <exception cref="ArgumentException">Thrown when <paramref name="challengeName"/> is not a valid <see cref="IConsoleChallenge"/> identifier.</exception>
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
			try { return assembly.GetTypes (); }
			catch (ReflectionTypeLoadException e)
				{ return e.Types.Where (t => t != null); }
		}
	}
}
