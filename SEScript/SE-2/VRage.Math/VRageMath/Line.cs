using System;

namespace VRageMath
{
	public struct Line
	{
		public Vector3 From;

		public Vector3 To;

		public Vector3 Direction;

		public float Length;

		public BoundingBox BoundingBox;

		public Line(Vector3 from, Vector3 to, bool calculateBoundingBox = true)
		{
			From = from;
			To = to;
			Direction = to - from;
			Length = Direction.Normalize();
			BoundingBox = BoundingBox.CreateInvalid();
			if (calculateBoundingBox)
			{
				BoundingBox = BoundingBox.Include(ref from);
				BoundingBox = BoundingBox.Include(ref to);
			}
		}

		public static float GetShortestDistanceSquared(Line line1, Line line2)
		{
			Vector3 res;
			Vector3 res2;
			Vector3 shortestVector = GetShortestVector(ref line1, ref line2, out res, out res2);
			return Vector3.Dot(shortestVector, shortestVector);
		}

		public static Vector3 GetShortestVector(ref Line line1, ref Line line2, out Vector3 res1, out Vector3 res2)
		{
			float num = 1E-06f;
			Vector3 vector = default(Vector3);
			vector.X = line1.To.X - line1.From.X;
			vector.Y = line1.To.Y - line1.From.Y;
			vector.Z = line1.To.Z - line1.From.Z;
			Vector3 vector2 = default(Vector3);
			vector2.X = line2.To.X - line2.From.X;
			vector2.Y = line2.To.Y - line2.From.Y;
			vector2.Z = line2.To.Z - line2.From.Z;
			Vector3 vector3 = default(Vector3);
			vector3.X = line1.From.X - line2.From.X;
			vector3.Y = line1.From.Y - line2.From.Y;
			vector3.Z = line1.From.Z - line2.From.Z;
			float num2 = Vector3.Dot(vector, vector);
			float num3 = Vector3.Dot(vector, vector2);
			float num4 = Vector3.Dot(vector2, vector2);
			float num5 = Vector3.Dot(vector, vector3);
			float num6 = Vector3.Dot(vector2, vector3);
			float num8;
			float num7;
			float num9;
			float num10;
			if ((num8 = (num7 = num2 * num4 - num3 * num3)) < num)
			{
				num9 = 0f;
				num7 = 1f;
				num10 = num6;
				num8 = num4;
			}
			else
			{
				num9 = num3 * num6 - num4 * num5;
				num10 = num2 * num6 - num3 * num5;
				if ((double)num9 < 0.0)
				{
					num9 = 0f;
					num10 = num6;
					num8 = num4;
				}
				else if (num9 > num7)
				{
					num9 = num7;
					num10 = num6 + num3;
					num8 = num4;
				}
			}
			if ((double)num10 < 0.0)
			{
				num10 = 0f;
				if (0f - num5 < 0f)
				{
					num9 = 0f;
				}
				else if (0f - num5 > num2)
				{
					num9 = num7;
				}
				else
				{
					num9 = 0f - num5;
					num7 = num2;
				}
			}
			else if (num10 > num8)
			{
				num10 = num8;
				if ((double)(0f - num5 + num3) < 0.0)
				{
					num9 = 0f;
				}
				else if (0f - num5 + num3 > num2)
				{
					num9 = num7;
				}
				else
				{
					num9 = 0f - num5 + num3;
					num7 = num2;
				}
			}
			float num11 = (!(Math.Abs(num9) < num)) ? (num9 / num7) : 0f;
			float num12 = (!(Math.Abs(num10) < num)) ? (num10 / num8) : 0f;
			res1.X = num11 * vector.X;
			res1.Y = num11 * vector.Y;
			res1.Z = num11 * vector.Z;
			Vector3 vector4 = default(Vector3);
			vector4.X = vector3.X - num12 * vector2.X + res1.X;
			vector4.Y = vector3.Y - num12 * vector2.Y + res1.Y;
			vector4.Z = vector3.Z - num12 * vector2.Z + res1.Z;
			res2 = res1 - vector4;
			return vector4;
		}
	}
}
