using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRageRender.Messages
{
	[ProtoContract]
	public struct MyAtmosphereSettings
	{
		protected class VRageRender_Messages_MyAtmosphereSettings_003C_003ERayleighScattering_003C_003EAccessor : IMemberAccessor<MyAtmosphereSettings, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereSettings owner, in Vector3 value)
			{
				owner.RayleighScattering = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereSettings owner, out Vector3 value)
			{
				value = owner.RayleighScattering;
			}
		}

		protected class VRageRender_Messages_MyAtmosphereSettings_003C_003EMieScattering_003C_003EAccessor : IMemberAccessor<MyAtmosphereSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereSettings owner, in float value)
			{
				owner.MieScattering = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereSettings owner, out float value)
			{
				value = owner.MieScattering;
			}
		}

		protected class VRageRender_Messages_MyAtmosphereSettings_003C_003EMieColorScattering_003C_003EAccessor : IMemberAccessor<MyAtmosphereSettings, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereSettings owner, in Vector3 value)
			{
				owner.MieColorScattering = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereSettings owner, out Vector3 value)
			{
				value = owner.MieColorScattering;
			}
		}

		protected class VRageRender_Messages_MyAtmosphereSettings_003C_003ERayleighHeight_003C_003EAccessor : IMemberAccessor<MyAtmosphereSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereSettings owner, in float value)
			{
				owner.RayleighHeight = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereSettings owner, out float value)
			{
				value = owner.RayleighHeight;
			}
		}

		protected class VRageRender_Messages_MyAtmosphereSettings_003C_003ERayleighHeightSpace_003C_003EAccessor : IMemberAccessor<MyAtmosphereSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereSettings owner, in float value)
			{
				owner.RayleighHeightSpace = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereSettings owner, out float value)
			{
				value = owner.RayleighHeightSpace;
			}
		}

		protected class VRageRender_Messages_MyAtmosphereSettings_003C_003ERayleighTransitionModifier_003C_003EAccessor : IMemberAccessor<MyAtmosphereSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereSettings owner, in float value)
			{
				owner.RayleighTransitionModifier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereSettings owner, out float value)
			{
				value = owner.RayleighTransitionModifier;
			}
		}

		protected class VRageRender_Messages_MyAtmosphereSettings_003C_003EMieHeight_003C_003EAccessor : IMemberAccessor<MyAtmosphereSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereSettings owner, in float value)
			{
				owner.MieHeight = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereSettings owner, out float value)
			{
				value = owner.MieHeight;
			}
		}

		protected class VRageRender_Messages_MyAtmosphereSettings_003C_003EMieG_003C_003EAccessor : IMemberAccessor<MyAtmosphereSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereSettings owner, in float value)
			{
				owner.MieG = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereSettings owner, out float value)
			{
				value = owner.MieG;
			}
		}

		protected class VRageRender_Messages_MyAtmosphereSettings_003C_003EIntensity_003C_003EAccessor : IMemberAccessor<MyAtmosphereSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereSettings owner, in float value)
			{
				owner.Intensity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereSettings owner, out float value)
			{
				value = owner.Intensity;
			}
		}

		protected class VRageRender_Messages_MyAtmosphereSettings_003C_003EFogIntensity_003C_003EAccessor : IMemberAccessor<MyAtmosphereSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereSettings owner, in float value)
			{
				owner.FogIntensity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereSettings owner, out float value)
			{
				value = owner.FogIntensity;
			}
		}

		protected class VRageRender_Messages_MyAtmosphereSettings_003C_003ESeaLevelModifier_003C_003EAccessor : IMemberAccessor<MyAtmosphereSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereSettings owner, in float value)
			{
				owner.SeaLevelModifier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereSettings owner, out float value)
			{
				value = owner.SeaLevelModifier;
			}
		}

		protected class VRageRender_Messages_MyAtmosphereSettings_003C_003EAtmosphereTopModifier_003C_003EAccessor : IMemberAccessor<MyAtmosphereSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereSettings owner, in float value)
			{
				owner.AtmosphereTopModifier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereSettings owner, out float value)
			{
				value = owner.AtmosphereTopModifier;
			}
		}

		protected class VRageRender_Messages_MyAtmosphereSettings_003C_003EScale_003C_003EAccessor : IMemberAccessor<MyAtmosphereSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereSettings owner, in float value)
			{
				owner.Scale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereSettings owner, out float value)
			{
				value = owner.Scale;
			}
		}

		protected class VRageRender_Messages_MyAtmosphereSettings_003C_003ESunColorLinear_003C_003EAccessor : IMemberAccessor<MyAtmosphereSettings, Vector3?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereSettings owner, in Vector3? value)
			{
				owner.SunColorLinear = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereSettings owner, out Vector3? value)
			{
				value = owner.SunColorLinear;
			}
		}

		protected class VRageRender_Messages_MyAtmosphereSettings_003C_003ESunSpecularColorLinear_003C_003EAccessor : IMemberAccessor<MyAtmosphereSettings, Vector3?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereSettings owner, in Vector3? value)
			{
				owner.SunSpecularColorLinear = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereSettings owner, out Vector3? value)
			{
				value = owner.SunSpecularColorLinear;
			}
		}

		protected class VRageRender_Messages_MyAtmosphereSettings_003C_003ESunColor_003C_003EAccessor : IMemberAccessor<MyAtmosphereSettings, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereSettings owner, in Vector3 value)
			{
				owner.SunColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereSettings owner, out Vector3 value)
			{
				value = owner.SunColor;
			}
		}

		protected class VRageRender_Messages_MyAtmosphereSettings_003C_003ESunSpecularColor_003C_003EAccessor : IMemberAccessor<MyAtmosphereSettings, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAtmosphereSettings owner, in Vector3 value)
			{
				owner.SunSpecularColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAtmosphereSettings owner, out Vector3 value)
			{
				value = owner.SunSpecularColor;
			}
		}

		[ProtoMember(1)]
		public Vector3 RayleighScattering;

		[ProtoMember(4)]
		public float MieScattering;

		[ProtoMember(7)]
		public Vector3 MieColorScattering;

		[ProtoMember(10)]
		public float RayleighHeight;

		[ProtoMember(13)]
		public float RayleighHeightSpace;

		[ProtoMember(16)]
		public float RayleighTransitionModifier;

		[ProtoMember(19)]
		public float MieHeight;

		[ProtoMember(22)]
		public float MieG;

		[ProtoMember(25)]
		public float Intensity;

		[ProtoMember(28)]
		public float FogIntensity;

		[ProtoMember(31)]
		public float SeaLevelModifier;

		[ProtoMember(34)]
		public float AtmosphereTopModifier;

		[ProtoMember(37)]
		public float Scale;

		[XmlIgnore]
		public Vector3? SunColorLinear;

		[XmlIgnore]
		public Vector3? SunSpecularColorLinear;

		public Vector3 SunColor
		{
			get
			{
				if (!SunColorLinear.HasValue)
				{
					return Vector3.One;
				}
				return SunColorLinear.Value.ToSRGB();
			}
			set
			{
				SunColorLinear = value.ToLinearRGB();
			}
		}

		public Vector3 SunSpecularColor
		{
			get
			{
				if (!SunSpecularColorLinear.HasValue)
				{
					return Vector3.One;
				}
				return SunSpecularColorLinear.Value.ToSRGB();
			}
			set
			{
				SunSpecularColorLinear = value.ToLinearRGB();
			}
		}

		public static MyAtmosphereSettings Defaults()
		{
			MyAtmosphereSettings result = default(MyAtmosphereSettings);
			result.RayleighScattering = new Vector3(20f, 7.5f, 10f);
			result.MieScattering = 50f;
			result.MieColorScattering = new Vector3(50f, 50f, 50f);
			result.RayleighHeight = 10f;
			result.RayleighHeightSpace = 10f;
			result.RayleighTransitionModifier = 1f;
			result.MieHeight = 50f;
			result.MieG = 0.9998f;
			result.Intensity = 1f;
			result.FogIntensity = 0f;
			result.SeaLevelModifier = 1f;
			result.AtmosphereTopModifier = 1f;
			result.Scale = 0.5f;
			result.SunColorLinear = null;
			result.SunSpecularColorLinear = null;
			return result;
		}
	}
}
