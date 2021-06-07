using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	[Serializable]
	internal class Gjk
	{
		protected class VRageMath_Gjk_003C_003EclosestPoint_003C_003EAccessor : IMemberAccessor<Gjk, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Gjk owner, in Vector3 value)
			{
				owner.closestPoint = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Gjk owner, out Vector3 value)
			{
				value = owner.closestPoint;
			}
		}

		protected class VRageMath_Gjk_003C_003Ey_003C_003EAccessor : IMemberAccessor<Gjk, Vector3[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Gjk owner, in Vector3[] value)
			{
				owner.y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Gjk owner, out Vector3[] value)
			{
				value = owner.y;
			}
		}

		protected class VRageMath_Gjk_003C_003EyLengthSq_003C_003EAccessor : IMemberAccessor<Gjk, float[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Gjk owner, in float[] value)
			{
				owner.yLengthSq = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Gjk owner, out float[] value)
			{
				value = owner.yLengthSq;
			}
		}

		protected class VRageMath_Gjk_003C_003Eedges_003C_003EAccessor : IMemberAccessor<Gjk, Vector3[][]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Gjk owner, in Vector3[][] value)
			{
				owner.edges = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Gjk owner, out Vector3[][] value)
			{
				value = owner.edges;
			}
		}

		protected class VRageMath_Gjk_003C_003EedgeLengthSq_003C_003EAccessor : IMemberAccessor<Gjk, float[][]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Gjk owner, in float[][] value)
			{
				owner.edgeLengthSq = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Gjk owner, out float[][] value)
			{
				value = owner.edgeLengthSq;
			}
		}

		protected class VRageMath_Gjk_003C_003Edet_003C_003EAccessor : IMemberAccessor<Gjk, float[][]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Gjk owner, in float[][] value)
			{
				owner.det = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Gjk owner, out float[][] value)
			{
				value = owner.det;
			}
		}

		protected class VRageMath_Gjk_003C_003EsimplexBits_003C_003EAccessor : IMemberAccessor<Gjk, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Gjk owner, in int value)
			{
				owner.simplexBits = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Gjk owner, out int value)
			{
				value = owner.simplexBits;
			}
		}

		protected class VRageMath_Gjk_003C_003EmaxLengthSq_003C_003EAccessor : IMemberAccessor<Gjk, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Gjk owner, in float value)
			{
				owner.maxLengthSq = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Gjk owner, out float value)
			{
				value = owner.maxLengthSq;
			}
		}

		private static int[] BitsToIndices;

		private Vector3 closestPoint;

		private Vector3[] y;

		private float[] yLengthSq;

		private Vector3[][] edges;

		private float[][] edgeLengthSq;

		private float[][] det;

		private int simplexBits;

		private float maxLengthSq;

		public bool FullSimplex => simplexBits == 15;

		public float MaxLengthSquared => maxLengthSq;

		public Vector3 ClosestPoint => closestPoint;

		static Gjk()
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

		public Gjk()
		{
			y = new Vector3[4];
			yLengthSq = new float[4];
			edges = new Vector3[4][]
			{
				new Vector3[4],
				new Vector3[4],
				new Vector3[4],
				new Vector3[4]
			};
			edgeLengthSq = new float[4][]
			{
				new float[4],
				new float[4],
				new float[4],
				new float[4]
			};
			det = new float[16][];
			for (int i = 0; i < 16; i++)
			{
				det[i] = new float[4];
			}
		}

		public void Reset()
		{
			simplexBits = 0;
			maxLengthSq = 0f;
		}

		public bool AddSupportPoint(ref Vector3 newPoint)
		{
			int num = (BitsToIndices[simplexBits ^ 0xF] & 7) - 1;
			y[num] = newPoint;
			yLengthSq[num] = newPoint.LengthSquared();
			for (int num2 = BitsToIndices[simplexBits]; num2 != 0; num2 >>= 3)
			{
				int num3 = (num2 & 7) - 1;
				Vector3 vector = y[num3] - newPoint;
				edges[num3][num] = vector;
				edges[num][num3] = -vector;
				edgeLengthSq[num][num3] = (edgeLengthSq[num3][num] = vector.LengthSquared());
			}
			UpdateDeterminant(num);
			return UpdateSimplex(num);
		}

		private static float Dot(ref Vector3 a, ref Vector3 b)
		{
			return (float)((double)a.X * (double)b.X + (double)a.Y * (double)b.Y + (double)a.Z * (double)b.Z);
		}

		private void UpdateDeterminant(int xmIdx)
		{
			int num = 1 << xmIdx;
			det[num][xmIdx] = 1f;
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
					int num12 = ((double)edgeLengthSq[num5][num9] < (double)edgeLengthSq[xmIdx][num9]) ? num5 : xmIdx;
					det[num11][num9] = (float)((double)det[num7][num5] * (double)Dot(ref edges[num12][num9], ref y[num5]) + (double)det[num7][xmIdx] * (double)Dot(ref edges[num12][num9], ref y[xmIdx]));
					int num13 = ((double)edgeLengthSq[num9][num5] < (double)edgeLengthSq[xmIdx][num5]) ? num9 : xmIdx;
					det[num11][num5] = (float)((double)det[num10 | num][num9] * (double)Dot(ref edges[num13][num5], ref y[num9]) + (double)det[num10 | num][xmIdx] * (double)Dot(ref edges[num13][num5], ref y[xmIdx]));
					int num14 = ((double)edgeLengthSq[num5][xmIdx] < (double)edgeLengthSq[num9][xmIdx]) ? num5 : num9;
					det[num11][xmIdx] = (float)((double)det[num6 | num10][num9] * (double)Dot(ref edges[num14][xmIdx], ref y[num9]) + (double)det[num6 | num10][num5] * (double)Dot(ref edges[num14][xmIdx], ref y[num5]));
					num8 >>= 3;
				}
				num3 >>= 3;
				num4++;
			}
			if ((simplexBits | num) == 15)
			{
				int num15 = (!((double)edgeLengthSq[1][0] < (double)edgeLengthSq[2][0])) ? (((double)edgeLengthSq[2][0] < (double)edgeLengthSq[3][0]) ? 2 : 3) : (((double)edgeLengthSq[1][0] < (double)edgeLengthSq[3][0]) ? 1 : 3);
				det[15][0] = (float)((double)det[14][1] * (double)Dot(ref edges[num15][0], ref y[1]) + (double)det[14][2] * (double)Dot(ref edges[num15][0], ref y[2]) + (double)det[14][3] * (double)Dot(ref edges[num15][0], ref y[3]));
				int num16 = (!((double)edgeLengthSq[0][1] < (double)edgeLengthSq[2][1])) ? (((double)edgeLengthSq[2][1] < (double)edgeLengthSq[3][1]) ? 2 : 3) : ((!((double)edgeLengthSq[0][1] < (double)edgeLengthSq[3][1])) ? 3 : 0);
				det[15][1] = (float)((double)det[13][0] * (double)Dot(ref edges[num16][1], ref y[0]) + (double)det[13][2] * (double)Dot(ref edges[num16][1], ref y[2]) + (double)det[13][3] * (double)Dot(ref edges[num16][1], ref y[3]));
				int num17 = (!((double)edgeLengthSq[0][2] < (double)edgeLengthSq[1][2])) ? (((double)edgeLengthSq[1][2] < (double)edgeLengthSq[3][2]) ? 1 : 3) : ((!((double)edgeLengthSq[0][2] < (double)edgeLengthSq[3][2])) ? 3 : 0);
				det[15][2] = (float)((double)det[11][0] * (double)Dot(ref edges[num17][2], ref y[0]) + (double)det[11][1] * (double)Dot(ref edges[num17][2], ref y[1]) + (double)det[11][3] * (double)Dot(ref edges[num17][2], ref y[3]));
				int num18 = (!((double)edgeLengthSq[0][3] < (double)edgeLengthSq[1][3])) ? (((double)edgeLengthSq[1][3] < (double)edgeLengthSq[2][3]) ? 1 : 2) : ((!((double)edgeLengthSq[0][3] < (double)edgeLengthSq[2][3])) ? 2 : 0);
				det[15][3] = (float)((double)det[7][0] * (double)Dot(ref edges[num18][3], ref y[0]) + (double)det[7][1] * (double)Dot(ref edges[num18][3], ref y[1]) + (double)det[7][2] * (double)Dot(ref edges[num18][3], ref y[2]));
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

		private Vector3 ComputeClosestPoint()
		{
			float num = 0f;
			Vector3 zero = Vector3.Zero;
			maxLengthSq = 0f;
			for (int num2 = BitsToIndices[simplexBits]; num2 != 0; num2 >>= 3)
			{
				int num3 = (num2 & 7) - 1;
				float num4 = det[simplexBits][num3];
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
					if ((double)det[xBits][num2] <= 0.0)
					{
						result = false;
						break;
					}
				}
				else if ((double)det[xBits | num3][num2] > 0.0)
				{
					result = false;
					break;
				}
			}
			return result;
		}
	}
}
