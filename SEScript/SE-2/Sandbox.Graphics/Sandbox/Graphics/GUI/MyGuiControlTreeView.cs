using System;
using System.Text;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	internal class MyGuiControlTreeView : MyGuiControlBase, ITreeView
	{
		public bool WholeRowHighlight;

		private Vector4 m_treeBackgroundColor;

		private MyTreeView m_treeView;

		public MyGuiControlTreeView(Vector2 position, Vector2 size, Vector4 backgroundColor, bool canHandleKeyboardActiveControl)
			: base(position, size, null, null, null, isActiveControl: true, canHandleKeyboardActiveControl)
		{
			base.Visible = true;
			base.Name = "TreeView";
			m_treeBackgroundColor = backgroundColor;
			m_treeView = new MyTreeView(this, GetPositionAbsolute() - base.Size / 2f, base.Size);
		}

		public MyTreeViewItem AddItem(StringBuilder text, string icon, Vector2 iconSize, string expandIcon, string collapseIcon, Vector2 expandIconSize)
		{
			return m_treeView.AddItem(text, icon, iconSize, expandIcon, collapseIcon, expandIconSize);
		}

		public void DeleteItem(MyTreeViewItem item)
		{
			m_treeView.DeleteItem(item);
		}

		public MyTreeViewItem GetFocusedItem()
		{
			return m_treeView.FocusedItem;
		}

		public void ClearItems()
		{
			m_treeView.ClearItems();
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			if (base.Visible)
			{
				m_treeView.Layout();
				m_treeView.Draw(transitionAlpha);
			}
			else
			{
				ShowToolTip(null);
			}
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase myGuiControlBase = base.HandleInput();
			if (myGuiControlBase == null && base.Visible && m_treeView.HandleInput())
			{
				myGuiControlBase = this;
			}
			return myGuiControlBase;
		}

		public void ShowToolTip(MyToolTips tooltip)
		{
			m_showToolTip = false;
			m_toolTip = tooltip;
			ShowToolTip();
		}

		public MyTreeViewItem GetItem(int index)
		{
			return m_treeView.GetItem(index);
		}

		public MyTreeViewItem GetItem(StringBuilder name)
		{
			return m_treeView.GetItem(name);
		}

		public int GetItemCount()
		{
			return m_treeView.GetItemCount();
		}

		protected override void OnPositionChanged()
		{
			base.OnPositionChanged();
			m_treeView.SetPosition(GetPositionAbsolute() - base.Size / 2f);
		}

		public void SetSize(Vector2 size)
		{
			base.Size = size;
			m_treeView.SetPosition(GetPositionAbsolute() - base.Size / 2f);
			m_treeView.SetSize(size);
		}

		public void FilterTree(Predicate<MyTreeViewItem> itemFilter)
		{
			MyTreeView.FilterTree(this, itemFilter);
		}
	}
}
