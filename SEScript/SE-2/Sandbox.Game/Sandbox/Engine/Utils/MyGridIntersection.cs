using BulletXNA;
using System;
using System.Collections.Generic;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Utils
{
	public class MyGridIntersection
	{
		private static bool IsPointInside(Vector3 p, Vector3I min, Vector3I max)
		{
			if (p.X >= (float)min.X && p.X < (float)(max.X + 1) && p.Y >= (float)min.Y && p.Y < (float)(max.Y + 1) && p.Z >= (float)min.Z)
			{
				return p.Z < (float)(max.Z + 1);
			}
			return false;
		}

		private static bool IntersectionT(double n, double d, ref double tE, ref double tL)
		{
			if (MyUtils.IsZero(d))
			{
				return n <= 0.0;
			}
			double num = n / d;
			if (d > 0.0)
			{
				if (num > tL)
				{
					return false;
				}
				if (num > tE)
				{
					tE = num;
				}
			}
			else
			{
				if (num < tE)
				{
					return false;
				}
				if (num < tL)
				{
					tL = num;
				}
			}
			return true;
		}

		private static bool ClipLine(ref Vector3D start, ref Vector3D end, Vector3I min, Vector3I max)
		{
			Vector3D value = end - start;
			if (MyUtils.IsZero(value))
			{
				return IsPointInside(start, min, max);
			}
			double tE = 0.0;
			double tL = 1.0;
			if (IntersectionT((double)min.X - start.X, value.X, ref tE, ref tL) && IntersectionT(start.X - (double)max.X - 1.0, 0.0 - value.X, ref tE, ref tL) && IntersectionT((double)min.Y - start.Y, value.Y, ref tE, ref tL) && IntersectionT(start.Y - (double)max.Y - 1.0, 0.0 - value.Y, ref tE, ref tL) && IntersectionT((double)min.Z - start.Z, value.Z, ref tE, ref tL) && IntersectionT(start.Z - (double)max.Z - 1.0, 0.0 - value.Z, ref tE, ref tL))
			{
				if (tL < 1.0)
				{
					end = start + tL * value;
				}
				if (tE > 0.0)
				{
					start += tE * value;
				}
				return true;
			}
			return false;
		}

		private static Vector3I SignInt(Vector3 v)
		{
			return new Vector3I((v.X >= 0f) ? 1 : (-1), (v.Y >= 0f) ? 1 : (-1), (v.Z >= 0f) ? 1 : (-1));
		}

		private static Vector3 Sign(Vector3 v)
		{
			return new Vector3((v.X >= 0f) ? 1 : (-1), (v.Y >= 0f) ? 1 : (-1), (v.Z >= 0f) ? 1 : (-1));
		}

		private static Vector3I GetGridPoint(ref Vector3D v, Vector3I min, Vector3I max)
		{
			Vector3I result = default(Vector3I);
			if (v.X < (double)min.X)
			{
				v.X = (result.X = min.X);
			}
			else if (v.X >= (double)(max.X + 1))
			{
				v.X = MathUtil.NextAfter(max.X + 1, float.NegativeInfinity);
				result.X = max.X;
			}
			else
			{
				result.X = (int)Math.Floor(v.X);
			}
			if (v.Y < (double)min.Y)
			{
				v.Y = (result.Y = min.Y);
			}
			else if (v.Y >= (double)(max.Y + 1))
			{
				v.Y = MathUtil.NextAfter(max.Y + 1, float.NegativeInfinity);
				result.Y = max.Y;
			}
			else
			{
				result.Y = (int)Math.Floor(v.Y);
			}
			if (v.Z < (double)min.Z)
			{
				v.Z = (result.Z = min.Z);
			}
			else if (v.Z >= (double)(max.Z + 1))
			{
				v.Z = MathUtil.NextAfter(max.Z + 1, float.NegativeInfinity);
				result.Z = max.Z;
			}
			else
			{
				result.Z = (int)Math.Floor(v.Z);
			}
			return result;
		}

		public static void CalculateHavok(List<Vector3I> result, float gridSize, Vector3D lineStart, Vector3D lineEnd, Vector3I min, Vector3I max)
		{
			Vector3D vector3D = Vector3D.Normalize(lineEnd - lineStart);
			Vector3D vector3D2 = Vector3D.Normalize(Vector3D.CalculatePerpendicularVector(vector3D)) * 0.059999998658895493;
			Vector3D value = Vector3D.Normalize(Vector3D.Cross(vector3D, vector3D2)) * 0.06;
			Calculate(result, gridSize, lineStart + vector3D2, lineEnd + vector3D2, min, max);
			Calculate(result, gridSize, lineStart - vector3D2, lineEnd - vector3D2, min, max);
			Calculate(result, gridSize, lineStart + value, lineEnd + value, min, max);
			Calculate(result, gridSize, lineStart - value, lineEnd - value, min, max);
		}

		public static void Calculate(List<Vector3I> result, float gridSize, Vector3D lineStart, Vector3D lineEnd, Vector3I min, Vector3I max)
		{
			Vector3D vector3D = lineEnd - lineStart;
			Vector3D v = lineStart / gridSize;
			if (MyUtils.IsZero(vector3D))
			{
				if (IsPointInside(v, min, max))
				{
					result.Add(GetGridPoint(ref v, min, max));
				}
				return;
			}
			Vector3D end = lineEnd / gridSize;
			if (!ClipLine(ref v, ref end, min, max))
			{
				return;
			}
			Vector3 vector = Sign(vector3D);
			Vector3I b = SignInt(vector3D);
			Vector3I a = GetGridPoint(ref v, min, max) * b;
			Vector3I vector3I = GetGridPoint(ref end, min, max) * b;
			vector3D *= vector;
			v *= vector;
			double num = 1.0 / vector3D.X;
			double num2 = num * (Math.Floor(v.X + 1.0) - v.X);
			double num3 = 1.0 / vector3D.Y;
			double num4 = num3 * (Math.Floor(v.Y + 1.0) - v.Y);
			double num5 = 1.0 / vector3D.Z;
			double num6 = num5 * (Math.Floor(v.Z + 1.0) - v.Z);
			while (true)
			{
				result.Add(a * b);
				if (num2 < num6)
				{
					if (num2 < num4)
					{
						num2 += num;
						if (++a.X > vector3I.X)
						{
							break;
						}
					}
					else
					{
						num4 += num3;
						if (++a.Y > vector3I.Y)
						{
							break;
						}
					}
				}
				else if (num6 < num4)
				{
					num6 += num5;
					if (++a.Z > vector3I.Z)
					{
						break;
					}
				}
				else
				{
					num4 += num3;
					if (++a.Y > vector3I.Y)
					{
						break;
					}
				}
			}
		}
	}
}
