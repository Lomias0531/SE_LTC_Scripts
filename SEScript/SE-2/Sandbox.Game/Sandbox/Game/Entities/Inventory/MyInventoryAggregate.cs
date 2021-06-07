using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.Entities.Inventory
{
	[MyComponentBuilder(typeof(MyObjectBuilder_InventoryAggregate), true)]
	[StaticEventOwner]
	public class MyInventoryAggregate : MyInventoryBase, IMyComponentAggregate, IMyEventProxy, IMyEventOwner
	{
		protected sealed class InventoryConsumeItem_Implementation_003C_003EVRage_MyFixedPoint_0023VRage_ObjectBuilders_SerializableDefinitionId_0023System_Int64 : ICallSite<MyInventoryAggregate, MyFixedPoint, SerializableDefinitionId, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyInventoryAggregate @this, in MyFixedPoint amount, in SerializableDefinitionId itemId, in long consumerEntityId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.InventoryConsumeItem_Implementation(amount, itemId, consumerEntityId);
			}
		}

		private class Sandbox_Game_Entities_Inventory_MyInventoryAggregate_003C_003EActor : IActivator, IActivator<MyInventoryAggregate>
		{
			private sealed override object CreateInstance()
			{
				return new MyInventoryAggregate();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyInventoryAggregate CreateInstance()
			{
				return new MyInventoryAggregate();
			}

			MyInventoryAggregate IActivator<MyInventoryAggregate>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyAggregateComponentList m_children = new MyAggregateComponentList();

		private List<MyComponentBase> tmp_list = new List<MyComponentBase>();

		private List<MyPhysicalInventoryItem> m_allItems = new List<MyPhysicalInventoryItem>();

		private float? m_forcedPriority;

		private int m_inventoryCount;

		public override MyFixedPoint CurrentMass
		{
			get
			{
				float num = 0f;
				foreach (MyInventoryBase item in m_children.Reader)
				{
					num += (float)item.CurrentMass;
				}
				return (MyFixedPoint)num;
			}
		}

		public override MyFixedPoint MaxMass
		{
			get
			{
				float num = 0f;
				foreach (MyInventoryBase item in m_children.Reader)
				{
					num += (float)item.MaxMass;
				}
				return (MyFixedPoint)num;
			}
		}

		public override MyFixedPoint CurrentVolume
		{
			get
			{
				float num = 0f;
				foreach (MyInventoryBase item in m_children.Reader)
				{
					num += (float)item.CurrentVolume;
				}
				return (MyFixedPoint)num;
			}
		}

		public override MyFixedPoint MaxVolume
		{
			get
			{
				float num = 0f;
				foreach (MyInventoryBase item in m_children.Reader)
				{
					num += (float)item.MaxVolume;
				}
				return (MyFixedPoint)num;
			}
		}

		public override int MaxItemCount
		{
			get
			{
				int num = 0;
				foreach (MyInventoryBase item in m_children.Reader)
				{
					long num2 = (long)num + (long)item.MaxItemCount;
					num = (int)((num2 <= int.MaxValue) ? num2 : int.MaxValue);
				}
				return num;
			}
		}

		public override float? ForcedPriority
		{
			get
			{
				return m_forcedPriority;
			}
			set
			{
				m_forcedPriority = value;
				foreach (MyComponentBase item in m_children.Reader)
				{
					(item as MyInventoryBase).ForcedPriority = value;
				}
			}
		}

		public int InventoryCount
		{
			get
			{
				return m_inventoryCount;
			}
			private set
			{
				if (m_inventoryCount != value)
				{
					int arg = value - m_inventoryCount;
					m_inventoryCount = value;
					if (this.OnInventoryCountChanged != null)
					{
						this.OnInventoryCountChanged(this, arg);
					}
				}
			}
		}

		public MyAggregateComponentList ChildList => m_children;

		public virtual event Action<MyInventoryAggregate, MyInventoryBase> OnAfterComponentAdd;

		public virtual event Action<MyInventoryAggregate, MyInventoryBase> OnBeforeComponentRemove;

		public event Action<MyInventoryAggregate, int> OnInventoryCountChanged;

		public MyInventoryAggregate()
			: base("Inventory")
		{
		}

		public MyInventoryAggregate(string inventoryId)
			: base(inventoryId)
		{
		}

		public void Init()
		{
			foreach (MyInventoryBase item in m_children.Reader)
			{
				item.ContentsChanged += child_OnContentsChanged;
			}
		}

		public void DetachCallbacks()
		{
			foreach (MyInventoryBase item in m_children.Reader)
			{
				item.ContentsChanged -= child_OnContentsChanged;
			}
		}

		~MyInventoryAggregate()
		{
			DetachCallbacks();
		}

		public override MyFixedPoint ComputeAmountThatFits(MyDefinitionId contentId, float volumeRemoved = 0f, float massRemoved = 0f)
		{
			float num = 0f;
			foreach (MyInventoryBase item in m_children.Reader)
			{
				num += (float)item.ComputeAmountThatFits(contentId, volumeRemoved, massRemoved);
			}
			return (MyFixedPoint)num;
		}

		public override MyFixedPoint GetItemAmount(MyDefinitionId contentId, MyItemFlags flags = MyItemFlags.None, bool substitute = false)
		{
			float num = 0f;
			foreach (MyInventoryBase item in m_children.Reader)
			{
				num += (float)item.GetItemAmount(contentId, flags, substitute);
			}
			return (MyFixedPoint)num;
		}

		public override bool AddItems(MyFixedPoint amount, MyObjectBuilder_Base objectBuilder)
		{
			MyFixedPoint b = ComputeAmountThatFits(objectBuilder.GetId());
			MyFixedPoint myFixedPoint = amount;
			if (amount <= b)
			{
				foreach (MyInventoryBase item in m_children.Reader)
				{
					MyFixedPoint myFixedPoint2 = item.ComputeAmountThatFits(objectBuilder.GetId());
					if (myFixedPoint2 > myFixedPoint)
					{
						myFixedPoint2 = myFixedPoint;
					}
					if (myFixedPoint2 > 0 && item.AddItems(myFixedPoint2, objectBuilder))
					{
						myFixedPoint -= myFixedPoint2;
					}
					if (myFixedPoint == 0)
					{
						break;
					}
				}
			}
			return myFixedPoint == 0;
		}

		public override MyFixedPoint RemoveItemsOfType(MyFixedPoint amount, MyDefinitionId contentId, MyItemFlags flags = MyItemFlags.None, bool spawn = false)
		{
			MyFixedPoint myFixedPoint = amount;
			foreach (MyInventoryBase item in m_children.Reader)
			{
				myFixedPoint -= item.RemoveItemsOfType(myFixedPoint, contentId, flags, spawn);
			}
			return amount - myFixedPoint;
		}

		public MyInventoryBase GetInventory(MyStringHash id)
		{
			foreach (MyComponentBase item in m_children.Reader)
			{
				MyInventoryBase myInventoryBase = item as MyInventoryBase;
				if (myInventoryBase.InventoryId == id)
				{
					return myInventoryBase;
				}
			}
			return null;
		}

		public void AfterComponentAdd(MyComponentBase component)
		{
			MyInventoryBase myInventoryBase = component as MyInventoryBase;
			myInventoryBase.ForcedPriority = ForcedPriority;
			myInventoryBase.ContentsChanged += child_OnContentsChanged;
			if (component is MyInventory)
			{
				InventoryCount++;
			}
			else if (component is MyInventoryAggregate)
			{
				(component as MyInventoryAggregate).OnInventoryCountChanged += OnChildAggregateCountChanged;
				InventoryCount += (component as MyInventoryAggregate).InventoryCount;
			}
			if (this.OnAfterComponentAdd != null)
			{
				this.OnAfterComponentAdd(this, myInventoryBase);
			}
		}

		private void OnChildAggregateCountChanged(MyInventoryAggregate obj, int change)
		{
			InventoryCount += change;
		}

		public void BeforeComponentRemove(MyComponentBase component)
		{
			MyInventoryBase myInventoryBase = component as MyInventoryBase;
			myInventoryBase.ForcedPriority = null;
			myInventoryBase.ContentsChanged -= child_OnContentsChanged;
			if (this.OnBeforeComponentRemove != null)
			{
				this.OnBeforeComponentRemove(this, myInventoryBase);
			}
			if (component is MyInventory)
			{
				InventoryCount--;
			}
			else if (component is MyInventoryAggregate)
			{
				(component as MyInventoryAggregate).OnInventoryCountChanged -= OnChildAggregateCountChanged;
				InventoryCount -= (component as MyInventoryAggregate).InventoryCount;
			}
		}

		private void child_OnContentsChanged(MyComponentBase obj)
		{
			OnContentsChanged();
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_InventoryAggregate myObjectBuilder_InventoryAggregate = base.Serialize(copy: false) as MyObjectBuilder_InventoryAggregate;
			ListReader<MyComponentBase> reader = m_children.Reader;
			if (reader.Count > 0)
			{
				myObjectBuilder_InventoryAggregate.Inventories = new List<MyObjectBuilder_InventoryBase>(reader.Count);
				{
					foreach (MyComponentBase item in reader)
					{
						MyObjectBuilder_InventoryBase myObjectBuilder_InventoryBase = item.Serialize() as MyObjectBuilder_InventoryBase;
						if (myObjectBuilder_InventoryBase != null)
						{
							myObjectBuilder_InventoryAggregate.Inventories.Add(myObjectBuilder_InventoryBase);
						}
					}
					return myObjectBuilder_InventoryAggregate;
				}
			}
			return myObjectBuilder_InventoryAggregate;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			base.Deserialize(builder);
			MyObjectBuilder_InventoryAggregate myObjectBuilder_InventoryAggregate = builder as MyObjectBuilder_InventoryAggregate;
			if (myObjectBuilder_InventoryAggregate != null && myObjectBuilder_InventoryAggregate.Inventories != null)
			{
				foreach (MyObjectBuilder_InventoryBase inventory in myObjectBuilder_InventoryAggregate.Inventories)
				{
					MyComponentBase myComponentBase = MyComponentFactory.CreateInstanceByTypeId(inventory.TypeId);
					myComponentBase.Deserialize(inventory);
					this.AddComponent(myComponentBase);
				}
			}
		}

		public override void CountItems(Dictionary<MyDefinitionId, MyFixedPoint> itemCounts)
		{
			foreach (MyInventoryBase item in m_children.Reader)
			{
				item.CountItems(itemCounts);
			}
		}

		public override void ApplyChanges(List<MyComponentChange> changes)
		{
			foreach (MyInventoryBase item in m_children.Reader)
			{
				item.ApplyChanges(changes);
			}
		}

		public override bool ItemsCanBeAdded(MyFixedPoint amount, IMyInventoryItem item)
		{
			foreach (MyInventoryBase item2 in m_children.Reader)
			{
				if (item2.ItemsCanBeAdded(amount, item))
				{
					return true;
				}
			}
			return false;
		}

		public override bool ItemsCanBeRemoved(MyFixedPoint amount, IMyInventoryItem item)
		{
			foreach (MyInventoryBase item2 in m_children.Reader)
			{
				if (item2.ItemsCanBeRemoved(amount, item))
				{
					return true;
				}
			}
			return false;
		}

		public override bool Add(IMyInventoryItem item, MyFixedPoint amount)
		{
			foreach (MyInventoryBase item2 in m_children.Reader)
			{
				if (item2.ItemsCanBeAdded(amount, item) && item2.Add(item, amount))
				{
					return true;
				}
			}
			return false;
		}

		public override bool Remove(IMyInventoryItem item, MyFixedPoint amount)
		{
			foreach (MyInventoryBase item2 in m_children.Reader)
			{
				if (item2.ItemsCanBeRemoved(amount, item) && item2.Remove(item, amount))
				{
					return true;
				}
			}
			return false;
		}

		public override List<MyPhysicalInventoryItem> GetItems()
		{
			m_allItems.Clear();
			foreach (MyInventoryBase item in m_children.Reader)
			{
				m_allItems.AddRange(item.GetItems());
			}
			return m_allItems;
		}

		public override void OnContentsChanged()
		{
			RaiseContentsChanged();
			if (Sync.IsServer && RemoveEntityOnEmpty && GetItemsCount() == 0)
			{
				base.Container.Entity.Close();
			}
		}

		public override void OnBeforeContentsChanged()
		{
			RaiseBeforeContentsChanged();
		}

		public override void OnContentsAdded(MyPhysicalInventoryItem item, MyFixedPoint amount)
		{
			RaiseContentsAdded(item, amount);
			RaiseInventoryContentChanged(item, amount);
		}

		public override void OnContentsRemoved(MyPhysicalInventoryItem item, MyFixedPoint amount)
		{
			RaiseContentsRemoved(item, amount);
			RaiseInventoryContentChanged(item, -amount);
		}

		public override void ConsumeItem(MyDefinitionId itemId, MyFixedPoint amount, long consumerEntityId = 0L)
		{
			SerializableDefinitionId arg = itemId;
			MyMultiplayer.RaiseEvent(this, (MyInventoryAggregate x) => x.InventoryConsumeItem_Implementation, amount, arg, consumerEntityId);
		}

		public override int GetInventoryCount()
		{
			return InventoryCount;
		}

		public override MyInventoryBase IterateInventory(int searchIndex, int currentIndex)
		{
			foreach (MyComponentBase item in ChildList.Reader)
			{
				MyInventoryBase myInventoryBase = item as MyInventoryBase;
				if (myInventoryBase != null)
				{
					MyInventoryBase myInventoryBase2 = myInventoryBase.IterateInventory(searchIndex, currentIndex);
					if (myInventoryBase2 != null)
					{
						return myInventoryBase2;
					}
					if (myInventoryBase is MyInventory)
					{
						currentIndex++;
					}
				}
			}
			return null;
		}

		[Event(null, 479)]
		[Reliable]
		[Server]
		private void InventoryConsumeItem_Implementation(MyFixedPoint amount, SerializableDefinitionId itemId, long consumerEntityId)
		{
			if (consumerEntityId != 0L && !MyEntities.EntityExists(consumerEntityId))
			{
				return;
			}
			MyFixedPoint itemAmount = GetItemAmount(itemId);
			if (itemAmount < amount)
			{
				amount = itemAmount;
			}
			MyEntity myEntity = null;
			if (consumerEntityId != 0L)
			{
				myEntity = MyEntities.GetEntityById(consumerEntityId);
				if (myEntity == null)
				{
					return;
				}
			}
			if (myEntity.Components != null)
			{
				MyUsableItemDefinition myUsableItemDefinition = MyDefinitionManager.Static.GetDefinition(itemId) as MyUsableItemDefinition;
				if (myUsableItemDefinition != null)
				{
					(myEntity as MyCharacter)?.SoundComp.StartSecondarySound(myUsableItemDefinition.UseSound, sync: true);
				}
			}
			if (true)
			{
				RemoveItemsOfType(amount, itemId);
			}
		}

		public static MyInventoryAggregate FixInputOutputInventories(MyInventoryAggregate inventoryAggregate, MyInventoryConstraint inputInventoryConstraint, MyInventoryConstraint outputInventoryConstraint)
		{
			MyInventory myInventory = null;
			MyInventory myInventory2 = null;
			foreach (MyComponentBase item in inventoryAggregate.ChildList.Reader)
			{
				MyInventory myInventory3 = item as MyInventory;
				if (myInventory3 != null && myInventory3.GetItemsCount() > 0)
				{
					if (myInventory == null)
					{
						bool flag = true;
						if (inputInventoryConstraint != null)
						{
							foreach (MyPhysicalInventoryItem item2 in myInventory3.GetItems())
							{
								flag &= inputInventoryConstraint.Check(item2.GetDefinitionId());
							}
						}
						if (flag)
						{
							myInventory = myInventory3;
						}
					}
					if (myInventory2 == null && myInventory != myInventory3)
					{
						bool flag2 = true;
						if (outputInventoryConstraint != null)
						{
							foreach (MyPhysicalInventoryItem item3 in myInventory3.GetItems())
							{
								flag2 &= outputInventoryConstraint.Check(item3.GetDefinitionId());
							}
						}
						if (flag2)
						{
							myInventory2 = myInventory3;
						}
					}
				}
			}
			if (myInventory == null || myInventory2 == null)
			{
				foreach (MyComponentBase item4 in inventoryAggregate.ChildList.Reader)
				{
					MyInventory myInventory4 = item4 as MyInventory;
					if (myInventory4 != null)
					{
						if (myInventory == null)
						{
							myInventory = myInventory4;
						}
						else
						{
							if (myInventory2 != null)
							{
								break;
							}
							myInventory2 = myInventory4;
						}
					}
				}
			}
			inventoryAggregate.RemoveComponent(myInventory);
			inventoryAggregate.RemoveComponent(myInventory2);
			MyInventoryAggregate myInventoryAggregate = new MyInventoryAggregate();
			myInventoryAggregate.AddComponent(myInventory);
			myInventoryAggregate.AddComponent(myInventory2);
			return myInventoryAggregate;
		}

		MyComponentContainer IMyComponentAggregate.get_ContainerBase()
		{
			return base.ContainerBase;
		}
	}
}
