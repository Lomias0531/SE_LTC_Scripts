using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Defines a rectangle.
	/// </summary>
	[Serializable]
	public struct Rectangle : IEquatable<Rectangle>
	{
		protected class VRageMath_Rectangle_003C_003EX_003C_003EAccessor : IMemberAccessor<Rectangle, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Rectangle owner, in int value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Rectangle owner, out int value)
			{
				value = owner.X;
			}
		}

		protected class VRageMath_Rectangle_003C_003EY_003C_003EAccessor : IMemberAccessor<Rectangle, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Rectangle owner, in int value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Rectangle owner, out int value)
			{
				value = owner.Y;
			}
		}

		protected class VRageMath_Rectangle_003C_003EWidth_003C_003EAccessor : IMemberAccessor<Rectangle, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Rectangle owner, in int value)
			{
				owner.Width = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Rectangle owner, out int value)
			{
				value = owner.Width;
			}
		}

		protected class VRageMath_Rectangle_003C_003EHeight_003C_003EAccessor : IMemberAccessor<Rectangle, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Rectangle owner, in int value)
			{
				owner.Height = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Rectangle owner, out int value)
			{
				value = owner.Height;
			}
		}

		protected class VRageMath_Rectangle_003C_003ELocation_003C_003EAccessor : IMemberAccessor<Rectangle, Point>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Rectangle owner, in Point value)
			{
				owner.Location = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Rectangle owner, out Point value)
			{
				value = owner.Location;
			}
		}

		/// <summary>
		/// Specifies the x-coordinate of the rectangle.
		/// </summary>
		public int X;

		/// <summary>
		/// Specifies the y-coordinate of the rectangle.
		/// </summary>
		public int Y;

		/// <summary>
		/// Specifies the width of the rectangle.
		/// </summary>
		public int Width;

		/// <summary>
		/// Specifies the height of the rectangle.
		/// </summary>
		public int Height;

		/// <summary>
		/// Returns the x-coordinate of the left side of the rectangle.
		/// </summary>
		public int Left => X;

		/// <summary>
		/// Returns the x-coordinate of the right side of the rectangle.
		/// </summary>
		public int Right => X + Width;

		/// <summary>
		/// Returns the y-coordinate of the top of the rectangle.
		/// </summary>
		public int Top => Y;

		/// <summary>
		/// Returns the y-coordinate of the bottom of the rectangle.
		/// </summary>
		public int Bottom => Y + Height;

		/// <summary>
		/// Gets or sets the upper-left value of the Rectangle.
		/// </summary>
		public Point Location
		{
			get
			{
				return new Point(X, Y);
			}
			set
			{
				X = value.X;
				Y = value.Y;
			}
		}

		/// <summary>
		/// Gets the Point that specifies the center of the rectangle.
		/// </summary>
		public Point Center => new Point(X + Width / 2, Y + Height / 2);

		static Rectangle()
		{
		}

		/// <summary>
		/// Initializes a new instance of Rectangle.
		/// </summary>
		/// <param name="x">The x-coordinate of the rectangle.</param><param name="y">The y-coordinate of the rectangle.</param><param name="width">Width of the rectangle.</param><param name="height">Height of the rectangle.</param>
		public Rectangle(int x, int y, int width, int height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		/// <summary>
		/// Compares two rectangles for equality.
		/// </summary>
		/// <param name="a">Source rectangle.</param><param name="b">Source rectangle.</param>
		public static bool operator ==(Rectangle a, Rectangle b)
		{
			if (a.X == b.X && a.Y == b.Y && a.Width == b.Width)
			{
				return a.Height == b.Height;
			}
			return false;
		}

		/// <summary>
		/// Compares two rectangles for inequality.
		/// </summary>
		/// <param name="a">Source rectangle.</param><param name="b">Source rectangle.</param>
		public static bool operator !=(Rectangle a, Rectangle b)
		{
			if (a.X == b.X && a.Y == b.Y && a.Width == b.Width)
			{
				return a.Height != b.Height;
			}
			return true;
		}

		/// <summary>
		/// Changes the position of the Rectangle.
		/// </summary>
		/// <param name="amount">The values to adjust the position of the Rectangle by.</param>
		public void Offset(Point amount)
		{
			X += amount.X;
			Y += amount.Y;
		}

		/// <summary>
		/// Changes the position of the Rectangle.
		/// </summary>
		/// <param name="offsetX">Change in the x-position.</param><param name="offsetY">Change in the y-position.</param>
		public void Offset(int offsetX, int offsetY)
		{
			X += offsetX;
			Y += offsetY;
		}

		/// <summary>
		/// Pushes the edges of the Rectangle out by the horizontal and vertical values specified.
		/// </summary>
		/// <param name="horizontalAmount">Value to push the sides out by.</param><param name="verticalAmount">Value to push the top and bottom out by.</param>
		public void Inflate(int horizontalAmount, int verticalAmount)
		{
			X -= horizontalAmount;
			Y -= verticalAmount;
			Width += horizontalAmount * 2;
			Height += verticalAmount * 2;
		}

		/// <summary>
		/// Determines whether this Rectangle contains a specified point represented by its x- and y-coordinates.
		/// </summary>
		/// <param name="x">The x-coordinate of the specified point.</param><param name="y">The y-coordinate of the specified point.</param>
		public bool Contains(int x, int y)
		{
			if (X <= x && x < X + Width && Y <= y)
			{
				return y < Y + Height;
			}
			return false;
		}

		/// <summary>
		/// Determines whether this Rectangle contains a specified Point.
		/// </summary>
		/// <param name="value">The Point to evaluate.</param>
		public bool Contains(Point value)
		{
			if (X <= value.X && value.X < X + Width && Y <= value.Y)
			{
				return value.Y < Y + Height;
			}
			return false;
		}

		/// <summary>
		/// Determines whether this Rectangle contains a specified Point.
		/// </summary>
		/// <param name="value">The Point to evaluate.</param><param name="result">[OutAttribute] true if the specified Point is contained within this Rectangle; false otherwise.</param>
		public void Contains(ref Point value, out bool result)
		{
			result = (X <= value.X && value.X < X + Width && Y <= value.Y && value.Y < Y + Height);
		}

		/// <summary>
		/// Determines whether this Rectangle entirely contains a specified Rectangle.
		/// </summary>
		/// <param name="value">The Rectangle to evaluate.</param>
		public bool Contains(Rectangle value)
		{
			if (X <= value.X && value.X + value.Width <= X + Width && Y <= value.Y)
			{
				return value.Y + value.Height <= Y + Height;
			}
			return false;
		}

		/// <summary>
		/// Determines whether this Rectangle entirely contains a specified Rectangle.
		/// </summary>
		/// <param name="value">The Rectangle to evaluate.</param><param name="result">[OutAttribute] On exit, is true if this Rectangle entirely contains the specified Rectangle, or false if not.</param>
		public void Contains(ref Rectangle value, out bool result)
		{
			result = (X <= value.X && value.X + value.Width <= X + Width && Y <= value.Y && value.Y + value.Height <= Y + Height);
		}

		/// <summary>
		/// Determines whether a specified Rectangle intersects with this Rectangle.
		/// </summary>
		/// <param name="value">The Rectangle to evaluate.</param>
		public bool Intersects(Rectangle value)
		{
			if (value.X < X + Width && X < value.X + value.Width && value.Y < Y + Height)
			{
				return Y < value.Y + value.Height;
			}
			return false;
		}

		/// <summary>
		/// Determines whether a specified Rectangle intersects with this Rectangle.
		/// </summary>
		/// <param name="value">The Rectangle to evaluate</param><param name="result">[OutAttribute] true if the specified Rectangle intersects with this one; false otherwise.</param>
		public void Intersects(ref Rectangle value, out bool result)
		{
			result = (value.X < X + Width && X < value.X + value.Width && value.Y < Y + Height && Y < value.Y + value.Height);
		}

		/// <summary>
		/// Creates a Rectangle defining the area where one rectangle overlaps with another rectangle.
		/// </summary>
		/// <param name="value1">The first Rectangle to compare.</param><param name="value2">The second Rectangle to compare.</param>
		public static Rectangle Intersect(Rectangle value1, Rectangle value2)
		{
			int num = value1.X + value1.Width;
			int num2 = value2.X + value2.Width;
			int num3 = value1.Y + value1.Height;
			int num4 = value2.Y + value2.Height;
			int num5 = (value1.X > value2.X) ? value1.X : value2.X;
			int num6 = (value1.Y > value2.Y) ? value1.Y : value2.Y;
			int num7 = (num < num2) ? num : num2;
			int num8 = (num3 < num4) ? num3 : num4;
			Rectangle result = default(Rectangle);
			if (num7 > num5 && num8 > num6)
			{
				result.X = num5;
				result.Y = num6;
				result.Width = num7 - num5;
				result.Height = num8 - num6;
			}
			else
			{
				result.X = 0;
				result.Y = 0;
				result.Width = 0;
				result.Height = 0;
			}
			return result;
		}

		/// <summary>
		/// Creates a Rectangle defining the area where one rectangle overlaps with another rectangle.
		/// </summary>
		/// <param name="value1">The first Rectangle to compare.</param><param name="value2">The second Rectangle to compare.</param><param name="result">[OutAttribute] The area where the two first parameters overlap.</param>
		public static void Intersect(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
		{
			int num = value1.X + value1.Width;
			int num2 = value2.X + value2.Width;
			int num3 = value1.Y + value1.Height;
			int num4 = value2.Y + value2.Height;
			int num5 = (value1.X > value2.X) ? value1.X : value2.X;
			int num6 = (value1.Y > value2.Y) ? value1.Y : value2.Y;
			int num7 = (num < num2) ? num : num2;
			int num8 = (num3 < num4) ? num3 : num4;
			if (num7 > num5 && num8 > num6)
			{
				result.X = num5;
				result.Y = num6;
				result.Width = num7 - num5;
				result.Height = num8 - num6;
			}
			else
			{
				result.X = 0;
				result.Y = 0;
				result.Width = 0;
				result.Height = 0;
			}
		}

		/// <summary>
		/// Creates a new Rectangle that exactly contains two other rectangles.
		/// </summary>
		/// <param name="value1">The first Rectangle to contain.</param><param name="value2">The second Rectangle to contain.</param>
		public static Rectangle Union(Rectangle value1, Rectangle value2)
		{
			int num = value1.X + value1.Width;
			int num2 = value2.X + value2.Width;
			int num3 = value1.Y + value1.Height;
			int num4 = value2.Y + value2.Height;
			int num5 = (value1.X < value2.X) ? value1.X : value2.X;
			int num6 = (value1.Y < value2.Y) ? value1.Y : value2.Y;
			int num7 = (num > num2) ? num : num2;
			int num8 = (num3 > num4) ? num3 : num4;
			Rectangle result = default(Rectangle);
			result.X = num5;
			result.Y = num6;
			result.Width = num7 - num5;
			result.Height = num8 - num6;
			return result;
		}

		/// <summary>
		/// Creates a new Rectangle that exactly contains two other rectangles.
		/// </summary>
		/// <param name="value1">The first Rectangle to contain.</param><param name="value2">The second Rectangle to contain.</param><param name="result">[OutAttribute] The Rectangle that must be the union of the first two rectangles.</param>
		public static void Union(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
		{
			int num = value1.X + value1.Width;
			int num2 = value2.X + value2.Width;
			int num3 = value1.Y + value1.Height;
			int num4 = value2.Y + value2.Height;
			int num5 = (value1.X < value2.X) ? value1.X : value2.X;
			int num6 = (value1.Y < value2.Y) ? value1.Y : value2.Y;
			int num7 = (num > num2) ? num : num2;
			int num8 = (num3 > num4) ? num3 : num4;
			result.X = num5;
			result.Y = num6;
			result.Width = num7 - num5;
			result.Height = num8 - num6;
		}

		/// <summary>
		/// Determines whether the specified Object is equal to the Rectangle.
		/// </summary>
		/// <param name="other">The Object to compare with the current Rectangle.</param>
		public bool Equals(Rectangle other)
		{
			if (X == other.X && Y == other.Y && Width == other.Width)
			{
				return Height == other.Height;
			}
			return false;
		}

		/// <summary>
		/// Returns a value that indicates whether the current instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">Object to make the comparison with.</param>
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is Rectangle)
			{
				result = Equals((Rectangle)obj);
			}
			return result;
		}

		/// <summary>
		/// Retrieves a string representation of the current object.
		/// </summary>
		public override string ToString()
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return string.Format(currentCulture, "{{X:{0} Y:{1} Width:{2} Height:{3}}}", X.ToString(currentCulture), Y.ToString(currentCulture), Width.ToString(currentCulture), Height.ToString(currentCulture));
		}

		/// <summary>
		/// Gets the hash code for this object.
		/// </summary>
		public override int GetHashCode()
		{
			return X.GetHashCode() + Y.GetHashCode() + Width.GetHashCode() + Height.GetHashCode();
		}
	}
}
