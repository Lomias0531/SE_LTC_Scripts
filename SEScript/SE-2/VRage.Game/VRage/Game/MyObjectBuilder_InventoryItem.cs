using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_InventoryItem : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_InventoryItem_003C_003EAmount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryItem, MyFixedPoint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_InventoryItem owner, in MyFixedPoint value)
			{
				owner.Amount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_InventoryItem owner, out MyFixedPoint value)
			{
				value = owner.Amount;
			}
		}

		protected class VRage_Game_MyObjectBuilder_InventoryItem_003C_003EScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryItem, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_InventoryItem owner, in float value)
			{
				owner.Scale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_InventoryItem owner, out float value)
			{
				value = owner.Scale;
			}
		}

		protected class VRage_Game_MyObjectBuilder_InventoryItem_003C_003EContent_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryItem, MyObjectBuilder_PhysicalObject>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_InventoryItem owner, in MyObjectBuilder_PhysicalObject value)
			{
				owner.Content = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_InventoryItem owner, out MyObjectBuilder_PhysicalObject value)
			{
				value = owner.Content;
			}
		}

		protected class VRage_Game_MyObjectBuilder_InventoryItem_003C_003EPhysicalContent_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryItem, MyObjectBuilder_PhysicalObject>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_InventoryItem owner, in MyObjectBuilder_PhysicalObject value)
			{
				owner.PhysicalContent = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_InventoryItem owner, out MyObjectBuilder_PhysicalObject value)
			{
				value = owner.PhysicalContent;
			}
		}

		protected class VRage_Game_MyObjectBuilder_InventoryItem_003C_003EItemId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryItem, uint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_InventoryItem owner, in uint value)
			{
				owner.ItemId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_InventoryItem owner, out uint value)
			{
				value = owner.ItemId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_InventoryItem_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryItem, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_InventoryItem owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_InventoryItem owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_InventoryItem_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_InventoryItem owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_InventoryItem owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_InventoryItem_003C_003EObsolete_AmountDecimal_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryItem, decimal>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_InventoryItem owner, in decimal value)
			{
				owner.Obsolete_AmountDecimal = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_InventoryItem owner, out decimal value)
			{
				value = owner.Obsolete_AmountDecimal;
			}
		}

		protected class VRage_Game_MyObjectBuilder_InventoryItem_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryItem, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_InventoryItem owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_InventoryItem owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_InventoryItem_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_InventoryItem owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_InventoryItem owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_InventoryItem_003C_003EActor : IActivator, IActivator<MyObjectBuilder_InventoryItem>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_InventoryItem();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_InventoryItem CreateInstance()
			{
				return new MyObjectBuilder_InventoryItem();
			}

			MyObjectBuilder_InventoryItem IActivator<MyObjectBuilder_InventoryItem>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[XmlElement("Amount")]
		public MyFixedPoint Amount = 1;

		[ProtoMember(4)]
		[XmlElement("Scale")]
		public float Scale = 1f;

		/// <summary>
		/// Obsolete. It is here only to keep backwards compatibility with old saves. Nulls content when unsupported.
		/// </summary>
		[ProtoMember(7)]
		[XmlElement(Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_PhysicalObject>))]
		[DynamicObjectBuilder(false)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public MyObjectBuilder_PhysicalObject Content;

		[ProtoMember(10)]
		[XmlElement(Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_PhysicalObject>))]
		[DynamicObjectBuilder(false)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public MyObjectBuilder_PhysicalObject PhysicalContent;

		[ProtoMember(13)]
		public uint ItemId;

		[XmlElement("AmountDecimal")]
		[NoSerialize]
		public decimal Obsolete_AmountDecimal
		{
			get
			{
				return (decimal)Amount;
			}
			set
			{
				Amount = (MyFixedPoint)value;
			}
		}

		public bool ShouldSerializeScale()
		{
			return Scale != 1f;
		}

		public bool ShouldSerializeObsolete_AmountDecimal()
		{
			return false;
		}

		public bool ShouldSerializeContent()
		{
			return false;
		}
	}
}
