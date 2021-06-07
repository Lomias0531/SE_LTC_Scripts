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
	public class MyObjectBuilder_ShipBlueprintDefinition : MyObjectBuilder_PrefabDefinition
	{
		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003EWorkshopId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in ulong value)
			{
				owner.WorkshopId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out ulong value)
			{
				value = owner.WorkshopId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003EOwnerSteamId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in ulong value)
			{
				owner.OwnerSteamId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out ulong value)
			{
				value = owner.OwnerSteamId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003EPoints_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in ulong value)
			{
				owner.Points = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out ulong value)
			{
				value = owner.Points;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003ERespawnShip_003C_003EAccessor : VRage_Game_MyObjectBuilder_PrefabDefinition_003C_003ERespawnShip_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PrefabDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PrefabDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003ECubeGrid_003C_003EAccessor : VRage_Game_MyObjectBuilder_PrefabDefinition_003C_003ECubeGrid_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, MyObjectBuilder_CubeGrid>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in MyObjectBuilder_CubeGrid value)
			{
				Set(ref *(MyObjectBuilder_PrefabDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out MyObjectBuilder_CubeGrid value)
			{
				Get(ref *(MyObjectBuilder_PrefabDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003ECubeGrids_003C_003EAccessor : VRage_Game_MyObjectBuilder_PrefabDefinition_003C_003ECubeGrids_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, MyObjectBuilder_CubeGrid[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in MyObjectBuilder_CubeGrid[] value)
			{
				Set(ref *(MyObjectBuilder_PrefabDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out MyObjectBuilder_CubeGrid[] value)
			{
				Get(ref *(MyObjectBuilder_PrefabDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003EPrefabPath_003C_003EAccessor : VRage_Game_MyObjectBuilder_PrefabDefinition_003C_003EPrefabPath_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PrefabDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PrefabDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003EEnvironmentType_003C_003EAccessor : VRage_Game_MyObjectBuilder_PrefabDefinition_003C_003EEnvironmentType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, MyEnvironmentTypes>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in MyEnvironmentTypes value)
			{
				Set(ref *(MyObjectBuilder_PrefabDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out MyEnvironmentTypes value)
			{
				Get(ref *(MyObjectBuilder_PrefabDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003ETooltipImage_003C_003EAccessor : VRage_Game_MyObjectBuilder_PrefabDefinition_003C_003ETooltipImage_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PrefabDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PrefabDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ShipBlueprintDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ShipBlueprintDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ShipBlueprintDefinition CreateInstance()
			{
				return new MyObjectBuilder_ShipBlueprintDefinition();
			}

			MyObjectBuilder_ShipBlueprintDefinition IActivator<MyObjectBuilder_ShipBlueprintDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public ulong WorkshopId;

		[ProtoMember(4)]
		public ulong OwnerSteamId;

		[ProtoMember(7)]
		public ulong Points;
	}
}
