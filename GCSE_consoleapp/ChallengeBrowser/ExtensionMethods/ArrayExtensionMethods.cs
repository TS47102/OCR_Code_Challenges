using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCSE_consoleapp.ChallengeBrowser.ExtensionMethods
{
	public static class ArrayExtensionMethods
	{
		public static bool contains<T> (this T[] array, T target)
		{
			foreach (T item in array)
				if (Equals (target, item))
					return true;
			return false;
		}

		public static bool referenceContains<T> (this T[] array, T target)
		{
			foreach (T item in array)
				if (ReferenceEquals (target, item))
					return true;
			return false;
		}
	}
}
