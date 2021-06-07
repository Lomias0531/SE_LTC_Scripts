using System;

namespace VRageMath
{
	public class Base6Directions
	{
		public enum Direction : byte
		{
			Forward,
			Backward,
			Left,
			Right,
			Up,
			Down
		}

		[Flags]
		public enum DirectionFlags : byte
		{
			Forward = 0x1,
			Backward = 0x2,
			Left = 0x4,
			Right = 0x8,
			Up = 0x10,
			Down = 0x20,
			All = 0x3F
		}

		public enum Axis : byte
		{
			ForwardBackward,
			LeftRight,
			UpDown
		}

		public static readonly Direction[] EnumDirections = new Direction[6]
		{
			Direction.Forward,
			Direction.Backward,
			Direction.Left,
			Direction.Right,
			Direction.Up,
			Direction.Down
		};

		public static readonly Vector3[] Directions = new Vector3[6]
		{
			Vector3.Forward,
			Vector3.Backward,
			Vector3.Left,
			Vector3.Right,
			Vector3.Up,
			Vector3.Down
		};

		public static readonly Vector3I[] IntDirections = new Vector3I[6]
		{
			Vector3I.Forward,
			Vector3I.Backward,
			Vector3I.Left,
			Vector3I.Right,
			Vector3I.Up,
			Vector3I.Down
		};

		/// <summary>
		/// Pre-calculated left directions for given forward (index / 6) and up (index % 6) directions
		/// </summary>
		private static readonly Direction[] LeftDirections = new Direction[36]
		{
			Direction.Forward,
			Direction.Forward,
			Direction.Down,
			Direction.Up,
			Direction.Left,
			Direction.Right,
			Direction.Forward,
			Direction.Forward,
			Direction.Up,
			Direction.Down,
			Direction.Right,
			Direction.Left,
			Direction.Up,
			Direction.Down,
			Direction.Left,
			Direction.Left,
			Direction.Backward,
			Direction.Forward,
			Direction.Down,
			Direction.Up,
			Direction.Left,
			Direction.Left,
			Direction.Forward,
			Direction.Backward,
			Direction.Right,
			Direction.Left,
			Direction.Forward,
			Direction.Backward,
			Direction.Left,
			Direction.Right,
			Direction.Left,
			Direction.Right,
			Direction.Backward,
			Direction.Forward,
			Direction.Left,
			Direction.Right
		};

		private const float DIRECTION_EPSILON = 1E-05f;

		private static readonly int[] ForwardBackward = new int[3]
		{
			0,
			0,
			1
		};

		private static readonly int[] LeftRight = new int[3]
		{
			2,
			0,
			3
		};

		private static readonly int[] UpDown = new int[3]
		{
			5,
			0,
			4
		};

		private Base6Directions()
		{
		}

		public static bool IsBaseDirection(ref Vector3 vec)
		{
			return vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z - 1f < 1E-05f;
		}

		public static bool IsBaseDirection(Vector3 vec)
		{
			return IsBaseDirection(ref vec);
		}

		public static bool IsBaseDirection(ref Vector3I vec)
		{
			return vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z - 1 == 0;
		}

		public static Vector3 GetVector(int direction)
		{
			direction %= 6;
			return Directions[direction];
		}

		public static Vector3 GetVector(Direction dir)
		{
			return GetVector((int)dir);
		}

		public static Vector3I GetIntVector(int direction)
		{
			direction %= 6;
			return IntDirections[direction];
		}

		public static Vector3I GetIntVector(Direction dir)
		{
			int num = (int)dir % 6;
			return IntDirections[num];
		}

		public static void GetVector(Direction dir, out Vector3 result)
		{
			int num = (int)dir % 6;
			result = Directions[num];
		}

		public static DirectionFlags GetDirectionFlag(Direction dir)
		{
			return (DirectionFlags)(1 << (int)dir);
		}

		public static Direction GetPerpendicular(Direction dir)
		{
			if (GetAxis(dir) == Axis.UpDown)
			{
				return Direction.Right;
			}
			return Direction.Up;
		}

		public static Direction GetDirection(Vector3 vec)
		{
			return GetDirection(ref vec);
		}

		public static Direction GetDirection(ref Vector3 vec)
		{
			return (Direction)(0 + ForwardBackward[(int)Math.Round(vec.Z + 1f)] + LeftRight[(int)Math.Round(vec.X + 1f)] + UpDown[(int)Math.Round(vec.Y + 1f)]);
		}

		public static Direction GetDirection(Vector3I vec)
		{
			return GetDirection(ref vec);
		}

		public static Direction GetDirection(ref Vector3I vec)
		{
			return (Direction)(0 + ForwardBackward[vec.Z + 1] + LeftRight[vec.X + 1] + UpDown[vec.Y + 1]);
		}

		public static Direction GetClosestDirection(Vector3 vec)
		{
			return GetClosestDirection(ref vec);
		}

		public static Direction GetClosestDirection(ref Vector3 vec)
		{
			Vector3 value = Vector3.DominantAxisProjection(vec);
			value = Vector3.Sign(value);
			return GetDirection(ref value);
		}

		public static Direction GetDirectionInAxis(Vector3 vec, Axis axis)
		{
			return GetDirectionInAxis(ref vec, axis);
		}

		public static Direction GetDirectionInAxis(ref Vector3 vec, Axis axis)
		{
			Direction baseAxisDirection = GetBaseAxisDirection(axis);
			Vector3 vector = IntDirections[(uint)baseAxisDirection];
			vector *= vec;
			if (vector.X + vector.Y + vector.Z >= 1f)
			{
				return baseAxisDirection;
			}
			return GetFlippedDirection(baseAxisDirection);
		}

		public static Direction GetForward(Quaternion rot)
		{
			Vector3.Transform(ref Vector3.Forward, ref rot, out Vector3 result);
			return GetDirection(ref result);
		}

		public static Direction GetForward(ref Quaternion rot)
		{
			Vector3.Transform(ref Vector3.Forward, ref rot, out Vector3 result);
			return GetDirection(ref result);
		}

		public static Direction GetForward(ref Matrix rotation)
		{
			Vector3.TransformNormal(ref Vector3.Forward, ref rotation, out Vector3 result);
			return GetDirection(ref result);
		}

		public static Direction GetUp(Quaternion rot)
		{
			Vector3.Transform(ref Vector3.Up, ref rot, out Vector3 result);
			return GetDirection(ref result);
		}

		public static Direction GetUp(ref Quaternion rot)
		{
			Vector3.Transform(ref Vector3.Up, ref rot, out Vector3 result);
			return GetDirection(ref result);
		}

		public static Direction GetUp(ref Matrix rotation)
		{
			Vector3.TransformNormal(ref Vector3.Up, ref rotation, out Vector3 result);
			return GetDirection(ref result);
		}

		public static Axis GetAxis(Direction direction)
		{
			return (Axis)((int)direction >> 1);
		}

		public static Direction GetBaseAxisDirection(Axis axis)
		{
			return (Direction)((uint)axis << 1);
		}

		public static Direction GetFlippedDirection(Direction toFlip)
		{
			return toFlip ^ Direction.Backward;
		}

		public static Direction GetCross(Direction dir1, Direction dir2)
		{
			return GetLeft(dir1, dir2);
		}

		public static Direction GetLeft(Direction up, Direction forward)
		{
			return LeftDirections[(int)forward * 6 + (int)up];
		}

		public static Direction GetOppositeDirection(Direction dir)
		{
			switch (dir)
			{
			default:
				return Direction.Backward;
			case Direction.Backward:
				return Direction.Forward;
			case Direction.Up:
				return Direction.Down;
			case Direction.Down:
				return Direction.Up;
			case Direction.Left:
				return Direction.Right;
			case Direction.Right:
				return Direction.Left;
			}
		}

		public static Quaternion GetOrientation(Direction forward, Direction up)
		{
			Vector3 vector = GetVector(forward);
			Vector3 vector2 = GetVector(up);
			return Quaternion.CreateFromForwardUp(vector, vector2);
		}

		public static bool IsValidBlockOrientation(Direction forward, Direction up)
		{
			if ((int)forward <= 5 && (int)up <= 5)
			{
				return Vector3.Dot(GetVector(forward), GetVector(up)) == 0f;
			}
			return false;
		}
	}
}
