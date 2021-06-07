using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_StoreItem
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StoreItem_003C_003EId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StoreItem, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StoreItem owner, in long value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StoreItem owner, out long value)
			{
				value = owner.Id;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StoreItem_003C_003EItem_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StoreItem, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StoreItem owner, in SerializableDefinitionId? value)
			{
				owner.Item = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StoreItem owner, out SerializableDefinitionId? value)
			{
				value = owner.Item;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StoreItem_003C_003EItemType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StoreItem, ItemTypes>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StoreItem owner, in ItemTypes value)
			{
				owner.ItemType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StoreItem owner, out ItemTypes value)
			{
				value = owner.ItemType;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StoreItem_003C_003EAmount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StoreItem, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StoreItem owner, in int value)
			{
				owner.Amount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StoreItem owner, out int value)
			{
				value = owner.Amount;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StoreItem_003C_003ERemovedAmount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StoreItem, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StoreItem owner, in int value)
			{
				owner.RemovedAmount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StoreItem owner, out int value)
			{
				value = owner.RemovedAmount;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StoreItem_003C_003EPricePerUnit_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StoreItem, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StoreItem owner, in int value)
			{
				owner.PricePerUnit = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StoreItem owner, out int value)
			{
				value = owner.PricePerUnit;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StoreItem_003C_003EStoreItemType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StoreItem, StoreItemTypes>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StoreItem owner, in StoreItemTypes value)
			{
				owner.StoreItemType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StoreItem owner, out StoreItemTypes value)
			{
				value = owner.StoreItemType;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StoreItem_003C_003EUpdateCount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StoreItem, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StoreItem owner, in byte value)
			{
				owner.UpdateCount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StoreItem owner, out byte value)
			{
				value = owner.UpdateCount;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StoreItem_003C_003EPrefabName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StoreItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StoreItem owner, in string value)
			{
				owner.PrefabName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StoreItem owner, out string value)
			{
				value = owner.PrefabName;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StoreItem_003C_003EPrefabTotalPcu_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StoreItem, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StoreItem owner, in int value)
			{
				owner.PrefabTotalPcu = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StoreItem owner, out int value)
			{
				value = owner.PrefabTotalPcu;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StoreItem_003C_003EPricePerUnitDiscount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StoreItem, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StoreItem owner, in float value)
			{
				owner.PricePerUnitDiscount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StoreItem owner, out float value)
			{
				value = owner.PricePerUnitDiscount;
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StoreItem_003C_003EActor : IActivator, IActivator<MyObjectBuilder_StoreItem>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_StoreItem();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_StoreItem CreateInstance()
			{
				return new MyObjectBuilder_StoreItem();
			}

			MyObjectBuilder_StoreItem IActivator<MyObjectBuilder_StoreItem>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public long Id;

		[ProtoMember(3)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableDefinitionId? Item;

		[ProtoMember(4)]
		public ItemTypes ItemType;

		[ProtoMember(5)]
		public int Amount;

		[ProtoMember(7)]
		public int RemovedAmount;

		[ProtoMember(9)]
		public int PricePerUnit;

		[ProtoMember(11)]
		public StoreItemTypes StoreItemType;

		[ProtoMember(13)]
		public byte UpdateCount;

		[ProtoMember(15)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public string PrefabName;

		[ProtoMember(17)]
		public int PrefabTotalPcu;

		[ProtoMember(19)]
		public float PricePerUnitDiscount;
	}
}
