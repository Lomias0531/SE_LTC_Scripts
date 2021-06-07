using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRageMath
{
	[ProtoContract]
	public struct SerializableRange
	{
		protected class VRageMath_SerializableRange_003C_003EMin_003C_003EAccessor : IMemberAccessor<SerializableRange, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableRange owner, in float value)
			{
				owner.Min = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableRange owner, out float value)
			{
				value = owner.Min;
			}
		}

		protected class VRageMath_SerializableRange_003C_003EMax_003C_003EAccessor : IMemberAccessor<SerializableRange, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableRange owner, in float value)
			{
				owner.Max = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableRange owner, out float value)
			{
				value = owner.Max;
			}
		}

		[ProtoMember(1)]
		[XmlAttribute(AttributeName = "Min")]
		public float Min;

		[ProtoMember(4)]
		[XmlAttribute(AttributeName = "Max")]
		public float Max;

		public SerializableRange(float min, float max)
		{
			Max = max;
			Min = min;
		}

		public bool ValueBetween(float value)
		{
			if (value >= Min)
			{
				return value <= Max;
			}
			return false;
		}

		public override string ToString()
		{
			return $"Range[{Min}, {Max}]";
		}

		/// When the range is an angle this method changes it to the cosines of the angle.
		///
		/// The angle is expected to be in degrees.
		///
		/// Also beware that cosine is a decreasing function in [0,90], for that reason the minimum and maximum are swaped.
		public SerializableRange ConvertToCosine()
		{
			float max = Max;
			Max = (float)Math.Cos((double)Min * Math.PI / 180.0);
			Min = (float)Math.Cos((double)max * Math.PI / 180.0);
			return this;
		}

		/// When the range is an angle this method changes it to the sines of the angle.
		///
		/// The angle is expected to be in degrees.
		public SerializableRange ConvertToSine()
		{
			Max = (float)Math.Sin((double)Max * Math.PI / 180.0);
			Min = (float)Math.Sin((double)Min * Math.PI / 180.0);
			return this;
		}

		public SerializableRange ConvertToCosineLongitude()
		{
			Max = MathHelper.MonotonicCosine((float)((double)Max * Math.PI / 180.0));
			Min = MathHelper.MonotonicCosine((float)((double)Min * Math.PI / 180.0));
			return this;
		}

		public string ToStringAsin()
		{
			return $"Range[{MathHelper.ToDegrees(Math.Asin(Min))}, {MathHelper.ToDegrees(Math.Asin(Max))}]";
		}

		public string ToStringAcos()
		{
			return $"Range[{MathHelper.ToDegrees(Math.Acos(Min))}, {MathHelper.ToDegrees(Math.Acos(Max))}]";
		}

		public string ToStringLongitude()
		{
			return $"Range[{MathHelper.ToDegrees(MathHelper.MonotonicAcos(Min))}, {MathHelper.ToDegrees(MathHelper.MonotonicAcos(Max))}]";
		}
	}
}
