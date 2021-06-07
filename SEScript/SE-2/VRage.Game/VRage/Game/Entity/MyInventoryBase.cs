using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.Entity
{
	[MyComponentType(typeof(MyInventoryBase))]
	[StaticEventOwner]
	public abstract class MyInventoryBase : MyEntityComponentBase, IMyEventProxy, IMyEventOwner
	{
		/// <summary>
		/// Setting this flag to true causes to call Close() on the Entity of Container, when the GetItemsCount() == 0.
		/// This causes to remove entity from the world, when this inventory is empty.
		/// </summary>
		public bool RemoveEntityOnEmpty;

		/// <summary>
		/// This is for the purpose of identifying the inventory in aggregates (i.e. "Backpack", "LeftHand", ...)
		/// </summary>
		public MyStringHash InventoryId
		{
			get;
			private set;
		}

		public abstract MyFixedPoint CurrentMass
		{
			get;
		}

		public abstract MyFixedPoint MaxMass
		{
			get;
		}

		public abstract int MaxItemCount
		{
			get;
		}

		public abstract MyFixedPoint CurrentVolume
		{
			get;
		}

		public abstract MyFixedPoint MaxVolume
		{
			get;
		}

		public abstract float? ForcedPriority
		{
			get;
			set;
		}

		public override string ComponentTypeDebugString => "Inventory";

		public override bool AttachSyncToEntity => false;

		/// <summary>
		/// Called when items were added or removed, or their amount has changed
		/// </summary>
		public event Action<MyInventoryBase> ContentsChanged;

		public event Action<MyInventoryBase> BeforeContentsChanged;

		public event Action<MyPhysicalInventoryItem, MyFixedPoint> ContentsAdded;

		public event Action<MyPhysicalInventoryItem, MyFixedPoint> ContentsRemoved;

		public event Action<MyInventoryBase, MyPhysicalInventoryItem, MyFixedPoint> InventoryContentChanged;

		/// <summary>
		/// Called if this inventory changed its owner
		/// </summary>
		public event Action<MyInventoryBase, MyComponentContainer> OwnerChanged;

		public MyInventoryBase(string inventoryId)
		{
			InventoryId = MyStringHash.GetOrCompute(inventoryId);
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			base.Deserialize(builder);
			MyObjectBuilder_InventoryBase myObjectBuilder_InventoryBase = builder as MyObjectBuilder_InventoryBase;
			InventoryId = MyStringHash.GetOrCompute(myObjectBuilder_InventoryBase.InventoryId ?? "Inventory");
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_InventoryBase obj = base.Serialize() as MyObjectBuilder_InventoryBase;
			obj.InventoryId = InventoryId.ToString();
			return obj;
		}

		public override string ToString()
		{
			return base.ToString() + " - " + InventoryId.ToString();
		}

		public abstract MyFixedPoint ComputeAmountThatFits(MyDefinitionId contentId, float volumeRemoved = 0f, float massRemoved = 0f);

		public abstract MyFixedPoint GetItemAmount(MyDefinitionId contentId, MyItemFlags flags = MyItemFlags.None, bool substitute = false);

		public abstract bool ItemsCanBeAdded(MyFixedPoint amount, IMyInventoryItem item);

		public abstract bool ItemsCanBeRemoved(MyFixedPoint amount, IMyInventoryItem item);

		public abstract bool Add(IMyInventoryItem item, MyFixedPoint amount);

		public abstract bool Remove(IMyInventoryItem item, MyFixedPoint amount);

		public abstract void CountItems(Dictionary<MyDefinitionId, MyFixedPoint> itemCounts);

		public abstract void ApplyChanges(List<MyComponentChange> changes);

		public abstract List<MyPhysicalInventoryItem> GetItems();

		/// <summary>
		/// Adds item to inventory
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="objectBuilder"></param>
		/// <param name="index"></param>
		/// <returns>true if items were added, false if items didn't fit</returns>
		public abstract bool AddItems(MyFixedPoint amount, MyObjectBuilder_Base objectBuilder);

		/// <summary>
		/// Remove items of a given amount and definition
		/// </summary>
		/// <param name="amount">amount ot remove</param>
		/// <param name="contentId">definition id of items to be removed</param>
		/// <param name="spawn">Set tru to spawn object in the world, after it was removed</param>
		/// <returns>Returns the actually removed amount</returns>
		public abstract MyFixedPoint RemoveItemsOfType(MyFixedPoint amount, MyDefinitionId contentId, MyItemFlags flags = MyItemFlags.None, bool spawn = false);

		public abstract void OnContentsChanged();

		public abstract void OnBeforeContentsChanged();

		public abstract void OnContentsAdded(MyPhysicalInventoryItem item, MyFixedPoint amount);

		public abstract void OnContentsRemoved(MyPhysicalInventoryItem item, MyFixedPoint amount);

		/// <summary>
		/// Returns the number of items in the inventory. This needs to be overrided, otherwise it returns 0!
		/// </summary>
		/// <returns>int - number of items in inventory</returns>
		public virtual int GetItemsCount()
		{
			return 0;
		}

		/// <summary>
		/// Returns number of embedded inventories - this inventory can be aggregation of other inventories.
		/// </summary>
		/// <returns>Return one for simple inventory, different number when this instance is an aggregation.</returns>
		public abstract int GetInventoryCount();

		/// <summary>
		/// Search for inventory having given search index. 
		/// Aggregate inventory: Iterates through aggregate inventory until simple inventory with matching index is found.
		/// Simple inventory: Returns itself if currentIndex == searchIndex.
		///
		/// Usage: searchIndex = index of inventory being searched, leave currentIndex = 0.
		/// </summary>
		public abstract MyInventoryBase IterateInventory(int searchIndex, int currentIndex = 0);

		protected void OnOwnerChanged()
		{
			this.OwnerChanged?.Invoke(this, base.Container);
		}

		public override bool IsSerialized()
		{
			return true;
		}

		public abstract void ConsumeItem(MyDefinitionId itemId, MyFixedPoint amount, long consumerEntityId = 0L);

		public void RaiseContentsChanged()
		{
			this.ContentsChanged.InvokeIfNotNull(this);
		}

		public void RaiseInventoryContentChanged(MyPhysicalInventoryItem item, MyFixedPoint amount)
		{
			this.InventoryContentChanged.InvokeIfNotNull(this, item, amount);
		}

		public void RaiseBeforeContentsChanged()
		{
			this.BeforeContentsChanged.InvokeIfNotNull(this);
		}

		public void RaiseContentsAdded(MyPhysicalInventoryItem item, MyFixedPoint amount)
		{
			this.ContentsAdded.InvokeIfNotNull(item, amount);
		}

		public void RaiseContentsRemoved(MyPhysicalInventoryItem item, MyFixedPoint amount)
		{
			this.ContentsRemoved.InvokeIfNotNull(item, amount);
		}
	}
}
