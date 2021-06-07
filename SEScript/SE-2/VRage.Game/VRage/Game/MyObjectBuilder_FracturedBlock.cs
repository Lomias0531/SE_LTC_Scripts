using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
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
	public class MyObjectBuilder_FracturedBlock : MyObjectBuilder_CubeBlock
	{
		[ProtoContract]
		public struct ShapeB
		{
			protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EShapeB_003C_003EName_003C_003EAccessor : IMemberAccessor<ShapeB, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ShapeB owner, in string value)
				{
					owner.Name = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ShapeB owner, out string value)
				{
					value = owner.Name;
				}
			}

			protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EShapeB_003C_003EOrientation_003C_003EAccessor : IMemberAccessor<ShapeB, SerializableQuaternion>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ShapeB owner, in SerializableQuaternion value)
				{
					owner.Orientation = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ShapeB owner, out SerializableQuaternion value)
				{
					value = owner.Orientation;
				}
			}

			protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EShapeB_003C_003EFixed_003C_003EAccessor : IMemberAccessor<ShapeB, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ShapeB owner, in bool value)
				{
					owner.Fixed = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ShapeB owner, out bool value)
				{
					value = owner.Fixed;
				}
			}

			private class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EShapeB_003C_003EActor : IActivator, IActivator<ShapeB>
			{
				private sealed override object CreateInstance()
				{
					return default(ShapeB);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override ShapeB CreateInstance()
				{
					return (ShapeB)(object)default(ShapeB);
				}

				ShapeB IActivator<ShapeB>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public string Name;

			[ProtoMember(4)]
			public SerializableQuaternion Orientation;

			[ProtoMember(7)]
			[DefaultValue(false)]
			public bool Fixed;
		}

		[ProtoContract]
		public class MyMultiBlockPart
		{
			protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EMyMultiBlockPart_003C_003EMultiBlockDefinition_003C_003EAccessor : IMemberAccessor<MyMultiBlockPart, SerializableDefinitionId>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyMultiBlockPart owner, in SerializableDefinitionId value)
				{
					owner.MultiBlockDefinition = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyMultiBlockPart owner, out SerializableDefinitionId value)
				{
					value = owner.MultiBlockDefinition;
				}
			}

			protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EMyMultiBlockPart_003C_003EMultiBlockId_003C_003EAccessor : IMemberAccessor<MyMultiBlockPart, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyMultiBlockPart owner, in int value)
				{
					owner.MultiBlockId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyMultiBlockPart owner, out int value)
				{
					value = owner.MultiBlockId;
				}
			}

			private class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EMyMultiBlockPart_003C_003EActor : IActivator, IActivator<MyMultiBlockPart>
			{
				private sealed override object CreateInstance()
				{
					return new MyMultiBlockPart();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyMultiBlockPart CreateInstance()
				{
					return new MyMultiBlockPart();
				}

				MyMultiBlockPart IActivator<MyMultiBlockPart>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(10)]
			public SerializableDefinitionId MultiBlockDefinition;

			[ProtoMember(13)]
			public int MultiBlockId;
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EBlockDefinitions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FracturedBlock, List<SerializableDefinitionId>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in List<SerializableDefinitionId> value)
			{
				owner.BlockDefinitions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out List<SerializableDefinitionId> value)
			{
				value = owner.BlockDefinitions;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EShapes_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FracturedBlock, List<ShapeB>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in List<ShapeB> value)
			{
				owner.Shapes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out List<ShapeB> value)
			{
				value = owner.Shapes;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EBlockOrientations_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FracturedBlock, List<SerializableBlockOrientation>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in List<SerializableBlockOrientation> value)
			{
				owner.BlockOrientations = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out List<SerializableBlockOrientation> value)
			{
				value = owner.BlockOrientations;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003ECreatingFracturedBlock_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FracturedBlock, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in bool value)
			{
				owner.CreatingFracturedBlock = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out bool value)
			{
				value = owner.CreatingFracturedBlock;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EMultiBlocks_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FracturedBlock, List<MyMultiBlockPart>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in List<MyMultiBlockPart> value)
			{
				owner.MultiBlocks = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out List<MyMultiBlockPart> value)
			{
				value = owner.MultiBlocks;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EEntityId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EEntityId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in long value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out long value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EName_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EMin_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMin_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, SerializableVector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in SerializableVector3I value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out SerializableVector3I value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003Em_orientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003Em_orientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, SerializableQuaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in SerializableQuaternion value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out SerializableQuaternion value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EIntegrityPercent_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EIntegrityPercent_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EBuildPercent_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBuildPercent_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EBlockOrientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBlockOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, SerializableBlockOrientation>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in SerializableBlockOrientation value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out SerializableBlockOrientation value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EConstructionInventory_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EConstructionInventory_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, MyObjectBuilder_Inventory>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in MyObjectBuilder_Inventory value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out MyObjectBuilder_Inventory value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EColorMaskHSV_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EColorMaskHSV_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in SerializableVector3 value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out SerializableVector3 value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003ESkinSubtypeId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003ESkinSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in string value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out string value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EConstructionStockpile_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EConstructionStockpile_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, MyObjectBuilder_ConstructionStockpile>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in MyObjectBuilder_ConstructionStockpile value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out MyObjectBuilder_ConstructionStockpile value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EOwner_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EOwner_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in long value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out long value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EBuiltBy_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBuiltBy_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in long value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out long value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EShareMode_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EShareMode_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, MyOwnershipShareModeEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in MyOwnershipShareModeEnum value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out MyOwnershipShareModeEnum value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EDeformationRatio_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EDeformationRatio_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003ESubBlocks_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003ESubBlocks_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, MySubBlockId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in MySubBlockId[] value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out MySubBlockId[] value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EMultiBlockId_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EMultiBlockDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EMultiBlockIndex_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockIndex_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in int value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out int value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EBlockGeneralDamageModifier_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBlockGeneralDamageModifier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in float value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out float value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EComponentContainer_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EComponentContainer_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, MyObjectBuilder_ComponentContainer>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in MyObjectBuilder_ComponentContainer value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out MyObjectBuilder_ComponentContainer value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EOrientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_CubeBlock_003C_003EOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, SerializableQuaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in SerializableQuaternion value)
			{
				Set(ref *(MyObjectBuilder_CubeBlock*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out SerializableQuaternion value)
			{
				Get(ref *(MyObjectBuilder_CubeBlock*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FracturedBlock, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FracturedBlock owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FracturedBlock owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_FracturedBlock_003C_003EActor : IActivator, IActivator<MyObjectBuilder_FracturedBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_FracturedBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_FracturedBlock CreateInstance()
			{
				return new MyObjectBuilder_FracturedBlock();
			}

			MyObjectBuilder_FracturedBlock IActivator<MyObjectBuilder_FracturedBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(16)]
		public List<SerializableDefinitionId> BlockDefinitions = new List<SerializableDefinitionId>();

		[ProtoMember(19)]
		public List<ShapeB> Shapes = new List<ShapeB>();

		[ProtoMember(22)]
		public List<SerializableBlockOrientation> BlockOrientations = new List<SerializableBlockOrientation>();

		public bool CreatingFracturedBlock;

		[ProtoMember(25)]
		public List<MyMultiBlockPart> MultiBlocks = new List<MyMultiBlockPart>();
	}
}
