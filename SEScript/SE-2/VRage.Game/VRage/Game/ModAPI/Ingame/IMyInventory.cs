using System;
using System.Collections.Generic;

namespace VRage.Game.ModAPI.Ingame
{
	public interface IMyInventory
	{
		/// <summary>
		/// Returns entity this inventory belongs to.
		/// </summary>
		IMyEntity Owner
		{
			get;
		}

		/// <summary>
		/// Determines if inventory is absolutely full.
		/// </summary>
		bool IsFull
		{
			get;
		}

		/// <summary>
		/// Returns total mass of items inside this inventory in Kg.
		/// </summary>
		MyFixedPoint CurrentMass
		{
			get;
		}

		/// <summary>
		/// Returns maximum volume of items this inventory can contain in m^3.
		/// </summary>
		MyFixedPoint MaxVolume
		{
			get;
		}

		/// <summary>
		/// Returns total volume of items inside this inventory in m^3.
		/// </summary>
		MyFixedPoint CurrentVolume
		{
			get;
		}

		/// <summary>
		/// Returns number of occupied inventory slots.
		/// </summary>
		int ItemCount
		{
			get;
		}

		/// <summary>
		/// Determines if there is any item on given inventory slot.
		/// </summary>
		/// <param name="position">Zero-based index of queried inventory slot</param>
		/// <returns>True in case given inventory slot is occupied, false otherwise</returns>
		bool IsItemAt(int position);

		/// <summary>
		/// Sums up total amount of items of given type contained inside this inventory.
		/// </summary>
		/// <param name="itemType">Item type its amount is queried</param>
		/// <returns>Total amount of given item type contained inside this inventory. Kg or count, based on item type</returns>
		MyFixedPoint GetItemAmount(MyItemType itemType);

		/// <summary>
		/// Determines if there is at least given amount of items of given type contained inside this inventory.
		/// </summary>
		/// <param name="amount">Threshold amount. Kg or count, based on item type</param>
		/// <param name="itemType">Item type its amount is queried</param>
		/// <returns>True in case there is sufficient amount of items present inside this inventory, false otherwise</returns>
		bool ContainItems(MyFixedPoint amount, MyItemType itemType);

		/// <summary>
		/// Returns info about item at give position.
		/// </summary>
		/// <param name="index">Zero-based index of queried inventory slot</param>
		/// <returns>Info about queried inventory slot, null in case there is no item at given slot</returns>
		MyInventoryItem? GetItemAt(int index);

		/// <summary>
		/// Returns info about item contained inside this inventory.
		/// </summary>
		/// <param name="id">Id of queried item</param>
		/// <returns>Info about queried item, null in case there is no item with given Id inside this inventory</returns>
		MyInventoryItem? GetItemByID(uint id);

		/// <summary>
		/// Tries to find an item of given type inside this inventory.
		/// </summary>
		/// <param name="itemType">Type of item being queried</param>
		/// <returns>Info about item found, null in case there is no item of given type inside this inventory</returns>
		MyInventoryItem? FindItem(MyItemType itemType);

		/// <summary>
		/// Determines if given amount of items fits into this inventory on top of existing items.
		/// </summary>
		/// <param name="amount">Amount of items being tested</param>
		/// <param name="itemType">Type of items being tested</param>
		/// <returns>True if items can fit into this inventory on top of existing items, false otherwise</returns>
		bool CanItemsBeAdded(MyFixedPoint amount, MyItemType itemType);

		/// <summary>
		/// Collects all items present inside this inventory and returns snapshot of the current inventory state.
		/// </summary>
		/// <param name="items">Collection the item info will be returned to</param>
		/// <param name="filter">Filter function you can use to decide whether to add item into the items collection or not</param>
		void GetItems(List<MyInventoryItem> items, Func<MyInventoryItem, bool> filter = null);

		/// <summary>
		/// Attempts to transfer item from one inventory to another.
		/// </summary>
		/// <param name="dstInventory">Inventory to transfer item to</param>
		/// <param name="item">Item to transfer</param>
		/// <param name="amount">Amount that should be transferred. Kgs or count, based on item type</param>
		/// <returns>True in case item was successfully transferred, false otherwise</returns>
		bool TransferItemTo(IMyInventory dstInventory, MyInventoryItem item, MyFixedPoint? amount = null);

		/// <summary>
		/// Attempts to transfer item from one inventory to another.
		/// </summary>
		/// <param name="sourceInventory">Inventory to transfer item from</param>
		/// <param name="item">Item to transfer</param>
		/// <param name="amount">Amount that should be transferred. Kgs or count, based on item type</param>
		/// <returns>True in case item was successfully transferred, false otherwise</returns>
		bool TransferItemFrom(IMyInventory sourceInventory, MyInventoryItem item, MyFixedPoint? amount = null);

		/// <summary>
		/// Attempts to transfer item from one inventory to another.
		/// </summary>
		/// <param name="dst">Inventory to transfer item to</param>
		/// <param name="sourceItemIndex">Zero-based index of inventory slot to transfer item from</param>
		/// <param name="targetItemIndex">Zero-based index of inventory slot to transfer item to</param>
		/// <param name="stackIfPossible">If true, engine will attempt to stack items at destination inventory</param>
		/// <param name="amount">Amount that should be transferred. Kgs or count, based on item type</param>
		/// <returns>True in case item was successfully transferred, false otherwise</returns>
		bool TransferItemTo(IMyInventory dst, int sourceItemIndex, int? targetItemIndex = null, bool? stackIfPossible = null, MyFixedPoint? amount = null);

		/// <summary>
		/// Attempts to transfer item from one inventory to another.
		/// </summary>
		/// <param name="sourceInventory">Inventory to transfer item from</param>
		/// <param name="sourceItemIndex">Zero-based index of inventory slot to transfer item from</param>
		/// <param name="targetItemIndex">Zero-based index of inventory slot to transfer item to</param>
		/// <param name="stackIfPossible">If true, engine will attempt to stack items at destination inventory</param>
		/// <param name="amount">Amount that should be transferred. Kgs or count, based on item type</param>
		/// <returns>True in case item was successfully transferred, false otherwise</returns>
		bool TransferItemFrom(IMyInventory sourceInventory, int sourceItemIndex, int? targetItemIndex = null, bool? stackIfPossible = null, MyFixedPoint? amount = null);

		/// <summary>
		/// Checks if two inventories are connected.
		/// </summary>
		/// <param name="otherInventory">Inventory to check the connection to</param>
		/// <returns>True if there is working conveyor connection between inventories, false otherwise</returns>
		bool IsConnectedTo(IMyInventory otherInventory);

		/// <summary>
		/// Determines if there is working conveyor connection for item of give type to be transferred to other inventory.
		/// </summary>
		/// <param name="otherInventory">Inventory to check the connection to</param>
		/// <param name="itemType">Type of item to check the connection for</param>
		/// <returns>True if there is working conveyor connection between inventories so that item of give type can by transferred, false otherwise</returns>
		bool CanTransferItemTo(IMyInventory otherInventory, MyItemType itemType);

		/// <summary>
		/// Returns all items this inventory accepts.
		/// </summary>
		/// <param name="itemsTypes">Collection the item types info will be returned to</param>
		/// <param name="filter">Filter function you can use to decide whether to add item type into the itemsTypes collection or not</param>
		void GetAcceptedItems(List<MyItemType> itemsTypes, Func<MyItemType, bool> filter = null);
	}
}
