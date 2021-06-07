using System;

namespace VRageMath
{
	public struct LineD
	{
		public Vector3D From;

		public Vector3D To;

		public Vector3D Direction;

		public double Length;

		public LineD(Vector3D from, Vector3D to)
		{
			From = from;
			To = to;
			Direction = to - from;
			Length = Direction.Normalize();
		}

		public LineD(Vector3D from, Vector3D to, double lineLength)
		{
			From = from;
			To = to;
			Length = lineLength;
			Direction = (to - from) / lineLength;
		}

		public static double GetShortestDistanceSquared(LineD line1, LineD line2)
		{
			Vector3D res;
			Vector3D res2;
			Vector3D shortestVector = GetShortestVector(ref line1, ref line2, out res, out res2);
			return Vector3D.Dot(shortestVector, shortestVector);
		}

		public static Vector3D GetShortestVector(ref LineD line1, ref LineD line2, out Vector3D res1, out Vector3D res2)
		{
			double num = 9.9999999747524271E-07;
			Vector3D vector3D = default(Vector3D);
			vector3D.X = line1.To.X - line1.From.X;
			vector3D.Y = line1.To.Y - line1.From.Y;
			vector3D.Z = line1.To.Z - line1.From.Z;
			Vector3D vector3D2 = default(Vector3D);
			vector3D2.X = line2.To.X - line2.From.X;
			vector3D2.Y = line2.To.Y - line2.From.Y;
			vector3D2.Z = line2.To.Z - line2.From.Z;
			Vector3D vector = default(Vector3D);
			vector.X = line1.From.X - line2.From.X;
			vector.Y = line1.From.Y - line2.From.Y;
			vector.Z = line1.From.Z - line2.From.Z;
			double num2 = Vector3D.Dot(vector3D, vector3D);
			double num3 = Vector3D.Dot(vector3D, vector3D2);
			double num4 = Vector3D.Dot(vector3D2, vector3D2);
			double num5 = Vector3D.Dot(vector3D, vector);
			double num6 = Vector3D.Dot(vector3D2, vector);
			double num8;
			double num7;
			double num9;
			double num10;
			if ((num8 = (num7 = num2 * num4 - num3 * num3)) < num)
			{
				num9 = 0.0;
				num7 = 1.0;
				num10 = num6;
				num8 = num4;
			}
			else
			{
				num9 = num3 * num6 - num4 * num5;
				num10 = num2 * num6 - num3 * num5;
				if (num9 < 0.0)
				{
					num9 = 0.0;
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
			if (num10 < 0.0)
			{
				num10 = 0.0;
				if (0.0 - num5 < 0.0)
				{
					num9 = 0.0;
				}
				else if (0.0 - num5 > num2)
				{
					num9 = num7;
				}
				else
				{
					num9 = 0.0 - num5;
					num7 = num2;
				}
			}
			else if (num10 > num8)
			{
				num10 = num8;
				if (0.0 - num5 + num3 < 0.0)
				{
					num9 = 0.0;
				}
				else if (0.0 - num5 + num3 > num2)
				{
					num9 = num7;
				}
				else
				{
					num9 = 0.0 - num5 + num3;
					num7 = num2;
				}
			}
			double num11 = (!(Math.Abs(num9) < num)) ? (num9 / num7) : 0.0;
			double num12 = (!(Math.Abs(num10) < num)) ? (num10 / num8) : 0.0;
			res1.X = num11 * vector3D.X;
			res1.Y = num11 * vector3D.Y;
			res1.Z = num11 * vector3D.Z;
			Vector3D vector3D3 = default(Vector3D);
			vector3D3.X = vector.X - num12 * vector3D2.X + res1.X;
			vector3D3.Y = vector.Y - num12 * vector3D2.Y + res1.Y;
			vector3D3.Z = vector.Z - num12 * vector3D2.Z + res1.Z;
			res2 = res1 - vector3D3;
			return vector3D3;
		}

		public static explicit operator Line(LineD b)
		{
			return new Line(b.From, b.To);
		}

		public static explicit operator LineD(Line b)
		{
			return new LineD(b.From, b.To);
		}

		public BoundingBoxD GetBoundingBox()
		{
			return new BoundingBoxD(Vector3D.Min(From, To), Vector3D.Max(From, To));
		}

		public long GetHash()
		{
			return From.GetHash() ^ To.GetHash();
		}
	}
}
