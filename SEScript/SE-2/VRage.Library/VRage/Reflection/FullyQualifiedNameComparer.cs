using System;
using System.Collections.Generic;

namespace VRage.Reflection
{
	public class FullyQualifiedNameComparer : IComparer<Type>
	{
		public static readonly FullyQualifiedNameComparer Default = new FullyQualifiedNameComparer();

		public int Compare(Type x, Type y)
		{
			return x.FullName.CompareTo(y.FullName);
		}
	}
}
