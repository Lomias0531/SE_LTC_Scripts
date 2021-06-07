using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Stores an arbitrary collection of 2D CurveKey points, and provides methods for evaluating features of the curve they define.
	/// </summary>
	[Serializable]
	public class Curve
	{
		protected class VRageMath_Curve_003C_003Ekeys_003C_003EAccessor : IMemberAccessor<Curve, CurveKeyCollection>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Curve owner, in CurveKeyCollection value)
			{
				owner.keys = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Curve owner, out CurveKeyCollection value)
			{
				value = owner.keys;
			}
		}

		protected class VRageMath_Curve_003C_003EpreLoop_003C_003EAccessor : IMemberAccessor<Curve, CurveLoopType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Curve owner, in CurveLoopType value)
			{
				owner.preLoop = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Curve owner, out CurveLoopType value)
			{
				value = owner.preLoop;
			}
		}

		protected class VRageMath_Curve_003C_003EpostLoop_003C_003EAccessor : IMemberAccessor<Curve, CurveLoopType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Curve owner, in CurveLoopType value)
			{
				owner.postLoop = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Curve owner, out CurveLoopType value)
			{
				value = owner.postLoop;
			}
		}

		protected class VRageMath_Curve_003C_003EPreLoop_003C_003EAccessor : IMemberAccessor<Curve, CurveLoopType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Curve owner, in CurveLoopType value)
			{
				owner.PreLoop = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Curve owner, out CurveLoopType value)
			{
				value = owner.PreLoop;
			}
		}

		protected class VRageMath_Curve_003C_003EPostLoop_003C_003EAccessor : IMemberAccessor<Curve, CurveLoopType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Curve owner, in CurveLoopType value)
			{
				owner.PostLoop = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Curve owner, out CurveLoopType value)
			{
				value = owner.PostLoop;
			}
		}

		private CurveKeyCollection keys = new CurveKeyCollection();

		private CurveLoopType preLoop;

		private CurveLoopType postLoop;

		/// <summary>
		/// Specifies how to handle weighting values that are less than the first control point in the curve.
		/// </summary>
		public CurveLoopType PreLoop
		{
			get
			{
				return preLoop;
			}
			set
			{
				preLoop = value;
			}
		}

		/// <summary>
		/// Specifies how to handle weighting values that are greater than the last control point in the curve.
		/// </summary>
		public CurveLoopType PostLoop
		{
			get
			{
				return postLoop;
			}
			set
			{
				postLoop = value;
			}
		}

		/// <summary>
		/// The points that make up the curve.
		/// </summary>
		public CurveKeyCollection Keys => keys;

		/// <summary>
		/// Gets a value indicating whether the curve is constant.
		/// </summary>
		public bool IsConstant => keys.Count <= 1;

		/// <summary>
		/// Creates a copy of the Curve.
		/// </summary>
		public Curve Clone()
		{
			return new Curve
			{
				preLoop = preLoop,
				postLoop = postLoop,
				keys = keys.Clone()
			};
		}

		/// <summary>
		/// Computes both the TangentIn and the TangentOut for a CurveKey specified by its index.
		/// </summary>
		/// <param name="keyIndex">The index of the CurveKey for which to compute tangents (in the Keys collection of the Curve).</param><param name="tangentType">The type of tangents to compute (one of the types specified in the CurveTangent enumeration).</param>
		public void ComputeTangent(int keyIndex, CurveTangent tangentType)
		{
			ComputeTangent(keyIndex, tangentType, tangentType);
		}

		/// <summary>
		/// Computes a specified type of TangentIn and a specified type of TangentOut for a given CurveKey.
		/// </summary>
		/// <param name="keyIndex">The index of the CurveKey for which to compute tangents (in the Keys collection of the Curve).</param><param name="tangentInType">The type of TangentIn to compute (one of the types specified in the CurveTangent enumeration).</param><param name="tangentOutType">The type of TangentOut to compute (one of the types specified in the CurveTangent enumeration).</param>
		public void ComputeTangent(int keyIndex, CurveTangent tangentInType, CurveTangent tangentOutType)
		{
			if (keys.Count <= keyIndex || keyIndex < 0)
			{
				throw new ArgumentOutOfRangeException("keyIndex");
			}
			CurveKey curveKey = Keys[keyIndex];
			double num = curveKey.Position;
			float num2 = (float)num;
			float num3 = (float)num;
			float num4 = (float)num;
			double num5 = curveKey.Value;
			float num6 = (float)num5;
			float num7 = (float)num5;
			float num8 = (float)num5;
			if (keyIndex > 0)
			{
				num4 = Keys[keyIndex - 1].Position;
				num8 = Keys[keyIndex - 1].Value;
			}
			if (keyIndex + 1 < keys.Count)
			{
				num2 = Keys[keyIndex + 1].Position;
				num6 = Keys[keyIndex + 1].Value;
			}
			if (tangentInType == CurveTangent.Smooth)
			{
				float num9 = num2 - num4;
				float num10 = num6 - num8;
				curveKey.TangentIn = (((double)Math.Abs(num10) >= 1.19209289550781E-07) ? (num10 * Math.Abs(num4 - num3) / num9) : 0f);
			}
			else
			{
				curveKey.TangentIn = ((tangentInType != CurveTangent.Linear) ? 0f : (num7 - num8));
			}
			switch (tangentOutType)
			{
			case CurveTangent.Smooth:
			{
				float num11 = num2 - num4;
				float num12 = num6 - num8;
				if ((double)Math.Abs(num12) < 1.19209289550781E-07)
				{
					curveKey.TangentOut = 0f;
				}
				else
				{
					curveKey.TangentOut = num12 * Math.Abs(num2 - num3) / num11;
				}
				break;
			}
			case CurveTangent.Linear:
				curveKey.TangentOut = num6 - num7;
				break;
			default:
				curveKey.TangentOut = 0f;
				break;
			}
		}

		/// <summary>
		/// Computes all tangents for all CurveKeys in this Curve, using a specified tangent type for both TangentIn and TangentOut.
		/// </summary>
		/// <param name="tangentType">The type of TangentOut and TangentIn to compute (one of the types specified in the CurveTangent enumeration).</param>
		public void ComputeTangents(CurveTangent tangentType)
		{
			ComputeTangents(tangentType, tangentType);
		}

		/// <summary>
		/// Computes all tangents for all CurveKeys in this Curve, using different tangent types for TangentOut and TangentIn.
		/// </summary>
		/// <param name="tangentInType">The type of TangentIn to compute (one of the types specified in the CurveTangent enumeration).</param><param name="tangentOutType">The type of TangentOut to compute (one of the types specified in the CurveTangent enumeration).</param>
		public void ComputeTangents(CurveTangent tangentInType, CurveTangent tangentOutType)
		{
			for (int i = 0; i < Keys.Count; i++)
			{
				ComputeTangent(i, tangentInType, tangentOutType);
			}
		}

		/// <summary>
		/// Finds the value at a position on the Curve.
		/// </summary>
		/// <param name="position">The position on the Curve.</param>
		public float Evaluate(float position)
		{
			if (keys.Count == 0)
			{
				return 0f;
			}
			if (keys.Count == 1)
			{
				return keys[0].internalValue;
			}
			CurveKey curveKey = keys[0];
			CurveKey curveKey2 = keys[keys.Count - 1];
			float num = position;
			float num2 = 0f;
			if ((double)num < (double)curveKey.position)
			{
				if (preLoop == CurveLoopType.Constant)
				{
					return curveKey.internalValue;
				}
				if (preLoop == CurveLoopType.Linear)
				{
					return curveKey.internalValue - curveKey.tangentIn * (curveKey.position - num);
				}
				if (!keys.IsCacheAvailable)
				{
					keys.ComputeCacheValues();
				}
				float num3 = CalcCycle(num);
				float num4 = num - (curveKey.position + num3 * keys.TimeRange);
				if (preLoop == CurveLoopType.Cycle)
				{
					num = curveKey.position + num4;
				}
				else if (preLoop == CurveLoopType.CycleOffset)
				{
					num = curveKey.position + num4;
					num2 = (curveKey2.internalValue - curveKey.internalValue) * num3;
				}
				else
				{
					num = ((((int)num3 & 1) != 0) ? (curveKey2.position - num4) : (curveKey.position + num4));
				}
			}
			else if ((double)curveKey2.position < (double)num)
			{
				if (postLoop == CurveLoopType.Constant)
				{
					return curveKey2.internalValue;
				}
				if (postLoop == CurveLoopType.Linear)
				{
					return curveKey2.internalValue - curveKey2.tangentOut * (curveKey2.position - num);
				}
				if (!keys.IsCacheAvailable)
				{
					keys.ComputeCacheValues();
				}
				float num5 = CalcCycle(num);
				float num6 = num - (curveKey.position + num5 * keys.TimeRange);
				if (postLoop == CurveLoopType.Cycle)
				{
					num = curveKey.position + num6;
				}
				else if (postLoop == CurveLoopType.CycleOffset)
				{
					num = curveKey.position + num6;
					num2 = (curveKey2.internalValue - curveKey.internalValue) * num5;
				}
				else
				{
					num = ((((int)num5 & 1) != 0) ? (curveKey2.position - num6) : (curveKey.position + num6));
				}
			}
			CurveKey k = null;
			CurveKey k2 = null;
			float t = FindSegment(num, ref k, ref k2);
			return num2 + Hermite(k, k2, t);
		}

		private float CalcCycle(float t)
		{
			float num = (t - keys[0].position) * keys.InvTimeRange;
			if ((double)num < 0.0)
			{
				num -= 1f;
			}
			return (int)num;
		}

		private float FindSegment(float t, ref CurveKey k0, ref CurveKey k1)
		{
			float result = t;
			k0 = keys[0];
			for (int i = 1; i < keys.Count; i++)
			{
				k1 = keys[i];
				if ((double)k1.position >= (double)t)
				{
					double num = k0.position;
					double num2 = k1.position;
					double num3 = t;
					double num4 = num2 - num;
					result = 0f;
					if (num4 > 0.0)
					{
						result = (float)((num3 - num) / num4);
					}
					break;
				}
				k0 = k1;
			}
			return result;
		}

		private static float Hermite(CurveKey k0, CurveKey k1, float t)
		{
			if (k0.Continuity == CurveContinuity.Step)
			{
				if ((double)t >= 1.0)
				{
					return k1.internalValue;
				}
				return k0.internalValue;
			}
			float num = t * t;
			float num2 = num * t;
			float internalValue = k0.internalValue;
			float internalValue2 = k1.internalValue;
			float tangentOut = k0.tangentOut;
			float tangentIn = k1.tangentIn;
			return (float)((double)internalValue * (2.0 * (double)num2 - 3.0 * (double)num + 1.0) + (double)internalValue2 * (-2.0 * (double)num2 + 3.0 * (double)num) + (double)tangentOut * ((double)num2 - 2.0 * (double)num + (double)t) + (double)tangentIn * ((double)num2 - (double)num));
		}
	}
}
