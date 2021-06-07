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
	public class MyObjectBuilder_EnvironmentalParticleLogicSpace : MyObjectBuilder_EnvironmentalParticleLogic
	{
		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace_003C_003EMaterial_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003EMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, in string value)
			{
				Set(ref *(MyObjectBuilder_EnvironmentalParticleLogic*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, out string value)
			{
				Get(ref *(MyObjectBuilder_EnvironmentalParticleLogic*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace_003C_003EParticleColor_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003EParticleColor_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, in Vector4 value)
			{
				Set(ref *(MyObjectBuilder_EnvironmentalParticleLogic*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, out Vector4 value)
			{
				Get(ref *(MyObjectBuilder_EnvironmentalParticleLogic*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace_003C_003EMaxSpawnDistance_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003EMaxSpawnDistance_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, in float value)
			{
				Set(ref *(MyObjectBuilder_EnvironmentalParticleLogic*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, out float value)
			{
				Get(ref *(MyObjectBuilder_EnvironmentalParticleLogic*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace_003C_003EDespawnDistance_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003EDespawnDistance_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, in float value)
			{
				Set(ref *(MyObjectBuilder_EnvironmentalParticleLogic*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, out float value)
			{
				Get(ref *(MyObjectBuilder_EnvironmentalParticleLogic*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace_003C_003EDensity_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003EDensity_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, in float value)
			{
				Set(ref *(MyObjectBuilder_EnvironmentalParticleLogic*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, out float value)
			{
				Get(ref *(MyObjectBuilder_EnvironmentalParticleLogic*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace_003C_003EMaxLifeTime_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003EMaxLifeTime_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, in int value)
			{
				Set(ref *(MyObjectBuilder_EnvironmentalParticleLogic*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, out int value)
			{
				Get(ref *(MyObjectBuilder_EnvironmentalParticleLogic*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace_003C_003EMaxParticles_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003EMaxParticles_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, in int value)
			{
				Set(ref *(MyObjectBuilder_EnvironmentalParticleLogic*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, out int value)
			{
				Get(ref *(MyObjectBuilder_EnvironmentalParticleLogic*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace_003C_003EMaterialPlanet_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003EMaterialPlanet_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, in string value)
			{
				Set(ref *(MyObjectBuilder_EnvironmentalParticleLogic*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, out string value)
			{
				Get(ref *(MyObjectBuilder_EnvironmentalParticleLogic*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace_003C_003EParticleColorPlanet_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic_003C_003EParticleColorPlanet_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, in Vector4 value)
			{
				Set(ref *(MyObjectBuilder_EnvironmentalParticleLogic*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, out Vector4 value)
			{
				Get(ref *(MyObjectBuilder_EnvironmentalParticleLogic*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace_003C_003EActor : IActivator, IActivator<MyObjectBuilder_EnvironmentalParticleLogicSpace>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_EnvironmentalParticleLogicSpace();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_EnvironmentalParticleLogicSpace CreateInstance()
			{
				return new MyObjectBuilder_EnvironmentalParticleLogicSpace();
			}

			MyObjectBuilder_EnvironmentalParticleLogicSpace IActivator<MyObjectBuilder_EnvironmentalParticleLogicSpace>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
