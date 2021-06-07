using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRageMath
{
	/// Reflective because it can be reflected to the opposite range.
	[ProtoContract]
	public struct SymmetricSerializableRange
	{
		protected class VRageMath_SymmetricSerializableRange_003C_003EMin_003C_003EAccessor : IMemberAccessor<SymmetricSerializableRange, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SymmetricSerializableRange owner, in float value)
			{
				owner.Min = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SymmetricSerializableRange owner, out float value)
			{
				value = owner.Min;
			}
		}

		protected class VRageMath_SymmetricSerializableRange_003C_003EMax_003C_003EAccessor : IMemberAccessor<SymmetricSerializableRange, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SymmetricSerializableRange owner, in float value)
			{
				owner.Max = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SymmetricSerializableRange owner, out float value)
			{
				value = owner.Max;
			}
		}

		protected class VRageMath_SymmetricSerializableRange_003C_003Em_notMirror_003C_003EAccessor : IMemberAccessor<SymmetricSerializableRange, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SymmetricSerializableRange owner, in bool value)
			{
				owner.m_notMirror = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SymmetricSerializableRange owner, out bool value)
			{
				value = owner.m_notMirror;
			}
		}

		protected class VRageMath_SymmetricSerializableRange_003C_003EMirror_003C_003EAccessor : IMemberAccessor<SymmetricSerializableRange, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SymmetricSerializableRange owner, in bool value)
			{
				owner.Mirror = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SymmetricSerializableRange owner, out bool value)
			{
				value = owner.Mirror;
			}
		}

		[XmlAttribute(AttributeName = "Min")]
		public float Min;

		[XmlAttribute(AttributeName = "Max")]
		public float Max;

		private bool m_notMirror;

		[XmlAttribute(AttributeName = "Mirror")]
		public bool Mirror
		{
			get
			{
				return !m_notMirror;
			}
			set
			{
				m_notMirror = !value;
			}
		}

		public SymmetricSerializableRange(float min, float max, bool mirror = true)
		{
			Max = max;
			Min = min;
			m_notMirror = !mirror;
		}

		public bool ValueBetween(float value)
		{
			if (!m_notMirror)
			{
				value = Math.Abs(value);
			}
			if (value >= Min)
			{
				return value <= Max;
			}
			return false;
		}

		public override string ToString()
		{
			return string.Format("{0}[{1}, {2}]", Mirror ? "MirroredRange" : "Range", Min, Max);
		}

		/// When the range is an angle this method changes it to the cosines of the angle.
		///
		/// The angle is expected to be in degrees.
		///
		/// Also beware that cosine is a decreasing function in [0,90], for that reason the minimum and maximum are swaped.
		public SymmetricSerializableRange ConvertToCosine()
		{
			float max = Max;
			Max = (float)Math.Cos((double)Min * Math.PI / 180.0);
			Min = (float)Math.Cos((double)max * Math.PI / 180.0);
			return this;
		}

		/// When the range is an angle this method changes it to the sines of the angle.
		///
		/// The angle is expected to be in degrees.
		public SymmetricSerializableRange ConvertToSine()
		{
			Max = (float)Math.Sin((double)Max * Math.PI / 180.0);
			Min = (float)Math.Sin((double)Min * Math.PI / 180.0);
			return this;
		}

		public SymmetricSerializableRange ConvertToCosineLongitude()
		{
			Max = CosineLongitude(Max);
			Min = CosineLongitude(Min);
			return this;
		}

		private static float CosineLongitude(float angle)
		{
			if (angle > 0f)
			{
				return 2f - (float)Math.Cos((double)angle * Math.PI / 180.0);
			}
			return (float)Math.Cos((double)angle * Math.PI / 180.0);
		}

		public string ToStringAsin()
		{
			return $"Range[{MathHelper.ToDegrees(Math.Asin(Min))}, {MathHelper.ToDegrees(Math.Asin(Max))}]";
		}

		public string ToStringAcos()
		{
			return $"Range[{MathHelper.ToDegrees(Math.Acos(Min))}, {MathHelper.ToDegrees(Math.Acos(Max))}]";
		}
	}
}
