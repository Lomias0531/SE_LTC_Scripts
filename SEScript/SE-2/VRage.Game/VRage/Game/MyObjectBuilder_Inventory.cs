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
	public class MyObjectBuilder_Inventory : MyObjectBuilder_InventoryBase
	{
		protected class VRage_Game_MyObjectBuilder_Inventory_003C_003EItems_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Inventory, List<MyObjectBuilder_InventoryItem>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Inventory owner, in List<MyObjectBuilder_InventoryItem> value)
			{
				owner.Items = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Inventory owner, out List<MyObjectBuilder_InventoryItem> value)
			{
				value = owner.Items;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Inventory_003C_003EnextItemId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Inventory, uint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Inventory owner, in uint value)
			{
				owner.nextItemId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Inventory owner, out uint value)
			{
				value = owner.nextItemId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Inventory_003C_003EVolume_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Inventory, MyFixedPoint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Inventory owner, in MyFixedPoint? value)
			{
				owner.Volume = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Inventory owner, out MyFixedPoint? value)
			{
				value = owner.Volume;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Inventory_003C_003EMass_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Inventory, MyFixedPoint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Inventory owner, in MyFixedPoint? value)
			{
				owner.Mass = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Inventory owner, out MyFixedPoint? value)
			{
				value = owner.Mass;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Inventory_003C_003EMaxItemCount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Inventory, int?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Inventory owner, in int? value)
			{
				owner.MaxItemCount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Inventory owner, out int? value)
			{
				value = owner.MaxItemCount;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Inventory_003C_003ESize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Inventory, SerializableVector3?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Inventory owner, in SerializableVector3? value)
			{
				owner.Size = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Inventory owner, out SerializableVector3? value)
			{
				value = owner.Size;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Inventory_003C_003EInventoryFlags_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Inventory, MyInventoryFlags?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Inventory owner, in MyInventoryFlags? value)
			{
				owner.InventoryFlags = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Inventory owner, out MyInventoryFlags? value)
			{
				value = owner.InventoryFlags;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Inventory_003C_003ERemoveEntityOnEmpty_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Inventory, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Inventory owner, in bool value)
			{
				owner.RemoveEntityOnEmpty = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Inventory owner, out bool value)
			{
				value = owner.RemoveEntityOnEmpty;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Inventory_003C_003EInventoryId_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryBase_003C_003EInventoryId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Inventory, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Inventory owner, in string value)
			{
				Set(ref *(MyObjectBuilder_InventoryBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Inventory owner, out string value)
			{
				Get(ref *(MyObjectBuilder_InventoryBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Inventory_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Inventory, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Inventory owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Inventory owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Inventory_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Inventory, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Inventory owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Inventory owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Inventory_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Inventory, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Inventory owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Inventory owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Inventory_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Inventory, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Inventory owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Inventory owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_Inventory_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Inventory>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Inventory();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Inventory CreateInstance()
			{
				return new MyObjectBuilder_Inventory();
			}

			MyObjectBuilder_Inventory IActivator<MyObjectBuilder_Inventory>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public List<MyObjectBuilder_InventoryItem> Items = new List<MyObjectBuilder_InventoryItem>();

		[ProtoMember(4)]
		public uint nextItemId;

		[ProtoMember(7)]
		[DefaultValue(null)]
		public MyFixedPoint? Volume;

		[ProtoMember(10)]
		[DefaultValue(null)]
		public MyFixedPoint? Mass;

		[ProtoMember(13)]
		[DefaultValue(null)]
		public int? MaxItemCount;

		[ProtoMember(16)]
		[DefaultValue(null)]
		public SerializableVector3? Size;

		[ProtoMember(19)]
		[DefaultValue(null)]
		public MyInventoryFlags? InventoryFlags;

		[ProtoMember(22)]
		public bool RemoveEntityOnEmpty;

		public bool ShouldSerializeMaxItemCount()
		{
			return MaxItemCount.HasValue;
		}

		public override void Clear()
		{
			Items.Clear();
			nextItemId = 0u;
			base.Clear();
		}
	}
}
