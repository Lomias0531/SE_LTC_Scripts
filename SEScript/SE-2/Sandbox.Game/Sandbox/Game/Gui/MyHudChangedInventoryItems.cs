using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Gui
{
	public class MyHudChangedInventoryItems
	{
		public struct MyItemInfo
		{
			public MyDefinitionId DefinitionId;

			public string[] Icons;

			public MyFixedPoint ChangedAmount;

			public MyFixedPoint TotalAmount;

			public bool Added;

			public double AddTime;

			public double RemoveTime;
		}

		private const int GROUP_ITEMS_COUNT = 6;

		private const float TIME_TO_REMOVE_ITEM_SEC = 5f;

		private List<MyItemInfo> m_items = new List<MyItemInfo>();

		public ListReader<MyItemInfo> Items => new ListReader<MyItemInfo>(m_items);

		public bool Visible
		{
			get;
			private set;
		}

		public MyHudChangedInventoryItems()
		{
			Visible = false;
		}

		public void Show()
		{
			Visible = true;
		}

		public void Hide()
		{
			Visible = false;
		}

		private void AddItem(MyItemInfo item)
		{
			double num = MySession.Static.ElapsedGameTime.TotalSeconds;
			if (m_items.Count > 0)
			{
				MyItemInfo value = m_items[m_items.Count - 1];
				if (value.DefinitionId == item.DefinitionId && value.Added == item.Added)
				{
					value.ChangedAmount += item.ChangedAmount;
					value.TotalAmount = item.TotalAmount;
					if (m_items.Count <= 6)
					{
						value.RemoveTime = num + 5.0;
					}
					m_items[m_items.Count - 1] = value;
					return;
				}
				if (m_items.Count >= 6)
				{
					int index = m_items.Count - 6;
					double val = m_items[index].AddTime + 5.0;
					num = Math.Max(num, val);
				}
			}
			item.AddTime = num;
			item.RemoveTime = num + 5.0;
			m_items.Add(item);
		}

		public void AddChangedPhysicalInventoryItem(MyPhysicalInventoryItem intentoryItem, MyFixedPoint changedAmount, bool added)
		{
			MyDefinitionBase itemDefinition = intentoryItem.GetItemDefinition();
			if (itemDefinition != null)
			{
				if (changedAmount < 0)
				{
					changedAmount = -changedAmount;
				}
				MyItemInfo myItemInfo = default(MyItemInfo);
				myItemInfo.DefinitionId = itemDefinition.Id;
				myItemInfo.Icons = itemDefinition.Icons;
				myItemInfo.TotalAmount = intentoryItem.Amount;
				myItemInfo.ChangedAmount = changedAmount;
				myItemInfo.Added = added;
				MyItemInfo item = myItemInfo;
				AddItem(item);
			}
		}

		public void Update()
		{
			double totalSeconds = MySession.Static.ElapsedGameTime.TotalSeconds;
			for (int num = m_items.Count - 1; num >= 0; num--)
			{
				if (totalSeconds - Items[num].RemoveTime > 0.0)
				{
					m_items.RemoveAt(num);
				}
			}
		}

		public void Clear()
		{
			m_items.Clear();
		}
	}
}
