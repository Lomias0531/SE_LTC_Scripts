using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Structure using the same layout than <see cref="T:System.Drawing.RectangleF" />
	/// </summary>
	[Serializable]
	public struct RectangleF : IEquatable<RectangleF>
	{
		protected class VRageMath_RectangleF_003C_003EPosition_003C_003EAccessor : IMemberAccessor<RectangleF, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref RectangleF owner, in Vector2 value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref RectangleF owner, out Vector2 value)
			{
				value = owner.Position;
			}
		}

		protected class VRageMath_RectangleF_003C_003ESize_003C_003EAccessor : IMemberAccessor<RectangleF, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref RectangleF owner, in Vector2 value)
			{
				owner.Size = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref RectangleF owner, out Vector2 value)
			{
				value = owner.Size;
			}
		}

		protected class VRageMath_RectangleF_003C_003EX_003C_003EAccessor : IMemberAccessor<RectangleF, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref RectangleF owner, in float value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref RectangleF owner, out float value)
			{
				value = owner.X;
			}
		}

		protected class VRageMath_RectangleF_003C_003EY_003C_003EAccessor : IMemberAccessor<RectangleF, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref RectangleF owner, in float value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref RectangleF owner, out float value)
			{
				value = owner.Y;
			}
		}

		protected class VRageMath_RectangleF_003C_003EWidth_003C_003EAccessor : IMemberAccessor<RectangleF, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref RectangleF owner, in float value)
			{
				owner.Width = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref RectangleF owner, out float value)
			{
				value = owner.Width;
			}
		}

		protected class VRageMath_RectangleF_003C_003EHeight_003C_003EAccessor : IMemberAccessor<RectangleF, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref RectangleF owner, in float value)
			{
				owner.Height = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref RectangleF owner, out float value)
			{
				value = owner.Height;
			}
		}

		/// <summary>
		/// The Position.
		/// </summary>
		public Vector2 Position;

		/// <summary>
		/// The Size.
		/// </summary>
		public Vector2 Size;

		/// <summary>
		/// Left coordinate.
		/// </summary>
		public float X
		{
			get
			{
				return Position.X;
			}
			set
			{
				Position.X = value;
			}
		}

		/// <summary>
		/// Top coordinate.
		/// </summary>
		public float Y
		{
			get
			{
				return Position.Y;
			}
			set
			{
				Position.Y = value;
			}
		}

		/// <summary>
		/// Width of this rectangle.
		/// </summary>
		public float Width
		{
			get
			{
				return Size.X;
			}
			set
			{
				Size.X = value;
			}
		}

		/// <summary>
		/// Height of this rectangle.
		/// </summary>
		public float Height
		{
			get
			{
				return Size.Y;
			}
			set
			{
				Size.Y = value;
			}
		}

		public float Right => Position.X + Size.X;

		public float Bottom => Position.Y + Size.Y;

		public Vector2 Center => Position + Size / 2f;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VRageMath.RectangleF" /> struct.
		/// </summary>
		/// <param name="position">The x-y position of this rectangle.</param>
		/// <param name="size">The x-y size of this rectangle.</param>
		public RectangleF(Vector2 position, Vector2 size)
		{
			Position = position;
			Size = size;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VRageMath.RectangleF" /> struct.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		public RectangleF(float x, float y, float width, float height)
		{
			Position = new Vector2(x, y);
			Size = new Vector2(width, height);
		}

		public bool Contains(int x, int y)
		{
			if ((float)x >= X && (float)x <= X + Width && (float)y >= Y && (float)y <= Y + Height)
			{
				return true;
			}
			return false;
		}

		public bool Contains(float x, float y)
		{
			if (x >= X && x <= X + Width && y >= Y && y <= Y + Height)
			{
				return true;
			}
			return false;
		}

		public bool Contains(Vector2 vector2D)
		{
			if (vector2D.X >= X && vector2D.X <= X + Width && vector2D.Y >= Y && vector2D.Y <= Y + Height)
			{
				return true;
			}
			return false;
		}

		public bool Contains(Point point)
		{
			if ((float)point.X >= X && (float)point.X <= X + Width && (float)point.Y >= Y && (float)point.Y <= Y + Height)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object" /> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="T:System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(RectangleF other)
		{
			if (other.X == X && other.Y == Y && other.Width == Width)
			{
				return other.Height == Height;
			}
			return false;
		}

		/// <summary>
		/// Creates a Rectangle defining the area where one rectangle overlaps with another rectangle.
		/// </summary>
		/// <param name="value1">The first Rectangle to compare.</param>
		/// <param name="value2">The second Rectangle to compare.</param>
		/// <param name="result">[OutAttribute] The area where the two first parameters overlap.</param>
		public static bool Intersect(ref RectangleF value1, ref RectangleF value2, out RectangleF result)
		{
			float num = value1.X + value1.Width;
			float num2 = value2.X + value2.Width;
			float num3 = value1.Y + value1.Height;
			float num4 = value2.Y + value2.Height;
			float num5 = (value1.X > value2.X) ? value1.X : value2.X;
			float num6 = (value1.Y > value2.Y) ? value1.Y : value2.Y;
			float num7 = (num < num2) ? num : num2;
			float num8 = (num3 < num4) ? num3 : num4;
			if (num7 > num5 && num8 > num6)
			{
				result = new RectangleF(num5, num6, num7 - num5, num8 - num6);
				return true;
			}
			result = new RectangleF(0f, 0f, 0f, 0f);
			return false;
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != typeof(RectangleF))
			{
				return false;
			}
			return Equals((RectangleF)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return (((((X.GetHashCode() * 397) ^ Y.GetHashCode()) * 397) ^ Width.GetHashCode()) * 397) ^ Height.GetHashCode();
		}

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator ==(RectangleF left, RectangleF right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator !=(RectangleF left, RectangleF right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return $"(X: {X} Y: {Y} W: {Width} H: {Height})";
		}
	}
}
