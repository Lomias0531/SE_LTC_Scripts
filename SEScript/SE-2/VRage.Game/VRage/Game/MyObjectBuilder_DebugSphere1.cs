using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_DebugSphere1 : MyObjectBuilder_FunctionalBlock
	{
		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_FunctionalBlock_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_FunctionalBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_FunctionalBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003ECustomName_003C_003EAccessor : VRage_Game_MyObjectBuilder_TerminalBlock_003C_003ECustomName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in string value)
			{
				Set(ref *(MyObjectBuilder_TerminalBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out string value)
			{
				Get(ref *(MyObjectBuilder_TerminalBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EShowOnHUD_003C_003EAccessor : VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EShowOnHUD_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_TerminalBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_TerminalBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EShowInTerminal_003C_003EAccessor : VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EShowInTerminal_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_TerminalBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_TerminalBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EShowInToolbarConfig_003C_003EAccessor : VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EShowInToolbarConfig_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_TerminalBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_TerminalBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EShowInInventory_003C_003EAccessor : VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EShowInInventory_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_TerminalBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_TerminalBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EEntityId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EEntityId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in long value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out long value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EName_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EMin_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMin_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, SerializableVector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in SerializableVector3I value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out SerializableVector3I value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003Em_orientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003Em_orientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, SerializableQuaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in SerializableQuaternion value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out SerializableQuaternion value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EIntegrityPercent_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EIntegrityPercent_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EBuildPercent_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBuildPercent_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EBlockOrientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBlockOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, SerializableBlockOrientation>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in SerializableBlockOrientation value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out SerializableBlockOrientation value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EConstructionInventory_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EConstructionInventory_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, MyObjectBuilder_Inventory>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in MyObjectBuilder_Inventory value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out MyObjectBuilder_Inventory value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EColorMaskHSV_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EColorMaskHSV_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in SerializableVector3 value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out SerializableVector3 value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003ESkinSubtypeId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003ESkinSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EConstructionStockpile_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EConstructionStockpile_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, MyObjectBuilder_ConstructionStockpile>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in MyObjectBuilder_ConstructionStockpile value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out MyObjectBuilder_ConstructionStockpile value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EOwner_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EOwner_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in long value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out long value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EBuiltBy_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBuiltBy_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in long value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out long value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EShareMode_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EShareMode_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, MyOwnershipShareModeEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in MyOwnershipShareModeEnum value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out MyOwnershipShareModeEnum value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EDeformationRatio_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EDeformationRatio_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003ESubBlocks_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003ESubBlocks_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, MySubBlockId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in MySubBlockId[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out MySubBlockId[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EMultiBlockId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EMultiBlockDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EMultiBlockIndex_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockIndex_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EBlockGeneralDamageModifier_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBlockGeneralDamageModifier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EComponentContainer_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EComponentContainer_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, MyObjectBuilder_ComponentContainer>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in MyObjectBuilder_ComponentContainer value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out MyObjectBuilder_ComponentContainer value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EOrientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, SerializableQuaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in SerializableQuaternion value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out SerializableQuaternion value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere1, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DebugSphere1 owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DebugSphere1 owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_DebugSphere1_003C_003EActor : IActivator, IActivator<MyObjectBuilder_DebugSphere1>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_DebugSphere1();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_DebugSphere1 CreateInstance()
			{
				return new MyObjectBuilder_DebugSphere1();
			}

			MyObjectBuilder_DebugSphere1 IActivator<MyObjectBuilder_DebugSphere1>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
