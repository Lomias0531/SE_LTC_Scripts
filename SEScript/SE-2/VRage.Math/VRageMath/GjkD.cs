using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	[Serializable]
	internal class GjkD
	{
		protected class VRageMath_GjkD_003C_003EclosestPoint_003C_003EAccessor : IMemberAccessor<GjkD, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref GjkD owner, in Vector3D value)
			{
				owner.closestPoint = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref GjkD owner, out Vector3D value)
			{
				value = owner.closestPoint;
			}
		}

		protected class VRageMath_GjkD_003C_003Ey_003C_003EAccessor : IMemberAccessor<GjkD, Vector3D[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref GjkD owner, in Vector3D[] value)
			{
				owner.y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref GjkD owner, out Vector3D[] value)
			{
				value = owner.y;
			}
		}

		protected class VRageMath_GjkD_003C_003EyLengthSq_003C_003EAccessor : IMemberAccessor<GjkD, double[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref GjkD owner, in double[] value)
			{
				owner.yLengthSq = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref GjkD owner, out double[] value)
			{
				value = owner.yLengthSq;
			}
		}

		protected class VRageMath_GjkD_003C_003Eedges_003C_003EAccessor : IMemberAccessor<GjkD, Vector3D[][]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref GjkD owner, in Vector3D[][] value)
			{
				owner.edges = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref GjkD owner, out Vector3D[][] value)
			{
				value = owner.edges;
			}
		}

		protected class VRageMath_GjkD_003C_003EedgeLengthSq_003C_003EAccessor : IMemberAccessor<GjkD, double[][]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref GjkD owner, in double[][] value)
			{
				owner.edgeLengthSq = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref GjkD owner, out double[][] value)
			{
				value = owner.edgeLengthSq;
			}
		}

		protected class VRageMath_GjkD_003C_003Edet_003C_003EAccessor : IMemberAccessor<GjkD, double[][]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref GjkD owner, in double[][] value)
			{
				owner.det = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref GjkD owner, out double[][] value)
			{
				value = owner.det;
			}
		}

		protected class VRageMath_GjkD_003C_003EsimplexBits_003C_003EAccessor : IMemberAccessor<GjkD, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref GjkD owner, in int value)
			{
				owner.simplexBits = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref GjkD owner, out int value)
			{
				value = owner.simplexBits;
			}
		}

		protected class VRageMath_GjkD_003C_003EmaxLengthSq_003C_003EAccessor : IMemberAccessor<GjkD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref GjkD owner, in double value)
			{
				owner.maxLengthSq = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref GjkD owner, out double value)
			{
				value = owner.maxLengthSq;
			}
		}

		private static int[] BitsToIndices;

		private Vector3D closestPoint;

		private Vector3D[] y;

		private double[] yLengthSq;

		private Vector3D[][] edges;

		private double[][] edgeLengthSq;

		private double[][] det;

		private int simplexBits;

		private double maxLengthSq;

		public bool FullSimplex => simplexBits == 15;

		public double MaxLengthSquared => maxLengthSq;

		public Vector3D ClosestPoint => closestPoint;

		static GjkD()
		{
			BitsToIndices = new int[16]
			{
				0,
				1,
				2,
				17,
				3,
				25,
				26,
				209,
				4,
				33,
				34,
				273,
				35,
				281,
				282,
				2257
			};
		}

		public GjkD()
		{
			y = new Vector3D[4];
			yLengthSq = new double[4];
			edges = new Vector3D[4][]
			{
				new Vector3D[4],
				new Vector3D[4],
				new Vector3D[4],
				new Vector3D[4]
			};
			edgeLengthSq = new double[4][]
			{
				new double[4],
				new double[4],
				new double[4],
				new double[4]
			};
			det = new double[16][];
			for (int i = 0; i < 16; i++)
			{
				det[i] = new double[4];
			}
		}

		public void Reset()
		{
			simplexBits = 0;
			maxLengthSq = 0.0;
		}

		public bool AddSupportPoint(ref Vector3D newPoint)
		{
			int num = (BitsToIndices[simplexBits ^ 0xF] & 7) - 1;
			y[num] = newPoint;
			yLengthSq[num] = newPoint.LengthSquared();
			for (int num2 = BitsToIndices[simplexBits]; num2 != 0; num2 >>= 3)
			{
				int num3 = (num2 & 7) - 1;
				Vector3D vector3D = y[num3] - newPoint;
				edges[num3][num] = vector3D;
				edges[num][num3] = -vector3D;
				edgeLengthSq[num][num3] = (edgeLengthSq[num3][num] = vector3D.LengthSquared());
			}
			UpdateDeterminant(num);
			return UpdateSimplex(num);
		}

		private static double Dot(ref Vector3D a, ref Vector3D b)
		{
			return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
		}

		private void UpdateDeterminant(int xmIdx)
		{
			int num = 1 << xmIdx;
			det[num][xmIdx] = 1.0;
			int num2 = BitsToIndices[simplexBits];
			int num3 = num2;
			int num4 = 0;
			while (num3 != 0)
			{
				int num5 = (num3 & 7) - 1;
				int num6 = 1 << num5;
				int num7 = num6 | num;
				det[num7][num5] = Dot(ref edges[xmIdx][num5], ref y[xmIdx]);
				det[num7][xmIdx] = Dot(ref edges[num5][xmIdx], ref y[num5]);
				int num8 = num2;
				for (int i = 0; i < num4; i++)
				{
					int num9 = (num8 & 7) - 1;
					int num10 = 1 << num9;
					int num11 = num7 | num10;
					int num12 = (edgeLengthSq[num5][num9] < edgeLengthSq[xmIdx][num9]) ? num5 : xmIdx;
					det[num11][num9] = det[num7][num5] * Dot(ref edges[num12][num9], ref y[num5]) + det[num7][xmIdx] * Dot(ref edges[num12][num9], ref y[xmIdx]);
					int num13 = (edgeLengthSq[num9][num5] < edgeLengthSq[xmIdx][num5]) ? num9 : xmIdx;
					det[num11][num5] = det[num10 | num][num9] * Dot(ref edges[num13][num5], ref y[num9]) + det[num10 | num][xmIdx] * Dot(ref edges[num13][num5], ref y[xmIdx]);
					int num14 = (edgeLengthSq[num5][xmIdx] < edgeLengthSq[num9][xmIdx]) ? num5 : num9;
					det[num11][xmIdx] = det[num6 | num10][num9] * Dot(ref edges[num14][xmIdx], ref y[num9]) + det[num6 | num10][num5] * Dot(ref edges[num14][xmIdx], ref y[num5]);
					num8 >>= 3;
				}
				num3 >>= 3;
				num4++;
			}
			if ((simplexBits | num) == 15)
			{
				int num15 = (!(edgeLengthSq[1][0] < edgeLengthSq[2][0])) ? ((edgeLengthSq[2][0] < edgeLengthSq[3][0]) ? 2 : 3) : ((edgeLengthSq[1][0] < edgeLengthSq[3][0]) ? 1 : 3);
				det[15][0] = det[14][1] * Dot(ref edges[num15][0], ref y[1]) + det[14][2] * Dot(ref edges[num15][0], ref y[2]) + det[14][3] * Dot(ref edges[num15][0], ref y[3]);
				int num16 = (!(edgeLengthSq[0][1] < edgeLengthSq[2][1])) ? ((edgeLengthSq[2][1] < edgeLengthSq[3][1]) ? 2 : 3) : ((!(edgeLengthSq[0][1] < edgeLengthSq[3][1])) ? 3 : 0);
				det[15][1] = det[13][0] * Dot(ref edges[num16][1], ref y[0]) + det[13][2] * Dot(ref edges[num16][1], ref y[2]) + det[13][3] * Dot(ref edges[num16][1], ref y[3]);
				int num17 = (!(edgeLengthSq[0][2] < edgeLengthSq[1][2])) ? ((edgeLengthSq[1][2] < edgeLengthSq[3][2]) ? 1 : 3) : ((!(edgeLengthSq[0][2] < edgeLengthSq[3][2])) ? 3 : 0);
				det[15][2] = det[11][0] * Dot(ref edges[num17][2], ref y[0]) + det[11][1] * Dot(ref edges[num17][2], ref y[1]) + det[11][3] * Dot(ref edges[num17][2], ref y[3]);
				int num18 = (!(edgeLengthSq[0][3] < edgeLengthSq[1][3])) ? ((edgeLengthSq[1][3] < edgeLengthSq[2][3]) ? 1 : 2) : ((!(edgeLengthSq[0][3] < edgeLengthSq[2][3])) ? 2 : 0);
				det[15][3] = det[7][0] * Dot(ref edges[num18][3], ref y[0]) + det[7][1] * Dot(ref edges[num18][3], ref y[1]) + det[7][2] * Dot(ref edges[num18][3], ref y[2]);
			}
		}

		private bool UpdateSimplex(int newIndex)
		{
			int num = simplexBits | (1 << newIndex);
			int num2 = 1 << newIndex;
			for (int num3 = simplexBits; num3 != 0; num3--)
			{
				if ((num3 & num) == num3 && IsSatisfiesRule(num3 | num2, num))
				{
					simplexBits = (num3 | num2);
					closestPoint = ComputeClosestPoint();
					return true;
				}
			}
			bool result = false;
			if (IsSatisfiesRule(num2, num))
			{
				simplexBits = num2;
				closestPoint = y[newIndex];
				maxLengthSq = yLengthSq[newIndex];
				result = true;
			}
			return result;
		}

		private Vector3D ComputeClosestPoint()
		{
			double num = 0.0;
			Vector3D zero = Vector3D.Zero;
			maxLengthSq = 0.0;
			for (int num2 = BitsToIndices[simplexBits]; num2 != 0; num2 >>= 3)
			{
				int num3 = (num2 & 7) - 1;
				double num4 = det[simplexBits][num3];
				num += num4;
				zero += y[num3] * num4;
				maxLengthSq = MathHelper.Max(maxLengthSq, yLengthSq[num3]);
			}
			return zero / num;
		}

		private bool IsSatisfiesRule(int xBits, int yBits)
		{
			bool result = true;
			for (int num = BitsToIndices[yBits]; num != 0; num >>= 3)
			{
				int num2 = (num & 7) - 1;
				int num3 = 1 << num2;
				if ((num3 & xBits) != 0)
				{
					if (det[xBits][num2] <= 0.0)
					{
						result = false;
						break;
					}
				}
				else if (det[xBits | num3][num2] > 0.0)
				{
					result = false;
					break;
				}
			}
			return result;
		}
	}
}
