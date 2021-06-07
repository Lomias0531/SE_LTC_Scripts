using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_EnvironmentSettings : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_EnvironmentSettings_003C_003ESunAzimuth_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentSettings owner, in float value)
			{
				owner.SunAzimuth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentSettings owner, out float value)
			{
				value = owner.SunAzimuth;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentSettings_003C_003ESunElevation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentSettings owner, in float value)
			{
				owner.SunElevation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentSettings owner, out float value)
			{
				value = owner.SunElevation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentSettings_003C_003ESunIntensity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentSettings owner, in float value)
			{
				owner.SunIntensity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentSettings owner, out float value)
			{
				value = owner.SunIntensity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentSettings_003C_003EFogMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentSettings owner, in float value)
			{
				owner.FogMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentSettings owner, out float value)
			{
				value = owner.FogMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentSettings_003C_003EFogDensity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentSettings owner, in float value)
			{
				owner.FogDensity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentSettings owner, out float value)
			{
				value = owner.FogDensity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentSettings_003C_003EFogColor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentSettings, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentSettings owner, in SerializableVector3 value)
			{
				owner.FogColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentSettings owner, out SerializableVector3 value)
			{
				value = owner.FogColor;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentSettings_003C_003EEnvironmentDefinition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentSettings, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentSettings owner, in SerializableDefinitionId value)
			{
				owner.EnvironmentDefinition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentSettings owner, out SerializableDefinitionId value)
			{
				value = owner.EnvironmentDefinition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentSettings_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentSettings, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentSettings owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentSettings owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentSettings_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentSettings, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentSettings owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentSettings owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentSettings_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentSettings, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentSettings owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentSettings owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EnvironmentSettings_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentSettings, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentSettings owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentSettings owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_EnvironmentSettings_003C_003EActor : IActivator, IActivator<MyObjectBuilder_EnvironmentSettings>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_EnvironmentSettings();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_EnvironmentSettings CreateInstance()
			{
				return new MyObjectBuilder_EnvironmentSettings();
			}

			MyObjectBuilder_EnvironmentSettings IActivator<MyObjectBuilder_EnvironmentSettings>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public float SunAzimuth;

		[ProtoMember(4)]
		public float SunElevation;

		[ProtoMember(7)]
		public float SunIntensity;

		[ProtoMember(10)]
		public float FogMultiplier;

		[ProtoMember(13)]
		public float FogDensity;

		[ProtoMember(16)]
		public SerializableVector3 FogColor;

		[ProtoMember(19)]
		public SerializableDefinitionId EnvironmentDefinition = new SerializableDefinitionId(typeof(MyObjectBuilder_EnvironmentDefinition), "Default");
	}
}
