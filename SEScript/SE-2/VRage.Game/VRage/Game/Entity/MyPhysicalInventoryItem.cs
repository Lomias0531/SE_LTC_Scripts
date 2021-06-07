using System;
using System.Runtime.CompilerServices;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Network;
using VRage.ObjectBuilders;

namespace VRage.Game.Entity
{
	[Serializable]
	public struct MyPhysicalInventoryItem : VRage.Game.ModAPI.IMyInventoryItem, VRage.Game.ModAPI.Ingame.IMyInventoryItem
	{
		protected class VRage_Game_Entity_MyPhysicalInventoryItem_003C_003EAmount_003C_003EAccessor : IMemberAccessor<MyPhysicalInventoryItem, MyFixedPoint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPhysicalInventoryItem owner, in MyFixedPoint value)
			{
				owner.Amount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPhysicalInventoryItem owner, out MyFixedPoint value)
			{
				value = owner.Amount;
			}
		}

		protected class VRage_Game_Entity_MyPhysicalInventoryItem_003C_003EScale_003C_003EAccessor : IMemberAccessor<MyPhysicalInventoryItem, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPhysicalInventoryItem owner, in float value)
			{
				owner.Scale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPhysicalInventoryItem owner, out float value)
			{
				value = owner.Scale;
			}
		}

		protected class VRage_Game_Entity_MyPhysicalInventoryItem_003C_003EContent_003C_003EAccessor : IMemberAccessor<MyPhysicalInventoryItem, MyObjectBuilder_PhysicalObject>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPhysicalInventoryItem owner, in MyObjectBuilder_PhysicalObject value)
			{
				owner.Content = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPhysicalInventoryItem owner, out MyObjectBuilder_PhysicalObject value)
			{
				value = owner.Content;
			}
		}

		protected class VRage_Game_Entity_MyPhysicalInventoryItem_003C_003EItemId_003C_003EAccessor : IMemberAccessor<MyPhysicalInventoryItem, uint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPhysicalInventoryItem owner, in uint value)
			{
				owner.ItemId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPhysicalInventoryItem owner, out uint value)
			{
				value = owner.ItemId;
			}
		}

		protected class VRage_Game_Entity_MyPhysicalInventoryItem_003C_003EVRage_002EGame_002EModAPI_002EIngame_002EIMyInventoryItem_002EAmount_003C_003EAccessor : IMemberAccessor<MyPhysicalInventoryItem, MyFixedPoint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPhysicalInventoryItem owner, in MyFixedPoint value)
			{
				owner.VRage_002EGame_002EModAPI_002EIngame_002EIMyInventoryItem_002EAmount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPhysicalInventoryItem owner, out MyFixedPoint value)
			{
				value = owner.VRage_002EGame_002EModAPI_002EIngame_002EIMyInventoryItem_002EAmount;
			}
		}

		protected class VRage_Game_Entity_MyPhysicalInventoryItem_003C_003EVRage_002EGame_002EModAPI_002EIngame_002EIMyInventoryItem_002EScale_003C_003EAccessor : IMemberAccessor<MyPhysicalInventoryItem, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPhysicalInventoryItem owner, in float value)
			{
				owner.VRage_002EGame_002EModAPI_002EIngame_002EIMyInventoryItem_002EScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPhysicalInventoryItem owner, out float value)
			{
				value = owner.VRage_002EGame_002EModAPI_002EIngame_002EIMyInventoryItem_002EScale;
			}
		}

		protected class VRage_Game_Entity_MyPhysicalInventoryItem_003C_003EVRage_002EGame_002EModAPI_002EIngame_002EIMyInventoryItem_002EContent_003C_003EAccessor : IMemberAccessor<MyPhysicalInventoryItem, MyObjectBuilder_Base>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPhysicalInventoryItem owner, in MyObjectBuilder_Base value)
			{
				owner.VRage_002EGame_002EModAPI_002EIngame_002EIMyInventoryItem_002EContent = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPhysicalInventoryItem owner, out MyObjectBuilder_Base value)
			{
				value = owner.VRage_002EGame_002EModAPI_002EIngame_002EIMyInventoryItem_002EContent;
			}
		}

		protected class VRage_Game_Entity_MyPhysicalInventoryItem_003C_003EVRage_002EGame_002EModAPI_002EIngame_002EIMyInventoryItem_002EItemId_003C_003EAccessor : IMemberAccessor<MyPhysicalInventoryItem, uint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPhysicalInventoryItem owner, in uint value)
			{
				owner.VRage_002EGame_002EModAPI_002EIngame_002EIMyInventoryItem_002EItemId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPhysicalInventoryItem owner, out uint value)
			{
				value = owner.VRage_002EGame_002EModAPI_002EIngame_002EIMyInventoryItem_002EItemId;
			}
		}

		public MyFixedPoint Amount;

		public float Scale;

		[DynamicObjectBuilder(false)]
		public MyObjectBuilder_PhysicalObject Content;

		public uint ItemId;

		MyFixedPoint VRage.Game.ModAPI.Ingame.IMyInventoryItem.Amount
		{
			get
			{
				return Amount;
			}
			set
			{
				Amount = value;
			}
		}

		float VRage.Game.ModAPI.Ingame.IMyInventoryItem.Scale
		{
			get
			{
				return Scale;
			}
			set
			{
				Scale = value;
			}
		}

		MyObjectBuilder_Base VRage.Game.ModAPI.Ingame.IMyInventoryItem.Content
		{
			get
			{
				return Content;
			}
			set
			{
				Content = (value as MyObjectBuilder_PhysicalObject);
			}
		}

		uint VRage.Game.ModAPI.Ingame.IMyInventoryItem.ItemId
		{
			get
			{
				return ItemId;
			}
			set
			{
				ItemId = value;
			}
		}

		public MyPhysicalInventoryItem(MyFixedPoint amount, MyObjectBuilder_PhysicalObject content, float scale = 1f)
		{
			ItemId = 0u;
			Amount = amount;
			Scale = scale;
			Content = content;
		}

		public MyPhysicalInventoryItem(MyObjectBuilder_InventoryItem item)
		{
			ItemId = 0u;
			Amount = item.Amount;
			Scale = item.Scale;
			Content = (item.PhysicalContent.Clone() as MyObjectBuilder_PhysicalObject);
		}

		public MyObjectBuilder_InventoryItem GetObjectBuilder()
		{
			MyObjectBuilder_InventoryItem myObjectBuilder_InventoryItem = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_InventoryItem>();
			myObjectBuilder_InventoryItem.Amount = Amount;
			myObjectBuilder_InventoryItem.Scale = Scale;
			myObjectBuilder_InventoryItem.PhysicalContent = Content;
			myObjectBuilder_InventoryItem.ItemId = ItemId;
			return myObjectBuilder_InventoryItem;
		}

		public override string ToString()
		{
			return $"{Amount}x {Content.GetId()}";
		}
	}
}
