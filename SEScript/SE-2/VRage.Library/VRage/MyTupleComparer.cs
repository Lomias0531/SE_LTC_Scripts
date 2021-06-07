using System;
using System.Collections.Generic;

namespace VRage
{
	/// <summary>
	/// Use this as a custom comparer for the dictionaries, where the tuple is a key
	/// </summary>
	public class MyTupleComparer<T1, T2> : IEqualityComparer<MyTuple<T1, T2>> where T1 : IEquatable<T1> where T2 : IEquatable<T2>
	{
		public bool Equals(MyTuple<T1, T2> x, MyTuple<T1, T2> y)
		{
			if (x.Item1.Equals(y.Item1))
			{
				return x.Item2.Equals(y.Item2);
			}
			return false;
		}

		public int GetHashCode(MyTuple<T1, T2> obj)
		{
			return obj.Item1.GetHashCode() * 1610612741 + obj.Item2.GetHashCode();
		}
	}
	/// <summary>
	/// Use this as a custom comparer for the dictionaries, where the tuple is a key
	/// </summary>
	public class MyTupleComparer<T1, T2, T3> : IEqualityComparer<MyTuple<T1, T2, T3>> where T1 : IEquatable<T1> where T2 : IEquatable<T2> where T3 : IEquatable<T3>
	{
		public bool Equals(MyTuple<T1, T2, T3> x, MyTuple<T1, T2, T3> y)
		{
			if (x.Item1.Equals(y.Item1) && x.Item2.Equals(y.Item2))
			{
				return x.Item3.Equals(y.Item3);
			}
			return false;
		}

		public int GetHashCode(MyTuple<T1, T2, T3> obj)
		{
			return obj.Item1.GetHashCode() * 1610612741 + obj.Item2.GetHashCode() * 1610612741 + obj.Item3.GetHashCode();
		}
	}
}
