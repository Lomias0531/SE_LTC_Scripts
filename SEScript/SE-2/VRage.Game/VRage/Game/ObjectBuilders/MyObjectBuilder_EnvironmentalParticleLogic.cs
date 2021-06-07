using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_EnvironmentalParticleLogic : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003EMaterial_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogic owner, in string value)
			{
				owner.Material = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogic owner, out string value)
			{
				value = owner.Material;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003EParticleColor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogic owner, in Vector4 value)
			{
				owner.ParticleColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogic owner, out Vector4 value)
			{
				value = owner.ParticleColor;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003EMaxSpawnDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogic owner, in float value)
			{
				owner.MaxSpawnDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogic owner, out float value)
			{
				value = owner.MaxSpawnDistance;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003EDespawnDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogic owner, in float value)
			{
				owner.DespawnDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogic owner, out float value)
			{
				value = owner.DespawnDistance;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003EDensity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogic owner, in float value)
			{
				owner.Density = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogic owner, out float value)
			{
				value = owner.Density;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003EMaxLifeTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogic owner, in int value)
			{
				owner.MaxLifeTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogic owner, out int value)
			{
				value = owner.MaxLifeTime;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003EMaxParticles_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogic owner, in int value)
			{
				owner.MaxParticles = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogic owner, out int value)
			{
				value = owner.MaxParticles;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003EMaterialPlanet_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogic owner, in string value)
			{
				owner.MaterialPlanet = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogic owner, out string value)
			{
				value = owner.MaterialPlanet;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003EParticleColorPlanet_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogic owner, in Vector4 value)
			{
				owner.ParticleColorPlanet = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogic owner, out Vector4 value)
			{
				value = owner.ParticleColorPlanet;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogic owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogic owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogic owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogic owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogic owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogic owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogic owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogic owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003EActor : IActivator, IActivator<MyObjectBuilder_EnvironmentalParticleLogic>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_EnvironmentalParticleLogic();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_EnvironmentalParticleLogic CreateInstance()
			{
				return new MyObjectBuilder_EnvironmentalParticleLogic();
			}

			MyObjectBuilder_EnvironmentalParticleLogic IActivator<MyObjectBuilder_EnvironmentalParticleLogic>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string Material;

		[ProtoMember(4)]
		public Vector4 ParticleColor;

		[ProtoMember(7)]
		public float MaxSpawnDistance;

		[ProtoMember(10)]
		public float DespawnDistance;

		[ProtoMember(13)]
		public float Density;

		[ProtoMember(16)]
		public int MaxLifeTime;

		[ProtoMember(19)]
		public int MaxParticles;

		[ProtoMember(22)]
		public string MaterialPlanet;

		[ProtoMember(25)]
		public Vector4 ParticleColorPlanet;
	}
}
