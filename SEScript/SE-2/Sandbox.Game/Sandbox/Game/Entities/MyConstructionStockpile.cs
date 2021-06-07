using System.Collections.Generic;
using VRage.Game;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Entities
{
	public class MyConstructionStockpile
	{
		private List<MyStockpileItem> m_items = new List<MyStockpileItem>();

		private static List<MyStockpileItem> m_syncItems = new List<MyStockpileItem>();

		public int LastChangeStamp
		{
			get;
			private set;
		}

		public MyConstructionStockpile()
		{
			LastChangeStamp = 0;
		}

		public MyObjectBuilder_ConstructionStockpile GetObjectBuilder()
		{
			MyObjectBuilder_ConstructionStockpile myObjectBuilder_ConstructionStockpile = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ConstructionStockpile>();
			myObjectBuilder_ConstructionStockpile.Items = new MyObjectBuilder_StockpileItem[m_items.Count];
			for (int i = 0; i < m_items.Count; i++)
			{
				MyStockpileItem myStockpileItem = m_items[i];
				MyObjectBuilder_StockpileItem myObjectBuilder_StockpileItem = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_StockpileItem>();
				myObjectBuilder_StockpileItem.Amount = myStockpileItem.Amount;
				myObjectBuilder_StockpileItem.PhysicalContent = myStockpileItem.Content;
				myObjectBuilder_ConstructionStockpile.Items[i] = myObjectBuilder_StockpileItem;
			}
			return myObjectBuilder_ConstructionStockpile;
		}

		public void Init(MyObjectBuilder_ConstructionStockpile objectBuilder)
		{
			m_items.Clear();
			if (objectBuilder == null)
			{
				return;
			}
			MyObjectBuilder_StockpileItem[] items = objectBuilder.Items;
			foreach (MyObjectBuilder_StockpileItem myObjectBuilder_StockpileItem in items)
			{
				if (myObjectBuilder_StockpileItem.Amount > 0)
				{
					MyStockpileItem item = default(MyStockpileItem);
					item.Amount = myObjectBuilder_StockpileItem.Amount;
					item.Content = myObjectBuilder_StockpileItem.PhysicalContent;
					m_items.Add(item);
				}
			}
			LastChangeStamp++;
		}

		public void Init(MyObjectBuilder_Inventory objectBuilder)
		{
			m_items.Clear();
			if (objectBuilder != null)
			{
				foreach (MyObjectBuilder_InventoryItem item2 in objectBuilder.Items)
				{
					if (item2.Amount > 0)
					{
						MyStockpileItem item = default(MyStockpileItem);
						item.Amount = (int)item2.Amount;
						item.Content = item2.PhysicalContent;
						m_items.Add(item);
					}
				}
				LastChangeStamp++;
			}
		}

		public bool IsEmpty()
		{
			return m_items.Count == 0;
		}

		public void ClearSyncList()
		{
			m_syncItems.Clear();
		}

		public List<MyStockpileItem> GetSyncList()
		{
			return m_syncItems;
		}

		public bool AddItems(int count, MyDefinitionId contentId, MyItemFlags flags = MyItemFlags.None)
		{
			MyObjectBuilder_PhysicalObject myObjectBuilder_PhysicalObject = (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(contentId);
			if (myObjectBuilder_PhysicalObject == null)
			{
				return false;
			}
			myObjectBuilder_PhysicalObject.Flags = flags;
			return AddItems(count, myObjectBuilder_PhysicalObject);
		}

		public bool AddItems(int count, MyObjectBuilder_PhysicalObject physicalObject)
		{
			int num = 0;
			using (List<MyStockpileItem>.Enumerator enumerator = m_items.GetEnumerator())
			{
				while (enumerator.MoveNext() && !enumerator.Current.Content.CanStack(physicalObject))
				{
					num++;
				}
			}
			if (num == m_items.Count)
			{
				if (count >= int.MaxValue)
				{
					return false;
				}
				MyStockpileItem myStockpileItem = default(MyStockpileItem);
				myStockpileItem.Amount = count;
				myStockpileItem.Content = physicalObject;
				m_items.Add(myStockpileItem);
				AddSyncItem(myStockpileItem);
				LastChangeStamp++;
				return true;
			}
			if ((long)m_items[num].Amount + (long)count >= int.MaxValue)
			{
				return false;
			}
			MyStockpileItem value = default(MyStockpileItem);
			value.Amount = m_items[num].Amount + count;
			value.Content = m_items[num].Content;
			m_items[num] = value;
			MyStockpileItem diffItem = default(MyStockpileItem);
			diffItem.Content = m_items[num].Content;
			diffItem.Amount = count;
			AddSyncItem(diffItem);
			LastChangeStamp++;
			return true;
		}

		public bool RemoveItems(int count, MyObjectBuilder_PhysicalObject physicalObject)
		{
			return RemoveItems(count, physicalObject.GetId(), physicalObject.Flags);
		}

		public bool RemoveItems(int count, MyDefinitionId id, MyItemFlags flags = MyItemFlags.None)
		{
			int num = 0;
			using (List<MyStockpileItem>.Enumerator enumerator = m_items.GetEnumerator())
			{
				while (enumerator.MoveNext() && !enumerator.Current.Content.CanStack(id.TypeId, id.SubtypeId, flags))
				{
					num++;
				}
			}
			return RemoveItemsInternal(num, count);
		}

		private bool RemoveItemsInternal(int index, int count)
		{
			if (index >= m_items.Count)
			{
				return false;
			}
			if (m_items[index].Amount == count)
			{
				MyStockpileItem diffItem = m_items[index];
				diffItem.Amount = -diffItem.Amount;
				AddSyncItem(diffItem);
				m_items.RemoveAt(index);
				LastChangeStamp++;
				return true;
			}
			if (count < m_items[index].Amount)
			{
				MyStockpileItem value = default(MyStockpileItem);
				value.Amount = m_items[index].Amount - count;
				value.Content = m_items[index].Content;
				m_items[index] = value;
				MyStockpileItem diffItem2 = default(MyStockpileItem);
				diffItem2.Content = value.Content;
				diffItem2.Amount = -count;
				AddSyncItem(diffItem2);
				LastChangeStamp++;
				return true;
			}
			return false;
		}

		private void AddSyncItem(MyStockpileItem diffItem)
		{
			int num = 0;
			foreach (MyStockpileItem syncItem in m_syncItems)
			{
				if (syncItem.Content.CanStack(diffItem.Content))
				{
					MyStockpileItem value = default(MyStockpileItem);
					value.Amount = syncItem.Amount + diffItem.Amount;
					value.Content = syncItem.Content;
					m_syncItems[num] = value;
					return;
				}
				num++;
			}
			m_syncItems.Add(diffItem);
		}

		public List<MyStockpileItem> GetItems()
		{
			return m_items;
		}

		public int GetItemAmount(MyDefinitionId contentId, MyItemFlags flags = MyItemFlags.None)
		{
			foreach (MyStockpileItem item in m_items)
			{
				if (item.Content.CanStack(contentId.TypeId, contentId.SubtypeId, flags))
				{
					return item.Amount;
				}
			}
			return 0;
		}

		internal void Change(List<MyStockpileItem> items)
		{
			int count = m_items.Count;
			foreach (MyStockpileItem item2 in items)
			{
				int i;
				for (i = 0; i < count; i++)
				{
					if (m_items[i].Content.CanStack(item2.Content))
					{
						MyStockpileItem value = m_items[i];
						value.Amount += item2.Amount;
						m_items[i] = value;
						break;
					}
				}
				if (i == count)
				{
					MyStockpileItem item = default(MyStockpileItem);
					item.Amount = item2.Amount;
					item.Content = item2.Content;
					m_items.Add(item);
				}
			}
			for (int num = m_items.Count - 1; num >= 0; num--)
			{
				if (m_items[num].Amount == 0)
				{
					m_items.RemoveAtFast(num);
				}
			}
			LastChangeStamp++;
		}
	}
}
