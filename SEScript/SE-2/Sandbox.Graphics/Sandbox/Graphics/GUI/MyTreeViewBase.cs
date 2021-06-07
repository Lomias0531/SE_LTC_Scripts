using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	internal class MyTreeViewBase : ITreeView
	{
		private List<MyTreeViewItem> m_items;

		public MyTreeView TreeView;

		public MyTreeViewItem this[int i] => m_items[i];

		public MyTreeViewBase()
		{
			m_items = new List<MyTreeViewItem>();
		}

		public MyTreeViewItem AddItem(StringBuilder text, string icon, Vector2 iconSize, string expandIcon, string collapseIcon, Vector2 expandIconSize)
		{
			MyTreeViewItem myTreeViewItem = new MyTreeViewItem(text, icon, iconSize, expandIcon, collapseIcon, expandIconSize);
			myTreeViewItem.TreeView = TreeView;
			m_items.Add(myTreeViewItem);
			myTreeViewItem.Parent = this;
			return myTreeViewItem;
		}

		public void DeleteItem(MyTreeViewItem item)
		{
			if (m_items.Remove(item))
			{
				item.TreeView = null;
				item.ClearItems();
			}
		}

		public void ClearItems()
		{
			foreach (MyTreeViewItem item in m_items)
			{
				item.TreeView = null;
				item.ClearItems();
			}
			m_items.Clear();
		}

		public Vector2 LayoutItems(Vector2 origin)
		{
			float num = 0f;
			float num2 = 0f;
			foreach (MyTreeViewItem item in m_items)
			{
				Vector2 vector = item.LayoutItem(origin + new Vector2(0f, num2));
				num = Math.Max(num, vector.X);
				num2 += vector.Y;
			}
			return new Vector2(num, num2);
		}

		public void DrawItems(float transitionAlpha)
		{
			foreach (MyTreeViewItem item in m_items)
			{
				item.Draw(transitionAlpha);
				if (item.IsExpanded)
				{
					item.DrawItems(transitionAlpha);
				}
			}
		}

		public bool HandleInput(bool hasKeyboardActiveControl)
		{
			bool flag = false;
			foreach (MyTreeViewItem item in m_items)
			{
				flag = (flag || item.HandleInputEx(hasKeyboardActiveControl));
				if (item.IsExpanded)
				{
					flag = (flag || item.HandleInput(hasKeyboardActiveControl));
				}
			}
			return flag;
		}

		public int GetItemCount()
		{
			return m_items.Count;
		}

		public MyTreeViewItem GetItem(int index)
		{
			return m_items[index];
		}

		public int GetIndex(MyTreeViewItem item)
		{
			return m_items.IndexOf(item);
		}

		public MyTreeViewItem GetItem(StringBuilder name)
		{
			return m_items.Find((MyTreeViewItem a) => a.Text == name);
		}
	}
}
