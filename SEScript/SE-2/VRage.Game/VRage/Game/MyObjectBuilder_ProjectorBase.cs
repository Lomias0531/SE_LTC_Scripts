using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public abstract class MyObjectBuilder_ProjectorBase : MyObjectBuilder_FunctionalBlock
	{
		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EProjectedGrid_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, MyObjectBuilder_CubeGrid>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in MyObjectBuilder_CubeGrid value)
			{
				owner.ProjectedGrid = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out MyObjectBuilder_CubeGrid value)
			{
				value = owner.ProjectedGrid;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EProjectedGrids_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, List<MyObjectBuilder_CubeGrid>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in List<MyObjectBuilder_CubeGrid> value)
			{
				owner.ProjectedGrids = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out List<MyObjectBuilder_CubeGrid> value)
			{
				value = owner.ProjectedGrids;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EProjectionOffset_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, Vector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in Vector3I value)
			{
				owner.ProjectionOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out Vector3I value)
			{
				value = owner.ProjectionOffset;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EProjectionRotation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, Vector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in Vector3I value)
			{
				owner.ProjectionRotation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out Vector3I value)
			{
				value = owner.ProjectionRotation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EKeepProjection_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in bool value)
			{
				owner.KeepProjection = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out bool value)
			{
				value = owner.KeepProjection;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EShowOnlyBuildable_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in bool value)
			{
				owner.ShowOnlyBuildable = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out bool value)
			{
				value = owner.ShowOnlyBuildable;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EInstantBuildingEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in bool value)
			{
				owner.InstantBuildingEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out bool value)
			{
				value = owner.InstantBuildingEnabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EMaxNumberOfProjections_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in int value)
			{
				owner.MaxNumberOfProjections = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out int value)
			{
				value = owner.MaxNumberOfProjections;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EMaxNumberOfBlocks_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in int value)
			{
				owner.MaxNumberOfBlocks = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out int value)
			{
				value = owner.MaxNumberOfBlocks;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EProjectionsRemaining_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in int value)
			{
				owner.ProjectionsRemaining = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out int value)
			{
				value = owner.ProjectionsRemaining;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EGetOwnershipFromProjector_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in bool value)
			{
				owner.GetOwnershipFromProjector = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out bool value)
			{
				value = owner.GetOwnershipFromProjector;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in float value)
			{
				owner.Scale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out float value)
			{
				value = owner.Scale;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003ETextPanels_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectorBase, List<MySerializedTextPanelData>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in List<MySerializedTextPanelData> value)
			{
				owner.TextPanels = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out List<MySerializedTextPanelData> value)
			{
				value = owner.TextPanels;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_FunctionalBlock_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_FunctionalBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_FunctionalBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003ECustomName_003C_003EAccessor : VRage_Game_MyObjectBuilder_TerminalBlock_003C_003ECustomName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_TerminalBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_TerminalBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EShowOnHUD_003C_003EAccessor : VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EShowOnHUD_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_TerminalBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_TerminalBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EShowInTerminal_003C_003EAccessor : VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EShowInTerminal_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_TerminalBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_TerminalBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EShowInToolbarConfig_003C_003EAccessor : VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EShowInToolbarConfig_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_TerminalBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_TerminalBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EShowInInventory_003C_003EAccessor : VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EShowInInventory_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_TerminalBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_TerminalBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EEntityId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EEntityId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in long value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out long value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EName_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EMin_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMin_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, SerializableVector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in SerializableVector3I value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out SerializableVector3I value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003Em_orientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003Em_orientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, SerializableQuaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in SerializableQuaternion value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out SerializableQuaternion value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EIntegrityPercent_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EIntegrityPercent_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EBuildPercent_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBuildPercent_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EBlockOrientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBlockOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, SerializableBlockOrientation>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in SerializableBlockOrientation value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out SerializableBlockOrientation value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EConstructionInventory_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EConstructionInventory_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, MyObjectBuilder_Inventory>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in MyObjectBuilder_Inventory value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out MyObjectBuilder_Inventory value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EColorMaskHSV_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EColorMaskHSV_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in SerializableVector3 value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out SerializableVector3 value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003ESkinSubtypeId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003ESkinSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EConstructionStockpile_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EConstructionStockpile_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, MyObjectBuilder_ConstructionStockpile>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in MyObjectBuilder_ConstructionStockpile value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out MyObjectBuilder_ConstructionStockpile value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EOwner_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EOwner_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in long value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out long value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EBuiltBy_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBuiltBy_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in long value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out long value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EShareMode_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EShareMode_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, MyOwnershipShareModeEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in MyOwnershipShareModeEnum value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out MyOwnershipShareModeEnum value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EDeformationRatio_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EDeformationRatio_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003ESubBlocks_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003ESubBlocks_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, MySubBlockId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in MySubBlockId[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out MySubBlockId[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EMultiBlockId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EMultiBlockDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EMultiBlockIndex_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockIndex_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EBlockGeneralDamageModifier_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBlockGeneralDamageModifier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EComponentContainer_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EComponentContainer_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, MyObjectBuilder_ComponentContainer>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in MyObjectBuilder_ComponentContainer value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out MyObjectBuilder_ComponentContainer value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003EOrientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, SerializableQuaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in SerializableQuaternion value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out SerializableQuaternion value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectorBase_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectorBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectorBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectorBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		/// <summary>
		/// This property is obsolete. Do not use it any more. Use Projected grids. In case of single projected grid, select index 0.
		/// </summary>
		[ProtoMember(1)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public MyObjectBuilder_CubeGrid ProjectedGrid;

		[ProtoMember(2)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public List<MyObjectBuilder_CubeGrid> ProjectedGrids;

		[ProtoMember(4)]
		public Vector3I ProjectionOffset;

		[ProtoMember(7)]
		public Vector3I ProjectionRotation;

		[ProtoMember(10)]
		public bool KeepProjection;

		[ProtoMember(13)]
		public bool ShowOnlyBuildable;

		[ProtoMember(16)]
		public bool InstantBuildingEnabled;

		[ProtoMember(19)]
		public int MaxNumberOfProjections = 5;

		[ProtoMember(22)]
		public int MaxNumberOfBlocks = 200;

		[ProtoMember(25)]
		public int ProjectionsRemaining;

		[ProtoMember(28)]
		public bool GetOwnershipFromProjector;

		[ProtoMember(31)]
		public float Scale = 1f;

		[ProtoMember(34)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public List<MySerializedTextPanelData> TextPanels;

		public override void Remap(IMyRemapHelper remapHelper)
		{
			base.Remap(remapHelper);
			if (ProjectedGrids != null && ProjectedGrids.Count > 0)
			{
				foreach (MyObjectBuilder_CubeGrid projectedGrid in ProjectedGrids)
				{
					projectedGrid.Remap(remapHelper);
				}
			}
		}
	}
}
