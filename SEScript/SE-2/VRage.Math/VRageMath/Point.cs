using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Defines a point in 2D space.
	/// </summary>
	[Serializable]
	public struct Point : IEquatable<Point>
	{
		protected class VRageMath_Point_003C_003EX_003C_003EAccessor : IMemberAccessor<Point, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Point owner, in int value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Point owner, out int value)
			{
				value = owner.X;
			}
		}

		protected class VRageMath_Point_003C_003EY_003C_003EAccessor : IMemberAccessor<Point, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Point owner, in int value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Point owner, out int value)
			{
				value = owner.Y;
			}
		}

		private static Point _zero;

		/// <summary>
		/// Specifies the x-coordinate of the Point.
		/// </summary>
		public int X;

		/// <summary>
		/// Specifies the y-coordinate of the Point.
		/// </summary>
		public int Y;

		/// <summary>
		/// Returns the point (0,0).
		/// </summary>
		public static Point Zero => _zero;

		static Point()
		{
		}

		/// <summary>
		/// Initializes a new instance of Point.
		/// </summary>
		/// <param name="x">The x-coordinate of the Point.</param><param name="y">The y-coordinate of the Point.</param>
		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Determines whether two Point instances are equal.
		/// </summary>
		/// <param name="a">Point on the left side of the equal sign.</param><param name="b">Point on the right side of the equal sign.</param>
		public static bool operator ==(Point a, Point b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Determines whether two Point instances are not equal.
		/// </summary>
		/// <param name="a">The Point on the left side of the equal sign.</param><param name="b">The Point on the right side of the equal sign.</param>
		public static bool operator !=(Point a, Point b)
		{
			if (a.X == b.X)
			{
				return a.Y != b.Y;
			}
			return true;
		}

		/// <summary>
		/// Determines whether two Point instances are equal.
		/// </summary>
		/// <param name="other">The Point to compare this instance to.</param>
		public bool Equals(Point other)
		{
			if (X == other.X)
			{
				return Y == other.Y;
			}
			return false;
		}

		/// <summary>
		/// Determines whether two Point instances are equal.
		/// </summary>
		/// <param name="obj">The object to compare this instance to.</param>
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is Point)
			{
				result = Equals((Point)obj);
			}
			return result;
		}

		/// <summary>
		/// Gets the hash code for this object.
		/// </summary>
		public override int GetHashCode()
		{
			return X.GetHashCode() + Y.GetHashCode();
		}

		/// <summary>
		/// Returns a String that represents the current Point.
		/// </summary>
		public override string ToString()
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return string.Format(currentCulture, "{{X:{0} Y:{1}}}", new object[2]
			{
				X.ToString(currentCulture),
				Y.ToString(currentCulture)
			});
		}
	}
}
