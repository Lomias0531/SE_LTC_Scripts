using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_CompoundCubeBlock : MyObjectBuilder_CubeBlock
	{
		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EBlocks_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, MyObjectBuilder_CubeBlock[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in MyObjectBuilder_CubeBlock[] value)
			{
				owner.Blocks = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out MyObjectBuilder_CubeBlock[] value)
			{
				value = owner.Blocks;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EBlockIds_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, ushort[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in ushort[] value)
			{
				owner.BlockIds = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out ushort[] value)
			{
				value = owner.BlockIds;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EEntityId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EEntityId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in long value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out long value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EName_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EMin_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMin_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, SerializableVector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in SerializableVector3I value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out SerializableVector3I value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003Em_orientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003Em_orientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, SerializableQuaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in SerializableQuaternion value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out SerializableQuaternion value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EIntegrityPercent_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EIntegrityPercent_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EBuildPercent_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBuildPercent_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EBlockOrientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBlockOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, SerializableBlockOrientation>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in SerializableBlockOrientation value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out SerializableBlockOrientation value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EConstructionInventory_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EConstructionInventory_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, MyObjectBuilder_Inventory>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in MyObjectBuilder_Inventory value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out MyObjectBuilder_Inventory value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EColorMaskHSV_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EColorMaskHSV_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in SerializableVector3 value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out SerializableVector3 value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003ESkinSubtypeId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003ESkinSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EConstructionStockpile_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EConstructionStockpile_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, MyObjectBuilder_ConstructionStockpile>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in MyObjectBuilder_ConstructionStockpile value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out MyObjectBuilder_ConstructionStockpile value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EOwner_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EOwner_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in long value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out long value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EBuiltBy_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBuiltBy_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in long value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out long value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EShareMode_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EShareMode_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, MyOwnershipShareModeEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in MyOwnershipShareModeEnum value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out MyOwnershipShareModeEnum value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EDeformationRatio_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EDeformationRatio_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003ESubBlocks_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003ESubBlocks_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, MySubBlockId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in MySubBlockId[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out MySubBlockId[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EMultiBlockId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EMultiBlockDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EMultiBlockIndex_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockIndex_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EBlockGeneralDamageModifier_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBlockGeneralDamageModifier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EComponentContainer_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EComponentContainer_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, MyObjectBuilder_ComponentContainer>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in MyObjectBuilder_ComponentContainer value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out MyObjectBuilder_ComponentContainer value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EOrientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, SerializableQuaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in SerializableQuaternion value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out SerializableQuaternion value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_CompoundCubeBlock_003C_003EActor : IActivator, IActivator<MyObjectBuilder_CompoundCubeBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_CompoundCubeBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_CompoundCubeBlock CreateInstance()
			{
				return new MyObjectBuilder_CompoundCubeBlock();
			}

			MyObjectBuilder_CompoundCubeBlock IActivator<MyObjectBuilder_CompoundCubeBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[DynamicItem(typeof(MyObjectBuilderDynamicSerializer), true)]
		[XmlArrayItem("MyObjectBuilder_CubeBlock", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_CubeBlock>))]
		public MyObjectBuilder_CubeBlock[] Blocks;

		[ProtoMember(4)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public ushort[] BlockIds;

		public override void Remap(IMyRemapHelper remapHelper)
		{
			base.Remap(remapHelper);
			MyObjectBuilder_CubeBlock[] blocks = Blocks;
			for (int i = 0; i < blocks.Length; i++)
			{
				blocks[i].Remap(remapHelper);
			}
		}
	}
}
