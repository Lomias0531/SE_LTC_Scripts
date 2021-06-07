using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_RespawnShipDefinition : MyObjectBuilder_DefinitionBase
	{
		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EPrefab_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string value)
			{
				owner.Prefab = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string value)
			{
				value = owner.Prefab;
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003ECooldownSeconds_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in int value)
			{
				owner.CooldownSeconds = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out int value)
			{
				value = owner.CooldownSeconds;
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003ESpawnWithDefaultItems_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in bool value)
			{
				owner.SpawnWithDefaultItems = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out bool value)
			{
				value = owner.SpawnWithDefaultItems;
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EUseForSpace_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in bool value)
			{
				owner.UseForSpace = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out bool value)
			{
				value = owner.UseForSpace;
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EMinimalAirDensity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in float value)
			{
				owner.MinimalAirDensity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out float value)
			{
				value = owner.MinimalAirDensity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EUseForPlanetsWithAtmosphere_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in bool value)
			{
				owner.UseForPlanetsWithAtmosphere = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out bool value)
			{
				value = owner.UseForPlanetsWithAtmosphere;
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EUseForPlanetsWithoutAtmosphere_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in bool value)
			{
				owner.UseForPlanetsWithoutAtmosphere = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out bool value)
			{
				value = owner.UseForPlanetsWithoutAtmosphere;
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EPlanetDeployAltitude_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in float? value)
			{
				owner.PlanetDeployAltitude = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out float? value)
			{
				value = owner.PlanetDeployAltitude;
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EInitialLinearVelocity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in SerializableVector3 value)
			{
				owner.InitialLinearVelocity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out SerializableVector3 value)
			{
				value = owner.InitialLinearVelocity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EInitialAngularVelocity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in SerializableVector3 value)
			{
				owner.InitialAngularVelocity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out SerializableVector3 value)
			{
				value = owner.InitialAngularVelocity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EHelpTextLocalizationId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string value)
			{
				owner.HelpTextLocalizationId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string value)
			{
				value = owner.HelpTextLocalizationId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003ESpawnNearProceduralAsteroids_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in bool value)
			{
				owner.SpawnNearProceduralAsteroids = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out bool value)
			{
				value = owner.SpawnNearProceduralAsteroids;
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EPlanetTypes_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string[] value)
			{
				owner.PlanetTypes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string[] value)
			{
				value = owner.PlanetTypes;
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003ESpawnPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, SerializableVector3D?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in SerializableVector3D? value)
			{
				owner.SpawnPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out SerializableVector3D? value)
			{
				value = owner.SpawnPosition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003ESpawnPositionDispersionMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in float value)
			{
				owner.SpawnPositionDispersionMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out float value)
			{
				value = owner.SpawnPositionDispersionMin;
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003ESpawnPositionDispersionMax_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in float value)
			{
				owner.SpawnPositionDispersionMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out float value)
			{
				value = owner.SpawnPositionDispersionMax;
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_RespawnShipDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_RespawnShipDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_RespawnShipDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_RespawnShipDefinition CreateInstance()
			{
				return new MyObjectBuilder_RespawnShipDefinition();
			}

			MyObjectBuilder_RespawnShipDefinition IActivator<MyObjectBuilder_RespawnShipDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string Prefab;

		[ProtoMember(4)]
		public int CooldownSeconds;

		[ProtoMember(7, IsRequired = false)]
		public bool SpawnWithDefaultItems = true;

		[ProtoMember(10, IsRequired = false)]
		public bool UseForSpace;

		[ProtoMember(13, IsRequired = false)]
		public float MinimalAirDensity = 0.7f;

		[ProtoMember(16, IsRequired = false)]
		public bool UseForPlanetsWithAtmosphere;

		[ProtoMember(19, IsRequired = false)]
		public bool UseForPlanetsWithoutAtmosphere;

		[ProtoMember(22, IsRequired = false)]
		public float? PlanetDeployAltitude = 500f;

		[ProtoMember(25, IsRequired = false)]
		public SerializableVector3 InitialLinearVelocity = Vector3.Zero;

		[ProtoMember(28, IsRequired = false)]
		public SerializableVector3 InitialAngularVelocity = Vector3.Zero;

		[ProtoMember(31)]
		public string HelpTextLocalizationId;

		[ProtoMember(33)]
		public bool SpawnNearProceduralAsteroids = true;

		[ProtoMember(35)]
		[XmlArrayItem("PlanetType")]
		public string[] PlanetTypes;

		[ProtoMember(37)]
		public SerializableVector3D? SpawnPosition;

		[ProtoMember(39)]
		public float SpawnPositionDispersionMin;

		[ProtoMember(41)]
		public float SpawnPositionDispersionMax = 10000f;
	}
}
