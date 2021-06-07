using System;
using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.ObjectBuilders;

namespace VRage.Game.ModAPI
{
	public interface IMyInventory : VRage.Game.ModAPI.Ingame.IMyInventory
	{
		new VRage.ModAPI.IMyEntity Owner
		{
			get;
		}

		bool Empty();

		void Clear(bool sync = true);

		bool CanAddItemAmount(IMyInventoryItem item, MyFixedPoint amount);

		void AddItems(MyFixedPoint amount, MyObjectBuilder_PhysicalObject objectBuilder, int index = -1);

		void RemoveItemsOfType(MyFixedPoint amount, MyObjectBuilder_PhysicalObject objectBuilder, bool spawn = false);

		void RemoveItemsOfType(MyFixedPoint amount, SerializableDefinitionId contentId, MyItemFlags flags = MyItemFlags.None, bool spawn = false);

		void RemoveItemsAt(int itemIndex, MyFixedPoint? amount = null, bool sendEvent = true, bool spawn = false);

		void RemoveItems(uint itemId, MyFixedPoint? amount = null, bool sendEvent = true, bool spawn = false);

		void RemoveItemAmount(IMyInventoryItem item, MyFixedPoint amount);

		bool TransferItemTo(IMyInventory dst, int sourceItemIndex, int? targetItemIndex = null, bool? stackIfPossible = null, MyFixedPoint? amount = null, bool checkConnection = true);

		bool TransferItemFrom(IMyInventory sourceInventory, int sourceItemIndex, int? targetItemIndex = null, bool? stackIfPossible = null, MyFixedPoint? amount = null, bool checkConnection = true);

		bool TransferItemFrom(IMyInventory sourceInventory, IMyInventoryItem item, MyFixedPoint amount);

		[Obsolete("Use non-allocating GetItems overload")]
		List<IMyInventoryItem> GetItems();

		new IMyInventoryItem GetItemByID(uint id);

		IMyInventoryItem FindItem(SerializableDefinitionId contentId);
	}
}
