using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
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
	public class MyObjectBuilder_CubeBlock : MyObjectBuilder_Base
	{
		[ProtoContract]
		public struct MySubBlockId
		{
			protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMySubBlockId_003C_003ESubGridId_003C_003EAccessor : IMemberAccessor<MySubBlockId, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MySubBlockId owner, in long value)
				{
					owner.SubGridId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MySubBlockId owner, out long value)
				{
					value = owner.SubGridId;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMySubBlockId_003C_003ESubGridName_003C_003EAccessor : IMemberAccessor<MySubBlockId, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MySubBlockId owner, in string value)
				{
					owner.SubGridName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MySubBlockId owner, out string value)
				{
					value = owner.SubGridName;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMySubBlockId_003C_003ESubBlockPosition_003C_003EAccessor : IMemberAccessor<MySubBlockId, SerializableVector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MySubBlockId owner, in SerializableVector3I value)
				{
					owner.SubBlockPosition = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MySubBlockId owner, out SerializableVector3I value)
				{
					value = owner.SubBlockPosition;
				}
			}

			private class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMySubBlockId_003C_003EActor : IActivator, IActivator<MySubBlockId>
			{
				private sealed override object CreateInstance()
				{
					return default(MySubBlockId);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MySubBlockId CreateInstance()
				{
					return (MySubBlockId)(object)default(MySubBlockId);
				}

				MySubBlockId IActivator<MySubBlockId>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(43)]
			public long SubGridId;

			[ProtoMember(46)]
			public string SubGridName;

			[ProtoMember(49)]
			public SerializableVector3I SubBlockPosition;
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in long value)
			{
				owner.EntityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out long value)
			{
				value = owner.EntityId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, SerializableVector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in SerializableVector3I value)
			{
				owner.Min = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out SerializableVector3I value)
			{
				value = owner.Min;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003Em_orientation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, SerializableQuaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in SerializableQuaternion value)
			{
				owner.m_orientation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out SerializableQuaternion value)
			{
				value = owner.m_orientation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EIntegrityPercent_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in float value)
			{
				owner.IntegrityPercent = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out float value)
			{
				value = owner.IntegrityPercent;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBuildPercent_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in float value)
			{
				owner.BuildPercent = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out float value)
			{
				value = owner.BuildPercent;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBlockOrientation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, SerializableBlockOrientation>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in SerializableBlockOrientation value)
			{
				owner.BlockOrientation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out SerializableBlockOrientation value)
			{
				value = owner.BlockOrientation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EConstructionInventory_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, MyObjectBuilder_Inventory>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in MyObjectBuilder_Inventory value)
			{
				owner.ConstructionInventory = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out MyObjectBuilder_Inventory value)
			{
				value = owner.ConstructionInventory;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EColorMaskHSV_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in SerializableVector3 value)
			{
				owner.ColorMaskHSV = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out SerializableVector3 value)
			{
				value = owner.ColorMaskHSV;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003ESkinSubtypeId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in string value)
			{
				owner.SkinSubtypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out string value)
			{
				value = owner.SkinSubtypeId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EConstructionStockpile_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, MyObjectBuilder_ConstructionStockpile>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in MyObjectBuilder_ConstructionStockpile value)
			{
				owner.ConstructionStockpile = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out MyObjectBuilder_ConstructionStockpile value)
			{
				value = owner.ConstructionStockpile;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EOwner_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in long value)
			{
				owner.Owner = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out long value)
			{
				value = owner.Owner;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBuiltBy_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in long value)
			{
				owner.BuiltBy = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out long value)
			{
				value = owner.BuiltBy;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EShareMode_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, MyOwnershipShareModeEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in MyOwnershipShareModeEnum value)
			{
				owner.ShareMode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out MyOwnershipShareModeEnum value)
			{
				value = owner.ShareMode;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EDeformationRatio_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in float value)
			{
				owner.DeformationRatio = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out float value)
			{
				value = owner.DeformationRatio;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003ESubBlocks_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, MySubBlockId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in MySubBlockId[] value)
			{
				owner.SubBlocks = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out MySubBlockId[] value)
			{
				value = owner.SubBlocks;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in int value)
			{
				owner.MultiBlockId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out int value)
			{
				value = owner.MultiBlockId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockDefinition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in SerializableDefinitionId? value)
			{
				owner.MultiBlockDefinition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out SerializableDefinitionId? value)
			{
				value = owner.MultiBlockDefinition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EMultiBlockIndex_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in int value)
			{
				owner.MultiBlockIndex = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out int value)
			{
				value = owner.MultiBlockIndex;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EBlockGeneralDamageModifier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in float value)
			{
				owner.BlockGeneralDamageModifier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out float value)
			{
				value = owner.BlockGeneralDamageModifier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EComponentContainer_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, MyObjectBuilder_ComponentContainer>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in MyObjectBuilder_ComponentContainer value)
			{
				owner.ComponentContainer = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out MyObjectBuilder_ComponentContainer value)
			{
				value = owner.ComponentContainer;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlock, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlock, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EOrientation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, SerializableQuaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in SerializableQuaternion value)
			{
				owner.Orientation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out SerializableQuaternion value)
			{
				value = owner.Orientation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlock, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlock_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlock, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlock owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlock owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_CubeBlock_003C_003EActor : IActivator, IActivator<MyObjectBuilder_CubeBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_CubeBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_CubeBlock CreateInstance()
			{
				return new MyObjectBuilder_CubeBlock();
			}

			MyObjectBuilder_CubeBlock IActivator<MyObjectBuilder_CubeBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[DefaultValue(0)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public long EntityId;

		[ProtoMember(4)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public string Name;

		[ProtoMember(7)]
		[Serialize(MyPrimitiveFlags.Variant, Kind = MySerializeKind.Item)]
		public SerializableVector3I Min = new SerializableVector3I(0, 0, 0);

		private SerializableQuaternion m_orientation;

		[ProtoMember(10)]
		[DefaultValue(1f)]
		[Serialize((MyPrimitiveFlags)66)]
		public float IntegrityPercent = 1f;

		[ProtoMember(13)]
		[DefaultValue(1f)]
		[Serialize((MyPrimitiveFlags)66)]
		public float BuildPercent = 1f;

		[ProtoMember(16)]
		public SerializableBlockOrientation BlockOrientation = SerializableBlockOrientation.Identity;

		[ProtoMember(19)]
		[DefaultValue(null)]
		[NoSerialize]
		public MyObjectBuilder_Inventory ConstructionInventory;

		[ProtoMember(22)]
		public SerializableVector3 ColorMaskHSV = new SerializableVector3(0f, -1f, 0f);

		[ProtoMember(25)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public string SkinSubtypeId;

		[ProtoMember(28)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public MyObjectBuilder_ConstructionStockpile ConstructionStockpile;

		[ProtoMember(31)]
		[DefaultValue(0)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public long Owner;

		[ProtoMember(34)]
		[DefaultValue(0)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public long BuiltBy;

		[ProtoMember(37)]
		[DefaultValue(MyOwnershipShareModeEnum.None)]
		public MyOwnershipShareModeEnum ShareMode;

		[ProtoMember(40)]
		[DefaultValue(0)]
		[NoSerialize]
		public float DeformationRatio;

		[XmlArrayItem("SubBlock")]
		[ProtoMember(52)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public MySubBlockId[] SubBlocks;

		[ProtoMember(55)]
		[DefaultValue(0)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public int MultiBlockId;

		[ProtoMember(58)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableDefinitionId? MultiBlockDefinition;

		[ProtoMember(61)]
		[DefaultValue(-1)]
		[Serialize]
		public int MultiBlockIndex = -1;

		[ProtoMember(64)]
		[DefaultValue(1f)]
		[Serialize]
		public float BlockGeneralDamageModifier = 1f;

		[ProtoMember(67)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public MyObjectBuilder_ComponentContainer ComponentContainer;

		[NoSerialize]
		public SerializableQuaternion Orientation
		{
			get
			{
				return m_orientation;
			}
			set
			{
				if (!MyUtils.IsZero(value))
				{
					m_orientation = value;
				}
				else
				{
					m_orientation = Quaternion.Identity;
				}
				BlockOrientation = new SerializableBlockOrientation(Base6Directions.GetForward(m_orientation), Base6Directions.GetUp(m_orientation));
			}
		}

		public bool ShouldSerializeEntityId()
		{
			return EntityId != 0;
		}

		public bool ShouldSerializeMin()
		{
			return Min != new SerializableVector3I(0, 0, 0);
		}

		public bool ShouldSerializeOrientation()
		{
			return false;
		}

		public bool ShouldSerializeBlockOrientation()
		{
			return BlockOrientation != SerializableBlockOrientation.Identity;
		}

		public bool ShouldSerializeConstructionInventory()
		{
			return false;
		}

		public bool ShouldSerializeColorMaskHSV()
		{
			return ColorMaskHSV != new SerializableVector3(0f, -1f, 0f);
		}

		public bool ShouldSerializeSkinSubtypeId()
		{
			return !string.IsNullOrEmpty(SkinSubtypeId);
		}

		public static MyObjectBuilder_CubeBlock Upgrade(MyObjectBuilder_CubeBlock cubeBlock, MyObjectBuilderType newType, string newSubType)
		{
			MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = MyObjectBuilderSerializer.CreateNewObject(newType, newSubType) as MyObjectBuilder_CubeBlock;
			if (myObjectBuilder_CubeBlock == null)
			{
				return null;
			}
			myObjectBuilder_CubeBlock.EntityId = cubeBlock.EntityId;
			myObjectBuilder_CubeBlock.Min = cubeBlock.Min;
			myObjectBuilder_CubeBlock.m_orientation = cubeBlock.m_orientation;
			myObjectBuilder_CubeBlock.IntegrityPercent = cubeBlock.IntegrityPercent;
			myObjectBuilder_CubeBlock.BuildPercent = cubeBlock.BuildPercent;
			myObjectBuilder_CubeBlock.BlockOrientation = cubeBlock.BlockOrientation;
			myObjectBuilder_CubeBlock.ConstructionInventory = cubeBlock.ConstructionInventory;
			myObjectBuilder_CubeBlock.ColorMaskHSV = cubeBlock.ColorMaskHSV;
			return myObjectBuilder_CubeBlock;
		}

		public bool ShouldSerializeConstructionStockpile()
		{
			return ConstructionStockpile != null;
		}

		public bool ShouldSerializeMultiBlockId()
		{
			return MultiBlockId != 0;
		}

		public bool ShouldSerializeMultiBlockDefinition()
		{
			if (MultiBlockId != 0)
			{
				return MultiBlockDefinition.HasValue;
			}
			return false;
		}

		public bool ShouldSerializeComponentContainer()
		{
			if (ComponentContainer != null && ComponentContainer.Components != null)
			{
				return ComponentContainer.Components.Count > 0;
			}
			return false;
		}

		public virtual void Remap(IMyRemapHelper remapHelper)
		{
			if (EntityId != 0L)
			{
				EntityId = remapHelper.RemapEntityId(EntityId);
			}
			if (SubBlocks != null)
			{
				for (int i = 0; i < SubBlocks.Length; i++)
				{
					if (SubBlocks[i].SubGridId != 0L)
					{
						SubBlocks[i].SubGridId = remapHelper.RemapEntityId(SubBlocks[i].SubGridId);
					}
				}
			}
			if (MultiBlockId != 0 && MultiBlockDefinition.HasValue)
			{
				MultiBlockId = remapHelper.RemapGroupId("MultiBlockId", MultiBlockId);
			}
		}

		public virtual void SetupForProjector()
		{
			Owner = 0L;
			ShareMode = MyOwnershipShareModeEnum.None;
			if (ComponentContainer != null)
			{
				MyObjectBuilder_ComponentContainer.ComponentData componentData = ComponentContainer.Components.Find((MyObjectBuilder_ComponentContainer.ComponentData s) => s.Component.TypeId == typeof(MyObjectBuilder_Inventory));
				if (componentData != null)
				{
					(componentData.Component as MyObjectBuilder_Inventory).Clear();
				}
				foreach (MyObjectBuilder_ComponentContainer.ComponentData component in ComponentContainer.Components)
				{
					if (component.Component.TypeId == typeof(MyObjectBuilder_InventoryAggregate))
					{
						(component.Component as MyObjectBuilder_InventoryAggregate).Clear();
					}
				}
			}
		}
	}
}
