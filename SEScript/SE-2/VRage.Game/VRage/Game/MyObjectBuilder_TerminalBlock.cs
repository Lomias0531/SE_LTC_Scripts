using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_TerminalBlock : MyObjectBuilder_CubeBlock
	{
		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003ECustomName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TerminalBlock, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in string value)
			{
				owner.CustomName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out string value)
			{
				value = owner.CustomName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EShowOnHUD_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TerminalBlock, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in bool value)
			{
				owner.ShowOnHUD = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out bool value)
			{
				value = owner.ShowOnHUD;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EShowInTerminal_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TerminalBlock, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in bool value)
			{
				owner.ShowInTerminal = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out bool value)
			{
				value = owner.ShowInTerminal;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EShowInToolbarConfig_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TerminalBlock, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in bool value)
			{
				owner.ShowInToolbarConfig = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out bool value)
			{
				value = owner.ShowInToolbarConfig;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EShowInInventory_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TerminalBlock, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in bool value)
			{
				owner.ShowInInventory = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out bool value)
			{
				value = owner.ShowInInventory;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EEntityId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EEntityId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in long value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out long value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EName_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EMin_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMin_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, SerializableVector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in SerializableVector3I value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out SerializableVector3I value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003Em_orientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003Em_orientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, SerializableQuaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in SerializableQuaternion value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out SerializableQuaternion value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EIntegrityPercent_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EIntegrityPercent_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EBuildPercent_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBuildPercent_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EBlockOrientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBlockOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, SerializableBlockOrientation>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in SerializableBlockOrientation value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out SerializableBlockOrientation value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EConstructionInventory_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EConstructionInventory_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, MyObjectBuilder_Inventory>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in MyObjectBuilder_Inventory value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out MyObjectBuilder_Inventory value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EColorMaskHSV_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EColorMaskHSV_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in SerializableVector3 value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out SerializableVector3 value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003ESkinSubtypeId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003ESkinSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EConstructionStockpile_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EConstructionStockpile_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, MyObjectBuilder_ConstructionStockpile>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in MyObjectBuilder_ConstructionStockpile value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out MyObjectBuilder_ConstructionStockpile value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EOwner_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EOwner_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in long value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out long value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EBuiltBy_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBuiltBy_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in long value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out long value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EShareMode_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EShareMode_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, MyOwnershipShareModeEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in MyOwnershipShareModeEnum value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out MyOwnershipShareModeEnum value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EDeformationRatio_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EDeformationRatio_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003ESubBlocks_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003ESubBlocks_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, MySubBlockId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in MySubBlockId[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out MySubBlockId[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EMultiBlockId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EMultiBlockDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EMultiBlockIndex_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockIndex_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EBlockGeneralDamageModifier_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBlockGeneralDamageModifier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EComponentContainer_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EComponentContainer_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, MyObjectBuilder_ComponentContainer>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in MyObjectBuilder_ComponentContainer value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out MyObjectBuilder_ComponentContainer value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EOrientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, SerializableQuaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in SerializableQuaternion value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out SerializableQuaternion value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TerminalBlock owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TerminalBlock owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_TerminalBlock_003C_003EActor : IActivator, IActivator<MyObjectBuilder_TerminalBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_TerminalBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_TerminalBlock CreateInstance()
			{
				return new MyObjectBuilder_TerminalBlock();
			}

			MyObjectBuilder_TerminalBlock IActivator<MyObjectBuilder_TerminalBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public string CustomName;

		[ProtoMember(4)]
		public bool ShowOnHUD;

		[ProtoMember(7)]
		public bool ShowInTerminal = true;

		[ProtoMember(10)]
		public bool ShowInToolbarConfig = true;

		[ProtoMember(13)]
		public bool ShowInInventory = true;
	}
}
