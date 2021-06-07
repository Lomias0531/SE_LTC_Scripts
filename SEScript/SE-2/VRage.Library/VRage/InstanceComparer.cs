using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VRage
{
	/// <summary>
	/// Most probably fastest possible comparer which compares instances of objects
	/// </summary>
	public class InstanceComparer<T> : IEqualityComparer<T> where T : class
	{
		public static readonly InstanceComparer<T> Default = new InstanceComparer<T>();

		public bool Equals(T x, T y)
		{
			return x == y;
		}

		public int GetHashCode(T obj)
		{
			return RuntimeHelpers.GetHashCode(obj);
		}
	}
}
