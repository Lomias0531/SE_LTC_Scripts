using System;
using System.Collections.Generic;

namespace VRage.Library.Collections.__helper_namespace
{
	internal class TypeListComparer : IEqualityComparer<List<Type>>
	{
		public bool Equals(List<Type> x, List<Type> y)
		{
			if (x.Count == y.Count)
			{
				for (int i = 0; i < x.Count; i++)
				{
					if (x[i] != y[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public int GetHashCode(List<Type> obj)
		{
			return obj.GetHashCode();
		}
	}
}
