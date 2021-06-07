using System;
using System.Collections.Generic;
using System.Globalization;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Represents a GPS coordinate
	/// </summary>
	public struct MyWaypointInfo : IEquatable<MyWaypointInfo>
	{
		/// <summary>
		/// Returns an empty (undefined) GPS coordinate
		/// </summary>
		public static MyWaypointInfo Empty;

		/// <summary>
		/// The name of this GPS coordinate
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// Gets the target coordinate as a <see cref="T:VRageMath.Vector3D" />
		/// </summary>
		public Vector3D Coords;

		private static bool IsPrecededByWhitespace(ref TextPtr ptr)
		{
			TextPtr textPtr = ptr - 1;
			char @char = textPtr.Char;
			if (!textPtr.IsOutOfBounds() && !char.IsWhiteSpace(@char))
			{
				return !char.IsLetterOrDigit(@char);
			}
			return true;
		}

		/// <summary>
		/// Searches for all GPS coordinates in the given text.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="gpsList"></param>
		/// <returns></returns>
		public static void FindAll(string source, List<MyWaypointInfo> gpsList)
		{
			TextPtr ptr = new TextPtr(source);
			gpsList.Clear();
			while (!ptr.IsOutOfBounds())
			{
				if (char.ToUpperInvariant(ptr.Char) == 'G' && IsPrecededByWhitespace(ref ptr) && TryParse(ref ptr, out MyWaypointInfo gps))
				{
					gpsList.Add(gps);
				}
				else
				{
					++ptr;
				}
			}
		}

		/// <summary>
		/// <para>
		/// Attempts to parse a GPS coordinate from the given text. The text cannot contain anything but the GPS coordinate.
		/// </para>
		/// <para>
		/// A GPS coordinate has the format GPS:Name:X:Y:Z:
		/// </para>
		/// </summary>
		/// <param name="text"></param>
		/// <param name="gps"></param>
		/// <returns></returns>
		public static bool TryParse(string text, out MyWaypointInfo gps)
		{
			if (text == null)
			{
				gps = Empty;
				return false;
			}
			TextPtr ptr = new TextPtr(text);
			bool flag = TryParse(ref ptr, out gps);
			if (flag && !ptr.IsOutOfBounds())
			{
				gps = Empty;
				return false;
			}
			return flag;
		}

		private static bool TryParse(ref TextPtr ptr, out MyWaypointInfo gps)
		{
			while (char.IsWhiteSpace(ptr.Char))
			{
				++ptr;
			}
			if (!ptr.StartsWithCaseInsensitive("gps:"))
			{
				gps = Empty;
				return false;
			}
			ptr += 4;
			if (!GrabSegment(ref ptr, out StringSegment segment))
			{
				gps = Empty;
				return false;
			}
			if (!GrabSegment(ref ptr, out StringSegment segment2))
			{
				gps = Empty;
				return false;
			}
			if (!GrabSegment(ref ptr, out StringSegment segment3))
			{
				gps = Empty;
				return false;
			}
			if (!GrabSegment(ref ptr, out StringSegment segment4))
			{
				gps = Empty;
				return false;
			}
			while (char.IsWhiteSpace(ptr.Char))
			{
				++ptr;
			}
			if (!double.TryParse(segment2.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
			{
				gps = Empty;
				return false;
			}
			if (!double.TryParse(segment3.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double result2))
			{
				gps = Empty;
				return false;
			}
			if (!double.TryParse(segment4.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double result3))
			{
				gps = Empty;
				return false;
			}
			string name = segment.ToString();
			gps = new MyWaypointInfo(name, result, result2, result3);
			return true;
		}

		private static bool GrabSegment(ref TextPtr ptr, out StringSegment segment)
		{
			if (ptr.IsOutOfBounds())
			{
				segment = default(StringSegment);
				return false;
			}
			TextPtr textPtr = ptr;
			while (!ptr.IsOutOfBounds() && ptr.Char != ':')
			{
				++ptr;
			}
			if (ptr.Char != ':')
			{
				segment = default(StringSegment);
				return false;
			}
			segment = new StringSegment(textPtr.Content, textPtr.Index, ptr.Index - textPtr.Index);
			++ptr;
			return true;
		}

		/// <summary>
		/// Creates a new GPS coordinate
		/// </summary>
		/// <param name="name"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public MyWaypointInfo(string name, double x, double y, double z)
		{
			Name = (name ?? "");
			Coords = new Vector3D(x, y, z);
		}

		/// <summary>
		/// Creates a new GPS coordinate
		/// </summary>
		/// <param name="name"></param>
		/// <param name="coords"></param>
		public MyWaypointInfo(string name, Vector3D coords)
			: this(name, coords.X, coords.Y, coords.Z)
		{
		}

		/// <summary>
		/// Determines whether this coordinate is empty (undefined)
		/// </summary>
		/// <returns></returns>
		public bool IsEmpty()
		{
			return Name == null;
		}

		/// <summary>
		/// Converts this GPS coordinate to its string equivalent
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "GPS:{0}:{1:R}:{2:R}:{3:R}:", Name, Coords.X, Coords.Y, Coords.Z);
		}

		/// <summary>
		/// Determines whether this coordinate is the same as another. Uses 0.0001 as the epsilon to counter floating point inaccuracies.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(MyWaypointInfo other)
		{
			return Equals(other, 0.0001);
		}

		/// <summary>
		/// Determines whether this coordinate is the same as another. Uses 0.0001 as the epsilon to counter floating point inaccuracies.
		/// </summary>
		/// <param name="other"></param>
		/// <param name="epsilon">The epsilon (tolerance) of this comparison</param>
		/// <returns></returns>
		public bool Equals(MyWaypointInfo other, double epsilon)
		{
			if (string.Equals(Name, other.Name) && Math.Abs(Coords.X - other.Coords.X) < epsilon && Math.Abs(Coords.Y - other.Coords.Y) < epsilon)
			{
				return Math.Abs(Coords.Z - other.Coords.Z) < epsilon;
			}
			return false;
		}

		/// <summary>
		/// Determines whether this coordinate is the same as another. Uses 0.0001 as the epsilon to counter floating point inaccuracies.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is MyWaypointInfo)
			{
				return Equals((MyWaypointInfo)obj);
			}
			return false;
		}

		/// <summary>
		/// Gets the hashcode of this coordinate
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			int num = (Name != null) ? Name.GetHashCode() : 0;
			num = ((num * 397) ^ Coords.X.GetHashCode());
			num = ((num * 397) ^ Coords.Y.GetHashCode());
			return (num * 397) ^ Coords.Z.GetHashCode();
		}
	}
}
