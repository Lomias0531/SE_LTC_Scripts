using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Represents a point in a multi-point curve.
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class CurveKey : IEquatable<CurveKey>, IComparable<CurveKey>
	{
		protected class VRageMath_CurveKey_003C_003Eposition_003C_003EAccessor : IMemberAccessor<CurveKey, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CurveKey owner, in float value)
			{
				owner.position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CurveKey owner, out float value)
			{
				value = owner.position;
			}
		}

		protected class VRageMath_CurveKey_003C_003EinternalValue_003C_003EAccessor : IMemberAccessor<CurveKey, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CurveKey owner, in float value)
			{
				owner.internalValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CurveKey owner, out float value)
			{
				value = owner.internalValue;
			}
		}

		protected class VRageMath_CurveKey_003C_003EtangentOut_003C_003EAccessor : IMemberAccessor<CurveKey, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CurveKey owner, in float value)
			{
				owner.tangentOut = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CurveKey owner, out float value)
			{
				value = owner.tangentOut;
			}
		}

		protected class VRageMath_CurveKey_003C_003EtangentIn_003C_003EAccessor : IMemberAccessor<CurveKey, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CurveKey owner, in float value)
			{
				owner.tangentIn = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CurveKey owner, out float value)
			{
				value = owner.tangentIn;
			}
		}

		protected class VRageMath_CurveKey_003C_003Econtinuity_003C_003EAccessor : IMemberAccessor<CurveKey, CurveContinuity>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CurveKey owner, in CurveContinuity value)
			{
				owner.continuity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CurveKey owner, out CurveContinuity value)
			{
				value = owner.continuity;
			}
		}

		protected class VRageMath_CurveKey_003C_003EValue_003C_003EAccessor : IMemberAccessor<CurveKey, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CurveKey owner, in float value)
			{
				owner.Value = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CurveKey owner, out float value)
			{
				value = owner.Value;
			}
		}

		protected class VRageMath_CurveKey_003C_003ETangentIn_003C_003EAccessor : IMemberAccessor<CurveKey, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CurveKey owner, in float value)
			{
				owner.TangentIn = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CurveKey owner, out float value)
			{
				value = owner.TangentIn;
			}
		}

		protected class VRageMath_CurveKey_003C_003ETangentOut_003C_003EAccessor : IMemberAccessor<CurveKey, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CurveKey owner, in float value)
			{
				owner.TangentOut = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CurveKey owner, out float value)
			{
				value = owner.TangentOut;
			}
		}

		protected class VRageMath_CurveKey_003C_003EContinuity_003C_003EAccessor : IMemberAccessor<CurveKey, CurveContinuity>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CurveKey owner, in CurveContinuity value)
			{
				owner.Continuity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CurveKey owner, out CurveContinuity value)
			{
				value = owner.Continuity;
			}
		}

		internal float position;

		internal float internalValue;

		internal float tangentOut;

		internal float tangentIn;

		internal CurveContinuity continuity;

		/// <summary>
		/// Position of the CurveKey in the curve.
		/// </summary>
		public float Position => position;

		/// <summary>
		/// Describes the value of this point.
		/// </summary>
		public float Value
		{
			get
			{
				return internalValue;
			}
			set
			{
				internalValue = value;
			}
		}

		/// <summary>
		/// Describes the tangent when approaching this point from the previous point in the curve.
		/// </summary>
		public float TangentIn
		{
			get
			{
				return tangentIn;
			}
			set
			{
				tangentIn = value;
			}
		}

		/// <summary>
		/// Describes the tangent when leaving this point to the next point in the curve.
		/// </summary>
		public float TangentOut
		{
			get
			{
				return tangentOut;
			}
			set
			{
				tangentOut = value;
			}
		}

		/// <summary>
		/// Describes whether the segment between this point and the next point in the curve is discrete or continuous.
		/// </summary>
		public CurveContinuity Continuity
		{
			get
			{
				return continuity;
			}
			set
			{
				continuity = value;
			}
		}

		public CurveKey()
		{
		}

		/// <summary>
		/// Initializes a new instance of CurveKey.
		/// </summary>
		/// <param name="position">Position in the curve.</param><param name="value">Value of the control point.</param>
		public CurveKey(float position, float value)
		{
			this.position = position;
			internalValue = value;
		}

		/// <summary>
		/// Initializes a new instance of CurveKey.
		/// </summary>
		/// <param name="position">Position in the curve.</param><param name="value">Value of the control point.</param><param name="tangentIn">Tangent approaching point from the previous point in the curve.</param><param name="tangentOut">Tangent leaving point toward next point in the curve.</param>
		public CurveKey(float position, float value, float tangentIn, float tangentOut)
		{
			this.position = position;
			internalValue = value;
			this.tangentIn = tangentIn;
			this.tangentOut = tangentOut;
		}

		/// <summary>
		/// Initializes a new instance of CurveKey.
		/// </summary>
		/// <param name="position">Position in the curve.</param><param name="value">Value of the control point.</param><param name="tangentIn">Tangent approaching point from the previous point in the curve.</param><param name="tangentOut">Tangent leaving point toward next point in the curve.</param><param name="continuity">Enum indicating whether the curve is discrete or continuous.</param>
		public CurveKey(float position, float value, float tangentIn, float tangentOut, CurveContinuity continuity)
		{
			this.position = position;
			internalValue = value;
			this.tangentIn = tangentIn;
			this.tangentOut = tangentOut;
			this.continuity = continuity;
		}

		/// <summary>
		/// Determines whether two CurveKey instances are equal.
		/// </summary>
		/// <param name="a">CurveKey on the left of the equal sign.</param><param name="b">CurveKey on the right of the equal sign.</param>
		public static bool operator ==(CurveKey a, CurveKey b)
		{
			bool flag = (object)a == null;
			bool flag2 = (object)b == null;
			if (!(flag || flag2))
			{
				return a.Equals(b);
			}
			return flag == flag2;
		}

		/// <summary>
		/// Determines whether two CurveKey instances are not equal.
		/// </summary>
		/// <param name="a">CurveKey on the left of the equal sign.</param><param name="b">CurveKey on the right of the equal sign.</param>
		public static bool operator !=(CurveKey a, CurveKey b)
		{
			bool flag = a == null;
			bool flag2 = b == null;
			if (!(flag || flag2))
			{
				if ((double)a.position == (double)b.position && (double)a.internalValue == (double)b.internalValue && (double)a.tangentIn == (double)b.tangentIn && (double)a.tangentOut == (double)b.tangentOut)
				{
					return a.continuity != b.continuity;
				}
				return true;
			}
			return flag != flag2;
		}

		/// <summary>
		/// Creates a copy of the CurveKey.
		/// </summary>
		public CurveKey Clone()
		{
			return new CurveKey(position, internalValue, tangentIn, tangentOut, continuity);
		}

		/// <summary>
		/// Determines whether the specified Object is equal to the CurveKey.
		/// </summary>
		/// <param name="other">The Object to compare with the current CurveKey.</param>
		public bool Equals(CurveKey other)
		{
			if (other != null && (double)other.position == (double)position && (double)other.internalValue == (double)internalValue && (double)other.tangentIn == (double)tangentIn && (double)other.tangentOut == (double)tangentOut)
			{
				return other.continuity == continuity;
			}
			return false;
		}

		/// <summary>
		/// Returns a value that indicates whether the current instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">Object with which to make the comparison.</param>
		public override bool Equals(object obj)
		{
			return Equals(obj as CurveKey);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		public override int GetHashCode()
		{
			return position.GetHashCode() + internalValue.GetHashCode() + tangentIn.GetHashCode() + tangentOut.GetHashCode() + continuity.GetHashCode();
		}

		/// <summary>
		/// Compares this instance to another CurveKey and returns an indication of their relative values.
		/// </summary>
		/// <param name="other">CurveKey to compare to.</param>
		public int CompareTo(CurveKey other)
		{
			if ((double)position == (double)other.position)
			{
				return 0;
			}
			if (!((double)position >= (double)other.position))
			{
				return -1;
			}
			return 1;
		}
	}
}
