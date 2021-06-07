using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_SchematicItemDefinition : MyObjectBuilder_UsableItemDefinition
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EResearch_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in SerializableDefinitionId? value)
			{
				owner.Research = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out SerializableDefinitionId? value)
			{
				value = owner.Research;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EUseSound_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_UsableItemDefinition_003C_003EUseSound_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_UsableItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_UsableItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003ESize_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ESize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in Vector3 value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out Vector3 value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EMass_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMass_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EModel_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModel_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EModels_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModels_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EIconSymbol_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EIconSymbol_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EVolume_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EVolume_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in float? value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out float? value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EModelVolume_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModelVolume_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in float? value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out float? value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EPhysicalMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EPhysicalMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EVoxelMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EVoxelMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003ECanSpawnFromScreen_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ECanSpawnFromScreen_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003ERotateOnSpawnX_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnX_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003ERotateOnSpawnY_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnY_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003ERotateOnSpawnZ_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnZ_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EHealth_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EHealth_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EDestroyedPieceId_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EDestroyedPieceId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EDestroyedPieces_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EDestroyedPieces_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EExtraInventoryTooltipLine_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EExtraInventoryTooltipLine_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EMaxStackAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaxStackAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, MyFixedPoint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in MyFixedPoint value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out MyFixedPoint value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EMinimalPricePerUnit_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimalPricePerUnit_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EMinimumOfferAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumOfferAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EMaximumOfferAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumOfferAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EMinimumOrderAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumOrderAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EMaximumOrderAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumOrderAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003ECanPlayerOrder_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ECanPlayerOrder_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EMinimumAcquisitionAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumAcquisitionAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EMaximumAcquisitionAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumAcquisitionAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_SchematicItemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_SchematicItemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_SchematicItemDefinition CreateInstance()
			{
				return new MyObjectBuilder_SchematicItemDefinition();
			}

			MyObjectBuilder_SchematicItemDefinition IActivator<MyObjectBuilder_SchematicItemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public SerializableDefinitionId? Research;
	}
}
