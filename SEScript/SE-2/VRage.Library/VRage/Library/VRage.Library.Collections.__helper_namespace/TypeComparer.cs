using System;
using System.Collections.Generic;

namespace VRage.Library.Collections.__helper_namespace
{
	internal class TypeComparer : IComparer<Type>
	{
		public static readonly TypeComparer Instance = new TypeComparer();

		public int Compare(Type x, Type y)
		{
			return string.CompareOrdinal(x.AssemblyQualifiedName, y.AssemblyQualifiedName);
		}
	}
}
